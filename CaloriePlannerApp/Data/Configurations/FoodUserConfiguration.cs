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
    public class FoodUserConfiguration : IEntityTypeConfiguration<FoodUser>
    {
        public void Configure(EntityTypeBuilder<FoodUser> builder)
        {
            builder.HasKey(a => a.Id);

            builder.Property(a => a.At)
                .HasColumnName(nameof(FoodUser.At))
                .IsRequired();

            builder.Property(a => a.Quantity)
                .HasColumnName(nameof(FoodUser.Quantity))
                .IsRequired();

            builder.Property(a => a.FoodId)
                .HasColumnName(nameof(FoodUser.FoodId))
                .IsRequired();

            builder.Property(a => a.UserId)
                .HasColumnName(nameof(FoodUser.UserId))
                .IsRequired();

            builder.HasOne(a => a.User)
                .WithMany(a => a.ConsumedFoods)
                .HasForeignKey(a => a.UserId)
                .OnDelete(DeleteBehavior.Cascade);
            
            builder.HasOne(a => a.Food)
                .WithMany()
                .HasForeignKey(a => a.FoodId)
                .OnDelete(DeleteBehavior.Cascade);

        }
    }
}
