using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Management.Migrations
{
    /// <inheritdoc />
    public partial class CitytoCityIdForeignKey : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "City",
                table: "Employees");

            migrationBuilder.RenameColumn(
                name: "EmployeeName",
                table: "Employees",
                newName: "Name");

            migrationBuilder.RenameColumn(
                name: "EmployeeID",
                table: "Employees",
                newName: "Id");

            migrationBuilder.RenameColumn(
                name: "DepartmentName",
                table: "Departments",
                newName: "Name");

            migrationBuilder.RenameColumn(
                name: "DepartmentId",
                table: "Departments",
                newName: "Id");

            migrationBuilder.AddColumn<int>(
                name: "CityId",
                table: "Employees",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Employees_CityId",
                table: "Employees",
                column: "CityId");

            migrationBuilder.AddForeignKey(
                name: "FK_Employees_Cities_CityId",
                table: "Employees",
                column: "CityId",
                principalTable: "Cities",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Employees_Cities_CityId",
                table: "Employees");

            migrationBuilder.DropIndex(
                name: "IX_Employees_CityId",
                table: "Employees");

            migrationBuilder.DropColumn(
                name: "CityId",
                table: "Employees");

            migrationBuilder.RenameColumn(
                name: "Name",
                table: "Employees",
                newName: "EmployeeName");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "Employees",
                newName: "EmployeeID");

            migrationBuilder.RenameColumn(
                name: "Name",
                table: "Departments",
                newName: "DepartmentName");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "Departments",
                newName: "DepartmentId");

            migrationBuilder.AddColumn<string>(
                name: "City",
                table: "Employees",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }
    }
}
