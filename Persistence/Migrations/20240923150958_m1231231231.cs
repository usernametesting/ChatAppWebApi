using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Persistence.Migrations
{
    /// <inheritdoc />
    public partial class m1231231231 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Status",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    StatusType = table.Column<int>(type: "int", nullable: false),
                    MediaUrl = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Decription = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UserId = table.Column<int>(type: "int", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Status", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Status_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "fa4b5632-6c4e-4dbc-93dd-e2b6f0b81c28", "AQAAAAIAAYagAAAAELztl+shy8PXfb0zr1U/oEjstJ71yG7gp7CfO+yzdd9PiAcTjV2D+GlLlhpsXHMyyg==", "3912ca18-1256-4b86-9c57-2ef38eb0ba08" });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "47594e47-cb3c-4bb8-b18f-c5ad434dad3f", "AQAAAAIAAYagAAAAEIz6RfkvonWH/NlZcU6kpdvyY40Ji7Voe7q25CcUewL+qAWVjl2lF6ISf7KE5g7eRA==", "e403d330-dac6-47e2-a4c3-73f30994c60a" });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "2af7943a-3542-4d30-8369-985760430406", "AQAAAAIAAYagAAAAEISSEP5r9eia/uCUJcC/atfRkc4tK6GmgUjxTNgTHuvtI0hzAoRi4MhLcu45GuIvtA==", "bb0cef7d-cd0d-4cb2-a259-0a9ae286b6b2" });

            migrationBuilder.CreateIndex(
                name: "IX_Status_UserId",
                table: "Status",
                column: "UserId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Status");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "4587fe6d-6cc2-4741-a8cf-43ef24db3074", "AQAAAAIAAYagAAAAEADNwcub5adlDQBFtTUebW0v6x9gctULspuV6a3igGAOVdJFN5EdumMufboKTcoBFA==", "3d39de2f-5e67-45b3-967e-bdeef48d2c49" });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "1a22adbe-5ed3-4d09-9c70-62ab4b96bb40", "AQAAAAIAAYagAAAAEO2h9AGNyIqI1erVCx4FafInENNTObh/WRyUnAv2hAFLojdnr7xN4CZq7BStd3pXJw==", "d334053d-d9b4-40ae-aad1-fb3728895e04" });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "746ad7c1-28fc-4385-827c-11caaf934e1b", "AQAAAAIAAYagAAAAEOMuq4uDZ6Qz0H55SaeI4vIOIfAR8OysG0Q8MAA3TC9+cMgHQiF1jXicM4RRTrRzMw==", "61ae8534-0ce7-45a5-b365-b5db3886a5dc" });
        }
    }
}
