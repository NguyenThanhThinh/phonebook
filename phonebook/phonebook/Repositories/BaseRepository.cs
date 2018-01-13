using System;
using System.Collections.Generic;
using System.Linq;

namespace phonebook.Repositories
{
    using phonebook.Models;
    using System.Data.Entity;
    using System.Linq.Expressions;

    public class BaseRepository<T> where T : BaseEntity
    {
        public DbContext Context { get; set; }

        public DbSet<T> DbSet { get; set; }

        public BaseRepository()
        {
            Context = new PhonebookDbcontext();

            DbSet = Context.Set<T>();
        }

        public List<T> GetAll(Expression<Func<T, bool>> filter = null)
        {
            IQueryable<T> result = DbSet;

            if (filter != null)
            {
                return result.Where(filter).ToList();
            }
            else
            {
                return result.ToList();
            }
        }

        public T GetById(int id)
        {
            return DbSet.Find(id);
        }

        public void Save(T entity)
        {
            if (entity.Id > 0)
            {
                Update(entity);
            }
            else
            {
                Insert(entity);
            }
        }

        private void Insert(T entity)
        {
            DbSet.Add(entity);
            Context.SaveChanges();
        }

        private void Update(T entity)
        {
            Context.Entry(entity).State = EntityState.Modified;
            Context.SaveChanges();
        }

        public void Delete(T entity)
        {
            DbSet.Remove(entity);
            Context.SaveChanges();
        }
    }
}