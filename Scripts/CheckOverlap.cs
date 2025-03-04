using UnityEngine;

public class CheckOverlap : MonoBehaviour
{
    //private void FixedUpdate()
    //{
    //    if (IsEntryPointOverlapping())
    //    {
    //        Debug.Log("Entry Point is overlapping!");
    //    }
    //}
    public bool IsEntryPointOverlapping()
    {
        Collider hitCollider = Physics.OverlapSphere(transform.position, 0.5f)[0];


        if (hitCollider.gameObject.CompareTag("Chunk"))
        {
            return true;
        }
        return false;
    }
}
