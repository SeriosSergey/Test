using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;

namespace Application2.Models
{
    //Класс инициализации базы данных и заполнения ее случайными значениями.
    public class DbInitializer: DropCreateDatabaseAlways<Context>
    {
        //Создаем генератор случайных чисел для дальнейшего заполнения базы данных случайнми значениями
        Random random = new Random((int)DateTime.Now.Ticks);
        //Функция запускается при каждом запуске программы и заполняет базу данных новыми значениями.
        protected override void Seed(Context db)
        {
            //Создаем 100 сотрудников
            for (int i = 1; i <= 100; i++)
            {
                //Создаем сотрудника со случайными данными и заносим его в базу данных
                Employer employer = Generate_random_employer(i);
                db.Employers.Add(employer);

               //Создаем 14-тидневный отпуск для сотрудника с началом в случайной дате 2024 года и заносим его в БД
                DateTime random_date = Random_date_in_2024();
                Vacation vacation_14 = new Vacation()
                {
                    Id = (i - 1) * 3 + 1,
                    Begin = random_date,
                    End = random_date + new TimeSpan(14, 0, 0, 0),
                    EmployerId = employer.Id
                };
                db.Vacations.Add(vacation_14);

                //Создаем 7-мидневный отпуск для сотрудника с началом в случайной дате 2024 года, 
                //проверяем не совпадает ли он с 14-тидневным отпуском и заносим его в БД
                Vacation vacation_7_1;
                while (true)
                {
                    random_date = Random_date_in_2024();
                    vacation_7_1 = new Vacation()
                    {
                        Id = (i - 1) * 3 + 1,
                        Begin = random_date,
                        End = random_date + new TimeSpan(7, 0, 0, 0),
                        EmployerId = employer.Id
                    };
                    if (!vacation_14.Match_with(vacation_7_1))
                    {
                        db.Vacations.Add(vacation_7_1);
                        break;
                    }
                }

                //Создаем второй 7-мидневный отпуск для сотрудника с началом в случайной дате 2024 года, 
                //проверяем не совпадает ли он с 14-тидневным отпуском и первым 7-мидневным отпуском и заносим его в БД
                Vacation vacation_7_2;
                while (true)
                {
                    random_date = Random_date_in_2024();
                    vacation_7_2 = new Vacation()
                    {
                        Id = (i - 1) * 3 + 1,
                        Begin = random_date,
                        End = random_date + new TimeSpan(7, 0, 0, 0),
                        EmployerId = employer.Id
                    };
                    if (!vacation_14.Match_with(vacation_7_2) && !vacation_7_1.Match_with(vacation_7_2))
                    {
                        db.Vacations.Add(vacation_7_2);
                        break;
                    }
                }
            }
            base.Seed(db);
            System.Threading.Thread.Sleep(500);
        }

        //Функция возвращает случайную дату в 2024 году
        private DateTime Random_date_in_2024()
        {
            DateTime start = new DateTime(2024, 1, 1);
            int range = 366;
            return start.AddDays(random.Next(range));
        }

        //Список отделов
        List<string> Department_list = new List<string>() { "Первый отдел",
                                                                "Второй отдел",
                                                                "Третий отдел",
                                                                "Четвертый отдел",
                                                                "Пятый отдел",
                                                                "Шестой отдел",
                                                                "Седьмой отдел",
                                                                "Восьмой отдел",
                                                                "Девятый отдел",
                                                                "Десятый отдел" };

        //Список случайных фамилий
        List<string> Random_second_names_list = new List<string>() {
                                                                    "Иванов",
                                                                    "Васильев",
                                                                    "Петров",
                                                                    "Смирнов",
                                                                    "Михайлов",
                                                                    "Фёдоров",
                                                                    "Соколов",
                                                                    "Яковлев",
                                                                    "Попов",
                                                                    "Андреев",
                                                                    "Алексеев",
                                                                    "Александров",
                                                                    "Лебедев",
                                                                    "Григорьев",
                                                                    "Степанов",
                                                                    "Семёнов",
                                                                    "Павлов",
                                                                    "Богданов",
                                                                    "Николаев",
                                                                    "Дмитриев",
                                                                    "Егоров",
                                                                    "Волков",
                                                                    "Кузнецов",
                                                                    "Никитин",
                                                                    "Соловьёв",
                                                                    "Тимофеев",
                                                                    "Орлов",
                                                                    "Афанасьев",
                                                                    "Филиппов",
                                                                    "Сергеев",
                                                                    "Захаров",
                                                                    "Матвеев",
                                                                    "Виноградов",
                                                                    "Кузьмин",
                                                                    "Максимов",
                                                                    "Козлов",
                                                                    "Ильин",
                                                                    "Герасимов",
                                                                    "Марков",
                                                                    "Новиков",
                                                                    "Морозов",
                                                                    "Романов",
                                                                    "Осипов",
                                                                    "Макаров",
                                                                    "Зайцев",
                                                                    "Беляев",
                                                                    "Гаврилов",
                                                                    "Антонов",
                                                                    "Ефимов",
                                                                    "Леонтьев",
                                                                    "Давыдов",
                                                                    "Гусев",
                                                                    "Данилов",
                                                                    "Киселёв",
                                                                    "Сорокин",
                                                                    "Тихомиров",
                                                                    "Крылов",
                                                                    "Никифоров",
                                                                    "Кондратьев",
                                                                    "Кудрявцев",
                                                                    "Борисов",
                                                                    "Жуков",
                                                                    "Воробьёв",
                                                                    "Щербаков",
                                                                    "Поляков",
                                                                    "Савельев",
                                                                    "Трофимов",
                                                                    "Чистяков",
                                                                    "Баранов",
                                                                    "Сидоров",
                                                                    "Соболев",
                                                                    "Карпов",
                                                                    "Белов",
                                                                    "Титов",
                                                                    "Львов",
                                                                    "Фролов",
                                                                    "Игнатьев",
                                                                    "Комаров",
                                                                    "Прокофьев",
                                                                    "Быков",
                                                                    "Абрамов",
                                                                    "Голубев",
                                                                    "Пономарёв",
                                                                    "Мартынов",
                                                                    "Кириллов",
                                                                    "Миронов",
                                                                    "Фомин",
                                                                    "Власов",
                                                                    "Федотов",
                                                                    "Назаров",
                                                                    "Ушаков",
                                                                    "Денисов",
                                                                    "Константинов",
                                                                    "Воронин",
                                                                    "Наумов"
            };

        //Список случайных мужских имен
        List<string> Random_mans_first_names_list = new List<string>()
                {
                    "Александр",
                    "Дмитрий",
                    "Максим",
                    "Сергей",
                    "Андрей",
                    "Алексей",
                    "Артём",
                    "Илья",
                    "Кирилл",
                    "Михаил",
                    "Никита",
                    "Матвей",
                    "Роман",
                    "Егор",
                    "Арсений",
                    "Иван",
                    "Денис",
                    "Евгений",
                    "Даниил",
                    "Тимофей",
                    "Владислав",
                    "Игорь",
                    "Владимир",
                    "Павел",
                    "Руслан",
                    "Марк",
                    "Константин",
                    "Тимур",
                    "Олег",
                    "Ярослав"
                };

        //Список случайных мужских отчеств
        List<string> Random_males_father_names_list = new List<string>()
                {
                    "Александрович",
                    "Дмитриевич",
                    "Максимович",
                    "Сергеевич",
                    "Андреевич",
                    "Алексеевич",
                    "Артёмович",
                    "Ильич",
                    "Кириллович",
                    "Михайлович",
                    "Никитич",
                    "Матвеевич",
                    "Романович",
                    "Егорович",
                    "Арсеньевич",
                    "Иванович",
                    "Денисович",
                    "Евгеньевич",
                    "Даниилович",
                    "Тимофеевич",
                    "Владиславович",
                    "Игоревич",
                    "Владимирович",
                    "Павлович",
                    "Русланович",
                    "Маркович",
                    "Константинович",
                    "Тимурович",
                    "Олегович",
                    "Ярославович"
                };

        //Список случайных женских имен
        List<string> Random_femails_first_names_list = new List<string>()
                {
                    "Анастасия",
                    "Анна",
                    "Мария",
                    "Елена",
                    "Дарья",
                    "Алина",
                    "Ирина",
                    "Екатерина",
                    "Арина",
                    "Полина",
                    "Ольга",
                    "Юлия",
                    "Татьяна",
                    "Наталья",
                    "Виктория",
                    "Елизавета",
                    "Ксения",
                    "Милана",
                    "Вероника",
                    "Алиса",
                    "Валерия",
                    "Александра",
                    "Ульяна",
                    "Кристина",
                    "София",
                    "Марина",
                    "Светлана",
                    "Варвара",
                    "Софья",
                    "Диана"
                };

        //Список случайных женских отчеств
        List<string> Random_femails_father_names_list = new List<string>()
                {
                    "Александровна",
                    "Дмитриевна",
                    "Максимовна",
                    "Сергеевна",
                    "Андреевна",
                    "Алексеевна",
                    "Артёмовна",
                    "Ильинична",
                    "Кирилловна",
                    "Михайловна",
                    "Никитична",
                    "Матвеевна",
                    "Романовна",
                    "Егоровна",
                    "Арсеньевна",
                    "Ивановна",
                    "Денисовна",
                    "Евгеньевна",
                    "Данииловна",
                    "Тимофеевна",
                    "Владиславовна",
                    "Игоревна",
                    "Владимировна",
                    "Павловна",
                    "Руслановна",
                    "Марковна",
                    "Константиновна",
                    "Тимуровна",
                    "Олеговна",
                    "Ярославовна"
                };

        //Функция создания сотрудника со случайными данными.
        private Employer Generate_random_employer(int id)
        {
            string second_name, first_name, father_name;

            //Является ли он мужчиной?
            bool is_a_man = (random.Next(0, 2) == 1);
            //Выбираем фамилию. Добавляем окончание "а", если сотрудник-женщина
            second_name = Random_second_names_list[random.Next(0, Random_second_names_list.Count - 1)] + (!is_a_man ? "а" : "");

            //Выбираем имя и отчество в зависимости от того, мужчина или женщина
            if (is_a_man)
            {
                first_name = Random_mans_first_names_list[random.Next(0, Random_mans_first_names_list.Count - 1)];
                father_name = Random_males_father_names_list[random.Next(0, Random_males_father_names_list.Count - 1)];
            }
            else
            {
                first_name = Random_femails_first_names_list[random.Next(0, Random_femails_first_names_list.Count - 1)];
                father_name = Random_femails_father_names_list[random.Next(0, Random_femails_father_names_list.Count - 1)];
            }

            //Выбираем отдел и возраст от 18 до 70 лет
            string отдел = Department_list[random.Next(0, Department_list.Count - 1)];
            int возраст = random.Next(18, 70);

            //Создаем сотрудника, заполняем данные, и возвращаем его
            Employer employer = new Employer()
            {
                Id =id,
                FirstName=first_name,
                LastName=second_name,
                FatherName=father_name,
                Age=возраст,
                Gender=is_a_man?"Мужчина":"Женщина",
                Department=отдел
            };
            
            return employer;
        }
    }
}