using Confluent.Kafka;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Threading;
using System.Threading.Tasks;
using WebAPI.Models;
using WebAPI.MongoRepository;

namespace WebAPI.Kafka
{
    public class Consumer : BackgroundService
    {
        private readonly ConsumerConfig config;
        private readonly IRepository repository;
        private readonly ILogger<Consumer> logger;
        private readonly string topic = "apiintegration";

        public Consumer(IKafkaConfig config, IRepository repository, ILogger<Consumer> logger)
        {
            this.config = new ConsumerConfig
            {
                BootstrapServers = config.BootstrapServers,
                GroupId = "webapi-integration",
                SaslMechanism = SaslMechanism.Plain,
                SecurityProtocol = SecurityProtocol.SaslSsl,
                SaslUsername = config.UserName,
                SaslPassword = config.Password,
                AutoOffsetReset = AutoOffsetReset.Earliest
            };

            this.repository = repository;
            this.logger = logger;
        }

        protected override async Task ExecuteAsync(CancellationToken cancellationToken)
        {
            using (var builder = new ConsumerBuilder<Null, string>(config).Build())
            {
                builder.Subscribe(topic);
                try
                {
                    await Task.Run(async () =>
                    {
                        while (!cancellationToken.IsCancellationRequested)
                        {
                            var consumer = builder.Consume(cancellationToken);
                            if (!await repository.AddReviewAsync(consumer.Message.Value))
                                logger.LogError("Message was not inserted");
                            else
                                logger.LogInformation("Message Processed");

                        }
                    }, cancellationToken);
                }
                catch (Exception ex)
                {
                    logger.LogError(ex.Message, ex);
                    builder.Close();
                }
            }
        }
    }
}
