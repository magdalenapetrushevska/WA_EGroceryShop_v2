using Microsoft.EntityFrameworkCore.Migrations;

namespace WA_EGroceryShop_v2.Web.Data.Migrations
{
    public partial class AddedNumOfReviews : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "NumOfReviews",
                table: "Products",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "NumOfReviews",
                table: "Products");
        }
    }
}
