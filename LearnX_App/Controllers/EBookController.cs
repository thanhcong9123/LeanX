using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using LearnX_ApiIntegration;
using LearnX_ApiIntegration.FileService;
using LearnX_App.Models;
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
        private readonly Cloudinary _cloudinary;
        private readonly HttpClient _httpClient;


        public EBookController(IEBookApiClient ebookApiClient,
                            IFileUpLoadServices fileUpLoadServices,
                            Cloudinary cloudinary,
                            HttpClient httpClient)
        {
            _ebookApiClient = ebookApiClient;
            _fileUpLoadServices = fileUpLoadServices;
            _cloudinary = cloudinary;
            _httpClient = httpClient ?? new HttpClient(); // <- gán HttpClient (tránh null)
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
                FilePath = book.FilePath,
                LinkYoutube = book.LinkYoutube,
                Author = book.Author,
                CountPages = book.CountPages,
                View = book.View,
                Status = book.Status,
                NameCategory = book.NameCategory,
                UploadedAt = book.UploadedAt
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
        public async Task<IActionResult> Upload(EbookViewModel model, [FromServices] Cloudinary cloudinary)
        {
            if (!ModelState.IsValid)
            {
                return RedirectToAction(nameof(Index));
            }
            if (model != null)
            {
                string? fileName = null;
                string? cloudFileUrl = null;
                string? cloudPublicId = null;
                if (model.FilePath != null && model.FilePath.Length > 0)
                {
                    try
                    {
                        using var stream = model.FilePath.OpenReadStream();
                        var uploadParams = new RawUploadParams()
                        {
                            File = new FileDescription(model.FilePath.FileName, stream),
                            Folder = "learnx/ebooks",
                        };
                        var uploadResult = await cloudinary.UploadAsync(uploadParams);
                        if (uploadResult.Error != null)
                        {
                            TempData["Error"] = "Lỗi Cloudinary: " + uploadResult.Error.Message;
                            return RedirectToAction(nameof(Upload));
                        }
                        cloudFileUrl = uploadResult.SecureUrl.ToString();
                        cloudPublicId = uploadResult.PublicId;
                        var book = new EBookRequest()
                        {
                            Title = model.Title,
                            Description = model.Description,
                            imgPath = model.imgPath,
                            FilePath = cloudFileUrl,
                            LinkYoutube = model.LinkYoutube,
                            Author = model.Author,
                            CountPages = model.CountPages,
                            NameCategory = model.NameCategory,
                            UploadedAt = DateTime.Now,
                            Status = model.Status

                        };
                        var result = await _ebookApiClient.UploadBookAsync(book);
                        if (result.IsSuccessed)
                        {
                            TempData["Success"] = "Book uploaded successfully.";
                            return RedirectToAction(nameof(Index));
                        }
                    }
                    catch (Exception ex)
                    {
                        TempData["Error"] = "File upload failed: " + ex.Message;
                        return RedirectToAction(nameof(Index));
                    }
                }

            }

            TempData["Error"] = "Failed to upload book.";
            return RedirectToAction(nameof(Index));
        }
        [HttpGet]
        public async Task<IActionResult> RedirectTo(string url)
        {
            if (string.IsNullOrEmpty(url))
                return BadRequest("Missing url");

            if (!Uri.TryCreate(url, UriKind.Absolute, out var uri) || (uri.Scheme != "http" && uri.Scheme != "https"))
                return BadRequest("Invalid url");

            // Optional: whitelist host(s) to avoid open proxy
            var allowedHosts = new[] { "res.cloudinary.com" };
            if (!allowedHosts.Contains(uri.Host, StringComparer.OrdinalIgnoreCase))
                return BadRequest("Host not allowed");

            try
            {
                var forwardRequest = new HttpRequestMessage(System.Net.Http.HttpMethod.Get, uri);

                // forward Range header if present (important for PDF viewers)
                if (Request.Headers.TryGetValue("Range", out var range))
                    forwardRequest.Headers.Add("Range", range.ToArray());

                var remoteResponse = await _httpClient.SendAsync(forwardRequest, HttpCompletionOption.ResponseHeadersRead);

                if (!remoteResponse.IsSuccessStatusCode && remoteResponse.StatusCode != System.Net.HttpStatusCode.PartialContent)
                    return StatusCode((int)remoteResponse.StatusCode);

                // copy selected headers
                foreach (var header in remoteResponse.Headers)
                    Response.Headers[header.Key] = header.Value.ToArray();
                foreach (var header in remoteResponse.Content.Headers)
                    Response.Headers[header.Key] = header.Value.ToArray();

                // remove hop-by-hop headers
                Response.Headers.Remove("transfer-encoding");

                if (remoteResponse.Content.Headers.ContentType != null)
                    Response.ContentType = remoteResponse.Content.Headers.ContentType.ToString();

                Response.StatusCode = (int)remoteResponse.StatusCode;

                // stream remote content to response body
                await remoteResponse.Content.CopyToAsync(Response.Body);
                return new EmptyResult();
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
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
        [HttpGet]
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
