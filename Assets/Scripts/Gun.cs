using UnityEngine;
using UnityEngine.UIElements;
using System.Collections;

public class Gun : MonoBehaviour
{
    public Transform fireTransform;
    public ParticleSystem gunParticles;
    public LayerMask targetLayer;
    public AudioClip gunShotClip;
    private LineRenderer bulletLineEffect;
    private AudioSource gunAudioPlayer;

    private float fireDistance = 50f;
    private float lastFireTime = 0f;
    private float fireInterval = 0.12f;

    private Coroutine coShot;

    private void Awake()
    {
        bulletLineEffect = GetComponent<LineRenderer>();
        gunAudioPlayer = GetComponent<AudioSource>();
    }

    public void Fire()
    {
        if (Time.time > lastFireTime + fireInterval)
        {
            Shot();
            lastFireTime = Time.time;
        }
    }

    public void Shot()
    {
        Vector3 hitPosition = Vector3.zero;

        Ray ray = new Ray(fireTransform.position, fireTransform.forward);

        if (Physics.Raycast(ray, out RaycastHit hit, fireDistance, targetLayer))
        {
            hitPosition = hit.point;

            var target = hit.collider.GetComponent<LivingEntity>();
            if (target != null)
            {
                target.OnDamage (20f, hit.point, hit.normal);
            }
        }
        else
        {
            hitPosition = fireTransform.position + fireTransform.forward * fireDistance;
        }

        if (coShot != null)
        {
            StopCoroutine(coShot);
            coShot = null;
        }

        coShot = StartCoroutine(CoShotEffect(hitPosition));
    }

    private IEnumerator CoShotEffect (Vector3 hitPosition)
    {
        gunParticles.Play();
        gunAudioPlayer.PlayOneShot(gunShotClip);

        bulletLineEffect.SetPosition(0, fireTransform.position);
        bulletLineEffect.SetPosition(1, hitPosition);
        bulletLineEffect.enabled = true;

        yield return new WaitForSeconds(0.03f);

        bulletLineEffect.enabled = false;
        coShot = null;
    }
}
