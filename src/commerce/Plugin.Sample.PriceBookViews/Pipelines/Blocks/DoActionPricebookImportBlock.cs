using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http.Internal;
using Newtonsoft.Json;
using Plugin.Sample.ImportExportPriceBook.Commands;
using Plugin.Sample.PriceBookViews.Models;
using Plugin.Sample.PriceBookViews.Policies;
using Sitecore.Commerce.Core;
using Sitecore.Commerce.Core.Commands;
using Sitecore.Commerce.EntityViews;
using Sitecore.Commerce.Plugin.Pricing;
using Sitecore.Framework.Pipelines;

namespace Plugin.Sample.PriceBookViews.Pipelines.Blocks
{
    public class DoActionPricebookImportBlock: PipelineBlock<EntityView, EntityView, CommercePipelineExecutionContext>
    {
        private readonly CommerceCommander _commerceCommander;

        public DoActionPricebookImportBlock(CommerceCommander commerceCommander)
        {
            _commerceCommander = commerceCommander;
        }

        public override Task<EntityView> Run(EntityView arg, CommercePipelineExecutionContext context)
        {
            if (string.IsNullOrEmpty(arg?.Name) ||
                !arg.Name.Equals(context.GetPolicy<KnownPricingViewsPolicy>().PriceBooks,
                    StringComparison.OrdinalIgnoreCase) || !string.IsNullOrEmpty(arg.Action) &&
                !arg.Action.Equals(context.GetPolicy<KnownImportPricingActionsPolicy>().PricebookImport,
                    StringComparison.OrdinalIgnoreCase))
                return Task.FromResult(arg);
            if (!arg.Properties.Any(p => p.Name.Equals(nameof(InputModel.FileContent))))
            {
                return Task.FromResult(arg);
            }
            var fileContent = arg.Properties.First(p => p.Name.Equals(nameof(InputModel.FileContent))).Value;
            var command = ExecuteLongRunningCommand(context.CommerceContext, async () =>
            {
                var importPriceBooksCommand = _commerceCommander.Command<ImportPriceBooksCommand>();
                var baseStream = new MemoryStream(Convert.FromBase64String(fileContent));
                var fileToImport = new FormFile(baseStream, 0, baseStream.Length, "file.zip", "file.zip");
                using (CommandActivity.Start(context.CommerceContext, importPriceBooksCommand))
                {
                    await importPriceBooksCommand.Process(context.CommerceContext, fileToImport, "replace", 100);
                }
                return importPriceBooksCommand;
            });
            // here should be import process
            arg.Properties.Add(new ViewProperty()
            {
                Name = "LongRunningCommand",
                DisplayName = "PriceBooks import status",
                RawValue = string.Empty,
                Value = JsonConvert.SerializeObject(new
                {
                    LongRunningTaskId = command.TaskId,
                    UpdatePeriod = TimeSpan.FromSeconds(3).TotalMilliseconds,
                    RunningMessage = "PriceBooks import is in progress",
                    SuccessMessage = "PriceBooks import is finished successfully",
                    ErrorMessage = "PriceBooks import is failed. See logs for more details."
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
            Task<CommerceCommand> task = action();
            context.GlobalEnvironment.AddLongRunningCommand(task, context);
            return task.ToCommerceCommand();
        }
    }
}