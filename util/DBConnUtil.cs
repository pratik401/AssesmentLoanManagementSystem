using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace util
{
    public static class DBConnUtil
    {
        private static IConfigurationRoot _configuration;
        static string s = null;

        static DBConnUtil()
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("C:\\Users\\HP\\source\\repos\\util\\appsettings.json",
                optional:true,reloadOnChange:true);
            _configuration = builder.Build();
        }
        
        public static string ReturnCn(string key)
        {
            s = _configuration.GetConnectionString("dbCn");
            return s;
        }
    }
}
