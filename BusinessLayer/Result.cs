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
        public String ErrorMessage { get; }

        private Result(bool IsSuccess, String ErrorMessage)
        {
            this.IsSuccess = IsSuccess;
            this.ErrorMessage = ErrorMessage;
        }

        public static Result Success()
        {
            return new Result(true, "");
        }

        public static Result Failure(String Message)
        {
            return new Result(false, Message);
        }
    }
}
