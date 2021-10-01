using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace WA_EGroceryShop_v2.Web.Data.Migrations
{
    public partial class AddedProductCategoryPromotion2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ProductCategoryPromotions",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    Title = table.Column<string>(nullable: true),
                    Category = table.Column<int>(nullable: false),
                    DiscountPercentage = table.Column<double>(nullable: false),
                    EndDateOfPromotion = table.Column<DateTime>(nullable: false),
                    ImageName = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProductCategoryPromotions", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ProductCategoryPromotions");
        }
    }
}
