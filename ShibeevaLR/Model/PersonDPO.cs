using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using ShibeevaLR.ViewModel;
using JetBrains.Annotations;

namespace ShibeevaLR.Model
{
    public class PersonDpo : INotifyPropertyChanged
    {
        public int Id { get; set; }
        private string roleName;
        public string RoleName
        {
            get { return roleName; }
            set
            {
                roleName = value;
                OnPropertyChanged("RoleName");
            }
        }
        private string firstName;
        public string FirstName
        {
            get { return firstName; }
            set
            {
                firstName = value;
                OnPropertyChanged("FirstName");
            }
        }
        private string lastName;
        public string LastName
        {
            get { return lastName; }
            set
            {
                lastName = value;
                OnPropertyChanged("LastName");
            }
        }
        private string birthday;
        public string Birthday
        {
            get { return birthday; }
            set
            {
                birthday = value;
                OnPropertyChanged("Birthday");
            }
        }


        public PersonDpo() { }
        public PersonDpo(
            int id,
            string roleName,
            string firstName,
            string lastName,
            string birthday)
        {
            this.Id = id; this.RoleName = roleName;
            this.FirstName = firstName; this.LastName = lastName; this.Birthday = birthday;
        }

        public PersonDpo ShallowCopy()
        {
            return (PersonDpo)this.MemberwiseClone();
        }
        /// <summary>
        /// копирование данных из класса Person
        /// </summary>
        /// <param name="person"></param>
        /// <returns></returns>
        public PersonDpo CopyFromPerson(Person person)
        {
            PersonDpo perDpo = new PersonDpo();
            RoleViewModel vmRole = new RoleViewModel();
            string role = string.Empty;
            foreach (var r in vmRole.ListRole)
            {
                if (r.Id == person.RoleId)
                {
                    role = r.NameRole;
                    break;
                }
            }
            if (role != string.Empty)
            {
                perDpo.Id = person.Id;
                perDpo.RoleName = role;
                perDpo.FirstName = person.FirstName;
                perDpo.LastName = person.LastName;
                perDpo.Birthday = person.Birthday;
            }
            return perDpo;
        }
        public static string GetStringBirthday(string birthday)
        {
            if (string.IsNullOrWhiteSpace(birthday))
                return DateTime.Now.ToString("dd.MM.yyyy");
            if (DateTime.TryParse(birthday, out DateTime parsedDate))
                return parsedDate.ToString("dd.MM.yyyy");
            return birthday;
        }
        public event PropertyChangedEventHandler PropertyChanged;
        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}