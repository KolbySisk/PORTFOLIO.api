﻿using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PORTFOLIO.api.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PORTFOLIO.api.Controllers.api
{
    [Route("api/stuff")]
    [ResponseCache(Duration = 7200)]
    public class Stuff : Controller
    {
        private readonly ApplicationDbContext _context;

        public Stuff(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> GetAsync()
        {
            var stuff = await _context.Stuff.ToListAsync();
            return Ok(new JsonResult(stuff).Value);
        }
    }
}
