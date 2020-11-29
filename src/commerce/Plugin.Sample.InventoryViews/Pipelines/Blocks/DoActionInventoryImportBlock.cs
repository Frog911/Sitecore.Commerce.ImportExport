using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http.Internal;
using Newtonsoft.Json;
using Plugin.Sample.CatalogViews.Models;
using Plugin.Sample.InventoryViews.Policies;
using Sitecore.Commerce.Core;
using Sitecore.Commerce.Core.Commands;
using Sitecore.Commerce.EntityViews;
using Sitecore.Commerce.Plugin.Inventory;
using Sitecore.Framework.Pipelines;

namespace Plugin.Sample.InventoryViews.Pipelines.Blocks
{
    public class DoActionInventoryImportBlock : PipelineBlock<EntityView, EntityView, CommercePipelineExecutionContext>
    {
        private readonly CommerceCommander _commerceCommander;

        public DoActionInventoryImportBlock(CommerceCommander commerceCommander)
        {
            _commerceCommander = commerceCommander;
        }

        public override Task<EntityView> Run(EntityView arg, CommercePipelineExecutionContext context)
        {
            if (string.IsNullOrEmpty(arg?.Name) ||
                !arg.Name.Equals(context.GetPolicy<KnownInventoryViewsPolicy>().InventorySets,
                    StringComparison.OrdinalIgnoreCase) || !string.IsNullOrEmpty(arg.Action) &&
                !arg.Action.Equals(context.GetPolicy<KnownImportInventoryActionsPolicy>().InventoryImport,
                    StringComparison.OrdinalIgnoreCase))
                return Task.FromResult(arg);
            if (!arg.Properties.Any(p => p.Name.Equals(nameof(InputModel.FileContent))))
            {
                return Task.FromResult(arg);
            }

            var fileContent = arg.Properties.First(p => p.Name.Equals(nameof(InputModel.FileContent))).Value;
            var command = ExecuteLongRunningCommand(context.CommerceContext, async () =>
            {
                var importInventorySetsCommand = _commerceCommander.Command<ImportInventorySetsCommand>();
                var baseStream = new MemoryStream(Convert.FromBase64String(fileContent));
                var fileToImport = new FormFile(baseStream, 0, baseStream.Length, "file.zip", "file.zip");
                using (CommandActivity.Start(context.CommerceContext, importInventorySetsCommand))
                {
                    await importInventorySetsCommand.Process(context.CommerceContext, fileToImport, "replace", 900, 10);
                }

                return importInventorySetsCommand;
            });
            // here should be import process
            arg.Properties.Add(new ViewProperty()
            {
                Name = "LongRunningCommand",
                DisplayName = "Inventories import status",
                RawValue = string.Empty,
                Value = JsonConvert.SerializeObject(new
                {
                    LongRunningTaskId = command.TaskId,
                    UpdatePeriod = TimeSpan.FromSeconds(3).TotalMilliseconds,
                    RunningMessage = "Inventories import is in progress",
                    SuccessMessage = "Inventories import is finished successfully",
                    ErrorMessage = "Inventories import is failed. See logs for more details."
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