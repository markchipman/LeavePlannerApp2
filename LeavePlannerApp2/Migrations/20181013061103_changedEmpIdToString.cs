using Microsoft.EntityFrameworkCore.Migrations;

namespace LeavePlannerApp2.Migrations
{
    public partial class changedEmpIdToString : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_DaysRemaining_AspNetUsers_EmployeeId",
                table: "DaysRemaining");

            migrationBuilder.DropIndex(
                name: "IX_DaysRemaining_EmployeeId",
                table: "DaysRemaining");

            migrationBuilder.RenameColumn(
                name: "EmployeeId",
                table: "DaysRemaining",
                newName: "Employee");

            migrationBuilder.AlterColumn<string>(
                name: "Employee",
                table: "DaysRemaining",
                nullable: true,
                oldClrType: typeof(string),
                oldNullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Employee",
                table: "DaysRemaining",
                newName: "EmployeeId");

            migrationBuilder.AlterColumn<string>(
                name: "EmployeeId",
                table: "DaysRemaining",
                nullable: true,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_DaysRemaining_EmployeeId",
                table: "DaysRemaining",
                column: "EmployeeId");

            migrationBuilder.AddForeignKey(
                name: "FK_DaysRemaining_AspNetUsers_EmployeeId",
                table: "DaysRemaining",
                column: "EmployeeId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
