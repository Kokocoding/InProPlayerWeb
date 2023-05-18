using Microsoft.AspNetCore.Mvc;
using InProPlayerWeb.Helper;

namespace InProWeb.Controllers
{
    public class HomeController : Controller
    {
        private readonly int areaCount = 20;
        private readonly int groupCount = 6;
        private readonly IPortHelper _portHelper;
        private bool[] ZoneStatus = Enumerable.Repeat(false, 20).ToArray();

        public HomeController(IPortHelper portHelper)
        {
            _portHelper = portHelper;
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