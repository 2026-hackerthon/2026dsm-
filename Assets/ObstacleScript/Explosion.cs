using UnityEngine;
using System.Collections;

public class Explosion : MonoBehaviour
{
    void OnEnable()
    {
        StartCoroutine(ExplosionOver());
    }

    IEnumerator ExplosionOver()
    {
        yield return new WaitForSeconds(0.7f);
        gameObject.SetActive(false);
    }
}
