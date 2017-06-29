using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace MyEvernote.DataAccessLayer.Abstract
{
    public interface IRepository<T>
    {
      List<T> List(Expression<Func<T, bool>> where);//list yerine Iqueryble<T> kullanılabilir faydasını tam olarak anlamadım (repository pattern2 dk:4.30)

      IQueryable<T> ListQueryable();

      List<T> List();

      int Insert(T obj);

      int Update(T obj);

      int Delete(T obj);

      int Save();

      T Find(Expression<Func<T, bool>> where);//where sorgusundan list değil tek değer çeker

    }
}
