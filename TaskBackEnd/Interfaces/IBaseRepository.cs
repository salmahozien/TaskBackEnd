using System.Linq.Expressions;

namespace TaskBackEnd.Interfaces
{
    public interface IBaseRepository<T>
    {
        Task<T> FindById(int id);
        Task<T> FindByIdWithData(int id);
        Task<T> FindById(string id);
        Task<T> FindByIdWithData(string id);
        Task<T> Add(T entity);
        Task<T> Update(T entity);
        Task<T> Delete(T entity);
        Task<IEnumerable<T>> GetAll();
        Task<IEnumerable<T>> GetAllWithData();
        Task<int> Count();
        Task<T> Find(Expression<Func<T, bool>> criteria);
        Task<T> FindWithData(Expression<Func<T, bool>> criteria);
        Task<IEnumerable<T>> FindAll(Expression<Func<T, bool>> criteria);
        Task<IEnumerable<T>> FindAllWithData(Expression<Func<T, bool>> criteria);
        Task<T> FindByIdWithCustomColData(int id, string Include);
        Task<T> FindByIdWithCustomColData(string id, string Include);
        List<string> GetCollections(Type entityType);

        Task<int> CommitChanges();
        Task<T> DetachEntry(T entity);
        Task RollBack();
        Task<T> FindByIdWithCustomRefData(string id, string Include);
        Task<T> FindByIdWithCustomRefData(int id, string Include);
        Expression<Func<T, bool>> CombineExpressions<T>(Expression<Func<T, bool>> left, Expression<Func<T, bool>> right, ExpressionType binaryType);
    }
}
