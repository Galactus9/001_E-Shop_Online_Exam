using EShopOnlineExam.Models;
using EShopOnlineExam.Repository.IRepository;
using EShopOnlineExam.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.EntityFrameworkCore.Metadata.Conventions;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;

namespace EShopOnlineExam.Controllers
{
    [Authorize(Roles = "Admin, QualityControl")]
    public class ExamCrudController : Controller
    {

        private readonly IUnitOfWork _unitOfWork;
        public ExamCrudController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }


        public async Task<IActionResult> Index()
        {
            return View(await _unitOfWork.Exam.GetAllWhithCertAs());
        }

        public async Task<IActionResult> Details(int id)
        {
            var exam = await _unitOfWork.Exam.GetLoadedAs(id);
            exam.ExamQuestions = (ICollection<ExamQuestion>)await _unitOfWork.ExamQuestion.WhereExamId(id);

            return View(exam);
        }


        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(int id)
        {
            return View(await _unitOfWork.Exam.GetLoadedAs(id));
        }

        // POST: Candidates/Delete/5
        [Authorize(Roles = "Admin")]
        [HttpPost, ActionName("Delete")]

        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_unitOfWork.Exam.GetAll() == null)
            {
                return Problem("Entity set 'MyDbContext.Candidates'  is null.");
            }
            var exam = await _unitOfWork.Exam.GetAs(id);
            if (exam != null)
            {
                _unitOfWork.Exam.Delete(exam);
            }

            _unitOfWork.Save();
            return RedirectToAction(nameof(Index));
        }


        //Create Step 1
        [Authorize(Roles = "Admin")]
        [HttpGet]
        public IActionResult Create(int id)
        {
            ViewBag.Cetificates = new SelectList(_unitOfWork.Certificate.GetAll(), "Id", "Title");
            return View();
        }

        //Create Step 2
        [Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<IActionResult> Create(Exam exam, string submitButton)
        {
            exam.ExamDate = DateTime.Now;
            exam.Certificate = await _unitOfWork.Certificate.GetAs(exam.Certificate.Id);
            await _unitOfWork.Exam.AddAs(exam);
            _unitOfWork.Save();
            switch (submitButton)
            {
                case "Add exam topics":
                    return RedirectToAction("CreateExamTopic", "ExamCrud", new { examId = exam.Id });
                case "Save and out":
                    return RedirectToAction("Index");
                default:
                    return (View());
            }
        }

        //Create Step 3
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> CreateExamTopic(int examId)
        {
            var exam = await _unitOfWork.Exam.GetLoadedAs(examId);
            var listTopics = new List<Topic>(_unitOfWork.Topic.GetTopicByCertificate(exam.Certificate.Id));

            List<TopicSelectionVM> topics = new List<TopicSelectionVM>();
            foreach (var topic in listTopics)
            {
                TopicSelectionVM topicSelectionVM = new TopicSelectionVM()
                {
                    Topic = topic,
                    Exam = exam
                };
                topics.Add(topicSelectionVM);
            }
            return View(topics);
        }

        //Create step 4
        [Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<IActionResult> CreateExamTopic(string submitButton, IEnumerable<TopicSelectionVM> topics)
        {
            //var topics

            List<ExamTopics> examTopics = new List<ExamTopics>();
            foreach (var topic in topics)
            {
                ExamTopics examTopic = new ExamTopics
                {
                    Topic = await _unitOfWork.Topic.GetAs(topic.Topic.Id),
                    SubjectWeight = topic.SubjectWeight,
                    Exam = await _unitOfWork.Exam.GetAs(topic.Exam.Id)
                };
                examTopics.Add(examTopic);
                //}
                _unitOfWork.ExamTopic.AddRange(examTopics);
            }
            _unitOfWork.Save();
            switch (submitButton)
            {
                case "Add questions":
                    return RedirectToAction("AddExamQuestions", "ExamCrud", new { examId = topics.First().Exam.Id });
                case "Save and out":
                    return RedirectToAction("Index");
                default:
                    return (View());
            }
        }
        //Create step 5
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> AddExamQuestions(int examId)
        {
            List<AddExamQuestionsVM> questionsVm = new List<AddExamQuestionsVM>();
            var examQUestions = new List<ExamQuestion>();
            var examTopics = (List<ExamTopics>)await _unitOfWork.ExamTopic.GetEnableExamTopic(examId);
            for (int i = 0; i < examTopics.Count(); i++)
            {
                var tempQuestions = (List<QuestionAnswers>)_unitOfWork.QuestionAnswers.WhereTopicId(examTopics[i].Topic.Id);

                for (int y = 0; y < tempQuestions.Count(); y++)
                {
                    examQUestions.Add(new ExamQuestion() { ExamTopics = examTopics[i], QuestionAnswer = tempQuestions[y], status = false });
                    var questionVm = new AddExamQuestionsVM()
                    {
                        Question = tempQuestions[y],
                        Status = "false",
                        ExamTopic = examTopics[i]
                    };
                    questionsVm.Add(questionVm);
                }
            }
            return View(questionsVm);
        }


        //Create step 6 
        [HttpPost]
        [Authorize(Roles = "Admin")]
        public void ExamQuestionManager(AddExamQuestionsVM model, string stringData, int intData)
        {

            if (stringData == "Clear")
            {
                List<ExamQuestion> questionsToRemove = new List<ExamQuestion>();
                var topics = _unitOfWork.ExamTopic.WhereExamIdSync(intData);
                foreach (var topic in topics)
                {
                    var tempQuestions = _unitOfWork.ExamQuestion.WhereExamTopicIdSync(topic.Id);
                    foreach (var tempQuestion in tempQuestions)
                    {
                        tempQuestion.status = false;
                    }

                    _unitOfWork.ExamQuestion.UpdateRange(questionsToRemove);
                    _unitOfWork.Save();
                }

            }
            else
            {
                var examTopic = _unitOfWork.ExamTopic.Get(model.ExamTopicId);
                var examQuestion = _unitOfWork.ExamQuestion.WhereQuestionId(model.QuestionId);
                if (model.Status == "true")
                {
                    examQuestion.status = true;
                    _unitOfWork.ExamQuestion.Update(examQuestion);
                }
                else
                {
                    examQuestion.status = false;
                    _unitOfWork.ExamQuestion.Update(examQuestion);
                }

                _unitOfWork.Save();
            }

        }

        //Edit step 1
        [Authorize(Roles = "Admin")]
        public IActionResult Edit(int id)
        {
            ViewBag.Cetificates = new SelectList(_unitOfWork.Certificate.GetAll(), "Id", "Title");
            var exam = _unitOfWork.Exam.Get(id);
            return View(exam);
        }

        //Edit step 2
        [Authorize(Roles = "Admin")]
        [HttpPost]
        public IActionResult Edit(Exam exam, string submitButton)
        {
            var certificate = _unitOfWork.Certificate.Get(exam.Certificate.Id);
            exam.ExamDate = DateTime.Now;
            exam.Certificate = certificate;
            _unitOfWork.Exam.Update(exam);
            _unitOfWork.Save();
            switch (submitButton)
            {
                case "Edit Topics":
                    return RedirectToAction("EditExamTopic", "ExamCrud", new { examId = exam.Id });
                case "Save and out":
                    return RedirectToAction("Index");
                default:
                    return (View());
            }
        }

        //Edit Step 3
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> EditExamTopic(int examId)
        {
            var exam = await _unitOfWork.Exam.GetExamLoaded(examId);

            var examTopics = (List<ExamTopics>)await _unitOfWork.ExamTopic.WhereExamId(examId);
            return View(examTopics);
        }

        //Edit step 4
        [Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<IActionResult> EditExamTopicPost(List<ExamTopics> examTopics, string submitButton)
        {
            var exam = await _unitOfWork.Exam.GetLoadedAs(examTopics.First().Exam.Id);
            for (int i = 0; i < examTopics.Count(); i++)
            {
                exam.Topic[i].SubjectWeight = examTopics[i].SubjectWeight;
            }
            _unitOfWork.Exam.Update(exam);
            _unitOfWork.Save();

            List<AddExamQuestionsVM> questionsVM = new List<AddExamQuestionsVM>();
            List<ExamQuestion> examTopicQuestions = new List<ExamQuestion>();
            List<QuestionAnswers> topicQuestions = new List<QuestionAnswers>(); ;


            foreach (var examTopic in examTopics)
            {
                if (examTopic.SubjectWeight > 0)
                {
                    examTopicQuestions.AddRange((List<ExamQuestion>)await _unitOfWork.ExamQuestion.WhereExamTopicId(examTopic.Id));
                    foreach (var item in examTopicQuestions)
                    {
                        AddExamQuestionsVM tempQuestion = new AddExamQuestionsVM()
                        {
                            ExamTopic = examTopic,
                            ExamTopicId = examTopic.Id,
                            QuestionId = item.QuestionAnswer.Id,
                            TopicId = examTopic.Topic.Id,
                            Question = item.QuestionAnswer,
                        };
                        if (item.status == true)
                        {
                            tempQuestion.Status = "true";
                        }
                        else
                        {
                            tempQuestion.Status = "false";
                        }
                        questionsVM.Add(tempQuestion);
                    }
                    topicQuestions.Clear();
                    examTopicQuestions.Clear();
                }
            }
            return View(questionsVM);
        }

        [Authorize(Roles = "Admin")]
        public JsonResult Test(int id)
        {
            int maxSW = _unitOfWork.QuestionAnswers.WhereTopicId(id).Count();
            return Json(maxSW);
        }
    }
}
