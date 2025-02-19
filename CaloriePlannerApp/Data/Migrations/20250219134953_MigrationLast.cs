using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CaloriePlannerApp.Data.Migrations
{
    /// <inheritdoc />
    public partial class MigrationLast : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_FoodUser_Foods_FoodId",
                table: "FoodUser");

            migrationBuilder.DropForeignKey(
                name: "FK_FoodUser_Users_UserId",
                table: "FoodUser");

            migrationBuilder.DropPrimaryKey(
                name: "PK_FoodUser",
                table: "FoodUser");

            migrationBuilder.RenameTable(
                name: "FoodUser",
                newName: "FoodUsers");

            migrationBuilder.RenameIndex(
                name: "IX_FoodUser_UserId",
                table: "FoodUsers",
                newName: "IX_FoodUsers_UserId");

            migrationBuilder.RenameIndex(
                name: "IX_FoodUser_FoodId",
                table: "FoodUsers",
                newName: "IX_FoodUsers_FoodId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_FoodUsers",
                table: "FoodUsers",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_FoodUsers_Foods_FoodId",
                table: "FoodUsers",
                column: "FoodId",
                principalTable: "Foods",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_FoodUsers_Users_UserId",
                table: "FoodUsers",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_FoodUsers_Foods_FoodId",
                table: "FoodUsers");

            migrationBuilder.DropForeignKey(
                name: "FK_FoodUsers_Users_UserId",
                table: "FoodUsers");

            migrationBuilder.DropPrimaryKey(
                name: "PK_FoodUsers",
                table: "FoodUsers");

            migrationBuilder.RenameTable(
                name: "FoodUsers",
                newName: "FoodUser");

            migrationBuilder.RenameIndex(
                name: "IX_FoodUsers_UserId",
                table: "FoodUser",
                newName: "IX_FoodUser_UserId");

            migrationBuilder.RenameIndex(
                name: "IX_FoodUsers_FoodId",
                table: "FoodUser",
                newName: "IX_FoodUser_FoodId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_FoodUser",
                table: "FoodUser",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_FoodUser_Foods_FoodId",
                table: "FoodUser",
                column: "FoodId",
                principalTable: "Foods",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_FoodUser_Users_UserId",
                table: "FoodUser",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
