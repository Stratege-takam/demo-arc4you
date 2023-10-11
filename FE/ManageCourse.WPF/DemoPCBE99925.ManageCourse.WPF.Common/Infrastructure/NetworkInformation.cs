using Arc4u.Dependency.Attribute;
using Arc4u.Network.Connectivity;
using System;
using System.Net.NetworkInformation;

namespace EG.DemoPCBE99925.ManageCourse.WPF.Common.Infrastructure;

[Export(typeof(INetworkInformation)), Shared]
class NetworkInformation : INetworkInformation
{
    public NetworkStatus Status => NetworkInterface.GetIsNetworkAvailable() ? NetworkStatus.Wifi : NetworkStatus.None;
        
    public event EventHandler<NetworkInformationArgs> StatusMonitoring;
}