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
        internal bool CheckingForTable()
        {
            if (context.Tables.Any())
            {
                return true;
            }
            return false;
        }
        public void PopulateTables()
        {
            //Context.Tables.Add();

            context.Tables.Add(new Table()
            {
                Chairs = 2,
                Location = Location.Inside

            });

            context.Tables.Add(new Table()
            {
                Chairs = 2,
                Location = Location.Inside

            });

            context.Tables.Add(new Table()
            {
                Chairs = 2,
                Location = Location.Roof

            });

            context.Tables.Add(new Table()
            {
                Chairs = 4,
                Location = Location.Inside

            });

            context.Tables.Add(new Table()
            {
                Chairs = 4,
                Location = Location.Inside

            });

            context.Tables.Add(new Table()
            {
                Chairs = 4,
                Location = Location.Outside

            });

            context.Tables.Add(new Table()
            {
                Chairs = 6,
                Location = Location.Inside

            });

            context.Tables.Add(new Table()
            {
                Chairs = 6,
                Location = Location.Inside

            });

            context.Tables.Add(new Table()
            {
                Chairs = 6,
                Location = Location.Outside

            });

            context.SaveChanges();
        }
        public void AddTables(Table table)
        {
            context.Tables.Add(table);
            context.SaveChanges();
        }
    }
}
