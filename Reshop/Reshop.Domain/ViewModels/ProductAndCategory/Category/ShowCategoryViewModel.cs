﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Reshop.Domain.Models.ProductAndCategory;

namespace Reshop.Domain.ViewModels.ProductAndCategory.Category
{
    public class ShowCategoryViewModel
    {
        public int CategoryId { get; set; }
        public string CategoryName { get; set; }

        public IEnumerable<ChildCategory> ChildCategories { get; set; }
    }
}
