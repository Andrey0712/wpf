using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UserBudzet.Application
{
   
        public class UserVM : INotifyPropertyChanged
        {
            private int _id;
            private string _name;
            private DateTime _tranіaction;
            private string _details;
            private string _image;
            private decimal _price;

        public int Id
            {
                get { return _id; }
                set
                {
                    _id = value;
                    this.NotifyPropertyChanged("Id");
                }
            }

            public string Name
            {
                get { return _name; }
                set
                {
                    _name = value;
                    this.NotifyPropertyChanged("Name");
                }
            }


            public DateTime Tranіaction
        {
                get { return _tranіaction; }
                set
                {
                _tranіaction = value;
                    this.NotifyPropertyChanged("Tranіaction");
                }
            }

            public string Details
            {
                get { return _details; }
                set
                {
                    _details = value;
                    NotifyPropertyChanged("Details");
                }
            }

            public string Image
            {
                get { return _image; }
                set
                {
                    _image = value;
                    this.NotifyPropertyChanged("Image");
                }
            }
        public decimal Price
        {
            get { return _price; }
            set
            {
                _price = value;
                this.NotifyPropertyChanged("Price");
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

            public void NotifyPropertyChanged(string propName)
            {
                //if (this.PropertyChanged != null)
                //    this.PropertyChanged(this, new PropertyChangedEventArgs(propName));

                this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propName));
            }

        }
    
}
