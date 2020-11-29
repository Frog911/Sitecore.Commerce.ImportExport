using Sitecore.Commerce.Plugin.Catalog;
using Sitecore.Commerce.Plugin.Pricing;
using Sitecore.Framework.Conditions;
using System;
using System.Collections.Generic;

namespace Plugin.Sample.ImportExportPriceBook.Helpers
{
    public static class PriceBookExportHelper
    {
        private static readonly Dictionary<string, Type> BuiltInEntityTypes = new Dictionary<string, Type>(StringComparer.OrdinalIgnoreCase)
        {
              {
                typeof (PriceCard).Name,
                typeof (PriceCard)
              },
              {
                typeof (PriceCard).FullName,
                typeof (PriceCard)
              },
              {
                typeof (PriceCard).AssemblyQualifiedName,
                typeof (PriceCard)
              },
              {
                typeof (PriceBook).Name,
                typeof (PriceBook)
              },
              {
                typeof (PriceBook).FullName,
                typeof (PriceBook)
              },
              {
                typeof (PriceBook).AssemblyQualifiedName,
                typeof (PriceBook)
              }
        };

        public static Type TryGetTargetEntityType(RelationshipDefinition relationshipDef)
        {
            Condition.Requires(relationshipDef, nameof(relationshipDef)).IsNotNull();
            Type type = Type.GetType(relationshipDef.TargetType);
            if (type == null)
            {
                BuiltInEntityTypes.TryGetValue(relationshipDef.TargetType, out type);
            }

            return type;
        }
    }
}
