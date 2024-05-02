using System;
using System.Collections.Generic;

namespace Troonch.Domain.Base.DTOs.Response
{
    public enum ResponseStatus
    {
        Error,
        Success
    }
    public class Person
    {
        private string _name;

        // Property with custom getter and setter
        public string Name
        {
            get
            {
                return _name;
            }
            set
            {
                // Custom logic can be applied here before setting the value
                if (!string.IsNullOrEmpty(value))
                {
                    _name = value;
                }
                else
                {
                    throw new ArgumentException("Name cannot be null or empty.");
                }
            }
        }
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
