using System;
using System.Threading;

namespace server
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("**** Server start ****");

            //WriteDataToDatabase(32);

            //TODO: Server socket adat fogadás küldés 
            Thread t = new Thread(delegate ()
            {
                
                Server myserver = new Server("192.168.8.253", 13000);
            });
            t.Start();

            
            Console.WriteLine("össszeg : {0}", Data.ReadColoumnSum());


            //TODO: server leállítás kilépés
        }

        
        

        

        

    }
}
