using ShibeevaLR.Helper;
using ShibeevaLR.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace ShibeevaLR.ViewModel
{
    public class NewPersonViewModel : INotifyPropertyChanged
    {
        private PersonDpo perDpo;
        private Window window;
        public NewPersonViewModel(PersonDpo perDpo, Window window)
        {
            this.perDpo = perDpo;
            this.window = window;
            Id = perDpo.Id;
            RoleName = perDpo.RoleName;
            FirstName = perDpo.FirstName;
            LastName = perDpo.LastName;
            Birthday = perDpo.Birthday;
            SaveCommand = new RelayCommand(Save);
            CancelCommand = new RelayCommand(Cancel);
            RoleViewModel vmRole = new RoleViewModel();
            Data = vmRole.ListRole;
            SelectedRole = vmRole.ListRole.FirstOrDefault(r=>r.NameRole==perDpo.RoleName);
        }

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
        private DateTime birthday;
        public DateTime Birthday
        {
            get { return birthday; }
            set
            {
                birthday = value;
                OnPropertyChanged("Birthday");
            }
        }

        private Role selectedRole;
        public Role SelectedRole
        {
            get { return selectedRole; }
            set
            {
                selectedRole = value;
                OnPropertyChanged("SelectedRole");
            }
        }
        public ObservableCollection<Role> Data {  get; set; }

        public ICommand SaveCommand { get; }
        public ICommand CancelCommand { get; }

        private void Save(object parameter)
        {
            // Записываем данные обратно в модель
            perDpo.RoleName = SelectedRole.NameRole;
            perDpo.FirstName = FirstName;
            perDpo.LastName = LastName;
            perDpo.Birthday = Birthday;



            // Устанавливаем DialogResult = true и закрываем окно
            window.DialogResult = true;
        }

        private void Cancel(object parameter)
        {
            // Закрываем окно без сохранения
            window.DialogResult = false;
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
