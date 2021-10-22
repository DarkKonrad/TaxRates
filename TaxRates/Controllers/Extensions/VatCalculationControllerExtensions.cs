using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TaxRateCalculator;
using TaxRates.Controllers;
using TaxRates.Data;
using TaxRates.Data.Models;
using VatRates.Data.DTO;

namespace VatRates.Controllers.Extensions
{
	public static class VatCalculationControllerExtensions
	{
		public static async Task<ActionResult<CalculationResult>> CalculateVat
			(this VatCalculationController controller, 
			ApplicationDbContext _context, 
			TaxInput input)
		{
			var rateAndCategory = await GetTaxRateAndCategoryAsync(_context, input);	
			var rate = rateAndCategory.Key;
			var category = rateAndCategory.Value;

			// In Database we keep rates as non-floating point
			rate /= 100;

			var computedResult = await ComputeVatResult(input, rate);

			if (computedResult == VatRateCalculator.ErrorResult)
				return new CalculationResult()
				{
					ErrorMessage = "Problem occured when calculating VAT tax. Check input or try again later"
				};

			computedResult = decimal.Round(computedResult, 2);

			return new CalculationResult()
			{
				CaclucaltedTaxValue = computedResult,
				ValueType = input.CalculatedVatType,
				VatCategory = category
			};
		}

		private static async Task<KeyValuePair<decimal,string>> GetTaxRateAndCategoryAsync(ApplicationDbContext _context, TaxInput input)
		{
			decimal taxRate = 0;

			if (!String.IsNullOrEmpty(input.Category))
			{
				
				var taxCategory = await _context.TaxCategories
						.Include(i => i.TaxRate)
						.FirstOrDefaultAsync(t => t.Name == input.Category);

				if (taxCategory != null
					&& taxCategory.TaxRate != null
					&& true ==  decimal.TryParse(taxCategory.TaxRate.Rate, out taxRate))
						return new KeyValuePair<decimal,string>(taxRate,input.Category);

			}

			return new KeyValuePair<decimal, string>(23, "");
		}

		private static async Task<decimal> ComputeVatResult(TaxInput input, decimal taxRate)
		{
			var calculator = new VatRateCalculator();
			decimal result = 0;

			if (input.CalculatedVatType == TaxInput.Net)
				result = await calculator.ComputeNetVatAsync(input.Amount, taxRate);

			else
				result = await calculator.ComputeGrossVatAsync(input.Amount, taxRate);

			return result;
		}
	}
}