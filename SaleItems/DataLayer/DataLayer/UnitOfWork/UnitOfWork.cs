using System;
using System.Collections.Generic;
using System.Data.Entity.Validation;
using System.Linq;
using System.Text;

namespace DataLayer.UnitOfWork
{
    public class UnitOfWork : IDisposable
    {
        StoreCommEntities context = new StoreCommEntities();

        StoreCommRepository<StoreUser> storeUserRepository;
        StoreCommRepository<AuthenticationToken> authenticationTokenRepository;
        StoreCommRepository<Reciept> recieptRepository;
        StoreCommRepository<Product> productRepository;
        StoreCommRepository<RecieptOrder> recieptOrderRepository;
        StoreCommRepository<OrderCandidate> orderCandidateRepository;

        public StoreCommRepository<OrderCandidate> OrderCandidateRepository
        {
            get
            {
                if (this.orderCandidateRepository == null)
                    this.orderCandidateRepository = new StoreCommRepository<OrderCandidate>(context);
                return orderCandidateRepository;
            }
        }

        public StoreCommRepository<RecieptOrder> RecieptOrderRepository
        {
            get
            {
                if (this.recieptOrderRepository == null)
                    this.recieptOrderRepository = new StoreCommRepository<RecieptOrder>(context);
                return recieptOrderRepository;
            }
        }

        public StoreCommRepository<StoreUser> StoreUserRepository
        {
            get
            {
                if (this.storeUserRepository == null)
                    this.storeUserRepository = new StoreCommRepository<StoreUser>(context);
                return storeUserRepository;
            }
        }

        public StoreCommRepository<AuthenticationToken> AuthenticationTokenRepository
        {
            get
            {
                if (this.authenticationTokenRepository == null)
                    this.authenticationTokenRepository = new StoreCommRepository<AuthenticationToken>(context);
                return authenticationTokenRepository;
            }
        }

        public StoreCommRepository<Reciept> RecieptRepository
        {
            get
            {
                if (this.recieptRepository == null)
                    this.recieptRepository = new StoreCommRepository<Reciept>(context);
                return recieptRepository;
            }
        }

        public StoreCommRepository<Product> ProductRepository
        {
            get
            {
                if (this.productRepository == null)
                    this.productRepository = new StoreCommRepository<Product>(context);
                return productRepository;
            }
        }

        public void Save()
        {
            try
            {
                context.SaveChanges();
            }
            catch (Exception e)
            {
               
            }
            //catch (DbEntityValidationException e)
            //{
            //    foreach (var eve in e.EntityValidationErrors)
            //    {
            //        Console.WriteLine("Entity of type \"{0}\" in state \"{1}\" has the following validation errors:",
            //            eve.Entry.Entity.GetType().Name, eve.Entry.State);
            //        foreach (var ve in eve.ValidationErrors)
            //        {
            //            Console.WriteLine("- Property: \"{0}\", Error: \"{1}\"",
            //                ve.PropertyName, ve.ErrorMessage);
            //        }
            //    }
            //    throw;
            //}
        }

        private bool disposed = false;

        protected virtual void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing)
                {
                    context.Dispose();
                }
            }
            this.disposed = true;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        public virtual IEnumerable<T> SQLQuery<T>(string sql)
        {
            return context.Database.SqlQuery<T>(sql);
        }

        public virtual IEnumerable<T> SQLQueryWithParameters<T>(string sql, params object[] parameters)
        {
            return context.Database.SqlQuery<T>(sql, parameters);
        }

    }
}
