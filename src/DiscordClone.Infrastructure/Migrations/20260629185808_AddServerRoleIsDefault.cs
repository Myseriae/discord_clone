using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DiscordClone.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddServerRoleIsDefault : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_ServerRoles_ServerId",
                table: "ServerRoles");

            migrationBuilder.AddColumn<bool>(
                name: "IsDefault",
                table: "ServerRoles",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.CreateIndex(
                name: "IX_ServerRoles_ServerId",
                table: "ServerRoles",
                column: "ServerId",
                unique: true,
                filter: "\"IsDefault\"");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_ServerRoles_ServerId",
                table: "ServerRoles");

            migrationBuilder.DropColumn(
                name: "IsDefault",
                table: "ServerRoles");

            migrationBuilder.CreateIndex(
                name: "IX_ServerRoles_ServerId",
                table: "ServerRoles",
                column: "ServerId");
        }
    }
}
