using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ObstructionsRenderModifier : MonoBehaviour
{
    public float transparency;
    public Transform startPosition;
    public List<int> obstructionLayers = new List<int>();

    private List<BlendMode> m_previousBlendModes = new List<BlendMode>();
    private List<Material> m_obstructionMaterials = new List<Material>();

    // private Ray m_obstructionRay;
    private RaycastHit m_obstructionHit;

    private int m_layerMask;
    private bool m_isFadeMaterialsRoutineRunning;

    public void Start()
    {
        m_layerMask = LayerMask.GetMask(LayerMask.LayerToName(9)); // Debug.Log("LayerMask = " + m_layerMask);
    }

    public void Update()
    {
        ObstructionDetector(false);
    }

    private void SetAlphaColor(List<Material> materials, float alphaValue, BlendMode blendMode)
    {
        for (int i = 0; i < materials.Count; i++)
        {
            SetupMaterialWithBlendMode(materials[i], blendMode);
            Color tempColor = materials[i].color;
            tempColor.a = alphaValue;
            materials[i].color = tempColor;
        }
    }
    private void SetAlphaColor(List<Material> materials, float alphaValue, List<BlendMode> blendModes)
    {
        for (int i = 0; i < materials.Count; i++)
        {
            SetupMaterialWithBlendMode(materials[i], blendModes[i]);
            Color tempColor = materials[i].color;
            tempColor.a = alphaValue;
            materials[i].color = tempColor;
        }
    }

    private void ObstructionDetector(bool debug)
    {
        if (debug) { Debug.DrawLine(startPosition.position, transform.position, Color.cyan); }
        // m_obstructionRay = new Ray(transform.position, (transform.root.position - transform.position).normalized * rayMaxDistance );
        // Debug.DrawRay             (transform.position, (transform.root.position - transform.position).normalized * rayMaxDistance, Color.cyan  );
        if (Physics.Linecast(startPosition.position, transform.position, out m_obstructionHit, m_layerMask))
        {
            if (m_obstructionMaterials.Count == 0)
            {
                Renderer[] renderers = m_obstructionHit.transform.GetComponentsInChildren<Renderer>();
                if (renderers.Length > 0)
                {
                    for (int i = 0; i < renderers.Length; i++)
                    {
                        Material[] materials = renderers[i].materials;
                        for (int j = 0; j < materials.Length; j++) { m_obstructionMaterials.Add(materials[j]); }
                    }
                    // Debug.Log("Obstruction Materials = " + m_obstructionMaterials.Count);
                }
                // else if (renderers.Length == 0) { Debug.Log("Failed to collect the Renderers = " + m_obstructionMaterials.Count); }
            }
            if (m_obstructionMaterials.Count > 0)
            {
                if (!m_isFadeMaterialsRoutineRunning)
                {
                    for (int i = 0; i < m_obstructionMaterials.Count; i++)
                    {
                        m_previousBlendModes.Add((BlendMode)m_obstructionMaterials[i].GetFloat("_Mode"));
                    }
                    // Debug.Log("Previous Blend Modes = " + m_previousBlendModes.Count);
                    m_fadeMaterialsRoutine = StartCoroutine(FadeMaterials(m_obstructionMaterials));
                }
            }
        }
        else if (m_obstructionMaterials.Count > 0)
        {
            ReturnToOpaque();
        }
    }

    private void ReturnToOpaque()
    {
        if (m_isFadeMaterialsRoutineRunning)
        {
            m_isFadeMaterialsRoutineRunning = false;
            if (!m_routineEnded) { StopCoroutine(m_fadeMaterialsRoutine); /* Debug.Log("Routine Interrupted.. "); */ }
        }
        SetAlphaColor(m_obstructionMaterials, 1, m_previousBlendModes);
        m_previousBlendModes.Clear();
        m_obstructionMaterials.Clear();
    }

    private bool m_routineEnded;
    private Coroutine m_fadeMaterialsRoutine;
    private IEnumerator FadeMaterials(List<Material> materials)
    {
        float alpha = 1f;
        m_routineEnded = false;
        m_isFadeMaterialsRoutineRunning = true;
        while (alpha > transparency)
        {
            alpha = Mathf.MoveTowards(alpha, transparency, Time.deltaTime);
            SetAlphaColor(materials, alpha, BlendMode.Fade); // Debug.Log("Smoothed alpha = " + m_smoothedAlpha);
            yield return null;
        }
        m_routineEnded = true; // Debug.Log("Routine Ended.. ");
        // SetAlphaColor(materials, StandardShaderGUI.BlendMode.Fade, transparency); Debug.Log("Smoothed alpha = " + alpha);
    }

    public enum BlendMode
    {
        Opaque,
        Cutout,
        Fade,   // Old school alpha-blending mode, fresnel does not affect amount of transparency
        Transparent // Physically plausible transparency mode, implemented as alpha pre-multiply
    }
    public static void SetupMaterialWithBlendMode(Material material, BlendMode blendMode)
    {
        switch (blendMode)
        {
            case BlendMode.Opaque:
                material.SetOverrideTag("RenderType", "");
                material.SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.One);
                material.SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.Zero);
                material.SetInt("_ZWrite", 1);
                material.DisableKeyword("_ALPHATEST_ON");
                material.DisableKeyword("_ALPHABLEND_ON");
                material.DisableKeyword("_ALPHAPREMULTIPLY_ON");
                material.renderQueue = -1;
                break;
            case BlendMode.Cutout:
                material.SetOverrideTag("RenderType", "TransparentCutout");
                material.SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.One);
                material.SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.Zero);
                material.SetInt("_ZWrite", 1);
                material.EnableKeyword("_ALPHATEST_ON");
                material.DisableKeyword("_ALPHABLEND_ON");
                material.DisableKeyword("_ALPHAPREMULTIPLY_ON");
                material.renderQueue = (int)UnityEngine.Rendering.RenderQueue.AlphaTest;
                break;
            case BlendMode.Fade:
                material.SetOverrideTag("RenderType", "Transparent");
                material.SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.SrcAlpha);
                material.SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.OneMinusSrcAlpha);
                material.SetInt("_ZWrite", 0);
                material.DisableKeyword("_ALPHATEST_ON");
                material.EnableKeyword("_ALPHABLEND_ON");
                material.DisableKeyword("_ALPHAPREMULTIPLY_ON");
                material.renderQueue = (int)UnityEngine.Rendering.RenderQueue.Transparent;
                break;
            case BlendMode.Transparent:
                material.SetOverrideTag("RenderType", "Transparent");
                material.SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.One);
                material.SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.OneMinusSrcAlpha);
                material.SetInt("_ZWrite", 0);
                material.DisableKeyword("_ALPHATEST_ON");
                material.DisableKeyword("_ALPHABLEND_ON");
                material.EnableKeyword("_ALPHAPREMULTIPLY_ON");
                material.renderQueue = (int)UnityEngine.Rendering.RenderQueue.Transparent;
                break;
        }
    }
}