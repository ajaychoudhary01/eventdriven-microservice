using Confluent.Kafka;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebAPI.Models
{
    public class IKafkaConfig
    {
        public string UserName { get; set; }
        public string BootstrapServers { get; set; }
        public string Password { get; set; }
    }

    public class KafkaConfig : IKafkaConfig
    {
    }
}
