using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer
{
    public class Result
    {
        public bool IsSuccess { get; }
        public String Message { get; }

        private Result(bool IsSuccess, String ErrorMessage)
        {
            this.IsSuccess = IsSuccess;
            this.Message = ErrorMessage;
        }

        public static Result Success()
        {
            return new Result(true, "");
        }
        public static Result Success(String Message)
        {
            return new Result(true, Message);
        }

        public static Result Failure(String Message)
        {
            return new Result(false, Message);
        }
    }
}
