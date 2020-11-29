using Plugin.Sample.ImportExportPriceBook.Pipelines.Arguments;
using Sitecore.Commerce.Core;
using Sitecore.Commerce.Plugin.Pricing;
using Sitecore.Framework.Pipelines;

namespace Plugin.Sample.ImportExportPriceBook.Pipelines.Blocks
{
    [PipelineDisplayName("Plugin.Sample.ImportExportPriceBook.Pipelines.Blocks.RemoveAllPriceCardsBlock")]
    public class RemoveAllPriceCardsBlock : RemoveAllPriceItemsBaseBlock<PriceCard, RemoveAllPriceBooksArgument, CommercePipelineExecutionContext>
    {
        public RemoveAllPriceCardsBlock(CommerceCommander commander)
          : base(commander)
        {
        }
    }
}
