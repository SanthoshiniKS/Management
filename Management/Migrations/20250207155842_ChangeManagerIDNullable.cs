using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Management.Migrations
{
    /// <inheritdoc />
    public partial class ChangeManagerIDNullable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(name: "ManagerID",
                table: "Employees",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int"
                );
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}
