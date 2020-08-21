using LibraryManagementSystem.Data;
using LibraryManagementSystem.Logic.Interfaces;
using LibraryManagementSystem.Data.Models;
using System.Globalization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Data.Entity.Migrations;
using System.Data.Entity;


namespace LibraryManagementSystem.Logic
{
    public class StatusLogic : IStatusLogic
    {

        private readonly LibraryDbContext _libraryDbContext;
        public StatusLogic(LibraryDbContext libraryDbContext)
        {
            _libraryDbContext = libraryDbContext;
        }
        public void Add(Status newStatus)
        {
            _libraryDbContext.Statuses.Add(newStatus);
            _libraryDbContext.SaveChanges();
        }

        public Status Get(int id)
        {
            return _libraryDbContext.Statuses.FirstOrDefault(p => p.Id == id);
        }

        public IEnumerable<Status> GetAll()
        {
            return _libraryDbContext.Statuses;
        }
    }
}
