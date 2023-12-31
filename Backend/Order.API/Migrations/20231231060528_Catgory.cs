using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Order.API.Migrations
{
    public partial class Catgory : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "MainCategories",
                columns: table => new
                {
                    Name = table.Column<string>(type: "text", nullable: false),
                    Deleted = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MainCategories", x => x.Name);
                });

            migrationBuilder.CreateTable(
                name: "Categories",
                columns: table => new
                {
                    Name = table.Column<string>(type: "text", nullable: false),
                    MainCategoryName = table.Column<string>(type: "text", nullable: false),
                    Deleted = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Categories", x => new { x.Name, x.MainCategoryName });
                    table.ForeignKey(
                        name: "FK_Categories_MainCategories_MainCategoryName",
                        column: x => x.MainCategoryName,
                        principalTable: "MainCategories",
                        principalColumn: "Name",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Subcategories",
                columns: table => new
                {
                    Name = table.Column<string>(type: "text", nullable: false),
                    CategoryName = table.Column<string>(type: "text", nullable: false),
                    MainCategoryName = table.Column<string>(type: "text", nullable: false),
                    Deleted = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Subcategories", x => new { x.Name, x.CategoryName, x.MainCategoryName });
                    table.ForeignKey(
                        name: "FK_Subcategories_Categories_CategoryName_MainCategoryName",
                        columns: x => new { x.CategoryName, x.MainCategoryName },
                        principalTable: "Categories",
                        principalColumns: new[] { "Name", "MainCategoryName" },
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ClosedSpecifications",
                columns: table => new
                {
                    Name = table.Column<string>(type: "text", nullable: false),
                    SubcategoryName = table.Column<string>(type: "text", nullable: false),
                    CategoryName = table.Column<string>(type: "text", nullable: false),
                    MainCategoryName = table.Column<string>(type: "text", nullable: false),
                    Deleted = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ClosedSpecifications", x => new { x.Name, x.SubcategoryName, x.CategoryName, x.MainCategoryName });
                    table.ForeignKey(
                        name: "FK_ClosedSpecifications_Subcategories_SubcategoryName_Category~",
                        columns: x => new { x.SubcategoryName, x.CategoryName, x.MainCategoryName },
                        principalTable: "Subcategories",
                        principalColumns: new[] { "Name", "CategoryName", "MainCategoryName" },
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "OpenSpecifications",
                columns: table => new
                {
                    Name = table.Column<string>(type: "text", nullable: false),
                    SubcategoryName = table.Column<string>(type: "text", nullable: false),
                    CategoryName = table.Column<string>(type: "text", nullable: false),
                    MainCategoryName = table.Column<string>(type: "text", nullable: false),
                    Deleted = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OpenSpecifications", x => new { x.Name, x.SubcategoryName, x.CategoryName, x.MainCategoryName });
                    table.ForeignKey(
                        name: "FK_OpenSpecifications_Subcategories_SubcategoryName_CategoryNa~",
                        columns: x => new { x.SubcategoryName, x.CategoryName, x.MainCategoryName },
                        principalTable: "Subcategories",
                        principalColumns: new[] { "Name", "CategoryName", "MainCategoryName" },
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Products",
                columns: table => new
                {
                    Name = table.Column<string>(type: "text", nullable: false),
                    SubcategoryName = table.Column<string>(type: "text", nullable: false),
                    CategoryName = table.Column<string>(type: "text", nullable: false),
                    MainCategoryName = table.Column<string>(type: "text", nullable: false),
                    Description = table.Column<string>(type: "text", nullable: false),
                    Price = table.Column<int>(type: "integer", nullable: false),
                    Deleted = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Products", x => new { x.Name, x.SubcategoryName, x.CategoryName, x.MainCategoryName });
                    table.ForeignKey(
                        name: "FK_Products_Subcategories_SubcategoryName_CategoryName_MainCat~",
                        columns: x => new { x.SubcategoryName, x.CategoryName, x.MainCategoryName },
                        principalTable: "Subcategories",
                        principalColumns: new[] { "Name", "CategoryName", "MainCategoryName" },
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ClosedSpecificationValues",
                columns: table => new
                {
                    Value = table.Column<string>(type: "text", nullable: false),
                    SpecificationName = table.Column<string>(type: "text", nullable: false),
                    SubcategoryName = table.Column<string>(type: "text", nullable: false),
                    CategoryName = table.Column<string>(type: "text", nullable: false),
                    MainCategoryName = table.Column<string>(type: "text", nullable: false),
                    Deleted = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ClosedSpecificationValues", x => new { x.Value, x.SpecificationName, x.SubcategoryName, x.CategoryName, x.MainCategoryName });
                    table.ForeignKey(
                        name: "FK_ClosedSpecificationValues_ClosedSpecifications_Specificatio~",
                        columns: x => new { x.SpecificationName, x.SubcategoryName, x.CategoryName, x.MainCategoryName },
                        principalTable: "ClosedSpecifications",
                        principalColumns: new[] { "Name", "SubcategoryName", "CategoryName", "MainCategoryName" },
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Filters",
                columns: table => new
                {
                    SubcategoryName = table.Column<string>(type: "text", nullable: false),
                    CategoryName = table.Column<string>(type: "text", nullable: false),
                    MainCategoryName = table.Column<string>(type: "text", nullable: false),
                    ClosedSpecificationName = table.Column<string>(type: "text", nullable: false),
                    Title = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Filters", x => new { x.ClosedSpecificationName, x.SubcategoryName, x.CategoryName, x.MainCategoryName });
                    table.ForeignKey(
                        name: "FK_Filters_ClosedSpecifications_ClosedSpecificationName_Subcat~",
                        columns: x => new { x.ClosedSpecificationName, x.SubcategoryName, x.CategoryName, x.MainCategoryName },
                        principalTable: "ClosedSpecifications",
                        principalColumns: new[] { "Name", "SubcategoryName", "CategoryName", "MainCategoryName" },
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Filters_Subcategories_SubcategoryName_CategoryName_MainCate~",
                        columns: x => new { x.SubcategoryName, x.CategoryName, x.MainCategoryName },
                        principalTable: "Subcategories",
                        principalColumns: new[] { "Name", "CategoryName", "MainCategoryName" },
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "OpenSpecificationValues",
                columns: table => new
                {
                    SpecificationName = table.Column<string>(type: "text", nullable: false),
                    SubcategoryName = table.Column<string>(type: "text", nullable: false),
                    CategoryName = table.Column<string>(type: "text", nullable: false),
                    MainCategoryName = table.Column<string>(type: "text", nullable: false),
                    ProductName = table.Column<string>(type: "text", nullable: false),
                    Value = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OpenSpecificationValues", x => new { x.ProductName, x.SpecificationName, x.SubcategoryName, x.CategoryName, x.MainCategoryName });
                    table.ForeignKey(
                        name: "FK_OpenSpecificationValues_OpenSpecifications_SpecificationNam~",
                        columns: x => new { x.SpecificationName, x.SubcategoryName, x.CategoryName, x.MainCategoryName },
                        principalTable: "OpenSpecifications",
                        principalColumns: new[] { "Name", "SubcategoryName", "CategoryName", "MainCategoryName" },
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_OpenSpecificationValues_Products_ProductName_SubcategoryNam~",
                        columns: x => new { x.ProductName, x.SubcategoryName, x.CategoryName, x.MainCategoryName },
                        principalTable: "Products",
                        principalColumns: new[] { "Name", "SubcategoryName", "CategoryName", "MainCategoryName" },
                        onDelete: ReferentialAction.Cascade);
                });

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
                table: "AspNetRoleClaims",
                keyColumn: "Id",
                keyValue: 7,
                column: "ClaimValue",
                value: "Controller.Manage");

            migrationBuilder.InsertData(
                table: "AspNetRoleClaims",
                columns: new[] { "Id", "ClaimType", "ClaimValue", "RoleId" },
                values: new object[] { 8, "Permission", "Employee", 1 });

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
                name: "IX_Categories_MainCategoryName",
                table: "Categories",
                column: "MainCategoryName");

            migrationBuilder.CreateIndex(
                name: "IX_ClosedSpecifications_SubcategoryName_CategoryName_MainCateg~",
                table: "ClosedSpecifications",
                columns: new[] { "SubcategoryName", "CategoryName", "MainCategoryName" });

            migrationBuilder.CreateIndex(
                name: "IX_ClosedSpecificationValueProduct_ClosedSpecificationValuesVa~",
                table: "ClosedSpecificationValueProduct",
                columns: new[] { "ClosedSpecificationValuesValue", "ClosedSpecificationValuesSpecificationName", "ClosedSpecificationValuesSubcategoryName", "ClosedSpecificationValuesCategoryName", "ClosedSpecificationValuesMainCategoryName" });

            migrationBuilder.CreateIndex(
                name: "IX_ClosedSpecificationValues_SpecificationName_SubcategoryName~",
                table: "ClosedSpecificationValues",
                columns: new[] { "SpecificationName", "SubcategoryName", "CategoryName", "MainCategoryName" });

            migrationBuilder.CreateIndex(
                name: "IX_Filters_SubcategoryName_CategoryName_MainCategoryName",
                table: "Filters",
                columns: new[] { "SubcategoryName", "CategoryName", "MainCategoryName" });

            migrationBuilder.CreateIndex(
                name: "IX_OpenSpecifications_SubcategoryName_CategoryName_MainCategor~",
                table: "OpenSpecifications",
                columns: new[] { "SubcategoryName", "CategoryName", "MainCategoryName" });

            migrationBuilder.CreateIndex(
                name: "IX_OpenSpecificationValues_ProductName_SubcategoryName_Categor~",
                table: "OpenSpecificationValues",
                columns: new[] { "ProductName", "SubcategoryName", "CategoryName", "MainCategoryName" });

            migrationBuilder.CreateIndex(
                name: "IX_OpenSpecificationValues_SpecificationName_SubcategoryName_C~",
                table: "OpenSpecificationValues",
                columns: new[] { "SpecificationName", "SubcategoryName", "CategoryName", "MainCategoryName" });

            migrationBuilder.CreateIndex(
                name: "IX_Products_SubcategoryName_CategoryName_MainCategoryName",
                table: "Products",
                columns: new[] { "SubcategoryName", "CategoryName", "MainCategoryName" });

            migrationBuilder.CreateIndex(
                name: "IX_Subcategories_CategoryName_MainCategoryName",
                table: "Subcategories",
                columns: new[] { "CategoryName", "MainCategoryName" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ClosedSpecificationValueProduct");

            migrationBuilder.DropTable(
                name: "Filters");

            migrationBuilder.DropTable(
                name: "OpenSpecificationValues");

            migrationBuilder.DropTable(
                name: "ClosedSpecificationValues");

            migrationBuilder.DropTable(
                name: "OpenSpecifications");

            migrationBuilder.DropTable(
                name: "Products");

            migrationBuilder.DropTable(
                name: "ClosedSpecifications");

            migrationBuilder.DropTable(
                name: "Subcategories");

            migrationBuilder.DropTable(
                name: "Categories");

            migrationBuilder.DropTable(
                name: "MainCategories");

            migrationBuilder.DeleteData(
                table: "AspNetRoleClaims",
                keyColumn: "Id",
                keyValue: 8);

            migrationBuilder.UpdateData(
                table: "AspNetRoleClaims",
                keyColumn: "Id",
                keyValue: 7,
                column: "ClaimValue",
                value: "Employee");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: 1,
                column: "ConcurrencyStamp",
                value: "d4e5e9de-9a48-4b7b-a563-7500727adf05");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: 2,
                column: "ConcurrencyStamp",
                value: "102c41b7-42b7-41ad-9b73-b5ff7fcbdc61");
        }
    }
}
