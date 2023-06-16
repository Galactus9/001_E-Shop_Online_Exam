using EShopOnlineExam.Repository.IRepository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Stripe.Checkout;
using System.Security.Claims;
namespace Group_Project.Controllers
{

    public class CertificateShopController : Controller
    {


        private readonly IUnitOfWork _unitOfWork;
        private readonly UserManager<Candidate> _userManager;
        private readonly IUserStore<Candidate> _userStore;

        public CertificateShopController(
            IUnitOfWork unitOfWork,
            UserManager<Candidate> userManager,
            IUserStore<Candidate> userStore)
        {
            _unitOfWork = unitOfWork;
            _userManager = userManager;
            _userStore = userStore;
        }
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            return View(await _unitOfWork.Certificate.GetEnabledCertificates());
        }
        public async Task<IActionResult> Details(int id)
        {
            return View(await _unitOfWork.Certificate.GetAs(id));
        }

        [Authorize]
        [HttpPost]
        public void Details(string certId)
        {
            var Identity = (ClaimsIdentity)User.Identity;
            var candidateId = Identity.FindFirst(ClaimTypes.NameIdentifier);

            Candidate cand = _unitOfWork.Candidate.GetCandidateByIdSync(candidateId.Value);
            Certificate cert = _unitOfWork.Certificate.Get(Int32.Parse(certId));
            CandidateCart candidateCart = new CandidateCart
            {
                Candidate = cand,
                Certificates = cert
            };
            _unitOfWork.CandidateCart.Add(candidateCart);
            _unitOfWork.Save();
        }
        [Authorize]
        [HttpPost]
        [ActionName("Index")]
        public void AddToCart(string certId)
        {
            var Identity = (ClaimsIdentity)User.Identity;
            var candidateId = Identity.FindFirst(ClaimTypes.NameIdentifier);

            Candidate cand = _unitOfWork.Candidate.GetCandidateByIdSync(candidateId.Value);
            Certificate cert = _unitOfWork.Certificate.Get(Int32.Parse(certId));
            CandidateCart candidateCart = new CandidateCart
            {
                Candidate = cand,
                Certificates = cert
            };
            _unitOfWork.CandidateCart.Add(candidateCart);
            _unitOfWork.Save();
        }

        // Step 1.
        public async Task<IActionResult> Cart()
        {
            var Identity = (ClaimsIdentity)User.Identity;
            var candidateId = Identity.FindFirst(ClaimTypes.NameIdentifier);
            Candidate cand = _unitOfWork.Candidate.Get(candidateId.Value);
            IEnumerable<CandidateCart> certs = await _unitOfWork.CandidateCart.GetAllCertsForCand(candidateId.Value);
            return View(certs);
        }

        public async Task<IActionResult> RemoveFromCart(int id)
        {
            var candCert = _unitOfWork.CandidateCart.Get(id);
            _unitOfWork.CandidateCart.Delete(candCert);
            _unitOfWork.Save();
            return RedirectToAction(nameof(Cart));
        }
        public async Task<IActionResult> RemoveAll(CandidateCart candidateCart)
        {
            var candidateCarts = _unitOfWork.CandidateCart.GetAll();
            foreach (var candCert in candidateCarts)
            {
                _unitOfWork.CandidateCart.Delete(candCert);
            }
            _unitOfWork.Save();
            return RedirectToAction(nameof(Cart));
        }


        public async Task<IActionResult> Buy(string id)
        {
            var candCerts = await _unitOfWork.CandidateCart.GetAllCertsForCand(id);
            Candidate cand = _unitOfWork.Candidate.Get(id);
            foreach (var cert in candCerts)
            {
                CandidateCertificate candCert = new CandidateCertificate
                {
                    Candidate = cand,
                    Certificate = cert.Certificates
                };

                _unitOfWork.CandidateCertificate.Add(candCert);
                _unitOfWork.Save();
            }
            return RedirectToAction("PurchasedPOST", "CertificateShop", new { cand = cand, candCerts = candCerts }); /*View("Purchased")*/;
        }

        // Step 2.
        public IActionResult Purchased(IEnumerable<CandidateCart> candidateCarts)
        {

            return LocalRedirect("/Identity/Account/Manage/Certificates");

        }

        public async Task<IActionResult> Completed(string orderId, string candId)
        {
            Candidate cand = _unitOfWork.Candidate.Get(candId);
            var orders = (List<Order>)await _unitOfWork.Order.OrdersGetByOrderIdLoadTopics(orderId);
            foreach (var order in orders)
            {

                List<Exam> exams = (List<Exam>)await _unitOfWork.Exam.WhereCertId(order.Certificate.Id);
                Random random = new Random();
                Exam exam = new Exam();

                if (exams.Count > 0)
                {
                    exam = exams.First();
                }
                else
                {
                    exam = new Exam()
                    {
                        Certificate = order.Certificate,
                        ExamDuration = 600,
                    };
                }


                CandidateExamination cexam = new CandidateExamination
                {
                    Candidate = cand,
                    Exam = exam,
                    timestamp = exam.ExamDuration
                };
                CandidateCertificate ccert = new CandidateCertificate
                {
                    Certificate = order.Certificate,
                    Candidate = cand,
                };
                _unitOfWork.CandidateCertificate.Add(ccert);
                _unitOfWork.CandidateExamination.Add(cexam);

            }


            var candCert = await _unitOfWork.CandidateCart.GetAllCertsForCand(candId);
            foreach (var cert in candCert)
            {
                _unitOfWork.CandidateCart.Delete(cert);
            }
            _unitOfWork.Save();
            return LocalRedirect("/Identity/Account/Manage/Certificates");

        }


        //Step 3.
        [HttpPost]
        [ActionName("Purchased")]
        public async Task<IActionResult> PurchasedPOST()
        {
            var Identity = (ClaimsIdentity)User.Identity;
            var candidateId = Identity.FindFirst(ClaimTypes.NameIdentifier);
            Candidate cand = _unitOfWork.Candidate.Get(candidateId.Value);
            IEnumerable<CandidateCart> certs = await _unitOfWork.CandidateCart.GetAllCertsForCand(candidateId.Value);

            HttpContext.Session.SetString("Email", cand.Email.ToString());
            var sesId = HttpContext.Session.Id;
            var orderId = sesId + DateTime.Now.ToString("MMddyyyyhhmmss");
            //stripe settings
            List<Order> orders = new List<Order>();
            foreach (var cert in certs)
            {
                orders.Add(new Order()
                {

                    Certificate = cert.Certificates,
                    Candidate = cert.Candidate,
                    OrderDate = new DateTime(),
                    ShippingDate = new DateTime(),
                    OrderTotal = (double)cert.Certificates.Price
                });
            }


            List<Certificate> certificates = new List<Certificate>();
            foreach (var cert in certs)
            {
                certificates.Add(cert.Certificates);

            }

            // stripe
            var domain = "https://localhost:7168/";
            var options = new SessionCreateOptions
            {
                PaymentMethodTypes = new List<string>
                {
                  "card",
                },
                LineItems = new List<SessionLineItemOptions>(),
                Mode = "payment",
                SuccessUrl = domain + $"CertificateShop/Completed?orderId={orderId}&candId={candidateId.Value}",
                CancelUrl = domain + $"CertificateShop/Cart",
            };

            foreach (var item in certificates)
            {

                var sessionLineItem = new SessionLineItemOptions
                {
                    PriceData = new SessionLineItemPriceDataOptions
                    {
                        UnitAmount = (long)(item.Price * 100),//20.00 -> 2000
                        Currency = "eur",
                        ProductData = new SessionLineItemPriceDataProductDataOptions
                        {
                            Name = item.Title
                        },

                    },
                    Quantity = 1,
                };
                options.LineItems.Add(sessionLineItem);

            }

            var service = new SessionService();
            Session session = service.Create(options);
            foreach (var order in orders)
            {
                order.SessionId = session.Id;
                order.PaymentIntentId = session.PaymentIntentId;
                order.OrderId = orderId;
            }
            _unitOfWork.Order.AddRange(orders);
            _unitOfWork.Save();

            Response.Headers.Add("Location", session.Url);
            return new StatusCodeResult(303);
            //return View();
        }
        [HttpGet]
        public async Task<int> GetCartCount()
        {
            var Identity = (ClaimsIdentity)User.Identity;
            var candidateId = Identity.FindFirst(ClaimTypes.NameIdentifier);
            if (candidateId == null)
            {
                return 0;
            }
            IEnumerable<CandidateCart> count = new List<CandidateCart>();
            try
            {
                count = await _unitOfWork.CandidateCart.GetAllCertsForCand(candidateId.Value);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return (count.Count());
        }
    }
}