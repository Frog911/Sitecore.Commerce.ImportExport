using Microsoft.AspNetCore.Http;
using Plugin.Sample.ImportExportPriceBook.Pipelines;
using Plugin.Sample.ImportExportPriceBook.Pipelines.Arguments;
using Sitecore.Commerce.Core;
using Sitecore.Commerce.Core.Commands;
using Sitecore.Commerce.Plugin.Catalog;
using Sitecore.Framework.Conditions;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Plugin.Sample.ImportExportPriceBook.Commands
{
    public class ImportPriceBooksCommand : CommerceCommand
    {
        public ImportPriceBooksCommand(
          IImportPriceBooksPipeline importPriceBooksPipeline,
          IServiceProvider serviceProvider)
          : base(serviceProvider)
        {
            Condition.Requires(importPriceBooksPipeline, nameof(importPriceBooksPipeline)).IsNotNull();
            this.ImportPriceBooksPipeline = importPriceBooksPipeline;
        }

        public IImportPriceBooksPipeline ImportPriceBooksPipeline { get; }

        public async Task<CommerceCommand> Process(
          CommerceContext commerceContext,
          IFormFile importFile,
          string mode,
          int errorThreshold,
          bool publishEntities = true)
        {
            CommerceCommand commerceCommand;
            using (CommandActivity.Start(commerceContext, (CommerceCommand)this))
            {
                CommercePipelineExecutionContextOptions pipelineContextOptions = commerceContext.PipelineContextOptions;
                ImportPriceBooksArgument priceBooksArgument = new ImportPriceBooksArgument(importFile, mode)
                {
                    ErrorThreshold = errorThreshold
                };

                if (publishEntities)
                {
                    commerceContext.Environment.SetPolicy(new AutoApprovePolicy());
                }

                ImportResult importResult = await ImportPriceBooksPipeline.Run(priceBooksArgument, pipelineContextOptions);

                if (importResult != null)
                {
                    this.Messages.AddRange((IEnumerable<CommandMessage>)importResult.Errors);
                    this.ResponseCode = importResult.ResultCode;
                }
                else
                {
                    this.ResponseCode = commerceContext.GetPolicy<KnownResultCodes>().Error;
                }

                if (publishEntities)
                {
                    commerceContext.Environment.RemovePolicy(typeof(AutoApprovePolicy));
                }

                commerceCommand = (CommerceCommand)this;
            }
            return commerceCommand;
        }
    }
}
