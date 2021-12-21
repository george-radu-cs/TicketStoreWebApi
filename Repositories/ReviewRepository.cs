using System.Linq;
using Microsoft.EntityFrameworkCore;
using TicketStore.Entities;

namespace TicketStore.Repositories
{
    public class ReviewRepository :IReviewRepository
    {
        private readonly TicketStoreContext _db;

        public ReviewRepository(TicketStoreContext db)
        {
            _db = db;
        }
        
        public IQueryable<Review> GetReviewsIQueryable()
        {
            var reviews = _db.Reviews;

            return reviews;
        }

        public IQueryable<Review> GetReviewsWithUserIQueryable()
        {
            var reviews = GetReviewsIQueryable()
                .Include(r => r.User);
            
            return reviews;
        }

        public IQueryable<Review> GetReviewsWithEventIQueryable()
        {
            var reviews = GetReviewsIQueryable()
                .Include(r => r.Event);

            return reviews;
        }

        public IQueryable<Review> GetReviewsWithUserAndEventIQueryable()
        {
            var reviews = GetReviewsWithUserIQueryable()
                .Include(r => r.Event);
            
            return reviews;
        }

        public void Create(Review r)
        {
            _db.Reviews.Add(r);

            _db.SaveChanges();
        }

        public void Update(Review r)
        {
            _db.Reviews.Update(r);

            _db.SaveChanges();
        }

        public void Delete(Review r)
        {
            _db.Reviews.Remove(r);

            _db.SaveChanges();
        }
    }
}