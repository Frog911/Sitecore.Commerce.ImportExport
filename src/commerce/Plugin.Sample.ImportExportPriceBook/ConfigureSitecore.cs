namespace Plugin.Sample.ImportExportPriceBook
{
    using System.Reflection;
    using global::Plugin.Sample.ImportExportPriceBook.Pipelines;
    using global::Plugin.Sample.ImportExportPriceBook.Pipelines.Blocks;
    using Microsoft.Extensions.DependencyInjection;
    using Sitecore.Commerce.Core;
    using Sitecore.Commerce.Plugin.Catalog;
    using Sitecore.Framework.Configuration;
    using Sitecore.Framework.Pipelines.Definitions.Extensions;

    public class ConfigureSitecore : IConfigureSitecore
    {
        public void ConfigureServices(IServiceCollection services)
        {
            var assembly = Assembly.GetExecutingAssembly();
            services.RegisterAllPipelineBlocks(assembly);

            services.Sitecore().Pipelines(config => config

             .AddPipeline<IImportPriceBooksPipeline, ImportPriceBooksPipeline>(
                    configure =>
                    {
                        configure.Add<ImportPriceBooksPrepareBlock>();
                        configure.Add<ImportBulkLocalizationEntitiesBlock>();
                        configure.Add<ImportPriceBooksBlock>();
                        configure.Add<ImportPriceCardsBlock>();
                        configure.Add<ImportIPriceRelationshipsBlock>();
                        configure.Add<ImportFinalizeBlock>();
                    })
              .AddPipeline<IRemoveAllPriceBooksPipeline, RemoveAllPriceBooksPipeline>(
                    configure =>
                    {
                        configure.Add<LoadRelationshipDefinitionsBlock>();
                        configure.Add<RemoveAllPriceCardsBlock>();
                        configure.Add<RemoveAllPriceBooksBlock>();
                    })
               .AddPipeline<IExportPriceBooksPipeline, ExportPriceBooksPipeline>(
                    configure =>
                    {
                        configure.Add<ExportPriceBooksBlock>();
                    })
               .AddPipeline<IStreamPriceBooksToArchivePipeline, StreamPriceBooksToArchivePipeline>(
                    configure =>
                    {
                        configure.Add<LoadRelationshipDefinitionsBlock>();
                        configure.Add<StreamEntitiesToArchivePrepareBlock>();
                        configure.Add<StreamPriceBooksToArchiveBlock>();
                        configure.Add<StreamPriceCardsToArchiveBlock>();
                       // configure.Add<StreamLocalizationEntitiesToArchiveBlock>(); ///TODO: investigate
                        configure.Add<StreamEntitiesToArchiveFinalizeBlock>();
                    })

               .ConfigurePipeline<IConfigureServiceApiPipeline>(configure => configure.Add<ConfigureServiceApiBlock>()));

            services.RegisterAllCommands(assembly);
        }
    }
}