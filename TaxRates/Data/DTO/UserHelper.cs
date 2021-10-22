using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VatRates.Data.DTO
{
	public class UserHelper
	{
		public UserHelper()
		{
			SampleCalculationResult = new CalculationResult()
			{
				CaclucaltedTaxValue = 108000.00M,
				ValueType = "gross",
				VatCategory = "Construction"
			};

			SampleTaxInput = new TaxInput()
			{
				Amount = 100000M,
				Category = "Construction",
				CalculatedVatType = "gross"
			};
			
			// ToDo: "Prettify messge below"

			Message = new StringBuilder()
				.Append("POST URL: <Host>/api/VatCalculation/Calculate")
				.AppendLine("With input json in body see example ")
				.AppendLine("Provide Amount as decimal. Amount will be rounded to 2 places after comma ")
				.AppendLine("Provide ValueType for calculated tax as string: \"gross\" (default) or \"net\"")
				.AppendLine("GET URL: <Host>/api/VatCalculation/Categories?pageSize=10 ")
				.Append("returns avalible paginated categories with tax rates ")
				.ToString();
		}

		public string Message { get; set; }
		public CalculationResult SampleCalculationResult { get; set; }
		public TaxInput SampleTaxInput { get; set; }
		
	}
}
