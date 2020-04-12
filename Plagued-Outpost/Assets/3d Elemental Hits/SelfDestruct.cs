using UnityEngine;

public class SelfDestruct : MonoBehaviour
{
    private void OnEnable()
    {
        Invoke("Deactivate", 1.5f);
    }

    private void OnDisable()
    {
        CancelInvoke();
    }

    private void Deactivate()
    {
        gameObject.SetActive(false);
    }
}
