using CaloriePlannerApp.Data.Model.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CaloriePlannerApp.Data.Entities
{
    public class User
    {
        [Key]
        public Guid Id { get; set; }
        public DateTime Birth { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public decimal Height { get; set; }
        public decimal Weight { get; set; }
        public ActivityLevelEnum ActivityLevelEnum { get; set; }
        public ProgressTypeEnum ProgressTypeEnum { get; set; }
        public WeightGoalEnum WeightGoalEnum { get; set; }
        public int CalorieGoal { get; set; }
        public IEnumerable<FoodItem> CustomFoods { get; set; }
        public IEnumerable<FoodUser> ConsumedFoods { get; set; }

        public string UserFullName()
        {
            return FirstName + " " + LastName;
        }
    }
}
