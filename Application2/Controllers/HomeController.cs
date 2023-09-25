using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Application2.Models;

namespace Application2.Controllers
{
    public class HomeController : Controller
    {
        //Создаем контекст данных для обращения к БД
        Context db = new Models.Context();

        //Функция по умолчанию. Отобажает список сотрудников
        public ActionResult Index()
        {
            IEnumerable<Employer> employers = db.Employers;
            ViewBag.Employers = employers;
            return View();
        }

        //Функция отображения отпусков сотрудника. В функцию передается id сотрудника. 
        //В представление передаются данные сотрудника и информация о его отпусках
        public ActionResult AddVacation(int id = 1)
        {
            //Ищем сотрудника по Id и передаем в представление
            Employer employer = db.Employers.Where(e => e.Id == id).First();
            ViewBag.employer = employer;

            //Ищем информацию об отпусках сотрудника и сортируем их по времени начала.
            var vacations = from vac in db.Vacations
                            where vac.EmployerId == id
                            orderby vac.Begin
                            select vac;
            ViewBag.Vacations = vacations;
            return View();
        }

        //Функция добавления отпуска определенному сотруднику.
        //В функцию передается id сотрудника, дата начала отпуска и количество дней в нем.
        [HttpGet]
        public ActionResult AddingVacation(DateTime BeginDate=new DateTime(), int PersonId = 0, int days = 0 )
        {
            //Ищем по id сотрудника
            Employer employer = db.Employers.Where(e => e.Id == PersonId).First();
            //Ищем последний id отпуска в списке
            IEnumerable<Vacation> vacations = db.Vacations.OrderBy(vac => vac.Id);
            int last_id = vacations.Last().Id;
            //Создаем новый отпуск для данного сотрудника заданной продолжительности с началом в заданной дате.
            Vacation vacation = new Vacation()
            {
                Id = last_id+1,
                Begin = BeginDate,
                End = BeginDate + new TimeSpan(days, 0, 0, 0),
                EmployerId = PersonId
            };

            //Ищем есть ли пересечения с отпусками данного сотрудника.
            bool is_crossing = vacations.Where(vac => vac.EmployerId == PersonId && vac.Match_with(vacation)).Any();
            
            //Если есть - выдаем предупреждение.
            if (is_crossing)
            {
                ViewBag.message = "У сотрудника уже есть отпуск в данный период";
            }
            //Иначе - сохраняем вакансию.
            else
            {
                db.Vacations.Add(vacation);
                db.SaveChanges();
                ViewBag.message = "Отпуск добавлен";
            }
            ViewBag.employer = employer;

            return View();
        }

        //Отображение пересечений отпуска с другими отпусками. 
        //В функцию передается id отпуска и номер типа пересечения
        public ActionResult Crossing(int id=1,int type=1)
        {
            //Ищем отпуск и сотрудника в БД
            Vacation vacation = db.Vacations.Where(v => v.Id == id).First();
            Employer employer = db.Employers.Where(e => e.Id == vacation.EmployerId).First();
            //Определяем тип пересечения
            switch (type)
            {
                case 1:
                    ViewBag.type = "Пересечение этого отпуска с отпусками сотрудников отдела этого сотрудника моложе 30 лет";
                    //Ищем отпуска
                    var vacations = from vac in db.Vacations                                    //в базе данных
                                    join emp in db.Employers on vac.EmployerId equals emp.Id    //Присоеденяем информацию о сотрудниках
                                    where emp.Id != employer.Id                                 //но не о сотруднике, к которому относится отпуск
                                    && vac.Id!=id                                               //и не о текущем отпуске
                                    && emp.Age<30                                               //возраст сотрудников до 30 лет
                                    && emp.Department==employer.Department                      //отдел тот же что и у сотрудника, к которому относится отпуск
                                    && (vac.Begin <= vacation.End && vac.End >= vacation.Begin) //отпуска пересекаются
                                    select new VacEmp {Id=vac.Id,Begin=vac.Begin,End=vac.End,LastName=emp.LastName,Age=emp.Age,Department = emp.Department};
                    
                    ViewBag.Vacations = vacations;
                    return View();
                case 2:
                    ViewBag.type = "Пересечение этого отпуска с отпусками сотрудников женщин не из отдела этого сотрудника старше 30, но моложе 50 лет";
                    vacations = from vac in db.Vacations                                        //в базе данных
                                join emp in db.Employers on vac.EmployerId equals emp.Id        //Присоеденяем информацию о сотрудниках
                                where emp.Id != employer.Id                                     //но не о сотруднике, к которому относится отпуск
                                && vac.Id != id                                                 //и не о текущем отпуске
                                && emp.Gender =="Женщина"                                       //только женщины
                                && (emp.Age > 30 && emp.Age<50)                                 //возрастом от 30 до 50 лет
                                && emp.Department != employer.Department                        //из другого отдела
                                && (vac.Begin <= vacation.End && vac.End >= vacation.Begin)     //отпуска пересекаются
                                select new VacEmp { Id = vac.Id, Begin = vac.Begin, End = vac.End, LastName = emp.LastName, Age = emp.Age, Department = emp.Department };

                    ViewBag.Vacations = vacations;
                    return View();
                case 3:
                    ViewBag.type = "Пересечение этого отпуска с отпусками сотрудников из любого отдела старше 50 лет";
                    vacations = from vac in db.Vacations                                        //в базе данных
                                join emp in db.Employers on vac.EmployerId equals emp.Id        //Присоеденяем информацию о сотрудниках
                                where emp.Id != employer.Id                                     //но не о сотруднике, к которому относится отпуск
                                && vac.Id != id                                                 //и не о текущем отпуске
                                && emp.Age > 50                                                 //возраст сотрудников от 50 лет
                                && (vac.Begin <= vacation.End && vac.End >= vacation.Begin)     //отпуска пересекаются
                                select new VacEmp { Id = vac.Id, Begin = vac.Begin, End = vac.End, LastName = emp.LastName, Age = emp.Age, Department = emp.Department };

                    ViewBag.Vacations = vacations;
                    return View();
                //Поиск непересекающихся отпусков
                case 4:
                    ViewBag.type = "Отпуска без пересечений";
                    //Берем список всех отпусков их БД
                    vacations = from vac in db.Vacations
                                join emp in db.Employers on vac.EmployerId equals emp.Id
                                orderby vac.Id
                                select new VacEmp { Id = vac.Id, Begin = vac.Begin, End = vac.End, LastName = emp.LastName, Age = emp.Age, Department = emp.Department };
                    
                    List<VacEmp> VacList = new List<VacEmp>(vacations);
                    //Список индексов отпусков в списке отпусков, имеющих пересечения.
                    List<int> Remote_indexes_list = new List<int>();
                    //Берем отпуска из списка по порядку
                    for (int i = 0; i < VacList.Count-1;i++)
                    {
                        //Если индекс отпуска есть в списке индексов - идем далее.
                        if (Remote_indexes_list.Contains(i)) continue;
                        //Сравниваем со всеми отпусками, имеющими индекс больше чем у текущего
                        for (int j = i + 1; j < VacList.Count; j++)
                        {
                            //Если индекс второго отпуска есть в списке индексов - идем далее.
                            if (Remote_indexes_list.Contains(j)) continue;
                            //Если отпуска совпадают по времени
                            if (VacList[i].Begin <= VacList[j].End && VacList[i].End >= VacList[j].Begin)
                            {
                                //Если индекс первого отпуска отсутствует в списке индексов
                                if (!Remote_indexes_list.Contains(i))
                                    //Добавляем индекс первого отпуска в список индексов
                                    Remote_indexes_list.Add(i);
                                //Добавляем индекс второго отпуска в список индексов
                                Remote_indexes_list.Add(j);
                            }
                        }
                    }
                    //Сортируем список индексов по порядку
                    Remote_indexes_list=Remote_indexes_list.OrderBy(i => i).ToList();
                    //Удаляем из списка отпусков все отпуска, индекс которых есть в списке индексов. Идем в обратном порядке
                    if (Remote_indexes_list.Count > 0)
                    {
                        for (int i = Remote_indexes_list.Count - 1; i >= 0; i--)
                        {
                            VacList.RemoveAt(Remote_indexes_list[i]);
                        }
                    }
                    ViewBag.Vacations = VacList.OrderBy(x => x.Id);
                    return View();
                    
                default: return View();
            }
        }
    }
}