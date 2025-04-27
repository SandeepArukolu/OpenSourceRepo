using System.Linq.Expressions;

namespace OpenSourceProj.DateFilterGenericRepo.IGenericService
{
    public interface IGenericRepository<T> where T : class
    {
        IQueryable<T> GetAll();
        IQueryable<T> FilterByDate<T>(IQueryable<T> query, DateFilter dateFilter, Expression<Func<T, DateTime>> dateColumn);
    }
}
