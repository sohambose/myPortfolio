using System.Collections.Generic;
using System.Linq;
using API.DTOS;
using API.Entities;

namespace API.BLL
{
    public class StockFundamentalBLL
    {
        //---For Singleton--
        private static StockFundamentalBLL Instance = new StockFundamentalBLL();
        public static StockFundamentalBLL GetInstance()
        {
            return Instance;
        }

        public void SortFundamentalDataByStmtType(List<StockFundamentalAttributes> lstStockFADB, List<StockFundamentalAttributes> retlstStockFA)
        {
            //--Order by statement type
            retlstStockFA.AddRange(lstStockFADB.FindAll(fa => fa.Statement.ToUpper().Equals("PL")));
            retlstStockFA.AddRange(lstStockFADB.FindAll(fa => fa.Statement.ToUpper().Equals("BALANCESHEET")));
            retlstStockFA.AddRange(lstStockFADB.FindAll(fa => fa.Statement.ToUpper().Equals("CASHFLOW")));
            retlstStockFA.AddRange(lstStockFADB.FindAll(fa => fa.Statement.ToUpper().Equals("PROFITABILITYRATIO")));
            retlstStockFA.AddRange(lstStockFADB.FindAll(fa => fa.Statement.ToUpper().Equals("LEVERAGERATIO")));
            retlstStockFA.AddRange(lstStockFADB.FindAll(fa => fa.Statement.ToUpper().Equals("OPERATINGRATIO ")));
        }

        public List<StockComparisonScores> GetStockComparisonFromSFA(List<StockFundamentalAttributes> lstStockFADB)
        {
            List<StockComparisonScores> lstScoreSave = new List<StockComparisonScores>();
            //---get all the heads----
            List<string> lstHeadsToCompare = lstStockFADB.Select(fa => fa.Head).Distinct().ToList();
            foreach (string headDesc in lstHeadsToCompare)
            {
                List<StockFundamentalAttributes> lstFilteredStockFAs = new List<StockFundamentalAttributes>();
                lstFilteredStockFAs = lstStockFADB.FindAll(sfa => sfa.Head.ToUpper().Equals(headDesc.ToUpper())).OrderBy(c => c.observationValue).ToList();
                List<StockComparisonScores> lstScore = (from sfa in lstFilteredStockFAs
                                                        select new StockComparisonScores
                                                        {
                                                            Head = sfa.Head,
                                                            Statement = sfa.Statement,
                                                            stockID = sfa.stockID,
                                                            ObservationValue = sfa.observationValue ?? 0,
                                                            ObservationValueType = sfa.observationValueType,
                                                            StockScore = lstFilteredStockFAs.IndexOf(sfa) + 1
                                                        }).ToList();

                lstScoreSave.AddRange(lstScore);
            }
            return lstScoreSave;
        }

        public IEnumerable<StockScoreDTO> GetScoreReportSummary(List<StockComparisonScores> lstScoreSave, List<Stock> lstAllStocks)
        {
            IEnumerable<StockScoreDTO> ScoreReport = new List<StockScoreDTO>();
            ScoreReport = from scoreObj in lstScoreSave
                          group scoreObj by scoreObj.stockID into r
                          select new StockScoreDTO { stockID = r.Key, Score = r.Sum(x => x.StockScore) };

            IEnumerable<StockScoreDTO> ScoreReportWithName = new List<StockScoreDTO>();
            ScoreReportWithName = from scoreObj in ScoreReport
                                  join stockObj in lstAllStocks on scoreObj.stockID equals stockObj.stockID
                                  select new StockScoreDTO
                                  {
                                      stockID = scoreObj.stockID,
                                      Score = scoreObj.Score,
                                      StockName = stockObj.stockSymbol
                                  };
            ScoreReportWithName = ScoreReportWithName.OrderByDescending(s => s.Score);
            return ScoreReportWithName;
        }

        public List<StockComparisonDetailsDTO> GenerateDetailedComparison(List<StockComparisonScores> lstStockCompareDB, List<Stock> lstAllStocks)
        {
            
            List<StockComparisonDetailsDTO> retlstStockCompare = new List<StockComparisonDetailsDTO>();            

            IEnumerable<StockComparisonDetailsDTO> Ienum_StockCompare = from stockCompareObj in lstStockCompareDB
                                                                        join stockObj in lstAllStocks on stockCompareObj.stockID equals stockObj.stockID
                                                                        select new StockComparisonDetailsDTO
                                                                        {
                                                                            stockID = stockCompareObj.stockID,
                                                                            Head = stockCompareObj.Head,
                                                                            Statement = stockCompareObj.Statement,
                                                                            stockSymbol = stockObj.stockSymbol,
                                                                            ObservationValue = stockCompareObj.ObservationValue,
                                                                            ObservationValueType = stockCompareObj.ObservationValueType,
                                                                            StockScore = stockCompareObj.StockScore
                                                                        };
            List<StockComparisonDetailsDTO> lstStockCompare = Ienum_StockCompare.ToList<StockComparisonDetailsDTO>();
            lstStockCompare = lstStockCompare.OrderBy(s => s.Head).ToList();

            //--Order by statement type
            retlstStockCompare.AddRange(lstStockCompare.FindAll(fa => fa.Statement.ToUpper().Equals("PL")));
            retlstStockCompare.AddRange(lstStockCompare.FindAll(fa => fa.Statement.ToUpper().Equals("BALANCESHEET")));
            retlstStockCompare.AddRange(lstStockCompare.FindAll(fa => fa.Statement.ToUpper().Equals("CASHFLOW")));
            retlstStockCompare.AddRange(lstStockCompare.FindAll(fa => fa.Statement.ToUpper().Equals("PROFITABILITYRATIO")));
            retlstStockCompare.AddRange(lstStockCompare.FindAll(fa => fa.Statement.ToUpper().Equals("LEVERAGERATIO")));
            retlstStockCompare.AddRange(lstStockCompare.FindAll(fa => fa.Statement.ToUpper().Equals("OPERATINGRATIO ")));

            return retlstStockCompare;
        }
    }
}