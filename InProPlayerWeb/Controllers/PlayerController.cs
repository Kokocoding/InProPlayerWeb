using InProPlayerWeb.Helper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System;

namespace InProPlayerWeb.Controllers
{
    public class PlayerController : Controller
    {
        private readonly IWebHostEnvironment _hostingEnvironment;
        private readonly ApplicationDbContext _context;
        private readonly NAudioHelper _np;

        
        public PlayerController(IWebHostEnvironment hostingEnvironment, ApplicationDbContext context, NAudioHelper np)
        {
            _hostingEnvironment = hostingEnvironment;
            _context = context;
            _np = np;
        }

        [HttpGet("Player/Index/{page?}")]
        public IActionResult Index(int page = 1)
        {
            PageHelper ph = new PageHelper();
            ph.page = page;            
            ph.pageSize = 10;
            ph.pageList = getFileFolder();

            ViewBag.TotalPages  = ph.TotalPage();
            ViewBag.CurrentPage = ph.page;
            ViewBag.fileList    = ph.PageList();

            return View();
        }
        public List<string> getFileFolder()
        {
            // 資料夾的路徑
            string folderPath = Path.Combine(_hostingEnvironment.WebRootPath, "uploads");

            List<string> fileList = new List<string>();// 建立一個清單來儲存檔案名稱
            // 讀取資料夾內的檔案名稱
            string[] files = Directory.GetFiles(folderPath);
            foreach (string file in files)
            {
                string fileName = Path.GetFileName(file);
                fileList.Add(fileName);
            }
            return fileList;
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
            else
            {
                return View("Index");
            }
        }
        private string GetUniqueFileName(string fileName)
        {
            string uniqueFileName = Path.GetFileNameWithoutExtension(fileName)
                + "_" + Path.GetRandomFileName().Substring(0, 4)
                + Path.GetExtension(fileName);
            return uniqueFileName;
        }

        [HttpPost]
        public double Play(string fileName, double startTime)
        {
            string uploadsFolder = Path.Combine(_hostingEnvironment.WebRootPath, "uploads");
            _np.audioFilePath = uploadsFolder + "\\" + fileName;
            TimeSpan timeSpan = TimeSpan.FromSeconds(startTime);
            _np.startTime = timeSpan;
            return _np.Play();
        }
        [HttpPost]
        public void Pause()
        {
            _np.Pause();
        }
        [HttpPost]
        public void Stop()
        {
            _np.Stop();
            _np.startTime = TimeSpan.Zero;
        }
        [HttpPost]
        public Dictionary<string, object> GetInit()
        {
            return _np.GetInit();
        }
        [HttpPost]
        public void SetVolume(float volume)
        {
            _np.SetVolume(volume/100);
        }
        [HttpPost]
        public string Track(string fileName, int Track, int flag)
        {
            Stop();            
            List<string> fileList = getFileFolder();
            string nextItem = fileList[0];

            //第一首
            if(flag < 0) return nextItem;

            int index = fileList.FindIndex(item => item == fileName);
            if ((index + Track > -1) && (index + Track < fileList.Count))
            {
                // 確保 index+1 不超出範圍
                nextItem = fileList[index + Track];
            }

            //最後一首
            if(index + Track == fileList.Count)
            {
                nextItem = fileList[index];
            }

            return nextItem;
        }
    }
}
