using Microsoft.Extensions.Logging;
using Plugin.Sample.ImportExportPriceBook.Pipelines.Arguments;
using Sitecore.Commerce.Core;
using Sitecore.Framework.Pipelines;

namespace Plugin.Sample.ImportExportPriceBook.Pipelines
{
    public class StreamPriceBooksToArchivePipeline : CommercePipeline<ExportPriceBooksArgument, bool>, IStreamPriceBooksToArchivePipeline, IPipeline<ExportPriceBooksArgument, bool, CommercePipelineExecutionContext>, IPipelineBlock<ExportPriceBooksArgument, bool, CommercePipelineExecutionContext>, IPipelineBlock, IPipeline
    {
        public StreamPriceBooksToArchivePipeline(
          IPipelineConfiguration<IStreamPriceBooksToArchivePipeline> configuration,
          ILoggerFactory loggerFactory)
          : base(configuration, loggerFactory)
        {
        }
    }
}
