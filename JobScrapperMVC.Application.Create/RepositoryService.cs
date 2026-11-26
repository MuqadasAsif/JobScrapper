using JobScrapperMVC.Domain;

namespace JobScrapperMVC.Application.Create
{
    public class RepositoryService : IRepositoryService
    {
        public List<DataClass> FilterByDate(List<DataClass> jobs, DateOnly postDate)
        {
            var filteredJobs = new List<DataClass>();

            foreach (var job in jobs)
            {
                // Agar date valid nahi hai (null ya galat format), to is job ko skip kar do
                if (!DateOnly.TryParse(job.PostedDate, out var jobDate))
                {
                    continue;
                }

                // Purani filtering logic — jo pehlay APIService mein thi
                if (jobDate < postDate)
                {
                    continue;
                }

                filteredJobs.Add(job);
            }

            return filteredJobs;
        }
    }
}