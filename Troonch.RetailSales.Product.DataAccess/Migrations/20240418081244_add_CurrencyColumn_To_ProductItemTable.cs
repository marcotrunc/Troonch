using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Troonch.RetailSales.Product.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class addCurrencyColumnToProductItemTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Currency",
                table: "ProductItems",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Currency",
                table: "ProductItems");
        }
    }
}
