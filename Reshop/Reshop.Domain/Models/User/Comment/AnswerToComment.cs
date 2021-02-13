using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Reshop.Domain.Models.User.Comment
{
    public class AnswerToComment
    {
        [Key]
        public int Id { get; set; }
        public int CommentId { get; set; }

        [Required]
        public string UserId { get; set; }
        [Required]
        public string FullName { get; set; }
        [Required]
        public string AnswerComment { get; set; }

        public string DateTime { get; set; }
        public int Like { get; set; }

        [ForeignKey("CommentId")]
        public CommentForProduct CommentForProduct { get; set; }
    }
}
