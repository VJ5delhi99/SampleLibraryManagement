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
    public class LibraryCardLogic : ILibraryCardLogic
    {

        private readonly LibraryDbContext _libraryDbContext;
        public LibraryCardLogic(LibraryDbContext libraryDbContext)
        {
            _libraryDbContext = libraryDbContext;
        } 

        public void Add(LibraryCard newCard)
        {
            _libraryDbContext.LibraryCards.Add(newCard);
            _libraryDbContext.SaveChanges();
        }

        public LibraryCard Get(int id)
        {
            return _libraryDbContext.LibraryCards.FirstOrDefault(p => p.Id == id);
        }

        public IEnumerable<LibraryCard> GetAll()
        {
            return _libraryDbContext.LibraryCards;
        }
    }
}
