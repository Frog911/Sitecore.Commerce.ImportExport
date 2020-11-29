using Sitecore.Commerce.Core;
using Sitecore.Commerce.Plugin.Catalog;
using System.Collections.Generic;

namespace Plugin.Sample.ImportExportPriceBook.Pipelines.Arguments
{
    public class RemoveAllPriceBooksArgument : PipelineArgument, IRemoveAllItemsArgument, ILoadRelationshipDefinitionsArgument
    {
        internal const int DefaultPageSize = 200;

        public int PageSize { get; set; } = 200;

        public IEnumerable<RelationshipDefinition> CustomRelationshipDefinitions { get; set; }

        public IEnumerable<RelationshipDefinition> SystemRelationshipDefinitions { get; set; }
    }
}
