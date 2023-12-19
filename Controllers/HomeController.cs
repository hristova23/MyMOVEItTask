using Microsoft.AspNetCore.Mvc;
using MyMOVEItTask.Models;
using System.Diagnostics;
using System.Net.Http.Headers;

namespace MyMOVEItTask.Controllers
{
    public class HomeController : Controller
    {
        private readonly IWebHostEnvironment hostingEnviroment;
        private readonly ILogger<HomeController> _logger;
        static HttpClient client = new HttpClient();
        static string hardCodedDirId = "115502127";

        public HomeController(IWebHostEnvironment _hostingEnviroment, ILogger<HomeController> logger)
        {
            hostingEnviroment = _hostingEnviroment;
            _logger = logger;
        }

        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Index(FileUploadViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            try
            {
                if (model.File != null)
                {
                    var url = $"https://mobile-1.moveitcloud.com/api/v1/folders/{hardCodedDirId}/files";

                    var filePath = Path.GetTempPath() + model.File.FileName;

                    using var stream = new FileStream(filePath, FileMode.CreateNew);
                    await model.File.CopyToAsync(stream);

                    using var fileContent = new StreamContent(stream);
                    fileContent.Headers.ContentType = MediaTypeHeaderValue.Parse("multipart/form-data");

                    using var formPayload = new MultipartFormDataContent
                    {
                        { fileContent, "file", Path.GetFileName(filePath) }
                    };

                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", model.AccessToken);
                    var response = await client.PostAsync(url, formPayload).ConfigureAwait(false);
                    var responseContent = await response.Content.ReadAsStringAsync();

                    if (response.IsSuccessStatusCode)
                        ViewBag.SuccessMessage = "File uploaded successfully.";
                    else
                        ModelState.AddModelError("", "Failed to upload file.");
                }

                return View(model);
            }
            catch (Exception)
            {
                ModelState.AddModelError("", "Something went wrong.");

                return View(model);
            }
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
