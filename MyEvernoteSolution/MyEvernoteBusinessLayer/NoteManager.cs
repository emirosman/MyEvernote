﻿using MyEvernote.DataAccessLayer.EntityFramework;
using MyEvernote.Entities;
using MyEvernoteBusinessLayer.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyEvernoteBusinessLayer
{
    public class NoteManager : ManagerBase<Note>
    {
        private Repository<Note> repo_note = new Repository<Note>();

    }
}

