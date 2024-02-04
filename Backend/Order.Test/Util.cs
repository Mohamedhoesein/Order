using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using MimeKit;
using Order.API;
using Order.API.Context;
using Order.API.Controllers.CategoryController.Models;
using Order.API.Controllers.ProductController.Models.Receive;
using Order.Test.CookieHttpClient;
using ClosedSpecification = Order.API.Context.ClosedSpecification;
using ClosedSpecificationValue = Order.API.Context.ClosedSpecificationValue;
using OpenSpecification = Order.API.Context.OpenSpecification;
using OpenSpecificationValue = Order.API.Controllers.ProductController.Models.Receive.OpenSpecificationValue;
using Product = Order.API.Controllers.ProductController.Models.Receive.Product;

namespace Order.Test
{
    /// <summary>
    /// A utility class for testing.
    /// </summary>
    public static class Util
    {
        /// <summary>
        /// A subcategory used by some tests when updating a subcategory.
        /// </summary>
        public static readonly WholeSubcategory NewSubcategory = new(new Subcategory
            {
                Name = "TestSubcategory",
                Deleted = false,
                OpenSpecifications = new List<OpenSpecification>
                {
                    new()
                    {
                        Name = "OpenSpecification1",
                        Deleted = false
                    },
                    new()
                    {
                        Name = "OpenSpecification2",
                        Deleted = false
                    },
                    new()
                    {
                        Name = "OpenSpecification3",
                        Deleted = false
                    }
                },
                ClosedSpecifications = new List<ClosedSpecification>
                {
                    new()
                    {
                        Name = "ClosedSpecification1",
                        Deleted = false,
                        ClosedSpecificationValues = new List<ClosedSpecificationValue>
                        {
                            new()
                            {
                                Deleted = false,
                                Value = "Value1"
                            },
                            new()
                            {
                                Deleted = false,
                                Value = "Value2"
                            },
                            new()
                            {
                                Deleted = false,
                                Value = "Value3"
                            }
                        },
                        Filter = new Filter
                        {
                            Title = "Title"
                        }
                    },
                     new()
                     {
                         Name = "ClosedSpecification2",
                         Deleted = false,
                         ClosedSpecificationValues = new List<ClosedSpecificationValue>()
                     },
                     new()
                     {
                         Name = "ClosedSpecification3",
                         Deleted = false,
                         ClosedSpecificationValues = new List<ClosedSpecificationValue>()
                     },
                     new()
                     {
                         Name = "ClosedSpecification4",
                         Deleted = false,
                         ClosedSpecificationValues = new List<ClosedSpecificationValue>()
                     }
                }
            });

        /// <summary>
        /// A subcategory used by some tests when updating a subcategory.
        /// </summary>
        public static readonly WholeSubcategory SimpleSubcategory = new(new Subcategory
            {
                Name = "TestSubcategory",
                Deleted = false,
                OpenSpecifications = new List<OpenSpecification>
                {
                    new()
                    {
                        Name = "OpenSpecification1",
                        Deleted = false
                    }
                },
                ClosedSpecifications = new List<ClosedSpecification>
                {
                    new()
                    {
                        Name = "ClosedSpecification1",
                        Deleted = false,
                        ClosedSpecificationValues = new List<ClosedSpecificationValue>
                        {
                            new()
                            {
                                Deleted = false,
                                Value = "Value1"
                            },
                            new()
                            {
                                Deleted = false,
                                Value = "Value2"
                            },
                            new()
                            {
                                Deleted = false,
                                Value = "Value3"
                            }
                        },
                        Filter = new Filter
                        {
                            Title = "Title"
                        }
                    },
                    new()
                    {
                        Name = "ClosedSpecification2",
                        Deleted = false,
                        ClosedSpecificationValues = new List<ClosedSpecificationValue>
                        {
                            new()
                            {
                                Deleted = false,
                                Value = "Value4"
                            },
                            new()
                            {
                                Deleted = false,
                                Value = "Value5"
                            },
                            new()
                            {
                                Deleted = false,
                                Value = "Value6"
                            }
                        }
                    }
                }
            });

        /// <summary>
        /// A subcategory used by some tests when updating a subcategory.
        /// </summary>
        public static readonly WholeSubcategory UpdatedSubcategory = new(new Subcategory
            {
                Name = "TestSubcategory",
                Deleted = false,
                OpenSpecifications = new List<OpenSpecification>
                {
                    new()
                    {
                        Name = "OpenSpecification1",
                        Deleted = false
                    },
                    new()
                    {
                        Name = "OpenSpecification2",
                        Deleted = true
                    },
                },
                ClosedSpecifications = new List<ClosedSpecification>
                {
                    new()
                    {
                        Name = "ClosedSpecification1",
                        Deleted = false,
                        ClosedSpecificationValues = new List<ClosedSpecificationValue>
                        {
                            new()
                            {
                                Deleted = false,
                                Value = "Value1"
                            },
                            new()
                            {
                                Deleted = true,
                                Value = "Value2"
                            }
                        },
                        Filter = null
                    },
                    new()
                    {
                        Name = "ClosedSpecification2",
                        Deleted = true,
                        ClosedSpecificationValues = new List<ClosedSpecificationValue>()
                    },
                    new()
                    {
                        Name = "ClosedSpecification4",
                        Deleted = false,
                        ClosedSpecificationValues = new List<ClosedSpecificationValue>(),
                        Filter = new Filter
                        {
                            Title = "Filter"
                        }
                    }
                }
            });

        /// <summary>
        /// A product used by some tests when creating a product, assumes the specifications in <see cref="SimpleSubcategory"/> exists.
        /// </summary>
        public static readonly NewProduct Product = new()
        {
            Name = "Product",
            Deleted = false,
            Price = 1,
            Description = "Description",
            ClosedSpecificationValues = new []
            {
                new API.Controllers.ProductController.Models.Receive.ClosedSpecificationValue
                {
                    Specification = "ClosedSpecification1",
                    Value = "Value1"
                },
                new API.Controllers.ProductController.Models.Receive.ClosedSpecificationValue
                {
                    Specification = "ClosedSpecification2",
                    Value = "Value6"
                }
            },
            OpenSpecificationValues = new []
            {
                new OpenSpecificationValue
                {
                    Specification = "OpenSpecification1",
                    Value = "TEST"
                }
            }
        };

        /// <summary>
        /// A product used by some tests when creating a product with an extra closed specification,
        /// assumes the specifications in <see cref="SimpleSubcategory"/> exists.
        /// </summary>
        public static readonly NewProduct ProductExtraClosed = new()
        {
            Name = "Product",
            Deleted = false,
            Price = 1,
            Description = "Description",
            ClosedSpecificationValues = new []
            {
                new API.Controllers.ProductController.Models.Receive.ClosedSpecificationValue
                {
                    Specification = "ClosedSpecification1",
                    Value = "Value1"
                },
                new API.Controllers.ProductController.Models.Receive.ClosedSpecificationValue
                {
                    Specification = "ClosedSpecification2",
                    Value = "Value6"
                },
                new API.Controllers.ProductController.Models.Receive.ClosedSpecificationValue
                {
                    Specification = "TEST",
                    Value = "TEST"
                }
            },
            OpenSpecificationValues = new []
            {
                new OpenSpecificationValue
                {
                    Specification = "OpenSpecification1",
                    Value = "TEST"
                }
            }
        };

        /// <summary>
        /// A product used by some tests when creating a product with an invalid closed specification value,
        /// assumes the specifications in <see cref="SimpleSubcategory"/> exists.
        /// </summary>
        public static readonly NewProduct ProductInvalidClosed = new()
        {
            Name = "Product",
            Deleted = false,
            Price = 1,
            Description = "Description",
            ClosedSpecificationValues = new []
            {
                new API.Controllers.ProductController.Models.Receive.ClosedSpecificationValue
                {
                    Specification = "ClosedSpecification1",
                    Value = "Value1"
                },
                new API.Controllers.ProductController.Models.Receive.ClosedSpecificationValue
                {
                    Specification = "ClosedSpecification2",
                    Value = "TEST"
                }
            },
            OpenSpecificationValues = new []
            {
                new OpenSpecificationValue
                {
                    Specification = "OpenSpecification1",
                    Value = "TEST"
                }
            }
        };

        /// <summary>
        /// A product used by some tests when creating a product with an extra open specification value,
        /// assumes the specifications in <see cref="SimpleSubcategory"/> exists.
        /// </summary>
        public static readonly NewProduct ProductExtraOpen = new()
        {
            Name = "Product",
            Deleted = false,
            Price = 1,
            Description = "Description",
            ClosedSpecificationValues = new []
            {
                new API.Controllers.ProductController.Models.Receive.ClosedSpecificationValue
                {
                    Specification = "ClosedSpecification1",
                    Value = "Value1"
                },
                new API.Controllers.ProductController.Models.Receive.ClosedSpecificationValue
                {
                    Specification = "ClosedSpecification2",
                    Value = "Value6"
                },
            },
            OpenSpecificationValues = new []
            {
                new OpenSpecificationValue
                {
                    Specification = "OpenSpecification1",
                    Value = "TEST"
                },
                new OpenSpecificationValue
                {
                    Specification = "TEST",
                    Value = "TEST"
                }
            }
        };

        /// <summary>
        /// A product used by some tests when updating a product, assumes the specifications in <see cref="SimpleSubcategory"/> exists.
        /// </summary>
        public static readonly Product UpdateProduct = new()
        {
            Name = "Product",
            Deleted = false,
            Price = 1,
            Description = "Description",
            ClosedSpecificationValues = new []
            {
                new API.Controllers.ProductController.Models.Receive.ClosedSpecificationValue
                {
                    Specification = "ClosedSpecification1",
                    Value = "Value1"
                },
                new API.Controllers.ProductController.Models.Receive.ClosedSpecificationValue
                {
                    Specification = "ClosedSpecification2",
                    Value = "Value6"
                }
            },
            OpenSpecificationValues = new []
            {
                new OpenSpecificationValue
                {
                    Specification = "OpenSpecification1",
                    Value = "TEST"
                }
            }
        };

        /// <summary>
        /// A product used by some tests when updating a product with an extra closed specification,
        /// assumes the specifications in <see cref="SimpleSubcategory"/> exists.
        /// </summary>
        public static readonly Product UpdateProductExtraClosed = new()
        {
            Name = "Product",
            Deleted = false,
            Price = 1,
            Description = "Description",
            ClosedSpecificationValues = new []
            {
                new API.Controllers.ProductController.Models.Receive.ClosedSpecificationValue
                {
                    Specification = "ClosedSpecification1",
                    Value = "Value1"
                },
                new API.Controllers.ProductController.Models.Receive.ClosedSpecificationValue
                {
                    Specification = "ClosedSpecification2",
                    Value = "Value6"
                },
                new API.Controllers.ProductController.Models.Receive.ClosedSpecificationValue
                {
                    Specification = "TEST",
                    Value = "TEST"
                }
            },
            OpenSpecificationValues = new []
            {
                new OpenSpecificationValue
                {
                    Specification = "OpenSpecification1",
                    Value = "TEST"
                }
            }
        };

        /// <summary>
        /// A product used by some tests when updating a product with an invalid closed specification value, assumes the specifications in <see cref="SimpleSubcategory"/> exists.
        /// </summary>
        public static readonly Product UpdateProductInvalidClosed = new()
        {
            Name = "Product",
            Deleted = false,
            Price = 1,
            Description = "Description",
            ClosedSpecificationValues = new []
            {
                new API.Controllers.ProductController.Models.Receive.ClosedSpecificationValue
                {
                    Specification = "ClosedSpecification1",
                    Value = "Value1"
                },
                new API.Controllers.ProductController.Models.Receive.ClosedSpecificationValue
                {
                    Specification = "ClosedSpecification2",
                    Value = "TEST"
                }
            },
            OpenSpecificationValues = new []
            {
                new OpenSpecificationValue
                {
                    Specification = "OpenSpecification1",
                    Value = "TEST"
                }
            }
        };

        /// <summary>
        /// A product used by some tests when updating a product with an extra open specification,
        /// assumes the specifications in <see cref="SimpleSubcategory"/> exists.
        /// </summary>
        public static readonly Product UpdateProductExtraOpen = new()
        {
            Name = "Product",
            Deleted = false,
            Price = 1,
            Description = "Description",
            ClosedSpecificationValues = new []
            {
                new API.Controllers.ProductController.Models.Receive.ClosedSpecificationValue
                {
                    Specification = "ClosedSpecification1",
                    Value = "Value1"
                },
                new API.Controllers.ProductController.Models.Receive.ClosedSpecificationValue
                {
                    Specification = "ClosedSpecification2",
                    Value = "Value6"
                }
            },
            OpenSpecificationValues = new []
            {
                new OpenSpecificationValue
                {
                    Specification = "OpenSpecification1",
                    Value = "TEST"
                },
                new OpenSpecificationValue
                {
                    Specification = "TEST",
                    Value = "TEST"
                }
            }
        };

        /// <summary>
        /// The default password used for all accounts.
        /// </summary>
        public const string DefaultPassword = "TestPassword123!@#";

        /// <summary>
        /// The password used for updating.
        /// </summary>
        public const string NewPassword = "TestPassword321!@#";

        /// <summary>
        /// Create the default <see cref="TestServer"/> instance.
        /// </summary>
        /// <returns>
        /// The <see cref="TestServer"/> to use during testing.
        /// </returns>
        public static TestServer CreateTestServer()
        {
            return new TestServer(
                new WebHostBuilder()
                    .ConfigureAppConfiguration((_, builder) => builder.AddJsonFile("appsettings.Test.json")
                ).UseStartup<Startup>()
                    .ConfigureLogging(options => options.AddConsole())
            );
        }

        /// <summary>
        /// Initialize the database.
        /// </summary>
        public static void CreateDatabase()
        {
            var connectionString = new ConfigurationBuilder().AddJsonFile("appsettings.Test.json")
                .Build()
                .GetConnectionString("OrderContextConnection");
            var options = new DbContextOptionsBuilder<OrderContext>().UseNpgsql(connectionString).Options;
            new DatabaseFacade(new OrderContext(options)).EnsureCreated();
        }

        /// <summary>
        /// Delete the database.
        /// </summary>
        public static void DeleteDatabase()
        {
            var connectionString = new ConfigurationBuilder().AddJsonFile("appsettings.Test.json")
                .Build()
                .GetConnectionString("OrderContextConnection");
            var options = new DbContextOptionsBuilder<OrderContext>().UseNpgsql(connectionString).Options;
            new DatabaseFacade(new OrderContext(options)).EnsureDeleted();
        }

        /// <summary>
        /// Retrieve the id and code from an email.
        /// </summary>
        /// <param name="name">
        /// The name of the user.
        /// </param>
        /// <param name="email">
        /// The email of the user.
        /// </param>
        /// <returns>
        /// The id and code from the email.
        /// </returns>
        public static (string, string, string) ParseLatestEmail(string name, string email)
        {
            var directory = Path.Combine("./emails", $"{name}.{email.Replace("@", "_at_")}");
            var file = new DirectoryInfo(directory).GetFiles().MaxBy(file => file.LastWriteTime);
            if (file == null)
                return (string.Empty, string.Empty, string.Empty);
            var message = MimeMessage.Load(file.OpenRead());
            var parts = message.TextBody.Split(" ").Last().Split("/").Reverse();
            var enumerable = parts.ToArray();
            var code = enumerable.ElementAt(0);
            var id = enumerable.ElementAt(1);
            var type = enumerable.ElementAt(2);
            return (type, id, code);
        }

        /// <summary>
        /// Delete all files and folders for the emails.
        /// </summary>
        public static void DeleteEmails()
        {
            try
            {
                new DirectoryInfo("./emails").Delete(true);
            }
            catch
            {
                // ignored
            }
        }

        /// <summary>
        /// Delete all files and folders for the images.
        /// </summary>
        public static void DeleteImages()
        {
            try
            {
                new DirectoryInfo("./images").Delete(true);
            }
            catch
            {
                // ignored
            }
        }

        /// <summary>
        /// Load an image based on a <see cref="FileType"/>.
        /// </summary>
        /// <param name="file">
        /// The <see cref="FileType"/> to load.
        /// </param>
        /// <returns>
        /// The binary of the image.
        /// </returns>
        /// <exception cref="ArgumentOutOfRangeException">
        /// An exception thrown when the <see cref="FileType"/> is invalid.
        /// </exception>
        public static byte[] GetImage(FileType file)
        {
            return file switch
            {
                FileType.Dark => System.IO.File.ReadAllBytes("Images/Dark.jpg"),
                FileType.Light => System.IO.File.ReadAllBytes("Images/Light.jpg"),
                FileType.Jellyfish => System.IO.File.ReadAllBytes("Images/Jellyfish.png"),
                _ => throw new ArgumentOutOfRangeException(nameof(file), file, null)
            };
        }
    }
}