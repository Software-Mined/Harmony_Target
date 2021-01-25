using System;
using System.Linq;

namespace Mineware.Systems.Production.Planning.Models
{
    public class Questions
    {
        public int ID { get; set; }
        public string Question { get; set; }
        public string QuestionSubCat { get; set; }
        public decimal OrderBy { get; set; }
        public string ValueType { get; set; }
    }
}
