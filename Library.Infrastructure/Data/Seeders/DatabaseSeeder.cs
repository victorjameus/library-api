using Library.Domain.Entities;

namespace Library.Infrastructure.Data.Seeders
{
    public static class DatabaseSeeder
    {
        public static async Task SeedAsync(ApplicationDbContext context)
        {
            await SeedAuthorsAsync(context);
            await SeedBooksAsync(context);
            await SeedMembersAsync(context);
        }

        private static async Task SeedAuthorsAsync(ApplicationDbContext context)
        {
            if (await context.Authors.AnyAsync())
            {
                return;
            }

            var authors = new List<Author>
            {
                new()
                {
                    FirstName = "Gabriel",
                    LastName = "García Márquez",
                    DateOfBirth = new DateTime(1927, 3, 6),
                    Nationality = "Colombiana",
                    Biography = "Escritor, novelista, cuentista, guionista, editor y periodista colombiano."
                },
                new()
                {
                    FirstName = "Mario",
                    LastName = "Vargas Llosa",
                    DateOfBirth = new DateTime(1936, 3, 28),
                    Nationality = "Peruana",
                    Biography = "Escritor, político y periodista peruano."
                },
                new()
                {
                    FirstName = "Isabel",
                    LastName = "Allende",
                    DateOfBirth = new DateTime(1942, 8, 2),
                    Nationality = "Chilena",
                    Biography = "Escritora chilena, considerada la escritora viva más leída del mundo en lengua española."
                }
            };

            context.Authors.AddRange(authors);

            await context.SaveChangesAsync();
        }

        private static async Task SeedBooksAsync(ApplicationDbContext context)
        {
            if (await context.Books.AnyAsync())
            {
                return;
            }

            var authors = await context.Authors.ToListAsync();
            var authorGarcia = authors.First(a => a.LastName == "García Márquez");
            var authorVargas = authors.First(a => a.LastName == "Vargas Llosa");

            var books = new List<Book>
            {
                new()
                {
                    Title = "Cien años de soledad",
                    ISBN = "9788437604947",
                    AuthorId = authorGarcia.Id,
                    PublishDate = new DateTime(1967, 5, 30),
                    Genre = "Realismo mágico",
                    Pages = 471,
                    Publisher = "Editorial Sudamericana",
                    Description = "La obra maestra de García Márquez"
                },
                new()
                {
                    Title = "La ciudad y los perros",
                    ISBN = "9788437604954",
                    AuthorId = authorVargas.Id,
                    PublishDate = new DateTime(1963, 1, 1),
                    Genre = "Novela",
                    Pages = 419,
                    Publisher = "Seix Barral",
                    Description = "Primera novela de Mario Vargas Llosa"
                }
            };

            context.Books.AddRange(books);

            await context.SaveChangesAsync();
        }

        private static async Task SeedMembersAsync(ApplicationDbContext context)
        {
            if (await context.Members.AnyAsync())
            {
                return;
            }

            var members = new List<Member>
            {
                new()
                {
                    FirstName = "Juan",
                    LastName = "Pérez",
                    Email = "juan.perez@email.com",
                    Phone = "+1234567890",
                    Address = "Calle 123, Ciudad"
                },
                new()
                {
                    FirstName = "María",
                    LastName = "González",
                    Email = "maria.gonzalez@email.com",
                    Phone = "+0987654321",
                    Address = "Avenida 456, Ciudad"
                }
            };

            context.Members.AddRange(members);

            await context.SaveChangesAsync();
        }
    }
}
