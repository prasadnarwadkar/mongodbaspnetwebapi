using MongoDB.Bson;
using MongoDB.Driver;
using MongoDbASPNetWebAPI.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;

namespace MongoDbASPNetWebAPI
{
    public class OurHeroesController : ApiController
    {
        MongoClient client = null;

        IMongoDatabase database = null;
        IMongoCollection<BsonDocument> collection = null;

        public OurHeroesController()
        {
            client = new MongoClient(ConfigurationManager.AppSettings["mongodburl"]);

            database = client.GetDatabase(ConfigurationManager.AppSettings["mongodbname"]);
            collection = database.GetCollection<BsonDocument>(ConfigurationManager.AppSettings["mongodbcollname"]);
        }

        // GET api/<controller>
        public HttpResponseMessage Get()
        {
            List<HeroItem> list = new List<HeroItem>();
            
            var documents = collection.Find(new BsonDocument()).ToList();

            foreach (var document in documents)
            {
                HeroItem item = new HeroItem();

                var str = document.Elements.ElementAt(1).Value.ToString();
                int id = 0;

                if (!int.TryParse(str, out id))
                {
                    continue;
                }

                item.id = id;
                item.name = document.Elements.ElementAt(2).Value.ToString();
                list.Add(item);
            }

            OurResponse response= new OurResponse();
            response.message ="";
            response.data = list;

            return Request.CreateResponse(HttpStatusCode.OK, response);
        }

        // GET api/<controller>/5
        public HeroItem Get(int id)
        {
            List<HeroItem> list = new List<HeroItem>();

            var myquery = "{\"id\":" + id + "}";
            var documents = collection.Find(myquery).ToList();

            foreach (var document in documents)
            {
                HeroItem item = new HeroItem();

                var str = document.Elements.ElementAt(1).Value.ToString();
                id = 0;

                if (!int.TryParse(str, out id))
                {
                    continue;
                }

                item.id = id;
                item.name = document.Elements.ElementAt(2).Value.ToString();
                list.Add(item);
            }

            return list.FirstOrDefault();
        }

        // POST api/<controller>
        [AcceptVerbs("OPTIONS", "POST")]
        [HttpPost]
        public HttpResponseMessage Post(HeroItem value)
        {
            Dictionary<string, object> dict = new Dictionary<string, object>();
            dict.Add("id", new Random().Next(1000));
            dict.Add("name", value.name);

            BsonDocument doc = new BsonDocument(dict);
            collection.InsertOne(doc);

            return Request.CreateResponse(HttpStatusCode.OK, "0");
        }

        // PUT api/<controller>/5
        [AcceptVerbs("OPTIONS", "PUT")]
        [HttpPut]
        public HttpResponseMessage Put(HeroItem value)
        {
            var filter = Builders<BsonDocument>.Filter.Eq<int>("id", value.id);
            var update = Builders<BsonDocument>.Update.Set("name", value.name);

            var result = collection.UpdateOne(filter, update);
           
            return Request.CreateResponse(HttpStatusCode.OK, result.ModifiedCount);
        }

        
    }
}