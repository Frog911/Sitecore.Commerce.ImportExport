using System;
using System.Threading.Tasks;
using Plugin.Sample.CatalogViews.Policies;
using Sitecore.Commerce.Core;
using Sitecore.Commerce.EntityViews;
using Sitecore.Commerce.Plugin.Catalog;
using Sitecore.Framework.Pipelines;

namespace Plugin.Sample.CatalogViews.Pipelines.Blocks
{
    public class PopulateCatalogImportActionsBlock: PipelineBlock<EntityView, EntityView, CommercePipelineExecutionContext>
    {
        public override Task<EntityView> Run(EntityView arg, CommercePipelineExecutionContext context)
        {
            if (string.IsNullOrEmpty(arg?.Name) ||
                !arg.Name.Equals(context.GetPolicy<KnownCatalogViewsPolicy>().Catalogs,
                    StringComparison.OrdinalIgnoreCase) || !string.IsNullOrEmpty(arg.Action))
                return Task.FromResult(arg);
            var actions = arg.GetPolicy<ActionsPolicy>().Actions;
            {
                actions.Add(new EntityActionView()
                {
                    Name = context.GetPolicy<KnownImportCatalogActionsPolicy>().CatalogImport,
                    DisplayName = "Import catalogs",
                    Description = "",
                    EntityView = arg.Name,
                    IsEnabled = true,
                    Icon = "upload"
                });
                actions.Add(new EntityActionView()
                {
                    Name = context.GetPolicy<KnownImportCatalogActionsPolicy>().CatalogExport,
                    DisplayName = "Export catalogs",
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