using Microsoft.EntityFrameworkCore.Migrations;

namespace LeavePlannerApp2.Migrations
{
    public partial class AddedNullableBoolValue : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_ApprovalANDRejections_LeaveApplicationId",
                table: "ApprovalANDRejections");

            migrationBuilder.AlterColumn<bool>(
                name: "IsApproved",
                table: "LeaveApplications",
                nullable: true,
                oldClrType: typeof(bool));

            migrationBuilder.CreateIndex(
                name: "IX_ApprovalANDRejections_LeaveApplicationId",
                table: "ApprovalANDRejections",
                column: "LeaveApplicationId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_ApprovalANDRejections_LeaveApplicationId",
                table: "ApprovalANDRejections");

            migrationBuilder.AlterColumn<bool>(
                name: "IsApproved",
                table: "LeaveApplications",
                nullable: false,
                oldClrType: typeof(bool),
                oldNullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_ApprovalANDRejections_LeaveApplicationId",
                table: "ApprovalANDRejections",
                column: "LeaveApplicationId",
                unique: true);
        }
    }
}
