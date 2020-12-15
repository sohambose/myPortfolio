using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.Data;
using API.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers
{
    public class StockFundamentalAttributeController : BaseAPIController
    {
        private readonly DataContext _context;
        public StockFundamentalAttributeController(DataContext context)
        {
            this._context = context;
        }

        [HttpGet("{stockID}")]
        [AllowAnonymous]
        public async Task<ActionResult<List<StockFundamentalAttributes>>> GetStockFundamentalAttribute(int stockID)
        {
            List<StockFundamentalAttributes> lstStockFADB = new List<StockFundamentalAttributes>();
            List<StockFundamentalAttributes> retlstStockFA = new List<StockFundamentalAttributes>();

            lstStockFADB = await _context.StockFundamentalAttributes.ToListAsync<StockFundamentalAttributes>();
            lstStockFADB = lstStockFADB.FindAll(sfa => sfa.stockID == stockID);

            //--Order by statement type
            retlstStockFA.AddRange(lstStockFADB.FindAll(fa => fa.Statement.ToUpper().Equals("PL")));
            retlstStockFA.AddRange(lstStockFADB.FindAll(fa => fa.Statement.ToUpper().Equals("BALANCESHEET")));
            retlstStockFA.AddRange(lstStockFADB.FindAll(fa => fa.Statement.ToUpper().Equals("CASHFLOW")));
            retlstStockFA.AddRange(lstStockFADB.FindAll(fa => fa.Statement.ToUpper().Equals("PROFITABILITYRATIO")));
            retlstStockFA.AddRange(lstStockFADB.FindAll(fa => fa.Statement.ToUpper().Equals("LEVERAGERATIO")));
            retlstStockFA.AddRange(lstStockFADB.FindAll(fa => fa.Statement.ToUpper().Equals("OPERATINGRATIO ")));

            return retlstStockFA;
        }
    }
}