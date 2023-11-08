using InProPlayerWeb.Helper;
using InProPlayerWeb.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace InProPlayerWeb.Controllers
{
    public class SchedulerNPDayController : Controller
    {
        private readonly ApplicationDbContext _context;

        public SchedulerNPDayController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet("SchedulerNPDay/Index/{page?}")]
        public IActionResult Index(int page = 1, string search = "")
        {
            List<SchedulerNPDay> configDB;
            if (String.IsNullOrEmpty(search))
            {
                configDB = _context.SchedulerNPDay.ToList();
            }
            else
            {
                configDB = _context.SchedulerNPDay.Where(g => EF.Functions.Like(g.DayName, $"%{search}%") || EF.Functions.Like(g.NPDay, $"%{search}%")).ToList();
                ViewBag.Search = search;
            }

            PageHelper<SchedulerNPDay> ph = new PageHelper<SchedulerNPDay>();
            ph.page = page;
            ph.pageSize = 10;
            ph.pageList = configDB;

            ViewBag.TotalPages = ph.TotalPage();
            ViewBag.CurrentPage = ph.page;
            ViewBag.DataList = ph.PageList();
            ViewBag.Title = "不撥放日期設定";
            return View();
        }

        [HttpGet]
        public ViewResult Append()
        {
            return View();
        }
        [HttpGet]
        public IActionResult Edit(int id)
        {
            // data serach ID from SchedulerNPDay
            SchedulerNPDay SchedulerNPDay = _context.SchedulerNPDay.Find(id);

            if (SchedulerNPDay != null)
            {
                return View(SchedulerNPDay);
            }

            return RedirectToAction("Index");
        }
        [HttpGet]
        public IActionResult Delete(int id)
        {
            SchedulerNPDay SchedulerNPDay = _context.SchedulerNPDay.Find(id);
            if (SchedulerNPDay != null)
            {
                _context.SchedulerNPDay.Remove(SchedulerNPDay);
                _context.SaveChanges();
            }
            return RedirectToAction("Index");
        }

        [HttpPost]
        public IActionResult Append(SchedulerNPDay SchedulerNPDay, string NPDayMonth, string NPDayDay)
        {
            SchedulerNPDay.NPDay = NPDayMonth+"/"+NPDayDay;

            _context.SchedulerNPDay.Add(SchedulerNPDay);
            _context.SaveChanges();
            return RedirectToAction("Index");
        }
        [HttpPost]
        public IActionResult Edit(SchedulerNPDay SchedulerNPDay, string NPDayMonth, string NPDayDay)
        {
            SchedulerNPDay.NPDay = NPDayMonth + "/" + NPDayDay;
            
            _context.SchedulerNPDay.Update(SchedulerNPDay);
            _context.SaveChanges();
            return RedirectToAction("Index");
        }
    }
}
