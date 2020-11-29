using Plugin.Sample.ImportExportPriceBook.Helpers;
using Sitecore.Commerce.Core;
using Sitecore.Commerce.Plugin.Catalog;
using System;

namespace Plugin.Sample.ImportExportPriceBook.Pipelines.Blocks
{
    public abstract class StreamPriceItemsToArchiveBaseBlock<TEntity, TInput, TContext> : StreamBulkCatalogItemsToArchiveBaseBlock<TEntity, TInput, TContext>
    where TEntity : CommerceEntity
    where TInput : IExportArgument
    where TContext : CommercePipelineExecutionContext
    {
        public StreamPriceItemsToArchiveBaseBlock(
          CommerceCommander commander,
          bool loadEntities = false)
          : base(commander, loadEntities)
        {
        }

        protected override Type GetTargetEntityType(RelationshipDefinition relationshipDef)
        {
            Type targetEntityType = PriceBookExportHelper.TryGetTargetEntityType(relationshipDef);
            return (object)targetEntityType != null ? targetEntityType : base.GetTargetEntityType(relationshipDef);
        }
    }
}
