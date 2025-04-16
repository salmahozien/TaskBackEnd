using TaskBackEnd.Dtos;
using TaskBackEnd.Models;

namespace TaskBackEnd.Interfaces
{
    public interface IImageService:IBaseRepository<Image>
    {
        Task<FailAndSuccessDto> AddImages(IFormFile Images,int userId);
    }
}
