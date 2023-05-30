using BizLandTemplate.DAL;
using BizLandTemplate.Migrations;
using BizLandTemplate.Models;
using BizLandTemplate.Utilities.Extensions;
using BizLandTemplate.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BizLandTemplate.Areas.AdminPanel.Controllers
{
    [Area("AdminPanel")]
    public class TeamController : Controller
    {
        private readonly AppDbContext _context;
        private readonly IWebHostEnvironment _env;

        public TeamController(AppDbContext context,IWebHostEnvironment env)
        {
            _context = context;
            _env = env;
        }
        public async Task<IActionResult> Index()
        {
            List<Team> teams = await _context.Teams.Include(t=>t.Position).ToListAsync();
            return View(teams);
        }
        public IActionResult Create()
        {
            ViewBag.Positions = _context.Positions;
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Create(CreateTeamVM createTeamVM)
        {
            if (!ModelState.IsValid) return BadRequest();
            if (!createTeamVM.Photo.CheckFileType(createTeamVM.Photo.ContentType))
            {
                ModelState.AddModelError("Photo", "Faylin formati uygun deyil");
                ViewBag.Positions = _context.Positions;
                return View();
            }
            if (!createTeamVM.Photo.CheckFileSize(200))
            {
                ModelState.AddModelError("Photo", "Faylin hecmi boyukdur");
                ViewBag.Position = _context.Positions;
                return View();
            }
            bool result = await _context.Positions.AnyAsync(p => p.Id == createTeamVM.PositionId);
            if (!result)
            {
                ModelState.AddModelError("PositionId", "Bele id'li position yoxdur");
                ViewBag.Positions = _context.Positions;
                return View();
            }
            Team team = new Team()
            {
                Name = createTeamVM.Name,
                PositionId = createTeamVM.PositionId,
                InstagramLink = createTeamVM.InstagramLink,
                FacebookLink = createTeamVM.FacebookLink,
                TwitterLink = createTeamVM.TwitterLink,
                LinkedinLink = createTeamVM.LinkedinLink,
                Image = await createTeamVM.Photo.CreateFileAsync(_env.WebRootPath, "assets/img/team")
            };
            team.Image = await createTeamVM.Photo.CreateFileAsync(_env.WebRootPath, "assets/img/team");
            await _context.Teams.AddAsync(team);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        public async Task<IActionResult> Update(int? id)
        {
            if (id == null || id < 1) return BadRequest();
            Team team = await _context.Teams.FirstOrDefaultAsync(t => t.Id == id);
            if (team == null) return NotFound();
            UpdateTeamVM updateTeamVM = new UpdateTeamVM()
            {
                Image = team.Image,
                Name = team.Name,
                LinkedinLink= team.LinkedinLink,
                InstagramLink= team.InstagramLink,
                FacebookLink= team.FacebookLink,
                TwitterLink= team.TwitterLink,
                PositionId = team.PositionId
            };
            ViewBag.Positions = _context.Positions;
            return View(updateTeamVM);
        }
        [HttpPost]
        public async Task<IActionResult> Update(int? id,UpdateTeamVM updateTeamVM)
        {
            if (id == null || id < 1) return BadRequest();
            Team team = await _context.Teams.FirstOrDefaultAsync(t => t.Id == id);
            if (team == null) return NotFound();
            bool result = await _context.Positions.AnyAsync(p => p.Id == updateTeamVM.PositionId);
            if (!result)
            {
                ModelState.AddModelError("PositionId", "Bele id'li position yoxdur");
                ViewBag.Positions = _context.Positions;
                return View();
            }
            if (updateTeamVM.Photo !=null)
            {
                if (!updateTeamVM.Photo.CheckFileType(updateTeamVM.Photo.ContentType))
                {
                    ModelState.AddModelError("Photo", "Faylin formati uygun deyil");
                    ViewBag.Positions = _context.Positions;
                    updateTeamVM.Image = team.Image;
                    return View(updateTeamVM);
                }
                if (!updateTeamVM.Photo.CheckFileSize(200))
                {
                    ModelState.AddModelError("Photo", "Faylin hecmi boyukdur");
                    ViewBag.Position = _context.Positions;
                    updateTeamVM.Image = team.Image;
                    return View(updateTeamVM);
                }
                team.Image.DeleteFile(_env.WebRootPath, "assets/img/team");
                team.Image = await updateTeamVM.Photo.CreateFileAsync(_env.WebRootPath, "assets/img/team");
            }
            team.PositionId = updateTeamVM.PositionId;
            team.TwitterLink = updateTeamVM.TwitterLink;
            team.FacebookLink = updateTeamVM.FacebookLink;
            team.LinkedinLink = updateTeamVM.LinkedinLink;
            team.InstagramLink = updateTeamVM.InstagramLink;     
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || id < 1) return BadRequest();
            Team team = await _context.Teams.FirstOrDefaultAsync(t => t.Id == id);
            if (team == null) return NotFound();
            team.Image.DeleteFile(_env.WebRootPath, "assets/img/team");
            _context.Teams.Remove(team);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
    }
}
