using System;
using System.Threading.Tasks;

namespace TaxRateCalculator
{
	public class VatRateCalculator : IVatRateCalulator
	{
		public static decimal ErrorResult { get => -1; }
		public decimal ComputeGrossVat(decimal amountNet, decimal taxRate)
		{
			if (taxRate < 0)
				taxRate *= -1;

			if (checkIfExceedsMaxOrMin(amountNet)
				|| checkIfExceedsMaxOrMin(taxRate))
				return ErrorResult;
		
			amountNet = decimal.Round(amountNet, 2);
		
			return amountNet * (1 + taxRate);
		}

		public decimal ComputeNetVat(decimal amountGross, decimal taxRate)
		{
			if (taxRate < 0)
				taxRate *= -1;

			if (checkIfExceedsMaxOrMin(amountGross)
				|| checkIfExceedsMaxOrMin(taxRate))
				return ErrorResult;

			amountGross = decimal.Round(amountGross, 2);

			return amountGross / (1 + taxRate);
		}

		public async Task<decimal> ComputeGrossVatAsync(decimal amountNet, decimal taxRate)
		{
			return await Task.Run(() => ComputeGrossVat(amountNet, taxRate));
		}

		public async Task<decimal> ComputeNetVatAsync(decimal amountNet, decimal taxRate)
		{
			return await Task.Run(() => ComputeNetVat(amountNet, taxRate));
		}

		private bool checkIfExceedsMaxOrMin(decimal val) => 
			val >= decimal.MaxValue || val <= decimal.MinValue;
	}
}
