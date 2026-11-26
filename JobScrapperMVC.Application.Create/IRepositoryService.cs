using JobScrapperMVC.Domain;

namespace JobScrapperMVC.Application.Create
{
    public interface IRepositoryService
    {
        List<DataClass> FilterByDate(List<DataClass> jobs, DateOnly postDate);
    }
}