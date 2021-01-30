using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;

namespace ContactListBD
{
    class Program
    {
        static void Main()
        {
        MainMenu:
            Console.Clear();
            Console.WriteLine("МЕНЮ");
            Console.WriteLine("Выберете пункт:\n1.Просмотр контактов.\n2.Добаваить контакт.\n3.Изменить контакт.\n4.Удалить контакт.\n5.Добавить номер в существующий контакт.\n6.Выход.");
            var menuNum = Console.ReadLine();
            switch (menuNum)
            {

                case "1":
                    {
                        ShowList();
                        Console.WriteLine("Нажмите любую клваишу для выхода в главное меню");
                        Console.ReadKey();
                        goto MainMenu;
                    }

                case "2":
                    {
                        AddContact();
                        goto MainMenu;
                    }

                case "3":
                    {
                        ChangeContact();
                        goto MainMenu;
                    }

                case "4":
                    {
                        DeleteContact();
                        goto MainMenu;
                    }

                case "5":
                    {
                        AddNumberToCntact();
                        goto MainMenu;
                    }

                case "6":
                    {
                        return;
                    }
                default:
                    {
                        Console.WriteLine("Вы выбрали отсутствующий пункт. Нажмите любую кнопку для возврата в главное меню");
                        Console.ReadKey();
                        goto MainMenu;
                    }
            }
        }


        /// <summary>
        /// Метод добавления контактов в БД
        /// </summary>
        public static void AddContact()
        {
            Console.WriteLine("Введите Имя:");
            var name = Console.ReadLine();
            Console.WriteLine("Введите Фамилию");
            var lname = Console.ReadLine();
            int Pid;

            using (var context = new ContactListDBContext())
            {
                var Person = new Person(name, lname);
                context.People.Add(Person);
                context.SaveChanges();
                Pid = Person.PersonId;
            };



            Console.WriteLine("Введите Номер телефона");
            var n = Console.ReadLine();
            List<PhoneNumber> arrayNumb = new List<PhoneNumber>();
            arrayNumb.Add(new PhoneNumber(n, Pid));
            bool indic = true;
            while (indic == true)
            {
                Console.WriteLine("Для введения дополнительного номера для этого контакта нажмите - 1.\nДля сохранения уже введенных данных и выхода в главное меню нажмите - 2");
                var ch = Console.ReadLine();
                switch (ch)
                {
                    case "1":
                        {
                            Console.WriteLine("Введите номер:");
                            var nb = Console.ReadLine();
                            arrayNumb.Add(new PhoneNumber(nb, Pid));
                            break;
                        }
                    case "2":
                        {
                            indic = false;
                            break;
                        }
                    default:
                        {
                            Console.WriteLine("Вы ввели неверный пункт.");
                            break;

                        }
                }
            }

            using (var context = new ContactListDBContext())
            {
                context.PhoneNumbers.AddRange(arrayNumb);
                context.SaveChanges();
            };

        }

        /// <summary>
        /// Метод который показывает пользователю контакты записаные в БД.
        /// </summary>
        public static void ShowList()
        {
            using var context = new ContactListDBContext();
            var data = context.People.Include(c => c.PhoneNumbers);
            var Pers = data.Take(data.Count());

            foreach (var Person in Pers)
            {
                Console.Write($"{Person.Name} {Person.LastName} [ID {Person.PersonId}]\t");
                foreach (PhoneNumber numb in Person.PhoneNumbers)
                {
                    Console.Write($"\n№ тел.- {numb.Number}");

                }
                Console.WriteLine();
                Console.WriteLine();
            }
        }

        /// <summary>
        /// Метод который на основании Id человека удаляет его из БД.
        /// </summary>
        public static void DeleteContact()
        {
            Console.WriteLine("Выберете ID контакта который хотите удалить:\nЛибо нажмите 10 для отмены");
            ShowList();
            int ch = Convert.ToInt32(Console.ReadLine());

            if (ch != 10)
            {
                using var context = new ContactListDBContext();
                var data = context.People.Include(c => c.PhoneNumbers);

                var pers = data.Single(x => x.PersonId == ch);
                context.People.Remove(pers);
                context.SaveChanges();
            }
            else { return; }
        }


        /// <summary>
        /// Метод позволяющий изменить существующий контакт в базе данных.
        /// </summary>
        public static void ChangeContact()
        {
            Console.WriteLine("Выберете ID контакта который хотите Изменить:\nЛибо нажмите 10 для отмены");
            ShowList();
            int ch = Convert.ToInt32(Console.ReadLine());
            if (ch != 10)
            {
                using var context = new ContactListDBContext();
                var data = context.People.Include(x => x.PhoneNumbers);
                link2:
                Console.WriteLine("Выберете что вы хотите изменить:\n1.Имя\n2.Фамилию.\n3.Номер телефона");
                var sw = Console.ReadLine();
                switch (sw)
                {
                    case "1":
                        {
                            Console.WriteLine("Введите новое имя:");
                            var name = Console.ReadLine();
                            var pers = data.Single(x => x.PersonId == ch);
                            pers.Name = name;
                            context.SaveChanges();
                            return;
                        }

                    case "2":
                        {
                            Console.WriteLine("Введите новую фамилию:");
                            var Lname = Console.ReadLine();
                            var pers = data.Single(x => x.PersonId == ch);
                            pers.LastName = Lname;
                            context.SaveChanges();
                            return; 
                        }

                    case "3":
                        {
                            Console.WriteLine("Выберете ID телефона который вы хотите заменить");
                            var pers = data.Single(x => x.PersonId == ch);
                            foreach (var Numbs in pers.PhoneNumbers)
                            {
                                Console.WriteLine($"{Numbs.Number}[ID {Numbs.Id}]\t");
                                Console.WriteLine();
                            }
                            int k = Convert.ToInt32(Console.ReadLine());
                            Console.WriteLine("Введите новый телефон:");
                            var pn = Console.ReadLine();
                            var PhoneN = pers.PhoneNumbers.Single(x => x.Id == k);
                            PhoneN.Number = pn;
                            context.SaveChanges();
                            return;
                        }
                    default:
                        {
                            Console.WriteLine("Вы выбрали несуществующий пункт.Нажмите Любую кнопку для повторного ввода");
                            Console.ReadKey();
                            goto link2;
                        }


                }
            }
            else { return; }
        }


        /// <summary>
        /// Добавляет номер к уже существующему контакту
        /// </summary>
        public static void AddNumberToCntact()
        {
            Console.WriteLine("Выберете ID контакта к которому хотите Добавить новый телефон:\nЛибо нажмите 10 для отмены");
            ShowList();
            int ch = Convert.ToInt32(Console.ReadLine());
            
            if (ch != 10)
            {
                Console.WriteLine("Введите номер телефона который хотите добавит к контакту:");
                var numb = Console.ReadLine();
                using var context = new ContactListDBContext();
                var data = context.People.Include(x => x.PhoneNumbers);
                var pers = data.Single(x => x.PersonId == ch);
                PhoneNumber phonenumber = new PhoneNumber(numb, pers.PersonId);
                context.PhoneNumbers.Add(phonenumber);
                context.SaveChanges();
            }
            else { return; }

        }
    }
}

