using TaskBackEnd.Dtos;
using TaskBackEnd.Models;

namespace TaskBackEnd.Interfaces
{
    public interface ISignatureService:IBaseRepository<Signature>
    {
        Task<FailAndSuccessDto> SaveSignature(IFormFile Signature, int UserId);
    }
}
