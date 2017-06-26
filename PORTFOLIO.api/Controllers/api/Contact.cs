using Microsoft.AspNetCore.Mvc;
using PORTFOLIO.api.Data;
using System.Threading.Tasks;

namespace PORTFOLIO.api.Controllers.api
{
    public class Contact : Controller
    {
        private readonly ApplicationDbContext _context;

        public Contact(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpPost]
        public async Task<IActionResult> PostAsync(Contact contactData)
        {
            _context.Add(contactData);
            await _context.SaveChangesAsync();

            //TODO: Send email

            return Ok();
        }
    }
}
