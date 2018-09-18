using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using ContosoUniversity.Models;

namespace ContosoUniversity.Pages.Students
{
    public class IndexModel : PageModel
    {
        private readonly SchoolContext _context;

        public IndexModel(SchoolContext context)
        {
            _context = context;
        }

        public IList<Student> Student { get; set; }

        public string NameSort { get; set; }
        public string DateSort { get; set; }
        public string CurrentFilter { get; set; }
        public string CurrentSort { get; set; }


        public async Task OnGetAsync(string sortOrder, string searchString)
        {
            NameSort = String.IsNullOrEmpty(sortOrder) ? "name_desc" : "";
            DateSort = sortOrder == "Date" ? "date_desc" : "Date";
            CurrentFilter = searchString;

            //this is a query 
            IQueryable<Student> studentIQ = from s in _context.Student
                                            select s;
            //
            if (!String.IsNullOrEmpty(searchString))
            {
                //called ToUpper() to fix case-sensitivity on searchString
                studentIQ = studentIQ.Where(s => s.LastName.ToUpper().Contains(searchString.ToUpper())
                                            || s.FirstMidName.ToUpper().Contains(searchString.ToUpper()));
            }

            //sort for column links
            switch (sortOrder)
            {
                case "name_desc":
                    studentIQ = studentIQ.OrderByDescending(s => s.LastName);
                    break;

                case "Date":
                    studentIQ = studentIQ.OrderBy(s => s.EnrollmentDate);
                    break;

                case "date_desc":
                    studentIQ = studentIQ.OrderByDescending(s => s.EnrollmentDate);
                    break;

                default:
                    studentIQ = studentIQ.OrderBy(s => s.LastName);
                    break;
            }
            //end of query ordered

            //IQueryable is executed when ToListAsync is called
            Student = await studentIQ.AsNoTracking().ToListAsync();
        }
    }
}
