using System;
using System.Threading.Tasks;
using Plugin.Sample.CatalogViews.Models;
using Plugin.Sample.CatalogViews.Policies;
using Sitecore.Commerce.Core;
using Sitecore.Commerce.EntityViews;
using Sitecore.Commerce.Plugin.Catalog;
using Sitecore.Framework.Pipelines;

namespace Plugin.Sample.CatalogViews.Pipelines.Blocks
{
    public class GetCatalogImportViewBlock : PipelineBlock<EntityView, EntityView, CommercePipelineExecutionContext>
    {
        public override Task<EntityView> Run(EntityView arg, CommercePipelineExecutionContext context)
        {
            if (string.IsNullOrEmpty(arg?.Name) ||
                !arg.Name.Equals(context.GetPolicy<KnownCatalogViewsPolicy>().Catalogs,
                    StringComparison.OrdinalIgnoreCase) || !string.IsNullOrEmpty(arg.Action) &&
                !arg.Action.Equals(context.GetPolicy<KnownImportCatalogActionsPolicy>().CatalogImport,
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