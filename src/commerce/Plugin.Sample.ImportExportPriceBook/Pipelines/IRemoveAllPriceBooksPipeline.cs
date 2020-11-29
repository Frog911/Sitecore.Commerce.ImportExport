using Plugin.Sample.ImportExportPriceBook.Pipelines.Arguments;
using Sitecore.Commerce.Core;
using Sitecore.Framework.Pipelines;

namespace Plugin.Sample.ImportExportPriceBook.Pipelines
{
    [PipelineDisplayName("Plugin.Sample.ImportExportPriceBook.Pipelines.IRemoveAllPriceBooksPipeline")]
    public interface IRemoveAllPriceBooksPipeline : IPipeline<RemoveAllPriceBooksArgument, RemoveAllPriceBooksArgument, CommercePipelineExecutionContext>, IPipelineBlock<RemoveAllPriceBooksArgument, RemoveAllPriceBooksArgument, CommercePipelineExecutionContext>, IPipelineBlock, IPipeline
    {
    }
}
