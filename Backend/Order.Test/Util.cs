using System.Collections.Generic;
using System.IO;
using System.Linq;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.Extensions.Configuration;
using MimeKit;
using Order.API;
using Order.API.Context;
using Order.API.Controllers.CategoryController.Models;
using ClosedSpecification = Order.API.Context.ClosedSpecification;
using ClosedSpecificationValue = Order.API.Context.ClosedSpecificationValue;
using OpenSpecification = Order.API.Context.OpenSpecification;

namespace Order.Test
{
    /// <summary>
    /// A utility class for testing.
    /// </summary>
    public static class Util
    {
        public static WholeSubcategory NewSubcategory = new(new Subcategory
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

        public static WholeSubcategory UpdatedSubcategory = new(new Subcategory
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
                    .ConfigureAppConfiguration((_, builder) => builder.AddJsonFile("appsettings.Test.json"))
                    .UseStartup<Startup>()
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
                
                var directories = new DirectoryInfo("./emails")
                    .GetDirectories();
                foreach (var directory in directories)
                {
                    directory.Delete(true);
                }
            }
            catch
            {
                // ignored
            }
        }
    }
}