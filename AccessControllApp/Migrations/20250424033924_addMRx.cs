using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AccessControllApp.Migrations
{
    /// <inheritdoc />
    public partial class addMRx : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Mrz",
                table: "Users",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Mrz",
                table: "Users");
        }
    }
}
