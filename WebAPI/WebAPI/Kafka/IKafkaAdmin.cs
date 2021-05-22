using System.Threading.Tasks;

namespace WebAPI.Kafka
{
    public interface IKafkaAdmin
    {
        Task<string> CreateTopicAsync(string name, int numPartitions, short replicationFactor);
    }
}
