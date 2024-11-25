using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ProductInventoryManagerAPI.Migrations
{
    /// <inheritdoc />
    public partial class FirstMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "tbl_Products",
                columns: table => new
                {
                    productID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "100000, 1"),
                    productName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    stockAvailable = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tbl_Products", x => x.productID);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "tbl_Products");
        }
    }
}
