using LearnX_Application.Comman;
using LearnX_ModelView.Catalog.EBook;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace MyApp.Namespace
{
    [Route("api/[controller]")]
    [ApiController]
    public class BooksController : ControllerBase
    {
        private readonly IEBookService _bookService;

        public BooksController(IEBookService bookService)
        {
            _bookService = bookService;
        }

        // Lấy danh sách sách
        [HttpGet]
        public async Task<IActionResult> GetBooks()
        {
            var books = await _bookService.GetBooksAsync();
            return Ok(books);
        }

        // Lấy thông tin chi tiết sách
        [HttpGet("{id}")]
        public async Task<IActionResult> GetBookById(int id)
        {
            var book = await _bookService.GetBookByIdAsync(id);
            if (book == null)
            {
                Console.WriteLine("Book not found.");
                return BadRequest();
            }

            return Ok(book);
        }

        // Tải lên sách (Admin)
        [HttpPost]
        public async Task<IActionResult> UploadBook([FromForm] EBookRequest eBookRequest)
        {
            if (eBookRequest.FilePath == null)
            {
                return BadRequest("No file selected.");
            }

            try
            {
                var result = await _bookService.UploadBookAsync(eBookRequest);
                if (result)
                {
                    return Ok(new { success = true, message = "Book uploaded successfully." });
                }
                else
                {
                    return StatusCode(500, "Failed to upload book.");
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        // Xóa sách (Admin)
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteBook(int id)
        {
            var result = await _bookService.DeleteBookAsync(id);
            if (result)
            {
                return Ok(result);
            }
            else
            {
                return BadRequest();
            }
        }
        [HttpPost("evaluate")]
        public async Task<IActionResult> AddEvaluate([FromBody] EvaluateBookRequest request)
        {
            var result = await _bookService.AddEvaluateAsync(request);
            if (result)
                return Ok(new { success = true, message = "Đánh giá thành công!" });
            return BadRequest(new { success = false, message = "Đánh giá thất bại!" });
        }
    }
}
