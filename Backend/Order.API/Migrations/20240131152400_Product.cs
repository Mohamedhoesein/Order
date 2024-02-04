using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Order.API.Migrations
{
    public partial class Product : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_OpenSpecificationValues_Products_ProductName_SubcategoryNam~",
                table: "OpenSpecificationValues");

            migrationBuilder.DropTable(
                name: "ClosedSpecificationValueProduct");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Products",
                table: "Products");

            migrationBuilder.DropPrimaryKey(
                name: "PK_OpenSpecificationValues",
                table: "OpenSpecificationValues");

            migrationBuilder.DropIndex(
                name: "IX_OpenSpecificationValues_ProductName_SubcategoryName_Categor~",
                table: "OpenSpecificationValues");

            migrationBuilder.DropColumn(
                name: "Name",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "Description",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "Price",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "ProductName",
                table: "OpenSpecificationValues");

            migrationBuilder.AddColumn<long>(
                name: "Id",
                table: "Products",
                type: "bigint",
                nullable: false,
                defaultValue: 0L)
                .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityAlwaysColumn);

            migrationBuilder.AddColumn<long>(
                name: "ProductId",
                table: "OpenSpecificationValues",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AddColumn<long>(
                name: "ProductVersionNumber",
                table: "OpenSpecificationValues",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AddPrimaryKey(
                name: "PK_Products",
                table: "Products",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_OpenSpecificationValues",
                table: "OpenSpecificationValues",
                columns: new[] { "ProductId", "ProductVersionNumber", "SpecificationName", "SubcategoryName", "CategoryName", "MainCategoryName" });

            migrationBuilder.CreateTable(
                name: "ProductVersions",
                columns: table => new
                {
                    VersionNumber = table.Column<long>(type: "bigint", nullable: false),
                    ProductId = table.Column<long>(type: "bigint", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: false),
                    Description = table.Column<string>(type: "text", nullable: false),
                    Price = table.Column<decimal>(type: "numeric(20,0)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProductVersions", x => new { x.VersionNumber, x.ProductId });
                    table.UniqueConstraint("AK_ProductVersions_ProductId_VersionNumber", x => new { x.ProductId, x.VersionNumber });
                    table.ForeignKey(
                        name: "FK_ProductVersions_Products_ProductId",
                        column: x => x.ProductId,
                        principalTable: "Products",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ClosedSpecificationValueProductVersion",
                columns: table => new
                {
                    ProductVersionsVersionNumber = table.Column<long>(type: "bigint", nullable: false),
                    ProductVersionsProductId = table.Column<long>(type: "bigint", nullable: false),
                    ClosedSpecificationValuesValue = table.Column<string>(type: "text", nullable: false),
                    ClosedSpecificationValuesSpecificationName = table.Column<string>(type: "text", nullable: false),
                    ClosedSpecificationValuesSubcategoryName = table.Column<string>(type: "text", nullable: false),
                    ClosedSpecificationValuesCategoryName = table.Column<string>(type: "text", nullable: false),
                    ClosedSpecificationValuesMainCategoryName = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ClosedSpecificationValueProductVersion", x => new { x.ProductVersionsVersionNumber, x.ProductVersionsProductId, x.ClosedSpecificationValuesValue, x.ClosedSpecificationValuesSpecificationName, x.ClosedSpecificationValuesSubcategoryName, x.ClosedSpecificationValuesCategoryName, x.ClosedSpecificationValuesMainCategoryName });
                    table.ForeignKey(
                        name: "FK_ClosedSpecificationValueProductVersion_ClosedSpecificationV~",
                        columns: x => new { x.ClosedSpecificationValuesValue, x.ClosedSpecificationValuesSpecificationName, x.ClosedSpecificationValuesSubcategoryName, x.ClosedSpecificationValuesCategoryName, x.ClosedSpecificationValuesMainCategoryName },
                        principalTable: "ClosedSpecificationValues",
                        principalColumns: new[] { "Value", "SpecificationName", "SubcategoryName", "CategoryName", "MainCategoryName" },
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ClosedSpecificationValueProductVersion_ProductVersions_Prod~",
                        columns: x => new { x.ProductVersionsVersionNumber, x.ProductVersionsProductId },
                        principalTable: "ProductVersions",
                        principalColumns: new[] { "VersionNumber", "ProductId" },
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ProductImages",
                columns: table => new
                {
                    Name = table.Column<string>(type: "text", nullable: false),
                    ProductId = table.Column<long>(type: "bigint", nullable: false),
                    ProductVersionNumber = table.Column<long>(type: "bigint", nullable: false),
                    File = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProductImages", x => new { x.Name, x.ProductVersionNumber, x.ProductId });
                    table.ForeignKey(
                        name: "FK_ProductImages_ProductVersions_ProductId_ProductVersionNumber",
                        columns: x => new { x.ProductId, x.ProductVersionNumber },
                        principalTable: "ProductVersions",
                        principalColumns: new[] { "ProductId", "VersionNumber" },
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "AspNetRoleClaims",
                columns: new[] { "Id", "ClaimType", "ClaimValue", "RoleId" },
                values: new object[] { 9, "Permission", "Product.Manage", 1 });

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: 1,
                column: "ConcurrencyStamp",
                value: "1aa44a12-9adf-45b2-beca-11b760b69533");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: 2,
                column: "ConcurrencyStamp",
                value: "8f477384-7efe-454c-b1c9-a7a4281a1572");

            migrationBuilder.CreateIndex(
                name: "IX_ClosedSpecificationValueProductVersion_ClosedSpecificationV~",
                table: "ClosedSpecificationValueProductVersion",
                columns: new[] { "ClosedSpecificationValuesValue", "ClosedSpecificationValuesSpecificationName", "ClosedSpecificationValuesSubcategoryName", "ClosedSpecificationValuesCategoryName", "ClosedSpecificationValuesMainCategoryName" });

            migrationBuilder.CreateIndex(
                name: "IX_ProductImages_ProductId_ProductVersionNumber",
                table: "ProductImages",
                columns: new[] { "ProductId", "ProductVersionNumber" });

            migrationBuilder.AddForeignKey(
                name: "FK_OpenSpecificationValues_ProductVersions_ProductId_ProductVe~",
                table: "OpenSpecificationValues",
                columns: new[] { "ProductId", "ProductVersionNumber" },
                principalTable: "ProductVersions",
                principalColumns: new[] { "ProductId", "VersionNumber" },
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_OpenSpecificationValues_ProductVersions_ProductId_ProductVe~",
                table: "OpenSpecificationValues");

            migrationBuilder.DropTable(
                name: "ClosedSpecificationValueProductVersion");

            migrationBuilder.DropTable(
                name: "ProductImages");

            migrationBuilder.DropTable(
                name: "ProductVersions");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Products",
                table: "Products");

            migrationBuilder.DropPrimaryKey(
                name: "PK_OpenSpecificationValues",
                table: "OpenSpecificationValues");

            migrationBuilder.DeleteData(
                table: "AspNetRoleClaims",
                keyColumn: "Id",
                keyValue: 9);

            migrationBuilder.DropColumn(
                name: "Id",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "ProductId",
                table: "OpenSpecificationValues");

            migrationBuilder.DropColumn(
                name: "ProductVersionNumber",
                table: "OpenSpecificationValues");

            migrationBuilder.AddColumn<string>(
                name: "Name",
                table: "Products",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Description",
                table: "Products",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<int>(
                name: "Price",
                table: "Products",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "ProductName",
                table: "OpenSpecificationValues",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Products",
                table: "Products",
                columns: new[] { "Name", "SubcategoryName", "CategoryName", "MainCategoryName" });

            migrationBuilder.AddPrimaryKey(
                name: "PK_OpenSpecificationValues",
                table: "OpenSpecificationValues",
                columns: new[] { "ProductName", "SpecificationName", "SubcategoryName", "CategoryName", "MainCategoryName" });

            migrationBuilder.CreateTable(
                name: "ClosedSpecificationValueProduct",
                columns: table => new
                {
                    ProductsName = table.Column<string>(type: "text", nullable: false),
                    ProductsSubcategoryName = table.Column<string>(type: "text", nullable: false),
                    ProductsCategoryName = table.Column<string>(type: "text", nullable: false),
                    ProductsMainCategoryName = table.Column<string>(type: "text", nullable: false),
                    ClosedSpecificationValuesValue = table.Column<string>(type: "text", nullable: false),
                    ClosedSpecificationValuesSpecificationName = table.Column<string>(type: "text", nullable: false),
                    ClosedSpecificationValuesSubcategoryName = table.Column<string>(type: "text", nullable: false),
                    ClosedSpecificationValuesCategoryName = table.Column<string>(type: "text", nullable: false),
                    ClosedSpecificationValuesMainCategoryName = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ClosedSpecificationValueProduct", x => new { x.ProductsName, x.ProductsSubcategoryName, x.ProductsCategoryName, x.ProductsMainCategoryName, x.ClosedSpecificationValuesValue, x.ClosedSpecificationValuesSpecificationName, x.ClosedSpecificationValuesSubcategoryName, x.ClosedSpecificationValuesCategoryName, x.ClosedSpecificationValuesMainCategoryName });
                    table.ForeignKey(
                        name: "FK_ClosedSpecificationValueProduct_ClosedSpecificationValues_C~",
                        columns: x => new { x.ClosedSpecificationValuesValue, x.ClosedSpecificationValuesSpecificationName, x.ClosedSpecificationValuesSubcategoryName, x.ClosedSpecificationValuesCategoryName, x.ClosedSpecificationValuesMainCategoryName },
                        principalTable: "ClosedSpecificationValues",
                        principalColumns: new[] { "Value", "SpecificationName", "SubcategoryName", "CategoryName", "MainCategoryName" },
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ClosedSpecificationValueProduct_Products_ProductsName_Produ~",
                        columns: x => new { x.ProductsName, x.ProductsSubcategoryName, x.ProductsCategoryName, x.ProductsMainCategoryName },
                        principalTable: "Products",
                        principalColumns: new[] { "Name", "SubcategoryName", "CategoryName", "MainCategoryName" },
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: 1,
                column: "ConcurrencyStamp",
                value: "5204dc93-7c2c-4c64-b37a-736352c69c6a");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: 2,
                column: "ConcurrencyStamp",
                value: "b548b588-e9da-448b-b408-aa8d151d708d");

            migrationBuilder.CreateIndex(
                name: "IX_OpenSpecificationValues_ProductName_SubcategoryName_Categor~",
                table: "OpenSpecificationValues",
                columns: new[] { "ProductName", "SubcategoryName", "CategoryName", "MainCategoryName" });

            migrationBuilder.CreateIndex(
                name: "IX_ClosedSpecificationValueProduct_ClosedSpecificationValuesVa~",
                table: "ClosedSpecificationValueProduct",
                columns: new[] { "ClosedSpecificationValuesValue", "ClosedSpecificationValuesSpecificationName", "ClosedSpecificationValuesSubcategoryName", "ClosedSpecificationValuesCategoryName", "ClosedSpecificationValuesMainCategoryName" });

            migrationBuilder.AddForeignKey(
                name: "FK_OpenSpecificationValues_Products_ProductName_SubcategoryNam~",
                table: "OpenSpecificationValues",
                columns: new[] { "ProductName", "SubcategoryName", "CategoryName", "MainCategoryName" },
                principalTable: "Products",
                principalColumns: new[] { "Name", "SubcategoryName", "CategoryName", "MainCategoryName" },
                onDelete: ReferentialAction.Cascade);
        }
    }
}
