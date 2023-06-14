using EShopOnlineExam.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System.Reflection.Emit;

namespace EShopOnlineExam.Data
{
	public class MyDbContext : IdentityDbContext
	{

		public MyDbContext(DbContextOptions options): base(options)
		{

		}
		DbSet<Candidate> Candidates { get; set; }
		DbSet<CandidateCertificate> CandidateCertificates { get; set; }
		DbSet<Certificate> Certificates { get; set; }
		DbSet<ExamQuestion> ExamQuestions { get; set; }
		DbSet<Exam> Exams { get; set; }
		DbSet<ExamTopics> ExamTopics { get; set; }
		DbSet<Topic> Topics { get; set; }
		DbSet<CandidateExamination> CandidateExaminations { get; set; }
		DbSet<CandidateResults> CandidateResults { get; set; }
		DbSet<QuestionAnswers> QuestionAnswers { get; set; }

        DbSet<Order> Order { get; set; }
        DbSet<CandidateCart> CandidateCart { get; set; }
        DbSet<ToDoList> ToDoList { get; set; }
       
        DbSet<Messages> Messages { get; set; }
        DbSet<QcNote> QcNotes { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.HasSequence<int>("CandidateNumber");
            builder.Entity<Candidate>()
                .Property(o => o.CandidateNumber)
                .HasDefaultValueSql("NEXT VALUE FOR CandidateNumber");
            builder.HasSequence<int>("CandidateNumber", schema: "shared").StartsAt(1).IncrementsBy(1);
            
            builder.Entity<ExamTopics>()
            .HasOne(e => e.Topic)
            .WithMany()
            .OnDelete(DeleteBehavior.NoAction);


            builder.Entity<ExamQuestion>()
            .HasOne(e => e.QuestionAnswer)
            .WithMany()
            .OnDelete(DeleteBehavior.NoAction);


            builder.Entity<CandidateResults>()
            .HasOne(e => e.ExamQuestion)
            .WithMany()
            .OnDelete(DeleteBehavior.NoAction);

            builder.Entity<Messages>()
                .HasOne<Candidate>(c => c.Sender)
                .WithMany(m => m.Messages)
                .HasForeignKey(u => u.UserId);
        }

        
    }

	
}

