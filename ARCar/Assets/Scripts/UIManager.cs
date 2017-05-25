using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour {
    private static UIManager _instance;

    public GameObject m_cameraGO;
    public GameObject m_lightParentGO;

    public GameObject m_ColorPickerGO;
    public GameObject m_currentCarGO;

    public ReflectionProbe m_reflectionProbe;

    bool m_isLightRotating = false;

    public static UIManager Instance
    { get { return _instance; } }

    // Use this for initialization
    void Awake()
    {
        _instance = this;
        Input.multiTouchEnabled = true;
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.touchCount > 0)
        {
            if (Input.touchCount == 2 && Input.GetTouch(1).phase == TouchPhase.Began)
            {
                StopCoroutine("RotateLight");
                StartCoroutine("RotateLight");
            }
            else if (Input.touchCount == 1 && Input.GetTouch(0).phase == TouchPhase.Began)
            {
                StopCoroutine("RotateCamera");
                StartCoroutine("RotateCamera");
            }
        }
        else
        {
            if (Input.GetKeyDown(KeyCode.LeftAlt))
            {
                StopCoroutine("RotateLight");
                StartCoroutine("RotateLight");
            }
            else if (Input.GetMouseButtonDown(0) && !m_isLightRotating)
            {
                StopCoroutine("RotateCamera");
                StartCoroutine("RotateCamera");
            }
        }
    }

    public void OnZoomSliderValueChange(float value)
    {
        m_cameraGO.transform.GetChild(0).GetComponent<Camera>().fieldOfView = 60 - 30 + (60 * value);
    }

    public void OnLightIntensityValueChange(float value)
    {
        m_lightParentGO.transform.GetChild(0).GetComponent<Light>().intensity = 30 * value;
        if (!renderSceneTimerRunning)
            StartCoroutine(RenderSceneProbeWithDelay(0.3f));
        else renderSceneDelayTimer = 0.3f;
    }

    float renderSceneDelayTimer = 0;
    bool renderSceneTimerRunning = false;
    IEnumerator RenderSceneProbeWithDelay(float time)
    {
        renderSceneTimerRunning = true;
        renderSceneDelayTimer  = time;
        while (renderSceneDelayTimer > 0)
        {
            renderSceneDelayTimer -= Time.deltaTime;
            yield return null;
        }
        Debug.Log("2");
        renderSceneTimerRunning = false;
        RenderSceneProbe();
        renderSceneDelayTimer = 0;
    }

    public void ToggleColorPicker()
    {
        m_ColorPickerGO.SetActive(!m_ColorPickerGO.activeSelf);
    }

    public void OnColorPickDone()
    {
        m_currentCarGO.GetComponent<VehicleLogic>().ChangeVehicleColor();
        ToggleColorPicker();
        RenderSceneProbe();
    }

    public void ToggleVehicleLights(bool value)
    {
        m_currentCarGO.GetComponent<VehicleLogic>().ToggleLights(value);
        RenderSceneProbe();
        m_reflectionProbe.intensity = value ? 10 : 2;
    }

    public void ToggleMainLight(bool value)
    {
        m_lightParentGO.SetActive(value);
    }

    public void RenderSceneProbe()
    {
        m_reflectionProbe.RenderProbe();
    }

    IEnumerator RotateCamera()
    {
        Vector3 lastMousePos = Input.touchCount == 0 ? Input.mousePosition :(Vector3)Input.GetTouch(0).position;
        Vector3 difference = Vector3.zero;

        while (Input.GetMouseButton(0) || Input.touchCount > 0)
        {
            if (CUIColorPicker.Instance != null && CUIColorPicker.Instance.IsPlayerChoosingColor)
                break;
            if (Input.touchCount > 0)
            {
                if (Input.touchCount > 1)
                    break;
                difference = (lastMousePos - (Vector3)Input.GetTouch(0).position);
                m_cameraGO.transform.eulerAngles -= new Vector3(0, difference.x, difference.y) / 10;
                lastMousePos = (Vector3)Input.GetTouch(0).position;
            }
            else
            {
                difference = (lastMousePos - Input.mousePosition);
                m_cameraGO.transform.eulerAngles -= new Vector3(0, difference.x, difference.y) / 10;
                lastMousePos = Input.mousePosition;
            }
            yield return null;
        }
    }

    IEnumerator RotateLight()
    {
        m_isLightRotating = true;
        Vector3 lastMousePos = Input.touchCount == 0 ? Input.mousePosition : (Vector3)Input.GetTouch(1).position;
        Vector3 difference = Vector3.zero;
        while (Input.GetKey(KeyCode.LeftAlt) || Input.touchCount > 1)
        {
            if (Input.touchCount > 1)
            {
                difference = (lastMousePos - (Vector3)Input.GetTouch(1).position);
                m_lightParentGO.transform.eulerAngles -= new Vector3(0, Input.GetTouch(1).deltaPosition.x, Input.GetTouch(1).deltaPosition.y)/ 10;
                lastMousePos = (Vector3)Input.GetTouch(1).position;
            }
            else if (Input.GetMouseButton(0))
            {
                if(Input.GetMouseButtonDown(0))
                    lastMousePos = Input.mousePosition;
                difference = (lastMousePos - Input.mousePosition);
                m_lightParentGO.transform.eulerAngles += new Vector3(0, difference.x, difference.y) / 10;
                lastMousePos = Input.mousePosition;
            }
            yield return null;
        }
        RenderSceneProbe();
        m_isLightRotating = false;
    }
}
