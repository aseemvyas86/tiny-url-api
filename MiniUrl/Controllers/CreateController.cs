using System;
using Microsoft.AspNetCore.Mvc;
using MiniUrl.Data;
using MiniUrl.Services;

namespace MiniUrl.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CreateController : ControllerBase
    {
        private IUrlShortener _urlShortener;

        public CreateController(IUrlShortener urlShortener)
        {
            _urlShortener = urlShortener;
        }

        [HttpPost]
        public IActionResult CreateUrl([FromBody] string originalUrl)
        {
            string shorten;
            string shortenUrl;
            try
            {
                shorten = _urlShortener.CreateTinyUrl(originalUrl);
                shortenUrl = $"{HttpContext.Request.Scheme}://{HttpContext.Request.Host.Host}/{shorten}";
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
            return Ok(new UrlMapDetails()
            {
                ShortenUrl = shortenUrl,
                OriginalUrl = originalUrl,
                Shorten = shorten
            });
        }
    }
}
