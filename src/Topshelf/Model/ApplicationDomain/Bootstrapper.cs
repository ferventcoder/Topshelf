namespace Topshelf.Model.ApplicationDomain
{
    using System;

    public interface Bootstrapper
    {
        Type ServiceType { get; }
        object BuildService();
    }
}