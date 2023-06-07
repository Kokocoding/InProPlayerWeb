using InProPlayerWeb.Helper;
using InProPlayerWeb.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace InProPlayerWeb.Controllers
{
    public class GroupController : Controller
    {
        private readonly ApplicationDbContext _context;

        public GroupController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet("Group/Index/{page?}")]
        public IActionResult Index(int page = 1, string search = "")
        {
            List<Group> configDB;
            if (String.IsNullOrEmpty(search))
            {
                configDB = _context.Group.ToList();
            }
            else
            {
                configDB = _context.Group.Where(g => EF.Functions.Like(g.title, $"%{search}%")).ToList();
                ViewBag.Search = search;
            }

            PageHelper<Group> ph = new PageHelper<Group>();
            ph.page     = page;
            ph.pageSize = 10;
            ph.pageList = configDB;

            ViewBag.TotalPages  = ph.TotalPage();
            ViewBag.CurrentPage = ph.page;
            ViewBag.DataList    = ph.PageList();
            ViewBag.Title       = "群組設定";
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
            Group group = _context.Group.Find(id);

            if (group != null)
            {
                return View(group);
            }

            return RedirectToAction("Index");
        }
        [HttpGet]
        public IActionResult Delete(int id)
        {
            Group group = _context.Group.Find(id);
            if (group != null)
            {
                _context.Group.Remove(group);
                _context.SaveChanges();
            }
            return RedirectToAction("Index");
        }

        [HttpPost]
        public IActionResult Append(string title)
        {
            //GroupModel
            Group newGroup = new Group
            {
                title = title
            };

            // 将新的 Group 对象添加到上下文中
            _context.Group.Add(newGroup);

            // 保存更改到数据库
            _context.SaveChanges();

            // 重定向到 Index 页面
            return RedirectToAction("Index");
        }
        [HttpPost]
        public IActionResult Edit(Group group)
        {
            // 更新 Group 对象的属性
            _context.Group.Update(group);

            // 保存更改到数据库
            _context.SaveChanges();

            // 重定向到 Index 页面
            return RedirectToAction("Index");
        }
    }
}
