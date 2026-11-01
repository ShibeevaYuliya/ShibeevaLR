using JetBrains.Annotations;
using Newtonsoft.Json;
using ShibeevaLR.Helper;
using ShibeevaLR.Model;
using ShibeevaLR.View;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace ShibeevaLR.ViewModel
{
    public class PersonViewModel : INotifyPropertyChanged
    {
        readonly string path = @"C:\Users\Krossovok\source\repos\ShibeevaLR\ShibeevaLR\DataModels\PersonData.json";
        string _jsonPersons = String.Empty; 
        public string Error { get; set; }
        public ObservableCollection<Person> LoadPerson()
        {
            _jsonPersons = File.ReadAllText(path); if (_jsonPersons != null)
            {
                ListPerson = JsonConvert.DeserializeObject<ObservableCollection<Person>>(_jsonPersons);
                return ListPerson;
            }
            else
            {
                return null;
            }
        }
        private void SaveChanges(ObservableCollection<Person> listPersons)
        {
            var jsonPerson = JsonConvert.SerializeObject(listPersons); try
            {
                using (StreamWriter writer = File.CreateText(path))
                {
                    writer.Write(jsonPerson);
                }
            }
            catch (IOException e)
            {
                Error = "Ошибка записи json файла /n" + e.Message;
            }
        }


        private PersonDpo selectedPersonDpo;
        /// <summary>
        /// выделенные в списке данные по сотруднику
        /// </summary>
        public PersonDpo SelectedPersonDpo
        {
            get { return selectedPersonDpo; }
            set
            {
                selectedPersonDpo = value; OnPropertyChanged("SelectedPersonDpo");
            }
        }
        /// <summary>
        /// коллекция данных по сотрудникам
        /// </summary>

        public ObservableCollection<Person> ListPerson { get; set; } = new ObservableCollection<Person>();
        public ObservableCollection<PersonDpo> ListPersonDpo { get; set; } = new ObservableCollection<PersonDpo>();
        public PersonViewModel()
        {
            ListPerson = new ObservableCollection<Person>();
            ListPersonDpo = new ObservableCollection<PersonDpo>();
            ListPerson = LoadPerson();
            ListPersonDpo = GetListPersonDpo();
        }

        public ObservableCollection<PersonDpo> GetListPersonDpo()
        {
            foreach (var person in ListPerson)
            {
                PersonDpo p = new PersonDpo(); p = p.CopyFromPerson(person); ListPersonDpo.Add(p);
            }
            return ListPersonDpo;
        }
        /// <summary>
        /// Нахождение максимального Id в коллекции данных
        /// </summary>
        /// <returns></returns> 
        public int MaxId()
        {
            int max = 0;
            foreach (var r in this.ListPerson)
            {
                if (max < r.Id)
                {
                    max = r.Id;
                }
                ;
            }
            return max;

        }
        /// <summary>
        /// добавление сотрудника
        /// </summary>
        private RelayCommand addPerson;
        /// <summary>
        /// добавление сотрудника
        /// </summary>
        public RelayCommand AddPerson
        {
            get
            {
                return addPerson ??
                (addPerson = new RelayCommand(obj =>
                {
                    WindowNewEmployee wnPerson = new WindowNewEmployee
                    {
                        Title = "Новый сотрудник"
                    };
                    // формирование кода нового сотрудника
                    int maxIdPerson = MaxId() + 1;
                    PersonDpo per = new PersonDpo
                    {
                        Id = maxIdPerson,
                        Birthday = DateTime.Now.ToString(),
                    };
                    wnPerson.DataContext = new NewPersonViewModel(per, wnPerson);
                    if (wnPerson.ShowDialog() == true)
                    {
                        Role r = (Role)wnPerson.CbRole.SelectedValue;
                        per.RoleName = r.NameRole;
                        per.Birthday = PersonDpo.GetStringBirthday(per.Birthday);
                        ListPersonDpo.Add(per);
                        // добавление нового сотрудника в коллекцию ListPerson<Person>
                        Person p = new Person();
                        p = p.CopyFromPersonDPO(per);
                        ListPerson.Add(p);
                        try
                        {
                            // сохранение изменений в файле json
                            SaveChanges(ListPerson);
                        }
                        catch (Exception e)
                        {
                            Error = "Ошибка добавления данных в json файл\n" +
                            e.Message;
                        }

                    }
                },
                (obj) => true));
            }
        }
        /// команда редактирования данных по сотруднику
        private RelayCommand editPerson;
        public RelayCommand EditPerson
        {
            get
            {
                return editPerson ??
                (editPerson = new RelayCommand(obj =>
                {
                    WindowNewEmployee wnPerson = new WindowNewEmployee()
                    {
                        Title = "Редактирование данных сотрудника",
                    };
                    PersonDpo personDpo = SelectedPersonDpo;
                    PersonDpo tempPerson = new PersonDpo();
                    tempPerson = personDpo.ShallowCopy();
                    wnPerson.DataContext = new NewPersonViewModel(tempPerson, wnPerson);

                    if (wnPerson.ShowDialog() == true)
                    {
                        // сохранение данных в оперативной памяти
                        // перенос данных из временного класса в класс отображения данных
                        Role r = (Role)wnPerson.CbRole.SelectedValue;
                        personDpo.RoleName = r.NameRole;
                        personDpo.FirstName = tempPerson.FirstName;
                        personDpo.LastName = tempPerson.LastName;
                        personDpo.Birthday = PersonDpo.GetStringBirthday(tempPerson.Birthday);
                        Person p = ListPerson.FirstOrDefault(x => x.Id == personDpo.Id);
                        p = p.CopyFromPersonDPO(personDpo);
                        SaveChanges(ListPerson);
                    

                }
                }, (obj) => SelectedPersonDpo != null && ListPersonDpo.Count > 0));
            }
        }

        /// команда удаления данных по сотруднику 
        private RelayCommand deletePerson;
        public RelayCommand DeletePerson
        {
            get
            {
                return deletePerson ??
                (deletePerson = new RelayCommand(obj =>
                {
                    PersonDpo person = SelectedPersonDpo;
                    MessageBoxResult result = MessageBox.Show("Удалить данные по сотруднику: \n" + person.LastName + " " + person.FirstName,
                    "Предупреждение", MessageBoxButton.OKCancel, MessageBoxImage.Warning);
                    if (result == MessageBoxResult.OK)
                    {
                        {
                            try
                            {
                                // удаление данных в списке отображения данных
                                ListPersonDpo.Remove(person);
                                // поиск удаляемого класса в коллекции ListRoles
                                var per = ListPerson.FirstOrDefault(p => p.Id == person.Id);

                                if (per != null)
                                {
                                    ListPerson.Remove(per);
                                    // сохраненее данных в файле json
                                    SaveChanges(ListPerson);
                                }
                            }
                            catch (Exception e)
                            {
                                Error = "Ошибка удаления данных\n" + e.Message;
                            }

                        }
                    }
                    }, (obj) => SelectedPersonDpo != null && ListPersonDpo.Count > 0));
            }
        }
        public event PropertyChangedEventHandler PropertyChanged;
        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
