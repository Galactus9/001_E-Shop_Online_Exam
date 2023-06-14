using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using EShopOnlineExam.Data;
using EShopOnlineExam.Models;
using EShopOnlineExam.Repository.IRepository;

using ToDoList = EShopOnlineExam.Models.ToDoList;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authorization;

namespace EShopOnlineExam.Controllers
{
    [Authorize(Roles = "Admin, QualityControl")]
    public class ToDoListController : Controller
    {
        private readonly UserManager<Candidate> _userManager;
        private readonly IUnitOfWork _unitOfWork;

        public ToDoListController(
            IUnitOfWork unitOfWork,
            UserManager<Candidate> userManager)
        {
            _unitOfWork = unitOfWork;
            _userManager = userManager;
        }

        public async Task<IActionResult> Index()
        {
            var cands = await _unitOfWork.ToDoList.GetAllAs();
            return View(cands);
        }

        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Create()
        {
            List<Candidate> markers = new List<Candidate>();
            var users = await _unitOfWork.Candidate.GetAllAs();
            foreach(var user in users)
            { 
                if (await _userManager.IsInRoleAsync(user, "Marker"))

                {
                    markers.Add(user); 
                }
            }
            ViewBag.Markers = markers;
            List<int> exams = new List<int>();    
            var CExams = await _unitOfWork.CandidateExamination.GetAllCandidateExaminationsAs();
            foreach (var exam in CExams) 
            {
                exams.Add(exam.Id);
            }
            ViewBag.Exams = exams;

			ToDoList toDoList = new ToDoList();
            return View(toDoList);
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ToDoList toDoList)
        {

            var marker = await _unitOfWork.Candidate.GetCandidateByEmail(toDoList.Marker.UserName);
            var exam = _unitOfWork.CandidateExamination.Get(toDoList.Exam.Id);
            ToDoList tempList = new ToDoList
            {
                Marker = marker,
                Exam = exam,
                DueDate= toDoList.DueDate,
                Status= toDoList.Status,
                TaskDescription= toDoList.TaskDescription,
            };


            _unitOfWork.ToDoList.Add(tempList);
            _unitOfWork.Save();
            return RedirectToAction(nameof(Index));
        }

        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Edit(int id)
        { 
            var toDoList = await _unitOfWork.ToDoList.GetAs(id);
            if (toDoList == null)
            {
                return NotFound();
            }
            List<Candidate> markers = new List<Candidate>();
            var users = await _unitOfWork.Candidate.GetAllAs();
            foreach (var user in users)
            {
                if (await _userManager.IsInRoleAsync(user, "Marker"))

                {
                    markers.Add(user);
                }
            }
            ViewBag.Markers = markers;
            List<int> exams = new List<int>();
            var CExams = await _unitOfWork.CandidateExamination.GetAllCandidateExaminationsAs();
            foreach (var exam in CExams)
            {
                exams.Add(exam.Id);
            }
            ViewBag.Exams = exams;
            return View(toDoList);
        }

        [Authorize(Roles = "Admin")]
        [ActionName("Edit")]
        [HttpPost]
        public async Task<IActionResult> EditSave( ToDoList toDoList)
        {


            var marker = await _unitOfWork.Candidate.GetCandidateByEmail(toDoList.Marker.UserName);
            var exam = _unitOfWork.CandidateExamination.Get(toDoList.Exam.Id);
            toDoList.Marker = marker;
            toDoList.Exam = exam;

            _unitOfWork.ToDoList.Update(toDoList);
            _unitOfWork.Save();
            return RedirectToAction("Index");
          
        }

        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> ListOfToDoList()
        {
            return View(await _unitOfWork.ToDoList.GetAllAs());
        }

        [Authorize(Roles = "Admin")]
        public IActionResult Delete(int id)
        {
            var toDoList =  _unitOfWork.ToDoList.Get(id);
            return View(toDoList);
        }

        [Authorize(Roles = "Admin")]
        [ActionName("Delete")]
        [HttpPost]
        public IActionResult DeleteOnPost(int id)
        {
            var toDoList = _unitOfWork.ToDoList.Get(id);
            _unitOfWork.ToDoList.Delete(toDoList);
            _unitOfWork.Save();
            return RedirectToAction("Index");
        }

        [Authorize(Roles = "Admin")]
        public IActionResult RequestHandler(string HandleRequest, int id)
        {
            var tempTask = _unitOfWork.ToDoList.Get(id);
            if (HandleRequest == "Accept")
            {
                tempTask.Status = ToDoListStatus.Closed;
            }
            else if (HandleRequest == "Deny")
            {
                tempTask.Status = ToDoListStatus.Open;
            }
            _unitOfWork.ToDoList.Update(tempTask);
            _unitOfWork.Save();
            return RedirectToAction("Index");
        }
    }    
}
