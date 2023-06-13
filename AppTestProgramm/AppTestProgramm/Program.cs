using System;
using System.Collections.Generic;

namespace AppTestProgramm
{
    class Program
    {
        // Эта функция вычисляет квадратные корни, и возвращает их в виде массива с числами
        // Первое значение в этом массиве - это код ответа
        public static double[] QuadSqrEquat(double a, double b, double c)
        {
            double discriminant = b * b - 4 * a * c;

            double[] outArr = new double[3];

            if (a == 0)
            {
                //return ("Коэффициент a не может равняться 0.");
                outArr[0] = 0;
            }
            else if (discriminant > 0)
            {
                double x1 = (-b + Math.Sqrt(discriminant)) / (2 * a);
                double x2 = (-b - Math.Sqrt(discriminant)) / (2 * a);

                //string outt = "x1 = " + x1 + "\n" + "x2 = " + x2;
                //return outt;

                outArr[0] = 3;  // Код ответа
                outArr[1] = x1; // х1 
                outArr[2] = x2; // х2
            }
            else if (discriminant == 0)
            {
                double x = -b / (2 * a);
                //return ("x = " + x);
                outArr[0] = 2;
                outArr[1] = x;
            }
            else
            {
                //return ("Уравнение не имеет рациональных корней.");
                outArr[0] = 1;
            }

            return (outArr);
        }

        // Эта процедура получает массив со значениями из пердыдущей, и выводит сообщения для пользователя
        public static string OnBox(double a, double b, double c)
        {
            double[] outArr = new double[3];
            // outArr[0] = Код завершения вычисления
            // outArr[1] = x1
            // outArr[2] = x2

            Console.WriteLine(" ");
            Console.WriteLine("Уравнение: " + a + "x^2 + " + b + "x + " + c + " = 0");
            Console.WriteLine(" ");
            Console.WriteLine("Ответ:");

            outArr = QuadSqrEquat(a, b, c);

            if (outArr[0] == 0) return("Коэффициент a не может равняться 0");
            else if (outArr[0] == 1) return("Уравнение не имеет рациональных корней");
            else if (outArr[0] == 2) return("x = " + outArr[1]);
            else if (outArr[0] == 3) return("x1 = " + outArr[1] + "\n" + "x2 = " + outArr[2]);
            return("");
        }

        /*
            Для вычисления корней квадратного уравнения я использую формулу Дискриминанта.
            Затем, в тестах, я использую теорему Виета, для обратного получения коэффициентов квадратного уравнения, из их корней,
            привожу их к одному значению по коэффициенту а, и сравниваю.
        */

        // Верхняя граница для генерации случайных коэффициентов, нижняя,
        // и степень на которую будет домножаться разница в ответах, для вычисления ошибки
        public static void Rand_Test(int gran_n, int gran_v, int coeff, int way)
        {
            Random Random = new Random();
            int ind = 1;

            while(ind<16)
            {
                int Rand_a = Random.Next(gran_n, gran_v);
                int Rand_b = Random.Next(gran_n, gran_v);
                int Rand_c = Random.Next(gran_n, gran_v);

                double[] outArr = new double[3];
                outArr = QuadSqrEquat(Rand_a, Rand_b, Rand_c);

                if ((outArr[0] == 0) || (outArr[0] == 1)) continue; 

                Console.WriteLine(" ");

                Console.WriteLine("Исходное уравнение: ");
                Console.WriteLine(OnBox(Rand_a, Rand_b, Rand_c));                

                Console.WriteLine(" ");
                Console.WriteLine("Уравнение из коэффициентов: ");
                double nw_a = 1;

                double otn = Rand_a / nw_a; // Определяю кэффициент отношения

                nw_a = Rand_a;

                double nw_b = 0;
                double nw_c = 0;

                // Теорема Виета
                if (outArr[0] == 3)
                {
                    // a = 1
                    // b = -(x1 + x2)
                    // c = x1 * x2 

                    nw_b = -(outArr[1] + outArr[2]); nw_b *= otn;
                    nw_c = (outArr[1] * outArr[2]); nw_c *= otn;
                }
                else if (outArr[0] == 2)
                {
                    // b = -2*x
                    // c = x**2

                    nw_b = (-2) * outArr[1]; nw_b *= otn;
                    nw_c = Math.Pow(outArr[1], 2); nw_c *= otn;
                }

                Console.WriteLine(nw_a + "x^2 + " + nw_b + "x + " + nw_c + " = 0");

                double f_b = (Rand_b - nw_b) * Math.Pow(10, coeff);
                double f_c = (Rand_c - nw_c) * Math.Pow(10, coeff);

                Console.WriteLine(" ");
                Console.WriteLine("Коэффициент ошибки в вычисениях:");
                Console.WriteLine("b * 10^" + coeff + " ~ " + f_b);
                Console.WriteLine("c * 10^" + coeff + " ~ " + f_c);
                Console.WriteLine(" ");

                if ((Math.Floor(Math.Abs(f_b)) <= 0) && (Math.Floor(Math.Abs(f_c)) <= 0))
                {
                    Console.BackgroundColor = ConsoleColor.Green; // Зелёный фон
                    Console.ForegroundColor = ConsoleColor.Black;
                    Console.Write("Тест №" + ind + " - Пройден");
                    Console.ResetColor();
                }
                else
                {
                    Console.BackgroundColor = ConsoleColor.Red;   // Красный фон
                    Console.Write("(!) Тест №" + ind + " - Не пройден");
                    ErrList.Add("Проход " + way + ", ошибка в тесте №" + ind);
                }
                
                ind++;

                Console.ResetColor();

                Console.WriteLine(" ");
                Console.WriteLine("-----");
            }
        }

        public static List<string> ErrList = new List<string>();

        public static void RandomTesting()
        {
            Console.WriteLine("Автоматическое тестирование со случайными коэффициентами, и проверка ответов теоремой Виета:");            

            Console.WriteLine(" "); Console.WriteLine("----- Ранднмное тестирование с маленькими числами ----- "); Console.WriteLine(" ");
            Rand_Test(-10, 10, 14, 1);

            Console.WriteLine(" "); Console.WriteLine("----- Ранднмное тестирование с небольшими числами -----"); Console.WriteLine(" ");
            Rand_Test(-100, 100, 12, 2);

            Console.WriteLine(" "); Console.WriteLine("----- Ранднмное тестирование с большими числами -----"); Console.WriteLine(" ");
            Rand_Test(-10000, 10000, 9, 3);

            Console.WriteLine(" ");
            if (ErrList.Count != 0)
            {
                Console.WriteLine("Обнаружены ошибки в следующих тестах:");
                ErrList.ForEach(Console.WriteLine);

                // Если вы хотите увидеть, как выводятся ошибки, измените 3е значение в вызове функции Rand_Test на большое. Например, на 15 или на 20.
            }
            else
            {
                Console.WriteLine("Ошибок не обнаружено, все тесты пройдены успешно!");
            }

        }

        public static void TestingAnsw()
        {
            Console.WriteLine("Тестирование уравнений путём сравнения уже вычисленных ответов");

            /*
                a = 2,      b = -3,     c = 1.      Ответ: x1 = 1, x2 = 0.5
                a = -1,     b = 4,      c = -4.     Ответ: x = 2
                a = 1,      b = -6,     c = 9.      Ответ: x = 3
                a = 0,      b = 3,      c = -7.     Ответ: Коэффициент a не может равняться 0
                a = 1,      b = 2,      c = 3.      Ответ: Уравнение не имеет рациональных корней
                a = 4,      b = 4,      c = 4.      Ответ: Уравнение не имеет рациональных корней
                a = 0,      b = 0,      c = 5.      Ответ: Коэффициент a не может равняться 0
                a = 1,      b = -4,     c = 0.      Ответ: x1 = 4, x2 = 0
                a = 1,      b = -8,     c = 0.      Ответ: x1 = 8, x2 = 0
                a = 1,      b = -9,     c = 0.      Ответ: x1 = 9, x2 = 0
                a = 1,      b = -10,    c = 0.      Ответ: x1 = 10, x2 = 0
                a = 1,      b = -14,    c = 0.      Ответ: x1 = 14, x2 = 0

                Вычисления производились с помощью сайта: https://ru.onlinemschool.com/math/assistance/equation/quadratic/
            */

            double[] outArr = new double[3];

            outArr = QuadSqrEquat(2, -3, 1);
            if((outArr[0] == 3) && (outArr[1] == 1) && (outArr[2] == 0.5)) Console.WriteLine("Тест №1 - Пройден");
            else Console.WriteLine("(!) Тест №1 - Не пройден");

            outArr = QuadSqrEquat(-1, 4, -4);
            if ((outArr[0] == 2) && (outArr[1] == 2)) Console.WriteLine("Тест №2 - Пройден");
            else Console.WriteLine("(!) Тест №2 - Не пройден");

            outArr = QuadSqrEquat(1, -6, 9);
            if ((outArr[0] == 2) && (outArr[1] == 3)) Console.WriteLine("Тест №3 - Пройден");
            else Console.WriteLine("(!) Тест №3 - Не пройден");

            outArr = QuadSqrEquat(0, 3, -7);
            if ((outArr[0] == 0)) Console.WriteLine("Тест №4 - Пройден");
            else Console.WriteLine("(!) Тест №4 - Не пройден");

            outArr = QuadSqrEquat(1, 2, 3);
            if ((outArr[0] == 1)) Console.WriteLine("Тест №5 - Пройден");
            else Console.WriteLine("(!) Тест №5 - Не пройден");

            outArr = QuadSqrEquat(4, 4, 4);
            if ((outArr[0] == 1)) Console.WriteLine("Тест №6 - Пройден");
            else Console.WriteLine("(!) Тест №6 - Не пройден");

            outArr = QuadSqrEquat(0, 7, 5);
            if ((outArr[0] == 0)) Console.WriteLine("Тест №7 - Пройден");
            else Console.WriteLine("(!) Тест №7 - Не пройден");

            outArr = QuadSqrEquat(1, -4, 0);
            if ((outArr[0] == 3) && (outArr[1] == 4) && (outArr[2] == 0)) Console.WriteLine("Тест №8 - Пройден");
            else Console.WriteLine("(!) Тест №8 - Не пройден");

            outArr = QuadSqrEquat(1, -8, 0);
            if ((outArr[0] == 3) && (outArr[1] == 8) && (outArr[2] == 0)) Console.WriteLine("Тест №9 - Пройден");
            else Console.WriteLine("(!) Тест №9 - Не пройден");

            outArr = QuadSqrEquat(1, -9, 0);
            if ((outArr[0] == 3) && (outArr[1] == 9) && (outArr[2] == 0)) Console.WriteLine("Тест №10 - Пройден");
            else Console.WriteLine("(!) Тест №10 - Не пройден");

            outArr = QuadSqrEquat(1, -10, 0);
            if ((outArr[0] == 3) && (outArr[1] == 10) && (outArr[2] == 0)) Console.WriteLine("Тест №11 - Пройден");
            else Console.WriteLine("(!) Тест №11 - Не пройден");

            outArr = QuadSqrEquat(1, -14, 0);
            if ((outArr[0] == 3) && (outArr[1] == 14) && (outArr[2] == 0)) Console.WriteLine("Тест №12 - Пройден");
            else Console.WriteLine("(!) Тест №12 - Не пройден");
            Console.WriteLine(" ");
        }

        static void Main(string[] args)
        {
            // Тестирование уравнений путём сравнения уже вычисленных ответов
            TestingAnsw();

            // Автоматическое тестирование со случайными коэффициентами, и проверка ответов теоремой Виета:
            RandomTesting();

            // Ручной ввод от пользователя и вычисление корней квадратного уравнения:
            /*
            Console.WriteLine("Введите коэффициенты квадратного уравнения:");
            Console.Write("a = "); double a = double.Parse(Console.ReadLine());
            Console.Write("b = "); double b = double.Parse(Console.ReadLine());
            Console.Write("c = "); double c = double.Parse(Console.ReadLine());

            Console.WriteLine(OnBox(a, b, c));
            */

            // Автоматический ввод коэффициентов и вычисление корней квадратного уравнения:
            /*
            Console.WriteLine(OnBox(4, 6, -98));
            */

            Console.ReadLine();
        }
    }
}
