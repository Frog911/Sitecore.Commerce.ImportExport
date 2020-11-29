using Sitecore.Commerce.Core;

namespace Plugin.Sample.CatalogViews.Policies
{
    public class KnownImportCatalogActionsPolicy: Policy
    {
        public string CatalogImport { get; set; } = nameof(CatalogImport);
        public string CatalogExport { get; set; } = nameof(CatalogExport);
    }
}