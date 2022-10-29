using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace _3TeamProject.Migrations
{
    public partial class UpdateActivitiesMessageState : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "ActivitiesMessageState",
                table: "ActivitiesMessageBoard",
                type: "int",
                unicode: false,
                maxLength: 10,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "varchar(10)",
                oldUnicode: false,
                oldMaxLength: 10);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "ActivitiesMessageState",
                table: "ActivitiesMessageBoard",
                type: "varchar(10)",
                unicode: false,
                maxLength: 10,
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int",
                oldUnicode: false,
                oldMaxLength: 10);
        }
    }
}
