using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace VatRates.Data.DTO
{
	public class CalculationResult
	{
		public string ValueType { get; set; }
		public decimal CaclucaltedTaxType { get; set; }
		public string VatCategory { get; set; }
	}
}
