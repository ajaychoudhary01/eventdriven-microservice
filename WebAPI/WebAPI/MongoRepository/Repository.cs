using Microsoft.Extensions.Logging;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebAPI.Models;

namespace WebAPI.MongoRepository
{
    public class Repository : IRepository
    {
        private readonly ILogger<Repository> logger;

        private readonly IMongoCollection<Review> reviews;

        public Repository(IMongoConnectionSetting mongoConnectionSetting, ILogger<Repository> logger)
        {
            var mongoClient = new MongoClient(mongoConnectionSetting.ConnectionString);
            IMongoDatabase mongoDatabase = mongoClient.GetDatabase(mongoConnectionSetting.DatabaseName);

            reviews = mongoDatabase.GetCollection<Review>(mongoConnectionSetting.ReviewCollectionName);

            this.logger = logger;
        }

        public async Task<IEnumerable<Review>> GetReviews(int numberOfRecords)
        {
            return await reviews.Find(s => true).Limit(numberOfRecords).ToListAsync();
        }

        public async Task<bool> AddReviewAsync(string review)
        {
            try
            {
                await reviews.InsertOneAsync(new Review { Content = review });
                return true;
            }
            catch(Exception ex)
            {
                logger.LogError(ex.Message, ex);
                return false;
            }
        }
    }
}
