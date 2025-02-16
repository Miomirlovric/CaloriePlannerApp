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
    public class FoodItemConfiguration : IEntityTypeConfiguration<FoodItem>
    {
        public void Configure(EntityTypeBuilder<FoodItem> builder)
        {
            builder.HasKey(a => a.Id);

            builder.Property(a => a.Name)
                .HasColumnName(nameof(FoodItem.Name))
                .IsRequired();

            builder.Property(a => a.FoodGroup)
                .HasColumnName(nameof(FoodItem.FoodGroup))
                .IsRequired(false);

            builder.Property(a => a.Calories)
                .HasColumnName(nameof(FoodItem.Calories))
                .HasDefaultValue(0)
                .IsRequired();

            builder.Property(a => a.Fat)
                .HasColumnName(nameof(FoodItem.Fat))
                .HasDefaultValue(0)
                .IsRequired();

            builder.Property(a => a.Protein)
                .HasColumnName(nameof(FoodItem.Protein))
                .HasDefaultValue(0)
                .IsRequired();

            builder.Property(a => a.Carbohydrate)
                .HasColumnName(nameof(FoodItem.Carbohydrate))
                .HasDefaultValue(0)
                .IsRequired();

            builder.Property(a => a.Sugars)
                .HasColumnName(nameof(FoodItem.Sugars))
                .HasDefaultValue(0)
                .IsRequired();

            builder.Property(a => a.Fiber)
                .HasColumnName(nameof(FoodItem.Fiber))
                .HasDefaultValue(0)
                .IsRequired();

            builder.Property(a => a.IsCustomFood)
                .HasColumnName(nameof(FoodItem.IsCustomFood))
                .IsRequired();

            builder.Property(a => a.UserId)
                .HasColumnName(nameof(FoodItem.UserId))
                .IsRequired(false);

            builder.HasOne(a => a.CreatedBy)
                .WithMany(a => a.CustomFoods)
                .HasForeignKey(a => a.UserId)
                .OnDelete(DeleteBehavior.Cascade);
                
        }
    }
}
