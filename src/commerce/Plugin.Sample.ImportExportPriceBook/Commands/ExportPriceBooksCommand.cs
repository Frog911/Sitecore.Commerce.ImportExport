using Plugin.Sample.ImportExportPriceBook.Pipelines;
using Plugin.Sample.ImportExportPriceBook.Pipelines.Arguments;
using Sitecore.Commerce.Core;
using Sitecore.Commerce.Core.Commands;
using Sitecore.Commerce.Plugin.Catalog;
using Sitecore.Framework.Conditions;
using System;
using System.Threading.Tasks;

namespace Plugin.Sample.ImportExportPriceBook.Commands
{
    public class ExportPriceBooksCommand : CommerceCommand
    {
        public ExportPriceBooksCommand(
          IExportPriceBooksPipeline exportPriceBooksPipeline,
          IServiceProvider serviceProvider)
          : base(serviceProvider)
        {
            Condition.Requires(exportPriceBooksPipeline, nameof(exportPriceBooksPipeline)).IsNotNull();
            this.ExportPriceBooksPipeline = exportPriceBooksPipeline;
        }

        protected IExportPriceBooksPipeline ExportPriceBooksPipeline { get; }

        public async Task<FileCallbackResult> Process(
          CommerceContext commerceContext,
          string fileName,
          string mode,
          int maximumItemsPerFile)
        {
            FileCallbackResult fileCallbackResult;
            using (CommandActivity.Start(commerceContext, this))
            {
                ExportPriceBooksArgument inventorySetsArgument = new ExportPriceBooksArgument(fileName, mode)
                {
                    MaximumItemsPerFile = maximumItemsPerFile
                };
                fileCallbackResult = await this.ExportPriceBooksPipeline.Run(inventorySetsArgument, commerceContext.PipelineContextOptions);
            }
            return fileCallbackResult;
        }
    }
}
