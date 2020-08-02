using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Test
{
    /// <summary>
    /// The observer monitors the list of servers and updates the database information after a certain delay
    /// </summary>
    public class Observer
    {
        // Array of servers monitored by the observer 
        public readonly ServerInfo[] ServerInfos;

        public int DelaySeconds { get; set; }

        public Observer(ServerInfo[] serverInfos, int delaySeconds)
        {
            ServerInfos = serverInfos;
            DelaySeconds = delaySeconds;
        }

        public void Update()
        {
            //date of update
            var date = DateTime.Now.Date.ToString("MM.dd.yyyy");
            foreach (var serverInfo in ServerInfos) // each of servers
            {
                var data = serverInfo.GetInfo().Result // data with updated information about server
               .Select(x => new List<object>() { serverInfo.ServerName, x.Name, x.Size, date })
               .Select(x => (IList<object>)x)
               .ToList();
                data.Add(new List<object>() { serverInfo.ServerName, "Свободно", serverInfo.SizeLeft, date }); // add last row with size left
                GService.Update(data, serverInfo.ServerName, 1);
            }
        }

        // For each server create sheets and add titles if does not exist 
        public void Create()
        {
            foreach (var serverInfo in ServerInfos)
            {
                GService.CreateSheet(serverInfo.ServerName);
                GService.Update(new List<IList<object>>() { new List<object>() { "Сервер", "База данных", "Размер в ГБ", "Дата обновления" } }, serverInfo.ServerName);
            }
        }

        // Async task for always monitoring
        void LoopUpdateAsync()
        {
            var task = Task.Run(async () =>
            {
                for (; ; )
                {
                    await Task.Delay(TimeSpan.FromSeconds(DelaySeconds)); // delay 
                    Update(); // update information of all servers
                }
            });
        }

        public void Run()
        {
            Create(); // init sheets and titles if does not exist
            LoopUpdateAsync(); // start a monitoring
        }
    }
}
