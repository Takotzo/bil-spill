using System;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using Unity.Netcode;
using UnityEngine;

public class PlayerPositionManager : NetworkBehaviour
{
    bool readyState = false;
    private static int playerReadyCount = 0;

    private List<PlayerPositionManager> players = new List<PlayerPositionManager>();
    private void OnEnable()
    {
        players.Add(this);
    }

    [Rpc(SendTo.NotServer)]
    public void SetPlayerPositionClientRpc(Vector3 position)
    {
        if (!IsOwner) return;
        
        transform.position = position;
    }

    private void OnInteract()
    {
        SetReadyStatus();
    }

    private void Update()
    {
        if (UnityEngine.Input.GetKeyDown(KeyCode.E)){
            
            Debug.Log("Interact");
            SetReadyStatus();
        }
    }

    // Button call 
    private void SetReadyStatus()
    {
        if (!IsOwner) return;
        
        readyState = !readyState;
        if (readyState)
        {
            TotalPlayerReadyServerRpc(1);
        }
        else
        {
            TotalPlayerReadyServerRpc(-1);
        }
    }

    [Rpc(SendTo.Server)]
    private void TotalPlayerReadyServerRpc(int i)
    {
        if (!IsServer) return;

        playerReadyCount += i;

        // Check is all players are ready
        if (playerReadyCount != players.Count) return;
        
        
        var positions = StartPoint.startPoints;
        // Assign players to StartPosition
        for (int j = 0; j < players.Count; j++)
        {
            print(positions);
            SetPlayerPositionClientRpc(positions[j].transform.position);
        }
    }


    private void OnDisable()
    {
        players.Remove(this);
    }
}
