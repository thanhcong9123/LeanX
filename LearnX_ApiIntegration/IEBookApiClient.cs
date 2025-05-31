using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LearnX_Data.Entities;
using LearnX_ModelView.Catalog.EBook;

namespace LearnX_ApiIntegration
{
    public interface IEBookApiClient
    {
        Task<List<EBook>> GetBooksAsync();
        Task<EBook> GetBookByIdAsync(int id);
        Task<bool> UploadBookAsync(EBookRequest eBookRequest);
         Task<bool> DeleteBookAsync(int id);
    }
}