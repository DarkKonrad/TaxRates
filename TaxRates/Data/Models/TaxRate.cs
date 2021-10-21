using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using System.ComponentModel.DataAnnotations.Schema;

namespace TaxRates.Data.Models
{
	public class TaxRate
	{
		#region Constructor
		public TaxRate() { }
		#endregion

		#region Properties
		/// <summary>
		/// The unique and primary key for this Tax Rate.
		/// </summary>
		[Key]
		[Required]
		public int Id { get; set; }
		/// <summary>
		/// Tax Rate.
		/// </summary>
		public string Rate { get; set; }
		#endregion

		#region Navigation Properties
		/// <summary>
		/// A list containing all the tax categories related to this tax rate. 
		/// </summary>
		[JsonIgnore]
		public virtual List<TaxCategory> Categories { get; set; }
		#endregion
	}
}
