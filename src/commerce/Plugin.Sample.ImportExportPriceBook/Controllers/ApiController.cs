using Microsoft.AspNetCore.Mvc;
using Plugin.Sample.ImportExportPriceBook.Commands;
using Sitecore.Commerce.Core;
using Sitecore.Framework.Conditions;
using System;
using System.Threading.Tasks;
using System.Web.Http.OData;

namespace Plugin.Sample.ImportExportPriceBook.Controllers
{

    public class ApiController : CommerceController
    {
        public ApiController(IServiceProvider serviceProvider, CommerceEnvironment globalEnvironment)
          : base(serviceProvider, globalEnvironment)
        {
        }

        [HttpPut]
        [Route("ExportPriceBooks()")]
        public async Task<IActionResult> ExportPriceBooks([FromBody] ODataActionParameters value)
        {
            Condition.Requires(value, nameof(value)).IsNotNull();
            string fileName = value["fileName"].ToString();
            string mode = value["mode"].ToString();
            int maximumItemsPerFile = 500;

            if (value.ContainsKey("maximumItemsPerFile"))
            {
                maximumItemsPerFile = int.Parse(value["maximumItemsPerFile"].ToString());
            }

            return await Command<ExportPriceBooksCommand>().Process(this.CurrentContext, fileName, mode, maximumItemsPerFile);
        }
    }
}
