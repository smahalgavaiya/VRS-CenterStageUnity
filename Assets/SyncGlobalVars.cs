using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[RequireComponent(typeof(PhotonView))]
public class SyncGlobalVars : MonoBehaviourPunCallbacks
{
    public List<GlobalInt> globalsList = new List<GlobalInt>();
    public float updateTime = 0.5f;

    private PhotonView view;
    // Start is called before the first frame update
    void Start()
    {
        view = GetComponent<PhotonView>();
        InvokeRepeating("syncVals", updateTime, updateTime);
    }

    void syncVals()
    {
        if (!PhotonNetwork.IsConnected || !PhotonNetwork.IsMasterClient)
        {
            CancelInvoke();
            return;
        }
        int[] intvals = globalsList.Select(x => x.globalInt).ToArray();
        view.RPC("updateVars", RpcTarget.AllBuffered, intvals);
    }

    [PunRPC]
    public void updateVars(int[] vars)
    {
        if (PhotonNetwork.IsMasterClient)
        {
            return;
        }
        for (int i = 0; i < vars.Length; i++)
        {
            Debug.Log($"Updating val:{globalsList[i].name} from {globalsList[i].globalInt} to {vars[i]}");
            globalsList[i].globalInt = vars[i];
        }
    }
}
