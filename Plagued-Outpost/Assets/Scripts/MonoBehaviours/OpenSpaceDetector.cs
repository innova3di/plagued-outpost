using UnityEngine;
using EventSystem;
using System.Collections;

public class OpenSpaceDetector : MonoBehaviour
{
    private SpaceLocatorEvent m_spaceLocatorEvent;

    void Start()
    {
        m_spaceLocatorEvent = new SpaceLocatorEvent();
        m_spaceLocatorEvent.occupiedSpaceEvent += notifyParent;
    }

    void Update()
    {

    }

    private void notifyParent()
    {
        string parentName = gameObject.name.Replace("-nextPos", "");

        Debug.Log(parentName);
        GameObject parentObj = GameObject.Find(parentName);

        EnemyZone parentZone = parentObj.GetComponentInChildren<EnemyZone>();

        parentZone.setOpenSpace = false;
        parentZone.foundDestination = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.name != "EnemyDetector" && other.name != "AttackZone")
        {
            Debug.Log("Position already occupied.. " + other.name);
            m_spaceLocatorEvent.occupiedSpaceDetected();

            Destroy(gameObject);
        }
    }


}