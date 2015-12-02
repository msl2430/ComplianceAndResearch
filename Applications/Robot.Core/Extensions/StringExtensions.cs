using System;
using System.Collections.Generic;
using System.Linq;

namespace Robot.Core.Extensions
{
    public static class StringExtensions
    {
        public static List<int> ToIntList(this string input, char separator = ',')
        {
            try
            {
                return input.Split(separator).Select(x => Convert.ToInt32(x)).ToList();
            }
            catch
            {
                return new List<int>();

            }
        }

        /// <summary>
        /// Takes magnetic card number string and returns in the format ********1234
        /// </summary>
        /// <param name="cardNumber"></param>
        /// <returns></returns>
        public static string ToProtectedMagneticCardNumber(this string cardNumber)
        {
            return !string.IsNullOrEmpty(cardNumber) && cardNumber.Length > 4 ? cardNumber.Substring(cardNumber.Length - 4).PadLeft(cardNumber.Length, '*') : cardNumber;
        }

        public static string ToRelativeDirectoryPath(this string path)
        {
            path = path.Replace(@"/", @"\");
            if (!path.Contains(@"~"))
            {
                path = path.Insert(0, path.Substring(0, 1) != @"\" ? @"~\" : @"~");
            }

            if (!path.EndsWith(@"\"))
                path = path.Insert(path.Length, @"\");

            return path;
        }

        /// <summary>
        /// Returns the left x characters in a string
        /// </summary>
        /// <param name="source">Source string</param>
        /// <param name="numberOfCharacters">Number of characters to include</param>
        /// <param name="addEllipsis">Add ... if the string gets cut off</param>
        /// <param name="looseCutoff">If cutoff occurs midword then cut off after the word</param>
        /// <returns>New string with x left characters</returns>
        public static string Left(this string source, int numberOfCharacters, bool addEllipsis, bool looseCutoff)
        {
            if (source == null || source.Length <= numberOfCharacters)
            {
                return source;
            }

            string optionalEllipsis = "";
            if (addEllipsis)
            {
                if (source.Length <= numberOfCharacters + 4)
                    return source;
                numberOfCharacters -= 4;
                optionalEllipsis = "...";
            }

            if (looseCutoff)
            {
                var idx = source.IndexOf(' ', numberOfCharacters);
                if (idx > 0)
                {
                    numberOfCharacters = idx;
                }
            }

            return String.Concat(source.Left(numberOfCharacters).Trim(), optionalEllipsis);
        }

        /// <summary>
        /// Returns the left x characters in a string
        /// </summary>
        /// <param name="source">Source string</param>
        /// <param name="numberOfCharacters">Number of characters to include</param>
        /// <returns>New string with x left characters</returns>
        public static string Left(this string source, int numberOfCharacters)
        {
            return source.LeftOrRight(numberOfCharacters, 0);
        }

        //trims left or right based on starting point
        private static string LeftOrRight(this string source, int numberOfCharacters, int whereToStart)
        {
            if (source.Length < numberOfCharacters || source.Length == 0 || String.IsNullOrEmpty(source))
                return source;
            else
                return source.Substring(whereToStart, numberOfCharacters);
        }

       
        /// <summary>
        /// Adds an * to both sides of the string for full wildcard searching
        /// </summary>
        /// <param name="strIn"></param>
        /// <returns></returns>
        public static string ToWildcardAll(this string strIn)
        {
            return string.Concat("*", strIn, "*");
        }

        public static string ToProtectedUrlOrQuerystring(this string strIn)
        {
            var protectedUrl = strIn;
            if (protectedUrl.Contains("password="))
            {
                var password = GetQuerystringValue(protectedUrl, "password");
                protectedUrl = protectedUrl.Replace(password, "".PadLeft(password.Length, '*'));
            }
            if (protectedUrl.Contains("magneticCardNumber="))
            {
                var mc = GetQuerystringValue(protectedUrl, "magneticCardNumber");
                protectedUrl = protectedUrl.Replace(mc, "".PadLeft(mc.Length, '*'));
            }
            if (protectedUrl.Contains("reference="))
            {
                var mc = GetQuerystringValue(protectedUrl, "reference");
                protectedUrl = protectedUrl.Replace(mc, "".PadLeft(mc.Length, '*'));
            }
            return protectedUrl;
        }

        private static string GetQuerystringValue(string strIn, string parameterName)
        {
            var searchString = parameterName + "=";
            return strIn.Substring(strIn.IndexOf(searchString) + searchString.Length, strIn.IndexOf("&", strIn.IndexOf(searchString) + searchString.Length) - (strIn.IndexOf(searchString) + searchString.Length));
        }
    }
}
