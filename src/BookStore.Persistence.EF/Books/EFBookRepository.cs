using BookStore.Entities;
using BookStore.Services.Books.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookStore.Persistence.EF.Books
{
    public class EFBookRepository : BookRepository
    {

        private readonly EFDataContext _dataContext;

        public EFBookRepository(EFDataContext dataContext)
        {
            _dataContext = dataContext;
        }

        public void Add(Book book)
        {
            _dataContext.Add(book);
        }

        public void Delete(int id)
        {
            var book = _dataContext.Books.Find(id);
            _dataContext.Books.Remove(book);
        }

        public Book FindById(int id)
        {
            return _dataContext.Books.Find(id);
        }

        public List<GetBookDto> GetAll()
        {
           return _dataContext.Books.
                Select(p => new GetBookDto
                {
                    Id = p.Id,
                    Title = p.Title,
                    Pages = p.Pages,
                    Description = p.Description,
                    Author = p.Author,
                }).ToList();
        }

        public bool IsCategoryExist(int categoryId)
        {
            return _dataContext.Categories.Any(p => p.Id == categoryId);
        }
    }
}
