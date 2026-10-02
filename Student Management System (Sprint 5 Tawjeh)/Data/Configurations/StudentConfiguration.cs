using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Student_Management_System__Sprint_5_Tawjeh_.Entities;

namespace Student_Management_System__Sprint_5_Tawjeh_.Data.Configurations
{
    internal class StudentConfiguration : IEntityTypeConfiguration<Student>
    {
        public void Configure(EntityTypeBuilder<Student> builder)
        {
            builder.HasKey (s => s.Id);

            builder.Property(s => s.FullName)
            .IsRequired()
            .HasMaxLength(150);

            builder.Property(s => s.Email)
                .IsRequired()
                .HasMaxLength(200);

            builder.HasIndex(s => s.Email)
                .IsUnique();

            builder.Property(s => s.DateOfBirth)
                .IsRequired();

            builder.Property(s => s.EnrollmentDate)
                .IsRequired();

            builder.Property(s => s.IsDeleted)
                .IsRequired()
                .HasDefaultValue(false);

            builder.HasQueryFilter(s => !s.IsDeleted);

            builder.HasMany(s => s.Enrollments)
                .WithOne(e => e.Student)
                .HasForeignKey(e => e.StudentId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
