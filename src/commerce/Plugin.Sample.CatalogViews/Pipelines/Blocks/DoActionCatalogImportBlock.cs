using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http.Internal;
using Newtonsoft.Json;
using Plugin.Sample.CatalogViews.Models;
using Plugin.Sample.CatalogViews.Policies;
using Sitecore.Commerce.Core;
using Sitecore.Commerce.Core.Commands;
using Sitecore.Commerce.EntityViews;
using Sitecore.Commerce.Plugin.Catalog;
using Sitecore.Framework.Pipelines;

namespace Plugin.Sample.CatalogViews.Pipelines.Blocks
{
    public class DoActionCatalogImportBlock: PipelineBlock<EntityView, EntityView, CommercePipelineExecutionContext>
    {
        private readonly CommerceCommander _commerceCommander;

        public DoActionCatalogImportBlock(CommerceCommander commerceCommander)
        {
            _commerceCommander = commerceCommander;
        }

        public override Task<EntityView> Run(EntityView arg, CommercePipelineExecutionContext context)
        {
            if (string.IsNullOrEmpty(arg?.Name) ||
                !arg.Name.Equals(context.GetPolicy<KnownCatalogViewsPolicy>().Catalogs,
                    StringComparison.OrdinalIgnoreCase) || !string.IsNullOrEmpty(arg.Action) &&
                !arg.Action.Equals(context.GetPolicy<KnownImportCatalogActionsPolicy>().CatalogImport,
                    StringComparison.OrdinalIgnoreCase))
                return Task.FromResult(arg);
            if (!arg.Properties.Any(p => p.Name.Equals(nameof(InputModel.FileContent))))
            {
                return Task.FromResult(arg);
            }
            var fileContent = arg.Properties.First(p => p.Name.Equals(nameof(InputModel.FileContent))).Value;
            var command = ExecuteLongRunningCommand(context.CommerceContext, async () =>
            {
                var importCatalogsCommand = _commerceCommander.Command<ImportCatalogsCommand>();
                var baseStream = new MemoryStream(Convert.FromBase64String(fileContent));
                var fileToImport = new FormFile(baseStream, 0, baseStream.Length, "file.zip", "file.zip");
                using (CommandActivity.Start(context.CommerceContext, importCatalogsCommand))
                {
                    await importCatalogsCommand.Process(context.CommerceContext, fileToImport, "replace", 900, 10);
                }
                return importCatalogsCommand;
            });
            // here should be import process
            arg.Properties.Add(new ViewProperty()
            {
                Name = "LongRunningCommand",
                DisplayName = "Catalogs import status",
                RawValue = string.Empty,
                Value = JsonConvert.SerializeObject(new
                {
                    LongRunningTaskId = command.TaskId,
                    UpdatePeriod = TimeSpan.FromSeconds(3).TotalMilliseconds,
                    RunningMessage = "Catalogs import is in progress",
                    SuccessMessage = "Catalogs import is finished successfully",
                    ErrorMessage = "Catalogs import is failed. See logs for more details."
                }),
                IsReadOnly = false,
                IsHidden = false,
                UiType = "LongRunningCommand"
            });
            context.CommerceContext.AddModel(arg);
            arg.Properties.Remove(arg.Properties.FirstOrDefault(x => x.Name.Equals(nameof(InputModel.FileContent))));
            return Task.FromResult(arg);
        }
        
        protected virtual CommerceCommand ExecuteLongRunningCommand(CommerceContext context,
            Func<Task<CommerceCommand>> action)
        {
            var task = action();
            context.GlobalEnvironment.AddLongRunningCommand(task, context);
            return task.ToCommerceCommand();
        }
    }
}