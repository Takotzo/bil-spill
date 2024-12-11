using System;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using Unity.Netcode;
using UnityEngine;

public class PlayerPositionManager : NetworkBehaviour
{
    public bool readyState = false;
    private static int playerReadyCount = 0;

    private MovePlayer movePlayer;
    private S_CarConnectionSettings_IS carPlayer;
    private static List<PlayerPositionManager> players = new List<PlayerPositionManager>();
    private void OnEnable()
    {
        players.Add(this);
    }

    private void Start()
    {
        movePlayer = FindObjectOfType<MovePlayer>();
        
        carPlayer = gameObject.GetComponent<S_CarConnectionSettings_IS>();
    }

    [Rpc(SendTo.Everyone)]
    public void SetPlayerPositionClientRpc(ulong clientIdToCompare)
    {
        if (clientIdToCompare != carPlayer.OwnerClientId) return;
        
        
        
        movePlayer.HandlePlayerMovement(carPlayer);
    }
    

    private void OnRespawnInput()
    {
        SetReadyStatus();
    }

    

    // Button call 
    private void SetReadyStatus()
    {
        if (!IsOwner) return;
        if (readyState) return;
        readyState = true;
        
        TotalPlayerReadyServerRpc(carPlayer.OwnerClientId);
        
    }
    
    
    
    [Rpc(SendTo.Server)]
    private void TotalPlayerReadyServerRpc(ulong clientId)
    {
        if (!IsServer) return;


        // Check is all players are ready
        //if (playerReadyCount != players.Count) return;
        
        
        // Assign players to StartPosition
        foreach (var t in players)
        {
            print("Send player to startline");
            t.SetPlayerPositionClientRpc(clientId);
        }
    }


    private void OnDisable()
    {
        players.Remove(this);
    }
}
