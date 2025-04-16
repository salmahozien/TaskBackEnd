using TaskBackEnd.Dtos;
using TaskBackEnd.Interfaces;
using TaskBackEnd.Models;

namespace TaskBackEnd.Services
{
    public class ImageService : BaseRepository<Image>,IImageService
    {
        public ImageService(UsersDbContext context) : base(context)
        {
        }

        public async Task<FailAndSuccessDto> AddImages(IFormFile image, int userId)
        {
            var output=new FailAndSuccessDto();
            if (image == null || image.Length == 0)
            {
                output.Fail = "No image uploaded";
            }
            else
            {
                var uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), $"wwwroot/images/{userId}");

                if (!Directory.Exists(uploadsFolder))
                    Directory.CreateDirectory(uploadsFolder);

                var filePath = Path.Combine(uploadsFolder, image.FileName);

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await image.CopyToAsync(stream);
                }

                var savedImage = new Image()
                {
                    UserId = userId,
                    Path = $"images/{image.FileName}",


                };
                await Add(savedImage);
               var count= await CommitChanges();
                if (count > 0) 
                {
                    output.Success = "Image Uploaded Successfully";
                }
            
        }
                return output;
        }
    }
}
