using System;
using System.Runtime.Serialization;

namespace BookStore.Services.Books
{
    [Serializable]
    public class TheCategoryIsNotExistException : Exception
    {
       
    }
}