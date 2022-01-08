namespace TicketStore.ResponseModels
{
    public class ResponseRecordWithErrors<T>
    {
        public T Record { get; init; }
        public string ErrorMessage { get; init; }
        public string ErrorType { get; init; }

        public ResponseRecordWithErrors(T record, string errorMessage, string errorType)
        {
            Record = record;
            ErrorMessage = errorMessage;
            ErrorType = errorType;
        }
    }
}