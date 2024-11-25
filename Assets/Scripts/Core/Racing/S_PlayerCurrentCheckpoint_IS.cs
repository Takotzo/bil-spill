using Unity.Netcode;
using UnityEngine;

public class S_PlayerCurrentCheckpoint_IS : NetworkBehaviour
{
    public NetworkVariable<int> currentCheckpoint = new NetworkVariable<int>();
    public NetworkVariable<int> currentLap = new NetworkVariable<int>();
    
    [SerializeField] private int checkpointInBetween;
    private int _totalCheckpoint;
    
    public override void OnNetworkSpawn()
    {
        if (!IsServer) {return;}
        currentCheckpoint.Value = 0;
        currentLap.Value = 0;
    }
    
    public void SetCurrentCheckpoint(int id)
    {
        if (!IsServer) {return;}
        if (id >= currentCheckpoint.Value && id <= currentCheckpoint.Value + checkpointInBetween || id <= currentCheckpoint.Value && id >= currentCheckpoint.Value - checkpointInBetween)
        {
            currentCheckpoint.Value = id;
        }

        if (id != 0 || currentCheckpoint.Value < _totalCheckpoint - 1 - checkpointInBetween) return;
        currentLap.Value++;
        currentCheckpoint.Value = 0;
    }
    
    public void SetCurrentLap(int lap, int totalCheckpoints)
    {
        if (!IsServer) {return;}
        if (currentCheckpoint.Value <= totalCheckpoints - 1 && currentCheckpoint.Value >= (totalCheckpoints - 1) - checkpointInBetween)
        {
            currentCheckpoint.Value = 0;
            currentLap.Value = lap;
        }
    }
}
