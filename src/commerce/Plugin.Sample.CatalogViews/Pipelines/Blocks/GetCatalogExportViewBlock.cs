using System;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Plugin.Sample.CatalogViews.Policies;
using Sitecore.Commerce.Core;
using Sitecore.Commerce.EntityViews;
using Sitecore.Commerce.Plugin.Catalog;
using Sitecore.Framework.Pipelines;

namespace Plugin.Sample.CatalogViews.Pipelines.Blocks
{
    public class GetCatalogExportViewBlock: PipelineBlock<EntityView, EntityView, CommercePipelineExecutionContext>
    {
        public override Task<EntityView> Run(EntityView arg, CommercePipelineExecutionContext context)
        {
            if (string.IsNullOrEmpty(arg?.Name) ||
                !arg.Name.Equals(context.GetPolicy<KnownCatalogViewsPolicy>().Catalogs,
                    StringComparison.OrdinalIgnoreCase) || !string.IsNullOrEmpty(arg.Action) &&
                !arg.Action.Equals(context.GetPolicy<KnownImportCatalogActionsPolicy>().CatalogExport,
                    StringComparison.OrdinalIgnoreCase))
                return Task.FromResult(arg);
            var value = new
            {
                Method = "POST",
                Action = "/api/ExportCatalogs()",
                FileName = "Catalogs.zip",
                Body = new
                {
                    fileName = "PriceBooks.zip",
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