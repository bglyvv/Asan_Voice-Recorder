using Microsoft.EntityFrameworkCore.Migrations;

namespace RecordVoice.Migrations
{
    public partial class New3 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "ApplicationOpened",
                table: "Records",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ApplicationOpened",
                table: "Records");
        }
    }
}
