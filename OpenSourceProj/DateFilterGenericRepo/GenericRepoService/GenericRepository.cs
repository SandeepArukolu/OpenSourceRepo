using Microsoft.EntityFrameworkCore;
using OpenSourceProj.DateFilterGenericRepo.IGenericService;
using OpenSourceProj.DbContextInfo;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore.Infrastructure;


namespace OpenSourceProj.DateFilterGenericRepo.GenericRepoService
{
    public class GenericRepository<T> : IGenericRepository<T> where T : class
    {
        private readonly DbContextFile _context;

        public GenericRepository(DbContextFile context)
        {
            _context = context;
        }

        public IQueryable<T> GetAll()
        {
            return _context.Set<T>();
        }
        //public IQueryable<T> FilterByDate<T>(IQueryable<T> query, DateFilter dateFilter, Expression<Func<T, DateTime>> dateColumn)
        //{
        //    var parameter = dateColumn.Parameters[0];
        //    var property = Expression.Property(parameter, ((MemberExpression)dateColumn.Body).Member.Name);

        //    Expression condition = null;
        //    if (dateFilter.StartDate.HasValue && dateFilter.EndDate.HasValue)
        //    {
        //        var startDateCondition = Expression.GreaterThanOrEqual(property, Expression.Constant(dateFilter.StartDate.Value));
        //        var endDateCondition = Expression.LessThanOrEqual(property, Expression.Constant(dateFilter.EndDate.Value));
        //        condition = Expression.AndAlso(startDateCondition, endDateCondition);
        //    }
        //    else if (dateFilter.StartDate.HasValue)
        //    {
        //        condition = Expression.GreaterThanOrEqual(property, Expression.Constant(dateFilter.StartDate.Value));
        //    }
        //    else if (dateFilter.EndDate.HasValue)
        //    {
        //        condition = Expression.LessThanOrEqual(property, Expression.Constant(dateFilter.EndDate.Value));
        //    }

        //    if (condition != null)
        //    {
        //        var lambda = Expression.Lambda<Func<T, bool>>(condition, parameter);
        //        query = query.Where(lambda);
        //    }

        //    return query;
        //}

        //public IQueryable<T> FilterByDate<T>(IQueryable<T> query, DateFilter dateFilter, Expression<Func<T, DateTime>> dateColumn)
        //{
        //    var parameter = dateColumn.Parameters[0];
        //    var property = Expression.Property(parameter, ((MemberExpression)dateColumn.Body).Member.Name);

        //    Expression condition = null;
        //    if (dateFilter.StartDate.HasValue)
        //    {
        //        condition = Expression.Equal(property, Expression.Constant(dateFilter.StartDate.Value));
        //    }

        //    if (condition != null)
        //    {
        //        var lambda = Expression.Lambda<Func<T, bool>>(condition, parameter);
        //        query = query.Where(lambda);
        //    }

        //    return query;
        //}

        public IQueryable<T> FilterByDate<T>(IQueryable<T> query, DateFilter dateFilter, Expression<Func<T, DateTime>> dateColumn)
        {
            var parameter = dateColumn.Parameters[0];
            var property = Expression.Property(parameter, ((MemberExpression)dateColumn.Body).Member.Name);

            // Extract the date part by converting to string format
            var datePart = Expression.Call(
                property,
                nameof(DateTime.ToString),
                null,
                Expression.Constant("yyyy-MM-dd")
            );

            Expression condition = null;
            if (dateFilter.StartDate.HasValue)
            {
                var startDateOnlyString = dateFilter.StartDate.Value.ToString("yyyy-MM-dd");
                condition = Expression.Equal(datePart, Expression.Constant(startDateOnlyString));
            }

            if (condition != null)
            {
                var lambda = Expression.Lambda<Func<T, bool>>(condition, parameter);
                query = query.Where(lambda);
            }

            return query;
        }



    }
}
