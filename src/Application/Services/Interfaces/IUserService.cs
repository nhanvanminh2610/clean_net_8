using Application.Dtos.Request;
using Application.Dtos.Response;

namespace Application.Services.Interfaces
{
    public interface IUserService
    {
        Task<RegisterResponse> RegisterAsync(RegisterRequest request);
    }
}
