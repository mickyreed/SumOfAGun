using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using Unity.VisualScripting.Antlr3.Runtime.Tree;
using UnityStandardAssets.Utility;

public class IntroScene : MonoBehaviour
{
    public CanvasGroup fadeCanvas;
    public GameObject Player; 

    public GameObject LightBeam;

    public TMP_Text[] textDisplayHUD;

    void Start()
    {
        StartCoroutine(PlayIntroSequence());
    }

    IEnumerator PlayIntroSequence()
    {
        // Fade in from black
        yield return StartCoroutine(FadeCanvasGroup(fadeCanvas, 0, 1, 2f));

        // Activate Teleport Beam and lights
        //LightBeam.SetActive(true);

        // Timing as Player is beamed down to compound (all gravity)
        float fallDuration = 3f;
        float elapsedTime = 0f;

        // Delay before displaying text to coincide with text animation timing
        yield return new WaitForSeconds(2f);

        while (elapsedTime < fallDuration)
        {
            elapsedTime += Time.deltaTime;
            float t = elapsedTime / fallDuration;
            yield return null;
        }

        // Display text on HUD
        foreach (TMP_Text textObject in textDisplayHUD)
        {
            textObject.gameObject.SetActive(true);
            yield return new WaitForSeconds(2f);
            textObject.gameObject.SetActive(false);
        }
        // Destroy Teleport Beam and lights
        Destroy(LightBeam);
    }

    void StartTeleportBeam()
    {
        // Activate Teleport Beam and lights
        LightBeam.SetActive(true);
    }

    IEnumerator FadeCanvasGroup(CanvasGroup canvasGroup, float start, float end, float duration)
    {
        float elapsedTime = 0f;
        canvasGroup.alpha = start;

        StartTeleportBeam();

        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            canvasGroup.alpha = Mathf.Lerp(start, end, elapsedTime / duration);
            yield return null;
        }
        canvasGroup.alpha = end;

    }
}
