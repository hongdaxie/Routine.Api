using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Routine.Api.Migrations
{
    public partial class AddEmployeesData : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Companies",
                keyColumn: "Id",
                keyValue: new Guid("72f7850c-f5e8-4762-98d7-e9709cf50750"));

            migrationBuilder.DeleteData(
                table: "Companies",
                keyColumn: "Id",
                keyValue: new Guid("ad147e58-4513-4f7f-86de-6972e6a3d9a8"));

            migrationBuilder.InsertData(
                table: "Companies",
                columns: new[] { "Id", "Introduction", "Name" },
                values: new object[] { new Guid("6fb600c1-9011-4fd7-9234-881379716440"), "Don`t be evil", "Google" });

            migrationBuilder.InsertData(
                table: "Companies",
                columns: new[] { "Id", "Introduction", "Name" },
                values: new object[] { new Guid("5efc910b-2f45-43df-afee-620d40542853"), "Fubao Company", "Alipapa" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Companies",
                keyColumn: "Id",
                keyValue: new Guid("5efc910b-2f45-43df-afee-620d40542853"));

            migrationBuilder.DeleteData(
                table: "Companies",
                keyColumn: "Id",
                keyValue: new Guid("6fb600c1-9011-4fd7-9234-881379716440"));

            migrationBuilder.InsertData(
                table: "Companies",
                columns: new[] { "Id", "Introduction", "Name" },
                values: new object[] { new Guid("ad147e58-4513-4f7f-86de-6972e6a3d9a8"), "Don`t be evil", "Google" });

            migrationBuilder.InsertData(
                table: "Companies",
                columns: new[] { "Id", "Introduction", "Name" },
                values: new object[] { new Guid("72f7850c-f5e8-4762-98d7-e9709cf50750"), "Fubao Company", "Alipapa" });
        }
    }
}
