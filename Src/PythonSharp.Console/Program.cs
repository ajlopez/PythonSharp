namespace PythonSharp.Console
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using PythonSharp;
    using PythonSharp.Commands;
    using PythonSharp.Compiler;
    using PythonSharp.Expressions;
    using PythonSharp.Utilities;
    using System.IO;

    public class Program
    {
        public static void Main(string[] args)
        {
            PythonSharp.Machine machine = new PythonSharp.Machine();

            if (args != null && args.Length > 0)
                if (ProcessFiles(args, machine))
                    return;

            PrintIntro();

            Parser parser = new Parser(System.Console.In);

            while (true)
            {
                try
                {
                    ICommand command = parser.CompileCommand();

                    if (command == null)
                        break;

                    if (command is ExpressionCommand)
                    {
                        IExpression expr = ((ExpressionCommand)command).Expression;
                        var value = expr.Evaluate(machine.Environment);

                        if (value != null)
                            Console.WriteLine(ValueUtilities.AsPrintString(value));
                    }
                    else
                        command.Execute(machine.Environment);
                }
                catch (Exception ex)
                {
                    System.Console.WriteLine(ex.Message);
                }
            }
        }

        private static void PrintIntro()
        {
            System.Console.WriteLine("PythonSharp 0.0.1");
            System.Console.Write(">>> ");
            System.Console.Out.Flush();
        }

        private static bool ProcessFiles(string[] args, Machine machine)
        {
            bool hasfiles = false;

            foreach (var arg in args)
            {
                if (!arg.EndsWith(".py"))
                    continue;

                hasfiles = true;
                Parser parser = new Parser(new StreamReader(arg));
                ICommand command = parser.CompileCommandList();
                command.Execute(machine.Environment);
            }

            return hasfiles;
        }
    }
}
