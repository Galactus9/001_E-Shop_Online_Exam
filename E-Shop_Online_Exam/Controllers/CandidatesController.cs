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
using Microsoft.AspNetCore.Authorization;

namespace EShopOnlineExam.Controllers
{
    [Authorize(Roles = "Admin")]
    public class CandidatesController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;

        public CandidatesController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        // GET: Candidates
        public async Task<IActionResult> Index()
        {
              return View(await _unitOfWork.Candidate.GetAllAs());
        }

        // GET: Candidates/Details/5
        public async Task<IActionResult> Details(int id)
        {
            if (id == null || _unitOfWork.Candidate == null)
            {
                return NotFound();
            }

            var candidate = await _unitOfWork.Candidate.GetAs(id);
                
            if (candidate == null)
            {
                return NotFound();
            }

            return View(candidate);
        }

        // GET: Candidates/Create
        [Authorize(Roles = "Admin,QualityControl")]

        public IActionResult Create()
        {
            return View();
        }

        // POST: Candidates/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("CandidateNumber,FirstName,LastName,MiddleName,Gender,NativeLanguage,BirthDate,PhotoIdType,PhotoIdNumber,PhotoIdIssueDate,Email,Address,AddressLine2,City,Region,PostalCode,Country,LandlineNumber,MobileNumber")] Candidate candidate)
        {
            if (ModelState.IsValid)
            {
                await _unitOfWork.Candidate.AddAs(candidate);
                _unitOfWork.Save();
                return RedirectToAction(nameof(Index));
            }
            return View(candidate);
        }

        // GET: Candidates/Edit/5
        public async Task<IActionResult> Edit(int id)
        {
            if (id == null || _unitOfWork.Candidate.GetAll() == null)
            {
                return NotFound();
            }

            var candidate = await _unitOfWork.Candidate.GetAs(id);
            if (candidate == null)
            {
                return NotFound();
            }
            return View(candidate);
        }

        // POST: Candidates/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(int id, [Bind("CandidateNumber,FirstName,LastName,MiddleName,Gender,NativeLanguage,BirthDate,PhotoIdType,PhotoIdNumber,PhotoIdIssueDate,Email,Address,AddressLine2,City,Region,PostalCode,Country,LandlineNumber,MobileNumber")] Candidate candidate)
        {
            if (id != candidate.CandidateNumber)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _unitOfWork.Candidate.Update(candidate);
                    _unitOfWork.Save();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CandidateExists(candidate.CandidateNumber))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(candidate);
        }

        // GET: Candidates/Delete/5
        public async Task<IActionResult> Delete(int id)
        {
            if (id == null || await _unitOfWork.Candidate.GetAllAs() == null)
            {
                return NotFound();
            }

            var candidate = await _unitOfWork.Candidate.GetAs(id);
            if (candidate == null)
            {
                return NotFound();
            }

            return View(candidate);
        }

        // POST: Candidates/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_unitOfWork.Candidate.GetAll() == null)
            {
                return Problem("Entity set 'MyDbContext.Candidates'  is null.");
            }
            var candidate = await _unitOfWork.Candidate.GetAs(id);
            if (candidate != null)
            {
                _unitOfWork.Candidate.Delete(candidate);
            }
            
            _unitOfWork.Save();
            return RedirectToAction(nameof(Index));
        }

        private bool CandidateExists(int id)
        {
            var candidate = _unitOfWork.Candidate.Get(id);
            try 
            {
                _unitOfWork.Candidate.Get(id);
                    return true;
            }
            catch(Exception)
            {
                return false;
            }
                
        }
    }
}
