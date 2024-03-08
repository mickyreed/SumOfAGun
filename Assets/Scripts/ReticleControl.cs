using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ReticleControl : MonoBehaviour
{
    public Image reticleRef;

    public void ChamgeReticle(Sprite reticle, Vector2 size)
    {
        reticleRef.sprite = reticle;
        reticleRef.GetComponent<RectTransform>().sizeDelta = size;
    }

}
