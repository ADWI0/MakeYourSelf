using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    public static PauseMenu Instance { get; private set; }

    public Slider slider;
    public TMP_InputField inputField;

    float sens;
    public float Sens { get {  return sens; } set { sens = value; } }

    bool pause;
    public bool Pause { get { return pause; } }

    float editSens;

    public KeyCode pauseKey = KeyCode.Escape;
    public GameObject panel;
    public GameObject counter;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        Resume();
    }

    void Update()
    {
        if (Input.GetKeyDown(pauseKey))
        {
            if (!pause)
            {
                PauseOn();
            }
            else
            {
                Resume();
            }
        }
    }

    public void SliderValue(float value)
    {
        sens = Mathf.Round(value * 10f) / 10f;
        inputField.text = sens.ToString();
    }

    public void EditValue()
    {
        if (float.TryParse(inputField.text, out float val))
        {
            editSens = val;
        }
        else return;
        sens = editSens;
        slider.value = sens;
    }

    public void PauseOn()
    {
        pause = true;
        panel.SetActive(pause);
        counter.SetActive(!pause);
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        Time.timeScale = 0;
    }

    public void Resume()
    {
        Time.timeScale = 1;
        pause = false;
        panel.SetActive(pause);
        counter.SetActive(!pause);
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    public void Restart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        Resume();
    }
}
