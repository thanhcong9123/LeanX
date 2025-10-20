using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LearnX_Data.Entities;

namespace LearnX_ModelView.Catalog.ViewApp
{
    public class HomeModel
    {
        public List<Course> Courses { get; set; }
        public List<LearnX_Data.Entities.Exercise> Exercises { get; set; }
        public List<LearnX_Data.Entities.Exercise> ExercisesNotDone { get; set; }

    }
}