namespace PythonSharp.Console
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    using PythonSharp;
    using PythonSharp.Compiler;
    using PythonSharp.Commands;

    public class Program
    {
        public static void Main(string[] args)
        {
            PrintIntro();

            PythonSharp.Machine machine = new PythonSharp.Machine();
            Parser parser = new Parser(System.Console.In);

            while (true)
            {
                try
                {
                    ICommand command = parser.CompileCommand();

                    if (command == null)
                        break;

                    command.Execute(machine, machine.Environment);
                }
                catch (Exception ex)
                {
                    System.Console.WriteLine(ex.Message);
                }
            }
        }

        private static void PrintIntro()
        {
            System.Console.WriteLine("PythonSharp 0.1");
            System.Console.Write(">>> ");
            System.Console.Out.Flush();
        }
    }
}
