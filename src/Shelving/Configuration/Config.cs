namespace Shelving.Configuration
{
    public interface Config
    {
        string ServicesDirectory { get; }
        RunAs HowToRun { get; }
    }

    public enum RunAs
    {
        LocalSystem,
        NetworkService,
        Interactive,
        LocalService
    }
}