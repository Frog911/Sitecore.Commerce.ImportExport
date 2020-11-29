using Microsoft.Extensions.Logging;
using Plugin.Sample.ImportExportPriceBook.Pipelines.Arguments;
using Sitecore.Commerce.Core;
using Sitecore.Commerce.Plugin.Catalog;
using Sitecore.Framework.Pipelines;

namespace Plugin.Sample.ImportExportPriceBook.Pipelines
{
    public class ImportPriceBooksPipeline : CommercePipeline<ImportPriceBooksArgument, ImportResult>, IImportPriceBooksPipeline, IPipeline<ImportPriceBooksArgument, ImportResult, CommercePipelineExecutionContext>, IPipelineBlock<ImportPriceBooksArgument, ImportResult, CommercePipelineExecutionContext>, IPipelineBlock, IPipeline
    {
        public ImportPriceBooksPipeline(
          IPipelineConfiguration<IImportPriceBooksPipeline> configuration,
          ILoggerFactory loggerFactory)
          : base(configuration, loggerFactory)
        {
        }
    }
}
