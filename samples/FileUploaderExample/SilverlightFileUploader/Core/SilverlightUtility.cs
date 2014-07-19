/*
Copyright 2003-2009 Virtual Chemistry, Inc. (VCI)
http://www.virtualchemistry.com
Using .Net, Silverlight, SharePoint and more to solve your tough problems in web-based data management.

Author: Peter Coley
*/

using System;
using System.Collections.Generic;
using System.Linq;

namespace Vci.Core
{
    public class SilverlightUtility
    {
        public const string COMMA_TOKEN = "{COMMA}";

        /// <summary>
        /// Build an init params string from the supplied collection of name,value pairs.
        /// </summary>
        /// <param name="InitParams"></param>
        /// <returns></returns>
        public static string BuildInitParamsString(Dictionary<string, string> InitParams)
        {
            Dictionary<string, string> parsedParams = new Dictionary<string, string>();
            foreach (string key in InitParams.Keys)
            {
                if (!string.IsNullOrEmpty(key) && !string.IsNullOrEmpty(InitParams[key]))
                    parsedParams[EscapeInitParam(key)] = EscapeInitParam(InitParams[key]);
            }

            return string.Join(",", parsedParams.Select(p => p.Key + "=" + p.Value).ToArray());
        }

        /// <summary>
        /// Escape things that are not allowed in the silverlight init params string (currently just replaces commas with a token).
        /// </summary>
        /// <param name="InitParam"></param>
        /// <returns></returns>
        public static string EscapeInitParam(string InitParam)
        {
            return string.IsNullOrEmpty(InitParam) ? InitParam : InitParam.Replace(",", COMMA_TOKEN);
        }

        /// <summary>
        /// Given the collection of all init params, parse any special tokens in the parameters.
        /// </summary>
        /// <param name="InitParams"></param>
        /// <returns></returns>
        public static Dictionary<string, string> ParseInitParams(IDictionary<string, string> InitParams)
        {
            Dictionary<string, string> ret = new Dictionary<string, string>(InitParams.Count);

            foreach (string key in InitParams.Keys)
                ret[ParseInitParam(key)] = ParseInitParam(InitParams[key]);

            return ret;
        }

        /// <summary>
        /// Parse any special tokens in a single init param.
        /// </summary>
        /// <param name="InitParam"></param>
        /// <returns></returns>
        public static string ParseInitParam(string InitParam)
        {
            return string.IsNullOrEmpty(InitParam) ? InitParam : InitParam.Replace(COMMA_TOKEN, ",");
        }
    }
}
