using TicketStore.Entities;
using TicketStore.Models;
using TicketStore.ResponseModels;

namespace TicketStore.Managers
{
    public interface ITicketManager
    {
        Ticket GetTicketById(string userId, string eventId, string auxiliaryId);

        ResponseRecordWithErrors<TicketResponseModel> GetTicketResponseById(string userId, string eventId,
            string auxiliaryId);

        ResponseRecordsListWithErrors<TicketResponseModel> GetTicketsResponse();
        ResponseRecordsListWithErrors<TicketResponseModel> GetBuyerTicketsResponse(string userId);

        ResponseRecordsListWithErrors<TicketResponseModel> GetBuyerTicketsForAnEventResponse(string userId,
            string eventId);

        ResponseRecordsListWithErrors<TicketResponseModel> GetTicketsSoldByOrganisation(string userId);
        ResponseRecordsListWithErrors<TicketResponseModel> GetEventSoldTicketsResponse(string eventId);
        ResponseSuccessWithErrors Create(TicketModel model);
        ResponseSuccessWithErrors Update(TicketModel model);
        ResponseSuccessWithErrors Delete(string userId, string eventId, string auxiliaryId);
    }
}