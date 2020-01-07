using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace ConsoleApp3
{

    class Program
    {
        static void Main(string[] args)
        {
            Random rnd = new Random();
            long number;
            string path = @"C:\Test1";
            string path1 = @"C:\Test1\File1.txt";
            string path2 = @"C:\Test1\File2.txt";
            string path3 = @"C:\Test1\File3.txt";
            int p = 30;// количество значений в 2 файлах
            int t = p / 2;// количество значений в 1 файле


            DirectoryInfo dirInfo = new DirectoryInfo(path);
            if (!dirInfo.Exists)
            {
                dirInfo.Create();
            }

            using (StreamWriter sw1 = new StreamWriter(path1, false, System.Text.Encoding.Default)) { }
            using (StreamWriter sw2 = new StreamWriter(path2, false, System.Text.Encoding.Default)) { }

            Console.WriteLine("Файлы созданы.Начинаем запись значений");
            for (int ctr = 1; ctr <= p; ctr++)
            {
                FileInfo file1Inf = new FileInfo(path1);
                FileInfo file2Inf = new FileInfo(path2);
                number = (long)(rnd.NextDouble() * Int64.MaxValue);
                if (file1Inf.Length > file2Inf.Length)
                {
                    using (StreamWriter sw2 = new StreamWriter(path2, true, System.Text.Encoding.Default))
                    {
                        sw2.WriteLine(number);
                    }

                }
                else if (file1Inf.Length < file2Inf.Length)
                {
                    using (StreamWriter sw1 = new StreamWriter(path1, true, System.Text.Encoding.Default))
                    {
                        sw1.WriteLine(number);
                    }

                }

                else
                {
                    using (StreamWriter sw2 = new StreamWriter(path2, true, System.Text.Encoding.Default))
                    {
                        sw2.WriteLine(number);
                    }

                }
            }
            Console.WriteLine("Значения записаны в файлы.Начинаем сортировку этих файлов и запись отсортированных значений в новый файл.");
            FileInfo file = new FileInfo(path3);
            if (file.Exists)
            {
                file.Delete();
            }
            // делим большие файлы 
            string path11 = @"C:\Test1\File11.txt";
            string path12 = @"C:\Test1\File12.txt";
            Segmentation(t, path1, path11, path12);
            string path21 = @"C:\Test1\File21.txt";
            string path22 = @"C:\Test1\File22.txt";
            Segmentation(t, path2, path21, path22);
            // сортируем маленькие файлы
            _Sort2(path11);
            _Sort2(path12);
            _Sort2(path21);
            _Sort2(path22);
            //Сливаем 2 отсортированных файла в один отсортированный
            _Sort1(path11, path12, path1);
            _Sort1(path21, path22, path2);
            _Sort1(path1, path2, path3);
            Console.WriteLine("Файл записан");
            Console.ReadKey();
        }
        // метод сортировки одного маленького файла
        static void _Sort2(string path)
        {
            long[] nums = null;
            using (StreamReader sr = new StreamReader(path))
            {

                string line;
                while ((line = sr.ReadLine()) != null)
                {
                    long _line = Convert.ToInt64(line);
                    if (nums == null)
                    {
                        nums = new long[] { _line };
                    }
                    else
                    {
                        long[] tempnums = new long[nums.Length + 1];
                        for (int j = 0; j < nums.Length; j++)
                            tempnums[j] = nums[j];
                        tempnums[tempnums.Length - 1] = _line;
                        nums = tempnums;
                    }


                }

            }

            nums=sort(nums);
            using (StreamWriter sw3 = new StreamWriter(path, false, System.Text.Encoding.Default))
            {
                for (int i = 0; i < nums.Length; i++)
                {
                    sw3.WriteLine(nums[i]);
                }

            }
        }
        // метод слива отсортированных значений двух файлов в один (отсортированный)
        static void _Sort1(string pathin1,string pathin2,string pathout)
        {
            using (StreamWriter sw3 = new StreamWriter(pathout, false, System.Text.Encoding.Default))
            using (StreamReader sr1 = new StreamReader(pathin1))
            using (StreamReader sr2 = new StreamReader(pathin2))
            {

                long line1 = Convert.ToInt64(sr1.ReadLine());
                long line2 = Convert.ToInt64(sr2.ReadLine());

                if (line1 < line2)
                {
                    sw3.WriteLine(line1);
                    line1 = Convert.ToInt64(sr1.ReadLine());
                    _Sort(line1, line2, sr1, sr2, sw3);
                }
                else
                {
                    sw3.WriteLine(line2);
                    line2 = Convert.ToInt64(sr2.ReadLine());
                    _Sort(line1, line2, sr1, sr2, sw3);
                }
            }
        }
        // рекурсивный метод слива значений двух файлов в один (отсортированный)
        static void _Sort(long line1,long line2, StreamReader sr1, StreamReader sr2, StreamWriter sw3)
        {
            if (sr1.EndOfStream == false && sr2.EndOfStream == false)
            {
                
                if (line1 < line2)
                {
                    sw3.WriteLine(line1);
                    line1 = Convert.ToInt64(sr1.ReadLine());
                    _Sort(line1, line2, sr1, sr2, sw3);
                }
                else
                {
                    sw3.WriteLine(line2);
                    line2 = Convert.ToInt64(sr2.ReadLine());
                    _Sort(line1, line2, sr1, sr2, sw3);
                }
                
            }
            else if (sr1.EndOfStream)
            {
                if (line1 < line2)
                {
                    sw3.WriteLine(line1);
                    sw3.WriteLine(line2);
                }
                else
                {
                    sw3.WriteLine(line2);
                    sw3.WriteLine(line1);
                }
                
                long[] nums = null;
                while (sr2.EndOfStream == false)
                {
                    long _line = Convert.ToInt64(sr2.ReadLine());
                    if (nums == null)
                    {
                        nums = new long[] { _line };
                    }
                    else
                    {
                        long[] tempnums = new long[nums.Length + 1];
                        for (int j = 0; j < nums.Length; j++)
                            tempnums[j] = nums[j];
                        tempnums[tempnums.Length - 1] = _line;
                        nums = tempnums;
                    }
                }
                nums = sort(nums);

                for (int i = 0; i < nums.Length; i++)
                {
                    sw3.WriteLine(nums[i]);
                  
                }
                
            }
            else 
            {
                if (line1 < line2)
                {
                    sw3.WriteLine(line1);
                    sw3.WriteLine(line2);
                }
                else
                {
                    sw3.WriteLine(line2);
                    sw3.WriteLine(line1);
                }
                long[] nums = null;
                while (sr1.EndOfStream == false)
                {
                    long _line = Convert.ToInt64(sr1.ReadLine());
                    if (nums == null)
                    {
                        nums = new long[] { _line };
                    }
                    else
                    {
                        long[] tempnums = new long[nums.Length + 1];
                        for (int j = 0; j < nums.Length; j++)
                            tempnums[j] = nums[j];
                        tempnums[tempnums.Length - 1] = _line;
                        nums = tempnums;
                    }
                }
                nums = sort(nums);
                for (int i = 0; i < nums.Length; i++)
                {
                    sw3.WriteLine(nums[i]);
                   
                }
               
            }
            

        }
        // метод деления файла пополам
        static void Segmentation( int t,string pathin,string pathout1,string pathout2)
        {
            int p = t / 2;
            int p1 = t - p;
            using (StreamReader sr = new StreamReader(pathin))
            using (StreamWriter sw1 = new StreamWriter(pathout1, false, System.Text.Encoding.Default))
            using (StreamWriter sw2 = new StreamWriter(pathout2, false, System.Text.Encoding.Default))
            {
                for (int j = 0; j < p; j++)
                {
                    string s = sr.ReadLine();
                    sw1.WriteLine(s); // запись в файл   
                }
                for (int j = 0; j < p1; j++)
                {
                    string s = sr.ReadLine();
                    sw2.WriteLine(s); // запись в файл   
                }
            }
        }
        static long[] sort(long[] massive)
        {
            if (massive.Length == 1)
                return massive;
            int mid_point = massive.Length / 2;
            return merge(sort(massive.Take(mid_point).ToArray()), sort(massive.Skip(mid_point).ToArray()));
        }
        static long[] merge(long[] mass1, long[] mass2)
        {
            int a = 0, b = 0;
            long[] merged = new long[mass1.Length + mass2.Length];
            for (int i = 0; i < mass1.Length + mass2.Length; i++)
            {
                if (b < mass2.Length && a < mass1.Length)
                    if (mass1[a] > mass2[b] && b < mass2.Length)
                        merged[i] = mass2[b++];
                    else
                        merged[i] = mass1[a++];
                else
                    if (b < mass2.Length)
                    merged[i] = mass2[b++];
                else
                    merged[i] = mass1[a++];
            }
            return merged;
        }
    }
}
