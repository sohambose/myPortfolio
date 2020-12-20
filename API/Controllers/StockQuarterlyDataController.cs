using System.Collections.Generic;
using System.Threading.Tasks;
using API.Data;
using API.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers
{
    public class StockQuarterlyDataController : BaseAPIController
    {
        private readonly DataContext _context;
        public StockQuarterlyDataController(DataContext context)
        {
            this._context = context;
        }

        [HttpGet("{stockID}")]
        [AllowAnonymous]
        public async Task<ActionResult<List<StockQuarterlyData>>> GetStockQuarterlyData(int stockID)
        {
            List<StockQuarterlyData> lstDBStockQtrlyData = new List<StockQuarterlyData>();
            List<StockQuarterlyData> retlstStockQtrlyData = new List<StockQuarterlyData>();

            lstDBStockQtrlyData = await _context.StockQuarterlyData.ToListAsync<StockQuarterlyData>();

            retlstStockQtrlyData = lstDBStockQtrlyData.FindAll(sfa => sfa.stockID == stockID);

            return retlstStockQtrlyData;
        }
    }
}