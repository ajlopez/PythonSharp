namespace PythonSharp.Commands
{
    public interface ICommand
    {
        void Execute(BindingEnvironment environment);
    }
}
