using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LearnX_Data.EF;
using LearnX_Data.Entities;
using LearnX_ModelView.Catalog.EBook;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace LearnX_Application.Comman
{
    public class EBookService : IEBookService
    {
        public LearnXDbContext _context;
        private readonly IWebHostEnvironment _environment;

        public EBookService(LearnXDbContext context)
        {
            _context = context;
        }

        public async Task<bool> DeleteBookAsync(int id)
        {
            var book = await _context.EBooks.FindAsync(id);
            if (book == null) return false;



            // Delete book record from database
            _context.EBooks.Remove(book);
            await _context.SaveChangesAsync();

            return true;
        }

        public async Task<EBook> GetBookByIdAsync(int id)
        {
            var book = await _context.EBooks.FindAsync(id);
            if (book == null)
            {
                return null;
            }

            return new EBook
            {
                Id = book.Id,
                Title = book.Title,
                Description = book.Description,
                FilePath = book.FilePath,
                UploadedAt = book.UploadedAt,
                LinkYoutube = book.LinkYoutube,
                imgPath = book.imgPath
            };
        }
        public async Task<bool> AddEvaluateAsync(EvaluateBookRequest request)
        {
            try
            {
                var evaluate = new EvaluateBook
                {
                    BookId = request.BookId,
                    UserId = request.UserId,
                    Rating = request.Rating,
                    Comment = request.Comment,
                    CreatedAt = DateTime.UtcNow
                };
                _context.EvaluateBooks.Add(evaluate);
                await _context.SaveChangesAsync();
                return true;
            }
            catch
            {
                return false;
            }
        }
        public async Task<List<EBook>> GetBooksAsync()
        {
            return await _context.EBooks.
            ToListAsync();
        }

        public async Task<bool> UploadBookAsync(EBookRequest eBookRequest)
        {
            try
            {
                // Save file to wwwroot/files


                // Save book details to database
                var book = new EBook
                {
                    Title = eBookRequest.Title,
                    imgPath = eBookRequest.imgPath,
                    Description = eBookRequest.Description,
                    FilePath = eBookRequest.FilePath, // Save relative path
                    UploadedAt = DateTime.Now,
                    LinkYoutube = eBookRequest.LinkYoutube,
                    CountPages = eBookRequest.CountPages,
                    Author = eBookRequest.Author,
                    Status = eBookRequest.Status,
                    NameCategory = eBookRequest.NameCategory
                };
                _context.EBooks.Add(book);
                await _context.SaveChangesAsync();

                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}