using UnityEngine;

public class CutoutObject : MonoBehaviour
{
    [SerializeField] 
    private GameObject _player;
    
    [SerializeField]
    private LayerMask wallMask;
    
    [SerializeField] 
    private Camera mainCamera;
    Vector2 lerpAlpha = Vector2.zero;
    //private bool _drawHole;

    private void Awake()
    {
    }

    //private void OnBecameVisible()
    //{
    //    _drawHole = true;
    //}

    //private void OnBecameInvisible()
    //{
    //    _drawHole = false;
    //}

    private void Update()
    {
        if (_player.transform.position.z >= transform.position.z)
        {
            SetCutAlpha(0f, 6f);
        }
        else
        {
            SetCutAlpha(0.15f, 3f);
        }
        foreach (Transform child in transform)
        {
            Vector2 cutoutPos = mainCamera.WorldToViewportPoint(transform.position);
            cutoutPos.y /= (Screen.width / Screen.height);

            if (child.TryGetComponent<Renderer>(out Renderer renrerer))
            {
                Material[] materials = child.GetComponent<Renderer>().materials;

                foreach (Material material in materials)
                {
                    material.SetInt("_EnableCutout", 1);
                    material.SetVector("_CutoutPos", cutoutPos + lerpAlpha);
                    material.SetFloat("_FalloffSize", 0.025f);
                }
            }
        }
    }
    private void SetCutAlpha(float targetAlpha, float fadeSpeed)
    {
        Vector2 targetColor = new Vector2(0f, targetAlpha);
        lerpAlpha = Vector2.Lerp(lerpAlpha, targetColor, Time.deltaTime * fadeSpeed);
    }
}
