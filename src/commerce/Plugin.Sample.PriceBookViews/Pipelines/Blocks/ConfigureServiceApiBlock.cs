using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Builder;
using Sitecore.Commerce.Core;
using Sitecore.Framework.Pipelines;

namespace Plugin.Sample.PriceBookViews.Pipelines.Blocks
{
    public class ConfigureServiceApiBlock: PipelineBlock<ODataConventionModelBuilder, ODataConventionModelBuilder, CommercePipelineExecutionContext>
    {
        public override Task<ODataConventionModelBuilder> Run(ODataConventionModelBuilder arg, CommercePipelineExecutionContext context)
        {
            ActionConfiguration pricebookDownload = arg.Action("PriceBookDownload");
            pricebookDownload.Returns<FileResult>();
            return Task.FromResult(arg);
        }
    }
}