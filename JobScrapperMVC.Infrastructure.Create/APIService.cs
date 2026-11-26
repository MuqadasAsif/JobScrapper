using HtmlAgilityPack;
using JobScrapperMVC.Domain;              // agar DataClass Domain mein hai
using JobScrapperMVC.Application.Create;  // agar IAPIService Application mein hai
namespace JobScrapperMVC.Infrastructure.Create
{
    public class APIService : IAPIService
    {
        private readonly HttpClient _httpClient;
        public APIService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }
        public async Task<List<DataClass>> FindJobs(string Title, string Location, int pages, DateOnly PostDate)
        {
            _httpClient.DefaultRequestHeaders.UserAgent.ParseAdd("Mozilla/5.0"); //extra information about the request send with the request means they are coming in which environment
            _httpClient.DefaultRequestHeaders.AcceptLanguage.ParseAdd("en-US,en;q=0.9");//our prefered language for the response q=0.9 ka matlab agar english us specific nhi to simple english bhi chalay gi

            //our url where we have to send the request
            string baseUrl = "https://www.linkedin.com/jobs-guest/jobs/api/seeMoreJobPostings/search";
            DateTime postDateTime = PostDate.ToDateTime(TimeOnly.MinValue);
            int secondsSincePost = (int)(DateTime.Now - postDateTime).TotalSeconds;
            if (secondsSincePost < 0) secondsSincePost = 0; // agar PostDate future mein ho to 0 kar do
            //List where we will save the objects of jobs
            List<DataClass> lists = new List<DataClass>();


            //loop according to the pages 
            for (int i = 0; i < pages; i++)
            {
                int start = i * 10;
                //our base url added with search parameters
                string url = baseUrl
                + "?keywords=" + Uri.EscapeDataString(Title) //uri.escapedatastring to remove the extra space between title
                + "&location=" + Uri.EscapeDataString(Location) +
                "&f_TPR=r" + secondsSincePost + "&sortBy=DD" +
                "&start=" + start;
                await Task.Delay(2000);
                //getting response message 
                HttpResponseMessage response = await _httpClient.GetAsync(url);

                Console.WriteLine("===== DEBUG START =====");
                Console.WriteLine("URL: " + url);
                Console.WriteLine("Status Code: " + response.StatusCode);


                if (response.IsSuccessStatusCode)
                {
                    //read the html response in string
                    string html = await response.Content.ReadAsStringAsync();

                    //using htmldocument class to convert the string response to html structure
                    HtmlDocument document = new HtmlDocument();
                    document.LoadHtml(html);

                    //searching the div with the class-- from node root here documentnode is the root node and selectnodes will select al the nodes of the condition
                    var jobs = document.DocumentNode.SelectNodes("//div[contains(@class,'job-search-card')]");
                    if (jobs != null)
                    {
                        foreach (var job in jobs)
                        {
                            //performing searches of our properties from all the jobs one by one
                            var title = job.SelectSingleNode(".//h3");
                            var TitleText = title.InnerText.Trim();
                            var company = job.SelectSingleNode(".//a[contains(@class,'hidden-nested-link')]");
                            var CompanyText = company.InnerText.Trim();
                            var Companylocation = job.SelectSingleNode(".//span[contains(@class, 'job-search-card__location')]");
                            var CompanyLocationText = Companylocation.InnerText.Trim();
                            var time = job.SelectSingleNode(".//time");
                            var actualDateStr = time.GetAttributeValue("datetime", "");
                         
                            var JobUrl = job.SelectSingleNode(".//a[contains(@class,'base-card__full-link')]");
                            var JobUrlText = JobUrl.GetAttributeValue("href", "...");

                            //creating class objects to make objects of each job
                            DataClass joblist = new DataClass();
                            joblist.Title = TitleText;
                            joblist.Company = CompanyText;
                            joblist.Location = CompanyLocationText;
                            joblist.PostedDate = actualDateStr;
                            joblist.JobUrl = JobUrlText;

                            //adding in the list
                            lists.Add(joblist);
                        }
                    }
                }

            }
            string path = "LinkdInJobmvc.csv";

            using (StreamWriter writer = new StreamWriter(path))
            {
                writer.WriteLine(
                    "Title,Company,Location,PostDate,JobUrl");

                foreach (var job in lists)
                {
                    writer.WriteLine(
                        $"\"{job.Title}\",\"{job.Company}\",\"{job.Location}\",\"{job.PostedDate}\",\"{job.JobUrl}\"");
                }
            }

            return lists;
        }
    }
}