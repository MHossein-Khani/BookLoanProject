using BookStore.Entities;
using BookStore.Persistence.EF;
using BookStore.Services.Books.Contracts;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace BookStore.RestAPI.Controllers
{
    [ApiController]
    [Route("api/books")]
    public class BooksController : ControllerBase
    {
        private readonly BookService _service;
        public BooksController(BookService service)
        {
            _service = service;
        }

        [HttpPost]
        public void Add(AddBookDto dto)
        {
            _service.Add(dto);
        }

        [HttpGet]
        public List<GetBookDto> GetAll()
        {
            return _service.GetAll();
        }

        [HttpPut]
        public void Update(UpdateBookDto dto, [FromRoute] int id)
        {
            _service.Update(dto, id);
        }

        [HttpDelete]
        public void Delete([FromRoute] int id)
        {
            _service.Delete(id);
        }
    }
}
