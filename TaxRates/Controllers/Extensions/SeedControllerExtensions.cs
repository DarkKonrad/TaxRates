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
        // helper variable initialised in LoadTaxRatesFromXlsx
        // used later to retreive tax rates in LoadTaxCategoriesFromXlsx
        private static List<TaxRate> lstTaxRates;


		private static async Task<uint> LoadTaxRatesFromXlsx(ApplicationDbContext dbContext, ExcelPackage excelPackage, uint worksheetIndex = 0)
		{
            lstTaxRates = dbContext.TaxRates.ToList();
            uint TaxRatesCount = 0;
            var workSheet = excelPackage.Workbook.Worksheets[(int)worksheetIndex];

            // iterates through all rows, skipping the first one
            for (int currentRowCount = 2;
                currentRowCount <= workSheet.Dimension.End.Row;
                currentRowCount++)
            {
                var row = workSheet.Cells[currentRowCount, 1, currentRowCount, workSheet.Dimension.End.Column];
                var rateVal = row[currentRowCount, 2].GetValue<string>();

                //this TaxRate already exist in the database?
                if (lstTaxRates.Where(c => c.Rate == rateVal).Count() != 0)
                    continue;

                var rate = new TaxRate
                {
                    Rate = rateVal
                };

                dbContext.TaxRates.Add(rate);

                // store the TaxRate to retrieve its Id later on
                lstTaxRates.Add(rate);

                TaxRatesCount++;
            }

            // save all the TaxRates into the Database
            if (TaxRatesCount > 0)
                await dbContext.SaveChangesAsync();

            return TaxRatesCount;
        }

        private static async Task<uint> LoadTaxCategoriesFromXlsx(ApplicationDbContext dbContext, ExcelPackage excelPackage, uint worksheetIndex = 0)
		{
            var taxCategoryList = dbContext.TaxCategories.ToList();
            uint TaxCategoryCount = 0;
            var workSheet = excelPackage.Workbook.Worksheets[(int)worksheetIndex];

            // iterates through all rows, skipping the first one
            for (int nRow = 2;
                nRow <= workSheet.Dimension.End.Row;
                nRow++)
            {
                var row = workSheet.Cells[nRow, 1, nRow, workSheet.Dimension.End.Column];

                var name = row[nRow, 1].GetValue<string>();
                var taxRateValue = row[nRow, 2].GetValue<string>();

                // retrieve TaxRate and taxRateId
                var taxRate = lstTaxRates.Where(c => c.Rate == taxRateValue)
                    .FirstOrDefault();
                var taxRateId = taxRate.Id;

                // does this TaxCategory already exist in the database?
                if (taxCategoryList.Where(c => c.Name == name).Count() != 0)
                    continue;

                var taxCategory = new TaxCategory
                {
                    Name = name.Trim(),
                    RateId = taxRateId
                };

                dbContext.TaxCategories.Add(taxCategory);

                TaxCategoryCount++;
            }

            // save all the TaxCategoryies into the Database
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
        public static async Task<ActionResult> LoadTaxRatesAndCategoriesFromXlsx(this SeedController seedController,
            ApplicationDbContext dbContext, 
            IWebHostEnvironment env, 
            uint worksheetIndex = 0)
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
