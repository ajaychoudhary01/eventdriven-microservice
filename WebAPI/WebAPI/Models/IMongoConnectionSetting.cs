using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebAPI.Models
{
    public interface IMongoConnectionSetting
    {
        public string ConnectionString { get; set; }
        public string DatabaseName { get; set; }
        public string ReviewCollectionName { get; set; }
    }

    public class MongoConnectionSetting : IMongoConnectionSetting
    {
        public string ConnectionString { get; set; }
        public string DatabaseName { get; set; }
        public string ReviewCollectionName { get; set; }
    }
}
