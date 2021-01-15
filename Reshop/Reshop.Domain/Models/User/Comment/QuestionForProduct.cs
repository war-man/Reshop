using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Reshop.Domain.Models.ProductAndCategory;

namespace Reshop.Domain.Models.User.Comment
{
    public class QuestionForProduct
    {
        [Key]
        public int Id { get; set; }

        public string UserId { get; set; }
        public int ProductId { get; set; }
        public string FullName { get; set; }
        public string QuestionText { get; set; }
        public int Like { get; set; }
        public string DateTime { get; set; }

        public Product Product { get; set; }
        public IList<AnswerToQuestion> AnswersToQuestion { get; set; }
    }
}
