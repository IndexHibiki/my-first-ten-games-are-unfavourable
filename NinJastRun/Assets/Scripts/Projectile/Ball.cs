using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ball : MonoBehaviour
{
    [SerializeField] SpriteRenderer innerBall;
    [SerializeField] SpriteRenderer outerBall;
    [SerializeField] [Range(0f, 5f)] float outerAlphaChangeSpeed = 0.5f;

    void Start()
    {
        
    }

    private void FixedUpdate()
    {
        float newAlpha = outerBall.color.a + outerAlphaChangeSpeed * Time.deltaTime;

        if(newAlpha > 1f)
        {
            newAlpha = 2 - newAlpha;
            outerAlphaChangeSpeed = -outerAlphaChangeSpeed;
        }
        else if(newAlpha < 0f)
        {
            newAlpha = -newAlpha;
            outerAlphaChangeSpeed = -outerAlphaChangeSpeed;
        }
        outerBall.color = new Color(
            outerBall.color.r, 
            outerBall.color.g, 
            outerBall.color.b, 
            newAlpha
            );
    }
}
