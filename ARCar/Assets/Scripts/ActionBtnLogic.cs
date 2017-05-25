using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActionBtnLogic : MonoBehaviour {

    public Animation m_animation;
    bool m_active = false;
    bool m_animating = false;

    void OnMouseUpAsButton()
    {
        if (!m_animating)
        {
            m_animating = true;
            m_active = !m_active;
            foreach (AnimationState state in m_animation)
            {
                state.speed = m_active ? 1 : -1;
                state.time = m_active ? 0 : state.length;
                Invoke("ResetAnimationFlag", state.length);
                break;
            }
            m_animation.Play();
        }
    }

    void ResetAnimationFlag()
    {
        m_animating = false;
    }
}
