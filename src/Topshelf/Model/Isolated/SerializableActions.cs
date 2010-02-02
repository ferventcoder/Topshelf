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

        public void StartActionObject(object obj)
        {
            StartAction((TService)obj);
        }

        public void StopActionObject(object obj)
        {
            StopAction((TService)obj);
        }

        public void PauseActionObject(object obj)
        {
            PauseAction((TService)obj);
        }

        public void ContinueActionObject(object obj)
        {
            ContinueAction((TService)obj);
        }

        public object BuildServiceObject(string name)
        {
            return BuildAction(name);
        }
    }
}