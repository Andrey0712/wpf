using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using UserBudzet.Application.Validators;

namespace UserBudzet.Application.ViewModels
{
    public class UserViewModel : INotifyPropertyChanged, IDataErrorInfo
    {
        private readonly UserValidator _userValidator;
        private int _id;
        private string _name;
        private DateTime _tranіaction;
        private string _details;
        private string _image;
        private decimal _price;
        public bool EnableValidation { get; set; }

        public UserViewModel()
        {
            _userValidator = new UserValidator();
        }
        public string Name
        {
            get { return _name; }
            set
            {
                _name = value;
                OnPropertyChanged("Name");
            }
        }

        public DateTime Tranіaction
        {
            get { return _tranіaction; }
            set
            {
                Tranіaction = value;
                OnPropertyChanged("Tranіaction");
            }
        }

        public decimal Price
        {
            get { return _price; }
            set
            {
                _price = value;
                OnPropertyChanged("Price");

            }
        }

        public string Details
        {
            get { return _details; }
            set
            {
                _details = value;
                OnPropertyChanged("Details");

            }
        }
        public string Image
        {
            get { return _image; }
            set
            {
                _image = value;
                OnPropertyChanged("Image");

            }
        }

        public string this[string columnName]
        {
            get
            {
                if (EnableValidation)
                {
                    var firstOrDefault = _userValidator.Validate(this)
                        .Errors.FirstOrDefault(lol => lol.PropertyName == columnName);
                    if (firstOrDefault != null)
                        return _userValidator != null ? firstOrDefault.ErrorMessage : "";
                }
                return "";
            }
        }

        public string Error
        {
            get
            {
                if (_userValidator != null)
                {
                    if (EnableValidation)
                    {
                        var results = _userValidator.Validate(this);
                        if (results != null && results.Errors.Any())
                        {
                            var errors = string.Join(Environment.NewLine, results.Errors.Select(x => x.ErrorMessage).ToArray());
                            return errors;
                        }
                    }
                }
                return string.Empty;
            }
        }

        public bool EnabledValidation { get; set; }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null) handler(this, new PropertyChangedEventArgs(propertyName));
        }


    }
}
