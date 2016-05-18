using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Slack.Models.CanteenModels
{
    public class Canteen
    {
        public string Name { get; set; }
        public List<MealGroup> MealGroups { get; set; }
        public string Status { get; set; }
        public DateTime Date { get; set; }
    }
}
