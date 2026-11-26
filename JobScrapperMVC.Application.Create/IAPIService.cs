using JobScrapperMVC.Domain;
namespace JobScrapperMVC.Application.Create
{

    public interface IAPIService
    {
        Task<List<DataClass>> FindJobs(string Ttle, string location, int pages, DateOnly PostDate);
    }
}
