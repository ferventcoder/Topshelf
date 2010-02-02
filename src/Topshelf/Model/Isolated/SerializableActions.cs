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






        public static Action<object> Convert<O>(Action<O> input)
        {
            return o => input((O)o);
        }

        public static SerializableActions<object> ConvertActions<O>(Action<O> startAction, Action<O> stopAction, Action<O> pauseAction, Action<O> continueAction, ServiceBuilder builder)
        {
            return new SerializableActions<object>
                   {
                       StartAction = Convert(startAction),
                       StopAction = Convert(stopAction),
                       PauseAction = Convert(pauseAction),
                       ContinueAction = Convert(continueAction),
                       BuildAction = builder
            };
        }
    }
}