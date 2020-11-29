using System.Reflection;
using Microsoft.Extensions.DependencyInjection;
using Plugin.Sample.InventoryViews.Pipelines.Blocks;
using Sitecore.Commerce.Core;
using Sitecore.Commerce.EntityViews;
using Sitecore.Framework.Configuration;
using Sitecore.Framework.Pipelines.Definitions.Extensions;

namespace Plugin.Sample.InventoryViews
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
                            c => { c.Add<PopulateInventoryImportActionsBlock>(); })
                        .ConfigurePipeline<IGetEntityViewPipeline>(c =>
                        {
                            c.Add<GetInventoryImportViewBlock>().Add<GetInventoryExportViewBlock>();
                        })
                        .ConfigurePipeline<IDoActionPipeline>(c =>
                        {
                            c.Add<DoActionInventoryImportBlock>();
                        })
            );
        }
    }
}