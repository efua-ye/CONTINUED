using Microsoft.EntityFrameworkCore.Migrations;

namespace StaffPortal.Migrations
{
    public partial class happiier : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Year",
                table: "Salaries",
                nullable: true,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Month",
                table: "Salaries",
                nullable: true,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Salaries_Month_Year",
                table: "Salaries",
                columns: new[] { "Month", "Year" },
                unique: true,
                filter: "[Month] IS NOT NULL AND [Year] IS NOT NULL");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Salaries_Month_Year",
                table: "Salaries");

            migrationBuilder.AlterColumn<string>(
                name: "Year",
                table: "Salaries",
                nullable: true,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Month",
                table: "Salaries",
                nullable: true,
                oldClrType: typeof(string),
                oldNullable: true);
        }
    }
}
