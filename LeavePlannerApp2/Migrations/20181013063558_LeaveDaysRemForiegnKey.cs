using Microsoft.EntityFrameworkCore.Migrations;

namespace LeavePlannerApp2.Migrations
{
    public partial class LeaveDaysRemForiegnKey : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "LeaveAllocationDaysRemainingId",
                table: "ApprovalANDRejections",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_ApprovalANDRejections_LeaveAllocationDaysRemainingId",
                table: "ApprovalANDRejections",
                column: "LeaveAllocationDaysRemainingId");

            migrationBuilder.AddForeignKey(
                name: "FK_ApprovalANDRejections_DaysRemaining_LeaveAllocationDaysRemainingId",
                table: "ApprovalANDRejections",
                column: "LeaveAllocationDaysRemainingId",
                principalTable: "DaysRemaining",
                principalColumn: "LeaveAllocationDaysRemainingId",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ApprovalANDRejections_DaysRemaining_LeaveAllocationDaysRemainingId",
                table: "ApprovalANDRejections");

            migrationBuilder.DropIndex(
                name: "IX_ApprovalANDRejections_LeaveAllocationDaysRemainingId",
                table: "ApprovalANDRejections");

            migrationBuilder.DropColumn(
                name: "LeaveAllocationDaysRemainingId",
                table: "ApprovalANDRejections");
        }
    }
}
