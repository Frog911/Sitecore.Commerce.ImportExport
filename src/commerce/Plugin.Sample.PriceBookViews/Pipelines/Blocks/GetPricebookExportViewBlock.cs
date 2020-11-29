using System;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Plugin.Sample.PriceBookViews.Policies;
using Sitecore.Commerce.Core;
using Sitecore.Commerce.EntityViews;
using Sitecore.Commerce.Plugin.Pricing;
using Sitecore.Framework.Pipelines;

namespace Plugin.Sample.PriceBookViews.Pipelines.Blocks
{
    public class GetPricebookExportViewBlock: PipelineBlock<EntityView, EntityView, CommercePipelineExecutionContext>
    {
        public override Task<EntityView> Run(EntityView arg, CommercePipelineExecutionContext context)
        {
            if (string.IsNullOrEmpty(arg?.Name) ||
                !arg.Name.Equals(context.GetPolicy<KnownPricingViewsPolicy>().PriceBooks,
                    StringComparison.OrdinalIgnoreCase) || !string.IsNullOrEmpty(arg.Action) &&
                !arg.Action.Equals(context.GetPolicy<KnownImportPricingActionsPolicy>().PricebookExport,
                    StringComparison.OrdinalIgnoreCase))
                return Task.FromResult(arg);
            var value = new
            {
                Method = "PUT",
                Action = "/api/ExportPriceBooks()",
                FileName = "PriceBooks.zip",
                Body = new
                {
                    fileName = "PriceBooks.zip",
                    mode = "full"
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