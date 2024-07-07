using System;
using System.Collections.Generic;

namespace Troonch.Domain.Base.DTOs.Response
{
    public enum ResponseStatus
    {
        Error,
        Success
    }
    public class ValidationError
    {
        private string _field;
        public string Field { 
            get 
            {
                return _field;
            } set
            {
                _field = value.ToLower().Trim();
            }

        }
        public string Message { get; set; }
    }

    public class Error
    {
        public string Message { get; set; } = string.Empty;
        public IEnumerable<ValidationError> ValidationErrors { get; set; } = new List<ValidationError>();
    }
    public class ResponseModel<T> 
    {
        public string Status { get; set; } = ResponseStatus.Success.ToString();
        public T Data { get; set; } = default;
        public Error Error { get; set; } = new Error();
    }
}
