using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UserAuthProject.DTO;
using UserAuthProject.Models.Webshop;

namespace UserAuthProject.Repositories.Interfaces
{
    public interface IReviewDataRepository
    {
        Task<ReviewDTO> GetProductReview(Guid productId);
        Task<List<ReviewData>> GetProductReviews(Guid productId);
    }
}
