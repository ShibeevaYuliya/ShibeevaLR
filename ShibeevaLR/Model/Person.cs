using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ShibeevaLR.ViewModel;

namespace ShibeevaLR.Model
{
    public class Person
    {
        public int Id { get; set; }
        public int RoleId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Birthday { get; set; }
        public Person() { }
        public Person(int id, int roleId, string firstName, string lastName, string birthday)
        {
            this.Id = id; this.RoleId = roleId;
            this.FirstName = firstName; this.LastName = lastName; this.Birthday = birthday;
        }

        public Person CopyFromPersonDPO(PersonDpo personDpo)
        {
            RoleViewModel vmRole = new RoleViewModel();
            int role = 0;
            foreach (var r in vmRole.ListRole)
            {
                if (r.NameRole == personDpo.RoleName)
                {
                    role = r.Id;
                    break;
                }
            }
            if (role != 0)
            {
                this.Id = personDpo.Id;
                this.RoleId = role;
                this.FirstName = personDpo.FirstName;
                this.LastName = personDpo.LastName;
                this.Birthday = personDpo.Birthday;
            }
            return this;
        }

    }
}
