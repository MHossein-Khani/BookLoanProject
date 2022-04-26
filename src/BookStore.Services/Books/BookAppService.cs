using BookStore.Entities;
using BookStore.Infrastructure.Application;
using BookStore.Services.Books.Contracts;
using BookStore.Services.Categories.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookStore.Services.Books
{
    public class BookAppService : BookService
    {
        private readonly BookRepository _repository;
        private readonly UnitOfWork _unitOfWork;
       
        public BookAppService(BookRepository bookRepository,
            UnitOfWork unitOfWork)
        {
            _repository = bookRepository;
            _unitOfWork = unitOfWork;
           
        }

        public void Add(AddBookDto dto)
        {
            var isCategory = _repository.IsCategoryExist(dto.CategoryId);
            if(isCategory == false)
            {
                throw new TheCategoryIsNotExistException();
            }
                

            var book = new Book
            {
                Title = dto.Title,
                Pages = dto.Pages,
                Description = dto.Description,
                Author = dto.Author,
                CategoryId = dto.CategoryId,
            };

            _repository.Add(book);
            _unitOfWork.Commit();
        }

        public Book FindById(int id)
        {
            return _repository.FindById(id);
        }

        public List<GetBookDto> GetAll()
        {
            return _repository.GetAll();

        }

        public void Update(UpdateBookDto dto, int id)
        {
            var book = _repository.FindById(id);
            
            if(book == null)
            {
                throw new BookNotFoundException();
            }

            book.Title = dto.Title;
            book.Pages = dto.Pages;
            book.Description = dto.Description;
            book.Author = dto.Author;

            _unitOfWork.Commit();
        }

        public void Delete(int id)
        {
            if(FindById(id) == null)
            {
                throw new BookNotFoundException();
            }
            _repository.Delete(id);
            _unitOfWork.Commit();
        }
    }
}
