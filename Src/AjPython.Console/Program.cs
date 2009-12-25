namespace AjPython.Console
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    using AjPython;
    using AjPython.Compiler;
    using AjPython.Commands;

    public class Program
    {
        public static void Main(string[] args)
        {
            PrintIntro();

            AjPython.Machine machine = new AjPython.Machine();
            Parser parser = new Parser(System.Console.In);

            while (true)
            {
                try
                {
                    ICommand command = parser.CompileCommand();

                    if (command == null)
                        break;

                    command.Execute(machine);
                }
                catch (Exception ex)
                {
                    System.Console.WriteLine(ex.Message);
                }
            }
        }

        private static void PrintIntro()
        {
            System.Console.WriteLine("AjPython 0.1");
            System.Console.Write(">>> ");
            System.Console.Out.Flush();
        }
    }
}
