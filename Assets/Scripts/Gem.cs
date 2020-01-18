using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gem : MonoBehaviour
{
    public enum Colors
    {
        Blue,
        Green,
        Orange,
        Purple,
        Red,
        Teal
    }

    public Colors Color { get { return color; } }

    [SerializeField]
    Colors color = Colors.Blue;

    [Header("Colldown in seconds")]
    public float blinkColldownMin = 60;
    public float blinkColldownMax = 360;

    
    Animator animator;
    float blinkColldown;

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        blinkColldown = Random.Range(blinkColldownMin, blinkColldownMax);
        StartCoroutine(StartBlinking());
    }

    // Update is called once per frame
    void Update()
    {

    }

    IEnumerator StartBlinking()
    {        
        yield return new WaitForSeconds(blinkColldown);
        animator.SetBool("Blink", true);
        blinkColldown = Random.Range(blinkColldownMin, blinkColldownMax);
        StartCoroutine(StartBlinking());
    }

    void StopBlinking()
    {
        animator.SetBool("Blink", false);
    }
}
