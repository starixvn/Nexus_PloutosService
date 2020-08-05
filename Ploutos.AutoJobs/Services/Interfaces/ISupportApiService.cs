namespace Ploutos.AutoJobs.Services.Interfaces
{
    using Ploutos.AutoJobs.Models;
    using System.Threading.Tasks;

    public interface ISupportApiService
    {
        Task<ProfileResponse> UpdateResponseFromSupportAPI(ProfileResponse profileResponse, string userName, SupportApiConfigs supportApiConfigs);

        Task<ProfileResponse> UpdateResponse(ProfileResponse profileResponse, string userName, SupportApiConfigs supportApiConfigs);
    }
}
