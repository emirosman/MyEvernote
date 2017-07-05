using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyEvernoteBusinessLayer.Result
{
    public class BusinessLayerResult<T> where T:class
    {
        public List<string> Errors { get; set; }//hataları döndürücek liste
        public T Result { get; set; }//hata yoksa eklenicek kişiyi döndürücek liste


        public BusinessLayerResult()//errors null dönerse patlamayalım diye çağırıldığında error için liste new liyo
        {
            Errors = new List<string>();
        }
    }
}
