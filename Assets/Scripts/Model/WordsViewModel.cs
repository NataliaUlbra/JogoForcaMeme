using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Scripts.Model
{
    public class WordsViewModel
    {
        public string WordsValue { get; set; }
        public int WordsId { get; set; }
        public int CategoryId { get; set; }

        public WordsViewModel()
        {
        }
        public WordsViewModel(string wordsValue, int categoryId)
        {
            WordsValue = wordsValue;
            CategoryId = categoryId;
        }
        public WordsViewModel(string wordsValue, int wordsId, int categoryId)
        {
            WordsValue = wordsValue;
            WordsId = wordsId;
            CategoryId = categoryId;
        }
    }
}
