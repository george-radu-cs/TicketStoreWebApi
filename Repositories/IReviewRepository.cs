using System.Linq;
using TicketStore.Entities;

namespace TicketStore.Repositories
{
    public interface IReviewRepository
    {
        IQueryable<Review> GetReviewsIQueryable();
        IQueryable<Review> GetReviewsWithUserIQueryable();
        IQueryable<Review> GetReviewsWithEventIQueryable();
        IQueryable<Review> GetReviewsWithUserAndEventIQueryable();
        void Create(Review r);
        void Update(Review r);
        void Delete(Review r);
    }
}