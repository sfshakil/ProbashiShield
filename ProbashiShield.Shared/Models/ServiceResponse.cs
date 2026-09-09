using System.Collections.Generic;

namespace ProbashiShield.Shared.Models
{
    public class ServiceResponse<T>
    {
        public bool IsSuccess { get; set; }
        public string Message { get; set; }
        public T Data { get; set; }
        public int TotalRecords { get; set; }
        public int DisplayedRecords { get; set; }
    }
}
