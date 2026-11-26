using JobScrapperMVC.Data;
using JobScrapperMVC.Domain;
using JobScrapperMVC.Application.Create;
using Microsoft.AspNetCore.Mvc;

namespace JobScrapperMVC.Controllers
{
    public class JobViewController : Controller
    {
        private readonly IAPIService _apiservice;
        private readonly AppDbContext _context;
        private readonly IRepositoryService _jobFilterService;
        public JobViewController(IAPIService apiservice, AppDbContext context, IRepositoryService jobFilterService)
        {
            _apiservice = apiservice;
            _context = context;
            _jobFilterService = jobFilterService;
        }

        public IActionResult Index()
        {
            return View();
        }
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Login(string email, string password)
        {
            var user = _context.Users.FirstOrDefault(u => u.Email == email && u.Password == password);
            if (user != null)
            {
                return RedirectToAction("GetStarted");
            }
            else
            {
                ViewBag.Error = "Invalid email or password";
                return View();
            }
        }

        public IActionResult Registeration()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Registeration(string fullname, string email, string password, string confirmPassword)
        {
            var user = new User { FullName = fullname, Email = email, Password = password };
            _context.Users.Add(user);
            _context.SaveChanges();
            return RedirectToAction("Login");
        }
        public IActionResult GetStarted()
        {
            return View(new List<DataClass>());
        }
       
            [HttpGet]
            public async Task<IActionResult> FindJobs(string Title, string location, int pages, DateOnly PostDate, int page = 1)
            {
            // STEP 1: Infrastructure se saari jobs lo (koi filter nahi)
            var allJobs = await _apiservice.FindJobs(Title, location, pages, PostDate);

            // STEP 2: Application se filter karwao (date ke hisab se)
            var filteredJobs = _jobFilterService.FilterByDate(allJobs, PostDate);
            // STEP 3: Pagination — ye Web/Controller ka apna kaam hai
            int pageSize = 10;
            var jobsForCurrentPage = filteredJobs
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToList();

            ViewBag.TotalPages = (int)Math.Ceiling(filteredJobs.Count / (double)pageSize);
            if (jobsForCurrentPage.Count == 0)
            {
                ViewBag.warning = "No jobs found";
            }
            return View("GetStarted", jobsForCurrentPage);

        }
        public IActionResult ExportToExcel()
        {
            string path = "LinkdInJobmvc.csv";

            byte[] fileBytes = System.IO.File.ReadAllBytes(path);

            return File(fileBytes, "text/csv", "Jobs.csv");
        }


    }
}
