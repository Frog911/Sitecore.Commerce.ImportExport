using Microsoft.AspNetCore.Http;
using Sitecore.Commerce.Core;
using Sitecore.Commerce.Plugin.Catalog;
using Sitecore.Framework.Conditions;
using System.Collections.Generic;
using System.IO.Compression;

namespace Plugin.Sample.ImportExportPriceBook.Pipelines.Arguments
{
    public class ImportPriceBooksArgument : PipelineArgument, IImportArgument
    {
        internal const int DefaultErrorThreshold = 100;

        public ImportPriceBooksArgument(IFormFile importFile, string mode)
        {
            Condition.Requires(importFile, nameof(importFile)).IsNotNull();
            Condition.Requires(mode, nameof(mode)).IsNotNullOrWhiteSpace();
            this.ImportFile = importFile;
            this.Mode = mode;
        }

        public IFormFile ImportFile { get; set; }

        public string Mode { get; set; }
        public int BatchSize { get; set; }

        public int ErrorThreshold { get; set; } = 100;

        public ZipArchive ImportArchive { get; set; }

        public IEnumerable<ZipArchiveEntry> EntityArchiveEntries { get; set; }

        public IEnumerable<ZipArchiveEntry> RelationshipArchiveEntries { get; set; }
    }
}
