using System.Reflection;
using Microsoft.Extensions.DependencyInjection;
using Plugin.Sample.CatalogViews.Pipelines.Blocks;
using Sitecore.Commerce.Core;
using Sitecore.Commerce.EntityViews;
using Sitecore.Framework.Configuration;
using Sitecore.Framework.Pipelines.Definitions.Extensions;

namespace Plugin.Sample.CatalogViews
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
                            c => { c.Add<PopulateCatalogImportActionsBlock>(); })
                        .ConfigurePipeline<IGetEntityViewPipeline>(c =>
                        {
                            c.Add<GetCatalogImportViewBlock>().Add<GetCatalogExportViewBlock>();
                        })
                        .ConfigurePipeline<IDoActionPipeline>(c =>
                        {
                            c.Add<DoActionCatalogImportBlock>();
                        })
            );
        }
    }
}