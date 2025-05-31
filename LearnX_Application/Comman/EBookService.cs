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
                UploadedAt = book.UploadedAt
            };
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
                    imgPath = eBookRequest.ImgPath,
                    Description = eBookRequest.Description,
                    FilePath = eBookRequest.IFormFile, // Save relative path
                    UploadedAt = DateTime.Now
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