using BookStore.Entities;
using BookStore.Services.Categories.Contracts;
using System;

namespace BookStore.Test.Tool
{
    public static class CategoryFactory
    {
        public static Category CreateCategory(string title)
        {
            return new Category
            {
                Title = title
            };
        }
    }
}
