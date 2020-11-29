using Plugin.Sample.ImportExportPriceBook.Pipelines.Arguments;
using Sitecore.Commerce.Core;
using Sitecore.Commerce.Plugin.Catalog;
using Sitecore.Framework.Pipelines;

namespace Plugin.Sample.ImportExportPriceBook.Pipelines
{
    [PipelineDisplayName("Plugin.Sample.ImportExportPriceBook.Pipelines.IImportPriceBooksPipeline")]
    public interface IImportPriceBooksPipeline : IPipeline<ImportPriceBooksArgument, ImportResult, CommercePipelineExecutionContext>, IPipelineBlock<ImportPriceBooksArgument, ImportResult, CommercePipelineExecutionContext>, IPipelineBlock, IPipeline
    {
    }
}
