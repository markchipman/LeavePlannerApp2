using Microsoft.EntityFrameworkCore.Migrations;

namespace LeavePlannerApp2.Migrations
{
    public partial class isReadNotification : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "hasread",
                table: "LeaveApplications",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "hasread",
                table: "LeaveApplications");
        }
    }
}
