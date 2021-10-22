using System.Collections.Generic;
using TaxRates.Data.Models;

namespace VatRates.Data.DTO
{
	public class TaxCategoriesSet
	{
		public bool HasNext { get; set; }
		public List<TaxCategory> Categories { get; set; }
	}
}