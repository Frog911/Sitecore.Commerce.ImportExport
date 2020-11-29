using Plugin.Sample.ImportExportPriceBook.Pipelines.Arguments;
using Sitecore.Commerce.Core;
using Sitecore.Commerce.Plugin.Catalog;
using Sitecore.Framework.Conditions;
using Sitecore.Framework.Pipelines;
using System.IO.Compression;
using System.Threading.Tasks;

namespace Plugin.Sample.ImportExportPriceBook.Pipelines.Blocks
{
    [PipelineDisplayName("Inventory.block.ExportPriceBooks")]
    public class ExportPriceBooksBlock : PipelineBlock<ExportPriceBooksArgument, FileCallbackResult, CommercePipelineExecutionContext>
    {
        public ExportPriceBooksBlock(
          IStreamPriceBooksToArchivePipeline streamPriceBooksToArchivePipeline)
          : base(null)
        {
            Condition.Requires<IStreamPriceBooksToArchivePipeline>(streamPriceBooksToArchivePipeline, nameof(streamPriceBooksToArchivePipeline)).IsNotNull();
            this.StreamPriceBooksToArchivePipeline = streamPriceBooksToArchivePipeline;
        }

        protected IStreamPriceBooksToArchivePipeline StreamPriceBooksToArchivePipeline { get; }

        public override Task<FileCallbackResult> Run(
          ExportPriceBooksArgument arg,
          CommercePipelineExecutionContext context)
        {
            Condition.Requires(arg, nameof(arg)).IsNotNull();
            Condition.Requires(arg.Mode, "arg.Mode").IsNotNull();
            FileCallbackResult result = new FileCallbackResult("application/octet-stream", async (outputStream, callbackContext) =>
            {
                using (ZipArchive zipArchive = new ZipArchive(new WriteOnlyStreamWrapperStream(outputStream), ZipArchiveMode.Create))
                {
                    arg.ExportArchive = zipArchive;
                    int num = await this.StreamPriceBooksToArchivePipeline.Run(arg, context) ? 1 : 0;
                }
            });
            result.FileDownloadName = arg.FileName;
            return Task.FromResult(result);
        }
    }
}
