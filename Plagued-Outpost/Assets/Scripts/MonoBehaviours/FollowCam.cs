using UnityEngine;

public class FollowCam : MonoBehaviour
{
    public Transform player;       
    private Vector3 m_offset;         

    void Start()
    {
        m_offset = transform.position - player.position;
    }

    void LateUpdate()
    {
        transform.position = player.position + m_offset;
    }
}