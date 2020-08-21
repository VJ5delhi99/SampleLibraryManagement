using LibraryManagementSystem.Model;

namespace LibraryManagementSystem.Data.Models
{
    public class ServiceModel
    {
        /// <summary>
        /// 
        /// </summary>
        public class ErrorableResult
        {
            public LogicError Error { get; set; }
        }

        /// <summary>
        /// Serializes data for all logic-level responses
        /// that use non-paginated data.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        public class ServiceResult<T> : ErrorableResult
        {
            public T Data { get; set; }
        }

        /// <summary>
        /// Serializes data for all logic-level responses
        /// that use paginated data.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        public class PagedResult<T> : ErrorableResult
        {
            public PaginationResult<T> Data { get; set; }
        }
    }
}
