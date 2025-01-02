using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LearnX_ModelView.Catalog.Courses;
using LearnX_ModelView.Catalog.Enrollment;

namespace LearnX_ApiIntegration
{
    public interface IEnrollmentClient
    {
        Task<bool> Create(EnrollmentRequest enrollmentRequest);
        Task<MyCourses> GetmyCourse(Guid id);
        Task<bool> Outclass(OutclassRequest id);

    }
}