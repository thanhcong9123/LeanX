using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LearnX_Data.Entities;
using LearnX_ModelView.Catalog.EBook;
using LearnX_ModelView.Common;

namespace LearnX_ApiIntegration
{
    public interface IEBookApiClient
    {
        Task<List<EBook>> GetBooksAsync();
        Task<EBook> GetBookByIdAsync(int id);
        Task<ApiResult<string>> UploadBookAsync(EBookRequest eBookRequest);
         Task<bool> DeleteBookAsync(int id);
    }
}