using Microsoft.Extensions.Logging;
using Plugin.Sample.ImportExportPriceBook.Pipelines.Arguments;
using Sitecore.Commerce.Core;
using Sitecore.Commerce.Plugin.Catalog;
using Sitecore.Framework.Pipelines;

namespace Plugin.Sample.ImportExportPriceBook.Pipelines
{
    public class ExportPriceBooksPipeline : CommercePipeline<ExportPriceBooksArgument, FileCallbackResult>, IExportPriceBooksPipeline, IPipeline<ExportPriceBooksArgument, FileCallbackResult, CommercePipelineExecutionContext>, IPipelineBlock<ExportPriceBooksArgument, FileCallbackResult, CommercePipelineExecutionContext>, IPipelineBlock, IPipeline
    {
        public ExportPriceBooksPipeline(
          IPipelineConfiguration<IExportPriceBooksPipeline> configuration,
          ILoggerFactory loggerFactory)
          : base(configuration, loggerFactory)
        {
        }
    }
}
