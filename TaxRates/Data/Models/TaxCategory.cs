using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using System.ComponentModel.DataAnnotations.Schema;

namespace TaxRates.Data.Models
{
	public class TaxCategory
	{
		#region Constructor
		public TaxCategory() { }
		#endregion

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
		[ForeignKey("Rate")]
		public int RateId { get; set; }
		#endregion

		#region Navigation Properties
		/// <summary>
		/// Tax Rate related to this category
		/// </summary>
		public virtual TaxRate TaxRate { get; set; }
		#endregion
	}
}
