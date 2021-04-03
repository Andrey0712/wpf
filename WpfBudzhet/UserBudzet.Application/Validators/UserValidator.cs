using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using UserBudzet.Application.ViewModels;

namespace UserBudzet.Application.Validators
{
    public class UserValidator : AbstractValidator<UserViewModel>
    {
        public UserValidator()
        {
            RuleFor(user => user.Name)
                .Must(BeAValidName)
                .WithMessage("Не корректно заполнено поле ИМЯ");
                
            RuleFor(user => user.Details)
                .Empty()
                .WithMessage("Поле пустое! Заполните его!");

            RuleFor(user => user.Price)
                .Must(BeValidPrice)
                .WithMessage("Не корректно заполнено поле КОМИССИЯ");



        }

        

        private static bool BeValidPrice(decimal price)
        {
            if (price > 0)
            {

                //var regex = new Regex("^[+-]?[0-9]+(\\.[0-9]{2,4})$");
                var regex = new Regex(@"^[-+]?\d*[0-9](|.\d*[0-9])(|,\d*[0-9])?$");
                return regex.IsMatch(price.ToString());
            }
            return false;
        }
        private static bool BeAValidName(string name)
        {
            if (!string.IsNullOrEmpty(name))
            {
                var regex = new Regex(@"^(?:([А-Яа-я])(?!\1))*$");
                return regex.IsMatch(name);
            }
            return false;
        }
        
    }
}
