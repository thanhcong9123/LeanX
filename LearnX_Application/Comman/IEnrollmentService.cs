using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LearnX_Data.Entities;
using LearnX_ModelView.Catalog.Enrollment;

namespace LearnX_Application.Comman
{
    public interface IEnrollmentService
    {
        Task<int> Create(EnrollmentRequest enrollment);
        Task<Enrollment> GetEnrollment(int id);
        Task<int> Outclass(int id, Guid idUser);
    }
}