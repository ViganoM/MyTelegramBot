using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using JsonTypes;
using System.Web;
using System.Net;
using System.Web.Script.Serialization;


namespace Project1 {
    class ServerMethods {

        const string token = "225365907:AAE0D5dgDjzLlwHp5jLVVZMBmRZyPBFpepI";
        const string website = "https://api.telegram.org/bot"+token+"/";

        static public getMe GetMe() {
            string response = new WebClient().DownloadString(website + "getMe");
            return new JavaScriptSerializer().Deserialize<getMe>(response);
        }
        static public getUpdates GetUpdates() {
            string response = new WebClient().DownloadString(website + "getUpdates");
            return new JavaScriptSerializer().Deserialize<getUpdates>(response);
        }
    }
}