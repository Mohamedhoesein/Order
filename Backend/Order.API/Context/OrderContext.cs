using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Order.API.Context
{
    /// <summary>
    /// The context used for database access.
    /// </summary>
    public class OrderContext : IdentityDbContext<User, Role, int, UserClaim, UserRole, UserLogin, RoleClaim, UserToken>
    {
        public DbSet<Category> Categories { get; set; }
        public DbSet<ClosedSpecification> ClosedSpecifications { get; set; }
        public DbSet<ClosedSpecificationValue> ClosedSpecificationValues { get; set; }
        public DbSet<Filter> Filters { get; set; }
        public DbSet<OpenSpecification> OpenSpecifications { get; set; }
        public DbSet<OpenSpecificationValue> OpenSpecificationValues { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<ProductVersion> ProductVersions { get; set; }
        public DbSet<ProductImage> ProductImages { get; set; }

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
            builder.Entity<Category>()
                .HasKey(category => category.Id);
            builder.Entity<Category>()
                .Property(category => category.Id)
                .ValueGeneratedOnAdd();
            builder.Entity<ClosedSpecification>()
                .HasKey(closedSpecification => closedSpecification.Id);
            builder.Entity<ClosedSpecificationValue>()
                .HasKey(closedSpecificationValue => closedSpecificationValue.Id);
            builder.Entity<Filter>()
                .HasKey(filter => filter.Id);
            builder.Entity<OpenSpecification>()
                .HasKey(openSpecification => openSpecification.Id);
            builder.Entity<OpenSpecificationValue>()
                .HasKey(openSpecificationValue => openSpecificationValue.Id);
            builder.Entity<Product>()
                .HasKey(product => product.Id);
            builder.Entity<Product>()
                .Property(product => product.Id)
                .UseIdentityAlwaysColumn();
            builder.Entity<ProductVersion>()
                .HasKey(productVersion => new
                {
                    productVersion.VersionNumber,
                    productVersion.ProductId
                });
            builder.Entity<ProductImage>()
                .HasKey(productImage => new
                {
                    productImage.Name,
                    productImage.ProductVersionNumber,
                    productImage.ProductId
                });
            builder.Entity<Category>()
                .Property(category => category.Deleted)
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
            builder.Entity<Category>()
                .HasMany<ClosedSpecification>(category => category.ClosedSpecifications)
                .WithOne(closedSpecification => closedSpecification.Category)
                .HasForeignKey(closedSpecification => closedSpecification.CategoryId)
                .HasPrincipalKey(category => category.Id);
            builder.Entity<Category>()
                .HasMany<OpenSpecification>(category => category.OpenSpecifications)
                .WithOne(openSpecification => openSpecification.Category)
                .HasForeignKey(openSpecification => openSpecification.CategoryId)
                .HasPrincipalKey(category => category.Id);
            builder.Entity<Category>()
                .HasMany<Filter>(category => category.Filters)
                .WithOne(filter => filter.Category)
                .HasForeignKey(filter => filter.CategoryId)
                .HasPrincipalKey(category => category.Id);
            builder.Entity<Category>()
                .HasMany<Product>(category => category.Products)
                .WithOne(product => product.Category)
                .HasForeignKey(product => product.CategoryId)
                .HasPrincipalKey(category => category.Id);
            builder.Entity<ClosedSpecification>()
                .HasMany<ClosedSpecificationValue>(closedSpecification => closedSpecification.ClosedSpecificationValues)
                .WithOne(closedSpecificationValue => closedSpecificationValue.ClosedSpecification)
                .HasForeignKey(closedSpecificationValue => closedSpecificationValue.SpecificationId)
                .HasPrincipalKey(closedSpecification => closedSpecification.Id);
            builder.Entity<ClosedSpecificationValue>()
                .HasMany<ProductVersion>(closedSpecificationValue => closedSpecificationValue.ProductVersions)
                .WithMany(product => product.ClosedSpecificationValues);
            builder.Entity<ClosedSpecification>()
                .HasOne<Filter>(closedSpecification => closedSpecification.Filter)
                .WithOne(filter => filter.ClosedSpecification)
                .HasForeignKey<Filter>(filter => filter.SpecificationId)
                .HasPrincipalKey<ClosedSpecification>(closedSpecification => closedSpecification.Id);
            builder.Entity<OpenSpecification>()
                .HasMany<OpenSpecificationValue>(openSpecification => openSpecification.OpenSpecificationValues)
                .WithOne(openSpecificationValue => openSpecificationValue.OpenSpecification)
                .HasForeignKey(openSpecificationValue => openSpecificationValue.Id)
                .HasPrincipalKey(openSpecification => openSpecification.Id);
            builder.Entity<ProductVersion>()
                .HasMany<OpenSpecificationValue>(product => product.OpenSpecificationValues)
                .WithOne(openSpecificationValue => openSpecificationValue.ProductVersion)
                .HasForeignKey(openSpecificationValue => new
                {
                    Id = openSpecificationValue.ProductId,
                    VersionNumber = openSpecificationValue.ProductVersionNumber
                })
                .HasPrincipalKey(product => new
                {
                    Id = product.ProductId,
                    product.VersionNumber
                });
            builder.Entity<Product>()
                .HasMany<ProductVersion>(product => product.ProductVersions)
                .WithOne(productVersion => productVersion.Product)
                .HasForeignKey(productVersion => productVersion.ProductId)
                .HasPrincipalKey(product => product.Id);
            builder.Entity<ProductVersion>()
                .HasMany<ProductImage>(productVersion => productVersion.ProductImages)
                .WithOne(productImage => productImage.ProductVersion)
                .HasForeignKey(productImage => new
                {
                    productImage.ProductId,
                    VersionNumber = productImage.ProductVersionNumber
                })
                .HasPrincipalKey(productVersion => new
                {
                    productVersion.ProductId,
                    productVersion.VersionNumber
                });

            base.OnModelCreating(builder);
        }
    }
}

