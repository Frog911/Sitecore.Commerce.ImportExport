using System;
using System.Threading.Tasks;
using Plugin.Sample.PriceBookViews.Policies;
using Sitecore.Commerce.Core;
using Sitecore.Commerce.EntityViews;
using Sitecore.Commerce.Plugin.Pricing;
using Sitecore.Framework.Pipelines;

namespace Plugin.Sample.PriceBookViews.Pipelines.Blocks
{
    public class
        PopulateImportPricingDashboardActionsBlock : PipelineBlock<EntityView, EntityView, CommercePipelineExecutionContext>
    {
        public override Task<EntityView> Run(EntityView arg, CommercePipelineExecutionContext context)
        {
            if (string.IsNullOrEmpty(arg?.Name) ||
                !arg.Name.Equals(context.GetPolicy<KnownPricingViewsPolicy>().PriceBooks,
                    StringComparison.OrdinalIgnoreCase) || !string.IsNullOrEmpty(arg.Action) &&
                !arg.Action.Equals(context.GetPolicy<KnownPricingActionsPolicy>().PaginatePriceBooks,
                    StringComparison.OrdinalIgnoreCase))
                return Task.FromResult(arg);
            var actions = arg.GetPolicy<ActionsPolicy>().Actions;
            {
                actions.Add(new EntityActionView()
                {
                    Name = context.GetPolicy<KnownImportPricingActionsPolicy>().PricebookImport,
                    DisplayName = "Import price books",
                    Description = "",
                    EntityView = arg.Name,
                    IsEnabled = true,
                    Icon = "upload"
                });
                actions.Add(new EntityActionView()
                {
                    Name = context.GetPolicy<KnownImportPricingActionsPolicy>().PricebookExport,
                    DisplayName = "Export price books",
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