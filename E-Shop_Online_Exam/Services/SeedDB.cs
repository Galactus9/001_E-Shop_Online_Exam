using Bogus;
using EShopOnlineExam.Data;
using EShopOnlineExam.Repository.IRepository;
using Microsoft.EntityFrameworkCore;
using Org.BouncyCastle.Ocsp;


namespace EShopOnlineExam.Services
{
    public static class SeedDB
    {
        public static async Task Seed(MyDbContext db)
        {

            try
            {
                bool migrated = await Migrate(db);

                SeedCertificates(db);
                SeedTopics(db);
                SeedQuestionAnswers(db);
				SeedExams(db);


            }
            catch (Exception Ex)
            {
                throw new Exception(Ex.Message);
            }
        }

        public static void SeedCertificates(MyDbContext db)
        {
            if (!db.Set<Certificate>().Any())
            {
                List<Certificate> certificates = new List<Certificate>();
                var certificate1 = new Certificate
                {
                    Title = "DevOps",
                    Description = "DevOps",
                    PossibleMarks = 100,
                    State = Models.CertificateStatus.Enabled,
                    Price = 100,

                };
                certificates.Add(certificate1);
                var certificate2 = new Certificate
                {
                    Title = "Python",
                    Description = "Python",
                    PossibleMarks = 100,
                    State = Models.CertificateStatus.Enabled,
                    Price = 100,

                };
                certificates.Add(certificate2);
                var certificate3 = new Certificate
                {
                    Title = "Javascript",
                    Description = "Javascript",
                    PossibleMarks = 100,
                    State = Models.CertificateStatus.Enabled,
                    Price = 100,

                };
                certificates.Add(certificate3);

                db.Set<Certificate>().AddRange(certificates);
                db.SaveChanges();
            }
        }
        public static void SeedTopics(MyDbContext db)
        {
            // Topic
            if (!db.Set<Topic>().Any())
            {
                List<Topic> topics = new List<Topic>();
                List<Certificate> certificates = db.Set<Certificate>().ToList();
                foreach (Certificate certificate in certificates)
                {
                    for (int i = 1; i <= 3; i++)
                    {
                        topics.Add(new Topic
                        {
                            Title = $"{certificate} Topic {i}",
                            Description = $"This is the {i} topic for Certificate of {certificate}",
                            Certificate = certificate
                        });
                    }
                }
                db.Set<Topic>().AddRange(topics);
                db.SaveChanges();
            }
        }
        public static void SeedQuestionAnswers(MyDbContext db)
        {

            if (!db.Set<QuestionAnswers>().Any())
            {
                List<Topic> topics = db.Set<Topic>().ToList();
                List<QuestionAnswers> questionAnswers = new List<QuestionAnswers>();
                foreach (var topic in topics)
                {
                    for (int i = 1; i <= 5; i++)
                    {
                        int correctAnswer = new Random().Next(1, 4);
                        QuestionAnswers q = new QuestionAnswers
                        {
                            // Possible answersPython
                            Answer1 = "This is a generic answer to a question",
                            Answer2 = "This is a generic answer to a question",
                            Answer3 = "This is a generic answer to a question",
                            Answer4 = "This is a generic answer to a question",
                            CorrectIndex = correctAnswer,

                            // Question
                            TextOfQuestion = $"This is an example of a Question and the correct answer is: {correctAnswer}",
                            Topics = topic
                        };
                        questionAnswers.Add(q);
                    }
                }
                db.Set<QuestionAnswers>().AddRange(questionAnswers);
                db.SaveChanges(); 
            }
        }



        public static void SeedExams(MyDbContext db)
        {
            List<Certificate>? certificates = db.Set<Certificate>().ToList();
            foreach(var cert in certificates)
            {
                if (!db.Set<Exam>().Where(x =>x.Certificate.Id == cert.Id).Any())
                {
                    var exam = new Exam 
                    {
                        Certificate = cert,
                        ExamDate = DateTime.Now,
                        ExamDuration = 600
                    };

                    var examTopics = new List<ExamTopics>();

                    List<Topic> topics = db.Set<Topic>()
                        .Where(x => x.Certificate.Id == cert.Id)
                        .Include(x => x.Certificate)
                        .ToList();

                    for (int i = 0; i < topics.Count(); i++)
                    {
                        ExamTopics examTopic = new()
                        {
                            Topic = topics[i],
                            Exam = exam,
                            SubjectWeight = 3
                        };
                        examTopics.Add(examTopic);

                        List<ExamQuestion> examQuestions = new List<ExamQuestion>();
                        List<QuestionAnswers> topicQuestion = db.Set<QuestionAnswers>()
                                                                .Include(x => x.Topics)
                                                                .Where(x => x.Topics.Id == topics[i].Id)
                                                                .ToList();
                        for (int k = 0; k < examTopic.SubjectWeight; k++)
                        {
                            ExamQuestion question = new ExamQuestion
                            {
                                ExamTopics = examTopic,
                                QuestionAnswer = topicQuestion[k]
							};
                            examQuestions.Add(question);
                            db.Set<ExamQuestion>().AddRange(examQuestions);
                        }
                    }
                    db.Set<ExamTopics>().AddRange(examTopics);

                    db.Set<Exam>().Add(exam);
                    db.SaveChanges(); 
                }
            }
        }



        public static async Task<bool> Migrate(MyDbContext db)
        {
            try
            {
                Console.WriteLine("Attempting to apply migration...");

                await db.Database.MigrateAsync();

            }
            catch (Exception)
            {
                Console.WriteLine("ERROR: Tried to apply migration but failed.");
                Console.WriteLine("Do you want to continue to drop db and update? (y/n)");

                string input = Console.ReadLine();
                if (input.Equals("y", StringComparison.OrdinalIgnoreCase))
                {
                    Console.WriteLine("Dropping the database and migrating...");
                    await db.Database.EnsureDeletedAsync();
                    await db.Database.MigrateAsync();
                }
                else if (input.Equals("n", StringComparison.OrdinalIgnoreCase))
                {
                    Console.WriteLine("Exiting program...");
                    Environment.Exit(0);
                }
            }
            return true;
        }

    }
}
