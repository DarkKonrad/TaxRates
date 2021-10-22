using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace VatRates.Data.DTO
{
	public class TaxInput
	{
		[JsonIgnore]
		public static string Gross { get =>  "gross"; }

		[JsonIgnore]
		public static string Net { get => "net"; }
	
		public decimal Amount { get; set; }
		public string CalculatedVatType { get; set; }
		public string Category { get; set; }
	}
}
