using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MongoDbASPNetWebAPI.Models
{
    public class HeroItem
    {
        public int id { get; set; }
        public string name { get; set; }        
    }

    public class OurResponse
    {
        public string message { get; set; }
        public List<HeroItem> data { get; set; }
    }
}