using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[RequireComponent(typeof(PhotonView))]
public class SyncGameTime : MonoBehaviourPunCallbacks
{
    public float updateTimer = 0.5f;
    private PhotonView view;
    private GameTimeManager gameTimeManager;
    // Start is called before the first frame update
    void Start()
    {
        view = GetComponent<PhotonView>();
        gameTimeManager = GetComponent<GameTimeManager>();
        InvokeRepeating("syncTime", updateTimer, updateTimer);
    }

    void syncTime()
    {
        if (!PhotonNetwork.IsConnected || !PhotonNetwork.IsMasterClient)
        {
            CancelInvoke();
            return;
        }

        view.RPC("updateTime", RpcTarget.AllBuffered, gameTimeManager.GetTime());
    }

    [PunRPC]
    public void updateTime(float[] vars)
    {
        if (PhotonNetwork.IsMasterClient)
        {
            return;
        }
        gameTimeManager.SyncTime(vars[0], vars[1], vars[2]);
    }
}
