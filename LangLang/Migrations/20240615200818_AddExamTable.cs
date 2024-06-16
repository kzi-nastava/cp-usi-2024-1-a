using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LangLang.Migrations
{
    public partial class AddExamTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Exams",
                columns: table => new
                {
                    Id = table.Column<string>(type: "text", nullable: false),
                    LanguageName = table.Column<string>(type: "text", nullable: false),
                    LanguageLevel = table.Column<int>(type: "integer", nullable: false),
                    Time = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    ClassroomNumber = table.Column<int>(type: "integer", nullable: false),
                    MaxStudents = table.Column<int>(type: "integer", nullable: false),
                    NumStudents = table.Column<int>(type: "integer", nullable: false),
                    ExamState = table.Column<int>(type: "integer", nullable: false),
                    TutorId = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Exams", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Exams_Languages_LanguageName",
                        column: x => x.LanguageName,
                        principalTable: "Languages",
                        principalColumn: "Name",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Exams_LanguageName",
                table: "Exams",
                column: "LanguageName");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Exams");
        }
    }
}
