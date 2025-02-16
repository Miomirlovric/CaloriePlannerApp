using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CaloriePlannerApp.Data.Entities
{
    public class FoodItem
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string FoodGroup { get; set; }
        public int Calories { get; set; }
        public decimal Fat { get; set; }
        public decimal Protein { get; set; }
        public decimal Carbohydrate { get; set; }
        public decimal Sugars { get; set; }
        public decimal Fiber { get; set; }
        public bool IsCustomFood { get; set; }
        public User? CreatedBy { get; set; }
        public Guid? UserId { get; set; }
    }
}
