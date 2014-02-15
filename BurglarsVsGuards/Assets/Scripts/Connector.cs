using UnityEngine;
using System.Collections;

public class Connector : MonoBehaviour {

    private bool messaged = false;
    private bool server = false;
    public Transform loader;

    void Start() {
        MasterServer.RequestHostList("Break In NGJ2014");
        Object.DontDestroyOnLoad(this);
	}

    void OnGUI() {
        if (Network.connections.Length > 0)  {
            if (!messaged) {
                Application.LoadLevel(1);
                messaged = true;
            }
            server = false;
            return;
        }
        messaged = false;

        GUILayout.BeginVertical();
        if (!server && GUILayout.Button("Host")) {
            Debug.Log("Hosting...");
            Network.InitializeServer(8, 3428, !Network.HavePublicAddress());
            MasterServer.RegisterHost("Break In NGJ2014", Random.value.ToString());
            server = true;
        }
        HostData[] data = MasterServer.PollHostList();

	    foreach(HostData element in data)
	    {
		    GUILayout.BeginHorizontal();	
		    string name = element.gameName + " " + element.connectedPlayers + " / " + element.playerLimit;
		    GUILayout.Label(name);	
		    GUILayout.Space(5);
		    string hostInfo = "[";
		    foreach (string host in element.ip)
			    hostInfo = hostInfo + host + ":" + element.port + " ";
		    hostInfo = hostInfo + "]";
		    GUILayout.Label(hostInfo);	
		    GUILayout.Space(5);
		    GUILayout.Label(element.comment);
		    GUILayout.Space(5);
		    GUILayout.FlexibleSpace();
		    if (GUILayout.Button("Connect"))
		    {
			    Network.Connect(element);
		    }
		    GUILayout.EndHorizontal();
	    }
        GUILayout.EndVertical();
    }
}
