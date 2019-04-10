using System.Collections.Generic;
using MiniUrl.Data;

namespace MiniUrl.Services
{
    public interface IUrlShortener
    {
        string CreateTinyUrl(string originalUrl);
        List<UrlDetails> RetrieveUrlDetails(string shortenUrl);
        void UpdateShortenUrlDetails(string shorten, string userAgent, string ipAddress);
        string RedirectToLongUrl(string shorten);
        string ShortToLongUrl(string shortenUrl);
    }
}
