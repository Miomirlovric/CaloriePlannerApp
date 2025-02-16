using CaloriePlannerApp.Data.Entities;
using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CaloriePlannerApp.Data.Helper
{
    public static class ExcelHelper
    {
        public static List<FoodItem> ReadFoodData(string filePath)
        {
            var foodItems = new List<FoodItem>();

            ExcelPackage.LicenseContext = LicenseContext.NonCommercial; 

            using (var package = new ExcelPackage(new FileInfo(filePath)))
            {
                var worksheet = package.Workbook.Worksheets[0]; 

                int headerRow = 4; 
                int totalRows = worksheet.Dimension.Rows;
                int totalCols = worksheet.Dimension.Columns;

                var columnIndexes = new Dictionary<string, int>();

                for (int col = 1; col <= totalCols; col++)
                {
                    string columnName = worksheet.Cells[headerRow, col].Text.Trim();
                    columnIndexes[columnName] = col;
                }

                for (int row = headerRow + 1; row <= totalRows; row++) 
                {
                    var foodItem = new FoodItem
                    {
                        Id = Guid.NewGuid(),
                        Name = worksheet.Cells[row, columnIndexes["Name"]].Text,
                        FoodGroup = worksheet.Cells[row, columnIndexes["Food Group"]].Text,
                        Calories = int.TryParse(worksheet.Cells[row, columnIndexes["Calories"]].Text, out int cals) ? cals : 0,
                        Fat = decimal.TryParse(worksheet.Cells[row, columnIndexes["Fat (g)"]].Text, out decimal fat) ? fat : 0,
                        Protein = decimal.TryParse(worksheet.Cells[row, columnIndexes["Protein (g)"]].Text, out decimal protein) ? protein : 0,
                        Carbohydrate = decimal.TryParse(worksheet.Cells[row, columnIndexes["Carbohydrate (g)"]].Text, out decimal carb) ? carb : 0,
                        Sugars = decimal.TryParse(worksheet.Cells[row, columnIndexes["Sugars (g)"]].Text, out decimal sugar) ? sugar : 0,
                        Fiber = decimal.TryParse(worksheet.Cells[row, columnIndexes["Fiber (g)"]].Text, out decimal fiber) ? fiber : 0,
                    };
                    foodItems.Add(foodItem);
                }
            }

            return foodItems;
        }
    }
}
