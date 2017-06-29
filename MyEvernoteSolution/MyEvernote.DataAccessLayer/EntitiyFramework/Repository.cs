using MyEvernote.Common;
using MyEvernote.DataAccessLayer;
using MyEvernote.DataAccessLayer.Abstract;
using MyEvernote.Entities;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace MyEvernote.DataAccessLayer.EntityFramework
{
    public class Repository<T>: RepositoryBase, IRepository<T> where T:class //int gibi alakasız tipte döndürülemesin diye kısıtlama geitrilmeli yoksa bağırıyo
    {
        //private DatabaseContext db = new DatabaseContext();//repository base gelmeden önceki şekli
        private DatabaseContext db;
        private DbSet<T> _objectSet;//dbSet<T> bizim türü bulmamızı sağlar, bunu her fonksiyonun içinde tekrar takrar bulmaktansa ilk geldiğinde değişkene atıcaz 

        public Repository()
        {
            db = RepositoryBase.CreateContext();
            _objectSet = db.Set<T>();
        }
        public List<T> List(Expression<Func<T,bool>>where)//list yerine Iqueryble<T> kullanılabilir faydasını tam olarak anlamadım (repository pattern2 dk:4.30)
        {
            return _objectSet.Where(where).ToList();//where in üstüne gelince gözüken istediği parametre aynen list fonksiyonunun parametresine yazıldı
        }
        public IQueryable<T> ListQueryable()
        {
            return _objectSet.AsQueryable<T>();
        }

        public List<T> List()
        {
            // return db.Set<T>.ToList();// değişken kullanılmadan önceki hali
            return _objectSet.ToList();
        }
        public int Insert(T obj)
        {
            _objectSet.Add(obj);

            if(obj is MyEntityBase)
            {
                MyEntityBase o = obj as MyEntityBase;// şu yapı kafa karıştırdı

                o.CratedOn = DateTime.Now;//her yerde aynı şeyi yazmamak için otomatikleştirildi 
                o.ModifiedOn = DateTime.Now;
                o.ModifiedUsername = App.Common.GetUsername();
            }
            return Save();
        }
        public int Update(T obj)
        {
            if (obj is MyEntityBase)
            {
                MyEntityBase o = obj as MyEntityBase;// şu yapı kafa karıştırdı

                o.ModifiedOn = DateTime.Now;//her yerde aynı şeyi yazmamak için otomatikleştirildi 
                o.ModifiedUsername = App.Common.GetUsername();
            }

            return Save();
        }
        public int Delete(T obj)
        {
            _objectSet.Remove(obj);
            return Save();
        }
        public int Save()
        {
            return db.SaveChanges();
        }
        public T Find(Expression<Func<T,bool>>where)//where sorgusundan list değil tek değer çeker
        {
            return _objectSet.FirstOrDefault(where);
        }
    }
}
