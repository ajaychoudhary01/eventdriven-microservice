using Confluent.Kafka;
using Confluent.Kafka.Admin;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebAPI.Models;

namespace WebAPI.Kafka
{
    public class KafkaAdmin : IKafkaAdmin
    {
        private readonly IDictionary<string, string> config;
        private readonly ILogger<KafkaAdmin> logger;

        public KafkaAdmin(ILogger<KafkaAdmin> logger, IKafkaConfig config)
        {
            this.config = new Dictionary<string, string> {
                { "bootstrap.servers", config.BootstrapServers },
                { "sasl.mechanisms", "PLAIN" },
                { "security.protocol", "SASL_SSL" },
                { "sasl.username", config.UserName },
                { "sasl.password", config.Password }
            };
            this.logger = logger;
        }

        public async Task<string> CreateTopicAsync(string name, int numPartitions, short replicationFactor)
        {
            using (var adminClient = new AdminClientBuilder(config).Build())
            {
                try
                {
                    await adminClient.CreateTopicsAsync(new List<TopicSpecification> {
                        new TopicSpecification { Name = name, NumPartitions = numPartitions, ReplicationFactor = replicationFactor } });

                    return "Topic Created";
                }
                catch (CreateTopicsException e)
                {
                    if (e.Results[0].Error.Code != ErrorCode.TopicAlreadyExists)
                    {

                       logger.LogError($"An error occured creating topic {name}: {e.Results[0].Error.Reason}");
                        return "Failed to create topic";
                    }
                    else
                    {
                        logger.LogError("Topic already exists");
                        return "Topic already exists";
                    }
                }
            }
        }
    }
}
