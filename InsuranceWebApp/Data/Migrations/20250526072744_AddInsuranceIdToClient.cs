using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace InsuranceWebApp.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddInsuranceIdToClient : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "PolicyholderId",
                table: "Insurance",
                newName: "ClientId");

            migrationBuilder.AddColumn<int>(
                name: "InsuranceId",
                table: "Policyholder",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Insurance_ClientId",
                table: "Insurance",
                column: "ClientId");

            migrationBuilder.AddForeignKey(
                name: "FK_Insurance_Policyholder_ClientId",
                table: "Insurance",
                column: "ClientId",
                principalTable: "Policyholder",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Insurance_Policyholder_ClientId",
                table: "Insurance");

            migrationBuilder.DropIndex(
                name: "IX_Insurance_ClientId",
                table: "Insurance");

            migrationBuilder.DropColumn(
                name: "InsuranceId",
                table: "Policyholder");

            migrationBuilder.RenameColumn(
                name: "ClientId",
                table: "Insurance",
                newName: "PolicyholderId");
        }
    }
}
