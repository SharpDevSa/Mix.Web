using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web.Mvc;
using System.Web.Security;
using DataAnnotationsExtensions;

namespace DotOrg.Web.Ozi.ViewModels.Accounts
{
    public class CreateModel
    {
        [Required(ErrorMessage = "Укажите имя пользователя")]
        public string UserName { get; set; }

        [Required(ErrorMessage = "Укажите адрес эл. почты")]
        [Email(ErrorMessage = "Проверьте правильность адреса эл. почты")]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }

        public string[] UserRoles { get; set; }

        private List<SelectListItem> _rolesSelectList;

        public List<SelectListItem> RolesSelectList
        {
            get
            {

                if (_rolesSelectList == null)
                {
                    _rolesSelectList = new List<SelectListItem>();

                    foreach (string role in Roles.GetAllRoles().Where(r => r != "SuperAdmin"))
                    {
                        _rolesSelectList.Add(new SelectListItem
                                                 {
                                                     Text = role,
                                                     Value = role,
                                                     Selected =
                                                         ((IList<string>)
                                                          Roles.GetRolesForUser(UserName ?? "")).Contains(role)
                                                 });
                    }
                }
                return _rolesSelectList;
            }
        }

        [Required(ErrorMessage = "Задайте новый пароль")]
        [DataType(DataType.Password)]
        public string NewPassword { get; set; }

        [Required(ErrorMessage = "Введите подтверждение нового пароля")]
        [DataType(DataType.Password)]
		[System.ComponentModel.DataAnnotations.Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
		public string NewPasswordConfirm { get; set; }

        public Guid ProviderUserKey { get; set; }


    }
}