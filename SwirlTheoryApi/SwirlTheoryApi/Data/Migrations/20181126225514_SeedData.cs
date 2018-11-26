using Microsoft.EntityFrameworkCore.Migrations;

namespace SwirlTheoryApi.Migrations
{
    public partial class SeedData : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Products",
                columns: new[] { "ProductId", "Cost", "ImageUrl", "ProductDescription", "ProductTitle" },
                values: new object[] { 1, 200f, "https://preview.redd.it/ogmlb210je021.jpg?width=960&crop=smart&auto=webp&s=9e7ac8f5267e3f6dfea4bddf027c552207308038", "A totally RAD shirt", "Test Product 1" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "ProductId",
                keyValue: 1);
        }
    }
}
