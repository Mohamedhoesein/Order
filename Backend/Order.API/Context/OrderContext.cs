using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Order.API.Context
{
    /// <summary>
    /// The context used for database access.
    /// </summary>
    public class OrderContext : IdentityDbContext<User, Role, int, UserClaim, UserRole, UserLogin, RoleClaim, UserToken>
    {
        public DbSet<MainCategory> MainCategories { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Subcategory> Subcategories { get; set; }
        public DbSet<ClosedSpecification> ClosedSpecifications { get; set; }
        public DbSet<ClosedSpecificationValue> ClosedSpecificationValues { get; set; }
        public DbSet<Filter> Filters { get; set; }
        public DbSet<OpenSpecification> OpenSpecifications { get; set; }
        public DbSet<OpenSpecificationValue> OpenSpecificationValues { get; set; }
        public DbSet<Product> Products { get; set; }

        /// <summary>
        /// Initialize a new <see cref="OrderContext"/> with just the options.
        /// </summary>
        /// <param name="options">
        /// The options sued by <see cref="DbContext"/>.
        /// </param>
        public OrderContext(DbContextOptions<OrderContext> options) : base(options) {}

        /// <summary>
        /// Configure the schema for the database.
        /// </summary>
        /// <param name="builder">
        /// The builder used to create the schema.
        /// </param>
        protected override void OnModelCreating(ModelBuilder builder)
        {
            //Auth
            builder.Entity<User>()
                .HasKey(user => user.Id);
            builder.Entity<Role>()
                .HasKey(role => role.Id);
            builder.Entity<RoleClaim>()
                .HasKey(roleClaim => roleClaim.Id);
            builder.Entity<UserClaim>()
                .HasKey(userClaim => userClaim.Id);
            builder.Entity<UserRole>()
                .HasKey(userRole => new {userRole.RoleId, userRole.UserId});
            builder.Entity<User>()
                .HasData(User.GetUsers());
            builder.Entity<Role>()
                .HasData(Role.GetRoles());
            builder.Entity<RoleClaim>()
                .HasData(RoleClaim.GetRoleClaim());
            builder.Entity<UserRole>()
                .HasData(UserRole.GetUserRoles());

            //Products
            builder.Entity<MainCategory>()
                .HasKey(mainCategory => mainCategory.Name);
            builder.Entity<Category>()
                .HasKey(category => new { category.Name, category.MainCategoryName });
            builder.Entity<Subcategory>()
                .HasKey(subCategory => new { subCategory.Name, subCategory.CategoryName, subCategory.MainCategoryName });
            builder.Entity<ClosedSpecification>()
                .HasKey(closedSpecification => new
                {
                    closedSpecification.Name,
                    closedSpecification.SubcategoryName,
                    closedSpecification.CategoryName,
                    closedSpecification.MainCategoryName
                });
            builder.Entity<ClosedSpecificationValue>()
                .HasKey(closedSpecificationValue => new
                {
                    closedSpecificationValue.Value,
                    closedSpecificationValue.SpecificationName,
                    closedSpecificationValue.SubcategoryName,
                    closedSpecificationValue.CategoryName,
                    closedSpecificationValue.MainCategoryName
                });
            builder.Entity<Filter>()
                .HasKey(filter => new
                {
                    filter.ClosedSpecificationName,
                    filter.SubcategoryName,
                    filter.CategoryName,
                    filter.MainCategoryName
                });
            builder.Entity<OpenSpecification>()
                .HasKey(openSpecification => new
                {
                    openSpecification.Name,
                    openSpecification.SubcategoryName,
                    openSpecification.CategoryName,
                    openSpecification.MainCategoryName
                });
            builder.Entity<OpenSpecificationValue>()
                .HasKey(openSpecificationValue => new
                {
                    openSpecificationValue.ProductName,
                    openSpecificationValue.SpecificationName,
                    openSpecificationValue.SubcategoryName,
                    openSpecificationValue.CategoryName,
                    openSpecificationValue.MainCategoryName
                });
            builder.Entity<Product>()
                .HasKey(product => new
                {
                    product.Name,
                    product.SubcategoryName,
                    product.CategoryName,
                    product.MainCategoryName
                });
            builder.Entity<MainCategory>()
                .Property(mainCategory => mainCategory.Deleted)
                .HasDefaultValue(false);
            builder.Entity<Category>()
                .Property(category => category.Deleted)
                .HasDefaultValue(false);
            builder.Entity<Subcategory>()
                .Property(subcategory => subcategory.Deleted)
                .HasDefaultValue(false);
            builder.Entity<Product>()
                .Property(product => product.Deleted)
                .HasDefaultValue(false);
            builder.Entity<ClosedSpecification>()
                .Property(closedSpecification => closedSpecification.Deleted)
                .HasDefaultValue(false);
            builder.Entity<ClosedSpecificationValue>()
                .Property(closedSpecificationValue => closedSpecificationValue.Deleted)
                .HasDefaultValue(false);
            builder.Entity<OpenSpecification>()
                .Property(openSpecification => openSpecification.Deleted)
                .HasDefaultValue(false);
            builder.Entity<MainCategory>()
                .HasMany<Category>(mainCategory => mainCategory.Categories)
                .WithOne(category => category.MainCategory)
                .HasForeignKey(category => category.MainCategoryName)
                .HasPrincipalKey(mainCategory => mainCategory.Name);
            builder.Entity<Category>()
                .HasMany<Subcategory>(category => category.Subcategories)
                .WithOne(subCategory => subCategory.Category)
                .HasForeignKey(subcategory => new
                {
                    Name = subcategory.CategoryName,
                    subcategory.MainCategoryName
                })
                .HasPrincipalKey(category => new
                {
                    category.Name,
                    category.MainCategoryName
                });
            builder.Entity<Subcategory>()
                .HasMany<ClosedSpecification>(subcategory => subcategory.ClosedSpecifications)
                .WithOne(closedSpecification => closedSpecification.Subcategory)
                .HasForeignKey(closedSpecification => new
                {
                    Name = closedSpecification.SubcategoryName,
                    closedSpecification.CategoryName,
                    closedSpecification.MainCategoryName
                })
                .HasPrincipalKey(subcategory => new
                {
                    subcategory.Name,
                    subcategory.CategoryName,
                    subcategory.MainCategoryName
                });
            builder.Entity<Subcategory>()
                .HasMany<OpenSpecification>(subcategory => subcategory.OpenSpecifications)
                .WithOne(openSpecification => openSpecification.Subcategory)
                .HasForeignKey(openSpecification => new
                {
                    Name = openSpecification.SubcategoryName,
                    openSpecification.CategoryName,
                    openSpecification.MainCategoryName
                })
                .HasPrincipalKey(subcategory => new
                {
                    subcategory.Name,
                    subcategory.CategoryName,
                    subcategory.MainCategoryName
                });
            builder.Entity<Subcategory>()
                .HasMany<Filter>(subcategory => subcategory.Filters)
                .WithOne(filter => filter.Subcategory)
                .HasForeignKey(filter => new
                {
                    Name = filter.SubcategoryName,
                    filter.CategoryName,
                    filter.MainCategoryName
                })
                .HasPrincipalKey(subcategory => new
                {
                    subcategory.Name,
                    subcategory.CategoryName,
                    subcategory.MainCategoryName
                });
            builder.Entity<Subcategory>()
                .HasMany<Product>(subcategory => subcategory.Products)
                .WithOne(product => product.Subcategory)
                .HasForeignKey(product => new
                {
                    Name = product.SubcategoryName,
                    product.CategoryName,
                    product.MainCategoryName
                })
                .HasPrincipalKey(subcategory => new
                {
                    subcategory.Name,
                    subcategory.CategoryName,
                    subcategory.MainCategoryName
                });
            builder.Entity<ClosedSpecification>()
                .HasMany<ClosedSpecificationValue>(closedSpecification => closedSpecification.ClosedSpecificationValues)
                .WithOne(closedSpecificationValue => closedSpecificationValue.ClosedSpecification)
                .HasForeignKey(closedSpecificationValue => new
                {
                    Name = closedSpecificationValue.SpecificationName,
                    closedSpecificationValue.SubcategoryName,
                    closedSpecificationValue.CategoryName,
                    closedSpecificationValue.MainCategoryName
                })
                .HasPrincipalKey(closedSpecification => new
                {
                    closedSpecification.Name,
                    closedSpecification.SubcategoryName,
                    closedSpecification.CategoryName,
                    closedSpecification.MainCategoryName
                });
            builder.Entity<ClosedSpecificationValue>()
                .HasMany<Product>(closedSpecificationValue => closedSpecificationValue.Products)
                .WithMany(product => product.ClosedSpecificationValues);
            builder.Entity<ClosedSpecification>()
                .HasOne<Filter>(closedSpecification => closedSpecification.Filter)
                .WithOne(filter => filter.ClosedSpecification)
                .HasForeignKey<Filter>(filter => new
                {
                    Name = filter.ClosedSpecificationName,
                    filter.SubcategoryName,
                    filter.CategoryName,
                    filter.MainCategoryName
                })
                .HasPrincipalKey<ClosedSpecification>(closedSpecification => new
                {
                    closedSpecification.Name,
                    closedSpecification.SubcategoryName,
                    closedSpecification.CategoryName,
                    closedSpecification.MainCategoryName
                });
            builder.Entity<OpenSpecification>()
                .HasMany<OpenSpecificationValue>(openSpecification => openSpecification.OpenSpecificationValues)
                .WithOne(openSpecificationValue => openSpecificationValue.OpenSpecification)
                .HasForeignKey(openSpecificationValue => new
                {
                    Name = openSpecificationValue.SpecificationName,
                    openSpecificationValue.SubcategoryName,
                    openSpecificationValue.CategoryName,
                    openSpecificationValue.MainCategoryName
                })
                .HasPrincipalKey(openSpecification => new
                {
                    openSpecification.Name,
                    openSpecification.SubcategoryName,
                    openSpecification.CategoryName,
                    openSpecification.MainCategoryName
                });
            builder.Entity<Product>()
                .HasMany<OpenSpecificationValue>(product => product.OpenSpecificationValues)
                .WithOne(openSpecificationValue => openSpecificationValue.Product)
                .HasForeignKey(openSpecificationValue => new
                {
                    Name = openSpecificationValue.ProductName,
                    openSpecificationValue.SubcategoryName,
                    openSpecificationValue.CategoryName,
                    openSpecificationValue.MainCategoryName
                })
                .HasPrincipalKey(product => new
                {
                    product.Name,
                    product.SubcategoryName,
                    product.CategoryName,
                    product.MainCategoryName
                });
                
            base.OnModelCreating(builder);
        }
    }
}

