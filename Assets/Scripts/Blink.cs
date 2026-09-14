using System.Collections;
using UnityEngine;

public class Blink : MonoBehaviour
{
    Collider col;
    Material mat;

    public float visibleTime;
    public float invisibleTime;

    [Range(0, 1)]
    public float alphaVal;

    void Start()
    {
        col = GetComponentInChildren<Collider>();
        mat = GetComponentInChildren<Renderer>().material;
        StartCoroutine("BlinkTime");
    }

    void Update()
    {

    }

    IEnumerator BlinkTime()
    {
        while(true)
        {
            col.enabled = true;
            mat.color = new Color(mat.color.r, mat.color.g, mat.color.b, 1);

            yield return new WaitForSeconds(visibleTime);

            col.enabled = false;
            mat.color = new Color(mat.color.r, mat.color.g, mat.color.b, alphaVal);

            yield return new WaitForSeconds(invisibleTime);
        }
    }
}
