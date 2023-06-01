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
            // 根据 id 获取对应的 Group 对象
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
            // 将新的 Group 对象添加到上下文中
            _context.SchedulerNPDay.Add(SchedulerNPDay);

            // 保存更改到数据库
            _context.SaveChanges();

            // 重定向到 Index 页面
            return RedirectToAction("Index");
        }
        [HttpPost]
        public IActionResult Edit(SchedulerNPDay SchedulerNPDay, string NPDayMonth, string NPDayDay)
        {
            SchedulerNPDay.NPDay = NPDayMonth + "/" + NPDayDay;
            // 更新 Group 对象的属性
            _context.SchedulerNPDay.Update(SchedulerNPDay);

            // 保存更改到数据库
            _context.SaveChanges();

            // 重定向到 Index 页面
            return RedirectToAction("Index");
        }
    }
}
