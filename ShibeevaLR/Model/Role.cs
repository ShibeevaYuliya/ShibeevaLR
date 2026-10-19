using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using JetBrains.Annotations;

namespace ShibeevaLR.Model
{
    public class Role : INotifyPropertyChanged
    {
        public int Id { get; set; }
        private string nameRole;
        public string NameRole
        {
            get { return nameRole; }
            set
            {
                nameRole = value;
                OnPropertyChanged("NameRole");
            }
        }
        public Role() { }
        public Role(int id, string nameRole)
        {
            this.Id = id; this.NameRole = nameRole;
        }
        /// <summary>
        /// Метод поверхностного копирования
        /// </summary>
        /// <returns></returns> 
        /// 
        public Role ShallowCopy()
        {
            return (Role)this.MemberwiseClone();
        }
        public event PropertyChangedEventHandler PropertyChanged; [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}