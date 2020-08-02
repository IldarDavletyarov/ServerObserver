using System;
using System.Configuration;
using System.Collections.Specialized;
using System.Linq;

namespace Test
{
    /// <summary>
    /// Main class.
    /// Get information from app.config and run monitoring
    /// </summary>
    class Program
    {
        static void Main()
        {
            var user = ConfigurationManager.AppSettings["user"]; // user for PostgreSQL
            var pass = ConfigurationManager.AppSettings["password"]; // password for PostgreSQL
            var delaySeconds = int.Parse(ConfigurationManager.AppSettings["delaySeconds"]); // delay for monitoring
            var servers =
                (NameValueCollection)ConfigurationManager.GetSection("Servers"); // get list of servers with ip addresses and capacity
            var allKeys = servers.AllKeys;
            var serverInfos = allKeys // from key-value to ServerInfo
            .Select(x => new ServerInfo(x, user, pass, double.Parse(servers[x])))
            .ToArray();
            var observer = new Observer(serverInfos, delaySeconds);
            observer.Run(); // run monitoring
            Console.ReadLine();
        }

    }
}
