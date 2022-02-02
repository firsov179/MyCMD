using System;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using System.Linq;
namespace CMD
{
    class Program
    {
        /// <summary>
        /// Выбор пользователем файла txt в текущем каталоге.
        /// </summary>
        /// <param name="path"> Путь в текущий каталог.</param>
        /// <returns> Адрес выбранного файла.</returns>
        public static string Choice(string path)
        {
            var allFiles = Directory.GetFiles(path);
            int j = 1;
            string[] files = new string[allFiles.Length];
            for (int i = 0; i < allFiles.Length; ++i)
            {
                if (Regex.IsMatch(allFiles[i], @".*txt$"))
                {
                    Console.WriteLine(j + ") " + allFiles[i]);
                    files[j - 1] = allFiles[i];
                    j++;
                }
            }
            int userNum;
            while (!int.TryParse(Console.ReadLine(), out userNum) || userNum < 1 || userNum > j - 1)
            {
                Console.WriteLine($"Что-то не так. Введите число от 1 до {j - 1}.");
            }
            userNum--;
            return files[userNum];
        }

        /// <summary>
        /// Выбор пользователем  одного из локальных дисков.
        /// </summary>
        /// <returns> Адрес выбранного диска.</returns>
        static public string Drivers()
        {
            Console.WriteLine("Выберите диск. Введите соответствующий ему номер.");
            var disks = Environment.GetLogicalDrives();
            for (int i = 0; i < disks.Length; ++i)
            {
                Console.WriteLine((i + 1) + ") " + disks[i]);
            }
            int userNum;
            while(!int.TryParse(Console.ReadLine(), out userNum) || userNum < 1 || userNum > disks.Length)
            {
                Console.WriteLine($"Что-то не так. Введите число от 1 до {disks.Length}.");
            }
            return disks[userNum - 1];
        }

        /// <summary>
        /// Переход из текущего каталога в подкаталог.
        /// </summary>
        /// <param name="path"> Путь в текущий каталог.</param>
        /// <returns> Адрес выбранного подкаталога.</returns>
        static public string Move(string path)
        {
            Console.WriteLine("Ваш текущий каталог: " + path);
            Console.WriteLine("Выберите подкаталог. Введите соответствующий ему номер.");
            var dirs = Directory.GetDirectories(path);
            if (dirs.Length == 0)
            {
                Console.WriteLine("В текущем каталоге нет подкаталогов.");
                return path;
            }
            for (int i = 0; i < dirs.Length; ++i)
                Console.WriteLine((i + 1) + ") " + dirs[i]);
            int userNum;
            while (!int.TryParse(Console.ReadLine(), out userNum) || userNum < 1 || userNum > dirs.Length)
            {
                Console.WriteLine($"Что-то не так. Введите число от 1 до {dirs.Length}.");
            }
            return dirs[userNum - 1];
        }

        /// <summary>
        /// Вывод в консоль всех файлов в текущем каталоге..
        /// </summary>
        /// <param name="path"> Путь в текущий каталог.</param>
        static public void Files(string path)
        {

            var x = Directory.GetFiles(path);
            foreach (var i in x)
            {
                Console.WriteLine(i);
            }
        }

        /// <summary>
        /// Вывод содержимого текстового файла в консоль в кодировке UTF-8.
        /// </summary>
        /// <param name="path"> Путь в текущий каталог.</param>
        static public void ReadDefault(string path)
        {
            Console.WriteLine("Выберите файл. Введите соответствующий ему номер.");
            using (StreamReader sr = new StreamReader(Choice(path)))
            {
                string line;
                Console.BackgroundColor = ConsoleColor.DarkBlue;
                while ((line = sr.ReadLine()) != null)
                    Console.WriteLine(line);
                Console.BackgroundColor = ConsoleColor.Black;
            }
        }


        /// <summary>
        /// Вывод содержимого текстового файла в консоль в выбранной пользователем кодировке.
        /// </summary>
        /// <param name="path"> Путь в текущий каталог.</param>
        static public void Read(string path)
        {

            var encoding = ChoseEncoding();
            Console.WriteLine("Выберите файл. Введите соответствующий ему номер.");
            
            using (StreamReader sr = new StreamReader(Choice(path), encoding: encoding))
            {
                string line;
                Console.BackgroundColor = ConsoleColor.DarkBlue;
                while ((line = sr.ReadLine()) != null)
                    Console.WriteLine(line);
                Console.BackgroundColor = ConsoleColor.Black;
            }
        }

        /// <summary>
        /// Копирование выбранного файла в текущем каталоге в выбранный каталог.
        /// </summary>
        /// <param name="path"> Путь в текущий каталог.</param>
        static public void Copy(string path)
        {
            Console.WriteLine("Выберите файл для копирования. Введите соответствующий ему номер.");
            var choice = Choice(path);
            Console.WriteLine("Введите имя нового файла. Это должны быть строка состоящая из латинских букв и цифр.");
            Console.WriteLine($"Если файл с таким названием уже есть в этой папке то он будет перезаписан.");
            string inp = Console.ReadLine();

            while (!Regex.IsMatch(inp, @"^[A-Za-z0-9]*$"))
            {
                Console.WriteLine($"Что-то не так. Введите имя нового файла. Это должны быть строка состоящая из латинских букв и цифр.");
                inp = Console.ReadLine();
            }

            using (StreamReader sr = new StreamReader(choice))
            {
                using (StreamWriter sw = new StreamWriter(path + '\\' + inp + ".txt"))
                {
                    string line;
                    while ((line = sr.ReadLine()) != null)
                        sw.WriteLine(line);
                }
            }
        }

        /// <summary>
        /// Перемещения файла из текущего каталога в выбранный каталог.
        /// </summary>
        /// <param name="path"> Путь в текущий каталог.</param>   
        public static void Moving(string path)
        {
            Console.WriteLine("Выберите файл для перемещения. Введите соответствующий ему номер.");
            var choice = Choice(path);
            Console.WriteLine("Введите адресс нового каталога.");
            string newPath = Console.ReadLine();
            while (!Directory.Exists(newPath))
            {
                Console.WriteLine("Что-то не так. Введите адресс нового каталога.");
                newPath = Console.ReadLine();
            }

            Console.WriteLine("Введите имя нового файла. Это должны быть строка состоящая из латинских букв и цифр.");
            Console.WriteLine($"Если файл с таким названием уже есть в этой папке то он будет перезаписан.");
            string inp = Console.ReadLine();

            while (!Regex.IsMatch(inp, @"^[A-Za-z0-9]*$"))
            {
                Console.WriteLine($"Что-то не так. Введите имя нового файла. Это должны быть строка состоящая из латинских букв и цифр.");
                inp = Console.ReadLine();
            }

            using (StreamReader sr = new StreamReader(choice))
            {
                using (StreamWriter sw = new StreamWriter(newPath + '\\' + inp + ".txt"))
                {
                    string line;
                    while ((line = sr.ReadLine()) != null)
                        sw.WriteLine(line);
                }
            }
            File.Delete(choice);
        }

        /// <summary>
        /// Удаление файла из текущего каталога.
        /// </summary>
        /// <param name="path"> Путь в текущий каталог.</param>
        public static void Dell(string path)
        {
            Console.WriteLine("Выберите файл для удаления. Введите соответствующий ему номер.");
            var choice = Choice(path);
            File.Delete(choice);
        }

        /// <summary>
        /// Создание простого текстового файла в кодировке UTF-8.
        /// </summary>
        /// <param name="path"> Путь в текущий каталог.</param>   
        public static void CreateDefault(string path)
        {

            Console.WriteLine("Введите имя нового файла. Это должны быть строка состоящая из латинских букв и цифр.");
            Console.WriteLine($"Если файл с таким названием уже есть в этой папке то он будет перезаписан.");
            string inp = Console.ReadLine();

            while (!Regex.IsMatch(inp, @"^[A-Za-z0-9]*$"))
            {
                Console.WriteLine($"Что-то не так. Введите имя нового файла. Это должны быть строка состоящая из латинских букв и цифр.");
                inp = Console.ReadLine();
            }

            using (StreamWriter sw = new StreamWriter(path + '\\' + inp + ".txt"))
            {
                Console.WriteLine($"Вводите текст файла. Для окончания введите строку: '_END_'.");

                string line;
                while ((line = Console.ReadLine()) != "_END_")
                    sw.WriteLine(line);
            }
        }

        /// <summary>
        /// Выбор пользователем кодировки.
        /// </summary>
        /// <returns> Выбранная кодировка.</returns>
        public static Encoding ChoseEncoding()
        {
            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
            Console.WriteLine("Введите номер крдировки которая вам нужна.");
            Console.WriteLine("1) UTF-8.");
            Console.WriteLine("2) UTF-16.");
            Console.WriteLine("3) Windows-1251.");
            int userNum;
            while(!int.TryParse(Console.ReadLine(), out userNum) || userNum < 1 || userNum > 3)
            {
                Console.WriteLine($"Что-то не так. Введите число от 1 до 3.");
            }
            if (userNum == 1)
                return Encoding.GetEncoding("utf-8");
            if (userNum == 2)
                return Encoding.GetEncoding("utf-16");
            else
                return Encoding.GetEncoding(1251);
        }

        /// <summary>
        /// Создание простого текстового файла в выбранной пользователем кодировке.
        /// </summary>
        /// <param name="path"> Путь в текущий каталог.</param>
        public static void Create(string path)
        {
            var enc = ChoseEncoding();
            Console.WriteLine("Введите имя нового файла. Это должны быть строка состоящая из латинских букв и цифр.");
            Console.WriteLine($"Если файл с таким названием уже есть в этой папке то он будет перезаписан.");
            string inp = Console.ReadLine();

            while (!Regex.IsMatch(inp, @"^[A-Za-z0-9]*$"))
            {
                Console.WriteLine($"Что-то не так. Введите имя нового файла. Это должны быть строка состоящая из латинских букв и цифр.");
                inp = Console.ReadLine();
            }
            using (StreamWriter sw = new StreamWriter(new FileStream(path + '\\' + inp + ".txt", FileMode.OpenOrCreate), enc))
            {
                Console.WriteLine($"Вводите текст файла. Для окончания введите строку: '_END_'.");

                string line;
                while ((line = Console.ReadLine()) != "_END_")
                    sw.WriteLine(line);
            }
        }

        /// <summary>
        /// Конкатенация содержимого двух или более текстовых файлов и вывод результата в консоль в кодировке UTF-8.
        /// </summary>
        /// <param name="path"> Путь в текущий каталог.</param>
        public static void Summ(string path)
        {
            string inp = "";
            string[] needs = new string[100];
            Console.WriteLine("Выберите 1-й файл.");
            for (int i = 0; i < 100; ++i)
            {
                needs[i] = Choice(path);
                Console.WriteLine("Хотите добавить еще один файл? Для завершения введите _END_, иначе что-то другое.");
                inp = Console.ReadLine();
                if (inp == "_END_")
                    break;
            }
            Console.BackgroundColor = ConsoleColor.DarkBlue;
            foreach (var elem in needs)
            {
                if (elem == null)
                    break;
                using (StreamReader sr = new StreamReader(elem))
                {
                    string line;
                    while ((line = sr.ReadLine()) != null)
                        Console.WriteLine(line);
                }
            }
            Console.BackgroundColor = ConsoleColor.Black;
        }

        /// <summary>
        /// Выбор пользователем маски.
        /// </summary>
        /// <returns> Выбранная маска.</returns>
        public static string ReadMask()
        {
            Console.WriteLine($" Введите регулярное выражение для поиска по маске.");
            String mask;
            while (true)
            {
                try
                {
                    mask = Console.ReadLine();
                    new Regex(mask);
                    break;
                }
                catch
                {
                    Console.WriteLine($"Что-то не так. Введите регулярное выражение.");
                }
            }
            return mask;
        }


        /// <summary>
        /// Вывод всех файлов в текущей директории по маске.
        /// </summary>
        /// <param name="path"> Путь в текущий каталог.</param>
        public static void FilesMask(string path)
        {
            String mask = ReadMask();
            var allFiles = Directory.GetFiles(path);
            int j = 1;
            string[] files = new string[allFiles.Length];
            for (int i = 0; i < allFiles.Length; ++i)
            {
                if (Regex.IsMatch(allFiles[i], mask))
                {
                    Console.WriteLine(allFiles[i]);
                }
            }
        }


        /// <summary>
        /// Вывод всех файлов в текущей директории и ее поддиректориям по маске.
        /// </summary>
        /// <param name="path"> Путь в текущий каталог.</param>
        public static void FilesMaskDir(string path, string mask)
        {
            try
            {
                var allFiles = Directory.GetFiles(path);
                int j = 1;
                string[] files = new string[allFiles.Length];
                for (int i = 0; i < allFiles.Length; ++i)
                {
                    if (Regex.IsMatch(allFiles[i], mask))
                    {
                        Console.WriteLine(allFiles[i]);
                    }
                }
                var allDirs = Directory.GetDirectories(path);
                for (int i = 0; i < allDirs.Length; ++i)
                {
                    FilesMaskDir(allDirs[i], mask);
                }
            }
            catch
            {
                return;
            }
        }


        /// <summary>
        /// Копирование всех файлов в текущей директории и ее поддиректориям по маске.
        /// </summary>
        /// <param name="path"> Путь в текущий каталог.</param>
        public static void MoveMaskDir(string path, string mask, string newDir)
        {
            try
            {
                var was = Directory.GetFiles(newDir);
                var allFiles = Directory.GetFiles(path);
                int j = 1;
                string[] files = new string[allFiles.Length];
                for (int i = 0; i < allFiles.Length; ++i)
                {
                    if (Regex.IsMatch(allFiles[i], mask))
                    {
                        using (StreamReader sr = new StreamReader(allFiles[i]))
                        {
                            var buf = allFiles[i].Split('\\');
                            int userNum = 1;
                            string name = buf[buf.Length - 1];
                            if (was.Contains(newDir + '\\' + name))
                            {
                                Console.WriteLine("Что сделать с файлом: " + name);
                                Console.WriteLine("1) Заменить.");
                                Console.WriteLine("2) Пропустить.");
                                while (!int.TryParse(Console.ReadLine(), out userNum) || userNum < 1 || userNum > 2)
                                {
                                    Console.WriteLine($"Что-то не так. Введите число от 1 до {j - 1}.");
                                }
                            }
                            if (userNum == 1)
                            {
                                using (StreamWriter sw = new StreamWriter(newDir + '\\' + name))
                                {
                                    string line;
                                    while ((line = sr.ReadLine()) != null)
                                        sw.WriteLine(line);

                                }
                            }
                        }
                    }
                }
                var allDirs = Directory.GetDirectories(path);
                for (int i = 0; i < allDirs.Length; ++i)
                {
                    MoveMaskDir(allDirs[i], mask, newDir);
                }
            }
            catch
            {
                return;
            }
        }


        /// <summary>
        /// Главное меню с выбором одной из операций.
        /// </summary>
        /// <param name="path"> Путь в текущий каталог до изменений.</param>
        /// <returns> Путь в текущий каталог после изменений.</returns>
        static public string MainMenu(string path)
        {
            int count = 14;
            Console.WriteLine("Выберите одну из предложенных операций. Введите соответствующий ей номер.");
            Console.WriteLine($"1) Выбор локального диска.");
            Console.WriteLine($"2) Переход в подкаталог.");
            Console.WriteLine($"3) Список файлов в текущем каталоге.");
            Console.WriteLine($"4) Чтение файла txt в кодировке UTF-8.");
            Console.WriteLine($"5) Чтение файла txt.");
            Console.WriteLine($"6) Копирование файла txt в кодировке UTF-8.");
            Console.WriteLine($"7) Перемещение файла txt.");
            Console.WriteLine($"8) Удаление файла txt.");
            Console.WriteLine($"9) Создание файла txt в кодировке UTF-8.");
            Console.WriteLine($"10) Создание файла txt.");
            Console.WriteLine($"11) Конкатенация txt файлов в кодировке UTF-8");
            Console.WriteLine($"12) Вывод всех файлов в текущей директории по маске. ");
            Console.WriteLine($"13) Вывод всех файлов в текущей директории и ее поддиректориях по маске. ");
            Console.WriteLine($"14) Копирование всех файлов в текущей директории и ее поддиректориях по маске. ");
            int userNum;
            while (!int.TryParse(Console.ReadLine(), out userNum) || userNum < 1 || userNum > count)
            {
                Console.WriteLine($"Что-то не так. Введите число от 1 до {count}.");
            }
            if (userNum == 1)
            {
                path = Drivers();
            }
            else if (userNum == 2)
            {
                path = Move(path);
            }
            else if (userNum == 3)
            {
                Files(path);
            }
            else if (userNum == 4)
            {
                ReadDefault(path);
            }
            else if (userNum == 5)
            {
                Read(path);
            }
            else if (userNum == 6)
            {
                Copy(path);
            }
            else if (userNum == 7)
            {
                Moving(path);
            }
            else if (userNum == 8)
            {
                Dell(path);
            }
            else if (userNum == 9)
            {
                CreateDefault(path);
            }
            else if (userNum == 10)
            {
                Create(path);
            }
            else if (userNum == 11)
            {
                Summ(path);
            }
            else if (userNum == 12)
            {
                FilesMask(path);
            }
            else if (userNum == 13)
            {
                FilesMaskDir(path, ReadMask());
            }
            else if (userNum == 14)
            {
                Console.WriteLine("Введите адресс нового каталога.");
                string newPath = Console.ReadLine();
                while (!Directory.Exists(newPath))
                {
                    Console.WriteLine("Что-то не так. Введите адресс нового каталога.");
                    newPath = Console.ReadLine();
                }
                MoveMaskDir(path, ReadMask(), newPath);
            }
            return path;
        }

        /// <summary>
        /// Вывод пути в текущий каталог. Реализация повтора.
        /// </summary>
        /// <param name="path"> Путь в текущий каталог до изменений.</param>
        /// <returns> Путь в текущий каталог после изменений.</returns>
        static void Main(string[] args)
        {
            string path = Drivers();
            while (true)
            {
                Console.ForegroundColor = ConsoleColor.DarkGreen;
                Console.WriteLine(path);
                Console.ForegroundColor = ConsoleColor.White;
                Console.WriteLine("Хотите выйти? Введите 'yes' чтобы выйти, иначе что-либо другое.");
                string input = Console.ReadLine();
                if (input == "yes")
                {
                    return;
                }
                path = MainMenu(path);
            }
        }
    }
}
