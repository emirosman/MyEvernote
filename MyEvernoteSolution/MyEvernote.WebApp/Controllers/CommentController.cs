using MyEvernote.Entities;
using MyEvernoteBusinessLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MyEvernote.WebApp.Controllers
{
    public class CommentController : Controller
    {
        // GET: Comment
        NoteManager nm = new NoteManager();
        public ActionResult ShowNoteComment(int? id)//gelen id note id si bu note  içindeki commentleri çekicez
        {
            Note note = nm.Find(x => x.Id == id);
            if (note == null)
                return HttpNotFound();

            return PartialView("_PartialComments",note.Comments);
        }
    }
}