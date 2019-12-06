using Ploutos.AutoJobs.Models;
using System.Threading.Tasks;

namespace Ploutos.AutoJobs.Services.Interfaces
{
    public interface ISupportApiService
    {
        Task<ProfileResponse> UpdateResponseFromSupportAPI(ProfileResponse profileResponse, string userName, SupportApiConfigs supportApiConfigs);
    }
}
