using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace InsuranceWebApp.Data.Migrations
{
    /// <inheritdoc />
    public partial class InsuranceMigrationUpdate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "InsuranceType",
                table: "Insurance",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "InsuranceType",
                table: "Insurance");
        }
    }
}
