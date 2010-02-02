namespace Topshelf.Model.Isolated
{
    using System;

    [Serializable]
    public class SerializableActions<TService>
    {
        public Action<TService> StartAction { get; set; }
        public Action<TService> StopAction { get; set; }
        public Action<TService> PauseAction { get; set; }
        public Action<TService> ContinueAction { get; set; }
        public ServiceBuilder BuildAction { get; set; }
    }
}