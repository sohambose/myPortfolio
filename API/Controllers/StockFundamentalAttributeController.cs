using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.Data;
using API.DTOS;
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

        [HttpGet("compare/{SelectedStocks}")]
        [AllowAnonymous]
        public async Task<ActionResult<IEnumerable<StockScoreDTO>>> CompareStockFundamentals(string SelectedStocks)
        {
            List<StockFundamentalAttributes> lstStockFADB = new List<StockFundamentalAttributes>();

            lstStockFADB = await _context.StockFundamentalAttributes.ToListAsync<StockFundamentalAttributes>();

            //---get all the heads----

            List<string> lstHeadsToCompare = lstStockFADB.Select(fa => fa.Head).Distinct().ToList();

            List<StockComparisonScores> lstScoreSave = new List<StockComparisonScores>();

            foreach (string headDesc in lstHeadsToCompare)
            {
                List<StockFundamentalAttributes> lstFilteredStockFAs = new List<StockFundamentalAttributes>();
                lstFilteredStockFAs = lstStockFADB.FindAll(sfa => sfa.Head.ToUpper().Equals(headDesc.ToUpper())).OrderBy(c => c.observationValue).ToList();
                List<StockComparisonScores> lstScore = (from sfa in lstFilteredStockFAs
                                                        select new StockComparisonScores
                                                        {
                                                            Head = sfa.Head,
                                                            stockID = sfa.stockID,
                                                            ObservationValue = sfa.observationValue ?? 0,
                                                            ObservationValueType = sfa.observationValueType,
                                                            StockScore = lstFilteredStockFAs.IndexOf(sfa) + 1
                                                        }).ToList();

                lstScoreSave.AddRange(lstScore);
            }

            IEnumerable<StockScoreDTO> ScoreReport = new List<StockScoreDTO>();
            ScoreReport = from scoreObj in lstScoreSave
                          group scoreObj by scoreObj.stockID into r
                          select new StockScoreDTO { stockID = r.Key, Score = r.Sum(x => x.StockScore) };


            _context.StockComparisonScores.AddRange(lstScoreSave);
            _context.SaveChanges();



            return ScoreReport.ToList<StockScoreDTO>();
        }
    }
}