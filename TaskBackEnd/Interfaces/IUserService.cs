using TaskBackEnd.Dtos;
using TaskBackEnd.Models;

namespace TaskBackEnd.Interfaces
{
    public interface IUserService:IBaseRepository<User>
    {
        Task<FailAndSuccessDto> AddUser(AddUserDto model);
        Task<List<UserDto>> GetAllUsers();
        Task<byte[]> GeneratePdf(int userId);

    }
}
