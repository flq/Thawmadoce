namespace Thawmadoce.Frame
{
    public interface IUserInteraction
    {
        IDialogStarter Dialog<VM>();
    }

    public interface IDialogStarter
    {
        void Run();
        I Run<I>(I arguments);
    }
}