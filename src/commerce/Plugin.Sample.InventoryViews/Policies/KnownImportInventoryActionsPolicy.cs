using Sitecore.Commerce.Core;

namespace Plugin.Sample.InventoryViews.Policies
{
    public class KnownImportInventoryActionsPolicy: Policy
    {
        public string InventoryImport { get; set; } = nameof(InventoryImport);
        public string InventoryExport { get; set; } = nameof(InventoryExport);
    }
}