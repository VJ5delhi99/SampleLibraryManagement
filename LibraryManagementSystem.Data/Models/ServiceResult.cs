using LibraryManagementSystem.Model;
namespace LibraryManagementSystem.Data.Models
{
    /// <summary>
    /// 
    /// </summary>
    public class ErrorableServiceResult
    {
        public LogicError Error { get; set; }
    }

    /// <summary>
    /// Serializes data for all service-level responses
    /// that use non-paginated data.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class ServiceResult<T> : ErrorableServiceResult
    {
        public T Data { get; set; }
    }

    /// <summary>
    /// Serializes data for all service-level responses
    /// that use paginated data.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class PagedLogicResult<T> : ErrorableServiceResult
    {
        public PaginationResult<T> Data { get; set; }
    }
}
