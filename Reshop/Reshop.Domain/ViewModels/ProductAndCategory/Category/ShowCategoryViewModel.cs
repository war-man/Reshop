﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Reshop.Domain.ViewModels.ProductAndCategory.Category
{
    public class ShowCategoryViewModel
    {
        public int CategoryId { get; set; }
        public string Name { get; set; }
        public int ProductCount { get; set; }
    }
}
