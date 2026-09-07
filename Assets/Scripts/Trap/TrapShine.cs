using AllIn1SpriteShader;
using System.Collections;
using UnityEngine;

public class TrapShine : MonoBehaviour
{
    private Material shineMaterial;

    [SerializeField]
    private float cooldownBetweenShines = 0.5f; 

    private Coroutine shineRoutine;
    public bool IsShining => shineRoutine != null;

    void Start()
    {
        shineMaterial = GetComponent<SpriteRenderer>().material;
    }


    public void StartShineSequence()
    {
        if (shineRoutine == null)
            shineRoutine = StartCoroutine(ShineSequence());
    }

    private IEnumerator ShineSequence()
    {
        try
        {
            float shineLocation = 0f;
            shineMaterial.EnableKeyword("SHINE_ON");

            while (shineLocation < 1f)
            {
                shineLocation += Time.deltaTime/2;
                shineMaterial.SetFloat("_ShineLocation", shineLocation);
                yield return null;
            }
            yield return new WaitForSeconds(cooldownBetweenShines);
        }
        finally
        {
            shineMaterial.DisableKeyword("SHINE_ON");
            shineRoutine = null;
        }
    }

}
