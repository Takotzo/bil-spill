using System;
using UnityEngine;

public class S_CheckpointManager_IS : MonoBehaviour
{
    [SerializeField] private int totalLapCount;
    [SerializeField] private S_Checkpoint_IS[] checkpoints;
    
    private void Start()
    {
        for (int i = 0; i < checkpoints.Length; i++)
        {
            checkpoints[i].SetID(i);
        }
        
        checkpoints[0].SetTotalCheckpoint(checkpoints.Length);
    }

    // public void PassStart()
    // {
    //     if (currentCheckpoint <= checkpoints.Length - 1 && currentCheckpoint >= (checkpoints.Length - 1) - checkpointInBetween)
    //     {
    //         currentCheckpoint = 0;
    //     }
    // }
}
