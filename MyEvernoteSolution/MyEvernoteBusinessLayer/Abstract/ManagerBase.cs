using MyEvernote.Core.DataAccess;
using MyEvernote.DataAccessLayer.EntityFramework;
using MyEvernote.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyEvernoteBusinessLayer.Abstract
{
    public abstract class ManagerBase<T> : IDataAccess<T> where T : class
    {
        private Repository<T> repo = new Repository<T>();

        public int Delete(T obj)
        {
            return repo.Delete(obj);
        }

        public T Find(System.Linq.Expressions.Expression<Func<T, bool>> where)
        {
            return repo.Find(where);
        }

        public int Insert(T obj)
        {
            return repo.Insert(obj);

        }

        public List<T> List(System.Linq.Expressions.Expression<Func<T, bool>> where)
        {
            return repo.List(where);
        }

        public List<T> List()
        {
            return repo.List();
        }

        public IQueryable<T> ListQueryable()
        {
            return repo.ListQueryable();
        }

        public int Save()
        {
            return repo.Save();
        }

        public int Update(T obj)
        {
            return repo.Update(obj);
        }

    }
    
}
