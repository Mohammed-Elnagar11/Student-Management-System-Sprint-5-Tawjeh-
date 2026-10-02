using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Student_Management_System__Sprint_5_Tawjeh_.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Student_Management_System__Sprint_5_Tawjeh_.Data.Configurations
{
    internal class EnrollmentConfiguration : IEntityTypeConfiguration<Enrollment>
    {
        public void Configure(EntityTypeBuilder<Enrollment> builder)
        {
            builder.HasIndex(e => new { e.StudentId, e.CourseId })
            .IsUnique();

            builder.Property(e => e.EnrollmentDate)
                .IsRequired();

            // Nullable Grade
            builder.Property(e => e.Grade)
                .IsRequired(false);
        }
    }
}
