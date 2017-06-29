using MyEvernote.DataAccessLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyEvernote.DataAccessLayer.EntityFramework
{
    public class RepositoryBase
    {
        private static DatabaseContext _db;
        private static object _lockSync = new object();

        protected RepositoryBase()
        {
            //DatabaseContext çağırıldığında newlenmemesi için constructor boş yapıldı
        }

        public static DatabaseContext CreateContext()  //sadece _db daha önceden oluşturulmamışsa new ler, hazırda _db varsa o kullanılır
        {

            if  (_db== null )
            {
                lock (_lockSync)
                {
                    if (_db == null)//ilk iften sonra lock lasak ikinci ife gerek kalmaz mı acaba ??????
                    {
                        _db = new DatabaseContext();
                    }
                   
                }
            }
            return _db;

        }
    }
}
