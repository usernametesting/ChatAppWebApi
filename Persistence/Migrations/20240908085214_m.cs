using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Persistence.Migrations
{
    /// <inheritdoc />
    public partial class m : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UsersMessages_Users_ToUserId1",
                table: "UsersMessages");

            migrationBuilder.DropIndex(
                name: "IX_UsersMessages_ToUserId1",
                table: "UsersMessages");

            migrationBuilder.DropColumn(
                name: "ToUserId1",
                table: "UsersMessages");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "c8a7a8aa-dad4-4522-919a-043ccd025ca5", "AQAAAAIAAYagAAAAEOdyWJxvBe2sUxlU2ur3AbW0ZkLCeT6HuFS8OWDz0P+ZtFvUILFB56oqOngrRIshfQ==", "735c62f3-5607-4cc2-8925-4828d020a909" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ToUserId1",
                table: "UsersMessages",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "e04f8a91-83d1-443a-92d5-4f5740d91600", "AQAAAAIAAYagAAAAEFFkDKSf28h3CYYs3OdtjJxxlT809/v2Tx5v8FyoBqy9B7tbEWKVEXjds7LGTZEHEA==", "eda56185-f864-41a8-a5dc-84f1921e20dc" });

            migrationBuilder.CreateIndex(
                name: "IX_UsersMessages_ToUserId1",
                table: "UsersMessages",
                column: "ToUserId1");

            migrationBuilder.AddForeignKey(
                name: "FK_UsersMessages_Users_ToUserId1",
                table: "UsersMessages",
                column: "ToUserId1",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
