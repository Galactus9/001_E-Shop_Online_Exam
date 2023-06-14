using EShopOnlineExam.Models;
using EShopOnlineExam.Repository.IRepository;
using EShopOnlineExam.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding.Binders;
using Microsoft.AspNetCore.Mvc.Rendering;
using Org.BouncyCastle.Asn1.Crmf;
using Org.BouncyCastle.Asn1.Ocsp;
using Org.BouncyCastle.Crypto.Tls;
using System.Linq;

namespace EShopOnlineExam.Controllers
{
   
    [Authorize(Roles = "Admin, QualityControl")]
    public class CertificateController : Controller
    {
        [BindProperty]
        public CertTopicViewModel CertTopicViewModel { get; set; }  
        
        private readonly IUnitOfWork _unitOfWork;
        public CertificateController(IUnitOfWork unitOfWork)
        {

            _unitOfWork = unitOfWork;
        }
        public async Task<IActionResult> Index()
        {
            var x = await _unitOfWork.Certificate.GetAllAs();
             return View(x);
        }


        // Create step 1
        [Authorize(Roles = "Admin")]
        public IActionResult Create()
        {
            CertTopicViewModel certTopicViewModel = new CertTopicViewModel();
            return View(certTopicViewModel);
        }

        [Authorize(Roles = "Admin")]
        public IActionResult CreateBack(Models.Certificate certificate)

        {
            return View(certificate);
        }


        //Create step 2 works perfect
        [Authorize(Roles = "Admin")]
        [HttpPost]
        public IActionResult Create(CertTopicViewModel certTopic, string submitButton)
        {
            _unitOfWork.Certificate.Add(certTopic.Certificate);
            _unitOfWork.Save();
            switch (submitButton)
            {
                case "Add Topics":
                    return RedirectToAction("CreateCertificateTopic", "Certificate", new { certId = certTopic.Certificate.Id });
                case "Save and out":
                    return RedirectToAction("Index");
                default:
                    // If they've submitted the form without a submitButton, 
                    // just return the view again.
                    return (View());
            }
        }

        //Create step 3
        [Authorize(Roles = "Admin")]
        public IActionResult CreateCertificateTopic(int certId)
        {
            CertTopicViewModel certTopicViewModel = new CertTopicViewModel
            {
                Certificate = _unitOfWork.Certificate.Get(certId)
            };
            return View(certTopicViewModel);
        }


        //Create step 4 works perfect
        [Authorize(Roles = "Admin")]
        [HttpPost]
        public IActionResult CreateCertificateTopic(CertTopicViewModel certTopic, string submitButton)
        {
            certTopic.Topic.Certificate = _unitOfWork.Certificate.Get(certTopic.Certificate.Id);
            _unitOfWork.Topic.Add(certTopic.Topic);
            _unitOfWork.Save();
            switch (submitButton)
            {
                case "Add Questions":
                    return RedirectToAction("CreateCertificateTopicQuestions", "Certificate", new { certId = certTopic.Certificate.Id });
                case "Save and out":
                    return RedirectToAction("Index");
                case "Add another Topic":
                    return RedirectToAction("CreateCertificateTopic", "Certificate" , new { certId = certTopic.Certificate.Id });
                default:
                    // If they've submitted the form without a submitButton, 
                    // just return the view again.
                    return (View());
            }
        }


        //Create step 5
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> CreateCertificateTopicQuestions(int certId)
        {
            //Topic topic = await _unitOfWork.Topic.GetT(Id);
            CertTopicViewModel certTopicViewModel = new CertTopicViewModel
            {
                //Topic = topic,
                Certificate = _unitOfWork.Certificate.Get(certId)
            };
            ViewBag.Topics = new SelectList(_unitOfWork.Topic.GetTopicByCertificate(certId), "Id", "Title");
            return View(certTopicViewModel);
        }




        //Create step 6
        [Authorize(Roles = "Admin")]
        [HttpPost]
        public IActionResult CreateCertificateTopicQuestions(CertTopicViewModel certTopic, string submitButton)
        {
            certTopic.QuestionAnswers.Topics = _unitOfWork.Topic.Get(certTopic.Topic.Id);
            _unitOfWork.QuestionAnswers.Add(certTopic.QuestionAnswers);
            _unitOfWork.Save();

             switch (submitButton)
            {
                case "Add another one":
                    return RedirectToAction("CreateCertificateTopicQuestions", "Certificate", new { certId = certTopic.Certificate.Id });
                case "Save and out":
                    return RedirectToAction("Index");
                default:
                    // If they've submitted the form without a submitButton, 
                    // just return the view again.
                    return (View());
            }
        }




        public async Task<IActionResult> Details (int id)
        {
            var certificate = await _unitOfWork.Certificate.GetAs(id);
            var topics = (List<Topic>)_unitOfWork.Topic.GetTopicByCertificate(certificate.Id);
            List<QuestionAnswers> questions = new List<QuestionAnswers>();
            foreach(var topic in topics)
            {
                questions.AddRange(_unitOfWork.QuestionAnswers.WhereTopicId(topic.Id));
            }


            CertificateVM certVM = new CertificateVM();
            certVM.Title = certificate.Title;
            certVM.Description = certificate.Description;
            certVM.Price = certificate.Price;
            certVM.PossibleMarks = certificate.PossibleMarks;
            certVM.Id = certificate.Id;
            certVM.State = certificate.State;
            certVM.ScoreNeededToPass = certificate.ScoreNeededToPass;
            certVM.Topics = topics;
            certVM.Questions = questions;
            return View(certVM);
        }

        //Edit Step 1
        [Authorize(Roles = "Admin")]
        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            return View(await _unitOfWork.Certificate.GetAs(id));
        }


        [Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<IActionResult> EditPost(string submitButton, Models.Certificate certificate)
        {
            _unitOfWork.Certificate.Update(certificate);
            _unitOfWork.Save();
            switch (submitButton)
            {
                case "Edit Topics":
                    return RedirectToAction("EditTopic", new { certId = certificate.Id });
                case "Save and out":
                    return RedirectToAction("Index");
                default:
                    return (View());
            }
        }


        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> EditTopic(int certId)
        {
            var topics = (List<Topic>)_unitOfWork.Topic.GetTopicByCertificate(certId);
            return View(topics);
        }
        [HttpPost]
        //Edit Step 4
        public async Task<IActionResult> EditTopic(List<Topic> topics,string submitButton)
        {
            var cert = _unitOfWork.Certificate.Get(topics.First().Certificate.Id);
            
            foreach(Topic topic in topics)
            {
                var dbTopic = _unitOfWork.Topic.Get(topic.Id);
                dbTopic.Certificate = cert;
                _unitOfWork.Topic.Update(dbTopic);
            }
            _unitOfWork.Save();
            switch (submitButton)
            {
                case "Edit Questions":
                    return RedirectToAction("EditQuestions", new { certId = topics.First().Certificate.Id });
                case "Save and out":
                    return RedirectToAction("Index");
                default:
                    return (View());
            }
        }

        //Edit Step 5
        public async Task<IActionResult> EditQuestions(int certId)
        {
            var topics = (List<Topic>)_unitOfWork.Topic.GetTopicByCertificate(certId);
            List<QuestionAnswers> questions = new List<QuestionAnswers>();
            foreach (var topic in topics)
            {
                questions.AddRange(_unitOfWork.QuestionAnswers.WhereTopicId(topic.Id));
            }
            return View(questions);
        }
        [HttpPost]
        public IActionResult EditQuestions(int cId, List<QuestionAnswers> questionsToUpaDate)
        {

            foreach (var question in questionsToUpaDate)
            {
                var tmpQuestion =  _unitOfWork.QuestionAnswers.Get(question.Id);
                tmpQuestion.Answer1 = question.Answer1;
                tmpQuestion.Answer2 = question.Answer2;
                tmpQuestion.Answer3 = question.Answer3;
                tmpQuestion.Answer4 = question.Answer4;
                tmpQuestion.Topics = question.Topics;
                tmpQuestion.TextOfQuestion = question.TextOfQuestion;
                tmpQuestion.CorrectIndex = question.CorrectIndex;
                _unitOfWork.QuestionAnswers.Update(tmpQuestion);
        
            }
            _unitOfWork.Save();
            return RedirectToAction("EditQuestions", new { certId = cId });
            //var topics = (List<Topic>)_unitOfWork.Topic.GetTopicByCertificate(certId);
            //List<QuestionAnswers> questions = new List<QuestionAnswers>();
            //foreach (var topic in topics)
            //{
            //    questions.AddRange(_unitOfWork.QuestionAnswers.WhereTopicId(topic.Id));
            //}


        }
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                var certifacete = await _unitOfWork.Certificate.GetAs(id);
                return View(certifacete);
            } catch (Exception)
            {
                return NotFound(); 
            }

        }

        [Authorize(Roles = "Admin")]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            try
            {
                var certifacete = await _unitOfWork.Certificate.GetAs(id);
                _unitOfWork.Certificate.Delete(certifacete);
                _unitOfWork.Save();
                return RedirectToAction(nameof(Index));
            }catch(Exception)
            {
                return NotFound();
            }
        }



    }

}
