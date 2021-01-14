using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using API.DTOS;
using API.Entities;
using CsvHelper;

namespace API.BLL
{
    public class CSVDataProcessor
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(typeof(CSVDataProcessor));
        public List<StockFundamentalAttributes> ProcessYearlyCSVData(string filePath, int stockID)
        {
            try
            {
                int roundingPlaces = 2;
                TextReader reader = new StreamReader(filePath);
                var csvReader = new CsvReader(reader, CultureInfo.InvariantCulture);

                //Get all records from the csv file.
                IEnumerable<StockFundamentalAttributeCSV> lstrecords = csvReader.GetRecords<StockFundamentalAttributeCSV>();

                List<StockFundamentalAttributes> lstSFA = new List<StockFundamentalAttributes>();
                foreach (StockFundamentalAttributeCSV csvitem in lstrecords)
                {
                    StockFundamentalAttributes sfaObj = new StockFundamentalAttributes();

                    sfaObj.stockID = stockID;

                    sfaObj.Statement = csvitem.Statement;
                    sfaObj.Head = csvitem.Head;

                    sfaObj.Y0 = string.IsNullOrEmpty(csvitem.Y0) ? 0 : decimal.Round(decimal.Parse(csvitem.Y0), roundingPlaces);
                    sfaObj.Y1 = string.IsNullOrEmpty(csvitem.Y1) ? 0 : decimal.Round(decimal.Parse(csvitem.Y1), roundingPlaces);
                    sfaObj.Y2 = string.IsNullOrEmpty(csvitem.Y2) ? 0 : decimal.Round(decimal.Parse(csvitem.Y2), roundingPlaces);
                    sfaObj.Y3 = string.IsNullOrEmpty(csvitem.Y3) ? 0 : decimal.Round(decimal.Parse(csvitem.Y3), roundingPlaces);
                    sfaObj.Y4 = string.IsNullOrEmpty(csvitem.Y4) ? 0 : decimal.Round(decimal.Parse(csvitem.Y4), roundingPlaces);
                    sfaObj.Y5 = string.IsNullOrEmpty(csvitem.Y5) ? 0 : decimal.Round(decimal.Parse(csvitem.Y5), roundingPlaces);
                    sfaObj.Y6 = string.IsNullOrEmpty(csvitem.Y6) ? 0 : decimal.Round(decimal.Parse(csvitem.Y6), roundingPlaces);
                    sfaObj.Y7 = string.IsNullOrEmpty(csvitem.Y7) ? 0 : decimal.Round(decimal.Parse(csvitem.Y7), roundingPlaces);
                    sfaObj.Y8 = string.IsNullOrEmpty(csvitem.Y8) ? 0 : decimal.Round(decimal.Parse(csvitem.Y8), roundingPlaces);
                    sfaObj.Y9 = string.IsNullOrEmpty(csvitem.Y9) ? 0 : decimal.Round(decimal.Parse(csvitem.Y9), roundingPlaces);

                    sfaObj.RecordTimeStamp = DateTime.Now;
                    CalculateAndSetObservationValues(sfaObj);

                    lstSFA.Add(sfaObj);
                }
                return lstSFA;
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                return null;
            }
        }

        public List<StockQuarterlyData> ProcessQuarterlyCSVData(string filePath, int stockID)
        {
            try
            {
                int roundingPlaces = 2;

                TextReader reader = new StreamReader(filePath);
                var csvReader = new CsvReader(reader, CultureInfo.InvariantCulture);
                IEnumerable<StockQuarterlyDataCSV> lstQtrlyrecords = csvReader.GetRecords<StockQuarterlyDataCSV>();

                List<StockQuarterlyData> lstSQD = new List<StockQuarterlyData>();

                foreach (StockQuarterlyDataCSV csvitem in lstQtrlyrecords)
                {
                    StockQuarterlyData sqdObj = new StockQuarterlyData();

                    sqdObj.stockID = stockID;
                    sqdObj.Narration = csvitem.Narration;

                    sqdObj.Q0 = string.IsNullOrEmpty(csvitem.Q0) ? 0 : decimal.Round(decimal.Parse(csvitem.Q0), roundingPlaces);
                    sqdObj.Q1 = string.IsNullOrEmpty(csvitem.Q1) ? 0 : decimal.Round(decimal.Parse(csvitem.Q1), roundingPlaces);
                    sqdObj.Q2 = string.IsNullOrEmpty(csvitem.Q2) ? 0 : decimal.Round(decimal.Parse(csvitem.Q2), roundingPlaces);
                    sqdObj.Q3 = string.IsNullOrEmpty(csvitem.Q3) ? 0 : decimal.Round(decimal.Parse(csvitem.Q3), roundingPlaces);
                    sqdObj.Q4 = string.IsNullOrEmpty(csvitem.Q4) ? 0 : decimal.Round(decimal.Parse(csvitem.Q4), roundingPlaces);
                    sqdObj.Q5 = string.IsNullOrEmpty(csvitem.Q5) ? 0 : decimal.Round(decimal.Parse(csvitem.Q5), roundingPlaces);
                    sqdObj.Q6 = string.IsNullOrEmpty(csvitem.Q6) ? 0 : decimal.Round(decimal.Parse(csvitem.Q6), roundingPlaces);
                    sqdObj.Q7 = string.IsNullOrEmpty(csvitem.Q7) ? 0 : decimal.Round(decimal.Parse(csvitem.Q7), roundingPlaces);
                    sqdObj.Q8 = string.IsNullOrEmpty(csvitem.Q8) ? 0 : decimal.Round(decimal.Parse(csvitem.Q8), roundingPlaces);
                    sqdObj.Q9 = string.IsNullOrEmpty(csvitem.Q9) ? 0 : decimal.Round(decimal.Parse(csvitem.Q9), roundingPlaces);

                    sqdObj.RecordTimeStamp = DateTime.Now;

                    //-----Calculate CAGR and insert---
                    decimal EndingValue = sqdObj.Q0;
                    decimal BeginningValue = sqdObj.Q9;
                    int quarters = 9;
                    double Value = 0.0;

                    if (EndingValue > 0 && BeginningValue > 0)
                        Value = (Math.Pow((Convert.ToDouble(EndingValue) / Convert.ToDouble(BeginningValue)), (double)1 / (double)quarters) - 1) * 100;

                    sqdObj.observationValueType = "CAGR";
                    sqdObj.observationValue = Convert.ToDecimal(Value);
                    //----------------------------------

                    lstSQD.Add(sqdObj);
                }

                //----------Insert Row For EBIT----------------------------------
                StockQuarterlyData sqdEBIT = new StockQuarterlyData();
                sqdEBIT.stockID = stockID;
                sqdEBIT.Narration = "EBIT";
                sqdEBIT.Q9 = lstSQD.Find(q => q.Narration.ToUpper().Equals("OPERATINGPROFIT")).Q9 +
                           lstSQD.Find(q => q.Narration.ToUpper().Equals("DEPRECIATION")).Q9 +
                           lstSQD.Find(q => q.Narration.ToUpper().Equals("INTEREST")).Q9 +
                           lstSQD.Find(q => q.Narration.ToUpper().Equals("TAX")).Q9;

                sqdEBIT.Q8 = lstSQD.Find(q => q.Narration.ToUpper().Equals("OPERATINGPROFIT")).Q8 +
                lstSQD.Find(q => q.Narration.ToUpper().Equals("DEPRECIATION")).Q8 +
                lstSQD.Find(q => q.Narration.ToUpper().Equals("INTEREST")).Q8 +
                lstSQD.Find(q => q.Narration.ToUpper().Equals("TAX")).Q8;

                sqdEBIT.Q7 = lstSQD.Find(q => q.Narration.ToUpper().Equals("OPERATINGPROFIT")).Q7 +
               lstSQD.Find(q => q.Narration.ToUpper().Equals("DEPRECIATION")).Q7 +
               lstSQD.Find(q => q.Narration.ToUpper().Equals("INTEREST")).Q7 +
               lstSQD.Find(q => q.Narration.ToUpper().Equals("TAX")).Q7;

                sqdEBIT.Q6 = lstSQD.Find(q => q.Narration.ToUpper().Equals("OPERATINGPROFIT")).Q6 +
               lstSQD.Find(q => q.Narration.ToUpper().Equals("DEPRECIATION")).Q6 +
               lstSQD.Find(q => q.Narration.ToUpper().Equals("INTEREST")).Q6 +
               lstSQD.Find(q => q.Narration.ToUpper().Equals("TAX")).Q6;

                sqdEBIT.Q5 = lstSQD.Find(q => q.Narration.ToUpper().Equals("OPERATINGPROFIT")).Q5 +
               lstSQD.Find(q => q.Narration.ToUpper().Equals("DEPRECIATION")).Q5 +
               lstSQD.Find(q => q.Narration.ToUpper().Equals("INTEREST")).Q5 +
               lstSQD.Find(q => q.Narration.ToUpper().Equals("TAX")).Q5;

                sqdEBIT.Q4 = lstSQD.Find(q => q.Narration.ToUpper().Equals("OPERATINGPROFIT")).Q4 +
               lstSQD.Find(q => q.Narration.ToUpper().Equals("DEPRECIATION")).Q4 +
               lstSQD.Find(q => q.Narration.ToUpper().Equals("INTEREST")).Q4 +
               lstSQD.Find(q => q.Narration.ToUpper().Equals("TAX")).Q4;

                sqdEBIT.Q3 = lstSQD.Find(q => q.Narration.ToUpper().Equals("OPERATINGPROFIT")).Q3 +
               lstSQD.Find(q => q.Narration.ToUpper().Equals("DEPRECIATION")).Q3 +
               lstSQD.Find(q => q.Narration.ToUpper().Equals("INTEREST")).Q3 +
               lstSQD.Find(q => q.Narration.ToUpper().Equals("TAX")).Q3;

                sqdEBIT.Q2 = lstSQD.Find(q => q.Narration.ToUpper().Equals("OPERATINGPROFIT")).Q2 +
               lstSQD.Find(q => q.Narration.ToUpper().Equals("DEPRECIATION")).Q2 +
               lstSQD.Find(q => q.Narration.ToUpper().Equals("INTEREST")).Q2 +
               lstSQD.Find(q => q.Narration.ToUpper().Equals("TAX")).Q2;

                sqdEBIT.Q1 = lstSQD.Find(q => q.Narration.ToUpper().Equals("OPERATINGPROFIT")).Q1 +
               lstSQD.Find(q => q.Narration.ToUpper().Equals("DEPRECIATION")).Q1 +
               lstSQD.Find(q => q.Narration.ToUpper().Equals("INTEREST")).Q1 +
               lstSQD.Find(q => q.Narration.ToUpper().Equals("TAX")).Q1;

                sqdEBIT.Q0 = lstSQD.Find(q => q.Narration.ToUpper().Equals("OPERATINGPROFIT")).Q0 +
                lstSQD.Find(q => q.Narration.ToUpper().Equals("DEPRECIATION")).Q0 +
                lstSQD.Find(q => q.Narration.ToUpper().Equals("INTEREST")).Q0 +
                lstSQD.Find(q => q.Narration.ToUpper().Equals("TAX")).Q0;


                //-----Calculate CAGR and insert---
                decimal EndingValueEBIT = sqdEBIT.Q0;
                decimal BeginningValueEBIT = sqdEBIT.Q9;
                int quartersEBIT = 9;
                double ValueEBIT = 0.0;

                if (EndingValueEBIT > 0 && BeginningValueEBIT > 0)
                    ValueEBIT = (Math.Pow((Convert.ToDouble(EndingValueEBIT) / Convert.ToDouble(BeginningValueEBIT)), (double)1 / (double)quartersEBIT) - 1) * 100;

                sqdEBIT.observationValueType = "CAGR";
                sqdEBIT.observationValue = Convert.ToDecimal(ValueEBIT);
                //----------------------------------

                lstSQD.Add(sqdEBIT);

                return lstSQD;

                //---------------------------------------------------------------


            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                return null;
            }
        }



        private void CalculateAndSetObservationValues(StockFundamentalAttributes sfa)
        {
            log.Debug("Processing " + sfa.Head);
            if (sfa.Head.ToUpper().Equals("GROSSPROFITMARGIN") || sfa.Head.ToUpper().Equals("NETPROFITPERCENTAGE")
                || sfa.Head.ToUpper().Equals("INVENTORYTURNOVER") || sfa.Head.ToUpper().Equals("ROE")
                || sfa.Head.ToUpper().Equals("EBTMARGIN") || sfa.Head.ToUpper().Equals("PATMARGIN")
                || sfa.Head.ToUpper().Equals("ROE") || sfa.Head.ToUpper().Equals("ROA")
                || sfa.Head.ToUpper().Equals("ROCE") || sfa.Head.ToUpper().Equals("INTERESTCOVERAGERATIO")
                || sfa.Head.ToUpper().Equals("DEBTTOEQUITYRATIO") || sfa.Head.ToUpper().Equals("FINANCIALLEVERAGERATIO")
                || sfa.Head.ToUpper().Equals("FIXEDASSETSTURNOVER") || sfa.Head.ToUpper().Equals("TOTALASSETSTURNOVERRATIO")
                || sfa.Head.ToUpper().Equals("INVENTORYTURNOVERRATIO") || sfa.Head.ToUpper().Equals("INVENTORYNUMBEROFDAYS")
                || sfa.Head.ToUpper().Equals("RECEIVABLETURNOVERRATIO") || sfa.Head.ToUpper().Equals("DAYSSALESOUTSTANDING")
               )
            {
                decimal average = (sfa.Y0 + sfa.Y1 + sfa.Y2 + sfa.Y3 + sfa.Y4 + sfa.Y5 + sfa.Y6 + sfa.Y7 + sfa.Y8 + sfa.Y9) / 9;
                sfa.observationValueType = "Average";
                sfa.observationValue = average;
            }
            else
            {
                decimal EndingValue = sfa.Y0;
                decimal BeginningValue = sfa.Y9;
                int years = 9;
                double Value = 0.0;

                if (EndingValue > 0 && BeginningValue > 0)
                    Value = (Math.Pow((Convert.ToDouble(EndingValue) / Convert.ToDouble(BeginningValue)), (double)1 / (double)years) - 1) * 100;

                sfa.observationValueType = "CAGR";
                sfa.observationValue = Convert.ToDecimal(Value);
            }
        }
    }
}