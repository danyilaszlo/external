using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using System.IO;
using System.Net.Mail;

namespace external
{
    class Program
    {

        

        static void Main(string[] args)
        {

            string mail_body = "\n";


            void filelist(FileInfo[] fi_array)
            {
                foreach (FileInfo fil in fi_array)
                {
                Console.WriteLine(Path.GetFullPath(fil.FullName));
                Console.WriteLine(Path.GetFileNameWithoutExtension(fil.Name));
                }
            }


            FileInfo[] filesort(FileInfo[] fi_array)
            {
                for (int i = fi_array.Length - 1; i > 0; i--)
                {
                    int max = i;
                    for (int j = 0; j <= i; j++)
                        if (int.Parse(Path.GetFileNameWithoutExtension(fi_array[j].Name.Remove(0, 3))) > int.Parse(Path.GetFileNameWithoutExtension(fi_array[max].Name.Remove(0, 3))))
                            max = j;
                    FileInfo swap = fi_array[i];
                    fi_array[i] = fi_array[max];
                    fi_array[max] = swap;
                }
                return fi_array;
            }


            string tomorit()
            {
                Process p = new Process();

                //Így is lehet: parancsor meghívás + a .bat file megadása paraméterként:
                //p.StartInfo.FileName = "cmd.exe";
                //p.StartInfo.Arguments = @"/c C:\Users\danyi.laszlo\source\repos\external\tomorit.bat";
                //p.StartInfo.Arguments = @"/c cd C:\Users\danyi.laszlo\source\repos\external\";

                //p.StartInfo.FileName = @"C:\Users\danyi.laszlo\source\repos\external\tomorit.bat";
                //p.StartInfo.Arguments = "ha valami paramétert kell megadni a meghívott külső programnak azt ide kell írni...";

                //p.StartInfo.UseShellExecute = false;
                //p.StartInfo.RedirectStandardOutput = true;
                //p.Start();

                //string output = p.StandardOutput.ReadToEnd();
                //p.WaitForExit();

                p.StartInfo.FileName = "cmd.exe";
                p.StartInfo.Arguments = @"/c rar.exe a -ep C:\Users\danyi.laszlo\source\repos\external\test.rar C:\Users\danyi.laszlo\source\repos\external\*.bkp";

                p.StartInfo.UseShellExecute = false;
                p.StartInfo.RedirectStandardOutput = true;
                p.Start();

                string output = p.StandardOutput.ReadToEnd();
                p.WaitForExit();

                return output;
 
            }



            //string curFile = @"C:\Users\danyi.laszlo\source\repos\external\list.bat";
            //Console.Write(@"C:\Users\danyi.laszlo\source\repos\external\list.bat file ");
            //Console.WriteLine(File.Exists(curFile) ? "létezik." : "nem létezik.");
            //Console.ReadKey();


            DirectoryInfo di = new DirectoryInfo(@"C:\Users\danyi.laszlo\source\repos\external");
            FileInfo[] files = di.GetFiles("*.bkp");


            //string[] lines = File.ReadAllLines(@"C:\Users\danyi.laszlo\source\repos\external\param.txt", Encoding.UTF8);
            //Console.WriteLine("A file 1. sora: " + lines[0]);
            //Console.WriteLine("Első sor hossza: " + lines[0].Length);
            //Console.WriteLine("A file 2. sora: " + lines[1]);
            //Console.WriteLine("Második sor hossza: " + lines[1].Length);

            //Console.WriteLine("Első paraméter .Remove-val: " + lines[0].Remove(0, lines[0].IndexOf('=')+1));

            //if ( String.Equals(lines[0].Substring(0, 4), "ELSO"))
            //    {
            //        Console.WriteLine("Első paraméter .SubString-gel: " + lines[0].Substring(5, lines[0].Length-5));
            //    }

            //if (String.Equals(lines[1].Substring(0, 7), "MASODIK"))
            //{
            //    Console.WriteLine("Második paraméter .SubString-gel: " + lines[1].Substring(8, lines[1].Length - 8));
            //}


            //filelist(files);

            //DirectoryInfo di = new DirectoryInfo(@"C:\Users\danyi.laszlo\source\repos\external");
            //FileInfo[] files = di.GetFiles("*.bkp");
            //Console.WriteLine("Fájlok száma a mappában: {0}.", files.Length);


            //foreach (FileInfo file in files)
            //{
            //    Console.WriteLine(file);
            //}

            //Console.WriteLine("A fájl nevek, kiterjesztés nélkül rendezés előtt:");
            //foreach (FileInfo fi in files)
            //{
            //    Console.WriteLine(Path.GetFileNameWithoutExtension(fi.Name));
            //}

            //Console.WriteLine( Path.GetFileNameWithoutExtension(files[0].Name.Remove(0,3)));

            files = filesort(files);

            //Console.WriteLine("A fájl nevek, kiterjesztés nélkül rendezés után:");
            //foreach (FileInfo fi in files)
            //{
            //    Console.WriteLine(Path.GetFileNameWithoutExtension(fi.Name));
            //}
            //Console.ReadKey();

            Console.WriteLine("A fájl nevek, rendezés után:");
            foreach (FileInfo fi1 in files)
            {
                Console.WriteLine(Path.GetFullPath(fi1.FullName));
                Console.WriteLine(Path.GetFileName(fi1.Name));
            }
            Console.ReadKey();


            if (files.Length > 2)
            {
                for (int i = 0; i < files.Length - 2; i++)
                {
                    //Console.WriteLine(Path.GetFullPath(files[i].FullName));
                    //Console.WriteLine(Path.GetFileName(files[i].Name));
 
                    if (File.Exists(Path.GetFullPath(files[i].FullName)))
                    {
                        File.Delete(Path.GetFullPath(files[i].FullName));
                        Console.WriteLine(Path.GetFullPath(files[i].FullName) + " nevű fájl törölve!");
                        mail_body = mail_body + Path.GetFullPath(files[i].FullName) + " nevű fájl törölve!\n" ;
                    }
                    else
                    {
                        Console.WriteLine("Ilyen nevű file nem létezik!");
                    }
                }
            }
            else
            {
                mail_body = mail_body + "\nNincs törlendő .BKP file\n";
                Console.WriteLine("Csak 2 file van, nincs mit törölni");
                //MailMessage mail = new MailMessage("support@kedvenckereskedohaz.hu", "l.danyi99@gmail.com");

            }

            File.Delete(@"C:\Users\danyi.laszlo\source\repos\external\test.rar");

            mail_body = mail_body + tomorit();
            Console.WriteLine("Mail body tartalma: " + mail_body);

            try
            {
                //do submit
                MailMessage mail = new MailMessage("support@kedvenckereskedohaz.hu", "l.danyi99@gmail.com");

                mail.Subject = "Mentés log...";
                mail.Body = mail_body;
                mail.Priority = MailPriority.Normal;
                SmtpClient MailClient = new SmtpClient("192.168.8.23");
                //MailClient.Credentials = new System.Net.NetworkCredential("account2@gmail.com", "password");
                MailClient.Send(mail);
                Console.WriteLine("E-mail elküdve");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                Console.ReadKey();
            }


            Console.ReadKey();



            ////  **************************************************************
            ////  **************  Külső program meghívás  **********************

            //Process p = new Process();

            //Így is lehet: parancsor meghívás +a bat file megadása paraméterként:
            //            p.StartInfo.FileName = "cmd.exe";
            //p.StartInfo.Arguments = @"/c C:\Users\danyi.laszlo\source\repos\external\list.bat";

            //p.StartInfo.FileName = @"C:\Users\danyi.laszlo\source\repos\external\list.bat";
            //p.StartInfo.Arguments = "ha valami paramétert kell megadni a meghívott külső programnak azt ide kell írni...";

            //p.StartInfo.UseShellExecute = false;
            //p.StartInfo.RedirectStandardOutput = true;
            //p.Start();

            //string output = p.StandardOutput.ReadToEnd();
            //p.WaitForExit();

            //Console.WriteLine("Output:");
            //Console.WriteLine(output);
            //Console.ReadKey();



        }
    }
}
