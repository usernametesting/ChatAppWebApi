using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Persistence.Migrations
{
    /// <inheritdoc />
    public partial class m8900987 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "UnreadMessageCount",
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

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "UnreadMessageCount",
                table: "Users",
                type: "int",
                nullable: true);

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp", "UnreadMessageCount" },
                values: new object[] { "4123c268-f69f-43e2-a611-b4ba6026a645", "AQAAAAIAAYagAAAAEDRTJpZnYqgMeD5S+hyQTccDON934HiEK1qZKLslyt9cBUWg9dhLysLE6aWhRlUllA==", "ad85c486-525b-4190-8576-f60af61d695d", null });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp", "UnreadMessageCount" },
                values: new object[] { "0af17e30-1bb4-405e-a9f4-052624c373c8", "AQAAAAIAAYagAAAAEGWQecAPkgJxv1qYDHwI6NC7zgr5f+32OSXEzMNH3YIJslsJQDQK/ChHoWnrC3kHhA==", "89a9a607-14cc-41f3-9375-ab47777e7e35", null });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp", "UnreadMessageCount" },
                values: new object[] { "8aa98daa-d1f3-46bc-8434-89dd4611af3f", "AQAAAAIAAYagAAAAEEvoU+MMxLG2dkId5FqG1X1rHqGd4NMfTAq98tFmp8klBHdGT2K3gtXa9vZerXSxww==", "496a0ea8-3f0e-40a7-823c-3a6f101e0b7b", null });
        }
    }
}
