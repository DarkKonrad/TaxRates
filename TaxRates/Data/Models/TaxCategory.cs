using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TaxRates.Data.Models
{
	public class TaxCategory
	{
		#region Constructor

		public TaxCategory()
		{
		}

		#endregion Constructor

		#region Properties

		/// <summary>
		/// The unique and primary key for this TaxCategory.
		/// </summary>
		[Key]
		[Required]
		public int Id { get; set; }

		/// <summary>
		/// Tax Category's name.
		/// </summary>
		public string Name { get; set; }

		/// <summary>
		/// Tax Rate Id (foregin key).
		/// </summary>
		[ForeignKey("TaxRate")]
		public int RateId { get; set; }

		#endregion Properties

		#region Navigation Properties

		/// <summary>
		/// Tax Rate related to this category
		/// </summary>
		public virtual TaxRate TaxRate { get; set; }

		#endregion Navigation Properties
	}
}