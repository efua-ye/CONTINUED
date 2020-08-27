using Microsoft.EntityFrameworkCore.Migrations;

namespace StaffPortal.Migrations
{
    public partial class happiiers : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Departments_DeptCode_DeptName",
                table: "Departments");

            migrationBuilder.CreateIndex(
                name: "IX_Departments_DeptCode_DeptName",
                table: "Departments",
                columns: new[] { "DeptCode", "DeptName" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Departments_DeptCode_DeptName",
                table: "Departments");

            migrationBuilder.CreateIndex(
                name: "IX_Departments_DeptCode_DeptName",
                table: "Departments",
                columns: new[] { "DeptCode", "DeptName" },
                unique: true,
                filter: "[DeptCode] IS NOT NULL AND [DeptName] IS NOT NULL");
        }
    }
}
