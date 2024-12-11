using System.Collections;
using Unity.Mathematics;
using Unity.Netcode;
using UnityEngine;

public class MovePlayer : NetworkBehaviour
{
    [SerializeField] private S_CarConnectionSettings_IS playerPrefab;
    

    public void HandlePlayerMovement(S_CarConnectionSettings_IS player)
    {
        if (!IsOwner) return;

        Destroy(player.gameObject);

        StartCoroutine(RespawnPlayer(player.OwnerClientId));
    }

    private IEnumerator RespawnPlayer(ulong ownerClientID)
    {
        yield return null;

        var startTransform = StartPoint.GetStartPos();
        var playerInstance = Instantiate(playerPrefab, startTransform.position + (Vector3.up)*2, startTransform.rotation);
            
        playerInstance.NetworkObject.SpawnAsPlayerObject(ownerClientID);

        playerInstance.GetComponent<PlayerPositionManager>().readyState = true;

    }
}
