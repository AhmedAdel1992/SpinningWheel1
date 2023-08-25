using FluentValidation.Results;

namespace Common.Exceptions
{
    public class InvalidInvoiceException : Exception
    {
        public InvalidInvoiceException(List<ValidationFailure> errors):
            base(String.Format( "Invalid Invoice with Errors {0}", string.Join(",", errors)))
        {
            
        }

        public InvalidInvoiceException(string error) :
           base(String.Format("Invalid Invoice with Errors {0}", error))
        {

        }
    }
}
