using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using UserAuthProject.DTO;
using UserAuthProject.Models.DbContexts;
using UserAuthProject.Models.Webshop;
using UserAuthProject.Repositories.Interfaces;

namespace UserAuthProject.Repositories
{
    public class ReviewDataRepository : IReviewDataRepository
    {
        private GlobalDbContext GlobalDbContext { get; set; }

        public ReviewDataRepository(GlobalDbContext globalDbContext)
        {
            this.GlobalDbContext = globalDbContext;
        }

        public async Task<ReviewDTO> GetProductReview(Guid productId)
        {
            var data = await GlobalDbContext.Reviews.Include(review => review.User).Include(review => review.Product)
                .Where(reviewData => reviewData.ProductId == productId).ToListAsync();
            if (data.Count == 0)
            {
                return null;
            }

            var sum = data.Sum(reviewData => reviewData.Score);
            return new ReviewDTO()
            {
                AvgScore = sum / (data.Count * 1D),
                ReviewCount = data.Count
            };
        }

        public async Task<List<ReviewData>> GetProductReviews(Guid productId)
        {
            return await GlobalDbContext.Reviews.Include(review => review.User).Include(review => review.Product)
                .Where(reviewData => reviewData.ProductId == productId).ToListAsync();
        }
    }
}