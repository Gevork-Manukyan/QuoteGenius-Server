using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using QuoteModel;

using System;
using System.Collections.Generic;
using System.Threading.Tasks;


namespace QuoteGenius_Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SeedController : ControllerBase
    {
        private readonly UserManager<QuoteGeniusUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IConfiguration _configuration;
        private readonly QuoteGeniusContext _context;

        public SeedController(UserManager<QuoteGeniusUser> userManager, RoleManager<IdentityRole> roleManager,
            IConfiguration configuration, QuoteGeniusContext context)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _configuration = configuration;
            _context = context;
        }


        [HttpPost("Users")]
        public async Task<IActionResult> ImportUsers()
        {
            const string roleUser = "RegisteredUser";
            const string roleAdmin = "Administrator";

            if (await _roleManager.FindByNameAsync(roleUser) is null)
            {
                await _roleManager.CreateAsync(new IdentityRole(roleUser));
            }
            if (await _roleManager.FindByNameAsync(roleAdmin) is null)
            {
                await _roleManager.CreateAsync(new IdentityRole(roleAdmin));
            }

            List<QuoteGeniusUser> addedUserList = new();
            (string name, string email) = ("admin", "admin@email.com");

            if (await _userManager.FindByNameAsync(name) is null)
            {
                QuoteGeniusUser userAdmin = new()
                {
                    UserName = name,
                    Email = email,
                    SecurityStamp = Guid.NewGuid().ToString()
                };
                await _userManager.CreateAsync(userAdmin, _configuration["DefaultPasswords:Administrator"]
                    ?? throw new InvalidOperationException());
                await _userManager.AddToRolesAsync(userAdmin, new[] { roleUser, roleAdmin });
                userAdmin.EmailConfirmed = true;
                userAdmin.LockoutEnabled = false;
                addedUserList.Add(userAdmin);
            }

            (string name, string email) registered = ("user", "user@email.com");

            if (await _userManager.FindByNameAsync(registered.name) is null)
            {
                QuoteGeniusUser user = new()
                {
                    UserName = registered.name,
                    Email = registered.email,
                    SecurityStamp = Guid.NewGuid().ToString()
                };
                await _userManager.CreateAsync(user, _configuration["DefaultPasswords:RegisteredUser"]
                    ?? throw new InvalidOperationException());
                await _userManager.AddToRoleAsync(user, roleUser);
                user.EmailConfirmed = true;
                user.LockoutEnabled = false;
                addedUserList.Add(user);
            }

            if (addedUserList.Count > 0)
            {
                await _context.SaveChangesAsync();
            }

            // Seed quotes and authors
            await ImportQuotesAndAuthors();

            return new JsonResult(new
            {
                addedUserList.Count,
                Users = addedUserList
            });
        }

        [HttpPost("QuotesAndAuthors")]
        public async Task<IActionResult> ImportQuotesAndAuthors()
        {
            // Create a list to store added quotes
            List<Quote> addedQuoteList = new List<Quote>();

            // Create authors
            Author author1 = new Author
            {
                Name = "Author 1",
                Birthday = new DateTime(1990, 1, 1),
                Race = "Race 1",
                Gender = "Male"
            };
            _context.Authors.Add(author1);

            Author author2 = new Author
            {
                Name = "Author 2",
                Birthday = new DateTime(1995, 2, 2),
                Race = "Race 2",
                Gender = "Female"
            };
            _context.Authors.Add(author2);

            Author author3 = new Author
            {
                Name = "Author 3",
                Birthday = new DateTime(1985, 3, 3),
                Race = "Race 3",
                Gender = "Female"
            };
            _context.Authors.Add(author3);

            Author author4 = new Author
            {
                Name = "Author 4",
                Birthday = new DateTime(1980, 4, 4),
                Race = "Race 4",
                Gender = "Male"
            };
            _context.Authors.Add(author4);

            Author author5 = new Author
            {
                Name = "Author 5",
                Birthday = new DateTime(1992, 5, 5),
                Race = "Race 5",
                Gender = "Male"
            };
            _context.Authors.Add(author5);

            await _context.SaveChangesAsync();

            // Create quotes
            Quote quote1 = new Quote
            {
                Text = "Quote 1",
                DatePublished = new DateTime(2023, 1, 1),
                AuthorId = author1.Id
            };
            _context.Quotes.Add(quote1);
            addedQuoteList.Add(quote1);

            Quote quote2 = new Quote
            {
                Text = "Quote 2",
                DatePublished = new DateTime(2023, 2, 2),
                AuthorId = author2.Id
            };
            _context.Quotes.Add(quote2);
            addedQuoteList.Add(quote2);

            Quote quote3 = new Quote
            {
                Text = "Quote 3",
                DatePublished = new DateTime(2023, 3, 3),
                AuthorId = author3.Id
            };
            _context.Quotes.Add(quote3);
            addedQuoteList.Add(quote3);

            Quote quote4 = new Quote
            {
                Text = "Quote 4",
                DatePublished = new DateTime(2023, 4, 4),
                AuthorId = author4.Id
            };
            _context.Quotes.Add(quote4);
            addedQuoteList.Add(quote4);

            Quote quote5 = new Quote
            {
                Text = "Quote 5",
                DatePublished = new DateTime(2023, 5, 5),
                AuthorId = author5.Id
            };
            _context.Quotes.Add(quote5);
            addedQuoteList.Add(quote5);

            await _context.SaveChangesAsync();

            return new JsonResult(new
            {
                addedQuoteList.Count,
                Quotes = addedQuoteList
            });
        }

    }

}
