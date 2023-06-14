// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
#nullable disable

using System;
using System.ComponentModel.DataAnnotations;
using System.Security.Claims;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using EShopOnlineExam.Models;
using EShopOnlineExam.Repository.IRepository;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.FlowAnalysis.DataFlow.CopyAnalysis;

namespace EShopOnlineExam.Areas.Identity.Pages.Account.Manage
{
    public class QcNoteModel : PageModel
    {
        private readonly UserManager<Candidate> _userManager;
        private readonly SignInManager<Candidate> _signInManager;
        private readonly IUnitOfWork _unitOfWork;
        public QcNoteModel(
            UserManager<Candidate> userManager,
            SignInManager<Candidate> signInManager,
            IUnitOfWork unitOfWork)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _unitOfWork = unitOfWork;
        }
        [TempData]
        public string StatusMessage { get; set; }

        [BindProperty]
        public IEnumerable<QcNote> QcNotes { get; set; }
        [BindProperty]
        public QcNote QcNote {get; set;}

        private async Task LoadAsync(Candidate user)
        {
            var Identity = (ClaimsIdentity)User.Identity;
            var candidateId = Identity.FindFirst(ClaimTypes.NameIdentifier);
            QcNotes =  await _unitOfWork.QcNote.GetAllQcNotesForUser(candidateId.Value);
            //Certificates = await _unitOfWork.CandidateCertificate.GetAllCertsForCand("526f2cf4-ae97-4b23-8a31-d6e4fdf8c987");
        }
        //Copy   paste
        public async Task<IActionResult> OnGetAsync()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }

            await LoadAsync(user);
            return Page();
        }

        public async Task<IActionResult> OnPostAsync(int id, string button)
        {
            if (button == "closeReport")
            {
                QcNote note = _unitOfWork.QcNote.Get(id);
                _unitOfWork.QcNote.Delete(note);
            }
            else { 
                var Identity = (ClaimsIdentity)User.Identity;
                var name = Identity.Name; 
                _unitOfWork.QcNote.GetQcNoteById(id).Supervisor = name;
                _unitOfWork.QcNote.GetQcNoteById(id).State = "Handled";
            }
            _unitOfWork.Save();

            return RedirectToPage();
        }
    }
}
