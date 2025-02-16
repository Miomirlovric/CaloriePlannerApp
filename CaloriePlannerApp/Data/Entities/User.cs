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
    }
}
