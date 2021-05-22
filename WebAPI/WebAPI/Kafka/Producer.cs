using Confluent.Kafka;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebAPI.Models;

namespace WebAPI.Kafka
{
    public class Producer : IProducer
    {

        private readonly ProducerConfig config;
        private readonly ILogger<Producer> logger;
        private readonly string topic = "apiintegration";

        public Producer(IKafkaConfig config, ILogger<Producer> logger)
        {
            this.config = new ProducerConfig
            {
                BootstrapServers = config.BootstrapServers,
                SaslMechanism = SaslMechanism.Plain,
                SecurityProtocol = SecurityProtocol.SaslSsl,
                SaslUsername = config.UserName,
                SaslPassword = config.Password
            };
            this.logger = logger;
        }

        public async Task<bool> SendMessageToKafkaAsync(string message)
        {
            using (var producer = new ProducerBuilder<Null, string>(config).Build())
            {
                try
                {
                    var result = await producer.ProduceAsync(topic, new Message<Null, string> { Value = message });
                    return true;
                }
                catch (ProduceException<Null, string> e)
                {
                    logger.LogError(e.Error.Reason, e);
                }
            }
            return false;
        }
    }
}
