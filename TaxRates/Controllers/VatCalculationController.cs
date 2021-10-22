using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using VatRates.Data.DTO;
using TaxRateCalculator;
using TaxRates.Data;
using Microsoft.EntityFrameworkCore;
using TaxRates.Data.Models;
// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace TaxRates.Controllers
{
	[Route("api/[controller]/[action]")]
	[ApiController]
	public class VatCalculationController : ControllerBase
	{
		private readonly ApplicationDbContext _context;

		public VatCalculationController(ApplicationDbContext context)
		{
			_context = context;
		}

		// GET: api/<ValuesController>/Categories
		[HttpGet]
		public async Task<ActionResult<TaxCategoriesSet>> Categories(int pageIndex=0, int pageSize = 30)
		{
			var totalCount = await _context.TaxCategories.CountAsync();
			var totalPages = (int)Math.Ceiling(totalCount / (double)pageSize);
			bool hasNext = ((pageIndex + 1) < totalPages);
			
			var query = _context.TaxCategories
				.Include(t => t.TaxRate)
				.Skip(pageIndex * pageSize)
				.Take(pageSize);

			var data = await query.ToListAsync();

			return new TaxCategoriesSet()
			{
				HasNext = hasNext,
				Categories = data
			};
		}

		// POST api/<ValuesController>
		[HttpPost]
		public async Task<ActionResult<CalculationResult>> Calculate([FromBody] TaxInput input)
		{
			input.CalculatedVatType = input.CalculatedVatType.ToLower();
			decimal result = 0;
			decimal taxRate = 0;
			TaxCategory taxCategory = null;

			if (!String.IsNullOrEmpty(input.Category))
			{
				taxCategory = await _context.TaxCategories
						.Include(i => i.TaxRate)
						.FirstOrDefaultAsync(t => t.Name == input.Category);

				if (taxCategory != null
					&& taxCategory.TaxRate != null)

					if(!decimal.TryParse(taxCategory.TaxRate.Rate, out taxRate))
						return BadRequest();
			}
			else // If Category is not provided or there is no such category in DB we assume
				//default VAT stack - 23 
				taxRate = 23;

			taxRate /= 100;

			if (input.CalculatedVatType == TaxInput.Gross)
				result = await new VatRateCalculator().ComputeGrossVatAsync(input.Amount, taxRate);

			if (input.CalculatedVatType == TaxInput.Net)
				result = await new VatRateCalculator().ComputeNetVatAsync(input.Amount, taxRate);

			if (result == VatRateCalculator.ErrorResult)
				return BadRequest();

			string vatCategory = taxCategory != null ?
								 taxCategory.Name : "";

			result = decimal.Round(result, 2);

			return new CalculationResult()
			{
				CaclucaltedTaxType = result,
				ValueType = input.CalculatedVatType,
				VatCategory = vatCategory
			};

		}
	}
}
