using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using LearnX_Data.EF;
using LearnX_Data.Entities;
using LearnX_ApiIntegration;

namespace LearnX_App.Controllers
{
    public class CourseController : Controller
    {
        private readonly ICourseApiClient _context;

        public CourseController(ICourseApiClient context)
        {
            _context = context;
        }
        public IActionResult Home()
        {
            return View();
        }

        // GET: Course
        public async Task<IActionResult> Index()
        {
            return View();
        }
        public async Task<IActionResult> Exercise()
        {
            return View();
        }

        // GET: Course/Details/5
        public async Task<IActionResult> Details(int? id)
        {
           
            return View();
        }

        // GET: Course/Create
        public IActionResult Create()
        {
      
            return View();
        }

        // POST: Course/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("CourseID,CourseName,Description,InstructorID,CategoryID,StartDate,EndDate,Price")] Course course)
        {
           
            return View(course);
        }

        // GET: Course/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
          
            return View();
        }

        // POST: Course/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("CourseID,CourseName,Description,InstructorID,CategoryID,StartDate,EndDate,Price")] Course course)
        {
           
            return View(course);
        }

        // GET: Course/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
         
            return View();
        }

        // POST: Course/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
         
            return RedirectToAction(nameof(Index));
        }

        
    }
}
