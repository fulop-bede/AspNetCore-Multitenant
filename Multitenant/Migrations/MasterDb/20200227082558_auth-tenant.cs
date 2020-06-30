using Microsoft.EntityFrameworkCore.Migrations;

namespace Multitenant.Migrations.MasterDb
{
    public partial class authtenant : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ValidAudience",
                table: "AuthenticationSettings");

            migrationBuilder.DropColumn(
                name: "ValidIssuer",
                table: "AuthenticationSettings");

            migrationBuilder.DropColumn(
                name: "ValidateAudience",
                table: "AuthenticationSettings");

            migrationBuilder.DropColumn(
                name: "ValidateIssuer",
                table: "AuthenticationSettings");

            migrationBuilder.DropColumn(
                name: "ValidateLifetime",
                table: "AuthenticationSettings");

            migrationBuilder.AddColumn<string>(
                name: "AuthTenantId",
                table: "AuthenticationSettings",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AuthTenantId",
                table: "AuthenticationSettings");

            migrationBuilder.AddColumn<string>(
                name: "ValidAudience",
                table: "AuthenticationSettings",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ValidIssuer",
                table: "AuthenticationSettings",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "ValidateAudience",
                table: "AuthenticationSettings",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "ValidateIssuer",
                table: "AuthenticationSettings",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "ValidateLifetime",
                table: "AuthenticationSettings",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }
    }
}
