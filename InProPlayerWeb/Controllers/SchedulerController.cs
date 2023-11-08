using InProPlayerWeb.Helper;
using InProPlayerWeb.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace InProPlayerWeb.Controllers
{
    public class SchedulerController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IWebHostEnvironment _hostingEnvironment;

        private readonly int areaCount = 20;
        private Dictionary<string, string> weekDayDict = new Dictionary<string, string>()
        {
            {"2", "星期一"}, {"3", "星期二"}, {"4", "星期三"}, {"5", "星期四"}, {"6", "星期五"}, {"7", "星期六"}, {"1", "星期日"}
        };
        private Dictionary<string, string> monthDayDict = new Dictionary<string, string>()
        {
            {"1", "31"}, {"2", "29"}, {"3", "31"}, {"4", "30"}, {"5", "31"}, {"6", "30"},
            {"7", "31"}, {"8", "31"}, {"9", "30"}, {"10", "31"}, {"11", "30"}, {"12", "31"}
        };


        public SchedulerController(IWebHostEnvironment hostingEnvironment, ApplicationDbContext context)
        {
            _hostingEnvironment = hostingEnvironment;
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
            string uploadsFolder = Path.Combine(_hostingEnvironment.WebRootPath, "uploads");
            string[] filePath = Directory.GetFiles(uploadsFolder, "*.mp3");
            string[] fileName = filePath.Select(filePath => Path.GetFileName(filePath)).ToArray();

            List<Group> group = _context.Group.ToList();
            ViewBag.GroupSelect = group;
            ViewBag.FileName = fileName;
            ViewBag.WeekDict = weekDayDict;
            ViewBag.areaCount = areaCount;
            return View();
        }
        [HttpGet]
        public IActionResult Edit(int id)
        {
            string uploadsFolder = Path.Combine(_hostingEnvironment.WebRootPath, "uploads");
            string[] filePath = Directory.GetFiles(uploadsFolder, "*.mp3");
            string[] fileName = filePath.Select(filePath => Path.GetFileName(filePath)).ToArray();

            List<Group> group = _context.Group.ToList();
            ViewBag.GroupSelect = group;
            ViewBag.FileName = fileName;
            ViewBag.WeekDict = weekDayDict;
            ViewBag.areaCount = areaCount;

            Scheduler scheduler = _context.Scheduler.Find(id);

            SchedulerEdit newScheduler = new SchedulerEdit
            {
                id = id,
                SchedulerName = scheduler.SchedulerName,
                GroupID = scheduler.GroupID,
                LoopType = scheduler.LoopType,
                LoopTimes = scheduler.LoopTimes,
                KeepTimes = scheduler.KeepTimes,
                Music = scheduler.Music,
                Time = CronReString(scheduler.StartCron, "Time"),
                Week = CronReString(scheduler.StartCron, "WeekDay").Split(","),
                Terminal = scheduler.Terminal.Split(",")
            };

            if (newScheduler != null)
            {
                return View(newScheduler);
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
        public IActionResult Append(SchedulerAppend scheduler)
        {
            //SchedulerModel
            Scheduler newScheduler = new Scheduler
            {
                SchedulerName = scheduler.SchedulerName,
                GroupID = scheduler.GroupID,
                LoopType = scheduler.LoopType,
                LoopTimes = scheduler.LoopTimes,
                KeepTimes = scheduler.KeepTimes,
                Music = scheduler.Music,
                StartCron = CronString(scheduler.Time.ToString("ss mm HH"), scheduler.Week),
                Terminal = TerminalString(scheduler.Terminal)
            };

            _context.Scheduler.Add(newScheduler);
            _context.SaveChanges();
            return RedirectToAction("Index");
        }
        [HttpPost]
        public IActionResult Edit(SchedulerAppend scheduler)
        {
            //SchedulerModel
            Scheduler newScheduler = new Scheduler
            {
                id = scheduler.id,
                SchedulerName = scheduler.SchedulerName,
                GroupID = scheduler.GroupID,
                LoopType = scheduler.LoopType,
                LoopTimes = scheduler.LoopTimes,
                KeepTimes = scheduler.KeepTimes,
                Music = scheduler.Music,
                StartCron = CronString(scheduler.Time.ToString("ss mm HH"), scheduler.Week),
                Terminal = TerminalString(scheduler.Terminal)
            };
            
            _context.Scheduler.Update(newScheduler);
            _context.SaveChanges();
            return RedirectToAction("Index");
        }

        public string TerminalString(string[] Terminal)
        {
            string[] result = new string[areaCount];

            for(int i = 0; i < areaCount; i++)
            {
                if (Terminal.Any(s => s == (i + 1).ToString()))
                {
                    result[i] = "1";
                }
                else
                {
                    result[i] = "0";
                }
            }

            return string.Join(',', result);
        }
        public string CronString(string sTime, string[] Days)
        {
            string Day = "";
            if(Days.Any(s => s == "0")) {
                Day = "*";
            }
            else
            {
                Day = string.Join(",", Days);
            }

            //result 補 日 月 * 格式: Start Cron:End Cron
            string result = sTime + " ? * " + Day;
            return result;
        }
        public string CronReString(string Cron, string reType)
        {
            string result = "";
            string[] cronString = Cron.Split(' ');

            switch (reType)
            {
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
                        result = "每日";
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
