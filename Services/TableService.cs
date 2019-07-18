using System.Collections.Generic;
using System.Linq;
using Web.Data;
using Web.Models;

namespace Web.Services
{
    public class GenericService<T> where T : Identifiable
    {
        protected BookingContext context;

        public GenericService(BookingContext context)
        {
            this.context = context;
        }
        
        public virtual void Add(T entity)
        {
            context.Set<T>().Add(entity);
            context.SaveChanges();
        }
        public virtual IEnumerable<T> GetAll()
        {
            return context.Set<T>().ToList();
        }

        public virtual T Get(int Id)
        {
            return context.Set<T>().SingleOrDefault(x => x.Id == Id);
        }

        public virtual void Delete(int Id)
        {
            var deletedTable = Get(Id);
            context.Set<T>().Remove(deletedTable);
            context.SaveChanges();
        }

    }


    public class TableService : GenericService<Table>
    {
        public TableService(BookingContext context) : base(context)
        {
        }
    }
}
