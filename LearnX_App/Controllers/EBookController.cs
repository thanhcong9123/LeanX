using LearnX_ApiIntegration;
using LearnX_ApiIntegration.FileService;
using LearnX_Data.Entities;
using LearnX_ModelView.Catalog.EBook;
using Microsoft.AspNetCore.Mvc;

namespace MyApp.Namespace
{
    public class EBookController : Controller
    {
        // GET: EBookController
        private readonly IEBookApiClient _ebookApiClient;
        private readonly IFileUpLoadServices _fileUpLoadServices;

        public EBookController(IEBookApiClient ebookApiClient,
        IFileUpLoadServices fileUpLoadServices)
        {
            _ebookApiClient = ebookApiClient;
            _fileUpLoadServices = fileUpLoadServices;
        }

        // Danh sách sách
        public async Task<IActionResult> Index()
        {
            var books = await _ebookApiClient.GetBooksAsync();
            return View(books);
        }

        // Chi tiết sách
        public async Task<IActionResult> Details(int id)
        {
            if (id <= 0)
            {
                return BadRequest(id);
            }
            var book = await _ebookApiClient.GetBookByIdAsync(id);
            if (book == null)
            {
                return NotFound();
            }
            var model = new EBook()
            {
                Id = book.Id,
                Title = book.Title,
                Description = book.Description,
                imgPath = book.imgPath,
                FilePath = book.FilePath
            };
            return View(model);
        }

        // Upload sách
        [HttpGet]
        public IActionResult Upload()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Upload(EBookRequest model)
        {

            if (model != null)
            {
                
                var result = await _ebookApiClient.UploadBookAsync(model);
                if (result)
                {
                    TempData["Success"] = "Book uploaded successfully.";
                    return RedirectToAction(nameof(Index));
                }
            }

            TempData["Error"] = "Failed to upload book.";
            return RedirectToAction(nameof(Index));
        }
        [HttpPost]
        public ActionResult UpLoadFile(IFormFile formFile)
        {
            if (formFile != null)
            {
                var filePath = _fileUpLoadServices.UpLoadFile(formFile);
                return Json(formFile.FileName);
            }
            return NotFound();
        }
        // Xóa sách
        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _ebookApiClient.DeleteBookAsync(id);
            if (result)
            {
                TempData["Success"] = "Book deleted successfully.";
            }
            else
            {
                TempData["Error"] = "Failed to delete book.";
            }
            return RedirectToAction(nameof(Index));
        }

    }
}
