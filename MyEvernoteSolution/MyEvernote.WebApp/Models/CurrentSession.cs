using MyEvernote.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MyEvernote.WebApp.Models
{
    public class CurrentSession
    {
        public static EvernoteUser user
        {
            get {
                if (HttpContext.Current.Session["login"] != null)
                {
                    return HttpContext.Current.Session["login"] as EvernoteUser;
                }
                else
                    return null;
            }
        }
        public static void Set<T> (string key, T obj)
        {
            HttpContext.Current.Session[key] = obj;
        }
        //public static void Get<T>(string key, T obj) burda kaldın 

    }
}