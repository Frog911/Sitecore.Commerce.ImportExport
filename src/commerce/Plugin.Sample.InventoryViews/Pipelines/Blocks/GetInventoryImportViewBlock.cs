﻿using System;
using System.Threading.Tasks;
using Plugin.Sample.CatalogViews.Models;
using Plugin.Sample.InventoryViews.Policies;
using Sitecore.Commerce.Core;
using Sitecore.Commerce.EntityViews;
using Sitecore.Commerce.Plugin.Inventory;
using Sitecore.Framework.Pipelines;

namespace Plugin.Sample.InventoryViews.Pipelines.Blocks
{
    public class GetInventoryImportViewBlock : PipelineBlock<EntityView, EntityView, CommercePipelineExecutionContext>
    {
        public override Task<EntityView> Run(EntityView arg, CommercePipelineExecutionContext context)
        {
            if (string.IsNullOrEmpty(arg?.Name) ||
                !arg.Name.Equals(context.GetPolicy<KnownInventoryViewsPolicy>().InventorySets,
                    StringComparison.OrdinalIgnoreCase) || !string.IsNullOrEmpty(arg.Action) &&
                !arg.Action.Equals(context.GetPolicy<KnownImportInventoryActionsPolicy>().InventoryImport,
                    StringComparison.OrdinalIgnoreCase))
                return Task.FromResult(arg);
            arg.Properties.Add(new ViewProperty()
                {
                    Name = nameof(InputModel.FileContent),
                    DisplayName = "File",
                    RawValue = string.Empty,
                    Value = string.Empty,
                    IsReadOnly = false,
                    IsHidden = false,
                    UiType = "FileUpload"
                }
            );
            return Task.FromResult(arg);
        }
    }
}