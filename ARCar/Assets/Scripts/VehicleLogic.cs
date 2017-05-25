using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VehicleLogic : MonoBehaviour
{
    public List<MeshRenderer> m_bodyMeshRendererList;
    public List<GameObject> m_LightsList;

    public LightsRenderers m_frontLightMeshRenderers;
    public LightsRenderers m_rearLightMeshRenderers;
    public LightsRenderers m_otherLightMeshRenderers;

    public void ChangeVehicleColor()
    {
        Color tempColor = CUIColorPicker.Instance.Color;
        Material tempMat = null;
        for (int i = 0; i < m_bodyMeshRendererList.Count; i++)
        {
            for (int j = 0; j < m_bodyMeshRendererList[i].materials.Length; j++)
            {
                if (m_bodyMeshRendererList[i].materials[j].name.Contains("Body"))
                {
                    tempMat = new Material(m_bodyMeshRendererList[i].materials[j]);
                    tempMat.color = tempColor;

                    var mats = m_bodyMeshRendererList[i].materials;
                    mats[j] = tempMat;
                    m_bodyMeshRendererList[i].materials = mats;
                }
            }
        }
    }


    public void ToggleLights(bool value)
    {
        for (int i = 0; i < m_LightsList.Count; i++)
        {
            m_LightsList[i].SetActive(value);
        }

        if (m_frontLightMeshRenderers.m_lightMeshRenderer != null)
        {
            ChangeMat(m_frontLightMeshRenderers, value);
        }
        if (m_rearLightMeshRenderers.m_lightMeshRenderer != null)
        {
            ChangeMat(m_rearLightMeshRenderers, value);
        }
        if (m_otherLightMeshRenderers.m_lightMeshRenderer != null)
        {
            ChangeMat(m_otherLightMeshRenderers, value);
        }
    }

    void ChangeMat(LightsRenderers renderers,bool value)
    {
        Material[] allMat = renderers.m_lightMeshRenderer.materials;
        for (int j = 0; j < allMat.Length; j++)
        {
            if (allMat[j].name.Contains("Light"))
            {
                allMat[j] = value ? renderers.m_lightEmissionMat : renderers.m_lightNormalMat;
            }
        }
        renderers.m_lightMeshRenderer.materials = allMat;
    }
}

[System.Serializable]
public class LightsRenderers
{
    public MeshRenderer m_lightMeshRenderer;
    public Material m_lightNormalMat;
    public Material m_lightEmissionMat;
}
