using BookStore.Services.Categories.Contracts;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace BookStore.RestAPI.Controllers
{
    [Route("api/catgeories")]
    [ApiController]
    public class CategoriesController : ControllerBase
    {
        private readonly CategoryService _service;
        public CategoriesController(CategoryService service)
        {
            _service = service;
        }

        [HttpPost]
        public void Add(AddCategoryDto dto)
        {
            _service.Add(dto);
        }

        [HttpGet]
        public IList<GetCategoryDto> GetAll()
        {
            return _service.GetAll();
        }

        [HttpPut]
        public void Update(UpdateCategoryDto dto, [FromRoute]int id)
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
