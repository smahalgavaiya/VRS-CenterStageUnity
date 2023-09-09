using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using System.Runtime.InteropServices.WindowsRuntime;
using System.IO;

public class ReleaseSubstationCone : MonoBehaviourPunCallbacks
{
    List<GameObject> conePositions = new List<GameObject>();
    [SerializeField] GameObject conePositionsParent;
    [SerializeField] TeamColor teamColor;

    [SerializeField] GameObject cone;

    List<GameObject> cones;
    PhotonView view;


    int numberOfConesReleased = 0;
    bool activeForLocalPlayer = true;

    // Start is called before the first frame update
    void Start()
    {
        FieldManager.OnLocalTeamSet += SetIsAvailable;
        conePositions = new List<GameObject>(); 

        cones = new List<GameObject>();

        foreach(ConeDispenser coneDispenser in GetComponentsInChildren<ConeDispenser>())
        {
            foreach(Cone cone in coneDispenser.DummyCones.GetComponentsInChildren<Cone>())
            {
                cones.Add(cone.gameObject);
            }
        }

        for(int i = 0; i < conePositionsParent.transform.childCount; i++)
        {
            conePositions.Add(conePositionsParent.transform.GetChild(i).gameObject);
        }
        view = gameObject.GetComponent<PhotonView>();
    }

    public void SetIsAvailable(TeamColor localPlayerTeam)
    {
        
        FieldManager.OnLocalTeamSet -= SetIsAvailable;
        if(teamColor != localPlayerTeam) { activeForLocalPlayer = false; }
    }

    public void ReleaseNewCone()
    {
        // We can only fit a certain number of cones in the substation
        //get team identity of local player.
        if (numberOfConesReleased > cones.Count - 1)
            return;
        if (!activeForLocalPlayer) { return; }

        for (int i = 0; i < conePositions.Count; i++)
        {
            if (conePositions[i].GetComponent<ConeOccupancyChecker>().IsEmptyNoCone)
            {
                GameObject coneToRelease = cones[numberOfConesReleased];
                coneToRelease.GetComponentInParent<ConeDispenser>().DispenseCone();

                if(PhotonNetwork.IsConnected)
                {
                    //int viewNum  = SpawnCone(conePositions[i].transform.position);
                    if(PhotonNetwork.IsMasterClient)
                    {
                        initialSpawnCone(conePositions[i].transform.position);
                    }
                    else { view.RPC("initialSpawnCone", RpcTarget.MasterClient, conePositions[i].transform.position); }
                    
                }
                else
                {
                    SpawnCone(conePositions[i].transform.position);
                }

                break;
            }
        }
    }

    [PunRPC]
    void initialSpawnCone(Vector3 position)
    {
        GameObject newCone = PhotonNetwork.InstantiateRoomObject(Path.Combine("PhotonPrefabs", "Cone"), position, Quaternion.identity);
        PhotonView v = newCone.GetComponent<PhotonView>();

        view.RPC("SpawnCone", RpcTarget.AllBuffered, newCone.transform.position, v.ViewID);
    }
    //instantiate room object for proper instantiation but master client needs to be the client to spawn it.
    [PunRPC]
    int SpawnCone(Vector3 position, int view=-1)
    {
        GameObject newCone;
        if (PhotonNetwork.IsConnected)
        {
            PhotonView vi = PhotonNetwork.GetPhotonView(view);
            newCone = vi.gameObject;
        }
        else
        {
            newCone = Instantiate(cone);
        }
        //GameObject newCone = PhotonNetwork.InstantiateRoomObject("cone", position, Quaternion.identity);
        //GameObject newCone = FieldManager.createGameObj(cone, position, name, Quaternion.identity);
        newCone.GetComponent<ColorSwitcher>().TeamColor_ = teamColor;
        newCone.GetComponent<ColorSwitcher>().SetColor();
        newCone.GetComponent<ScoreObjectTypeLink>().LastTouchedTeamColor = teamColor;
        newCone.transform.position = position;
        newCone.GetComponent<Cone>().MakeScorable();
        
        numberOfConesReleased++;    

        /*if (PhotonNetwork.IsConnected)
        {
            //PhotonRigidbodyView rig = newCone.AddComponent<PhotonRigidbodyView>();

            PhotonTransformView photonTransformView = newCone.AddComponent<PhotonTransformView>();
            photonTransformView.m_SynchronizeScale = false;
            photonTransformView.m_SynchronizeRotation = true;
            photonTransformView.m_SynchronizePosition = true;

            PhotonView v = newCone.AddComponent<PhotonView>();
            v.OwnershipTransfer = OwnershipOption.Takeover;
            v.ObservedComponents = new List<Component>() { photonTransformView };
            if (view == -1)
            {
                v.ViewID = PhotonNetwork.AllocateViewID(0);
            }
            else { v.ViewID = PhotonNetwork.AllocateViewID(view); }
            v.Synchronization = ViewSynchronization.ReliableDeltaCompressed;
            
            //Debug
            DisplayViewInfo inf = FindObjectOfType<DisplayViewInfo>();
            inf.SetNewConeView(newCone);
            return v.ViewID;
        }*/
        return -1;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
