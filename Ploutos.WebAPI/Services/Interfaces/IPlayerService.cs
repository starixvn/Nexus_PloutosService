namespace Ploutos.WebAPI.Services.Interfaces
{
    using System.Threading.Tasks;

    public interface IPlayerService
    {
        Task<string> GetUserAccountNoByUsername(string username);
    }
}