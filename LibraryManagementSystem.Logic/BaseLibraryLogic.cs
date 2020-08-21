using System;
using LibraryManagementSystem.Data.Models;

namespace LibraryManagementSystem.Logic
{
    public class BaseLibraryLogic
    {
        protected static PagedLogicResult<T> HandleDatabaseCollectionError<T>(Exception ex)
        {
            return new PagedLogicResult<T>
            {
                Data = default,
                Error = new LogicError
                {
                    Message = ex.Message,
                    Stacktrace = ex.StackTrace
                }
            };
        }

        protected static ServiceResult<T> HandleDatabaseError<T>(Exception ex)
        {
            return new ServiceResult<T>
            {
                Data = default,
                Error = new LogicError
                {
                    Message = ex.Message,
                    Stacktrace = ex.StackTrace
                }
            };
        }
    }
}
