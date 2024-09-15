using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Persistence.Migrations
{
    /// <inheritdoc />
    public partial class m89 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
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

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "483eb92a-52e5-4eaa-99cc-45da81f78d90", "AQAAAAIAAYagAAAAEHtZHuleNDmTDXc5Du88qptCCJA80jvpSfysP7juvKAX7pN9KgPpjdgfpo529xEYwg==", "2455c13f-c6a0-48ce-9c03-a91d240b68f1" });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "e44f394f-6058-4c34-8f7e-9fa14e046bc7", "AQAAAAIAAYagAAAAECI86MkVT+s3TIrPVHKepgCqPFfnxLvc5vaY9jf/oKwYpdHUg+GNspHa1uzsNb4GDw==", "60bf6a12-c933-4df9-9dc0-54871ef6cb6b" });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "27235aee-b5ed-4590-88de-875e4f4da9c7", "AQAAAAIAAYagAAAAEITAA5lVW9z2Urn+9cWHNKFKS4PdH+dvl2W29FeKGp7n0mo+45BrTBtBMpGRb/BMag==", "409ff7ac-2846-4853-bd1e-73ab10767adb" });
        }
    }
}
