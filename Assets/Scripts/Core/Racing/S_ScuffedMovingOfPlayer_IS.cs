using Unity.Netcode;
using UnityEngine;

public class S_ScuffedMovingOfPlayer_IS : NetworkBehaviour
{
    private static readonly int RaceFinished = Animator.StringToHash("RaceFinished");
    [SerializeField] private Animator animator;
    [Rpc(SendTo.Everyone)]
    public void MovePlayerRpc()
    {
        if (!IsOwner) return;
        animator.SetTrigger(RaceFinished);
    }
}
