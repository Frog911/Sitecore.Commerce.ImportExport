using System.Reflection;
using Microsoft.Extensions.DependencyInjection;
using Plugin.Sample.PriceBookViews.Pipelines.Blocks;
using Sitecore.Commerce.Core;
using Sitecore.Commerce.EntityViews;
using Sitecore.Framework.Configuration;
using Sitecore.Framework.Pipelines.Definitions.Extensions;
using ConfigureServiceApiBlock = Plugin.Sample.PriceBookViews.Pipelines.Blocks.ConfigureServiceApiBlock;

namespace Plugin.Sample.PriceBookViews
{
    public class ConfigureSitecore : IConfigureSitecore
    {
        public void ConfigureServices(IServiceCollection services)
        {
            var assembly = Assembly.GetExecutingAssembly();
            services.RegisterAllPipelineBlocks(assembly);

            services.Sitecore().Pipelines(
                config =>
                    config
                        .ConfigurePipeline<IPopulateEntityViewActionsPipeline>(
                            c => { c.Add<PopulateImportPricingDashboardActionsBlock>(); })
                        .ConfigurePipeline<IGetEntityViewPipeline>(c =>
                        {
                            c.Add<GetPricebookImportViewBlock>().Add<GetPricebookExportViewBlock>();
                        })
                        .ConfigurePipeline<IDoActionPipeline>(c =>
                        {
                            c.Add<DoActionPricebookImportBlock>();
                        })
                        .ConfigurePipeline<IConfigureServiceApiPipeline>(c => { c.Add<ConfigureServiceApiBlock>(); })
            );
        }
    }
}