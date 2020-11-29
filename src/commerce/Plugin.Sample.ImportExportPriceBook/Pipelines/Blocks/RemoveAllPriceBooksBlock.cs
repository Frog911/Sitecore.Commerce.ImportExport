using System;
using System.Threading.Tasks;
using Plugin.Sample.ImportExportPriceBook.Pipelines.Arguments;
using Sitecore.Commerce.Core;
using Sitecore.Commerce.Plugin.Catalog;
using Sitecore.Commerce.Plugin.Pricing;
using Sitecore.Framework.Conditions;
using Sitecore.Framework.Pipelines;

namespace Plugin.Sample.ImportExportPriceBook.Pipelines.Blocks
{
    [PipelineDisplayName("Plugin.Sample.ImportExportPriceBook.Pipelines.Blocks.RemoveAllPriceBooksBlock")]
    public class RemoveAllPriceBooksBlock : RemoveAllPriceItemsBaseBlock<PriceBook, RemoveAllPriceBooksArgument,
        CommercePipelineExecutionContext>
    {
        public RemoveAllPriceBooksBlock(CommerceCommander commander)
            : base(commander)
        {
        }
        protected override async Task RemoveAdditionalDependencies(
            RemoveAllPriceBooksArgument arg,
            CommercePipelineExecutionContext context,
            PriceBook entity)
        {
            Condition.Requires(arg, nameof(arg)).IsNotNull();
            Condition.Requires(context, nameof(context)).IsNotNull();
            Condition.Requires(entity, nameof(entity)).IsNotNull();
            var relationshipDef = new RelationshipDefinition(Array.Empty<Component>()) {Name = "PriceBookToCatalog"};
            var entitiesInListArgument = new FindEntitiesInListArgument(typeof(Catalog),
                CatalogExportHelper.GetRelationshipListName(entity, relationshipDef), 0, int.MaxValue)
            {
                LoadEntities = false,
                LoadTotalItemCount = false
            };
            foreach (var entityReference in (await Commander.Pipeline<IFindEntitiesInListPipeline>()
                .Run(entitiesInListArgument, context).ConfigureAwait(false)).EntityReferences)
            {
                var inventorySetArgument =
                    new CatalogAndBookArgument(entity.FriendlyId, entityReference.EntityId.SimplifyEntityName());
                await Commander.Pipeline<IDisassociateCatalogFromBookPipeline>()
                    .Run(inventorySetArgument, context).ConfigureAwait(false);
            }
        }
    }
}