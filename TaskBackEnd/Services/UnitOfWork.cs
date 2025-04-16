using TaskBackEnd.Interfaces;
using TaskBackEnd.Models;

namespace TaskBackEnd.Services
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly UsersDbContext _context;
        public IUserService Users { get; private set; }
        public IImageService Images { get; private set; }
        public UnitOfWork(UsersDbContext context)
        {
            _context = context;
            Users=new UserService(context,this);
            Images = new ImageService(context);
        }
            public void Dispose()
        {
            throw new NotImplementedException();
        }
    }
}
