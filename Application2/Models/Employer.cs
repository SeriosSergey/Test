using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Application2.Models
{
    //Класс сотрудник
    public class Employer
    {
        public int Id { get; set; }             //Id
        public string FirstName { get; set; }   //Имя
        public string LastName { get; set; }    //Фамилия
        public string FatherName { get; set; }  //Отчество
        public string Gender { get; set; }      //Пол
        public string Department { get; set; }  //Отдел
        public int Age { get; set; }            //Возраст
    }
}