using Plugin.Sample.ImportExportPriceBook.Pipelines.Arguments;
using Sitecore.Commerce.Core;
using Sitecore.Commerce.Plugin.Catalog;
using Sitecore.Commerce.Plugin.Pricing;
using Sitecore.Framework.Pipelines;
using System.Collections.Generic;

namespace Plugin.Sample.ImportExportPriceBook.Pipelines.Blocks
{
    [PipelineDisplayName("Plugin.Sample.ImportExportPriceBook.Pipelines.Blocks.ImportPriceBooksBlock")]
    public class ImportPriceBooksBlock : ImportBulkCatalogItemsBaseBlock<PriceBook, ImportPriceBooksArgument,
        CommercePipelineExecutionContext>
    {
        public ImportPriceBooksBlock(CommerceCommander commander) : base(commander)
        {
            //Condition.Requires(persistEntityPipeline, nameof(persistEntityPipeline)).IsNotNull();
            //this.PersistEntityPipeline = persistEntityPipeline;
        }

        //protected IPersistEntityPipeline PersistEntityPipeline { get; }

        //protected override async Task ImportSingleEntity(
        //  ImportPriceBooksArgument arg,
        //  CommercePipelineExecutionContext context,
        //  PriceBook entity)
        //{
        //    Condition.Requires(arg, nameof(arg)).IsNotNull();
        //    Condition.Requires(context, nameof(context)).IsNotNull();
        //    Condition.Requires(entity, nameof(entity)).IsNotNull();
        //    var persistEntityArgument = await this.PersistEntityPipeline.Run(new PersistEntityArgument(entity), context);
        //}

        protected override IEnumerable<string> GetListMemberships(
            CommercePipelineExecutionContext context)
        {
            return new[]
            {
                CommerceEntity.ListName<PriceBook>()
            };
        }
    }
}