using Microsoft.EntityFrameworkCore.Migrations;

namespace CounselWeb.Migrations
{
    public partial class seeded : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "Department", "Email", "FirstName", "IsAdmin", "LastName", "MatricNo", "Password" },
                values: new object[] { 1, null, "admin@counsel.com", "Admin", true, "SuperAdmin", null, "admin123" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1);
        }
    }
}
