using System.Security.Claims;
using EShopOnlineExam.Models;
using EShopOnlineExam.Repository.IRepository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace EShopOnlineExam.Controllers
{
    [Authorize(Roles = "Admin, Marker, QualityControl")]
    public class MarkerController : Controller
    {
        private readonly UserManager<Candidate> _userManager;
        private readonly IUnitOfWork _unitOfWork;

        public MarkerController(UserManager<Candidate> userManager, IUnitOfWork unitOfWork)
        {
            _userManager = userManager;
            _unitOfWork = unitOfWork;
        }

        public IActionResult Index()
        {
            return View();
        }

        [Authorize(Roles = "Admin, Marker")]
        public async Task<IActionResult> GetToDoList()
        {
            var Identity = (ClaimsIdentity)User.Identity;
            var markerId = Identity.FindFirst(ClaimTypes.NameIdentifier);
            List<ToDoList> tasks = _unitOfWork.ToDoList.GetToDoListByMarkerId(markerId.Value);
            return View(tasks);
        }

        [Authorize(Roles = "Admin, Marker")]
        public async Task<IActionResult> MarkerTask(int id)
        {
            var task = (List<CandidateResults>)await _unitOfWork.CandidateResults.WhereExaminationIdFull(id);
            return View(task);
        }

        [Authorize(Roles = "Admin, Marker")]
        [HttpPost]
        public IActionResult TaskPost(List<CandidateResults> results)
        {
            foreach(var item in results)
            {
                item.candidateExamination = _unitOfWork.CandidateExamination.Get(item.candidateExamination.Id);
                item.ExamQuestion = _unitOfWork.ExamQuestion.Get(item.ExamQuestion.Id);
            }
            _unitOfWork.CandidateResults.UpdateList(results);
            _unitOfWork.Save();
            return RedirectToAction("GetToDoList");
        }


        [Authorize(Roles = "Admin, Marker")]
        public IActionResult StatusManager(int id)
        {
            var toDoList = _unitOfWork.ToDoList.Get(id);
            toDoList.Status = ToDoListStatus.Close_Request;
            _unitOfWork.ToDoList.Update(toDoList);
            _unitOfWork.Save();
            return RedirectToAction("GetToDoList");
        }
    }
}
