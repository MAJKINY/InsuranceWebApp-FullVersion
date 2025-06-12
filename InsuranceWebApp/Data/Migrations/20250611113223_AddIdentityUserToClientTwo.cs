using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace InsuranceWebApp.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddIdentityUserToClientTwo : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Clients_IdentityUserId",
                table: "Clients");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Clients_IdentityUserId",
                table: "Clients");

            migrationBuilder.CreateIndex(
                name: "IX_Clients_IdentityUserId",
                table: "Clients",
                column: "IdentityUserId");
        }
    }
}
