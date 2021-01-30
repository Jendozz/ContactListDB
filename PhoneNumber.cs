using System;
using System.Collections.Generic;
using System.Text;

namespace ContactListBD
{
    public class PhoneNumber
    {


        public int Id { get; set; }


        public string Number { get; set; }


        public int PersonId { get; set; }

        public virtual Person Person { get; set; }


        public PhoneNumber() { }
        public PhoneNumber(string _numb, int _pid)
        {
            Number = _numb;
            PersonId = _pid;
        }
    }
}
