using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using WebAPI.Kafka;
using WebAPI.Models;
using WebAPI.MongoRepository;

namespace WebAPI.Controllers
{
    [Route("api")]
    [ApiController]
    public class ReviewController : ControllerBase
    {
        private readonly IRepository repository;

        private readonly IProducer producer;

        public ReviewController(IRepository repository, IProducer producer)
        {
            this.repository = repository;
            this.producer = producer;
        }

        [HttpGet]
        [Route("reviews")]
        public async Task<IEnumerable<Review>> GetRestaurants([FromQuery] int numberOfRecords = 100)
        {
            return await repository.GetReviews(numberOfRecords);
        }

        [HttpPost]
        [Route("review")]
        public async Task<bool> AddReviewAsync(string review)
        {
            return await producer.SendMessageToKafkaAsync(review);
        }
    }
}
