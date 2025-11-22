using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LearnX_ApiIntegration
{
    public interface ICategoryApiClient
    {
        public Task<List<LearnX_Data.Entities.Category>> GetAll();
    }
}