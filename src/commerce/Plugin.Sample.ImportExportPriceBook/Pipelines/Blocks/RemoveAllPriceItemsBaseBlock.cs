using Plugin.Sample.ImportExportPriceBook.Helpers;
using Sitecore.Commerce.Core;
using Sitecore.Commerce.Plugin.Catalog;
using System;

namespace Plugin.Sample.ImportExportPriceBook.Pipelines.Blocks
{
    public abstract class RemoveAllPriceItemsBaseBlock<TEntity, TInput, TContext> : RemoveAllCatalogItemsBaseBlock<TEntity, TInput, TContext>
    where TEntity : CommerceEntity
    where TInput : IRemoveAllItemsArgument
    where TContext : CommercePipelineExecutionContext
    {
        public RemoveAllPriceItemsBaseBlock(CommerceCommander commander)
          : base(commander)
        {
        }

        protected override Type GetTargetEntityType(RelationshipDefinition relationshipDef)
        {
            Type targetEntityType = PriceBookExportHelper.TryGetTargetEntityType(relationshipDef);
            return (object)targetEntityType != null ? targetEntityType : base.GetTargetEntityType(relationshipDef);
        }
    }
}
