using System;
using Microsoft.AspNetCore.Mvc;
using Sitecore.Commerce.Core;

namespace Plugin.Sample.PriceBookViews.Controllers
{
    public class ApiController: CommerceController
    {
        public ApiController(IServiceProvider serviceProvider, CommerceEnvironment globalEnvironment) : base(serviceProvider, globalEnvironment)
        {
        }

        [HttpPost]
        [Route("PriceBookDownload()")]
        public IActionResult PriceBookDownload()
        {
            var filePath = @"c:\inetpub\wwwroot\CommerceAuthoring_Sc9\App.config";
            return PhysicalFile(filePath, "text/plain");
        }
    }
}