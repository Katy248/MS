using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MS.Mvc.Data.Migrations
{
    /// <inheritdoc />
    public partial class Migration003 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsEmployee",
                table: "AspNetUsers",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsEmployee",
                table: "AspNetUsers");
        }
    }
}
