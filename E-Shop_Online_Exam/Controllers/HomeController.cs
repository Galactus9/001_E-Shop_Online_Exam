using EShopOnlineExam.Models;
using EShopOnlineExam.Repository.IRepository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System.Security.Claims;

namespace EShopOnlineExam.Controllers
{
    public class HomeController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly UserManager<Candidate> _userManager;
        private readonly ILogger<HomeController> _logger;

        public HomeController(
            ILogger<HomeController> logger,
            UserManager<Candidate> userManager,
            IUnitOfWork unitOfWork)
        {
            _userManager = userManager;
            _unitOfWork = unitOfWork;
            _logger = logger;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }


        [Authorize]

        public async Task<IActionResult> Support()
        {
            var currentUser = await _userManager.GetUserAsync(User);
            if (User.Identity.IsAuthenticated)
            {
                ViewBag.CurrentUserName = currentUser.UserName;
            }

            var messages = await _unitOfWork.Messages.GetAllAs();
            return View(messages);
        }


        //public async Task<IActionResult> Support(Messages message)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        message.UserName = User.Identity.Name;
        //        var sender = await _userManager.GetUserAsync(User);
        //        message.UserId = sender.Id;
        //        await _unitOfWork.Messages.AddAs(message);
        //        _unitOfWork.Save();
        //        return Ok();
        //    }

        //    return Ok();
        //}

        [Authorize]

        [HttpPost]
        public async void Create(Messages model)
        {
            //Messages message = new Messages
            //{
            //	Text = model.Text,
            //	UserName= model.UserName,
            //};
            //if (message.IsValid)
            //{
            var Identity = (ClaimsIdentity)User.Identity;
            var candidateId = Identity.FindFirst(ClaimTypes.NameIdentifier);
            Candidate cand = _unitOfWork.Candidate.Get(candidateId.Value);
            model.UserName = User.Identity.Name;
            model.Sender = cand;
            model.UserId = candidateId.Value;

            //   var sender = await _userManager.GetUserAsync(candidateId.Value);
            //         model.UserId = sender.Id;
            await _unitOfWork.Messages.AddAs(model);
            _unitOfWork.Save();

            //}

        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
        [HttpPost]
        public JsonResult AddComment(string text, string pageUrl,string priorityLevel)
        {
            var Identity = (ClaimsIdentity)User.Identity;
            var userId = Identity.FindFirst(ClaimTypes.NameIdentifier).Value;

            var qcnote = new QcNote()
            {
                Text = text,
                PageUrl = pageUrl,
                DateCreated = DateTime.Now,
                UserId = userId,
                PriorityLevel = priorityLevel
            };


            _unitOfWork.QcNote.Add(qcnote);
            _unitOfWork.Save();
            return Json(new { success = true });
        }

    }
}