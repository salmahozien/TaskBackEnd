﻿using Mapster;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;
using QuestPDF.Drawing;
using TaskBackEnd.Dtos;
using TaskBackEnd.Interfaces;
using TaskBackEnd.Models;

namespace TaskBackEnd.Services
{
    public class UserService : BaseRepository<User>, IUserService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly UsersDbContext _context;
        public UserService(UsersDbContext context, IUnitOfWork unitOfWork) : base(context)
        {
            _unitOfWork = unitOfWork;
            _context = context;
        }

        public async Task<FailAndSuccessDto> AddUser(AddUserDto model)
        {
            var output = new FailAndSuccessDto();
            if (model == null)
            {
                output.Fail = "Empty Model";

            }
            else
            {
                var transaction = _context.Database.BeginTransaction();
                var user = model.Adapt<User>();
                user.Images = null;
                await Add(user);
                
                var count = await CommitChanges();
                if (count > 0)
                {
                    foreach (var image in model.Images)
                    {
                        var addedImage = await _unitOfWork.Images.AddImages(image, user.Id);
                        if (addedImage.Fail != string.Empty)
                        {
                            output.Fail = addedImage.Fail;
                            await transaction.RollbackAsync();
                            return output;
                        }
                    }
                    var signature = await _unitOfWork.Signatures.SaveSignature(model.Signature, user.Id);
                    if (signature.Fail != string.Empty)
                    {
                        output.Fail = signature.Fail;
                    }
                    else
                    {
                        await transaction.CommitAsync();
                        output.Success = "User Added SuccessFully";
                    }

                }


            }
            return output;
        }
        public async Task<List<UserDto>> GetAllUsers()
        {
            var users = await _unitOfWork.Users.GetAll();
            if (users.Any())
            {
                var allUsers = users.Adapt<List<UserDto>>();
                return allUsers;
            }
            else
            {
                return new List<UserDto>();
            }
        }
        public async Task<byte[]> GeneratePdf(int userId)
        {
            var user = await FindById(userId);
            var imagePaths = GetUserImagePaths(userId);
            var signature = await _unitOfWork.Signatures.Find(x => x.UserId == userId);
            return Document.Create(container =>
            {
                container.Page(page =>
                {
                    page.Size(PageSizes.A4);
                    page.Margin(30);
                    page.DefaultTextStyle(x => x.FontSize(14));

                    page.Content()
                    .Border(1) 
                    .Padding(10)
                        .Column(column =>
                        {
                            column.Item().Text($"First Name: {user.FirstName}");
                            column.Item().Text("");
                            column.Item().Text($"Last Name: {user.LastName}");
                            column.Item().Text("");
                            column.Item().Text($"Email: {user.Email}");
                            column.Item().Text("");
                            column.Item().Text($"Phone Number: {user.PhoneNumber}");

                            column.Item().PaddingTop(10).Text("Uploaded Images:").Bold();
                            column.Item().Text("");
                            foreach (var imagePath in imagePaths)
                            {
                                column.Item().Element(container =>
                                container
                               .PaddingBottom(10)
                               
                              .Image(imagePath, ImageScaling.FitWidth)
                            );
                                column.Item().Text($"Signature:");
                                column.Item().Text("");
                                column.Item().Element(container =>
                               container
                              .PaddingBottom(10)

                             .Image(signature.SignaturePath, ImageScaling.FitWidth));

                            }
                        })
                        ;
                });
            }).GeneratePdf();
        }
        private List<string> GetUserImagePaths(int userId)
        {
            var folderPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "images", userId.ToString());

            if (!Directory.Exists(folderPath))
                return new List<string>();

            return Directory.GetFiles(folderPath)
                            .Where(file => file.EndsWith(".jpg") || file.EndsWith(".png") || file.EndsWith(".jpeg"))
                            .ToList();
        }
    }
}
