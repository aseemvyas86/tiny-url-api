using System;
using System.Collections.Generic;
using System.Linq;

namespace MiniUrl.Services
{
    public class Base62Conversion:IConversion
    {
        private readonly string _alphabet;
        public Base62Conversion()
        {
            _alphabet = "0123456789abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ";
        }

        public ulong Decode(string value)
        {
            char[] charArray = value.ToCharArray();
            int arrayLength = charArray.Length;
            int counter = 1;
            List<ulong> storevalues = new List<ulong>();

            foreach (var i in charArray)
            {

                int index = _alphabet.IndexOf(i);
                double power = Math.Pow(62, arrayLength - counter);
                ulong result = (ulong)index * (ulong)power;
                storevalues.Add(result);
                counter++;
            }

            return storevalues.Aggregate(func: (result, item) => result + item);
        }

        public string Encode(ulong value)
        {
            var n = value;
            ulong basis = 62;
            var result = "";
            while (n > 0)
            {
                ulong temp = n % basis;
                result = _alphabet[(int)temp] + result;
                n = (n / basis);
            }
            return result;
        }
    }
}
