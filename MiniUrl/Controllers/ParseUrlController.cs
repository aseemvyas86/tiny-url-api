using System;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using MiniUrl.Services;

namespace MiniUrl.Controllers
{
    [Route("{shorten:minlength(4)}")]
    [ApiController]
    public class ParseUrlController : Controller
    {
        private IUrlShortener _urlShortener;

        public ParseUrlController(IUrlShortener urlShortner)
        {
            _urlShortener = urlShortner;

        }

        [HttpGet]
        public IActionResult Get([FromRoute]string shorten)
        {
            string result = null;
            try
            {
                string userAgent = HttpContext.Request.Headers["User-Agent"].ToString();
                string ipaddress = HttpContext.Connection.RemoteIpAddress.ToString();
                _urlShortener.UpdateShortenUrlDetails(shorten, userAgent, ipaddress);
                result = _urlShortener.RedirectToLongUrl(shorten);
            }
            catch (Exception e)
            {
                return StatusCode(500, e.Message);
            }

            return new RedirectResult(result, false);
        }
    }
}
