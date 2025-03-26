using MongoDB.Bson;
using MongoDB.Driver;
using MongoDbASPNetWebAPI.Models;
using Newtonsoft.Json;
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

        [Route("api/getHero")]
        // GET api/<controller>
        public HttpResponseMessage getHero()
        {
            List<HeroItem> list = new List<HeroItem>();
            
            var documents = collection.Find(new BsonDocument()).ToList();

            foreach (var document in documents)
            {
                HeroItem item = new HeroItem();

                var str = document.Elements.Where(e => e.Name == "id").FirstOrDefault().Value.ToString();

                item.id = str;
                item.name = document.Elements.Where(e => e.Name == "name").FirstOrDefault().Value.ToString();
                list.Add(item);
            }

            OurResponse response= new OurResponse();
            response.message ="";
            response.success = 200;
            response.data = list;

            var responseMsg = new HttpResponseMessage { StatusCode = HttpStatusCode.OK, Content = new StringContent( JsonConvert.SerializeObject( response) )};

            // Define and add values to variables: origins, headers, methods (can be global)               
            responseMsg.Headers.Add("Access-Control-Allow-Origin", "*");
            responseMsg.Headers.Add("Access-Control-Allow-Headers", "*");
            responseMsg.Headers.Add("Access-Control-Allow-Methods", "*");
            responseMsg.Headers.Add("Access-Control-Allow-Credentials", "true");

            return responseMsg;
        }

        // GET api/<controller>/5
        public HeroItem Get(Guid id)
        {
            List<HeroItem> list = new List<HeroItem>();

            var myquery = "{\"id\":" + id + "}";
            var documents = collection.Find(myquery).ToList();

            foreach (var document in documents)
            {
                HeroItem item = new HeroItem();

                var str = document.Elements.ElementAt(1).Value.ToString();

                item.id = str;
                item.name = document.Elements.ElementAt(2).Value.ToString();
                list.Add(item);
            }

            return list.FirstOrDefault();
        }

        // POST api/<controller>
        [AcceptVerbs("OPTIONS", "POST")]
        [HttpPost]
        [Route("api/addHero")]
        public HttpResponseMessage Post(heroPayload value)
        {
            Dictionary<string, object> dict = new Dictionary<string, object>();
            dict.Add("id", Guid.NewGuid().ToString());
            dict.Add("name", value.heroItem.name);

            BsonDocument doc = new BsonDocument(dict);
            collection.InsertOne(doc);

            OurResponse response = new OurResponse();
            response.message = "";
            response.success = 200;
            response.data = "successful";

            var responseMsg = new HttpResponseMessage { StatusCode = HttpStatusCode.OK, Content = new StringContent(JsonConvert.SerializeObject(response)) };

            // Define and add values to variables: origins, headers, methods (can be global)               
            responseMsg.Headers.Add("Access-Control-Allow-Origin", "*");
            responseMsg.Headers.Add("Access-Control-Allow-Headers", "*");
            responseMsg.Headers.Add("Access-Control-Allow-Methods", "*");
            responseMsg.Headers.Add("Access-Control-Allow-Credentials", "true");

            return responseMsg;
        }

        // PUT api/<controller>/5
        [AcceptVerbs("OPTIONS", "POST")]
        [HttpPost]
        [Route("api/updateHero")]
        public HttpResponseMessage Put(heroPayload value)
        {
            var filter = Builders<BsonDocument>.Filter.Eq<string>("id", value.heroItem.id);
            var update = Builders<BsonDocument>.Update.Set("name", value.heroItem.name);

            var result = collection.UpdateOne(filter, update);

            OurResponse response = new OurResponse();
            response.message = "";
            response.success = 200;
            response.data = "successful";

            var responseMsg = new HttpResponseMessage { StatusCode = HttpStatusCode.OK, Content = new StringContent(JsonConvert.SerializeObject(response)) };

            // Define and add values to variables: origins, headers, methods (can be global)               
            responseMsg.Headers.Add("Access-Control-Allow-Origin", "*");
            responseMsg.Headers.Add("Access-Control-Allow-Headers", "*");
            responseMsg.Headers.Add("Access-Control-Allow-Methods", "*");
            responseMsg.Headers.Add("Access-Control-Allow-Credentials", "true");

            return responseMsg;
        }

        
    }
}
