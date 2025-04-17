namespace TaskBackEnd.Interfaces
{
    public interface IUnitOfWork : IDisposable
    {
        IUserService Users { get; }
        IImageService Images {  get; }
        ISignatureService Signatures { get; }
    }
}
