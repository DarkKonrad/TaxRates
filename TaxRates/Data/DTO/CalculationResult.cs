namespace VatRates.Data.DTO
{
	public class CalculationResult
	{
		public string ValueType { get; set; }
		public decimal CaclucaltedTaxValue { get; set; }
		public string VatCategory { get; set; }
		public string ErrorMessage { get; set; } 
	}
}