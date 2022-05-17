using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Scripts.Model
{
    public class CategoryViewModel
    {
        public int Id { get; set; }
        public string CategoryValue { get; set; }

        public CategoryViewModel()
        {
        }

        public CategoryViewModel(int id, string categoryValue)
        {
            Id = id;
            CategoryValue = categoryValue;
        }
    }
}
