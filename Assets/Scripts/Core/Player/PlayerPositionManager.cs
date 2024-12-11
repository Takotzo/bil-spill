using Unity.Netcode;
using UnityEngine;

public class PlayerPositionManager : NetworkBehaviour
{
    bool readyState = false;
    private static int playerReadyCount = 0;
    
    [Rpc(SendTo.NotServer)]
    private void SetPlayerPositionClientRpc(Vector3 position)
    {
        if (!IsOwner) return;
        
        transform.position = position;
    }

    // Button call 
    public void SetReadyStatus()
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
        var players = GameObject.FindGameObjectsWithTag("Player");

        // Check is all players are ready
        if (playerReadyCount != players.Length) return;
        
        
        var positions = StartPoint.StartPoints;
        // Assign players to StartPosition
        for (int j = 0; j < players.Length; j++)
        {
            players[j].GetComponent<PlayerPositionManager>().SetPlayerPositionClientRpc(positions[j].transform.position);
        }
    }
}
