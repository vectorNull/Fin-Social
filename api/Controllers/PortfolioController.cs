using api.Extensions;
using api.Interfaces;
using api.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace api.Controllers
{
    [Route("api/v1/portfolio")]
    public class PortfolioController : ControllerBase
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly IStockRepo _stockRepo;

        private readonly IPortfolioRepo _portfolioRepo;
        public PortfolioController(UserManager<AppUser> userManager, IStockRepo stockRepo, IPortfolioRepo portfolioRepo)
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

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> AddStockToPortfolio(string symbol)
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

            var stockDto = await _portfolioRepo.AddStockToPortfolioAsync(appUser, symbol);

            if (stockDto == null)
            {
                return NotFound();
            }

            return CreatedAtAction(nameof(GetUserPortfolio), new { id = stockDto.Id }, stockDto);
        }

        [HttpDelete]
        [Authorize]
        public async Task<IActionResult> RemoveStockFromPortfolio(string symbol)
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

            var result = await _portfolioRepo.RemoveStockFromPortfolioAsync(appUser, symbol);

            if (!result)
            {
                return NotFound();
            }

            return NoContent();
        }
    }
}