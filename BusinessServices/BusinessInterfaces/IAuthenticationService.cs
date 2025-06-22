using Events_system.DTOs;

namespace Events_system.BusinessServices.BusinessInterfaces
{
    public interface IAuthenticationService
    {
        Task<AuthResultDTO> LoginAsync(AuthLoginDTO dto);
        Task<AuthResultDTO> RegisterAsync(AuthRegisterDTO dto);
    }
}