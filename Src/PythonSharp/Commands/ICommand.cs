namespace PythonSharp.Commands
{
    using PythonSharp.Language;

    public interface ICommand
    {
        void Execute(IContext context);
    }
}
