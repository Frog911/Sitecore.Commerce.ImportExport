using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Internal;
using Microsoft.AspNetCore.Mvc;
using Plugin.Sample.ImportExportPriceBook.Commands;
using Sitecore.Commerce.Core;
using System;
using System.Globalization;
using System.IO;
using System.Web.Http.OData;

namespace Plugin.Sample.ImportExportPriceBook.Controllers
{
    public class CommandsController : CommerceController
    {
        public CommandsController(
      IServiceProvider serviceProvider,
      CommerceEnvironment globalEnvironment)
      : base(serviceProvider, globalEnvironment)
        {
        }

        [HttpPost]
        [DisableRequestSizeLimit]
        [Route("ImportPriceBooks()")]
        public IActionResult ImportPriceBooks([FromBody] ODataActionParameters value)
        {
            if (!this.ModelState.IsValid || value == null)
            {
                return new BadRequestObjectResult(this.ModelState);
            }

            if (!value.ContainsKey("importFile") || value["importFile"] == null)
            {
                return new BadRequestObjectResult(value);
            }

            IFormFile formFile = (IFormFile)value["importFile"];
            MemoryStream memoryStream = new MemoryStream();
            formFile.CopyTo(memoryStream);
            FormFile file = new FormFile(memoryStream, 0L, formFile.Length, formFile.Name, formFile.FileName);
            string mode = value["mode"].ToString();
            //int batchSize = 900;

            //if (value.ContainsKey("batchSize"))
            //{
            //    batchSize = int.Parse(value["batchSize"].ToString(), CultureInfo.InvariantCulture);
            //}

            int errorThreshold = 100;

            if (value.ContainsKey("errorThreshold"))
            {
                errorThreshold = int.Parse(value["errorThreshold"].ToString(), CultureInfo.InvariantCulture);
            }

            bool publishEntities = true;
            bool result;

            if (value.ContainsKey("publish") && bool.TryParse(value["publish"].ToString(), out result))
            {
                publishEntities = result;
            }

            var command = this.ExecuteLongRunningCommand(() => this.Command<ImportPriceBooksCommand>().Process(this.CurrentContext, file, mode, errorThreshold, publishEntities));

            return new ObjectResult(command);
        }
    }
}
