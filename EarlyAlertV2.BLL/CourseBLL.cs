using System;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;
using EarlyAlertV2.Interfaces.BLL;
using EarlyAlertV2.Interfaces.Repository;
using EarlyAlertV2.Models;

namespace EarlyAlertV2.BLL
{
    public class CourseBLL : ICourseBLL
    {
        private readonly ICourseRepository courseRepository;

        public CourseBLL(ICourseRepository courseRepository)
        {
            this.courseRepository = courseRepository;
        }

        public Course Add(Course model)
        {
            model.DateCreated = DateTime.Now;
            return courseRepository.Add(model);
        }

        public IEnumerable<Course> GetAll()
        {
            return courseRepository.GetAll();
        }

        public Course Get(int modelId)
        {
            return courseRepository.Get(modelId);
        }

        public Course GetByCanvasId(int canvasId)
        {
            return courseRepository.GetByCanvasId(canvasId);
        }

        public Course Update(Course model)
        {
            model.LastUpdated = DateTime.Now;
            return courseRepository.Update(model);
        }
    }
}