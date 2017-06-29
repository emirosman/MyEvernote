using MyEvernote.DataAccessLayer.EntityFramework;
using MyEvernote.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyEvernoteBusinessLayer
{
    public class CategoryManager
    {
        private Repository<Category> repo_cat = new Repository<Category>();
        public List<Category> GetCategories()
        {
            return repo_cat.List();
        } 
        public Category GetCategoryById(int id)
        {
            return repo_cat.Find(x => x.Id == id);
        }

    }
}
