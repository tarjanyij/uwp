using System;
using System.Collections.Generic;
using System.Text;
using static server.Data;

namespace server.Controller
{
    class AppMain
    {
        
        public AppMain()
        {
           
        }
        public object ReadColoum(int options)
        {
            var num = Data.ReadColoumnSum();
            return new Number() { Nums = num };
        }

        public object AddNumber(int data)
        {
            Data.WriteDataToDatabase(data);

            return ReadColoum(0);
        }
        public object SubtractsNumber(int data)
        {
            data = data * -1;
            Data.WriteDataToDatabase(data);

            return ReadColoum(0);
        }
    }
}
