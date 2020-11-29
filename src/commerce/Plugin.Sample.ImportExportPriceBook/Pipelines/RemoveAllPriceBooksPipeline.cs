using Microsoft.Extensions.Logging;
using Plugin.Sample.ImportExportPriceBook.Pipelines.Arguments;
using Sitecore.Commerce.Core;
using Sitecore.Framework.Pipelines;

namespace Plugin.Sample.ImportExportPriceBook.Pipelines
{

    public class RemoveAllPriceBooksPipeline : CommercePipeline<RemoveAllPriceBooksArgument, RemoveAllPriceBooksArgument>, IRemoveAllPriceBooksPipeline, IPipeline<RemoveAllPriceBooksArgument, RemoveAllPriceBooksArgument, CommercePipelineExecutionContext>, IPipelineBlock<RemoveAllPriceBooksArgument, RemoveAllPriceBooksArgument, CommercePipelineExecutionContext>, IPipelineBlock, IPipeline
    {
        public RemoveAllPriceBooksPipeline(
          IPipelineConfiguration<IRemoveAllPriceBooksPipeline> configuration,
          ILoggerFactory loggerFactory)
          : base(configuration, loggerFactory)
        {
        }
    }
}
