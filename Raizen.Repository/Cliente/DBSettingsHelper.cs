using System;
using System.Collections.Generic;
using System.Configuration;

namespace Raizen.Repository
{
    internal static class DBSettingsHelper
    {
        private static IDictionary<string, string> CONN_STR_DICT = new Dictionary<string, string>();

        public static string ConnectionString(string name)
        {
            if (CONN_STR_DICT.ContainsKey(name))
            {
                return CONN_STR_DICT[name];
            }

            if (ConfigurationManager.ConnectionStrings[name] == null)
            {
                throw new Exception("Invalid connection name: " + name);
            }

            string connStr = ConfigurationManager.ConnectionStrings[name].ConnectionString;

            CONN_STR_DICT.Add(name, connStr);

            return connStr;
        }
    }
}
