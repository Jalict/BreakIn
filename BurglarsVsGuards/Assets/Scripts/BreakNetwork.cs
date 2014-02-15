using UnityEngine;
using System.Collections;

public class BreakNetwork {

    private static string name;
    private static bool _server = false;
    public static bool server { get { return _server;}
        set { 
            _server = value;
            if (_server) { // Start server
                name = Random.value.ToString();
                Network.InitializeServer(8, 3428, !Network.HavePublicAddress());
                MasterServer.RegisterHost("Break In NGJ2014", name);
            } else { // Stop server
                Network.Disconnect();
            }
        }
    }

    public static bool ConnectRandom() {

        HostData[] hosts = MasterServer.PollHostList();
        if(hosts.Length == 0) return false;
        Network.Connect(hosts[Mathf.FloorToInt(Random.value * hosts.Length)]);
        return true;
    }

    public static void Disconnect() {
        if (_server) MasterServer.UnregisterHost();
        Network.Disconnect();
    }
}
