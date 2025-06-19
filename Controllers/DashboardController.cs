using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TrackCure.Interfaces;
using TrackCure.Services;

namespace TrackCure.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DashboardController : ControllerBase
    {
        private readonly JwtService _jwtService;
        private readonly string _connectionString;
        private readonly IAdminRepository _adminRepository;
        public DashboardController(JwtService jwtService, IConfiguration configuration, IAdminRepository adminRepository)
        {
            _jwtService = jwtService;
            _adminRepository = adminRepository;
            _connectionString = configuration.GetConnectionString("DefaultConnection")!;

        }

        [HttpGet("admin-dashboard")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetDashboardStats()
        {
            var stats = await _adminRepository.GetDashboardStatsAsync();
            return Ok(stats);
        }
    }
}
