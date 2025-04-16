using TaskBackEnd.Interfaces;
using TaskBackEnd.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections;
using System.Linq.Expressions;

namespace TaskBackEnd.Services
{
    public class BaseRepository<T> : IBaseRepository<T> where T : class
    {
        private readonly UsersDbContext _Context;

        public BaseRepository(UsersDbContext context)
        {
            _Context = context;
        }


        public async Task<T> Add(T entity)
        {
            await _Context.Set<T>().AddAsync(entity);
            return entity;
        }

        public async Task<T> Delete(T entity)
        {
            _Context.Set<T>().Remove(entity);
            return entity;
        }

        public async Task<T> Update(T entity)
        {
            var x = _Context.Entry(entity).State;
            _Context.Set<T>().Update(entity);
            return entity;
        }

        public async Task<IEnumerable<T>> GetAll()
        {
            return await _Context.Set<T>().ToListAsync();
        }

        public async Task<T> FindById(int id)
        {
            var entity = await _Context.Set<T>().FindAsync(id);
            if (entity == null) return null;
            _Context.Entry(entity).State = EntityState.Detached;
            return entity;

        }
        public async Task<T> FindByIdWithData(int id)
        {
            var col = GetCollections(typeof(T));
            var entry = await FindById(id);
            if (entry == null) return null;
            IQueryable<T> query = _Context.Set<T>();
            foreach (var inc in col)
            {
                query = query.Include(inc).AsQueryable();
            }
            var entity = await query.Where(d => d.Equals(entry)).AsNoTracking().SingleOrDefaultAsync();
            if (entity == null) return null;
            _Context.Entry(entity).State = EntityState.Detached;
            return entity;
        }

        public async Task<T> FindById(string id)
        {
            var entity = await _Context.Set<T>().FindAsync(id);
            if (entity == null) return null;
            _Context.Entry(entity).State = EntityState.Detached;
            return entity;

        }
        public async Task<T> FindByIdWithData(string id)
        {
            var col = GetCollections(typeof(T));
            var entry = await FindById(id);
            if (entry == null) return null;
            IQueryable<T> query = _Context.Set<T>().AsNoTracking();
            foreach (var inc in col)
            {
                query = query.Include(inc).AsQueryable();
            }
            var entity = await query.Where(d => d.Equals(entry)).AsNoTracking().SingleOrDefaultAsync();
            if (entity == null) return null;
            _Context.Entry(entity).State = EntityState.Detached;
            return entity;
        }

        public async Task<int> Count()
        {
            return await _Context.Set<T>().CountAsync();
        }

        public async Task<IEnumerable<T>> GetAllWithData()
        {
            var query = _Context.Set<T>().AsNoTracking().AsQueryable();
            var includes = GetCollections(typeof(T));
            foreach (var inc in includes)
            {
                query = query.Include(inc);
            }
            return await query.ToListAsync();
        }

        public async Task<T> Find(Expression<Func<T, bool>> criteria)
        {

            var result = await _Context.Set<T>().AsNoTracking().AsQueryable().AsNoTracking().FirstOrDefaultAsync(criteria);
            if (result == null) return null;
            _Context.Entry(result).State = EntityState.Detached;
            return result;

        }
        public async Task<T> FindWithData(Expression<Func<T, bool>> criteria)
        {
            var col = GetCollections(typeof(T));
            var query = _Context.Set<T>().AsNoTracking().AsQueryable();
            foreach (var inc in col)
            {
                query = query.Include(inc);
            }
            var entity = await query.SingleOrDefaultAsync(criteria);
            if (entity == null) return null;
            _Context.Entry(entity).State = EntityState.Detached;
            return entity;
        }
        public async Task<IEnumerable<T>> FindAll(Expression<Func<T, bool>> criteria)
        {
            return await _Context.Set<T>().AsNoTracking().AsQueryable().Where(criteria).ToListAsync();
        }
        public async Task<IEnumerable<T>> FindAllWithData(Expression<Func<T, bool>> criteria)
        {
            //use AsNotTraking because hete only retrieve data no need to upply any updates 
            var col = GetCollections(typeof(T));
            var query = _Context.Set<T>().AsNoTracking().AsQueryable();

            foreach (var inc in col)
            {
                query = query.Include(inc).AsNoTracking();
            }
            query = query.Where(criteria);
            return await query.ToListAsync();
        }

        public async Task<T> FindByIdWithCustomColData(int id, string Include)
        {
            var entity = await _Context.Set<T>().FindAsync(id);
            if (entity == null) return null;
            _Context.Entry(entity).Collection(Include).Load();
            _Context.Entry(entity).State = EntityState.Detached;
            return entity;
        }

        public async Task<T> FindByIdWithCustomRefData(int id, string Include)
        {
            var entity = await _Context.Set<T>().FindAsync(id);
            if (entity == null) return null;
            _Context.Entry(entity).Reference(Include).Load();
            _Context.Entry(entity).State = EntityState.Detached;
            return entity;
        }
        public async Task<T> FindByIdWithCustomColData(string id, string Include)
        {
            var entity = await _Context.Set<T>().FindAsync(id);
            if (entity == null) return null;
            _Context.Entry(entity).Collection(Include).Load();
            _Context.Entry(entity).State = EntityState.Detached;
            return entity;
        }
        public async Task<T> FindByIdWithCustomRefData(string id, string Include)
        {
            var entity = await _Context.Set<T>().FindAsync(id);
            if (entity == null) return null;
            _Context.Entry(entity).Reference(Include).Load();
            _Context.Entry(entity).State = EntityState.Detached;
            return entity;
        }

        public async Task<T> DetachEntry(T entity)
        {
            var a = _Context.Entry(entity).State;
            _Context.Entry(entity).State = EntityState.Detached;
            return entity;
        }

        public async Task<int> CommitChanges()
        {
            return await _Context.SaveChangesAsync();
        }

        public async Task RollBack() => await _Context.DisposeAsync();

        public List<string> GetCollections(Type entityType)
        {
            var col = entityType.GetProperties()
                                .Where(p => (typeof(IEnumerable).IsAssignableFrom(p.PropertyType)
                                    && p.PropertyType != typeof(string))
                                    && p.PropertyType != typeof(byte[])
                                    || (p.PropertyType.Namespace == entityType.Namespace))
                                .Select(p => p.Name)
                                .ToList();
            return col;
        }

        public Expression<Func<T, bool>> CombineExpressions<T>(
        Expression<Func<T, bool>> left,
        Expression<Func<T, bool>> right,
        ExpressionType binaryType)
        {
            var combinedBody = Expression.MakeBinary(binaryType, left.Body, right.Body);
            var combinedLambda = Expression.Lambda<Func<T, bool>>(combinedBody, left.Parameters[0]);
            return combinedLambda;
        }
    }
}
