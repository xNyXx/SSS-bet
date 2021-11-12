using System.Collections.Generic;
using BLL.MongoSettings;
using DAL.Models;
using MongoDB.Driver;

namespace BLL.Services
{
    public class MongoService
    {
        private readonly IMongoCollection<Ads> _ads;

        public MongoService(IMongoDatabaseSettings settings)
        {
            var client = new MongoClient(settings.ConnectionString);
            var database = client.GetDatabase(settings.DatabaseName);

            _ads = database.GetCollection<Ads>(settings.CollectionName);
        }

        public List<Ads> Get() =>
            _ads.Find(a => true).ToList();

        public Ads Get(string id) =>
            _ads.Find<Ads>(a => a.Id == id).FirstOrDefault();

        public Ads Create(Ads ads)
        {
            _ads.InsertOne(ads);
            return ads;
        }

        public void Update(string id, Ads ads) =>
            _ads.ReplaceOne(ad => ad.Id == id, ads);


        public void Remove(string id) =>
            _ads.DeleteOne(book => book.Id == id);
    }
}
