using System;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace ForumProjectWebAPI.Filters
{
    public class CustomExceptionFilterAttribute : Attribute, IExceptionFilter
    {
        public  void OnException(ExceptionContext context)
        {
            string action = context.ActionDescriptor.DisplayName;
            string callStack = context.Exception.StackTrace;
            string exceptionMessage = context.Exception.Message;
            context.Result = new ContentResult
            {
                Content = $"Calling {action} failed, because: {exceptionMessage}. Callstack: {callStack}.",
                StatusCode = 500
            };
            context.ExceptionHandled = true;
        }
    }
}