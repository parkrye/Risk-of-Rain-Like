using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrowType : MonoBehaviour
{
    protected TrailRenderer trail;
    protected float damage;
    [SerializeField] protected float speed, yModifier;

    protected virtual void Awake()
    {
        trail = GetComponent<TrailRenderer>();
    }

    void OnEnable()
    {
        trail.enabled = false;
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
        trail.Clear();
        trail.enabled = true;
        GameManager.Resource.Destroy(gameObject, 10f);
        while (true)
        {
            transform.Translate((transform.forward * speed + Vector3.up * Physics.gravity.y) * Time.deltaTime, Space.World);
            yield return new WaitForFixedUpdate();
        }
    }

    void OnDisable()
    {
        trail.Clear();
    }


    protected virtual void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Enemy")
        {
            other.GetComponent<IHitable>()?.Hit(damage);
            GameManager.Pool.Release(gameObject);
        }
        else if (1 << other.gameObject.layer == LayerMask.GetMask("Ground"))
        {
            GameManager.Pool.Release(gameObject);
        }
    }
}