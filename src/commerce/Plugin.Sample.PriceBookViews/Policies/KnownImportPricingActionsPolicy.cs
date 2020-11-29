using Sitecore.Commerce.Core;

namespace Plugin.Sample.PriceBookViews.Policies
{
    public class KnownImportPricingActionsPolicy: Policy
    {
        public string PricebookImport { get; set; } = nameof(PricebookImport);
        public string PricebookExport { get; set; } = nameof(PricebookExport);
    }
}