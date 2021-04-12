using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App1.Models
{
    class Dataview
    {
        public class NumberSend
        {
            public string Command { get; set; }
            
            public int Nums { get; set; }
        }
        public class NumberReceive
        {
            public int Nums { get; set; }
        }
    }
}
