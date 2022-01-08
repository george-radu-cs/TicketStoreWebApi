using System;

namespace TicketStore.Services
{
    public interface IDateTimeProvider
    {
        public DateTime DateTimeNow { get; }
    }
}