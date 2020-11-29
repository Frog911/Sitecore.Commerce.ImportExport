using Sitecore.Commerce.Core;
using Sitecore.Commerce.Plugin.Catalog;
using Sitecore.Commerce.Plugin.Pricing;
using Sitecore.Framework.Conditions;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Plugin.Sample.ImportExportPriceBook.Pipelines.Blocks
{
    public class ImportIPriceRelationshipsBlock : ImportBulkRelationshipsBlock
    {
        public ImportIPriceRelationshipsBlock(
          CommerceCommander commerceCommander,
          IAssociateCatalogToBookPipeline associateCatalogToBookPipeline)
          : base(commerceCommander)
        {
            Condition.Requires(associateCatalogToBookPipeline, nameof(associateCatalogToBookPipeline)).IsNotNull();
            this.AssociateCatalogToBookPipeline = associateCatalogToBookPipeline;
        }

        protected IAssociateCatalogToBookPipeline AssociateCatalogToBookPipeline { get; }

        protected override async Task BulkImportRelationships(IImportArgument arg, CommercePipelineExecutionContext context,
            ListsEntitiesArgument listsEntitiesArgument)
        {
            Condition.Requires(arg, nameof(arg)).IsNotNull();
            Condition.Requires(context, nameof(context)).IsNotNull();
            Condition.Requires(listsEntitiesArgument, nameof(listsEntitiesArgument)).IsNotNull();
            foreach (var listNamesAndEntityId in listsEntitiesArgument.ListNamesAndEntityIds)
            {
                var listName = listNamesAndEntityId.Key;
                var entitiesId = listNamesAndEntityId.Value;
                if (listName.StartsWith("PriceBookToCatalog", StringComparison.OrdinalIgnoreCase))
                {
                    using (IEnumerator<string> enumerator = entitiesId.GetEnumerator())
                    {
                        if (enumerator.MoveNext())
                        {
                            string current = enumerator.Current;
                            await this.AssociateCatalogToBookPipeline.Run(new CatalogAndBookArgument(listName.Substring(listName.IndexOf('-') + 1), current.SimplifyEntityName()), context);
                        }
                    }
                }   
            }
            await base.BulkImportRelationships(arg, context, listsEntitiesArgument);
        }
    }
}
