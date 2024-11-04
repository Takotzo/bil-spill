using Unity.Cinemachine;
using Unity.Netcode;
using UnityEngine;

public class S_CarConnectionSettings_IS : NetworkBehaviour
{
    [SerializeField] private CinemachineCamera playerCamera;
    [SerializeField] private int cameraPriority = 10;

    public override void OnNetworkSpawn()
    {
        if (IsOwner)
        {
            playerCamera.Priority = cameraPriority;
        }
    }
}
