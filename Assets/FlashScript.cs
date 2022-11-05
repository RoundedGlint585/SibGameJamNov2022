using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlashScript : MonoBehaviour
{

    public float flashTime = 0.3f;

    protected SpriteRenderer spriteRenderer;
    protected Material material;



    private float currentFlashTime = 0;
    public void Flash()
    {
        currentFlashTime = flashTime;
    }

    // Start is called before the first frame update
    private void Start()
    {
        spriteRenderer = GetComponentInChildren<SpriteRenderer>();
        material = spriteRenderer.material;
    }

    private void Update()
    {
        if (currentFlashTime > 0)
        {
            currentFlashTime -= Time.deltaTime;
            float flashAlpha = currentFlashTime / flashTime;
            material.SetFloat("_FlashAlpha", flashAlpha);
        }
    }
}
