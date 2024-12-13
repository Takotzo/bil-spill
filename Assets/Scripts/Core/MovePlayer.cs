using System;
using System.Collections;
using Core;
using Unity.Mathematics;
using Unity.Netcode;
using UnityEngine;

public class MovePlayer : NetworkBehaviour
{
    [SerializeField] private S_CarConnectionSettings_IS playerPrefab;
    
    int numbOfPlayers;
    int readyPlayers = 0;

    public void HandlePlayerMovement(S_CarConnectionSettings_IS player, int players)
    {
        if (!IsOwner) return;

        Destroy(player.gameObject);

        StartCoroutine(RespawnPlayer(player.OwnerClientId));
        
        numbOfPlayers = players;
    }

    private IEnumerator RespawnPlayer(ulong ownerClientID)
    {
        yield return null;

        var startTransform = StartPoint.GetStartPos();
        var playerInstance = Instantiate(playerPrefab, startTransform.position + (Vector3.up)*2, startTransform.rotation);
            
        playerInstance.NetworkObject.SpawnAsPlayerObject(ownerClientID);
        playerInstance.NetworkObject.ChangeOwnership(ownerClientID);


        playerInstance.GetComponent<PlayerPositionManager>().readyState = true;
        playerInstance.GetComponent<PrometeoCarController>().enabled = false;
        playerInstance.GetComponent<PlayerPositionManager>().TurnOffVehicleRpc();
        readyPlayers++;

        
        
        if (readyPlayers >= numbOfPlayers)
        {
            yield return new WaitForSeconds(3f);
            StartRace();
        }
    }

    
    private void StartRace()
    {
        var players = FindObjectsOfType<PlayerPositionManager>();
        foreach (var player in players)
        {
            player.TurnOnVehicleRpc();
        }
    }


    public void FinishRace(PlayerPositionManager player)
    {
        print(IsOwner);
        if (!IsOwner) return;

        var ownerClientID = player.OwnerClientId;
        
        Destroy(player.gameObject);
        
        
        StartCoroutine(SendPlayersBackToSpawn(ownerClientID));

    }

    private IEnumerator SendPlayersBackToSpawn(ulong ownerClientID)
    {
        yield return null;
        var startTransform = SpawnPoint.GetSpawnPos();
        var playerInstance = Instantiate(playerPrefab, startTransform + (Vector3.up)*2, quaternion.identity);
            
        playerInstance.NetworkObject.SpawnAsPlayerObject(ownerClientID);
        playerInstance.NetworkObject.ChangeOwnership(ownerClientID);

    }
}
