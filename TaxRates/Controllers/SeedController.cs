using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using TaxRates.Controllers.Extensions;
using TaxRates.Data;

namespace TaxRates.Controllers
{
	[Route("api/[controller]/[action]")]
	[ApiController]
	public class SeedController : ControllerBase
	{
		private readonly ApplicationDbContext _context;
		private readonly IWebHostEnvironment _env;

		public SeedController(
			ApplicationDbContext context,
			IWebHostEnvironment env)
		{
			_context = context;
			_env = env;
		}

		[HttpGet]
		public async Task<ActionResult> Import()
		{
			return await this.LoadTaxRatesAndCategoriesFromXlsx(_context, _env);
		}
	}
}