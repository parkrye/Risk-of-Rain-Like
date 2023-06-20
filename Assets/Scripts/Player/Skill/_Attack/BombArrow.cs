using System.Collections;
using UnityEngine;

public class BombArrow : ArrowType
{
    ParticleSystem bombParticle;

    [SerializeField] float range;

    protected override void Awake()
    {
        base.Awake();
        bombParticle = GameManager.Resource.Load<ParticleSystem>("Particle/Explosion");
    }

    protected override IEnumerator ReadyToShot(float delay)
    {
        trail.Clear();
        trail.enabled = true;
        yield return new WaitForSeconds(delay);
        GameManager.Resource.Destroy(gameObject, 10f);
        while (true)
        {
            transform.Translate((transform.forward * speed + Vector3.up * Physics.gravity.y) * Time.deltaTime, Space.World);
            yield return new WaitForFixedUpdate();
        }
    }

    protected override void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Enemy" || (1 << other.gameObject.layer == LayerMask.GetMask("Ground")))
        {
            ParticleSystem effect = GameManager.Resource.Instantiate(bombParticle, transform.position, Quaternion.identity, true);
            GameManager.Resource.Destroy(effect.gameObject, 2f);

            Collider[] colliders = Physics.OverlapSphere(transform.position, range);
            foreach (Collider collider in colliders)
            {
                IHitable hitable = collider.GetComponent<IHitable>();
                hitable?.Hit(damage);
            }
            GameManager.Pool.Release(gameObject);
        }
    }
}