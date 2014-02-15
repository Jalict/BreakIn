using UnityEngine;
using System.Collections;

public class Run : MonoBehaviour {

    void Start() {
        MasterServer.RequestHostList("Break In NGJ2014");
	}

    void OnGUI() {
        GUILayout.BeginVertical();
        if (GUILayout.Button("Host")) {
            Network.InitializeServer(8, 3428, !Network.HavePublicAddress());
            MasterServer.RegisterHost("Break In NGJ2014", name);
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
