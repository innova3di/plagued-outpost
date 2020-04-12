using InnoVision;
using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class PauseMenu : MonoBehaviour
{
    public GameObject settingsOptions;
    public GameObject pausedMenuBG;
    public GameObject pausedOptions;
    public GameObject quitOptions;

    private Animator m_pausedMenuAnimator;
    private PauseMenu() { GetGameState = GameState.Running; }

    public static int fadeInPara = Animator.StringToHash("FadeIn");

    public Toggle mobAggressionToggle;
    public Dictionary<int, Animator> enemyAnimator = new Dictionary<int, Animator>();

    private void ToggleValueChanged()
    {
        foreach (KeyValuePair<int, Animator> animator in enemyAnimator)
        {
            animator.Value.SetBool(AnimatorUtility.ES.aggressivePara, mobAggressionToggle.isOn);
        }
    }

    void Start()
    {
        SetCursorLockedState(CursorLockMode.Locked, false);
        m_pausedMenuAnimator = pausedMenuBG.GetComponentInChildren<Animator>();
        mobAggressionToggle.onValueChanged.AddListener(delegate { ToggleValueChanged(); });
    }

    void Update() { CursorBehaviour(); }

    private void SetCursorLockedState(CursorLockMode mode, bool visibility) { Cursor.lockState = mode; Cursor.visible = visibility; }
    private void CursorBehaviour()
    {
        if (Input.GetKeyDown(KeyCode.Escape) && GetGameState == GameState.Running)
        {
            ChangeGameState(GameState.Paused, true);
            SetCursorLockedState(CursorLockMode.Confined, true);
        }
        if (Input.GetKeyDown(KeyCode.Mouse0) || Input.GetKeyDown(KeyCode.Mouse1) || Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.D))
        {
            if (GetGameState == GameState.Running) { SetCursorLockedState(CursorLockMode.Locked, false); }
        }
    }

    public static GameState GetGameState { get; private set; }
    private void ChangeGameState(GameState state, bool fadeInMenu)
    {
        pausedOptions.SetActive(fadeInMenu);
        m_pausedMenuAnimator.SetBool(fadeInPara, fadeInMenu);
        if (state == GameState.Paused) { Time.timeScale = 0f; GetGameState = GameState.Paused; }
        else if (state == GameState.Running) { Time.timeScale = 1f; GetGameState = GameState.Running; }
    }

    private static bool m_inputDeferred;
    public static bool InputDeferred
    {
        get
        {
            if (Cursor.lockState != CursorLockMode.Locked)
            {
                if (!m_inputDeferred) { m_inputDeferred = true; }
            }
            else if (m_inputDeferred && Cursor.lockState == CursorLockMode.Locked)
            {
                if (Input.GetKeyUp(KeyCode.Mouse0) || Input.GetKeyUp(KeyCode.Mouse1) || Input.GetKeyUp(KeyCode.W) || Input.GetKeyUp(KeyCode.A) || Input.GetKeyUp(KeyCode.S) || Input.GetKeyUp(KeyCode.D))
                {
                    m_inputDeferred = false; // Debug.Log("Input deferred, next input will be accepted.. ");
                }
            }
            return m_inputDeferred;
        }
    }

    public void Settings()
    {
        settingsOptions.SetActive(true);
        pausedOptions.SetActive(false);
    }

    public void CloseSettings()
    {
        pausedOptions.SetActive(true);
        settingsOptions.SetActive(false);
    }

    public void Quit()
    {
        quitOptions.SetActive(true);
        pausedOptions.SetActive(false);
    }
    public void Resume()
    {
        m_inputDeferred = false;
        ChangeGameState(GameState.Running, false);
        SetCursorLockedState(CursorLockMode.Locked, false);
    }
    public void No()
    {
        quitOptions.SetActive(false);
        pausedOptions.SetActive(true);
    }
    public void Yes()
    {
        Application.Quit();
    }
}