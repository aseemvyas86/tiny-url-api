using System;
using Microsoft.AspNetCore.Mvc;
using MiniUrl.Data;
using MiniUrl.Services;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace MiniUrl.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DetailController : ControllerBase
    {
        private IUrlShortener _urlShortener;

        public DetailController(IUrlShortener urlShortener)
        {
            _urlShortener = urlShortener;
        }

        [HttpGet("{shortUrl}")]
        public IActionResult ShortUrlDetails([FromRoute] string shortUrl)
        {
            DetailsViewModel model;
            try
            {
                var details = _urlShortener.RetrieveUrlDetails(shortUrl);
                var originalUrl = _urlShortener.ShortToLongUrl(shortUrl);
                model = new DetailsViewModel()
                {
                    Details = details,
                    ShortenUrl = shortUrl,
                    OriginalUrl = originalUrl
                };
            }
            catch (Exception e)
            {
                return NotFound(e.Message);
            }

            return Ok(model);
        }
    }
}
