using UnityEngine;

public class Rocket : Part
{
    private ParticleSystem _particles;
    private Rigidbody _rb;
    public float force = 10.0f;
    public Vector3Int exhaustDir;
    [SerializeField]
    private GameObject _exhaust;
    private void Awake()=>_rb = GetComponent<Rigidbody>();
    public override void Activate()
    {
        base.Activate();
        _exhaust.SetActive(true);
    }
    public override void DeActivate()
    {
        base.DeActivate();
        _exhaust.SetActive(false);
    }
    private void FixedUpdate()
    {
        if (isActive)
            _rb.AddForceAtPosition(transform.rotation * (Vector3)exhaustDir  * force,transform.position,ForceMode.Force);
    }
}
