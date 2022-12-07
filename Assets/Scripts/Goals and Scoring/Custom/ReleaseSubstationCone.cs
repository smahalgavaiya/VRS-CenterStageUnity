using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class ReleaseSubstationCone : MonoBehaviourPunCallbacks
{
    List<GameObject> conePositions = new List<GameObject>();
    [SerializeField] GameObject conePositionsParent;
    [SerializeField] TeamColor teamColor;

    [SerializeField] GameObject cone;

    List<GameObject> cones;
    PhotonView view;


    int numberOfConesReleased = 0;

    // Start is called before the first frame update
    void Start()
    {
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

    public void ReleaseNewCone()
    {
        // We can only fit a certain number of cones in the substation

        if (numberOfConesReleased > cones.Count - 1)
            return;

        for (int i = 0; i < conePositions.Count; i++)
        {
            if (conePositions[i].GetComponent<ConeOccupancyChecker>().IsEmptyNoCone)
            {
                GameObject coneToRelease = cones[numberOfConesReleased];
                coneToRelease.GetComponentInParent<ConeDispenser>().DispenseCone();

                view.RPC("SpawnCone", RpcTarget.All, conePositions[i].transform.position);

                break;
            }
        }
    }

    [PunRPC]
    void SpawnCone(Vector3 position)
    {
        GameObject newCone = Instantiate(cone);
        newCone.GetComponent<ColorSwitcher>().TeamColor_ = teamColor;
        newCone.GetComponent<ColorSwitcher>().SetColor();
        newCone.GetComponent<ScoreObjectTypeLink>().LastTouchedTeamColor = teamColor;
        newCone.transform.position = position;
        newCone.GetComponent<Cone>().MakeScorable();
        numberOfConesReleased++;
        /*PhotonRigidbodyView rig = newCone.AddComponent<PhotonRigidbodyView>();*/

        PhotonTransformView photonTransformView = newCone.AddComponent<PhotonTransformView>();
        photonTransformView.m_SynchronizeScale = false;
        photonTransformView.m_SynchronizeRotation = true;
        photonTransformView.m_SynchronizePosition = true;

        PhotonView v = newCone.AddComponent<PhotonView>();
        v.OwnershipTransfer = OwnershipOption.Takeover;
        v.ObservedComponents = new List<Component>() { /*rig,*/ photonTransformView };
        v.ViewID = PhotonNetwork.AllocateViewID(0);
        v.Synchronization = ViewSynchronization.ReliableDeltaCompressed;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
