using System.Collections.Generic; 
using System.Linq;
using System.Data.Entity;
using FluentAssertions;
using LibraryManagementSystem.Data;
using LibraryManagementSystem.Data.Models;
using LibraryManagementSystem.Logic; 
using Moq; 
using Microsoft.VisualStudio.TestTools.UnitTesting;
using AutoMapper;
using LibraryManagementSystem.Model.DTOs;

namespace LibraryManagementSystem.Tests.Logic
{
    [TestClass]
    public class BookLogicShould
    {
        [TestMethod]
        public void Add_New_Book_To_Context()
        {
            var mockSet = new Mock<DbSet<Book>>();
            var mockImapper = new Mock<IMapper>();
            var mockCtx = new Mock<LibraryDbContext>();

            mockCtx.Setup(c => c.Books).Returns(mockSet.Object);
            var sut = new BookLogic(mockCtx.Object, mockImapper.Object);

           var result = sut.Add(new Book());

            mockCtx.Verify(s => s.Books.Add(It.IsAny<Book>()), Times.Once());
            mockCtx.Verify(c => c.SaveChangesAsync(), Times.Once());
        }

        [TestMethod]
        public void Test_Get_Given_Null_Book_Returns_LogicResult_With_Null_Data()
        {
            var mockSet = new Mock<DbSet<Book>>();
            var mockMapper = new Mock<IMapper>();

            // SetupData from package

            var books = new List<Book> {
                new Book {
                    Title = "The Waves",
                    Id = 1234
                },
                new Book {
                    Title = "The Snow Leopard",
                    Id = -6
                }
            }.AsQueryable();

            mockSet.As<IQueryable<Book>>()
                .Setup(b => b.Provider)
                .Returns(books.Provider);

            mockSet.As<IQueryable<Book>>()
                .Setup(b => b.Expression)
                .Returns(books.Expression);

            mockSet.As<IQueryable<Book>>()
                .Setup(b => b.ElementType)
                .Returns(books.ElementType);

            mockSet.As<IQueryable<Book>>()
                .Setup(b => b.GetEnumerator())
                .Returns(books.GetEnumerator);

            var mockCtx = new Mock<LibraryDbContext>();

            mockCtx
                .Setup(c => c.Books)
                .Returns(mockSet.Object);

            var sut = new BookLogic(
                mockCtx.Object,
                mockMapper.Object);

            var book = sut.Get(23);
            book.Should().BeEquivalentTo(new ServiceResult<BookDto>
            {
                Data = null,
                Error = null
            }); 
        }
         

        [TestMethod]
        public void Get_Book_By_Author_Partial_Match()
        {
            var books = new List<Book>
            {
                new Book
                {
                    ISBN = "1234",
                    Title = "Siddhartha"
                },

                new Book
                {
                    ISBN = "1234",
                    Title = "Knulp"
                },

                new Book
                {
                    ISBN = "5678",
                    Title = "The Magic Mountain"
                }
            }.AsQueryable();

            var mockSet = new Mock<DbSet<Book>>();

            mockSet.As<IQueryable<Book>>().Setup(b => b.Provider).Returns(books.Provider);
            mockSet.As<IQueryable<Book>>().Setup(b => b.Expression).Returns(books.Expression);
            mockSet.As<IQueryable<Book>>().Setup(b => b.ElementType).Returns(books.ElementType);
            mockSet.As<IQueryable<Book>>().Setup(b => b.GetEnumerator()).Returns(books.GetEnumerator);
            var mockImapper = new Mock<IMapper>();
            var mockCtx = new Mock<LibraryDbContext>();
            mockCtx.Setup(c => c.Books).Returns(mockSet.Object);

            var sut = new BookLogic(mockCtx.Object, mockImapper.Object);
            var queryResult = sut.GetByISBN("1234").ToList();

            queryResult.Should().AllBeOfType(typeof(Book));
            queryResult.Should().HaveCount(2);
            queryResult.Should().Contain(b => b.Title == "Siddhartha");
            queryResult.Should().Contain(b => b.Title == "Knulp");
        }

        [TestMethod]
        public void Get_Book_By_Isbn()
        {
            var books = new List<Book>
            {
                new Book
                {
                    ISBN = "123a",
                    Title = "Pale Fire"
                },
                new Book
                {
                    ISBN = "zz09",
                    Title = "War and Peace"
                }
            }.AsQueryable();

            var mockSet = new Mock<DbSet<Book>>();
            mockSet.As<IQueryable<Book>>().Setup(b => b.Provider).Returns(books.Provider);
            mockSet.As<IQueryable<Book>>().Setup(b => b.Expression).Returns(books.Expression);
            mockSet.As<IQueryable<Book>>().Setup(b => b.ElementType).Returns(books.ElementType);
            mockSet.As<IQueryable<Book>>().Setup(b => b.GetEnumerator()).Returns(books.GetEnumerator);
            var mockImapper = new Mock<IMapper>();
            var mockCtx = new Mock<LibraryDbContext>();
            mockCtx.Setup(c => c.Books).Returns(mockSet.Object);

            var sut = new BookLogic(mockCtx.Object, mockImapper.Object);
            var queryResult = sut.GetByISBN("123a").ToList();

            queryResult.Should().AllBeOfType(typeof(Book));
            queryResult.Should().HaveCount(1);
            queryResult.Should().Contain(b => b.Title == "Pale Fire");
        }
    }
}