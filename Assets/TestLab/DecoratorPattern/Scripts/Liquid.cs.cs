using UnityEngine;

public class Liquid : MonoBehaviour
{
    public const string TopColorId = "_TopColor";
    public const string BottomColorId = "_BottomColor";
    public const string FoamColorId = "_FoamColor";
    
    public enum UpdateMode { Normal, UnscaledTime }
    public UpdateMode updateMode;

    [SerializeField] private float MaxWobble = 0.03f;
    [SerializeField] private float WobbleSpeedMove = 1f;
    [SerializeField] private float fillAmount = 0.5f;
    [SerializeField] private float Recovery = 1f;
    [SerializeField] private float Thickness = 1f;
    [Range(0, 1)] public float CompensateShapeAmount;
    [SerializeField] private Mesh mesh;
    [SerializeField] private Renderer rend;
    
    private Vector3 pos;
    private Vector3 lastPos;
    private Vector3 velocity;
    private Quaternion lastRot;
    private Vector3 angularVelocity;
    private float wobbleAmountX;
    private float wobbleAmountZ;
    private float wobbleAmountToAddX;
    private float wobbleAmountToAddZ;
    private float pulse;
    private float sinewave;
    private float time = 0.5f;
    private Vector3 comp;

    private void Start()
    {
        GetMeshAndRend();
    }

    private void OnValidate()
    {
        GetMeshAndRend();
    }
    
    private void Update()
    {
        float deltaTime = 0;
        switch (updateMode)
        {
            case UpdateMode.Normal:
                deltaTime = Time.deltaTime;
                break;

            case UpdateMode.UnscaledTime:
                deltaTime = Time.unscaledDeltaTime;
                break;
        }

        time += deltaTime;
        if (deltaTime != 0)
        {
            wobbleAmountToAddX = Mathf.Lerp(wobbleAmountToAddX, 0, (deltaTime * Recovery));
            wobbleAmountToAddZ = Mathf.Lerp(wobbleAmountToAddZ, 0, (deltaTime * Recovery));

            pulse = 2 * Mathf.PI * WobbleSpeedMove;
            sinewave = Mathf.Lerp(sinewave, Mathf.Sin(pulse * time), deltaTime * Mathf.Clamp(velocity.magnitude + angularVelocity.magnitude, Thickness, 10));

            wobbleAmountX = wobbleAmountToAddX * sinewave;
            wobbleAmountZ = wobbleAmountToAddZ * sinewave;

            velocity = (lastPos - transform.position) / deltaTime;
            angularVelocity = GetAngularVelocity(lastRot, transform.rotation);

            wobbleAmountToAddX += Mathf.Clamp((velocity.x + (velocity.y * 0.2f) + angularVelocity.z + angularVelocity.y) * MaxWobble, -MaxWobble, MaxWobble);
            wobbleAmountToAddZ += Mathf.Clamp((velocity.z + (velocity.y * 0.2f) + angularVelocity.x + angularVelocity.y) * MaxWobble, -MaxWobble, MaxWobble);
        }

        rend.material.SetFloat("_WobbleX", wobbleAmountX);
        rend.material.SetFloat("_WobbleZ", wobbleAmountZ);

        UpdatePos(deltaTime);
        lastPos = transform.position;
        lastRot = transform.rotation;
    }

    public void SetFillAmount(float fill) => fillAmount = fill;
    
    public void AddFillAmount(float fill) => fillAmount += fill;

    public void SetColor(Color newColor, string colorId) => rend.material.SetColor(colorId, newColor);
    
    public void ReduceColorIntensity(float intensity, string colorId)
    {
        float factor = Mathf.Pow(2,-intensity);
        Color newColor = rend.material.GetColor(colorId) * factor;
        newColor.a = 1f;
        rend.material.SetColor(colorId, newColor);
    }

    public void MixColor(Color mixColor, float ratio, string colorId)
    {
        Color newColor = Color.Lerp(rend.material.GetColor(colorId), mixColor, ratio);
        rend.material.SetColor(colorId, newColor);
    }
    
    private void GetMeshAndRend()
    {
        if (mesh == null)
            mesh = GetComponent<MeshFilter>().sharedMesh;
        
        if (rend == null)
            rend = GetComponent<Renderer>();
    }
    
    private void UpdatePos(float deltaTime)
    {
        Vector3 worldPos = transform.TransformPoint(new Vector3(mesh.bounds.center.x, mesh.bounds.center.y, mesh.bounds.center.z));
        if (CompensateShapeAmount > 0)
        {
            if (deltaTime != 0)
                comp = Vector3.Lerp(comp, (worldPos - new Vector3(0, GetLowestPoint(), 0)), deltaTime * 10);
            else
                comp = (worldPos - new Vector3(0, GetLowestPoint(), 0));

            pos = worldPos - transform.position - new Vector3(0, fillAmount - (comp.y * CompensateShapeAmount), 0);
        }
        else
            pos = worldPos - transform.position - new Vector3(0, fillAmount, 0);
        rend.material.SetVector("_FillAmount", pos);
    }

    //https://forum.unity.com/threads/manually-calculate-angular-velocity-of-gameobject.289462/#post-4302796
    private Vector3 GetAngularVelocity(Quaternion foreLastFrameRotation, Quaternion lastFrameRotation)
    {
        var q = lastFrameRotation * Quaternion.Inverse(foreLastFrameRotation);
        // no rotation?
        // You may want to increase this closer to 1 if you want to handle very small rotations.
        // Beware, if it is too close to one your answer will be Nan
        if (Mathf.Abs(q.w) > 1023.5f / 1024.0f)
            return Vector3.zero;
        float gain;
        // handle negatives, we could just flip it but this is faster
        if (q.w < 0.0f)
        {
            var angle = Mathf.Acos(-q.w);
            gain = -2.0f * angle / (Mathf.Sin(angle) * Time.deltaTime);
        }
        else
        {
            var angle = Mathf.Acos(q.w);
            gain = 2.0f * angle / (Mathf.Sin(angle) * Time.deltaTime);
        }
        Vector3 angularVelocity = new Vector3(q.x * gain, q.y * gain, q.z * gain);

        if (float.IsNaN(angularVelocity.z))
        {
            angularVelocity = Vector3.zero;
        }
        return angularVelocity;
    }

    private float GetLowestPoint()
    {
        float lowestY = float.MaxValue;
        Vector3 lowestVert = Vector3.zero;
        Vector3[] vertices = mesh.vertices;

        for (int i = 0; i < vertices.Length; i++)
        {
            Vector3 position = transform.TransformPoint(vertices[i]);
            if (position.y < lowestY)
            {
                lowestY = position.y;
                lowestVert = position;
            }
        }
        return lowestVert.y;
    }
}
