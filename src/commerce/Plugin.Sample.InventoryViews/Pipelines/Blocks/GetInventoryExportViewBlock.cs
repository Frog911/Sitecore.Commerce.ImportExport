using System;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Plugin.Sample.InventoryViews.Policies;
using Sitecore.Commerce.Core;
using Sitecore.Commerce.EntityViews;
using Sitecore.Commerce.Plugin.Inventory;
using Sitecore.Framework.Pipelines;

namespace Plugin.Sample.InventoryViews.Pipelines.Blocks
{
    public class GetInventoryExportViewBlock: PipelineBlock<EntityView, EntityView, CommercePipelineExecutionContext>
    {
        public override Task<EntityView> Run(EntityView arg, CommercePipelineExecutionContext context)
        {
            if (string.IsNullOrEmpty(arg?.Name) ||
                !arg.Name.Equals(context.GetPolicy<KnownInventoryViewsPolicy>().InventorySets,
                    StringComparison.OrdinalIgnoreCase) || !string.IsNullOrEmpty(arg.Action) &&
                !arg.Action.Equals(context.GetPolicy<KnownImportInventoryActionsPolicy>().InventoryExport,
                    StringComparison.OrdinalIgnoreCase))
                return Task.FromResult(arg);
            var value = new
            {
                Method = "POST",
                Action = "/api/ExportInventorySets()",
                FileName = "Inventories.zip",
                Body = new
                {
                    fileName = "Inventories.zip",
                    mode = "full",
                    maximumItemsPerFile = "200"
                }
            };
            var valueStr = JsonConvert.SerializeObject(value);
            arg.Properties.Add(new ViewProperty()
                {
                    Name = "FileDownload",
                    DisplayName = "File Download",
                    RawValue = valueStr,
                    Value = valueStr,
                    UiType = "DownloadFile",
                    IsReadOnly = false,
                    IsHidden = false
                }
            );
            return Task.FromResult(arg);
        }
    }
}