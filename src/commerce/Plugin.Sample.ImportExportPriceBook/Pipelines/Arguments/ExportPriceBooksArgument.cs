using Sitecore.Commerce.Plugin.Catalog;

namespace Plugin.Sample.ImportExportPriceBook.Pipelines.Arguments
{
    public class ExportPriceBooksArgument : ExportArgumentBase
    {
        public ExportPriceBooksArgument(string fileName, string mode) : base(fileName, mode)
        {
        }
    }
}
