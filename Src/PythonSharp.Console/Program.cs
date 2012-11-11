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
    }
}
