using UnityEngine;

[System.Serializable]
public class RaycastService
{
    public Transform transform;
    public Transform Character;
    public Transform CrossHair;
    public Transform Hip;
    public Transform Base;
    public Transform StaticBase;

    public const float FrontBackMaxDistance = 10f;
    public const float BackFlipMaxDistance = 5f;
    public const float SideDashMaxDistance = 8.5f;

    // private float m_rayFrontHitDistance, m_raySideHitDistance;
    private bool m_rayFrontColliding, m_raySideColliding;

    private bool[] m_frontAndSideCollidingArray = new bool[5];

    public Animator Target { get; private set; }
    public bool TargetColliding { get; private set; }
    public float TargetDistance
    {
        get { if (Target != null) { return Vector3.Distance(Character.position, Target.transform.position); } else { return 0f; } }
    }

    private RaycastHit m_hit;
    private string m_rayCastHitTag;
    private Vector3 forward, side, frontForward, frontSide, backStepForward, backStepSide, backFlipForward, backFlipSide, sideDashForward,
                    sideDashSide, targetForward, targetSide;
    private Vector3 m_frontRay, m_sideRay, m_frontLeftRay, m_frontRightRay, m_crossHairFront, m_crossHairSide;
    // public  Vector3 HitDirection { get; private set; }

    public bool IsFacingTowardsObstacle
    {
        get
        {
            if (m_rayCastHitTag == "HigherGround")
            {
                for (int i = 0; i < m_frontAndSideCollidingArray.Length; i++) { if (m_frontAndSideCollidingArray[i] == true) { return true; } }
            }
            return false;
        }
    }
    public bool IsOnHigherGround { get; private set; }
    private bool m_crossHairColliding; public  bool CrosshairColliding { get { return m_crossHairColliding; } }
    private bool m_frontColliding; public bool FrontColliding { get { return m_frontColliding; } }
    private bool m_frontRightColliding; public bool FrontRightColliding { get { return m_frontRightColliding; } }
    private bool m_frontLeftColliding; public bool FrontLeftColliding { get { return m_frontLeftColliding; } }
    private bool m_rightColliding; public bool RightColliding { get { return m_rightColliding; } }
    private bool m_leftColliding;  public bool LeftColliding { get { return m_leftColliding; } }
    private bool m_backColliding; public bool BackColliding { get { return m_backColliding; } }
    private bool m_backRightColliding; public bool BackRightColliding { get { return m_backRightColliding; } }
    private bool m_backLeftColliding; public bool BackLeftColliding { get { return m_backLeftColliding; } }

    // Raycast for detecting nearby obstacles
    public void RayCastColliders(bool debug, int layerMask)
    {
        m_frontAndSideCollidingArray[0] = FrontColliding;
        m_frontAndSideCollidingArray[1] = FrontRightColliding;
        m_frontAndSideCollidingArray[2] = FrontLeftColliding;
        m_frontAndSideCollidingArray[3] = RightColliding;
        m_frontAndSideCollidingArray[4] = LeftColliding;

        m_frontRay = transform.forward * 1.775f;
        m_sideRay = transform.right * 1.775f;
        m_frontLeftRay = (transform.forward - transform.right) * 1.25f;
        m_frontRightRay = (transform.forward + transform.right) * 1.25f;
        float[] frontDirections = new float[] { m_frontRay.z, -m_frontRay.z, m_sideRay.z, -m_sideRay.z };
        float[] backDirections = new float[] { -m_frontRay.z, m_frontRay.z, -m_sideRay.z, m_sideRay.z };
        float[] rightDirections = new float[] { m_sideRay.z, -m_sideRay.z, m_frontRay.z, -m_frontRay.z };
        float[] leftDirections = new float[] { -m_sideRay.z, m_sideRay.z, -m_frontRay.z, m_frontRay.z };
        float[] frontLeftDirections = new float[] { m_frontLeftRay.z, -m_frontLeftRay.z, m_frontRightRay.z, -m_frontRightRay.z };
        float[] backRightDirections = new float[] { -m_frontLeftRay.z, m_frontLeftRay.z, -m_frontRightRay.z, m_frontRightRay.z };
        float[] frontRightDirections = new float[] { m_frontRightRay.z, -m_frontRightRay.z, m_frontLeftRay.z, -m_frontLeftRay.z };
        float[] backLeftDirections = new float[] { -m_frontRightRay.z, m_frontRightRay.z, -m_frontLeftRay.z, m_frontLeftRay.z };

        CreateRayCast(ref m_frontColliding, ref m_backColliding, new string[] { "HigherGround", "EnemyHitBound" }, layerMask, m_frontRay, frontDirections, backDirections, debug, 1.775f, Color.red);
        CreateRayCast(ref m_rightColliding, ref m_leftColliding, new string[] { "HigherGround", "EnemyHitBound" }, layerMask, m_sideRay, rightDirections, leftDirections, debug, 1.775f, Color.green);
        CreateRayCast(ref m_frontLeftColliding, ref m_backRightColliding, new string[] { "HigherGround", "EnemyHitBound" }, layerMask, m_frontLeftRay, frontLeftDirections, backRightDirections, debug, 1.75f, Color.magenta);
        CreateRayCast(ref m_frontRightColliding, ref m_backLeftColliding, new string[] { "HigherGround", "EnemyHitBound" }, layerMask, m_frontRightRay, frontRightDirections, backLeftDirections, debug, 1.75f, Color.blue);
    }

    // Raycast for aqcuiring attack target
    public void RaycastTarget(float distance, string targetTag, bool debug)
    {
        targetForward = CrossHair.forward * distance;
        targetSide = CrossHair.right * distance;
        float[] targetDirection = { targetForward.z, -targetForward.z, targetSide.z, -targetSide.z };
        Vector3 originPosition;
        if (targetTag == "EnemyHitBound")
        {
            originPosition = new Vector3(StaticBase.position.x, StaticBase.position.y + 0.75f, StaticBase.position.z);
        }
        else { originPosition = transform.position; }
        if (IsRayCastColliding(new Ray(originPosition, targetForward), targetTag, targetDirection, distance, debug, new Color32(150, 255, 200, 255)))
        {
            TargetColliding = true;
            if (Target == null || m_hit.transform.root.GetComponent<Animator>() != Target)
            {
                Target = m_hit.transform.root.GetComponent<Animator>(); // Debug.Log(m_hit.transform.root.GetComponent<Animator>().ToString());
            }
        }
        else { TargetColliding = false; }
    }
    public void CrossHairRaycast(float distance, string hitTag, bool debug)
    {
        m_crossHairFront = CrossHair.forward * distance;
        m_crossHairSide = CrossHair.right * distance;
        float[] crossHairDirections = new float[] { m_crossHairFront.z, -m_crossHairFront.z, m_crossHairSide.z, -m_crossHairSide.z };
        if (hitTag == "EnemyHitBound")
        {
            CrossHair.position = new Vector3(StaticBase.position.x, StaticBase.position.y + 1, StaticBase.position.z);
        }
        m_crossHairColliding = IsRayCastColliding(new Ray(CrossHair.position, m_crossHairFront), hitTag, crossHairDirections, distance, debug, Color.red);
    }

    // Raycast Advance for dash movements
    private void UpdateRayAdvanceDirections()
    {
        forward = transform.forward; side = transform.right;

        frontForward = forward * FrontBackMaxDistance;
        frontSide = side * FrontBackMaxDistance;
        backStepForward = forward * FrontBackMaxDistance;
        backStepSide = side * FrontBackMaxDistance;
        backFlipForward = forward * BackFlipMaxDistance;
        backFlipSide = side * BackFlipMaxDistance;
        sideDashForward = forward * SideDashMaxDistance;
        sideDashSide = side * SideDashMaxDistance;
    }
    public void RayCastAdvanceUpdate(Animator animator, int layerMask, bool debug)
    {
        UpdateRayAdvanceDirections();
        if (AnimatorUtility.GetCurrentStateHash(animator) == AnimatorUtility.PS.dashState || AnimatorUtility.GetCurrentStateHash(animator) == AnimatorUtility.PS.dashBreakState)
        {
            RayCastAdvance(frontForward, frontSide, FrontBackMaxDistance, false, false, layerMask, debug, frontForward.z, -frontForward.z, frontSide.z, -frontSide.z);
        }
        else if (AnimatorUtility.GetCurrentStateHash(animator) == AnimatorUtility.PS.dashStateC3 || AnimatorUtility.GetCurrentStateHash(animator) == AnimatorUtility.PS.dashBreakStateC3 || AnimatorUtility.GetCurrentStateHash(animator) == AnimatorUtility.PS.dashStateC7 || AnimatorUtility.GetCurrentStateHash(animator) == AnimatorUtility.PS.dashBreakStateC7)
        {
            RayCastAdvance(frontForward, frontSide, 8, false, false, layerMask, debug, frontForward.z, -frontForward.z, frontSide.z, -frontSide.z);
        }
        else if (AnimatorUtility.GetCurrentStateHash(animator) == AnimatorUtility.PS.dashStateC6 || AnimatorUtility.GetCurrentStateHash(animator) == AnimatorUtility.PS.dashBreakStateC6)
        {
            RayCastAdvance(frontForward, frontSide, 7, false, false, layerMask, debug, frontForward.z, -frontForward.z, frontSide.z, -frontSide.z);
        }

        else if (AnimatorUtility.GetCurrentStateHash(animator) == AnimatorUtility.PS.backStepState || AnimatorUtility.GetCurrentStateHash(animator) == AnimatorUtility.PS.backStepBreakState)
        {
            RayCastAdvance(backStepForward, backStepSide, FrontBackMaxDistance, false, true, layerMask, debug, -backStepForward.z, backStepForward.z, -backStepSide.z, backStepSide.z);
        }
        else if (AnimatorUtility.GetCurrentStateHash(animator) == AnimatorUtility.PS.backFlipState)
        {
            RayCastAdvance(backFlipForward, backFlipSide, BackFlipMaxDistance, false, true, layerMask, debug, -backFlipForward.z, backFlipForward.z, -backFlipSide.z, backFlipSide.z);
        }
        else if (AnimatorUtility.GetCurrentStateHash(animator) == AnimatorUtility.PS.dashLeftState)
        {
            RayCastAdvance(sideDashForward, sideDashSide, SideDashMaxDistance, true, true, layerMask, debug, sideDashSide.z, -sideDashSide.z, sideDashForward.z, -sideDashForward.z);
        }
        else if (AnimatorUtility.GetCurrentStateHash(animator) == AnimatorUtility.PS.dashRightState)
        {
            RayCastAdvance(sideDashForward, sideDashSide, SideDashMaxDistance, true, false, layerMask, debug, -sideDashSide.z, sideDashSide.z, -sideDashForward.z, sideDashForward.z);
        }
        else { m_rayFrontColliding = false; m_raySideColliding = false; }
    }
    public bool IsRayAdvanceColliding(Animator animator, out RaycastHit hitInfo)
    {
        if (Hip.GetComponent<TransformManipulator>().DashLock) { hitInfo = default(RaycastHit); return false; }
        else if (AnimatorUtility.GetCurrentStateHash(animator) == AnimatorUtility.PS.dashLeftState || AnimatorUtility.GetCurrentStateHash(animator) == AnimatorUtility.PS.dashRightState)
        {
            hitInfo = m_hit; if (m_raySideColliding) { return m_raySideColliding; }
        }
        else { hitInfo = m_hit; if (m_rayFrontColliding) { return m_rayFrontColliding; } }
        return false;
    }

    // LineCast On Ground to adjust player's transform.position.y
    public void LineCastOnGround(Animator animator, bool debug)
    {
        RaycastHit lineCastHit;
        var StartPoint = new Vector3(Character.position.x, transform.position.y, Character.position.z);
        var EndPoint = new Vector3(Character.position.x, -0.5f, Character.position.z);
        if (debug) { Debug.DrawLine(StartPoint, EndPoint); }
        Base.position = new Vector3(Base.position.x, Hip.position.y - 1.22f, Base.position.z);

        if (Physics.Linecast(StartPoint, EndPoint, out lineCastHit))
        {
            bool IsSideJogging = AnimatorUtility.GetCurrentStateHash(animator) == AnimatorUtility.PS.jogState && PlayerState.Vertical == 1 && (PlayerState.Horizontal == 1 || PlayerState.Horizontal == -1);
            if (StaticBase.position.y < Base.localPosition.y && !IsSideJogging && AnimatorUtility.GetCurrentStateHash(animator) != AnimatorUtility.PS.backFlipState)
            {
                Character.position = new Vector3(Character.position.x, Base.localPosition.y, Character.position.z);
            }
            // if ((SkillState.SkillSetNumber == 12 || SkillState.SkillSetNumber == 13 || SkillState.SkillSetNumber == 14 || SkillState.SkillSetNumber == 15)) { return; }
            if (lineCastHit.transform.tag == "HigherGround")
            {
                IsOnHigherGround = true;
                if (Base.localPosition.y <= 0.125f && Character.position.y != lineCastHit.transform.lossyScale.y)
                {
                    Character.position = new Vector3(Character.position.x, Mathf.MoveTowards(Character.position.y, lineCastHit.transform.lossyScale.y, Time.deltaTime * 50f), Character.position.z);
                }
                else if (Mathf.Abs(Character.position.y - lineCastHit.transform.lossyScale.y) < 0.075f)
                {
                    Character.position = new Vector3(Character.position.x, lineCastHit.transform.lossyScale.y, Character.position.z);
                }
                StaticBase.position = new Vector3(StaticBase.position.x, lineCastHit.transform.lossyScale.y, StaticBase.position.z);
            }
            if (lineCastHit.transform.tag == "Terrain")
            {
                IsOnHigherGround = false;
                if (Base.localPosition.y <= 0.125f && Character.position.y != lineCastHit.transform.position.y)
                {
                    Character.position = new Vector3(Character.position.x, Mathf.MoveTowards(Character.position.y, lineCastHit.transform.position.y, Time.deltaTime * 50f), Character.position.z);
                }
                else if (Mathf.Abs(Character.position.y - lineCastHit.transform.position.y) < 0.075f)
                {
                    Character.position = new Vector3(Character.position.x, lineCastHit.transform.position.y, Character.position.z);
                }
                StaticBase.position = new Vector3(StaticBase.position.x, lineCastHit.transform.position.y, StaticBase.position.z);
            }

        }
    }

    private void CreateRayCast(ref bool colliding1, ref bool colliding2, string[] hitTag, int layerMask, params object[] info)
    {
        colliding1 = IsRayCastColliding(new Ray(transform.position, (Vector3)info[0]), hitTag, layerMask, (float[])info[1], (float)info[4], (bool)info[3], (Color)info[5]);
        colliding2 = IsRayCastColliding(new Ray(transform.position, -(Vector3)info[0]), hitTag, layerMask, (float[])info[2], (float)info[4], (bool)info[3], (Color)info[5]);
    }

    private bool IsRayCastColliding(Ray ray, string[] hitTag, int layerMask, float[] direction, float rayDistance, bool debug, Color color)
    {
        if (debug) { Debug.DrawRay(ray.origin, ray.direction * rayDistance, color); }
        if (Physics.Raycast(ray, out m_hit, direction[0], layerMask) || Physics.Raycast(ray, out m_hit, direction[1], layerMask) || Physics.Raycast(ray, out m_hit, direction[2], layerMask) || Physics.Raycast(ray, out m_hit, direction[3], layerMask))
        {
            for (int i = 0; i < hitTag.Length; i++) { if (m_hit.transform.tag == hitTag[i]) { m_rayCastHitTag = m_hit.transform.tag; return true; } }
        }
        return false;
    }

    private bool IsRayCastColliding(Ray ray, string hitTag, float[] direction, float rayDistance, bool debug, Color color)
    {
        if (debug) { Debug.DrawRay(ray.origin, ray.direction * rayDistance, color); }
        if (Physics.Raycast(ray, out m_hit, direction[0]) || Physics.Raycast(ray, out m_hit, direction[1]) || Physics.Raycast(ray, out m_hit, direction[2]) || Physics.Raycast(ray, out m_hit, direction[3]))
        {
            if (m_hit.transform.tag == hitTag) { m_rayCastHitTag = m_hit.transform.tag; return true; }
        }
        return false;
    }

    private void RayCastAdvance(Vector3 rayForward, Vector3 raySide, float rayDistance, bool isSideDirection, bool isInverseDirection, int layerMask, bool debug, params float[] direction)
    {
        if (!isInverseDirection && !isSideDirection)
        {
            m_rayFrontColliding = IsRayCastColliding(new Ray(transform.position, rayForward), new string[] { "HigherGround", "EnemyHitBound" }, layerMask, direction, rayDistance, debug, Color.yellow);
            // if (m_hit.transform != null) { /*HitDirection = m_hit.normal;*/ }
        }
        else if (isInverseDirection && !isSideDirection)
        {
            m_rayFrontColliding = IsRayCastColliding(new Ray(transform.position, -rayForward), new string[] { "HigherGround", "EnemyHitBound" }, layerMask, direction, rayDistance, debug, Color.yellow);
            // if (m_hit.transform != null) { /*HitDirection = m_hit.normal;*/ }
        }
        else if (!isInverseDirection && isSideDirection)
        {
            m_raySideColliding = IsRayCastColliding(new Ray(transform.position, raySide), new string[] { "HigherGround", "EnemyHitBound" }, layerMask, direction, rayDistance, debug, new Color32(255, 128, 0, 255));
            // if (m_hit.transform != null) { /*HitDirection = m_hit.normal;*/ }
        }
        else if (isInverseDirection && isSideDirection)
        {
            m_raySideColliding = IsRayCastColliding(new Ray(transform.position, -raySide), new string[] { "HigherGround", "EnemyHitBound" }, layerMask, direction, rayDistance, debug, new Color32(255, 128, 0, 255));
            // if (m_hit.transform != null) { /*HitDirection = m_hit.normal;*/ }
        }
    }
}