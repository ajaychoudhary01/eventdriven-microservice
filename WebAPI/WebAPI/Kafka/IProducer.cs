using System.Threading.Tasks;

namespace WebAPI.Kafka
{
    public interface IProducer
    {
        Task<bool> SendMessageToKafkaAsync(string message);
    }
}
