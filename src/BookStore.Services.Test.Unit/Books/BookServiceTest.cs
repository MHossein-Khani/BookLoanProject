using BookStore.Entities;
using BookStore.Infrastructure.Application;
using BookStore.Infrastructure.Test;
using BookStore.Persistence.EF;
using BookStore.Persistence.EF.Books;
using BookStore.Persistence.EF.Categories;
using BookStore.Services.Books;
using BookStore.Services.Books.Contracts;
using BookStore.Services.Categories;
using BookStore.Services.Categories.Contracts;
using BookStore.Test.Tool;
using FluentAssertions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace BookStore.Services.Test.Unit.Books
{
    public class BookServiceTest
    {
        private readonly EFDataContext _dataContext;
        private readonly UnitOfWork _unitOfWork;
        private readonly BookService _sut;
        private readonly BookRepository _repository;

        public BookServiceTest()
        {
            _dataContext =
                new EFInMemoryDatabase()
                .CreateDataContext<EFDataContext>();
            _unitOfWork = new EFUnitOfWork(_dataContext);
            _repository = new EFBookRepository(_dataContext);
            _sut = new BookAppService(_repository, _unitOfWork);
        }

        [Fact]
        public void Add_adds_book_with_category_properly()
        {
            var category = CategoryFactory.CreateCategory("Dummy");
            _dataContext.Manipulate(_ => _.Categories.Add(category));

            var dto = new AddBookDto
            {
                Title = "Broken Glass",
                Pages = 124,
                Description = "again provides a view of Africa's urban bottom-dwellers",
                Author = "Alain Mabanckou",
                CategoryId = category.Id,
            };

            _sut.Add(dto);

            var expected = _dataContext.Books.FirstOrDefault();
            expected.Title.Should().Be(dto.Title);
            expected.Pages.Should().Be(dto.Pages);
            expected.Description.Should().Be(dto.Description);
            expected.Author.Should().Be(dto.Author);
            expected.Category.Id.Should().Be(category.Id);
        }

        [Fact]
        public void Throw_Exception_if_category_does_not_exist()
        {
           
            var dto = new AddBookDto
            {
                Title = "Broken Glass",
                Pages = 124,
                Description = "again provides a view of Africa's urban bottom-dwellers",
                Author = "Alain Mabanckou",
                
            };

            Action expected = () => _sut.Add(dto);
            expected.Should().ThrowExactly<TheCategoryIsNotExistException>();
        }

        [Fact]
        public void GetAll_returns_all_books()
        {
            Create_books_in_dataBase();

            var expected = _sut.GetAll();
            expected.Should().HaveCount(2);
            expected.Should().Contain(p => p.Title == "Broken Glass");
            expected.Should().Contain(p => p.Title == "Ulysses");
        }

        [Fact]
        public void Update_updates_a_book_by_id_properly()
        {
            var category = CategoryFactory.CreateCategory("Dummy");
            _dataContext.Manipulate(p => p.Add(category));


            var book = new Book
            {
                Title = "Ulysses",
                Pages = 611,
                Description = "Ulysses is a modernist novel by Irish writer James Joyce.",
                Author = "James Joyce",
                CategoryId = category.Id,
            };
            _dataContext.Manipulate(_ => _.Books.Add(book));

            //Creat_one_book_in_dataBase(category.Id);
            var dto = new UpdateBookDto
            {
                Title = "DumDum",
                Pages = 0,
                Description = "Dororo",
                Author = "Dury",
            };

            _sut.Update(dto, book.Id);

            var expected = _dataContext.Books.FirstOrDefault(p => p.Id == book.Id);
            expected.Title.Should().Be(dto.Title);
            expected.Pages.Should().Be(dto.Pages);
            expected.Description.Should().Be(dto.Description);
            expected.Author.Should().Be(dto.Author);
        }

        [Fact]
        public void Throw_exception_if_Book_does_not_exist()
        {
            var fakeCategoryId = 100;

            var dto = new UpdateBookDto
            {
                Title = "Not Dummy"
            };

            Action expected = () => _sut.Update(dto, fakeCategoryId);
            expected.Should().ThrowExactly<BookNotFoundException>();
        }

        [Fact]
        public void Delete_deletes_a_book_by_id_properly()
        {
            var category = CategoryFactory.CreateCategory("Dummy");
            _dataContext.Manipulate(_ => _.Add(category));

            var book = new Book
            {
                Title = "Ulysses",
                Pages = 611,
                Description = "Ulysses is a modernist novel by Irish writer James Joyce.",
                Author = "James Joyce",
                CategoryId = category.Id,
            };
            _dataContext.Manipulate(_ => _.Books.Add(book));

            _sut.Delete(book.Id);

            _dataContext.Books.Should().HaveCount(0);
        }

        [Fact]
        public void Throw_exception_if_Book_does_not_exist_for_delete()
        {
            var fakeCategoryId = 100;

            var category = CategoryFactory.CreateCategory("Dummy");
            _dataContext.Manipulate(_ => _.Add(category));

            var book = new Book
            {
                Title = "Ulysses",
                Pages = 611,
                Description = "Ulysses is a modernist novel by Irish writer James Joyce.",
                Author = "James Joyce",
                CategoryId = category.Id,
            };
            _dataContext.Manipulate(_ => _.Books.Add(book));

            Action expected = () => _sut.Delete(fakeCategoryId);
            expected.Should().ThrowExactly<BookNotFoundException>();
        }

        private void Create_books_in_dataBase()
        {
            var category = CategoryFactory.CreateCategory("Dummy");
            _dataContext.Manipulate(_ => _.Add(category));

            var books = new List<Book>
            {
                new Book
                {
                    Title = "Broken Glass",
                    Pages = 124,
                    Description = "again provides a view of Africa's urban bottom-dwellers",
                    Author = "Alain Mabanckou",
                    CategoryId = category.Id
                },

                 new Book
                 {
                    Title = "Ulysses",
                    Pages = 611,
                    Description = "Ulysses is a modernist novel by Irish writer James Joyce.",
                    Author = "James Joyce",
                    CategoryId = category.Id
                 }
            };

            _dataContext.Manipulate(_ => _.Books.AddRange(books));
        }

        private Book Creat_one_book_in_dataBase(int categoryId)
        {

            var book = new Book
            {
                Title = "Ulysses",
                Pages = 611,
                Description = "Ulysses is a modernist novel by Irish writer James Joyce.",
                Author = "James Joyce",
                CategoryId = categoryId
            };

            _dataContext.Manipulate(_ => _.Books.Add(book));

            return book;
        }
    }
}
