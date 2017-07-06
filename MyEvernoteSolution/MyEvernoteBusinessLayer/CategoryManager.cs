using MyEvernote.DataAccessLayer.EntityFramework;
using MyEvernote.Entities;
using MyEvernoteBusinessLayer.Abstract;
using MyEvernoteBusinessLayer.Result;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyEvernoteBusinessLayer
{
    public class CategoryManager: ManagerBase<Category>
    {
        BusinessLayerResult<Category> result = new BusinessLayerResult<Category>();
        Category cat = new Category();

        public BusinessLayerResult<Category> create_cat(Category create_cat)
        {
            cat = Find(x => x.Title == create_cat.Title);
            if(cat!=null)
            {
                result.Errors.Add("Bu başlık daha önce açılmış");
                return result;
            }
            else
            {
                int i = Insert(create_cat);
                if(i==0)
                {
                    result.Errors.Add("Kategori oluşturulamadı!");
                    return result;
                }
            }
            
            return result;
        }
        public BusinessLayerResult<Category> update_cat(Category update_cat)
        {
            cat = Find(x => x.Title == update_cat.Title && x.Id!=update_cat.Id);
            if (cat != null)
            {
                result.Errors.Add("Bu başlık daha önce açılmış");
                return result;
            }

            cat = Find(x => x.Id == update_cat.Id);
            {
                cat.Title = update_cat.Title;
                cat.Description = update_cat.Description;
            }
            if(Update(cat)==0)
            {
                result.Errors.Add("update de problem var");
                return result;
            }
            return result;
        }

        public BusinessLayerResult<Category> delete_cat(Category delete_cat)
        {
            //result ve cat yukarda tanımlı
            NoteManager nm = new NoteManager();
            BusinessLayerResult<Note> result_n = new BusinessLayerResult<Note>();

            List<Note> delete_notes = nm.List(x => x.Category.Id == delete_cat.Id);
            foreach(Note dn in delete_notes)
            {
                nm.remove_note(dn.Id);//kontrol yapılabilir ??
            }
            if (Delete(delete_cat)==0)
                result.Errors.Add("Kategori silme başarısız");

            return result;
        }
    }

}
