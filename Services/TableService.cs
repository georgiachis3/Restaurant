using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Web.Data;
using Web.Models;

namespace Web.Services
{
    public class TableService
    {
        private BookingContext context;

        public TableService(BookingContext context)
        {
            this.context = context;
        }

        public void AddTables(Table table)
        {
            context.Tables.Add(table);
            context.SaveChanges();
        }
        public IEnumerable<Table> GetAll()
        {
            return context.Tables.ToList();
        }

        public Table ViewTable(int Id)
        {
            return context.Tables.SingleOrDefault(x => x.Id == Id);
        }
    }
}
