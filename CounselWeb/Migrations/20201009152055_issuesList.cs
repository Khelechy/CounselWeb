using Microsoft.EntityFrameworkCore.Migrations;

namespace CounselWeb.Migrations
{
    public partial class issuesList : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Issues",
                columns: new[] { "Id", "Name" },
                values: new object[,]
                {
                    { 1, "Smoking" },
                    { 2, "Career and course choices" },
                    { 3, "Project choice" },
                    { 4, "Depression and suicidal thoughts" },
                    { 5, "Constant failing of courses" },
                    { 6, "Prostitution" }
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Issues",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Issues",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Issues",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "Issues",
                keyColumn: "Id",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "Issues",
                keyColumn: "Id",
                keyValue: 5);

            migrationBuilder.DeleteData(
                table: "Issues",
                keyColumn: "Id",
                keyValue: 6);
        }
    }
}
