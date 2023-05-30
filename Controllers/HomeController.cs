using BizLandTemplate.DAL;
using BizLandTemplate.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BizLandTemplate.Controllers
{
    public class HomeController : Controller
    {
        private readonly AppDbContext _context;

        public HomeController(AppDbContext context)
        {
            _context = context;
        }
        public IActionResult Index()
        {
            List<Team> teams = _context.Teams.Include(t=>t.Position).ToList();
            return View(teams);
        }
    }
}
