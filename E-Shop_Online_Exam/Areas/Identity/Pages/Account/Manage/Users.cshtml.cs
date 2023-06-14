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
using Microsoft.EntityFrameworkCore;
using NuGet.Versioning;

namespace EShopOnlineExam.Areas.Identity.Pages.Account.Manage
{
    public class UsersModel : PageModel
    {
        private readonly UserManager<Candidate> _userManager;
        private readonly SignInManager<Candidate> _signInManager;
        private readonly IUnitOfWork _unitOfWork;
        private readonly RoleManager<IdentityRole> _roleManager;

        public UsersModel(
            UserManager<Candidate> userManager,
            SignInManager<Candidate> signInManager,
            IUnitOfWork unitOfWork, 
            RoleManager<IdentityRole> roleManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _unitOfWork = unitOfWork;
            _roleManager = roleManager;
        }

        public List<Candidate> Users { get; set; }


        /// <summary>
        ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
        ///     directly from your code. This API may change or be removed in future releases.
        /// </summary>
        [TempData]
        public string StatusMessage { get; set; }
        public List<Candidate> TempUsers { get; set; }
        public List<Candidate> UserRolesAdmin { get; set; }
        public List<Candidate> UserRolesMarker { get; set; }
        public List<Candidate> UserRolesQuality { get; set; }


        private async Task LoadAsync(Candidate user)
        {
            var Identity = (ClaimsIdentity)User.Identity;
            //Certificates =  await _unitOfWork.CandidateCertificate.GetAllCertsForCand(candidateId.Value);
            //var users = await _userManager.Users.ToListAsync();
            var users = (List<Candidate>)_unitOfWork.Candidate.GetAll();
            TempUsers = users;
            
            List<Candidate> adminUsers = new List<Candidate>();
            List<Candidate> markerUsers = new List<Candidate>();
            List<Candidate> qualityUsers = new List<Candidate>();
            foreach (Candidate tempUser in users)
            {
                if (await _userManager.IsInRoleAsync(tempUser, "Admin"))
                {
                    adminUsers.Add(tempUser);
                }
                if (await _userManager.IsInRoleAsync(tempUser, "Marker"))
                {
                    markerUsers.Add(tempUser);
                }
                if (await _userManager.IsInRoleAsync(tempUser, "QualityControl"))
                {
                    qualityUsers.Add(tempUser);
                }

            }
            UserRolesAdmin = adminUsers;
            UserRolesMarker = markerUsers;
            UserRolesQuality= qualityUsers;
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

        public async Task<IActionResult> OnPostAsync(string id, string checkBox)
        {
            var tempUser = await _userManager.Users.SingleAsync(x => x.Id == id);
            //List<string> roles = new List<string>() { "Admin", "Marker", "QualityControl" };
            //_userManager.RemoveFromRolesAsync(tempUser, roles);
            if (checkBox == "checkAdminEnable") 
            {
                await _userManager.AddToRoleAsync(tempUser, "Admin"); 
            }
            else if(checkBox == "checkAdminDisable")
            {
                await _userManager.RemoveFromRoleAsync(tempUser, "Admin");
            }
            else if (checkBox == "checkMarkerEnable")
            {
                await _userManager.AddToRoleAsync(tempUser, "Marker");
            }
            else if (checkBox == "checkMarkerDisable")
            {
                await _userManager.RemoveFromRoleAsync(tempUser, "Marker");
            }
            else if (checkBox == "checkQualityEnable")
            {
                await _userManager.AddToRoleAsync(tempUser, "QualityControl");
            }
            else if (checkBox == "checkQualityDisable")
            {
                await _userManager.RemoveFromRoleAsync(tempUser, "QualityControl");
            }



            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }

            if (!ModelState.IsValid)
            {
                await LoadAsync(user);
                return Page();
            }

            await _signInManager.RefreshSignInAsync(user);
            StatusMessage = "Your profile has been updated";
            return RedirectToPage();
        }
    }
}
