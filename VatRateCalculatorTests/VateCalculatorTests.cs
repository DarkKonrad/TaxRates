using NUnit.Framework;
using System.Threading.Tasks;
using TaxRateCalculator;

namespace VatRateCalculatorTests
{
	public class VateCalculatorTests
	{
		private VatRateCalculator calculator;

		[SetUp]
		public void Setup()
		{
			calculator = new VatRateCalculator();
		}

		[Test]
		public void given_23Tax_ammount_1000_net_should_be_1230_gross()
		{
			const decimal taxRate = 0.23M;
			const decimal amount = 1000;
			const decimal expectedResult = 1230.00M;

			decimal result = this.calculator.ComputeGrossVat(amount, taxRate);

			Assert.AreEqual(expectedResult, result);
		}

		[Test]
		public void given_23Tax_ammount_1000_gross_should_be_813dot01_net()
		{
			const decimal taxRate = 0.23M;
			const decimal amount = 1000;
			const decimal expectedResult = 813.01M;

			decimal result = this.calculator.ComputeNetVat(amount, taxRate);
			result = decimal.Round(result, 2);
			Assert.AreEqual(expectedResult, result);
		}

		[Test]
		public void given_negative_23Tax_ammount_1000_gross_should_be_813dot01_net()
		{
			const decimal taxRate = -0.23M;
			const decimal amount = 1000;
			const decimal expectedResult = 813.01M;

			decimal result = this.calculator.ComputeNetVat(amount, taxRate);
			result = decimal.Round(result, 2);
			Assert.AreEqual(expectedResult, result);
		}

		[Test]
		public void given_23Tax_ammount_decimalMax_gross_result_is_minusOne()
		{
			const decimal taxRate = 0.23M;
			const decimal amount = decimal.MaxValue;
			const decimal expectedResult = -1M;

			decimal result = this.calculator.ComputeGrossVat(amount, taxRate);

			Assert.AreEqual(expectedResult, result);
		}

		[Test]
		public void given_23Tax_ammount_decimalMax_net_result_is_minusOne()
		{
			const decimal taxRate = 0.23M;
			const decimal amount = decimal.MaxValue;
			const decimal expectedResult = -1M;

			decimal result = this.calculator.ComputeNetVat(amount, taxRate);

			Assert.AreEqual(expectedResult, result);
		}

		[Test]
		public void given_23Tax_ammount_decimalMin_gross_result_is_minusOne()
		{
			const decimal taxRate = 0.23M;
			const decimal amount = decimal.MinValue;
			const decimal expectedResult = -1M;

			decimal result = this.calculator.ComputeGrossVat(amount, taxRate);

			Assert.AreEqual(expectedResult, result);
		}

		[Test]
		public void given_23Tax_ammount_decimalMin_net_result_is_minusOne()
		{
			const decimal taxRate = 0.23M;
			const decimal amount = decimal.MinValue;
			const decimal expectedResult = -1M;

			decimal result = this.calculator.ComputeNetVat(amount, taxRate);

			Assert.AreEqual(expectedResult, result);
		}

		[Test]
		public async Task given_8Tax_ammount_129873_gross_should_be_140262_dot_84_async()
		{
			const decimal taxRate = 0.08M;
			const decimal amount = 129873;
			const decimal expectedResult = 140262.84M;

			decimal result = await this.calculator.ComputeGrossVatAsync(amount, taxRate);
			result = decimal.Round(result, 2);

			Assert.AreEqual(expectedResult, result);
		}

		[Test]
		public async Task given_8Tax_ammount_129873_net_should_be_120252_dot_78_async()
		{
			const decimal taxRate = 0.08M;
			const decimal amount = 129873;
			const decimal expectedResult = 120252.78M;

			decimal result = await this.calculator.ComputeNetVatAsync(amount, taxRate);
			result = decimal.Round(result, 2);

			Assert.AreEqual(expectedResult, result);
		}
	}
}