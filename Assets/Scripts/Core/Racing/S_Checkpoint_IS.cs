using UnityEngine;

public class S_Checkpoint_IS : MonoBehaviour
{
    private int _id;

    private bool _isGoal;
    
    private int _totalCheckpoint;
    
    public void SetID(int id)
    {
        gameObject.name = "Checkpoint " + id;
        _id = id;
        if (id == 0)
        {
            _isGoal = true;
        }
    }
    
    public void SetTotalCheckpoint(int totalCheckpoint)
    {
        _totalCheckpoint = totalCheckpoint;
    }
    
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (!_isGoal)
            {
                other.GetComponent<S_PlayerCurrentCheckpoint_IS>().SetCurrentCheckpoint(_id);
            }
            else
            {
                other.GetComponent<S_PlayerCurrentCheckpoint_IS>().SetCurrentLap(other.GetComponent<S_PlayerCurrentCheckpoint_IS>().currentLap.Value + 1, _totalCheckpoint);
            }
        }
    }
}
