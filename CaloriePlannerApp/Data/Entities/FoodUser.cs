using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CaloriePlannerApp.Data.Entities
{
    public class FoodUser
    {
        public Guid Id { get; set; }
        public DateTime At { get; set; }
        public int Quantity { get; set; }
        public Guid FoodId { get; set; }
        public FoodItem Food { get; set; }
        public Guid UserId { get; set; }
        public User User { get; set; }

    }
}
