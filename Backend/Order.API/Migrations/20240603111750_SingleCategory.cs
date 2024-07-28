using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Order.API.Migrations
{
    public partial class SingleCategory : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Categories_MainCategories_MainCategoryName",
                table: "Categories");

            migrationBuilder.DropForeignKey(
                name: "FK_ClosedSpecifications_Subcategories_SubcategoryName_Category~",
                table: "ClosedSpecifications");

            migrationBuilder.DropForeignKey(
                name: "FK_ClosedSpecificationValueProductVersion_ClosedSpecificationV~",
                table: "ClosedSpecificationValueProductVersion");

            migrationBuilder.DropForeignKey(
                name: "FK_ClosedSpecificationValues_ClosedSpecifications_Specificatio~",
                table: "ClosedSpecificationValues");

            migrationBuilder.DropForeignKey(
                name: "FK_Filters_ClosedSpecifications_ClosedSpecificationName_Subcat~",
                table: "Filters");

            migrationBuilder.DropForeignKey(
                name: "FK_Filters_Subcategories_SubcategoryName_CategoryName_MainCate~",
                table: "Filters");

            migrationBuilder.DropForeignKey(
                name: "FK_OpenSpecifications_Subcategories_SubcategoryName_CategoryNa~",
                table: "OpenSpecifications");

            migrationBuilder.DropForeignKey(
                name: "FK_OpenSpecificationValues_OpenSpecifications_SpecificationNam~",
                table: "OpenSpecificationValues");

            migrationBuilder.DropForeignKey(
                name: "FK_Products_Subcategories_SubcategoryName_CategoryName_MainCat~",
                table: "Products");

            migrationBuilder.DropTable(
                name: "MainCategories");

            migrationBuilder.DropTable(
                name: "Subcategories");

            migrationBuilder.DropIndex(
                name: "IX_Products_SubcategoryName_CategoryName_MainCategoryName",
                table: "Products");

            migrationBuilder.DropPrimaryKey(
                name: "PK_OpenSpecificationValues",
                table: "OpenSpecificationValues");

            migrationBuilder.DropIndex(
                name: "IX_OpenSpecificationValues_SpecificationName_SubcategoryName_C~",
                table: "OpenSpecificationValues");

            migrationBuilder.DropPrimaryKey(
                name: "PK_OpenSpecifications",
                table: "OpenSpecifications");

            migrationBuilder.DropIndex(
                name: "IX_OpenSpecifications_SubcategoryName_CategoryName_MainCategor~",
                table: "OpenSpecifications");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Filters",
                table: "Filters");

            migrationBuilder.DropIndex(
                name: "IX_Filters_SubcategoryName_CategoryName_MainCategoryName",
                table: "Filters");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ClosedSpecificationValues",
                table: "ClosedSpecificationValues");

            migrationBuilder.DropIndex(
                name: "IX_ClosedSpecificationValues_SpecificationName_SubcategoryName~",
                table: "ClosedSpecificationValues");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ClosedSpecificationValueProductVersion",
                table: "ClosedSpecificationValueProductVersion");

            migrationBuilder.DropIndex(
                name: "IX_ClosedSpecificationValueProductVersion_ClosedSpecificationV~",
                table: "ClosedSpecificationValueProductVersion");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ClosedSpecifications",
                table: "ClosedSpecifications");

            migrationBuilder.DropIndex(
                name: "IX_ClosedSpecifications_SubcategoryName_CategoryName_MainCateg~",
                table: "ClosedSpecifications");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Categories",
                table: "Categories");

            migrationBuilder.DropIndex(
                name: "IX_Categories_MainCategoryName",
                table: "Categories");

            migrationBuilder.DropColumn(
                name: "CategoryName",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "MainCategoryName",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "SubcategoryName",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "SpecificationName",
                table: "OpenSpecificationValues");

            migrationBuilder.DropColumn(
                name: "SubcategoryName",
                table: "OpenSpecificationValues");

            migrationBuilder.DropColumn(
                name: "CategoryName",
                table: "OpenSpecificationValues");

            migrationBuilder.DropColumn(
                name: "MainCategoryName",
                table: "OpenSpecificationValues");

            migrationBuilder.DropColumn(
                name: "SubcategoryName",
                table: "OpenSpecifications");

            migrationBuilder.DropColumn(
                name: "CategoryName",
                table: "OpenSpecifications");

            migrationBuilder.DropColumn(
                name: "MainCategoryName",
                table: "OpenSpecifications");

            migrationBuilder.DropColumn(
                name: "ClosedSpecificationName",
                table: "Filters");

            migrationBuilder.DropColumn(
                name: "SubcategoryName",
                table: "Filters");

            migrationBuilder.DropColumn(
                name: "CategoryName",
                table: "Filters");

            migrationBuilder.DropColumn(
                name: "MainCategoryName",
                table: "Filters");

            migrationBuilder.DropColumn(
                name: "SpecificationName",
                table: "ClosedSpecificationValues");

            migrationBuilder.DropColumn(
                name: "SubcategoryName",
                table: "ClosedSpecificationValues");

            migrationBuilder.DropColumn(
                name: "CategoryName",
                table: "ClosedSpecificationValues");

            migrationBuilder.DropColumn(
                name: "MainCategoryName",
                table: "ClosedSpecificationValues");

            migrationBuilder.DropColumn(
                name: "ClosedSpecificationValuesValue",
                table: "ClosedSpecificationValueProductVersion");

            migrationBuilder.DropColumn(
                name: "ClosedSpecificationValuesSpecificationName",
                table: "ClosedSpecificationValueProductVersion");

            migrationBuilder.DropColumn(
                name: "ClosedSpecificationValuesSubcategoryName",
                table: "ClosedSpecificationValueProductVersion");

            migrationBuilder.DropColumn(
                name: "ClosedSpecificationValuesCategoryName",
                table: "ClosedSpecificationValueProductVersion");

            migrationBuilder.DropColumn(
                name: "ClosedSpecificationValuesMainCategoryName",
                table: "ClosedSpecificationValueProductVersion");

            migrationBuilder.DropColumn(
                name: "SubcategoryName",
                table: "ClosedSpecifications");

            migrationBuilder.DropColumn(
                name: "CategoryName",
                table: "ClosedSpecifications");

            migrationBuilder.DropColumn(
                name: "MainCategoryName",
                table: "ClosedSpecifications");

            migrationBuilder.DropColumn(
                name: "MainCategoryName",
                table: "Categories");

            migrationBuilder.AddColumn<int>(
                name: "CategoryId",
                table: "Products",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Id",
                table: "OpenSpecificationValues",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "SpecificationId",
                table: "OpenSpecificationValues",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Id",
                table: "OpenSpecifications",
                type: "integer",
                nullable: false,
                defaultValue: 0)
                .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

            migrationBuilder.AddColumn<int>(
                name: "CategoryId",
                table: "OpenSpecifications",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Id",
                table: "Filters",
                type: "integer",
                nullable: false,
                defaultValue: 0)
                .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

            migrationBuilder.AddColumn<int>(
                name: "CategoryId",
                table: "Filters",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "SpecificationId",
                table: "Filters",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Id",
                table: "ClosedSpecificationValues",
                type: "integer",
                nullable: false,
                defaultValue: 0)
                .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

            migrationBuilder.AddColumn<int>(
                name: "SpecificationId",
                table: "ClosedSpecificationValues",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "ClosedSpecificationValuesId",
                table: "ClosedSpecificationValueProductVersion",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Id",
                table: "ClosedSpecifications",
                type: "integer",
                nullable: false,
                defaultValue: 0)
                .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

            migrationBuilder.AddColumn<int>(
                name: "CategoryId",
                table: "ClosedSpecifications",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Id",
                table: "Categories",
                type: "integer",
                nullable: false,
                defaultValue: 0)
                .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

            migrationBuilder.AddPrimaryKey(
                name: "PK_OpenSpecificationValues",
                table: "OpenSpecificationValues",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_OpenSpecifications",
                table: "OpenSpecifications",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Filters",
                table: "Filters",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ClosedSpecificationValues",
                table: "ClosedSpecificationValues",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ClosedSpecificationValueProductVersion",
                table: "ClosedSpecificationValueProductVersion",
                columns: new[] { "ClosedSpecificationValuesId", "ProductVersionsVersionNumber", "ProductVersionsProductId" });

            migrationBuilder.AddPrimaryKey(
                name: "PK_ClosedSpecifications",
                table: "ClosedSpecifications",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Categories",
                table: "Categories",
                column: "Id");

            migrationBuilder.UpdateData(
                table: "AspNetRoleClaims",
                keyColumn: "Id",
                keyValue: 7,
                column: "ClaimValue",
                value: "Category.Manage");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: 1,
                column: "ConcurrencyStamp",
                value: "54820877-fc36-4e58-9480-51fb82ec8ca1");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: 2,
                column: "ConcurrencyStamp",
                value: "82aae5fe-7eaa-4694-876d-1ec43af68130");

            migrationBuilder.CreateIndex(
                name: "IX_Products_CategoryId",
                table: "Products",
                column: "CategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_OpenSpecificationValues_ProductId_ProductVersionNumber",
                table: "OpenSpecificationValues",
                columns: new[] { "ProductId", "ProductVersionNumber" });

            migrationBuilder.CreateIndex(
                name: "IX_OpenSpecifications_CategoryId",
                table: "OpenSpecifications",
                column: "CategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_Filters_CategoryId",
                table: "Filters",
                column: "CategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_Filters_SpecificationId",
                table: "Filters",
                column: "SpecificationId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ClosedSpecificationValues_SpecificationId",
                table: "ClosedSpecificationValues",
                column: "SpecificationId");

            migrationBuilder.CreateIndex(
                name: "IX_ClosedSpecificationValueProductVersion_ProductVersionsVersi~",
                table: "ClosedSpecificationValueProductVersion",
                columns: new[] { "ProductVersionsVersionNumber", "ProductVersionsProductId" });

            migrationBuilder.CreateIndex(
                name: "IX_ClosedSpecifications_CategoryId",
                table: "ClosedSpecifications",
                column: "CategoryId");

            migrationBuilder.AddForeignKey(
                name: "FK_ClosedSpecifications_Categories_CategoryId",
                table: "ClosedSpecifications",
                column: "CategoryId",
                principalTable: "Categories",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ClosedSpecificationValueProductVersion_ClosedSpecificationV~",
                table: "ClosedSpecificationValueProductVersion",
                column: "ClosedSpecificationValuesId",
                principalTable: "ClosedSpecificationValues",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ClosedSpecificationValues_ClosedSpecifications_Specificatio~",
                table: "ClosedSpecificationValues",
                column: "SpecificationId",
                principalTable: "ClosedSpecifications",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Filters_Categories_CategoryId",
                table: "Filters",
                column: "CategoryId",
                principalTable: "Categories",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Filters_ClosedSpecifications_SpecificationId",
                table: "Filters",
                column: "SpecificationId",
                principalTable: "ClosedSpecifications",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_OpenSpecifications_Categories_CategoryId",
                table: "OpenSpecifications",
                column: "CategoryId",
                principalTable: "Categories",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_OpenSpecificationValues_OpenSpecifications_Id",
                table: "OpenSpecificationValues",
                column: "Id",
                principalTable: "OpenSpecifications",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Products_Categories_CategoryId",
                table: "Products",
                column: "CategoryId",
                principalTable: "Categories",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ClosedSpecifications_Categories_CategoryId",
                table: "ClosedSpecifications");

            migrationBuilder.DropForeignKey(
                name: "FK_ClosedSpecificationValueProductVersion_ClosedSpecificationV~",
                table: "ClosedSpecificationValueProductVersion");

            migrationBuilder.DropForeignKey(
                name: "FK_ClosedSpecificationValues_ClosedSpecifications_Specificatio~",
                table: "ClosedSpecificationValues");

            migrationBuilder.DropForeignKey(
                name: "FK_Filters_Categories_CategoryId",
                table: "Filters");

            migrationBuilder.DropForeignKey(
                name: "FK_Filters_ClosedSpecifications_SpecificationId",
                table: "Filters");

            migrationBuilder.DropForeignKey(
                name: "FK_OpenSpecifications_Categories_CategoryId",
                table: "OpenSpecifications");

            migrationBuilder.DropForeignKey(
                name: "FK_OpenSpecificationValues_OpenSpecifications_Id",
                table: "OpenSpecificationValues");

            migrationBuilder.DropForeignKey(
                name: "FK_Products_Categories_CategoryId",
                table: "Products");

            migrationBuilder.DropIndex(
                name: "IX_Products_CategoryId",
                table: "Products");

            migrationBuilder.DropPrimaryKey(
                name: "PK_OpenSpecificationValues",
                table: "OpenSpecificationValues");

            migrationBuilder.DropIndex(
                name: "IX_OpenSpecificationValues_ProductId_ProductVersionNumber",
                table: "OpenSpecificationValues");

            migrationBuilder.DropPrimaryKey(
                name: "PK_OpenSpecifications",
                table: "OpenSpecifications");

            migrationBuilder.DropIndex(
                name: "IX_OpenSpecifications_CategoryId",
                table: "OpenSpecifications");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Filters",
                table: "Filters");

            migrationBuilder.DropIndex(
                name: "IX_Filters_CategoryId",
                table: "Filters");

            migrationBuilder.DropIndex(
                name: "IX_Filters_SpecificationId",
                table: "Filters");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ClosedSpecificationValues",
                table: "ClosedSpecificationValues");

            migrationBuilder.DropIndex(
                name: "IX_ClosedSpecificationValues_SpecificationId",
                table: "ClosedSpecificationValues");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ClosedSpecificationValueProductVersion",
                table: "ClosedSpecificationValueProductVersion");

            migrationBuilder.DropIndex(
                name: "IX_ClosedSpecificationValueProductVersion_ProductVersionsVersi~",
                table: "ClosedSpecificationValueProductVersion");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ClosedSpecifications",
                table: "ClosedSpecifications");

            migrationBuilder.DropIndex(
                name: "IX_ClosedSpecifications_CategoryId",
                table: "ClosedSpecifications");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Categories",
                table: "Categories");

            migrationBuilder.DropColumn(
                name: "CategoryId",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "Id",
                table: "OpenSpecificationValues");

            migrationBuilder.DropColumn(
                name: "SpecificationId",
                table: "OpenSpecificationValues");

            migrationBuilder.DropColumn(
                name: "Id",
                table: "OpenSpecifications");

            migrationBuilder.DropColumn(
                name: "CategoryId",
                table: "OpenSpecifications");

            migrationBuilder.DropColumn(
                name: "Id",
                table: "Filters");

            migrationBuilder.DropColumn(
                name: "CategoryId",
                table: "Filters");

            migrationBuilder.DropColumn(
                name: "SpecificationId",
                table: "Filters");

            migrationBuilder.DropColumn(
                name: "Id",
                table: "ClosedSpecificationValues");

            migrationBuilder.DropColumn(
                name: "SpecificationId",
                table: "ClosedSpecificationValues");

            migrationBuilder.DropColumn(
                name: "ClosedSpecificationValuesId",
                table: "ClosedSpecificationValueProductVersion");

            migrationBuilder.DropColumn(
                name: "Id",
                table: "ClosedSpecifications");

            migrationBuilder.DropColumn(
                name: "CategoryId",
                table: "ClosedSpecifications");

            migrationBuilder.DropColumn(
                name: "Id",
                table: "Categories");

            migrationBuilder.AddColumn<string>(
                name: "CategoryName",
                table: "Products",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "MainCategoryName",
                table: "Products",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "SubcategoryName",
                table: "Products",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "SpecificationName",
                table: "OpenSpecificationValues",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "SubcategoryName",
                table: "OpenSpecificationValues",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "CategoryName",
                table: "OpenSpecificationValues",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "MainCategoryName",
                table: "OpenSpecificationValues",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "SubcategoryName",
                table: "OpenSpecifications",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "CategoryName",
                table: "OpenSpecifications",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "MainCategoryName",
                table: "OpenSpecifications",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "ClosedSpecificationName",
                table: "Filters",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "SubcategoryName",
                table: "Filters",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "CategoryName",
                table: "Filters",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "MainCategoryName",
                table: "Filters",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "SpecificationName",
                table: "ClosedSpecificationValues",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "SubcategoryName",
                table: "ClosedSpecificationValues",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "CategoryName",
                table: "ClosedSpecificationValues",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "MainCategoryName",
                table: "ClosedSpecificationValues",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "ClosedSpecificationValuesValue",
                table: "ClosedSpecificationValueProductVersion",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "ClosedSpecificationValuesSpecificationName",
                table: "ClosedSpecificationValueProductVersion",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "ClosedSpecificationValuesSubcategoryName",
                table: "ClosedSpecificationValueProductVersion",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "ClosedSpecificationValuesCategoryName",
                table: "ClosedSpecificationValueProductVersion",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "ClosedSpecificationValuesMainCategoryName",
                table: "ClosedSpecificationValueProductVersion",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "SubcategoryName",
                table: "ClosedSpecifications",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "CategoryName",
                table: "ClosedSpecifications",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "MainCategoryName",
                table: "ClosedSpecifications",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "MainCategoryName",
                table: "Categories",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddPrimaryKey(
                name: "PK_OpenSpecificationValues",
                table: "OpenSpecificationValues",
                columns: new[] { "ProductId", "ProductVersionNumber", "SpecificationName", "SubcategoryName", "CategoryName", "MainCategoryName" });

            migrationBuilder.AddPrimaryKey(
                name: "PK_OpenSpecifications",
                table: "OpenSpecifications",
                columns: new[] { "Name", "SubcategoryName", "CategoryName", "MainCategoryName" });

            migrationBuilder.AddPrimaryKey(
                name: "PK_Filters",
                table: "Filters",
                columns: new[] { "ClosedSpecificationName", "SubcategoryName", "CategoryName", "MainCategoryName" });

            migrationBuilder.AddPrimaryKey(
                name: "PK_ClosedSpecificationValues",
                table: "ClosedSpecificationValues",
                columns: new[] { "Value", "SpecificationName", "SubcategoryName", "CategoryName", "MainCategoryName" });

            migrationBuilder.AddPrimaryKey(
                name: "PK_ClosedSpecificationValueProductVersion",
                table: "ClosedSpecificationValueProductVersion",
                columns: new[] { "ProductVersionsVersionNumber", "ProductVersionsProductId", "ClosedSpecificationValuesValue", "ClosedSpecificationValuesSpecificationName", "ClosedSpecificationValuesSubcategoryName", "ClosedSpecificationValuesCategoryName", "ClosedSpecificationValuesMainCategoryName" });

            migrationBuilder.AddPrimaryKey(
                name: "PK_ClosedSpecifications",
                table: "ClosedSpecifications",
                columns: new[] { "Name", "SubcategoryName", "CategoryName", "MainCategoryName" });

            migrationBuilder.AddPrimaryKey(
                name: "PK_Categories",
                table: "Categories",
                columns: new[] { "Name", "MainCategoryName" });

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

            migrationBuilder.UpdateData(
                table: "AspNetRoleClaims",
                keyColumn: "Id",
                keyValue: 7,
                column: "ClaimValue",
                value: "Controller.Manage");

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
                name: "IX_Products_SubcategoryName_CategoryName_MainCategoryName",
                table: "Products",
                columns: new[] { "SubcategoryName", "CategoryName", "MainCategoryName" });

            migrationBuilder.CreateIndex(
                name: "IX_OpenSpecificationValues_SpecificationName_SubcategoryName_C~",
                table: "OpenSpecificationValues",
                columns: new[] { "SpecificationName", "SubcategoryName", "CategoryName", "MainCategoryName" });

            migrationBuilder.CreateIndex(
                name: "IX_OpenSpecifications_SubcategoryName_CategoryName_MainCategor~",
                table: "OpenSpecifications",
                columns: new[] { "SubcategoryName", "CategoryName", "MainCategoryName" });

            migrationBuilder.CreateIndex(
                name: "IX_Filters_SubcategoryName_CategoryName_MainCategoryName",
                table: "Filters",
                columns: new[] { "SubcategoryName", "CategoryName", "MainCategoryName" });

            migrationBuilder.CreateIndex(
                name: "IX_ClosedSpecificationValues_SpecificationName_SubcategoryName~",
                table: "ClosedSpecificationValues",
                columns: new[] { "SpecificationName", "SubcategoryName", "CategoryName", "MainCategoryName" });

            migrationBuilder.CreateIndex(
                name: "IX_ClosedSpecificationValueProductVersion_ClosedSpecificationV~",
                table: "ClosedSpecificationValueProductVersion",
                columns: new[] { "ClosedSpecificationValuesValue", "ClosedSpecificationValuesSpecificationName", "ClosedSpecificationValuesSubcategoryName", "ClosedSpecificationValuesCategoryName", "ClosedSpecificationValuesMainCategoryName" });

            migrationBuilder.CreateIndex(
                name: "IX_ClosedSpecifications_SubcategoryName_CategoryName_MainCateg~",
                table: "ClosedSpecifications",
                columns: new[] { "SubcategoryName", "CategoryName", "MainCategoryName" });

            migrationBuilder.CreateIndex(
                name: "IX_Categories_MainCategoryName",
                table: "Categories",
                column: "MainCategoryName");

            migrationBuilder.CreateIndex(
                name: "IX_Subcategories_CategoryName_MainCategoryName",
                table: "Subcategories",
                columns: new[] { "CategoryName", "MainCategoryName" });

            migrationBuilder.AddForeignKey(
                name: "FK_Categories_MainCategories_MainCategoryName",
                table: "Categories",
                column: "MainCategoryName",
                principalTable: "MainCategories",
                principalColumn: "Name",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ClosedSpecifications_Subcategories_SubcategoryName_Category~",
                table: "ClosedSpecifications",
                columns: new[] { "SubcategoryName", "CategoryName", "MainCategoryName" },
                principalTable: "Subcategories",
                principalColumns: new[] { "Name", "CategoryName", "MainCategoryName" },
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ClosedSpecificationValueProductVersion_ClosedSpecificationV~",
                table: "ClosedSpecificationValueProductVersion",
                columns: new[] { "ClosedSpecificationValuesValue", "ClosedSpecificationValuesSpecificationName", "ClosedSpecificationValuesSubcategoryName", "ClosedSpecificationValuesCategoryName", "ClosedSpecificationValuesMainCategoryName" },
                principalTable: "ClosedSpecificationValues",
                principalColumns: new[] { "Value", "SpecificationName", "SubcategoryName", "CategoryName", "MainCategoryName" },
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ClosedSpecificationValues_ClosedSpecifications_Specificatio~",
                table: "ClosedSpecificationValues",
                columns: new[] { "SpecificationName", "SubcategoryName", "CategoryName", "MainCategoryName" },
                principalTable: "ClosedSpecifications",
                principalColumns: new[] { "Name", "SubcategoryName", "CategoryName", "MainCategoryName" },
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Filters_ClosedSpecifications_ClosedSpecificationName_Subcat~",
                table: "Filters",
                columns: new[] { "ClosedSpecificationName", "SubcategoryName", "CategoryName", "MainCategoryName" },
                principalTable: "ClosedSpecifications",
                principalColumns: new[] { "Name", "SubcategoryName", "CategoryName", "MainCategoryName" },
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Filters_Subcategories_SubcategoryName_CategoryName_MainCate~",
                table: "Filters",
                columns: new[] { "SubcategoryName", "CategoryName", "MainCategoryName" },
                principalTable: "Subcategories",
                principalColumns: new[] { "Name", "CategoryName", "MainCategoryName" },
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_OpenSpecifications_Subcategories_SubcategoryName_CategoryNa~",
                table: "OpenSpecifications",
                columns: new[] { "SubcategoryName", "CategoryName", "MainCategoryName" },
                principalTable: "Subcategories",
                principalColumns: new[] { "Name", "CategoryName", "MainCategoryName" },
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_OpenSpecificationValues_OpenSpecifications_SpecificationNam~",
                table: "OpenSpecificationValues",
                columns: new[] { "SpecificationName", "SubcategoryName", "CategoryName", "MainCategoryName" },
                principalTable: "OpenSpecifications",
                principalColumns: new[] { "Name", "SubcategoryName", "CategoryName", "MainCategoryName" },
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Products_Subcategories_SubcategoryName_CategoryName_MainCat~",
                table: "Products",
                columns: new[] { "SubcategoryName", "CategoryName", "MainCategoryName" },
                principalTable: "Subcategories",
                principalColumns: new[] { "Name", "CategoryName", "MainCategoryName" },
                onDelete: ReferentialAction.Cascade);
        }
    }
}
