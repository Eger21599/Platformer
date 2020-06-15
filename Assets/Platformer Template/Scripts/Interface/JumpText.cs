using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Text))]
public class JumpText : MonoBehaviour
{
    
    [SerializeField] private float minSizeRange = 90;
    [SerializeField] private float maxSizeRange = 107;
    [SerializeField] private float rate = 2;

    private Text currentText;

    private float zeroVaulue = 0f;
    private float timer;
    public bool needUp;

    private void Start()
    {
        currentText = GetComponent<Text>();
    }

    
    private void Update()
    {
        if(currentText.fontSize == maxSizeRange)
        {
            needUp = false;
        }
        if(currentText.fontSize == minSizeRange)
        {
            needUp = true;
        }

        timer += Time.deltaTime;

        if(needUp)
        {
            if(timer >= rate)
            {
                currentText.fontSize++;
                timer = 0.0f;
            }
        }
        if(!needUp)
        {
            if(timer >= rate)
            {
                currentText.fontSize--;
                timer = 0.0f;
            }
        }
    }
}
