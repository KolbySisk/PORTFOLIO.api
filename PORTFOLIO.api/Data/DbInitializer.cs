using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PORTFOLIO.api.Data
{
    public class DbInitializer
    {
        public static void Initialize(ApplicationDbContext context)
        {
            context.Database.EnsureCreated();

            if (context.Stuff.Any()) return;
        }
    }
}
