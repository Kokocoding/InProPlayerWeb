using InProPlayerWeb.Helper;
using InProPlayerWeb.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.ObjectModel;

namespace InProPlayerWeb.Controllers
{
    public class SchedulerController : Controller
    {
        private readonly ApplicationDbContext _context;

        private Dictionary<string, string> weekDayDict = new Dictionary<string, string>()
        {
            {"2", "星期一"}, {"3", "星期二"}, {"4", "星期三"}, {"5", "星期四"}, {"6", "星期五"}, {"7", "星期六"}, {"1", "星期日"}
        };
        private Dictionary<string, string> monthDayDict = new Dictionary<string, string>()
        {
            {"1", "31"}, {"2", "29"}, {"3", "31"}, {"4", "30"}, {"5", "31"}, {"6", "30"},
            {"7", "31"}, {"8", "31"}, {"9", "30"}, {"10", "31"}, {"11", "30"}, {"12", "31"}
        };
        public SchedulerController(ApplicationDbContext context)
        {
            _context = context;
        }
        [HttpGet("Scheduler/Index/{page?}")]
        public IActionResult Index(int page = 1, string search = "")
        {
            List<Scheduler> configDB;
            if (String.IsNullOrEmpty(search))
            {
                configDB = _context.Scheduler.ToList();
            }
            else
            {
                configDB = _context.Scheduler.Where(g => EF.Functions.Like(g.SchedulerName, $"%{search}%")).ToList();
                ViewBag.Search = search;
            }

            PageHelper<SchedulerList> ph = new PageHelper<SchedulerList>();

            List<SchedulerList> phL = configDB.Select(scheduler => new SchedulerList
            {
                GroupName = _context.Group.FirstOrDefault(g => g.id == scheduler.GroupID)?.title,
                Week = CronReString(scheduler.StartCron, "WeekString"), 
                Time = CronReString(scheduler.StartCron, "Time"),
                id = scheduler.id,
                GroupID = scheduler.GroupID,
                SchedulerName = scheduler.SchedulerName,
                StartCron = scheduler.StartCron,
                LoopType = scheduler.LoopType,
                LoopTimes = scheduler.LoopTimes,
                KeepTimes = scheduler.KeepTimes,
                Music = scheduler.Music,
                Terminal = scheduler.Terminal
            }).ToList();

            ph.page = page;
            ph.pageSize = 10;
            ph.pageList = phL;

            ViewBag.TotalPages = ph.TotalPage();
            ViewBag.CurrentPage = ph.page;
            ViewBag.DataList = phL;
            ViewBag.Title = "定時設定";
            return View();
        }

        [HttpGet]
        public ViewResult Append()
        {
            List<Group> group = _context.Group.ToList();
            ViewBag.GroupSelect = group;
            ViewBag.WeekDict = weekDayDict;
            return View();
        }
        [HttpGet]
        public IActionResult Edit(int id)
        {
            // 根据 id 获取对应的 Group 对象
            Scheduler scheduler = _context.Scheduler.Find(id);

            if (scheduler != null)
            {
                return View(scheduler);
            }

            return RedirectToAction("Index");
        }
        [HttpGet]
        public IActionResult Delete(int id)
        {
            Scheduler scheduler = _context.Scheduler.Find(id);
            if (scheduler != null)
            {
                _context.Scheduler.Remove(scheduler);
                _context.SaveChanges();
            }
            return RedirectToAction("Index");
        }

        [HttpPost]
        public IActionResult Append(Scheduler scheduler)
        {
            //SchedulerModel
            Scheduler newScheduler = new Scheduler
            {
                SchedulerName = scheduler.SchedulerName
            };

            // 将新的 Scheduler 对象添加到上下文中
            _context.Scheduler.Add(newScheduler);

            // 保存更改到数据库
            _context.SaveChanges();

            // 重定向到 Index 页面
            return RedirectToAction("Index");
        }
        [HttpPost]
        public IActionResult Edit(Scheduler scheduler)
        {
            // 更新 Scheduler 对象的属性
            _context.Scheduler.Update(scheduler);

            // 保存更改到数据库
            _context.SaveChanges();

            // 重定向到 Index 页面
            return RedirectToAction("Index");
        }

        public string CronString(string sTime, ObservableCollection<bool> Days)
        {
            List<string> week = new List<string>();
            if (Days[7]) week.Add("*");
            else
            {
                if (Days[0]) week.Add("1");
                if (Days[1]) week.Add("2");
                if (Days[2]) week.Add("3");
                if (Days[3]) week.Add("4");
                if (Days[4]) week.Add("5");
                if (Days[5]) week.Add("6");
                if (Days[6]) week.Add("7");
            }

            if (week.Count == 0) week.Add("*");
            //result 補 日 月 * 格式: Start Cron:End Cron
            string result = sTime + " ? * " + string.Join(",", week.ToArray());
            return result;
        }
        public string CronReString(string Cron, string reType, int addSecond = 0)
        {
            string result = "";
            string[] cronString = Cron.Split(' ');

            switch (reType)
            {
                case "OpenCron":
                    int newHour = int.Parse(cronString[2]);
                    int newMinute = int.Parse(cronString[1]);
                    int newSecond = int.Parse(cronString[0]) - addSecond;

                    if (newSecond < 0)
                    {
                        newSecond += 60;
                        newMinute -= 1;
                    }
                    if (newMinute < 0)
                    {
                        newMinute += 60;
                        newHour -= 1;
                    }

                    if (newSecond >= 60)
                    {
                        newSecond -= 60;
                        newMinute += 1;
                    }
                    if (newMinute >= 60)
                    {
                        newMinute -= 60;
                        newHour += 1;
                    }
                    result = $"{newSecond} {newMinute} {newHour} {cronString[3]} {cronString[4]} {cronString[5]}";
                    break;
                case "DateTime":
                    result = DateTime.Now.ToString("yyyy/mm/dd ") + cronString[2] + ":" + cronString[1] + ":" + cronString[0];
                    break;
                case "Time":
                    result = cronString[2] + ":" + cronString[1] + ":" + cronString[0];
                    break;
                case "Day":
                    result = cronString[3];
                    break;
                case "Month":
                    result = cronString[4];
                    break;
                case "WeekString":
                    if (cronString[5] == "*")
                    {
                        result = $"每日";
                    }
                    else
                    {
                        string[] weekArr = cronString[5].Split(',');
                        List<string> week = new List<string>();
                        foreach (var item in weekArr)
                        {
                            week.Add(weekDayDict[item].ToString());
                        }
                        result = string.Join(" , ", week.ToArray());
                    }
                    break;
                case "WeekDay":
                    result = cronString[5];
                    break;
                default:
                    result = "";
                    break;
            }

            return result;
        }
    }
}
