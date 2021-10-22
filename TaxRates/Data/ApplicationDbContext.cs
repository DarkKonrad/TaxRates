using Microsoft.EntityFrameworkCore;
using TaxRates.Data.Models;

namespace TaxRates.Data
{
	public class ApplicationDbContext : DbContext
	{
		#region Constructor

		public ApplicationDbContext() : base()
		{
		}

		public ApplicationDbContext(DbContextOptions options) : base(options)
		{
		}

		#endregion Constructor

		#region Methods

		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			base.OnModelCreating(modelBuilder);

			//Map Entity names to DB Table names
			modelBuilder.Entity<TaxRate>()
				.ToTable("TaxRates");
			modelBuilder.Entity<TaxCategory>()
				.ToTable("TaxCategories");
		}

		#endregion Methods

		#region Properties

		public DbSet<TaxRate> TaxRates { get; set; }
		public DbSet<TaxCategory> TaxCategories { get; set; }

		#endregion Properties
	}
}