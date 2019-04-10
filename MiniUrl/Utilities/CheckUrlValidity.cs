using System;
namespace MiniUrl.Utilities
{
    public static class CheckUrlValidity
    {
        public static bool CheckURLValid(string source)
        {

            return Uri.IsWellFormedUriString(source, UriKind.Absolute);
        }

        public static bool CheckHttpInUrl(string source)
        {
            if (source.Contains("http"))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public static string RemoveProtocol(string url)
        {

            int i = url.IndexOf(':');
            if (i > 0)
            {
                url = url.Substring(i + 1);
            }
            return url;

        }
    }
}
