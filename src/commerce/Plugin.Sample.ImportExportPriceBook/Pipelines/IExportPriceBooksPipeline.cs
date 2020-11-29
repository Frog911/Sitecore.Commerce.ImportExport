using Plugin.Sample.ImportExportPriceBook.Pipelines.Arguments;
using Sitecore.Commerce.Core;
using Sitecore.Commerce.Plugin.Catalog;
using Sitecore.Framework.Pipelines;

namespace Plugin.Sample.ImportExportPriceBook.Pipelines
{
    [PipelineDisplayName("Plugin.Sample.ImportExportPriceBook.Pipelines.IExportPriceBooksPipeline")]
    public interface IExportPriceBooksPipeline : IPipeline<ExportPriceBooksArgument, FileCallbackResult, CommercePipelineExecutionContext>, IPipelineBlock<ExportPriceBooksArgument, FileCallbackResult, CommercePipelineExecutionContext>, IPipelineBlock, IPipeline
    {
    }
}
