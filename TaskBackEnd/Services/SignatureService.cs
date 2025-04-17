using Microsoft.AspNetCore.Mvc;
using TaskBackEnd.Dtos;
using TaskBackEnd.Interfaces;
using TaskBackEnd.Models;

namespace TaskBackEnd.Services
{
    public class SignatureService : BaseRepository<Signature>, ISignatureService
    {
        public SignatureService(UsersDbContext context) : base(context)
        {
        }

        public async Task<FailAndSuccessDto> SaveSignature(IFormFile Signature, int UserId)
        {
            var output = new FailAndSuccessDto();



            // Remove the base64 prefix (e.g., "data:image/png;base64,")
            string signatureFileName = $"{Guid.NewGuid()}_{Signature.FileName}_{UserId}";
            var signaturePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/signatures", signatureFileName);
            Directory.CreateDirectory(Path.GetDirectoryName(signaturePath));
            using (var stream = new FileStream(signaturePath, FileMode.Create))
            {
                await Signature.CopyToAsync(stream);
            }



            var addedSignature = new Signature()
            {
                SignaturePath = signaturePath,
                UserId = UserId

            };
            await Add(addedSignature);
            var count = await CommitChanges();
            if (count > 0)
            {
                output.Success = "Signature added successfully";
            }
            else
            {
                output.Fail = "Cann't add Signature ";
            }
            return output;

        }
        
    
    }
}
