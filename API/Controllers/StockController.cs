using System.Collections.Generic;
using System.Threading.Tasks;
using API.Data;
using API.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers
{
    public class StockController : BaseAPIController
    {
        private readonly DataContext _context;
        public StockController(DataContext context)
        {
            this._context = context;
        }


        [HttpGet]
        [AllowAnonymous]
        public async Task<ActionResult<List<Stock>>> GetAllStocks()
        {
            return await _context.Stocks.ToListAsync();
        }
    }
}