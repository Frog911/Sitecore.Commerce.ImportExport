using Plugin.Sample.ImportExportPriceBook.Pipelines.Arguments;
using Sitecore.Commerce.Core;
using Sitecore.Commerce.Plugin.Pricing;
using Sitecore.Framework.Conditions;
using Sitecore.Framework.Pipelines;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Plugin.Sample.ImportExportPriceBook.Pipelines.Blocks
{

    [PipelineDisplayName("Plugin.Sample.ImportExportPriceBook.Pipelines.Blocks.StreamPriceCardsToArchiveBlock")]
    public class StreamPriceCardsToArchiveBlock : StreamPriceItemsToArchiveBaseBlock<PriceCard, ExportPriceBooksArgument, CommercePipelineExecutionContext>
    {
        public StreamPriceCardsToArchiveBlock(CommerceCommander commerceCommander)
          : base(commerceCommander, true)
        {
        }

        public override async Task<ExportPriceBooksArgument> Run(
          ExportPriceBooksArgument arg,
          CommercePipelineExecutionContext context)
        {
            await StreamCatalogItems(arg, context);
            return arg;
        }

        protected override IEnumerable<PriceCard> GetEntitiesToExport(
          ExportPriceBooksArgument arg,
          CommercePipelineExecutionContext context,
          FindEntitiesInListArgument searchResults)
        {
            Condition.Requires(searchResults, nameof(searchResults)).IsNotNull();
            Condition.Requires(searchResults.List, "searchResults.List").IsNotNull();
            return searchResults.List.Items.OfType<PriceCard>().ToList();
        }
    }
}
