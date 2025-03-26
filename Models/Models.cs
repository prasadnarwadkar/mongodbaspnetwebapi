using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MongoDbASPNetWebAPI.Models
{
    public class HeroItem
    {
        public string id { get; set; }
        public string name { get; set; }        
    }

    public class OurResponse
    {
        public string message { get; set; }
        public dynamic data { get; set; }
        public int success { get; internal set; }
    }

    public class heroPayload
    {
        public HeroItem heroItem { get; set; }
    }
}