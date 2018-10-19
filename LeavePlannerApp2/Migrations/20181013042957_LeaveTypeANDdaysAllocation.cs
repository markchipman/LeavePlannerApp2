using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace LeavePlannerApp2.Migrations
{
    public partial class LeaveTypeANDdaysAllocation : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "LeaveType",
                table: "LeaveApplications");

            migrationBuilder.AlterColumn<string>(
                name: "Responsibilities",
                table: "LeaveApplications",
                nullable: false,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "ReasonForLeave",
                table: "LeaveApplications",
                nullable: false,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.CreateTable(
                name: "DaysRemaining",
                columns: table => new
                {
                    LeaveAllocationDaysRemainingId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    EmployeeId = table.Column<string>(nullable: true),
                    DaysRemaining = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DaysRemaining", x => x.LeaveAllocationDaysRemainingId);
                    table.ForeignKey(
                        name: "FK_DaysRemaining_AspNetUsers_EmployeeId",
                        column: x => x.EmployeeId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "LeaveType",
                columns: table => new
                {
                    LeaveTypeId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    LeaveTypeName = table.Column<string>(nullable: true),
                    LeaveTypeDays = table.Column<int>(nullable: false),
                    LeaveApplicationId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LeaveType", x => x.LeaveTypeId);
                    table.ForeignKey(
                        name: "FK_LeaveType_LeaveApplications_LeaveApplicationId",
                        column: x => x.LeaveApplicationId,
                        principalTable: "LeaveApplications",
                        principalColumn: "LeaveApplicationId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_DaysRemaining_EmployeeId",
                table: "DaysRemaining",
                column: "EmployeeId");

            migrationBuilder.CreateIndex(
                name: "IX_LeaveType_LeaveApplicationId",
                table: "LeaveType",
                column: "LeaveApplicationId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "DaysRemaining");

            migrationBuilder.DropTable(
                name: "LeaveType");

            migrationBuilder.AlterColumn<string>(
                name: "Responsibilities",
                table: "LeaveApplications",
                nullable: true,
                oldClrType: typeof(string));

            migrationBuilder.AlterColumn<string>(
                name: "ReasonForLeave",
                table: "LeaveApplications",
                nullable: true,
                oldClrType: typeof(string));

            migrationBuilder.AddColumn<int>(
                name: "LeaveType",
                table: "LeaveApplications",
                nullable: false,
                defaultValue: 0);
        }
    }
}
