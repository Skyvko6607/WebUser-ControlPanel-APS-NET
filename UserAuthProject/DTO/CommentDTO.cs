using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UserAuthProject.Models.Webshop;

namespace UserAuthProject.DTO
{
    public class CommentDTO
    {
        public QuestionData Question { get; set; }
        public List<AnswerData> AnswerData { get; set; }
    }
}
