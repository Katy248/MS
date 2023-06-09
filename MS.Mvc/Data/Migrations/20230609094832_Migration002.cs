using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MS.Mvc.Data.Migrations
{
    /// <inheritdoc />
    public partial class Migration002 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "MeasurementUnit",
                table: "Products",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "MeasurementUnitAbbreviation",
                table: "Products",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "MeasurementUnit",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "MeasurementUnitAbbreviation",
                table: "Products");
        }
    }
}
