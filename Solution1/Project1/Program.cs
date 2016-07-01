using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Web.UI;
using System.Web.Script.Serialization;
using System.IO;
using System.Diagnostics;

namespace Project1 {
    class Program {

        static void Main(string[] args) {
            Console.Write(ServerMethods.GetMe());
            Console.ReadKey();

        }
    }
}
