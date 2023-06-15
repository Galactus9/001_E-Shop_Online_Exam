using Bogus;
using EShopOnlineExam.Data;
using EShopOnlineExam.Repository.IRepository;
using Microsoft.EntityFrameworkCore;

namespace EShopOnlineExam.Services
{
    public static class SeedDB
    {
        public static async Task Seed(MyDbContext db)
        {

            try
            {
                bool migrated = await Migrate(db);

                SeedStandaloneTables(db);


            }
            catch (Exception Ex)
            {
                throw new Exception(Ex.Message);
            }
        }

        public static void SeedStandaloneTables(MyDbContext db)
        {
            List<Certificate> certificates = new List<Certificate>();
            var certificate1 = new Certificate
            {
                Title = "DevOps",
                Description = "DevOps",
                PossibleMarks = 100,
                State = CertificateStatus.Enabled,
                Price = 100,

            };
            certificates.Add(certificate1);
            var certificate2 = new Certificate
            {
                Title = "Python",
                Description = "Python",
                PossibleMarks = 100,
                State = CertificateStatus.Enabled,
                Price = 100,

            };
            certificates.Add(certificate2);
            var certificate3 = new Certificate
            {
                Title = "Javascript",
                Description = "Javascript",
                PossibleMarks = 100,
                State = CertificateStatus.Enabled,
                Price = 100,

            };
            certificates.Add(certificate3);

            db.Set<Certificate>().AddRange(certificates);
            db.SaveChanges();


            // Topic
            List<Topic> topics = new List<Topic>();
            foreach (Certificate certificate in certificates)
            {
                for(int i = 1; i <= 3; i++)
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

        public static Exam SeedAnExam(IUnitOfWork _unitOfWork, int certId)
        {
            var cert = _unitOfWork.Certificate.Get(certId);


            Random rnd = new Random();
            List<int> subjectWeight = new List<int>();
            subjectWeight.Add(rnd.Next(1, 5));
            subjectWeight.Add(rnd.Next(1, 5));
            subjectWeight.Add(rnd.Next(1, 5));
            var exam = new Exam { Certificate = cert };
            var examTopics = new List<ExamTopics>();
            List<Topic> topics = (List<Topic>)_unitOfWork.Topic.GetTopicByCertificate(cert.Id);
            for (int i = 0; i < topics.Count(); i++)
            {
                var examTopic = new ExamTopics { Topic = topics[i], Exam = exam, SubjectWeight = subjectWeight[i] };
                examTopics.Add(examTopic);
                List<ExamQuestion> examQuestions = new List<ExamQuestion>();
                List<int> randomNumbers = new List<int>();
                ICollection<QuestionAnswers> topicQuestion = _unitOfWork.QuestionAnswers.WhereTopicId(topics[i].Id);
                while (randomNumbers.Count < topicQuestion.Count())
                {
                    int newNum = rnd.Next(topicQuestion.First().Id, topicQuestion.Last().Id); // generates a random number between 1 and 100
                    if (!randomNumbers.Contains(newNum))
                    {
                        randomNumbers.Add(newNum);
                        ExamQuestion question = new ExamQuestion { ExamTopics = examTopic, QuestionAnswer = _unitOfWork.QuestionAnswers.Get(newNum) };
                        examQuestions.Add(question);
                    }
                }
                _unitOfWork.ExamQuestion.AddRange(examQuestions);
            }
            _unitOfWork.ExamTopic.AddRange(examTopics);
            exam.ExamDuration = 600;

            _unitOfWork.Exam.Add(exam);
            _unitOfWork.Save();
            return exam;
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
