using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LangLang.Migrations
{
    public partial class Mig1 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Courses",
                columns: table => new
                {
                    Id = table.Column<string>(type: "text", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: false),
                    LanguageName = table.Column<string>(type: "text", nullable: false),
                    LanguageCode = table.Column<string>(type: "text", nullable: false),
                    Level = table.Column<int>(type: "integer", nullable: false),
                    Duration = table.Column<int>(type: "integer", nullable: false),
                    Schedule = table.Column<string>(type: "text", nullable: false),
                    ScheduleSerialized = table.Column<string>(type: "text", nullable: false),
                    Start = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    Online = table.Column<bool>(type: "boolean", nullable: false),
                    MaxStudents = table.Column<int>(type: "integer", nullable: false),
                    NumStudents = table.Column<int>(type: "integer", nullable: false),
                    State = table.Column<int>(type: "integer", nullable: false),
                    TutorId = table.Column<string>(type: "text", nullable: true),
                    IsCreatedByTutor = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Courses", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Courses");
        }
    }
}
