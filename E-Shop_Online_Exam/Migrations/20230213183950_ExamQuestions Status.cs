using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EShopOnlineExam.Migrations
{
    public partial class ExamQuestionsStatus : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "status",
                table: "ExamQuestions",
                type: "bit",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "status",
                table: "ExamQuestions");
        }
    }
}
