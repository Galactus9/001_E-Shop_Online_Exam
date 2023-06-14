using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.Metrics;
using System.Drawing;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Stripe;

namespace EShopOnlineExam.Areas.Identity.Pages.Account.Manage
{
    public class IndexModel : PageModel
    {
        private readonly UserManager<Candidate> _userManager;
        private readonly SignInManager<Candidate> _signInManager;

        public IndexModel(
            UserManager<Candidate> userManager,
            SignInManager<Candidate> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }

        /// <summary>
        ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
        ///     directly from your code. This API may change or be removed in future releases.
        /// </summary>
        public string Username { get; set; }

        /// <summary>
        ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
        ///     directly from your code. This API may change or be removed in future releases.
        /// </summary>
        [TempData]
        public string StatusMessage { get; set; }

        /// <summary>
        ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
        ///     directly from your code. This API may change or be removed in future releases.
        /// </summary>
        [BindProperty]
        public InputModel Input { get; set; }

        /// <summary>
        ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
        ///     directly from your code. This API may change or be removed in future releases.
        /// </summary>
        public class InputModel
        {
            /// <summary>
            ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
            ///     directly from your code. This API may change or be removed in future releases.
            /// </summary>
            [DisplayName("First Name")]
            public string FirstName { get; set; }
            [DisplayName("Last Name")]
            public string LastName { get; set; }
            [DisplayName("Middle Name")]
            public string? MiddleName { get; set; }
            [DisplayName("Gender")]
            public string Gender { get; set; }
            [DisplayName("Native Language")]
            public string NativeLanguage { get; set; }
            [DisplayName("Birth Date")]
            [DataType(DataType.Date)]
            public DateTime? BirthDate { get; set; }
            [DisplayName("Photo Id Type")]
            public string PhotoIdType { get; set; }
            [DisplayName("Photo Id Number")]
            public string PhotoIdNumber { get; set; }
            [DisplayName("Photo Id Issue Date")]
            [DataType(DataType.Date)]
            public DateTime? PhotoIdIssueDate { get; set; }
            [DisplayName("Address")]
            public string Address { get; set; }
            [DisplayName("Address Line 2")]
            public string? AddressLine2 { get; set; }
            [DisplayName("City")]
            public string City { get; set; }
            [DisplayName("Region")]
            public string Region { get; set; }
            [DisplayName("Postal Code")]
            public int PostalCode { get; set; }
            [DisplayName("Country")]
            public string Country { get; set; }
            [DisplayName("Landline Number")]
            public string? LandlineNumber { get; set; }
            [Phone]
            [Display(Name = "Phone Number")]
            public string PhoneNumber { get; set; }
        }

        private async Task LoadAsync(Candidate user)
        {
            var userName = await _userManager.GetUserNameAsync(user);
            var phoneNumber = await _userManager.GetPhoneNumberAsync(user);

            Username = userName;
            Input = new InputModel
            {

                FirstName = user.FirstName,
                LastName = user.LastName,
                MiddleName= user.MiddleName,
                Gender = user.Gender,
                Address= user.Address,
                City= user.City,
                Region= user.Region,
                PostalCode= user.PostalCode,
                Country= user.Country,
                LandlineNumber= user.LandlineNumber,
                AddressLine2 = user.AddressLine2,
                BirthDate= user.BirthDate,
                NativeLanguage= user.NativeLanguage,
                PhotoIdIssueDate= user.PhotoIdIssueDate,
                PhotoIdNumber= user.PhotoIdNumber,
                PhotoIdType = user.PhotoIdType,
                PhoneNumber = phoneNumber
            };
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

        public async Task<IActionResult> OnPostAsync()
        {
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

            user.FirstName = Input.FirstName;
            user.LastName = Input.LastName;
            user.MiddleName= Input.MiddleName ;
            user.PhoneNumber = Input.PhoneNumber ;
            user.Country = Input.Country ;
            user.LandlineNumber = Input.LandlineNumber ;
            user.AddressLine2 = Input.AddressLine2;
            user.Address = Input.Address ;
            user.City = Input.City ;
            user.PostalCode = (int)Input.PostalCode;
            user.Country = Input.Country ;
            user.BirthDate = Input.BirthDate ;
            user.PhotoIdIssueDate = Input.PhotoIdIssueDate;
            user.PhotoIdType = Input.PhotoIdType ;
            user.Country = Input.Country ;
            user.LandlineNumber = Input.LandlineNumber ;
            user.Gender= Input.Gender ;
            user.NativeLanguage= Input.NativeLanguage ;
            user.PhotoIdNumber= Input.PhotoIdNumber ;
            user.Region = Input.Region ;

            await _userManager.UpdateAsync(user);

            await _signInManager.RefreshSignInAsync(user);
            StatusMessage = "Your profile has been updated";
            return RedirectToPage();
        }
    }
}