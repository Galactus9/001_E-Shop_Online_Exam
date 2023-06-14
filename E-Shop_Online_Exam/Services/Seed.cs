using EShopOnlineExam.Data;
using EShopOnlineExam.Models;
using EShopOnlineExam.Repository.IRepository;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Routing.Matching;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics.Metrics;
using System.Drawing.Text;
//using EShopOnlineExam.Utility;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace EShopOnlineExam.Services
{
	public class Seed
	{
        private readonly IUnitOfWork _unitOfWork;
        public Seed(IUnitOfWork unitOfWork) 
		{
            _unitOfWork = unitOfWork;
        }

        public void SeedDb()
		{
			// Certificates
			if (_unitOfWork.Certificate.GetAll().Count() == 0)
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

				_unitOfWork.Certificate.AddRange(certificates);
				_unitOfWork.Save();


				// Topic
				List<Topic> topics= new List<Topic>();
				var topic1Cert1 = new Topic
				{
					Title = "DevOps Topic 1",
					Description = "This is the first topic for Certificate of DevOps",
					Certificate = certificate1
				};
				topics.Add(topic1Cert1);

                var topic2Cert1 = new Topic
				{
					Title = "DevOps Topic 2",
					Description = "This is the second topic for Certificate of DevOps",
					Certificate = certificate1
				};
				topics.Add(topic2Cert1);

				var topic3Cert1 = new Topic
				{
					Title = "DevOps Topic 3",
					Description = "This is the third topic for Certificate of DevOps",
					Certificate = certificate1

				};
				topics.Add(topic3Cert1);

				var topic1Cert2 = new Topic
				{
					Title = "Python Topic 1",
					Description = "This is the first topic for Certificate of Python",
					Certificate = certificate2
				};
				topics.Add(topic1Cert2);

				var topic2Cert2 = new Topic
				{
					Title = "Python Topic 2",
					Description = "This is the second topic for Certificate of Python",
					Certificate = certificate2
				};
				topics.Add(topic2Cert2);

				var topic3Cert2 = new Topic
				{
					Title = "Python Topic 3",
					Description = "This is the third topic for Certificate of Python",
					Certificate = certificate2
				};
				topics.Add(topic3Cert2);

				var topic1Cert3 = new Topic
				{
					Title = "Javascript Topic 1",
					Description = "This is the first topic for Certificate of Javascript",
					Certificate = certificate3
				};
				topics.Add(topic1Cert3);

				var topic2Cert3 = new Topic
				{
					Title = "Javascript Topic 2",
					Description = "This is the second topic for Certificate of Javascript",
					Certificate = certificate3
				};
				topics.Add(topic2Cert3);

				var topic3Cert3 = new Topic
				{
					Title = "Javascript Topic 3",
					Description = "This is the third topic for Certificate of Javascript",
					Certificate = certificate3
				};
				topics.Add(topic3Cert3);

				_unitOfWork.Topic.AddRange(topics);
				_unitOfWork.Save();

			}
			if (_unitOfWork.QuestionAnswers.GetAll().Count() == 0)
			{ 
				var topics = _unitOfWork.Topic.GetAll();
				List<QuestionAnswers> questionAnswers= new List<QuestionAnswers>();
				foreach (var topic in topics)
				{
					for (int i = 1; i <= 5; i++)
					{
                        QuestionAnswers q = new QuestionAnswers
                        {
                            // Possible answersPython
                            Answer1 = "This is a generic answer to a question called A1",
							Answer2 = "This is a generic answer to a question called A2",
							Answer3 = "This is a generic answer to a question called A3",
							Answer4 = "This is a generic answer to a question called A4",
							CorrectIndex = new Random().Next(1, 4),

							// Question
							TextOfQuestion = $"This is an example of a Question with Number: {i}",
							Topics = topic
						};
						questionAnswers.Add(q);
					}
				}
				_unitOfWork.QuestionAnswers.AddRange(questionAnswers);
				_unitOfWork.Save();
			}
        }


        public void ExamSeedDb()
        {
            List<Exam> exams = new List<Exam>();
            var certificates = _unitOfWork.Certificate.GetAll();
            foreach (var cert in certificates)
            {
                var exam = new Exam { Certificate = cert };
                var examTopics = new List<ExamTopics>();
                List<Topic> topics = (List<Topic>)_unitOfWork.Topic.GetTopicByCertificate(cert.Id);
				for (int i = 0; i < topics.Count(); i++)
				{
					var examTopic = new ExamTopics { Topic = topics[i], Exam = exam, SubjectWeight = 5 };
					examTopics.Add(examTopic);
					List<ExamQuestion> examQuestions = new List<ExamQuestion>();
					ICollection<QuestionAnswers> topicQuestion = _unitOfWork.QuestionAnswers.WhereTopicId(topics[i].Id);
                    foreach(var question in topicQuestion)
                    { 
					    ExamQuestion examQuestion = new ExamQuestion { ExamTopics = examTopic, QuestionAnswer = question, status = true};
					    examQuestions.Add(examQuestion);
					}
					_unitOfWork.ExamQuestion.AddRange(examQuestions);
				}
				_unitOfWork.ExamTopic.AddRange(examTopics);
				exam.ExamDuration = 1200;
                exam.ExamDate = DateTime.Now.Date;
				exams.Add(exam);
			}
            _unitOfWork.Exam.AddRange(exams);
            _unitOfWork.Save();
        }
        public Exam SeedAnExam( int certId)
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
        public void SeedTopics( int certId)
        {
            var cert = _unitOfWork.Certificate.Get(certId);

            List<Topic> topics = new List<Topic>();
            var topic1Cert1 = new Topic
            {
                Title = $"{cert.Title} Topic 1",
                Description = "This is the first topic for Certificate of DevOps",
                Certificate = cert
            };
            topics.Add(topic1Cert1);

            var topic2Cert1 = new Topic
            {
                Title = $"{cert.Title} Topic 2",
                Description = "This is the second topic for Certificate of DevOps",
                Certificate = cert
            };
            topics.Add(topic2Cert1);

            var topic3Cert1 = new Topic
            {
                Title = $"{cert.Title} Topic 3",
                Description = "This is the third topic for Certificate of DevOps",
                Certificate = cert

            };
            topics.Add(topic3Cert1);

            List<QuestionAnswers> questionAnswers = new List<QuestionAnswers>();
            foreach (var topic in topics)
            {
                for (int i = 1; i <= 5; i++)
                {
                    QuestionAnswers q = new QuestionAnswers
                    {
                        // Possible answersPython
                        Answer1 = "This is a generic answer to a question called A1",
                        Answer2 = "This is a generic answer to a question called A2",
                        Answer3 = "This is a generic answer to a question called A3",
                        Answer4 = "This is a generic answer to a question called A4",
                        CorrectIndex = new Random().Next(1, 4),

                        // Question
                        TextOfQuestion = $"This is an example of a Question with Number: {i}",
                        Topics = topic
                    };
                    questionAnswers.Add(q);
                }
            }
            _unitOfWork.QuestionAnswers.AddRange(questionAnswers);

        }


        //public void CandidateExamSeedDB(IUnitOfWork _unitOfWork)
        //{
        //	var cand = _unitOfWork.Candidate.Get(1);
        //	var exam = _unitOfWork.Exam.Get(1);
        //	CandidateExam candidateExam = new CandidateExam()
        //	{
        //		Candidate = cand,
        //		Exam = exam
        //	};
        //	_unitOfWork.CandidateExam.Add(candidateExam);
        //	_unitOfWork.Save();
        //	var examQuestions = _unitOfWork.ExamQuestion.WhereExamId(1);
        //	List<CandidateExamQuestions> listOfCandidateExamQuestions = new List<CandidateExamQuestions>();
        //	foreach (var examQuestion in examQuestions)
        //	{
        //		listOfCandidateExamQuestions.Add(new CandidateExamQuestions()
        //		{
        //			CandidateExam = candidateExam,
        //			ExamQuestion = examQuestion
        //		});
        //	}
        //	_unitOfWork.CandidateExamQuestion.AddRange(listOfCandidateExamQuestions);
        //	_unitOfWork.Save();
        //}
    }
}
