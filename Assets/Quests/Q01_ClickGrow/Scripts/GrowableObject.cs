using UnityEngine;


public class GrowableObject : MonoBehaviour, IGrowable
{
    [SerializeField] private float maxSize = 4.0f;
    private bool hasReachedMaxSize = false;

    /// <summary>
    /// Grows the object by the specified amount until it reaches the maximum size   
    /// </summary>
    public void Grow(float amount)
    {
        if (hasReachedMaxSize)
        {
            Debug.Log("Object has already reached maximum size.");
            return;
        }

        transform.localScale += Vector3.one * amount;
        Debug.Log("Object grew.");

        if (transform.localScale.x >= maxSize)
        {
            transform.localScale = Vector3.one * maxSize;
            hasReachedMaxSize = true;
            OnMaxSizeReached();
        }
    }

    public Vector3 GetCurrentScale()
    {
        return transform.localScale;
    }

    public GameObject GetGameObject()
    {
        return gameObject;
    }

    private void OnMaxSizeReached()
    {
        Debug.Log("The object has reached its maximum size!");
        // when max size is reached
        if (hasReachedMaxSize)
        {
            // stop further growth
            return;
        }
    }

    public void Reset()
    {
        transform.localScale = Vector3.one;
        hasReachedMaxSize = false;
        Debug.Log("Object reset to original size.");
    }

}
