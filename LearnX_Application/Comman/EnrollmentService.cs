using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using LearnX_Data.EF;
using LearnX_Data.Entities;
using LearnX_ModelView.Catalog.Enrollment;
using LearnX_Utilities.Exceptions;
using Microsoft.EntityFrameworkCore;

namespace LearnX_Application.Comman
{
    public class EnrollmentService : IEnrollmentService
    {
        public LearnXDbContext _context;
        private readonly IMapper _mapper;

        public EnrollmentService(LearnXDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<int> Create(EnrollmentRequest enrollment)
        {

            Enrollment enrollment1 = _mapper.Map<Enrollment>(enrollment);
            enrollment1.Progress = 0;
            await _context.Enrollments.AddAsync(enrollment1);
            await _context.SaveChangesAsync();
            return enrollment1.EnrollmentID;
        }

        public async Task<Enrollment> GetEnrollment(int id)
        {
            return await _context.Enrollments.FindAsync(id);
        }
        public async Task<int> Outclass(int id, Guid idUser)
        {

            var enrollment = await _context.Enrollments.FirstOrDefaultAsync(n => n.CourseID == id && n.UserID == idUser);

            if (enrollment == null)
            {
                throw new MyClassException($"Cannot find enrollment with CourseID: {id} and UserID: {idUser}");
            }

            // Xóa Enrollment
            _context.Enrollments.Remove(enrollment);

            // Lưu thay đổi vào cơ sở dữ liệu
            return await _context.SaveChangesAsync();
        }
    }
}