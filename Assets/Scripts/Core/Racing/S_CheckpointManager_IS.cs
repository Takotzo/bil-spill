using System;
using UnityEngine;

public class S_CheckpointManager_IS : MonoBehaviour
{
    [SerializeField] private int totalLapCount;
    [SerializeField] private int currentLap;
    [SerializeField] private S_Checkpoint_IS[] checkpoints;
    [SerializeField] private int currentCheckpoint;
    [SerializeField] private int checkpointInBetween;
    
    private void Start()
    {
        for (int i = 0; i < checkpoints.Length; i++)
        {
            checkpoints[i].SetID(i);
        }
    }

    public void PassStart()
    {
        if (currentCheckpoint <= checkpoints.Length - 1 && currentCheckpoint >= (checkpoints.Length - 1) - checkpointInBetween)
        {
            currentCheckpoint = 0;
        }
    }
}
