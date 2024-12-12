using Unity.Netcode;
using UnityEngine;

public class S_ScuffedMovingOfPlayer_IS : NetworkBehaviour
{
    private static readonly int RaceFinished = Animator.StringToHash("RaceFinished");
    [SerializeField] private PrometeoCarController carController;
    [SerializeField] private Rigidbody rigidbody;
    [SerializeField] private Animator animator;
    public void MovePlayer()
    {
        carController.ResetInputs();
        animator.SetTrigger(RaceFinished);
        carController.enabled = false;
        transform.position = new Vector3(0, 3, 0);
        transform.rotation = Quaternion.identity;
        rigidbody.isKinematic = true;
    }
}
