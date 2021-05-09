using UnityEngine;
using System.Collections;

public class OpenSpaceDetector : MonoBehaviour
{
    void Start()
    {

    }

    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        transform.GetComponentInParent<EnemyZone>().isOpenSpace = false;
        //Destroy(gameObject);

        Debug.Log("Position already occupied.. ");
    }


}