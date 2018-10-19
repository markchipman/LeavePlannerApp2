using Microsoft.EntityFrameworkCore.Migrations;

namespace LeavePlannerApp2.Migrations
{
    public partial class ApprovalANDRejections : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_ApprovalANDRejections_LeaveApplicationId",
                table: "ApprovalANDRejections");

            migrationBuilder.CreateIndex(
                name: "IX_ApprovalANDRejections_LeaveApplicationId",
                table: "ApprovalANDRejections",
                column: "LeaveApplicationId",
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_ApprovalANDRejections_LeaveApplicationId",
                table: "ApprovalANDRejections");

            migrationBuilder.CreateIndex(
                name: "IX_ApprovalANDRejections_LeaveApplicationId",
                table: "ApprovalANDRejections",
                column: "LeaveApplicationId");
        }
    }
}
