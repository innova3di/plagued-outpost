using UnityEngine;

public class EnemyDetector : MonoBehaviour
{
    public Animator EnemyAnimator { get; private set; }
    public void OnTriggerEnter(Collider other)
    {
        if (other.name == "Hips" && other.tag == "Enemy")
        {
            EnemyAnimator = other.transform.root.GetComponent<Animator>();
        }
    }
}
