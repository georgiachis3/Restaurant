using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Web.Data;
using Web.Models;

namespace Web.Services
{
    public class GenericService<T> where T : Identifiable
    {
        private BookingContext context;

        public GenericService(BookingContext context)
        {
            this.context = context;
        }

        protected virtual void Add(T entity)
        {
            context.Set<T>().Add(entity);
            context.SaveChanges();
        }
        protected virtual IEnumerable<T> GetAll()
        {
            return context.Set<T>().ToList();
        }

        protected virtual T ViewTable(int Id)
        {
            return context.Set<T>().SingleOrDefault(x => x.Id == Id);
        }

        protected virtual void DeleteTable(int Id)
        {
            var deletedTable = ViewTable(Id);
            context.Set<T>().Remove(deletedTable);
            context.SaveChanges();
        }

    }


    public class TableService : GenericService<Table>
    {
        public TableService(BookingContext context) : base(context)
        {
        }

        protected override void Add(Table entity)
        {
            base.Add(entity);
        }
    }
}
