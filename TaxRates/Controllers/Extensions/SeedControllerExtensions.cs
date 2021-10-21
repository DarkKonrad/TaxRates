using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TaxRates.Data;
using OfficeOpenXml;
using TaxRates.Data.Models;
using Microsoft.AspNetCore.Mvc;
using System.IO;
using Microsoft.AspNetCore.Hosting;

namespace TaxRates.Controllers.Extensions
{
	public static class SeedControllerExtensions
	{

        private static List<TaxRate> lstTaxRates;


		private static async Task<uint> LoadTaxRatesFromXlsx(ApplicationDbContext dbContext, ExcelPackage excelPackage, uint worksheetIndex = 0)
		{
            lstTaxRates = dbContext.TaxRates.ToList();
            uint TaxRatesCount = 0;
            var ws = excelPackage.Workbook.Worksheets[(int)worksheetIndex];

            // iterates through all rows, skipping the first one
            for (int currentRowCount = 2;
                currentRowCount <= ws.Dimension.End.Row;
                currentRowCount++)
            {
                var row = ws.Cells[currentRowCount, 1, currentRowCount, ws.Dimension.End.Column];
                var rateVal = row[currentRowCount, 2].GetValue<string>();

                // does this TaxRate already exist in the database?
                if (lstTaxRates.Where(c => c.Rate == rateVal).Count() != 0)
                    continue;

                // create the TaxRate entity and fill it with xlsx data
                var rate = new TaxRate
                {
                    Rate = rateVal
                };

                // add the new TaxRate to the DB context
                dbContext.TaxRates.Add(rate);

                // store the TaxRate to retrieve its Id later on
                lstTaxRates.Add(rate);

                // increment the counter
                TaxRatesCount++;
            }

            // save all the TaxRates into the Database
            if (TaxRatesCount > 0)
                await dbContext.SaveChangesAsync();

            return TaxRatesCount;
        }

        private static async Task<uint> LoadTaxCategoriesFromXlsx(ApplicationDbContext dbContext, ExcelPackage excelPackage, uint worksheetIndex = 0)
		{
            // create a list containing all the cities already existing
            // into the Database (it will be empty on first run).
            var taxCategoryList = dbContext.TaxCategories.ToList();
            uint TaxCategoryCount = 0;

            var ws = excelPackage.Workbook.Worksheets[(int)worksheetIndex];
            // iterates through all rows, skipping the first one
            for (int nRow = 2;
                nRow <= ws.Dimension.End.Row;
                nRow++)
            {
                var row = ws.Cells[nRow, 1, nRow, ws.Dimension.End.Column];

                var name = row[nRow, 1].GetValue<string>();
                var taxRateValue = row[nRow, 2].GetValue<string>();

                // retrieve TaxRate and taxRateId
                var taxRate = lstTaxRates.Where(c => c.Rate == taxRateValue)
                    .FirstOrDefault();
                var taxRateId = taxRate.Id;

                // does this TaxCategory already exist in the database?
                if (taxCategoryList.Where(c => c.Name == name).Count() != 0)
                    continue;

                // create the TaxCategory entity and fill it with xlsx data
                var taxCategory = new TaxCategory
                {
                    Name = name,
                    RateId = taxRateId
                };

                // add the new TaxCategory to the DB context
                dbContext.TaxCategories.Add(taxCategory);

                // increment the counter
                TaxCategoryCount++;

            }

            // save all the TaxCategory into the Database
            if (TaxCategoryCount > 0)
                await dbContext.SaveChangesAsync();

            return TaxCategoryCount;
        }
        
        /// <summary>
        /// Loads data tax rates and categories from xlsx file.
        /// </summary>
        /// <param name="seedController"></param>
        /// <param name="dbContext"></param>
        /// <param name="env"></param>
        /// <param name="worksheetIndex">Xlsx worksheet index ( counted from 0 ) where data are located.</param>
        /// <returns>Count of loaded Tax rates and categories</returns>
        public static async Task<ActionResult> LoadTaxRatesAndCategoriesFromXlsx(this SeedController seedController, ApplicationDbContext dbContext, IWebHostEnvironment env, uint worksheetIndex = 0)
		{
            var path = Path.Combine(
            env.ContentRootPath,
            String.Format("Data/Source/TaxRatesSource.xlsx"));
            uint _ImportedTaxCategories = 0;
            uint _ImportedTaxRates = 0;

            using (var stream = new FileStream(
                path,
                FileMode.Open,
                FileAccess.Read))
            {
                using (var ep = new ExcelPackage(stream))
                {
                    _ImportedTaxRates = await LoadTaxRatesFromXlsx(dbContext, ep, worksheetIndex);
                    _ImportedTaxCategories = await LoadTaxCategoriesFromXlsx(dbContext, ep, worksheetIndex);
                }
            }

            return new JsonResult(new
            {
                ImportedTaxCategories = _ImportedTaxCategories,
                ImportedTaxRates = _ImportedTaxRates
            });

        }
    }
}
