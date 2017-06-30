using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyEvernote.Entities.ValueObjects
{
    public class UpdateViewModel
    {
        [DisplayName("Id")]
        public string id { get; set; }
        [DisplayName("İsim"), StringLength(25, ErrorMessage = "{0} maksimum {1} karakter olmalı")]
        public string Name { get; set; }
        [DisplayName("Soy İsim"), StringLength(25, ErrorMessage = "{0} maksimum {1} karakter olmalı")]
        public string Surname { get; set; }
        [DisplayName("Şifre"), Required(ErrorMessage = "{0} alanı boş geçilemez"), StringLength(25, ErrorMessage = "{0} maksimum {1} karakter olmalı")]
        public string Password { get; set; }
        [DisplayName("E-posta"), StringLength(25, ErrorMessage = "{0} maksimum {1} karakter olmalı")]
        public string Email { get; set; }
        [DisplayName("Resim"), StringLength(50, ErrorMessage = "Link Çok uzun {0} maksimum {1} karakter olmalı")]
        public string ProfileImageFilename { get; set; }
        
    }
}
