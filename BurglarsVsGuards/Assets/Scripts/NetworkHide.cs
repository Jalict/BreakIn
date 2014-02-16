using UnityEngine;
using System.Collections;

public class NetworkHide : MonoBehaviour {
    public void ChangeMyVisibility(bool val) {
        if (networkView != null)
            networkView.RPC("MakeMeVisibleOrInvisible", RPCMode.AllBuffered, val);
    }

    [RPC]
    public void MakeMeVisibleOrInvisible(bool val) {
        renderer.enabled = val;
    }
}
