using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Application2.Models
{
    //Класс, совмещающий информацию о отпуске и сотруднике
    public class VacEmp
    {
        public int Id { get; set; }             //Id отпуска
        public DateTime Begin { get; set; }     //Начало
        public DateTime End { get; set; }       //Конец
        public string LastName { get; set; }    //Фамилия сотрудника
        public string Department { get; set; }  //Отдел сотрудника
        public int Age { get; set; }            //Возраст сотрудника
    }
}