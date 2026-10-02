using Student_Management_System__Sprint_5_Tawjeh_.Data;
using System;
using System.Collections.Generic;
using System.Text;

namespace Student_Management_System__Sprint_5_Tawjeh_.Services
{
    internal class EnrollmentService
    {
        private readonly AppDbContext context;

        public EnrollmentService(AppDbContext context)
        {
            this.context = context;
        }
    }
}
