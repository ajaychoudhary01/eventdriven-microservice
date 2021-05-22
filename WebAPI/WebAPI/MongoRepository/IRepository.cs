using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebAPI.Models;

namespace WebAPI.MongoRepository
{
    public interface IRepository
    {
        Task<IEnumerable<Review>> GetReviews(int numberOfRecords);

        Task<bool> AddReviewAsync(string review);

    }
}
