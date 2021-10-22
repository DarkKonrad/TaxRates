using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace TaxRateCalculator
{
	internal interface IVatRateCalulator
	{
		public static decimal ErrorResult { get => -1; }
		public decimal ComputeGrossVat(decimal amountNet, decimal taxRate);
		public decimal ComputeNetVat(decimal amountGross, decimal taxRate);
		public Task<decimal> ComputeGrossVatAsync(decimal amountNet, decimal taxRate);
		public Task<decimal> ComputeNetVatAsync(decimal amountGross, decimal taxRate);
	}
}
