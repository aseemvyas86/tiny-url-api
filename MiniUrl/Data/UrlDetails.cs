using System;
using System.Collections.Generic;

namespace MiniUrl.Data
{
    public class UrlDetails
    {
        public string OriginalUrl { get; set; }
        public string DeviceDetails { get; set; }
        public string IpAddress { get; set; }
        public string TimeStamp { get; set; }
    }

    public class DetailsViewModel
    {
        public string OriginalUrl { get; set; }
        public string ShortenUrl { get; set; }
        public List<UrlDetails> Details { get; set; }
    }
}
