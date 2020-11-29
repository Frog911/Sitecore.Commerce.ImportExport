using Plugin.Sample.ImportExportPriceBook.Pipelines.Arguments;
using Sitecore.Commerce.Core;
using Sitecore.Commerce.Plugin.Catalog;
using Sitecore.Commerce.Plugin.ManagedLists;
using Sitecore.Commerce.Plugin.Pricing;
using Sitecore.Framework.Pipelines;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Plugin.Sample.ImportExportPriceBook.Pipelines.Blocks
{
    [PipelineDisplayName("Inventory.block.ImportInventoryInformation")]
    public class ImportPriceCardsBlock : ImportBulkCatalogItemsBaseBlock<PriceCard, ImportPriceBooksArgument,
        CommercePipelineExecutionContext>
    {
        public ImportPriceCardsBlock(CommerceCommander commander) : base(commander)
        {
        }

        protected override async Task BulkImportEntities(ImportPriceBooksArgument arg,
            CommercePipelineExecutionContext context,
            List<PriceCard> preparedEntityList)
        {
            foreach (var entity in preparedEntityList)
            {
                var listName = await GetPricingListName(context, entity);
                entity.GetComponent<ListMembershipsComponent>().Memberships.Add(listName);
            }

            await base.BulkImportEntities(arg, context, preparedEntityList);
        }

        private async Task<string> GetPricingListName(CommercePipelineExecutionContext context, PriceCard entity)
        {
            var priceBookName = await Commander.Pipeline<IFindEntityPipeline>().Run(
                new FindEntityArgument(typeof(PriceBook),
                    entity.Book.EntityTarget.EnsurePrefix(CommerceEntity.IdPrefix<PriceBook>())), context);

            return string.Format(context.GetPolicy<KnownPricingListsPolicy>().PriceBookCards,
                priceBookName?.Name ?? entity.Book.EntityTarget);
        }

        protected override IEnumerable<string> GetListMemberships(
            CommercePipelineExecutionContext context)
        {
            return new string[]
            {
                CommerceEntity.ListName<PriceCard>(),
            };
        }
    }
}