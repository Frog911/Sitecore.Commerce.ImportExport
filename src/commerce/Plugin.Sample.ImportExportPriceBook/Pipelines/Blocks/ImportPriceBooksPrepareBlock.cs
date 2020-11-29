using System;
using Sitecore.Commerce.Core;
using Sitecore.Commerce.Plugin.Catalog;
using Sitecore.Framework.Pipelines;
using System.Threading.Tasks;
using Plugin.Sample.ImportExportPriceBook.Pipelines.Arguments;

namespace Plugin.Sample.ImportExportPriceBook.Pipelines.Blocks
{
    [PipelineDisplayName("Plugin.Sample.ImportExportPriceBook.Pipelines.Blocks.ImportPriceBooksPrepareBlock")]
    public class ImportPriceBooksPrepareBlock : ImportPrepareBaseBlock<ImportPriceBooksArgument, CommercePipelineExecutionContext>
    {
        public ImportPriceBooksPrepareBlock(
            CommerceCommander commander) : base(commander)
        {
            this.RemoveAllInventorySetsPipeline = commander.Pipeline<IRemoveAllPriceBooksPipeline>();
        }

        public IRemoveAllPriceBooksPipeline RemoveAllInventorySetsPipeline { get; }

        protected override async Task ImportReplace(
          ImportPriceBooksArgument arg,
          CommercePipelineExecutionContext context)
        {
            RemoveAllPriceBooksArgument inventorySetsArgument = await this.RemoveAllInventorySetsPipeline.Run(new RemoveAllPriceBooksArgument(), context);
        }

        protected override Task ImportAdd(ImportPriceBooksArgument arg, CommercePipelineExecutionContext context)
        {
            throw new NotImplementedException();
        }
    }
}
