using System;
using System.Configuration;
using System.Threading;

namespace server
{
    class Program
    {
        static void Main(string[] args)
        {
            ExeConfigurationFileMap customConfigFileMap = new ExeConfigurationFileMap();
            customConfigFileMap.ExeConfigFilename = "config.xml";

            Configuration customConfig = ConfigurationManager.OpenMappedExeConfiguration(customConfigFileMap, ConfigurationUserLevel.None);

            AppSettingsSection appSettings = (customConfig.GetSection("appSettings") as AppSettingsSection);

            string ip = appSettings.Settings["ip"].Value;
            string port = appSettings.Settings["port"].Value;
               


            Console.WriteLine("**** Server start ****");

                       
            Thread t = new Thread(delegate ()
            {
                
                Server myserver = new Server(ip, Int32.Parse(port));
            });
            t.Start();

            
            //Console.WriteLine("össszeg : {0}", Data.ReadColoumnSum());


            //TODO: server leállítás kilépés
        }

        
        

        

        

    }
}
