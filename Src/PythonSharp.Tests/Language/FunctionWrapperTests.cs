namespace PythonSharp.Tests.Language
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;
    using System.Text;
    using System.Threading;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using PythonSharp.Commands;
    using PythonSharp.Expressions;
    using PythonSharp.Language;
    using PythonSharp.Tests.Classes;
    using PythonSharp.Utilities;

    [TestClass]
    public class FunctionWrapperTests
    {
        [TestMethod]
        public void InvokeFunctionWrapper()
        {
            FunctionWrapper<int, int, int, Func<int, int, int>> wrapper = new FunctionWrapper<int, int, int, Func<int, int, int>>(new Adder(), null);

            var result = wrapper.CreateFunctionDelegate().DynamicInvoke(2, 3);

            Assert.IsNotNull(result);
            Assert.AreEqual(5, result);
        }

        [TestMethod]
        public void AddEventHandlerToCalculator()
        {
            Listener listener = new Listener();
            Calculator calculator = new Calculator();
            FunctionWrapper<int, int, int, Action<int, int>> wrapper = new FunctionWrapper<int, int, int, Action<int, int>>(listener, null);
            typeof(Calculator).GetEvent("MyEvent").AddEventHandler(calculator, wrapper.CreateActionDelegate());
            calculator.Add(1, 2);

            Assert.AreEqual(1, listener.X);
            Assert.AreEqual(2, listener.Y);

            MethodInfo invoke = typeof(Calculator).GetEvent("MyEvent").EventHandlerType.GetMethod("Invoke");
            MethodInfo add = typeof(Calculator).GetEvent("MyEvent").GetAddMethod();
            Assert.IsNotNull(add);
            Assert.AreEqual(1, add.GetParameters().Count());
            Assert.IsNotNull(invoke);
            var parameters = invoke.GetParameters();
            var returnparameter = invoke.ReturnParameter;
            Assert.IsNotNull(returnparameter);
            Assert.AreEqual("System.Void", returnparameter.ParameterType.FullName);

            Assert.AreEqual(typeof(MyEvent), typeof(Person).GetEvent("NameEvent").EventHandlerType);
        }

        [TestMethod]
        public void AddEventHandlerToPerson()
        {
            NameListener listener = new NameListener();
            Person person = new Person() { FirstName = "Adam" };
            FunctionWrapper<string, int, MyEvent> wrapper = new FunctionWrapper<string, int, MyEvent>(listener, null);
            typeof(Person).GetEvent("NameEvent").AddEventHandler(person, wrapper.CreateFunctionDelegate());
            person.GetName();

            Assert.AreEqual(4, listener.Length);
            Assert.AreEqual("Adam", listener.Name);
        }

        [TestMethod]
        public void AddFunctionAsCalculatorEventHandler()
        {
            Calculator calculator = new Calculator();
            Listener listener = new Listener();
            ObjectUtilities.AddHandler(calculator, "MyEvent", listener, null);
            calculator.Add(1, 2);

            Assert.AreEqual(1, listener.X);
            Assert.AreEqual(2, listener.Y);
        }

        [TestMethod]
        public void AddFunctionAsPersonEventHandler()
        {
            Person person = new Person() { FirstName = "Adam" };
            NameListener listener = new NameListener();
            ObjectUtilities.AddHandler(person, "NameEvent", listener, null);
            person.GetName();

            Assert.AreEqual(4, listener.Length);
            Assert.AreEqual("Adam", listener.Name);
        }

        [TestMethod]
        public void CreateThreadStart()
        {
            BindingEnvironment environment = new BindingEnvironment();
            Runner function = new Runner();
            FunctionWrapper wrapper = new FunctionWrapper(function, environment);
            Thread th = new Thread(wrapper.CreateThreadStart());
            th.Start();
            th.Join();
            Assert.IsTrue(function.WasInvoked);
        }

        internal class Runner : IFunction
        {
            public bool WasInvoked { get; set; }

            public object Apply(IContext context, IList<object> arguments, IDictionary<string, object> namedArguments)
            {
                this.WasInvoked = true;
                return null;
            }
        }

        internal class Adder : IFunction
        {
            public object Apply(IContext context, IList<object> arguments, IDictionary<string, object> namedArguments)
            {
                return (int)arguments[0] + (int)arguments[1];
            }
        }

        internal class Listener : IFunction
        {
            public int X { get; set; }

            public int Y { get; set; }

            public object Apply(IContext context, IList<object> arguments, IDictionary<string, object> namedArguments)
            {
                this.X = (int)arguments[0];
                this.Y = (int)arguments[1];
                return null;
            }
        }

        internal class NameListener : IFunction
        {
            public int Length { get; set; }

            public string Name { get; set; }

            public object Apply(IContext context, IList<object> arguments, IDictionary<string, object> namedArguments)
            {
                this.Name = (string)arguments[0];
                this.Length = this.Name.Length;
                return this.Length;
            }
        }
    }
}
