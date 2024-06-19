using api.Extensions;
using api.Models;
using api.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace api.Controllers
{
    [Route("api/v1/portfolio")]
    public class PortfolioController : ControllerBase
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly StockRepo _stockRepo;

        private readonly PortfolioRepo _portfolioRepo;
        public PortfolioController(UserManager<AppUser> userManager, StockRepo stockRepo, PortfolioRepo portfolioRepo)
        {
            _userManager = userManager;
            _stockRepo = stockRepo;
            _portfolioRepo = portfolioRepo;
        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> GetUserPortfolio()
        {
            var username = User.GetUsername();

            if (username == null)
            {
                return Unauthorized();
            }

            var appUser = await _userManager.FindByNameAsync(username);

            if (appUser == null)
            {
                return Unauthorized();
            }

            var userPortfolio = await _portfolioRepo.GetUserPortfolioAsync(appUser);

            return Ok(userPortfolio);
        }
    }
}