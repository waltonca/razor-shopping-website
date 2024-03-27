using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Groceries.Migrations
{
    /// <inheritdoc />
    public partial class AddUserNameAndPasswordTest : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "UserName",
                table: "Grocery",
                type: "TEXT",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "UserName",
                table: "Grocery");
        }
    }
}
