using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using VendingApp.Domain;
using VendingApp.Extensions;
using VendingApp.Infrastructure;
using VendingApp.Model.Models;
using VendingApp.Model.Views;

namespace EducationPlus.Controllers
{
    [ApiController]
    public class VendingController : Controller
    {
        private readonly ILogger _logger;
        private string UserId => User.GetUserId();

        public VendingController(ILogger<VendingController> logger)
        {
            _logger = logger;
        }


        [HttpGet("/product/list")]
        public async Task<ActionResult<List<ProductView>>> GetGroups(
            [FromServices] VendingAppDbContext db,
            CancellationToken ct)
        {
            return Ok(await db.Products
                .Where(x => x.Code != null)
                .Select(x => new ProductView(x))
                .ToListAsync(ct));
        }
    }
}
