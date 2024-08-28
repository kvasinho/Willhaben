namespace Willhaben.Domain.Exceptions
{
    public class CategoryExceptions : Exception
    {
        public CategoryExceptions(string message) : base(message) { }
    }

    public class InvalidCategoryException : CategoryExceptions
    {
        public InvalidCategoryException(string? category) 
            : base($"Category '{category}' is invalid") { }
    }

}