using UnityEngine;
using System.Collections;

public class BreakNetwork {


    private static bool _server = false;
    public static bool server { get { return _server;}
        set { 
            _server = value;
            if (_server) { // Start server
                Network.InitializeServer(8, 3428, !Network.HavePublicAddress());
                MasterServer.RegisterHost("Break In", Random.value.ToString());
            } else { // Stop server
                Network.Disconnect();
            }
        }
    }

    public static bool ConnectRandom() {
        MasterServer.RequestHostList("Break In");
        HostData[] hosts = MasterServer.PollHostList();
        if(hosts.Length == 0) return false;
        Network.Connect(hosts[Mathf.FloorToInt(Random.value * hosts.Length)]);
        return true;
    }
}
