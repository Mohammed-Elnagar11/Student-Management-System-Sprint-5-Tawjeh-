using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Student_Management_System__Sprint_5_Tawjeh_.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Student_Management_System__Sprint_5_Tawjeh_.Data.Configurations
{
    internal class InstructorConfiguration : IEntityTypeConfiguration<Instructor>
    {
        public void Configure(EntityTypeBuilder<Instructor> builder)
        {
            builder.Property(i => i.FullName)
            .IsRequired()
            .HasMaxLength(150);
        }
    }
}
