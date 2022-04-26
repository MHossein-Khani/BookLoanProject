using BookStore.Entities;
using BookStore.Infrastructure.Application;
using BookStore.Infrastructure.Test;
using BookStore.Persistence.EF;
using BookStore.Persistence.EF.Categories;
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

namespace BookStore.Services.Test.Unit.Categories
{
    public class CategoryServiceTests
    {
        private readonly EFDataContext _dataContext;
        private readonly UnitOfWork _unitOfWork;
        private readonly CategoryService _sut;
        private readonly CategoryRepository _repository;
        
        public CategoryServiceTests()
        {
            _dataContext =
                new EFInMemoryDatabase()
                .CreateDataContext<EFDataContext>();
            _unitOfWork = new EFUnitOfWork(_dataContext);
            _repository = new EFCategoryRepository(_dataContext);
            _sut = new CategoryAppService(_repository,_unitOfWork);
        }

        [Fact]
        public void Add_adds_category_properly()
        {
            AddCategoryDto dto = GenerateAddCategoryDto();

            _sut.Add(dto);

            _dataContext.Categories.Should()
                .Contain(_ => _.Title == dto.Title);
        }

        [Fact]
        public void GetAll_returns_all_categories()
        {
            CreateCategoriesInDataBase();

            var expected = _sut.GetAll();

            expected.Should().HaveCount(3);
            expected.Should().Contain(_ => _.Title == "dummy1");
            expected.Should().Contain(_ => _.Title == "dummy2");
            expected.Should().Contain(_ => _.Title == "dummy3");
        }

        private void CreateCategoriesInDataBase()
        {
            var categories = new List<Category>
            {
                new Category { Title = "dummy1"},
                new Category { Title = "dummy2"},
                new Category { Title = "dummy3"}
            };
            _dataContext.Manipulate(_ =>
            _.Categories.AddRange(categories));
        }

        private static AddCategoryDto GenerateAddCategoryDto()
        {
            return new AddCategoryDto
            {
                Title = "dummy"
            };
        }

        [Fact]
        public void Update_updates_a_category_by_id_properly()
        {
            var category = CategoryFactory.CreateCategory("Dummy");
            _dataContext.Manipulate(_ => _.Add(category));

            var dto = new UpdateCategoryDto
            {
                Title = "Not Dummy"
            };

            _sut.Update(dto, category.Id);

            var expected = _dataContext.Categories.FirstOrDefault(_ => _.Id == category.Id);
            expected.Title.Should().Be(dto.Title);
        }

        [Fact]
        public void Throw_exception_if_id_of_category_does_not_exist()
        {
            var fakeCategoryId = 100;

            var dto = new UpdateCategoryDto
            {
                Title = "Not Dummy"
            };

            Action expected = () => _sut.Update(dto, fakeCategoryId);
            expected.Should().ThrowExactly<CategoryNotFoundException>();
        }

        [Fact]
        public void Delete_deletes_a_category_by_id_properly()
        {
            var category = CategoryFactory.CreateCategory("Dummy");
            _dataContext.Manipulate(_ => _.Add(category));

            _sut.Delete(category.Id);

            _dataContext.Categories.Should().HaveCount(0);
  
        }

        [Fact]
        public void Throw_exception_if_id_of_category_does_not_exist_for_delete()
        {
            var fakeCategoryId = 100;

            var category = CategoryFactory.CreateCategory("Dummy");
            _dataContext.Manipulate(p => p.Add(category));

            Action expected = () => _sut.Delete(fakeCategoryId);
            expected.Should().ThrowExactly<CategoryNotFoundException>();
        }

    }



    
}
