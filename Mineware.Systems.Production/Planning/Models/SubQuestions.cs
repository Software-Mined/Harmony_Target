using System;
using System.Linq;

namespace Mineware.Systems.Production.Planning.Models
{
    public class SubQuestions
    {
        public int ID { get; set; }
        public string Question { get; set; }
        public int Likelihood { get; set; }
        public int Consequence { get; set; }
        public int RiskRating { get; set; }

    }
}
