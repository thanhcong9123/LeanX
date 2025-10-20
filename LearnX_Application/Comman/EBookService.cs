using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
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
        private readonly IMapper _mapper;


        public EBookService(LearnXDbContext context, IWebHostEnvironment environment, IMapper mapper)
        {
            _context = context;
            _environment = environment;
            _mapper = mapper;
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

            return book;
        }
        public async Task<bool> AddEvaluateAsync(EvaluateBookRequest request)
        {
            try
            {
                var evaluate = _mapper.Map<EvaluateBook>(request);
                evaluate.CreatedAt = DateTime.Now;
                await _context.EvaluateBooks.AddAsync(evaluate);
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
                // Save book details to database
                var book = _mapper.Map<EBook>(eBookRequest);
                book.UploadedAt = DateTime.Now;
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