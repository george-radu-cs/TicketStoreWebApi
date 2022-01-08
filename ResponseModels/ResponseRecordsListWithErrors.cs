using System.Collections.Generic;
using System.Linq;

namespace TicketStore.ResponseModels
{
    public class ResponseRecordsListWithErrors<T>
    {
        public List<T> Records { get; init; }
        public string ErrorMessage { get; init; }
        public string ErrorType { get; init; }

        public ResponseRecordsListWithErrors(List<T> records, string errorMessage, string errorType)
        {
            Records = records;
            ErrorMessage = errorMessage;
            ErrorType = errorType;
        }

        public bool HasRecords => Records?.Any() ?? false;
    }
}