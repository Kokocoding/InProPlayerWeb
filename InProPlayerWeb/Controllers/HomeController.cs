using Microsoft.AspNetCore.Mvc;
using InProPlayerWeb.Helper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Data.Sqlite;

namespace InProWeb.Controllers
{
    public class HomeController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly PortHelper _portHelper;

        private readonly int areaCount = 20;
        private readonly int groupCount = 6;        
        private bool[] ZoneStatus = Enumerable.Repeat(false, 20).ToArray();

        public HomeController(PortHelper portHelper, ApplicationDbContext context)
        {
            _portHelper = portHelper;
            _context = context;
            var configDB = _context.Configuration.ToList();
            _portHelper.PortName = configDB[0].PortName;
            _portHelper.BaudRate = configDB[0].BaudRate;
        }

        public IActionResult Index()
        {
            ViewData["areaCount"] = areaCount;
            ViewData["groupCount"] = groupCount;
            return View();
        }

        [HttpPost]
        public string SendCmd(string type, string num, bool onoff)
        {
            byte[] cmd = new byte[7] { 0xAA, 0xA1, 0xFF, 0xFF, 0xFF, 0xFF, 0xE0 };
            ZoneStatus[int.Parse(num)] = onoff;
            _portHelper.PortWrite(_portHelper.CreateCommand(ZoneStatus));
            return num;
        }
    }
}