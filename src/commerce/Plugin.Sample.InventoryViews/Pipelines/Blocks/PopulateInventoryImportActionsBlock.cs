using System;
using System.Threading.Tasks;
using Plugin.Sample.InventoryViews.Policies;
using Sitecore.Commerce.Core;
using Sitecore.Commerce.EntityViews;
using Sitecore.Commerce.Plugin.Inventory;
using Sitecore.Framework.Pipelines;

namespace Plugin.Sample.InventoryViews.Pipelines.Blocks
{
    public class PopulateInventoryImportActionsBlock: PipelineBlock<EntityView, EntityView, CommercePipelineExecutionContext>
    {
        public override Task<EntityView> Run(EntityView arg, CommercePipelineExecutionContext context)
        {
            if (string.IsNullOrEmpty(arg?.Name) ||
                !arg.Name.Equals(context.GetPolicy<KnownInventoryViewsPolicy>().InventorySets,
                    StringComparison.OrdinalIgnoreCase) || !string.IsNullOrEmpty(arg.Action))
                return Task.FromResult(arg);
            var actions = arg.GetPolicy<ActionsPolicy>().Actions;
            {
                actions.Add(new EntityActionView()
                {
                    Name = context.GetPolicy<KnownImportInventoryActionsPolicy>().InventoryImport,
                    DisplayName = "Import inventories",
                    Description = "",
                    EntityView = arg.Name,
                    IsEnabled = true,
                    Icon = "upload"
                });
                actions.Add(new EntityActionView()
                {
                    Name = context.GetPolicy<KnownImportInventoryActionsPolicy>().InventoryExport,
                    DisplayName = "Export inventories",
                    Description = "",
                    EntityView = arg.Name,
                    IsEnabled = true,
                    Icon = "download"
                });
            }
            return Task.FromResult(arg);
        }
    }
}