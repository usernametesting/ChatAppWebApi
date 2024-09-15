using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Persistence.Migrations
{
    /// <inheritdoc />
    public partial class m890 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
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

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "UnreadMessageCount",
                table: "Users");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "06dc5b17-828d-4666-befb-ad713daf14b2", "AQAAAAIAAYagAAAAEH380dcK6YFWCN7GAljDgkIRM2Nrr8hg78fogcQxHCXRwtd9Io6cvePutglpiSoSjg==", "64adba8a-bd24-4662-96e1-6345b5661f96" });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "a968441d-8369-474c-a87f-2f85f8d446f0", "AQAAAAIAAYagAAAAEK9crnir2M0axPlu7MclPBzW6qdHw8lhmJ7/SZI8Sf2SCeZn4EYxf5hzsK7BwMzLNA==", "619eea9a-70a1-4b6b-9272-17a125af23a8" });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "61fd1185-7c35-4b60-92c4-4288ee218732", "AQAAAAIAAYagAAAAEDyhFOeLHSsHm00+1mM5biUPKCIFkYIrsVO4vueIff/9HQvtvMNj1gi2klGSyhnYWg==", "a42a6094-8961-4e7c-8bee-b65f1730d0e2" });
        }
    }
}
