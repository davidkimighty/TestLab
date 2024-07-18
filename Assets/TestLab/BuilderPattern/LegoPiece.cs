using System;
using System.Collections;
using UnityEngine;

[Serializable]
public class Slot
{
    public Transform Anchor;
    public bool IsFull;
}

public abstract class LegoPiece<T> : MonoBehaviour where T : LegoSet
{
    public int Priority;

    [SerializeField] protected Rigidbody body;
    [SerializeField] protected float smoothAssembleDuration = 0.5f;
    [SerializeField] protected AnimationCurve smoothAssembleCurve;
    
    protected IEnumerator smoothAssembleCoroutine;
    
    public abstract void Assemble(T legoSet);

    public abstract void AssembleAfterDelay(T legoSet, float delay);

    public virtual void Disassemble()
    {
        if (smoothAssembleCoroutine != null)
            StopCoroutine(smoothAssembleCoroutine);
        smoothAssembleCoroutine = null;
        
        transform.SetParent(null);
        
        if (body == null)
        {
            body = gameObject.AddComponent<Rigidbody>();
            body.useGravity = false;
            body.linearDamping = 5;
            body.angularDamping = 3;
        }
    }
    
    protected virtual IEnumerator SmoothAssemble(Transform target, float duration, float delay, bool removeBody = true)
    {
        float elapsedTime = 0f;
        if (delay > 0f)
            yield return new WaitForSeconds(delay);
        
        transform.SetParent(target);
        Vector3 startPos = transform.localPosition;
        Quaternion startRot = transform.localRotation;
        
        while (elapsedTime < duration)
        {
            float fraction = smoothAssembleCurve.Evaluate(elapsedTime / duration);
            transform.localPosition = Vector3.SlerpUnclamped(startPos, Vector3.zero, fraction);
            transform.localRotation = Quaternion.SlerpUnclamped(startRot, target.localRotation, fraction);
            
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        
        if (removeBody)
            Destroy(body);
        transform.SetLocalPositionAndRotation(Vector3.zero, Quaternion.identity);
    }
}
