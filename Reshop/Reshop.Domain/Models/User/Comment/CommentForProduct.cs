﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Reshop.Domain.Models.ProductAndCategory;

namespace Reshop.Domain.Models.User.Comment
{
    public class CommentForProduct
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string UserId { get; set; }
        [Required]
        public string FullName { get; set; }
        [Required]
        public string Comment { get; set; }
        [Required]
        public int ProductId { get; set; }

        [Required]
        public string DateTime { get; set; }

        public int Like { get; set; }

        [ForeignKey("ProductId")]
        public Product Product { get; set; }

        public ICollection<AnswerToComment> AnswersToComment { get; set; }
    }
}