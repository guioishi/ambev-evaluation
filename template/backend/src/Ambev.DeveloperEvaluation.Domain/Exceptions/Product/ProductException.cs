namespace Ambev.DeveloperEvaluation.Domain.Exceptions.Product;

public class ProductException
{
    /// <summary>
    /// Exception thrown when a product cannot be found in the system
    /// </summary>
    public class ProductNotFoundException : DomainException
    {
        /// <summary>
        /// The sale number that was not found
        /// </summary>
        public Guid ProductId { get; }

        /// <summary>
        /// Creates a new instance of ProductNotFoundException
        /// </summary>
        /// <param name="productId">The product number that was not found</param>
        public ProductNotFoundException(Guid productId)
            : base($"Product with ID '{productId}' not found")
        {
            ProductId = productId;
        }
    }
    
    public class ProductOperationException : DomainException
    {
        public ProductOperationException(string message) : base(message)
        {
        }

        public ProductOperationException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}
