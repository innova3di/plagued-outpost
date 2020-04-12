using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

using System.Linq;

public class Notification : MonoBehaviour
{
    public Transform currentTrans;
    public Transform previousTrans;
    public Transform oldestTrans;
    public GameObject textNotification;

    internal Queue<GameObject> m_notificationQueue = new Queue<GameObject>();

    public void Push(string notification)
    {
        if (m_notificationQueue.Count == 0)
        {
            textNotification.GetComponent<Text>().text = notification;
            m_notificationQueue.Enqueue(Instantiate(textNotification, transform));

            StartCoroutine(NotificationScroll(m_notificationQueue.ElementAt(0), currentTrans.localPosition, 3.0f, 1, 30));
        }
        else if (m_notificationQueue.Count == 1)
        {
            textNotification.GetComponent<Text>().text = notification;

            StopAllCoroutines();

            StartCoroutine(NotificationScroll(m_notificationQueue.ElementAt(0), previousTrans.localPosition, 1.75f, 4, 25));
            Invoke("CurrentSecondNotif", 0.05f);
        }
        else if (m_notificationQueue.Count == 2)
        {
            textNotification.GetComponent<Text>().text = notification;

            StopAllCoroutines();

            StartCoroutine(NotificationScroll(m_notificationQueue.ElementAt(0), oldestTrans.localPosition, 0.125f, 1, 20));
            StartCoroutine(NotificationScroll(m_notificationQueue.ElementAt(1), previousTrans.localPosition, 1.75f, 4, 25));
            Invoke("CurrentThirdNotif", 0.05f);
        }
    }

    private void CurrentSecondNotif()
    {
        m_notificationQueue.Enqueue(Instantiate(textNotification, transform));
        StartCoroutine(NotificationScroll(m_notificationQueue.Last(), currentTrans.localPosition, 3.0f, 1, 30));
    }

    private void CurrentThirdNotif()
    {
        m_notificationQueue.Enqueue(Instantiate(textNotification, transform));
        StartCoroutine(NotificationScroll(m_notificationQueue.Last(), currentTrans.localPosition, 3.0f, 1, 30));
    }

    private IEnumerator NotificationScroll(GameObject notif, Vector3 destPos, float secondsToDisplay, int colorDivisor, int targetFontSize)
    {
        float currentSeconds = 0;

        while (currentSeconds < secondsToDisplay)
        {
            if (notif != null)
            {
                if (Vector3.Distance(notif.transform.localPosition, destPos) > 0.25f)
                {
                    notif.transform.localPosition = Vector3.Lerp(notif.transform.localPosition, destPos, Time.deltaTime * 5f);
                }
                else if (Vector3.Distance(notif.transform.localPosition, destPos) < 0.25f)
                {
                    notif.transform.localPosition = destPos;
                }

                notif.GetComponent<Text>().fontSize = (int)Mathf.MoveTowards(notif.GetComponent<Text>().fontSize, targetFontSize, Time.deltaTime * 5f);

                notif.GetComponent<Text>().color = new Color(255, 255, 255, (secondsToDisplay - currentSeconds) / colorDivisor);

                currentSeconds += Time.deltaTime;
            }
            yield return null;
        }
        Destroy(m_notificationQueue.Dequeue());
    }
}