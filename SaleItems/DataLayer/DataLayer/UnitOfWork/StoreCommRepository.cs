using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Transactions;

namespace DataLayer.UnitOfWork
{
    public class StoreCommRepository<T> where T : class
    {
        internal StoreCommEntities context;
        internal DbSet<T> dbset;

        public StoreCommRepository(StoreCommEntities context)
        {
            this.context=context;
            this.dbset=context.Set<T>();
        }

        public virtual IEnumerable<T> Get()
        {
            IQueryable<T> query = dbset;
            return query.ToList();
        }

        public virtual T GetById(object id)
        {
            return dbset.Find(id);
        }

        public virtual void Insert(T entity)
        {
            dbset.Add(entity);
        }

        public virtual void InsertBulk(IEnumerable<T> entities)
        {
            using (TransactionScope scope = new TransactionScope())
            {
                
                try
                {
                    context.Configuration.AutoDetectChangesEnabled = false;

                    int count = 0;
                    foreach (var entityToInsert in entities)
                    {
                        ++count;
                        context = AddToContext(context, entityToInsert, count, 100, true);
                    }

                    context.SaveChanges();
                }
                finally
                {
                    if (context != null)
                        context.Dispose();
                }

                scope.Complete();
            }
        }

        private StoreCommEntities AddToContext(StoreCommEntities context,T entity, int count, int commitCount, bool recreateContext)
        {
            context.Set<T>().Add(entity);

            if (count % commitCount == 0)
            {
                context.SaveChanges();
                if (recreateContext)
                {
                    context.Dispose();
                    context = new StoreCommEntities();
                    context.Configuration.AutoDetectChangesEnabled = false;
                }
            }

            return context;
        }

        public virtual void Delete(object id)
        {
            T entityToDelete = dbset.Find(id);
            Delete(entityToDelete);
        }

        public virtual void Delete(T entityToDelete)
        {
            if (context.Entry(entityToDelete).State == EntityState.Detached)
            {
                dbset.Attach(entityToDelete);
            }
            dbset.Remove(entityToDelete);
        }

        public virtual void Update(T entityToUpdate)
        {
            dbset.Attach(entityToUpdate);
            context.Entry(entityToUpdate).State = EntityState.Modified;
        }

        public virtual void UpdateBulk(IEnumerable<T> entities)
        {
            using (TransactionScope scope = new TransactionScope())
            {

                try
                {
                    context.Configuration.AutoDetectChangesEnabled = false;

                    int count = 0;
                    foreach (var entityToUpdate in entities)
                    {
                        ++count;
                        context = UpdateToContext(context, entityToUpdate, count, 100, true);
                    }

                    context.SaveChanges();
                }
                finally
                {
                    if (context != null)
                        context.Dispose();
                }

                scope.Complete();
            }
        }

        private StoreCommEntities UpdateToContext(StoreCommEntities context, T entity, int count, int commitCount, bool recreateContext)
        {
            context.Set<T>().Attach(entity);
            context.Entry(entity).State = EntityState.Modified;
            if (count % commitCount == 0)
            {
                context.SaveChanges();
                if (recreateContext)
                {
                    context.Dispose();
                    context = new StoreCommEntities();
                    context.Configuration.AutoDetectChangesEnabled = false;
                }
            }

            return context;
        }

        public virtual IEnumerable<T> SQLQuery<T>(string sql, params object[] parameters)
        {
            return context.Database.SqlQuery<T>(sql, parameters);
        }

    }
}
