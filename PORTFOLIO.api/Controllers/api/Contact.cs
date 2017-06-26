using Microsoft.AspNetCore.Mvc;
using PORTFOLIO.api.Data;
using System.Threading.Tasks;

namespace PORTFOLIO.api.Controllers.api
{
    [Route("api/contact")]
    public class Contact : Controller
    {
        private readonly ApplicationDbContext context;

        public Contact(ApplicationDbContext context)
        {
            this.context = context;
        }

        [HttpPost]
        public async Task<IActionResult> PostAsync(Models.Contact contactData)
        {
            context.Contacts.Add(contactData);
            await context.SaveChangesAsync();

            //TODO: Send email

            return Ok();
        }
    }
}
