using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UserAuthProject.DTO;

namespace UserAuthProject.Repositories.Interfaces
{
    public interface ICommentDataRepository
    {
        Task<List<CommentDTO>> GetProductComments(Guid productId);
    }
}
