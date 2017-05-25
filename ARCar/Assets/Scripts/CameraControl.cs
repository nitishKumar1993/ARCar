using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraControl : MonoBehaviour {

    public GameObject m_cameraGO;
    float m_currentCameraZoom = 60;

    // Use this for initialization
    void Start () {
		
	}

    // Update is called once per frame
    void Update()
    {
        if (Mathf.Abs(Input.GetAxis("Mouse ScrollWheel")) > 0)
        {
            m_currentCameraZoom -= Input.GetAxis("Mouse ScrollWheel") * 50;
            m_currentCameraZoom = Mathf.Clamp(m_currentCameraZoom, 30, 90);
            m_cameraGO.transform.GetChild(0).GetComponent<Camera>().fieldOfView = m_currentCameraZoom;
        }
    }
}
