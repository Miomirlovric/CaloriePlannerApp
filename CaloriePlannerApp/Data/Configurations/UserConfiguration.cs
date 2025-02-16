using CaloriePlannerApp.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CaloriePlannerApp.Data.Configurations
{
    public class UserConfiguration : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.Property(a => a.Birth)
                .HasColumnName(nameof(User.Birth))
                .IsRequired();

            builder.Property(a => a.FirstName)
                .HasColumnName(nameof(User.FirstName))
                .IsRequired();

            builder.Property(a => a.LastName)
                .HasColumnName(nameof(User.LastName))
                .IsRequired();

            builder.Property(a => a.Height)
                .HasColumnName(nameof(User.Height))
                .IsRequired();

            builder.Property(a => a.Weight)
                .HasColumnName(nameof(User.Weight))
                .IsRequired();

            builder.Property(a => a.ActivityLevelEnum)
                .HasColumnName(nameof(User.ActivityLevelEnum))
                .IsRequired();

            builder.Property(a => a.ProgressTypeEnum)
                .HasColumnName(nameof(User.ProgressTypeEnum))
                .IsRequired();

            builder.Property(a => a.WeightGoalEnum)
                .HasColumnName(nameof(User.WeightGoalEnum))
                .IsRequired();

            builder.Property(a => a.CalorieGoal)
                .HasColumnName(nameof(User.CalorieGoal))
                .IsRequired();
        }
    }
}
