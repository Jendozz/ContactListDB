using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace ContactListBD
{
    public class Person
    {
        [Key]
        public int PersonId { get; set; }

        public string Name { get; set; }

        public string LastName { get; set; }


        public virtual ICollection<PhoneNumber> PhoneNumbers { get; set; }
      
       
        public Person() { }
        public Person (string _name, string _Lastname)
        {
            Name = _name;
            LastName = _Lastname;
        }
    }
}
