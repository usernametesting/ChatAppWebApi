using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Persistence.Migrations
{
    /// <inheritdoc />
    public partial class mm4 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ProfImageUrl",
                table: "Users",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "ProfImageUrl", "SecurityStamp" },
                values: new object[] { "4e2883b8-bc71-431c-b669-300182cbc460", "AQAAAAIAAYagAAAAECRF9N2Z+wmpM0r03lsHFB56SodhIJ14gQvJx+/+YUmVaoBRHxB/19TSJ8UHJaDdYA==", null, "183ccdaf-e390-4fbd-8eb8-6474e3fba3bc" });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "ProfImageUrl", "SecurityStamp" },
                values: new object[] { "bbac2b7f-8077-4069-b483-9ff328ec9362", "AQAAAAIAAYagAAAAEAncOHB1A9fFzQT03Z3mo4CJvwjxYVq6wmmY/Z7Opbr9E+tqLQJJc/eOMs49Aqohag==", null, "a91da54e-a2d2-45b2-998e-78e58579eb2b" });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "ProfImageUrl", "SecurityStamp" },
                values: new object[] { "27a94c09-450c-4dba-b0ae-615e6d64b9cf", "AQAAAAIAAYagAAAAECquAZZNGQvc09eoplqdFGFrh9JtvzGanpGL2wHpaSQIn5oIKIbDg6qX81JAgd2U0Q==", null, "1898f4ce-63a6-4c1c-a805-80197981bed5" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ProfImageUrl",
                table: "Users");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "66a9e531-a436-45b5-a1fe-04f2ceed4c01", "AQAAAAIAAYagAAAAEDl2+URJSkGaqrbapCsrbadSSYRN706P+nWMAulyreNKX83kS0EyTvPJq9SizSEi9A==", "e2ef11a3-37bf-48dc-a72a-ffeeec117428" });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "0c41399c-ae72-42b1-9131-66913af4e609", "AQAAAAIAAYagAAAAEJh/L4Rdw+fe2vYXTaR0Eovqdd4JiIXg58olvR6Is2wxG8XtlujAGq72+x3jMVjnlg==", "57545dd6-e2a3-4b14-a5f0-2ce10cb8b558" });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "9b25146d-c021-4fcd-a7d7-6bc510c88a91", "AQAAAAIAAYagAAAAEHjG1K0LidfzWNaxY3iewHndI6v5OLJtUEu5Hb6OTRqizPWcw0kJvwh1LiQDi4FzvQ==", "35869607-c5ff-47fc-90e5-f3c3df76d4e6" });
        }
    }
}
