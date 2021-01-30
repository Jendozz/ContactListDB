using System;
using System.Data.Entity;

namespace ContactListBD
{
    public class ContactListDBContext : DbContext
    {
        public ContactListDBContext() : base("DBConnectionString")
        {

        }

        public DbSet<Person> People { get; set; }

        public DbSet<PhoneNumber> PhoneNumbers { get; set; }
    }
}
