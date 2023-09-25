using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Application2.Models
{
    //Класс отпуск
    public class Vacation
    {
        public int Id { get; set; }         //ID
        public DateTime Begin { get; set; } //Дата начала
        public DateTime End { get; set; }   //Дата конца
        public int EmployerId { get; set; } //Id сотрудника

        //Функция проверки на совпадение по времени с другим отпуском.
        public bool Match_with(Vacation other)
        {
            return Begin <= other.End && End >= other.Begin;
        }
    }
}