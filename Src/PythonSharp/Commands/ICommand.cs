namespace PythonSharp.Commands
{
    public interface ICommand
    {
        void Execute(Machine machine, BindingEnvironment environment);
    }
}
