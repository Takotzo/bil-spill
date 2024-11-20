using UnityEngine;

public class S_Checkpoint_IS : MonoBehaviour
{
    private int _id;
    
    public void SetID(int id)
    {
        gameObject.name = "Checkpoint " + id;
        _id = id;
    }
    
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            other.GetComponent<S_PlayerCurrentCheckpoint_IS>().SetCurrentCheckpoint(_id);
        }
    }
}
