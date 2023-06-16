using EShopOnlineExam.Models;
using EShopOnlineExam.Repository;
using EShopOnlineExam.Repository.IRepository;
using EShopOnlineExam.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace EShopOnlineExam.Controllers
{
    [Authorize]
    public class ExamController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly UserManager<Candidate> _userManager;
        public ExamController(IUnitOfWork unitOfWork, UserManager<Candidate> userManager)
        {
            _unitOfWork = unitOfWork;
            _userManager = userManager;
        }
        //[Authorize(Roles = "Admin")]

        // Exam Id -->
        [HttpGet]
        public async Task<IActionResult> ExamUI(int id, string button)
        {
            var currentUser = await _userManager.GetUserAsync(User);
            if (User.Identity.IsAuthenticated)
            {
                ViewBag.CurrentUserName = currentUser.UserName;
            }

            ViewBag.Messages = await _unitOfWork.Messages.GetAllAs();

            var candidateExamination = await _unitOfWork.CandidateExamination.GetWithExam(id);
            var examQuestionAnswers = await _unitOfWork.ExamQuestion.WhereExamId(candidateExamination.Exam.Id);
            List<CandidateResults> candidateResults = new List<CandidateResults>();

            if(button == "Start")
            {

                foreach(var question in examQuestionAnswers)
                {
                    candidateResults.Add(new CandidateResults 
                    { 
                        ExamQuestion = question, 
                        candidateExamination = candidateExamination, 
                        CandidateQuestionAnswer = -1 
                    });
                }
                _unitOfWork.CandidateResults.AddRange(candidateResults);
                _unitOfWork.Save();
            }
            else
            {
                candidateResults = (List<CandidateResults>)await _unitOfWork.CandidateResults.WhereExaminationId(candidateExamination.Id);
            }
            return View(candidateResults);
        }


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
        [HttpPost]
        [ActionName("ExamUI")]
        public void ExamPost(string id, string examId, string timeStamp)
        {
            if (timeStamp != null)
            {
                var examination = _unitOfWork.CandidateExamination.Get(Int32.Parse(examId));
                examination.timestamp = Int32.Parse(timeStamp);
                _unitOfWork.CandidateExamination.Update(examination);
                _unitOfWork.Save();
            }
            else
            {
                string[] value = id.Split(',');
                int answer = Int32.Parse(value[0]);
                int CandidateResultsId = Int32.Parse(value[1]);
                _unitOfWork.CandidateResults.UpdateCandidateQuestionAnswer(CandidateResultsId, answer);
                _unitOfWork.Save();
            }
        }



        [HttpGet]
        public async Task<IActionResult> ExamSubmit(int Id)
        {
            ExamEngine examEngine = new ExamEngine();
            await examEngine.ExamEngineResult(_unitOfWork, Id);
            var candidateExamination = await _unitOfWork.CandidateExamination.GetWithExam(Id);
            var candidateResults = (List<CandidateResults>)await _unitOfWork.CandidateResults.WhereExaminationIdFull(candidateExamination.Id);
            var examTopics = (List<ExamTopics>)await _unitOfWork.ExamTopic.GetEnableExamTopic(candidateExamination.Exam.Id);
            ViewBag.CandidateExamination = candidateExamination;
            ViewBag.CandidateResults = candidateResults;
            ViewBag.ExamTopics = examTopics;
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> ExamPDF(int Id)
        {
            var candidateExamination = await _unitOfWork.CandidateExamination.GetAllCandidateExaminationsExamId(Id);
            ExamEngine examEngine = new ExamEngine();
            var result = await examEngine.ExamEngineResult(_unitOfWork, Id);

            //return View(result);
            PdfHandler pdf = new PdfHandler(_unitOfWork);
            var constant = await pdf.GenerateCertificakePdf(Id);
            return File(constant, "application/void", "ItsPDF.pdf");
        }


    }
}