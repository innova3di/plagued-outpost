using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class LerpExperiment : MonoBehaviour
{
    public float speed;
    public Transform focus;
    public RotateMode rotateMode;
    public enum RotateMode
    {
        LERP           = 0,
        SLERP          = 1,
        ROTATE_TOWARDS = 2,
    }
    public MovementMode movementMode;
    public enum MovementMode
    {
        LERP = 0,
        SLERP = 1,
        MOVE_TOWARDS = 2,
    }
    public Transform destination;

    private float m_startTime;
    private float m_totalDistToDest;

    private bool m_startExp;
    private Vector3 m_initPosition;
    private Quaternion m_initRotation;
    private Vector3 m_lookPos;
    private Quaternion m_targetRotation;

    public float xSpeed = 150.0f;
    public float ySpeed = 150.0f;

    public float yMinLimit;
    public float yMaxLimit;

    private float m_vertical;
    private float m_horizontal;

    void Start()
    {
        m_startTime = Time.time;
        m_totalDistToDest = Vector3.Distance(transform.position, destination.position);
        Debug.Log("Scene is now running.. ");
        m_initPosition = transform.position;
        m_initRotation = transform.rotation;
    }

    void Update()
    {

        if (Input.GetKeyUp(KeyCode.Space)) { m_startExp = !m_startExp; }
        if (m_startExp)
        {
            // FaceTarget(rotateMode);
            Move(movementMode);
        }
        else if (!m_startExp)
        {
            transform.position = m_initPosition;
            transform.rotation = m_initRotation;
        }
    }

    private void RotateAround()
    {
        m_vertical -= Input.GetAxis("Mouse Y") * ySpeed;
        m_horizontal += Input.GetAxis("Mouse X") * xSpeed;
        m_vertical = Mathf.Clamp(m_vertical, yMinLimit, yMaxLimit);
        transform.rotation = Quaternion.Euler(m_vertical, focus.eulerAngles.y, 0);
        focus.rotation = Quaternion.Euler(0, m_horizontal, 0);
        transform.position = transform.rotation * new Vector3(0, 0.0f, -transform.position.z) + focus.position;
    }

    private void Move(MovementMode movementMode)
    {
        if (movementMode == MovementMode.LERP)
        {
            if (Mathf.Abs(Vector3.Distance(transform.position, focus.position)) > 0.75f)
            {
                transform.position = Vector3.Lerp(transform.position, focus.position, speed);
            }
            else if (transform.position != focus.position)
            {
                transform.position = focus.position;
                Debug.Log("Focus reached.. " + Time.deltaTime);
            }
        }
        else if (movementMode == MovementMode.SLERP)
        {
            if (Mathf.Abs(Vector3.Distance(transform.position, focus.position)) > 0.75f)
            {
                transform.position = Vector3.Slerp(transform.position, focus.position, speed);
            }
            else if (transform.position != focus.position)
            {
                transform.position = focus.position;
                Debug.Log("Focus reached.. " + Time.deltaTime);
            }
        }
        else if (movementMode == MovementMode.MOVE_TOWARDS)
        {
            if (Mathf.Abs(Vector3.Distance(transform.position, focus.position)) > 0.75f)
            {
                transform.position = Vector3.Lerp(transform.position, focus.position, Time.deltaTime * 13);
                // transform.position = Vector3.MoveTowards(transform.position, focus.position, Time.deltaTime * 13);
            }
            else if (transform.position != focus.position)
            {
                transform.position = focus.position;
                Debug.Log("Focus reached.. " + Time.deltaTime);
            }
            // transform.position = Vector3.MoveTowards(transform.position, focus.position, speed);
        }
    }

    private void FaceTarget(RotateMode rotateMode)
    {
        m_lookPos = (destination.transform.position - transform.position); m_lookPos.y = -3f;
        m_targetRotation = Quaternion.LookRotation(m_lookPos);
        if (Mathf.Abs(Quaternion.Angle(transform.rotation, m_targetRotation)) >= 0.75f)
        {
            if (rotateMode == RotateMode.LERP)
            {
                transform.rotation = Quaternion.Lerp(transform.rotation, m_targetRotation, (Time.deltaTime * speed));
            }
            else if (rotateMode == RotateMode.SLERP)
            {
                transform.rotation = Quaternion.Slerp(transform.rotation, m_targetRotation, (Time.deltaTime * speed));
            }
            else if (rotateMode == RotateMode.ROTATE_TOWARDS)
            {
                transform.rotation = Quaternion.RotateTowards(transform.rotation, m_targetRotation, (Time.deltaTime * speed));
            }
        }
        else
        {
            transform.rotation = m_targetRotation;
            Debug.Log("Dodge target finished.. " + Time.deltaTime);
        }
    }

    private void LerpTowards(bool debug)
    {
        float currentDuration = Time.time - m_startTime;
        float traveledFraction = currentDuration / m_totalDistToDest;
        transform.position = Vector3.Lerp(transform.position, destination.position, traveledFraction * 2);

        if (transform.position != destination.position && debug)
        {
            Debug.Log(string.Format("Duration = {0}, Traveled = {1}", currentDuration, traveledFraction));
        }
    }
}
