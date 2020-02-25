using Microsoft.EntityFrameworkCore.Migrations;

namespace Multitenant.Migrations.MasterDb
{
    public partial class AuthOptions : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "AuthenticationSettingsId",
                table: "Tenants",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "AuthenticationSettings",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ValidateIssuer = table.Column<bool>(nullable: false),
                    ValidIssuer = table.Column<string>(nullable: true),
                    ValidateAudience = table.Column<bool>(nullable: false),
                    ValidAudience = table.Column<string>(nullable: true),
                    ValidateLifetime = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AuthenticationSettings", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Tenants_AuthenticationSettingsId",
                table: "Tenants",
                column: "AuthenticationSettingsId");

            migrationBuilder.AddForeignKey(
                name: "FK_Tenants_AuthenticationSettings_AuthenticationSettingsId",
                table: "Tenants",
                column: "AuthenticationSettingsId",
                principalTable: "AuthenticationSettings",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Tenants_AuthenticationSettings_AuthenticationSettingsId",
                table: "Tenants");

            migrationBuilder.DropTable(
                name: "AuthenticationSettings");

            migrationBuilder.DropIndex(
                name: "IX_Tenants_AuthenticationSettingsId",
                table: "Tenants");

            migrationBuilder.DropColumn(
                name: "AuthenticationSettingsId",
                table: "Tenants");
        }
    }
}
