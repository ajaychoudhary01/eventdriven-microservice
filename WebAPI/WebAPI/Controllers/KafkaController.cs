using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebAPI.Kafka;

namespace WebAPI.Controllers
{
    [Route("api/")]
    [ApiController]
    public class KafkaController : ControllerBase
    {
        private readonly IKafkaAdmin kafkaAdmin;
        public KafkaController(IKafkaAdmin kafkaAdmin)
        {
            this.kafkaAdmin = kafkaAdmin;
        }

        [HttpPost]
        [Route("topic")]
        public async Task<string> CreateTopic(string name, int numPartitions, short replicationFactor)
        {
            return await kafkaAdmin.CreateTopicAsync(name, numPartitions, replicationFactor);
        }
    }
}
