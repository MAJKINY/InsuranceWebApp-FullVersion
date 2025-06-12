using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace InsuranceWebApp.Data.Migrations
{
    /// <inheritdoc />
    public partial class PolicyholderToClientMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Insurance_Policyholder_ClientId",
                table: "Insurance");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Policyholder",
                table: "Policyholder");

            migrationBuilder.RenameTable(
                name: "Policyholder",
                newName: "Clients");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Clients",
                table: "Clients",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Insurance_Clients_ClientId",
                table: "Insurance",
                column: "ClientId",
                principalTable: "Clients",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Insurance_Clients_ClientId",
                table: "Insurance");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Clients",
                table: "Clients");

            migrationBuilder.RenameTable(
                name: "Clients",
                newName: "Policyholder");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Policyholder",
                table: "Policyholder",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Insurance_Policyholder_ClientId",
                table: "Insurance",
                column: "ClientId",
                principalTable: "Policyholder",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
