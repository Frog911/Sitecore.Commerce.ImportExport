using System;
using System.Threading.Tasks;
using Plugin.Sample.PriceBookViews.Models;
using Plugin.Sample.PriceBookViews.Policies;
using Sitecore.Commerce.Core;
using Sitecore.Commerce.EntityViews;
using Sitecore.Commerce.Plugin.Pricing;
using Sitecore.Framework.Pipelines;

namespace Plugin.Sample.PriceBookViews.Pipelines.Blocks
{
    public class GetPricebookImportViewBlock : PipelineBlock<EntityView, EntityView, CommercePipelineExecutionContext>
    {
        public override Task<EntityView> Run(EntityView arg, CommercePipelineExecutionContext context)
        {
            if (string.IsNullOrEmpty(arg?.Name) ||
                !arg.Name.Equals(context.GetPolicy<KnownPricingViewsPolicy>().PriceBooks,
                    StringComparison.OrdinalIgnoreCase) || !string.IsNullOrEmpty(arg.Action) &&
                !arg.Action.Equals(context.GetPolicy<KnownImportPricingActionsPolicy>().PricebookImport,
                    StringComparison.OrdinalIgnoreCase))
                return Task.FromResult(arg);
            arg.Properties.Add(new ViewProperty()
                {
                    Name = nameof(InputModel.FileContent),
                    DisplayName = "File",
                    RawValue = string.Empty,
                    Value = string.Empty,
                    IsReadOnly = false,
                    IsHidden = false,
                    UiType = "FileUpload"
                }
            );
            return Task.FromResult(arg);
        }
    }
}