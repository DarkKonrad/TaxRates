using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;
using TaxRateCalculator;
using TaxRates.Data;
using TaxRates.Data.Models;
using VatRates.Data.DTO;
using VatRates.Controllers.Extensions;
// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace TaxRates.Controllers
{
	[Route("api/[controller]/[action]")]
	[ApiController]
	public class VatCalculationController : ControllerBase
	{
		private readonly ApplicationDbContext _context;

		public VatCalculationController(ApplicationDbContext context)
		{
			_context = context;
		}

		// GET: api/<ValuesController>/Categories
		[HttpGet]
		public async Task<ActionResult<TaxCategoriesSet>> Categories(int pageIndex = 0, int pageSize = 30)
		{
			// ToDo remove "id's" user do not need to see this.

			var totalCount = await _context.TaxCategories.CountAsync();
			var totalPages = (int)Math.Ceiling(totalCount / (double)pageSize);
			bool hasNext = ((pageIndex + 1) < totalPages);

			var query = _context.TaxCategories
				.Include(t => t.TaxRate)
				.Skip(pageIndex * pageSize)
				.Take(pageSize);

			var data = await query.ToListAsync();

			return new TaxCategoriesSet()
			{
				HasNext = hasNext,
				Categories = data
			};
		}

		[HttpGet]
		public ActionResult<UserHelper> Help()
		{
			return new UserHelper();
		}

		// POST api/<ValuesController>
		[HttpPost]
		public async Task<ActionResult<CalculationResult>> Calculate([FromBody] TaxInput input)
		{
			return await this.CalculateVat(_context, input);
		}
	}
}