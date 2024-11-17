using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class FPSCounter : MonoBehaviour
{
    [SerializeField] TMP_Text fpxCountr;
    public float avgFrameRate;

    public void Update()
    {
        avgFrameRate = Time.frameCount / Time.time;
        fpxCountr.text = avgFrameRate.ToString();
    }
}
