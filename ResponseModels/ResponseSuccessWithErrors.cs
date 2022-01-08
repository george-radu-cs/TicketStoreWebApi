namespace TicketStore.ResponseModels
{
    public class ResponseSuccessWithErrors
    {
        public bool Success { get; init; }
        public string ErrorMessage { get; init; }
        public string ErrorType { get; init; }
        
        public ResponseSuccessWithErrors(bool success, string errorMessage, string errorType)
        {
            Success = success;
            ErrorMessage = errorMessage;
            ErrorType = errorType;
        }
    }
}