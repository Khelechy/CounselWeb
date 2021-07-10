using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace CounselWeb.Migrations
{
    public partial class newMig : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Issues",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Issues", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Messages",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RequestId = table.Column<int>(nullable: false),
                    MessageBody = table.Column<string>(nullable: true),
                    created_at = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Messages", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Requests",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<int>(nullable: false),
                    FullName = table.Column<string>(nullable: false),
                    Email = table.Column<string>(nullable: false),
                    MatricNo = table.Column<string>(nullable: false),
                    Problem = table.Column<string>(nullable: false),
                    Department = table.Column<string>(nullable: false),
                    Status = table.Column<int>(nullable: false),
                    AdminId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Requests", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FirstName = table.Column<string>(nullable: false),
                    LastName = table.Column<string>(nullable: false),
                    MatricNo = table.Column<string>(nullable: true),
                    Password = table.Column<string>(nullable: false),
                    Department = table.Column<string>(nullable: true),
                    Email = table.Column<string>(nullable: false),
                    IsAdmin = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                });

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

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "Department", "Email", "FirstName", "IsAdmin", "LastName", "MatricNo", "Password" },
                values: new object[] { 1, null, "admin@counsel.com", "Admin", true, "SuperAdmin", null, "admin123" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Issues");

            migrationBuilder.DropTable(
                name: "Messages");

            migrationBuilder.DropTable(
                name: "Requests");

            migrationBuilder.DropTable(
                name: "Users");
        }
    }
}
