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
    public class CommentDataRepository : ICommentDataRepository
    {
        private GlobalDbContext GlobalDbContext { get; set; }

        public CommentDataRepository(GlobalDbContext globalDbContext)
        {
            this.GlobalDbContext = globalDbContext;
        }

        public async Task<List<CommentDTO>> GetProductComments(Guid productId)
        {
            var questions = await GlobalDbContext.Questions.Include(data => data.User).Include(data => data.Product)
                .Where(data => data.ProductId == productId).ToListAsync();
            var answers = await GlobalDbContext.Answers.Include(data => data.User).Include(data => data.Product)
                .Where(data => data.ProductId == productId).ToListAsync();
            var comments = new List<CommentDTO>();
            questions.ForEach(data => comments.Add(new CommentDTO
            {
                Question = data,
                AnswerData = answers.Where(answerData => answerData.QuestionDataId == data.Id).ToList()
            }));
            return comments;
        }
    }
}