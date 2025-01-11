using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Auth.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class NewUserFieldsNameNickName : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Name",
                table: "AUTH_Users",
                type: "nvarchar(1023)",
                maxLength: 1023,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "NickName",
                table: "AUTH_Users",
                type: "nvarchar(1023)",
                maxLength: 1023,
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Name",
                table: "AUTH_Users");

            migrationBuilder.DropColumn(
                name: "NickName",
                table: "AUTH_Users");
        }
    }
}
