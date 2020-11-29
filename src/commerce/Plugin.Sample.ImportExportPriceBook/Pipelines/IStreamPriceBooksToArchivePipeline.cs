using Plugin.Sample.ImportExportPriceBook.Pipelines.Arguments;
using Sitecore.Commerce.Core;
using Sitecore.Framework.Pipelines;

namespace Plugin.Sample.ImportExportPriceBook.Pipelines
{
    [PipelineDisplayName("Plugin.Sample.ImportExportPriceBook.Pipelines.IStreamPriceBooksToArchivePipeline")]
    public interface IStreamPriceBooksToArchivePipeline : IPipeline<ExportPriceBooksArgument, bool, CommercePipelineExecutionContext>, IPipelineBlock<ExportPriceBooksArgument, bool, CommercePipelineExecutionContext>, IPipelineBlock, IPipeline
    {
    }
}
