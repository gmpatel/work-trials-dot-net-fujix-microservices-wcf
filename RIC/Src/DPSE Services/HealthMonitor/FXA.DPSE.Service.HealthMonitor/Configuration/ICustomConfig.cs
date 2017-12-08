using System.Collections.Generic;
using FXA.DPSE.Service.HealthMonitor.Configuration.Elements;

namespace FXA.DPSE.Service.HealthMonitor.Configuration
{
    public interface ICustomConfig
    {
        IList<EndPointElement> EndPoints { get; }
    }
}