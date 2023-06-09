using System.Collections;
using UnityEngine;

public abstract class BoltType : MonoBehaviour
{
    protected TrailRenderer[] trails;
    protected float damage;
    protected Collider coll;
    [SerializeField] protected float speed, yModifier;

    protected virtual void Awake()
    {
        trails = GetComponentsInChildren<TrailRenderer>();
        coll = GetComponentInChildren<Collider>();
    }

    void OnEnable()
    {
        for(int i = 0; i < trails.Length; i++)
            trails[i].enabled = false;
        coll.enabled = false;
    }

    public void Shot(Vector3 target, float _damage, float delay)
    {
        transform.LookAt(target + Vector3.up * yModifier);
        damage = _damage;
        StartCoroutine(ReadyToShot(delay));
    }

    public void Shot(Vector3 target, float _damage = 1f)
    {
        Shot(target, _damage, 0f);
    }

    protected virtual IEnumerator ReadyToShot(float delay)
    {
        yield return new WaitForSeconds(delay);
        for (int i = 0; i < trails.Length; i++)
        {
            trails[i].Clear();
            trails[i].enabled = false;
        }
        coll.enabled = true;
        GameManager.Resource.Destroy(gameObject, 10f);
        while (true)
        {
            transform.Translate(transform.forward * speed * Time.deltaTime, Space.World);
            yield return new WaitForFixedUpdate();
        }
    }

    protected virtual void OnDisable()
    {
        for (int i = 0; i < trails.Length; i++)
        {
            trails[i].Clear();
        }
        StopAllCoroutines();
    }

    protected virtual void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            other.GetComponent<IHitable>()?.Hit(damage, 0f);
            GameManager.Resource.Destroy(gameObject);
        }
        else if ((1 << other.gameObject.layer) == LayerMask.GetMask("Ground"))
        {
            GameManager.Resource.Destroy(gameObject);
        }
    }
}
