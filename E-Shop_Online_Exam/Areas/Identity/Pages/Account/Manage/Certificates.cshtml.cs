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

namespace EShopOnlineExam.Areas.Identity.Pages.Account.Manage
{
    public class CertificatesModel : PageModel
    {
        private readonly UserManager<Candidate> _userManager;
        private readonly SignInManager<Candidate> _signInManager;
        private readonly IUnitOfWork _unitOfWork;
        public CertificatesModel(
            UserManager<Candidate> userManager,
            SignInManager<Candidate> signInManager,
            IUnitOfWork unitOfWork)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _unitOfWork = unitOfWork;
        }
        [BindProperty]
        public List<CandidateExamination> CandidateExaminations { get; set; }
        //public IEnumerable


        /// <summary>
        ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
        ///     directly from your code. This API may change or be removed in future releases.
        /// </summary>
        [TempData]
        public string StatusMessage { get; set; }


        private async Task LoadAsync(Candidate user)
        {
            var Identity = (ClaimsIdentity)User.Identity;
            var candidateId = Identity.FindFirst(ClaimTypes.NameIdentifier);
            CandidateExaminations =  await _unitOfWork.CandidateExamination.GetAllExaminationsForCand(candidateId.Value);
        }

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

        public async Task<IActionResult> OnPostAsync(int id, List<int> cexam, List<int> exam, List<string> date)
        {

            var index = cexam.IndexOf(id);
            CandidateExamination candidateExamination = await _unitOfWork.CandidateExamination.GetWithExam(cexam[index]);
            candidateExamination.ExamStartingTime = DateTime.Parse(date[index]);
            _unitOfWork.CandidateExamination.Update(candidateExamination);
            _unitOfWork.Save();
            return RedirectToPage();
        }
    }
}
