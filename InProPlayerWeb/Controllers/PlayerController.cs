using InProPlayerWeb.Helper;
using Microsoft.AspNetCore.Mvc;
using System;

namespace InProPlayerWeb.Controllers
{
    public class PlayerController : Controller
    {
        private readonly IWebHostEnvironment _hostingEnvironment;
        private readonly ApplicationDbContext _context;
        private readonly NAudioHelper _np;

        //NAudioHelper np = new NAudioHelper();

        public PlayerController(IWebHostEnvironment hostingEnvironment, ApplicationDbContext context, NAudioHelper np)
        {
            _hostingEnvironment = hostingEnvironment;
            _context = context;
            _np = np;
        }

        [HttpGet("Player/Index/{page?}")]
        public IActionResult Index(int page = 1)
        {
            // 資料夾的路徑
            string folderPath = Path.Combine(_hostingEnvironment.WebRootPath, "uploads");

            // 建立一個清單來儲存檔案名稱
            List<string> fileList = new List<string>();

            // 讀取資料夾內的檔案名稱
            string[] files = Directory.GetFiles(folderPath);
            foreach (string file in files)
            {
                string fileName = Path.GetFileName(file);
                fileList.Add(fileName);
            }

            PageHelper ph = new PageHelper();
            ph.page = page;            
            ph.pageSize = 10;
            ph.pageList = fileList;

            ViewBag.TotalPages  = ph.TotalPage();
            ViewBag.CurrentPage = ph.page;
            ViewBag.fileList    = ph.PageList();

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Upload(List<IFormFile> files)
        {
            // 檢查是否有選擇檔案
            if (files != null && files.Count > 0)
            {
                foreach (var file in files)
                {
                    // 儲存檔案到伺服器的指定路徑
                    string uploadsFolder = Path.Combine(_hostingEnvironment.WebRootPath, "uploads");
                    string uniqueFileName = GetUniqueFileName(file.FileName);
                    string filePath = Path.Combine(uploadsFolder, uniqueFileName);
                    using (var fileStream = new FileStream(filePath, FileMode.Create))
                    {
                        await file.CopyToAsync(fileStream);
                    }
                }

                // 上傳完成後進行相應的處理，例如返回成功訊息
                return RedirectToAction("Index");
            }

            // 如果沒有選擇檔案，返回錯誤訊息
            ModelState.AddModelError("", "請選擇檔案");
            return View("Index");
        }
        private string GetUniqueFileName(string fileName)
        {
            string uniqueFileName = Path.GetFileNameWithoutExtension(fileName)
                + "_" + Path.GetRandomFileName().Substring(0, 4)
                + Path.GetExtension(fileName);
            return uniqueFileName;
        }

        [HttpPost]
        public void PlaySelector(string fileName)
        {
            string uploadsFolder = Path.Combine(_hostingEnvironment.WebRootPath, "uploads");
            _np.audioFilePath = uploadsFolder+"\\"+fileName;
            _np.Play();
        }

        [HttpPost]
        public string Play(string fileName)
        {
            string uploadsFolder = Path.Combine(_hostingEnvironment.WebRootPath, "uploads");
            _np.audioFilePath = uploadsFolder + "\\" + fileName;
            _np.Play();
            return "success";
        }
        [HttpPost]
        public string Stop()
        {
            _np.Stop();
            return "success";
        }
    }
}
