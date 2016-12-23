using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TT.Models;

namespace TT.Lib.Util
{
    public class MatchHashGenerator
    {
        private static string chars = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";

        public static string GenerateMatchHash(Match match)
        {
            if (match != null)
            {
                throw new NotImplementedException();
            } else
            {
                var random = new Random();
                var randHash = "";
                for (var i = 0; i < 8; i++)
                    randHash += chars[random.Next(chars.Length)];
                return randHash;
            }
        }
    }
}
