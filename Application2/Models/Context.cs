using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;

namespace Application2.Models
{
    //Класс - контекст данных для моделей сотрудников и отпусков.
    public class Context:DbContext
    {
        public DbSet<Employer> Employers { get; set; }
        public DbSet<Vacation> Vacations { get; set; }
    }
}