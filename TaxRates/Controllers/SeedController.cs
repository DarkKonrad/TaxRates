using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using TaxRates.Data;
using TaxRates.Data.Models;

namespace TaxRates.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class SeedController : ControllerBase
	{
		private readonly ApplicationDbContext _context;
		private readonly IWebHostEnvironment _env;

		public SeedController(
			ApplicationDbContext context,
			IWebHostEnvironment env)
		{
			_context = context;
			_env = env;
		}

		[HttpGet]
		public async Task<ActionResult> Import()
		{
			var path = Path.Combine(
				_env.ContentRootPath,
				String.Format("Data/Source/TaxRatesSource.xlsx"));

			using (var stream = new FileStream(
				path,
				FileMode.Open,
				FileAccess.Read))
			{
                using (var ep = new ExcelPackage(stream))
                {
                    // Access first excel worksheet

                    var ws = ep.Workbook.Worksheets[0];

                    // Initialize the record counters
                    var TaxRatesCount = 0;
                    var TaxCategoryCount = 0;

                    #region Import all TaxRates
                    // create a list containing all the TaxCategory already existing
                    // into the Database (it will be empty on first run).
                    var lstTaxRates = _context.TaxRates.ToList();

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
                        _context.TaxRates.Add(rate);

                        // store the TaxRate to retrieve its Id later on
                        lstTaxRates.Add(rate);

                        // increment the counter
                        TaxRatesCount++;
                    }

                    // save all the TaxRates into the Database
                    if (TaxRatesCount > 0) 
                        await _context.SaveChangesAsync();
                    #endregion

                    #region Import all TaxCategories
                    // create a list containing all the cities already existing
                    // into the Database (it will be empty on first run).
                    var taxCategoryList = _context.TaxCategories.ToList();

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
                        _context.TaxCategories.Add(taxCategory);

                        // increment the counter
                        TaxCategoryCount++;
                        
                    }

                    // save all the TaxCategory into the Database
                    if (TaxCategoryCount > 0) 
                        await _context.SaveChangesAsync();
                    #endregion

                    return new JsonResult(new
                    {
                        ImportedTaxCategories = TaxCategoryCount,
                        ImportedTaxRates = TaxRatesCount
                    });
                }
            }
		}

	}
}
