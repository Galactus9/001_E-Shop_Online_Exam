using EShopOnlineExam.Data;
using EShopOnlineExam.Models;
using EShopOnlineExam.Repository;
using EShopOnlineExam.Repository.IRepository;
using EShopOnlineExam.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using NuGet.Packaging;
using System;
using System.Drawing.Printing;

namespace EShopOnlineExam.Controllers
{
	[Authorize (Roles = "Admin, QualityControl")]
	public class QuestionController : Controller
	{
		private readonly IUnitOfWork _unitOfWork;
        private readonly IWebHostEnvironment _env;


		public QuestionController(IWebHostEnvironment env, MyDbContext context, IUnitOfWork unitOfWork)
		{
			_env = env;
			_unitOfWork = unitOfWork;
		}
		
        public IActionResult Index()
        {		
            return View();
        }

        [Authorize(Roles = "Admin")]
		public IActionResult Seed()
		{
            Seed seed = new Seed(_unitOfWork);
            seed.SeedDb();
			seed.ExamSeedDb();

            return RedirectToAction("ListOfQuestions");
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
		public ActionResult UploadImage(List<FormFile> files)
		{
			var filePath = "";
			foreach(IFormFile photo in Request.Form.Files)
			{
				string serverMapPath = Path.Combine(_env.WebRootPath , "Image", photo.FileName);
				using (var stream = new FileStream( serverMapPath, FileMode.Create))
				{
					photo.CopyTo(stream);
				}
				filePath = "/Image/" + photo.FileName;
			}
			return Json(new { url = filePath});
		}


        //[Authorize(Roles = "Admin")]
        public  async Task<IActionResult> ListOfQuestions()
		{
			return View(await _unitOfWork.QuestionAnswers.GetAllQuestionAnswersAs());
		}

        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Edit(int id)
		{
			return View(await _unitOfWork.QuestionAnswers.GetAs(id));
		}

        [Authorize(Roles = "Admin")]
        [HttpPost]
		public ActionResult EditSave(QuestionAnswers questionAnswer)
		{
			_unitOfWork.QuestionAnswers.Update(questionAnswer);
			_unitOfWork.Save();
			return RedirectToAction("ListOfQuestions");
		}

        [Authorize(Roles = "Admin")]
        public IActionResult Create()
		{
            ViewBag.Cetificates = new SelectList(_unitOfWork.Certificate.GetAll(), "Id", "Title");
            return View();
		}

        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteConfirm(int id)
		{
			return View(await _unitOfWork.QuestionAnswers.GetLoadedAs(id));
		}

        [Authorize(Roles = "Admin")]
        public IActionResult Delete(int id)
		{
			try
			{
                _unitOfWork.QuestionAnswers.Delete(_unitOfWork.QuestionAnswers.Get(id));
                _unitOfWork.Save();
                return RedirectToAction("ListOfQuestions");
            }
			catch(Exception)
			{
				return View("DeleteException");
			}
			
        }

        [Authorize(Roles = "Admin")]
        public IActionResult Save(QuestionAnswers questionAnswer)
		{

			var topic = _unitOfWork.Topic.Get(questionAnswer.Topics.Id);
            questionAnswer.Topics = topic;
			_unitOfWork.QuestionAnswers.Add(questionAnswer);
			_unitOfWork.Save();
            return RedirectToAction ("ListOfQuestions");
		}


		public async Task<IActionResult> Details(int id)
		{
			return View(await _unitOfWork.QuestionAnswers.GetLoadedAs(id));

        }

        [HttpPost]
        public JsonResult GetTopics(int id)
        {
            var topicList = (List<Topic>)_unitOfWork.Topic.GetTopicByCertificate(id);
            return Json(new SelectList(topicList, "Id", "Title"));
        }
    }
}
