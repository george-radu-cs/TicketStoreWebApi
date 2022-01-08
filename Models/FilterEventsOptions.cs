namespace TicketStore.Models
{
    public class FilterEventsOptions
    {
        public int Limit { get; init; }
        public int Offset { get; init; }

        public FilterEventsOptions(int limit, int offset)
        {
            Limit = limit;
            Offset = offset;
        }
    }
}