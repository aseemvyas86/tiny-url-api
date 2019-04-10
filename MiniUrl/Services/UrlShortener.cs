using System;
using MiniUrl.Data;
using Newtonsoft.Json;
using MiniUrl.Utilities;
using System.Collections.Generic;

namespace MiniUrl.Services
{
    public class UrlShortener:IUrlShortener
    {

        private IConversion _conversion;
        private IStorage _storage;

        public UrlShortener(IConversion conversion, IStorage storage)
        {
            _conversion = conversion;
            _storage = storage;
        }

        public string CreateTinyUrl(string originalUrl)
        {
            // Check in the system for lonuRL CREATION
            string checkSum = MD5Hash.CalculateMD5Hash(originalUrl);
            if (_storage.ExistInHash(Constants.LongToShortUrlTable, checkSum))
            {
                return _storage.GetValueFromHash(Constants.LongToShortUrlTable, checkSum);
            }

            //Check currentId exist or not
            string currentId;
            if (_storage.ExistInHash(Constants.CurrentIdTable, Constants.CurrentId))
            {
                currentId = _storage.GetValueFromHash(
                             Constants.CurrentIdTable, Constants.CurrentId);
            }
            else
            {
                currentId = Constants.InitialCurrentIdValue;
                _storage.AddValueToHash
                         (Constants.CurrentIdTable, Constants.CurrentId, currentId);
            }

            ulong id = ulong.Parse(currentId);
            string shortenurl = _conversion.Encode(id);

            _storage.AddValueToHash(Constants.LongToShortUrlTable, checkSum, shortenurl);

            // Create a map of shortenUrl id to original url 
            _storage.AddValueToHash(Constants.UrlTable, currentId, originalUrl);

            // Increment the id by 1
            ulong next = id + (ulong)1;

            // update the current id table
            _storage.UpdateValueToHash(Constants.CurrentIdTable, Constants.CurrentId, next.ToString());

            return shortenurl;
        }

        public List<UrlDetails> RetrieveUrlDetails(string shortenUrl)
        {
            if (_storage.ExistInHash(Constants.ShortenUrlDetailTable, shortenUrl))
            {
                string urlDetailsJson = _storage
                                      .GetValueFromHash(Constants.ShortenUrlDetailTable, shortenUrl);
                List<UrlDetails> urldetails = JsonConvert
                                          .DeserializeObject<List<UrlDetails>>(urlDetailsJson);
                return urldetails;
            }
            return new List<UrlDetails>();
        }

        public string ShortToLongUrl(string shortenUrl)
        {
            ulong id = _conversion.Decode(shortenUrl);
            return _storage.GetValueFromHash(Constants.UrlTable, id.ToString());
        }

        public void UpdateShortenUrlDetails(string shorten, string userAgent, string ipaddress)
        {
            ulong id = _conversion.Decode(shorten);
            if (_storage.ExistInHash(Constants.ShortenUrlDetailTable, shorten))
            {
                string json = _storage.GetValueFromHash(Constants.ShortenUrlDetailTable, shorten);
                List<UrlDetails> listOfUrlDetails = JsonConvert
                                                   .DeserializeObject<List<UrlDetails>>(json);
                UrlDetails urlDetails = new UrlDetails();
                urlDetails.DeviceDetails = userAgent;
                urlDetails.IpAddress = ipaddress;
                urlDetails.TimeStamp = DateTime.Now.ToString("ddd, dd MMM yyy HH’:’mm’:’ss ‘GMT’");
                urlDetails.OriginalUrl = _storage.GetValueFromHash(Constants.UrlTable, id.ToString());
                listOfUrlDetails.Add(urlDetails);
                string updatedJson = JsonConvert.SerializeObject(listOfUrlDetails);
                _storage.UpdateValueToHash(Constants.ShortenUrlDetailTable, shorten, updatedJson);

            }
            else
            {
                List<UrlDetails> listOfUrlDetails = new List<UrlDetails>();
                UrlDetails urlDetails = new UrlDetails();
                urlDetails.DeviceDetails = userAgent;
                urlDetails.IpAddress = ipaddress;
                urlDetails.TimeStamp = DateTime.Now.ToString("ddd, dd MMM yyy HH’:’mm’:’ss ‘GMT’");
                urlDetails.OriginalUrl = _storage.GetValueFromHash(Constants.UrlTable, id.ToString());
                listOfUrlDetails.Add(urlDetails);
                string json = JsonConvert.SerializeObject(listOfUrlDetails);
                _storage.AddValueToHash(Constants.ShortenUrlDetailTable, shorten, json);
            }

        }

        public string RedirectToLongUrl(string shorten)
        {
            ulong id = _conversion.Decode(shorten);
            string originalUrl = _storage.GetValueFromHash(Constants.UrlTable, id.ToString());
            if (CheckUrlValidity.CheckHttpInUrl(originalUrl))
            {
                return originalUrl;
            }
            return $"http://{originalUrl}";
        }
    }
}
