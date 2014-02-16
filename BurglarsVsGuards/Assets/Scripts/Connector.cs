using UnityEngine;
using System.Collections;

public class Connector : MonoBehaviour {

    private static Connector instance;

    private bool messaged = false;
    private bool server = false;
    public Transform loader;
    private string ipconnect = "123.456.789.123";

    void Start() {
        if (!instance) instance = this;
        MasterServer.RequestHostList("Break In NGJ2014");
        Object.DontDestroyOnLoad(this);
        gameObject.AddComponent<NetworkView>();
        networkView.stateSynchronization = NetworkStateSynchronization.Off;
        networkView.observed = null;
	}

    public static void AddEntity(string name, Vector3 pos, Quaternion rot, string prefab, string id, bool isStatic) {
        Transform t = ((GameObject)Instantiate(Resources.Load(prefab),pos,rot)).transform;
        t.gameObject.tag = id;

        t.gameObject.AddComponent<NetworkView>();
        t.networkView.viewID = Network.AllocateViewID();
        t.networkView.observed = t.transform;
        t.networkView.stateSynchronization = isStatic?NetworkStateSynchronization.Off:NetworkStateSynchronization.Unreliable;
        instance.networkView.RPC("AddStuff", RPCMode.OthersBuffered, prefab, name, t.networkView.viewID, id, isStatic, pos, rot);

        t.name = name;
    }

    public static GameObject AddEntityReturn(string name, Vector3 pos, Quaternion rot, string prefab, string id, bool isStatic)
    {
        GameObject t = ((GameObject)Instantiate(Resources.Load(prefab),pos,rot));
        t.gameObject.tag = id;

        t.AddComponent<NetworkView>();
        t.transform.networkView.viewID = Network.AllocateViewID();
        t.transform.networkView.observed = t.transform;
        t.transform.networkView.stateSynchronization = isStatic?NetworkStateSynchronization.Off:NetworkStateSynchronization.Unreliable;
        instance.networkView.RPC("AddStuff", RPCMode.OthersBuffered, prefab, name, t.networkView.viewID, id, isStatic, pos, rot);

        t.name = name;
        return t;
    }

    [RPC]
    private void AddStuff(string prefab, string name, NetworkViewID viewid, string id, bool isStatic, Vector3 pos, Quaternion rot) {
        Transform t = ((GameObject)Instantiate(Resources.Load(prefab),pos,rot)).transform;
        t.gameObject.AddComponent<NetworkView>();
        t.networkView.observed = t.transform;
        t.networkView.stateSynchronization = isStatic ? NetworkStateSynchronization.Off : NetworkStateSynchronization.Unreliable;
        t.networkView.viewID = viewid;

        if (id == "Player") {
            t.GetComponent<movement>().enabled = false;
            t.GetComponentInChildren<FOV2DVisionCone>().enabled = false;
            t.GetComponentInChildren<FOV2DEyes>().enabled = false;
        }

        t.name = name;
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

        GUILayout.Space(5);
        GUILayout.BeginVertical();
        GUILayout.BeginHorizontal();
        GUILayout.Space(5);
        if (!server && GUILayout.Button("Host to master server")) {
            Debug.Log("Hosting...");
            Network.InitializeServer(8, 3428, true);
            MasterServer.RegisterHost("Break In NGJ2014", Random.value.ToString());
            server = true;
        }
        if (!server && GUILayout.Button("Host LAN")) {
            Debug.Log("Hosting...");
            Network.InitializeServer(8, 3428, true);
            server = true;
        }
        GUILayout.Space(10);
        if (GUILayout.Button("Join IP: ")) {
            Network.Connect(ipconnect, 3428);
        }
        GUILayout.Space(5);
        ipconnect = GUILayout.TextField(ipconnect);

        //ipconnect = GUI.TextField(new Rect(Screen.width / 2, Screen.height / 2, 175, 60), ipconnect); // show name field
        GUILayout.EndHorizontal();

        try {
            HostData[] data = MasterServer.PollHostList();

            foreach (HostData element in data) {
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
                if (GUILayout.Button("Connect")) {
                    Network.Connect(element);
                }
                GUILayout.EndHorizontal();
            }
        } catch (System.Exception ex) {
            Debug.Log(ex);
        }
        GUILayout.EndVertical();
    }
}
