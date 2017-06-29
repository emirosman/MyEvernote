using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
//Register.cshtml sayfasına model olarak gider ,
//textboxlar değişkenlere çekilir,
//Register Httppost una veriler yollanır 
namespace MyEvernote.Entities.ValueObjects
{
    public class RegisterViewModel
    {
        [DisplayName("Kullanıcı Adı"),
         Required(ErrorMessage = "{0} eskiden buralar hep dutluktu"), 
         StringLength(25, ErrorMessage = "{0} maksimum {1} karakter olmalı")]
        public string username { get; set; }

        [DisplayName("E-posta"),
         Required(ErrorMessage = "{0} alanı boş geçilemez"), 
         StringLength(25, ErrorMessage = "{0} maksimum {1} karakter olmalı"),
         EmailAddress(ErrorMessage ="Geçerli E-posta adresi girin")]
        public string email { get; set; }

        [DisplayName("Şifre"), 
            Required(ErrorMessage = "{0} alanı boş geçilemez"), 
            DataType(DataType.Password), 
            StringLength(25, ErrorMessage = "{0} maksimum {1} karakter olmalı")]
        public string password { get; set; }

        [DisplayName("Şifre Tekrar"),
            Required(ErrorMessage = "{0} alanı boş geçilemez"),
            DataType(DataType.Password),
            StringLength(25, ErrorMessage = "{0} maksimum {1} karakter olmalı"),
            Compare("password",ErrorMessage ="{0} ile {1} uyuşmuyor")]
        public string repassword { get; set; }
    }
}