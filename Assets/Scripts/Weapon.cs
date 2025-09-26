using UnityEngine;
using System.Collections;

public class Weapon : MonoBehaviour
{
    [SerializeField] private Camera playerCamera;
    [SerializeField] private float maxDistance = 10;
    [SerializeField] private ParticleSystem shotEffect;
    [SerializeField] private float takesDamage;

    [SerializeField] private float recoilUp = 0.05f;  // Vertical recoil
    [SerializeField] private float recoilBack = 0.05f; // Pushback recoil
    [SerializeField] private float recoilSpeed = 15f;  // Speed of recoil
    [SerializeField] private float returnSpeed = 15f;  // Speed of return

    private Vector3 originalPosition;
    private bool isRecoiling = false;

    private void Start()
    {
        originalPosition = transform.localPosition; // Store initial weapon position
    }

    void Update()
    {
        if (Input.GetButtonDown("Fire1"))
        {
            shotEffect.Play();
            AudioManager.PlaySound(Sounds.Shot, 0.4f);

            RaycastHit raycastHit;
            if (Physics.Raycast(playerCamera.transform.position, playerCamera.transform.forward, out raycastHit, maxDistance))
            {
                Target target = raycastHit.transform.GetComponent<Target>();

                if (target != null)
                    target.Damage(takesDamage, raycastHit.point, Quaternion.LookRotation(raycastHit.normal));
            }

            // Apply recoil effect
            if (!isRecoiling)
                StartCoroutine(RecoilEffect());
        }
    }

    private IEnumerator RecoilEffect()
    {
        isRecoiling = true;
        float elapsedTime = 0f;
        Vector3 recoilPosition = originalPosition + new Vector3(0, recoilUp, -recoilBack); // Move up & back

        // Move weapon up & back (recoil)
        while (elapsedTime < 1f / recoilSpeed)
        {
            elapsedTime += Time.deltaTime;
            transform.localPosition = Vector3.Lerp(transform.localPosition, recoilPosition, elapsedTime * recoilSpeed);
            yield return null;
        }

        elapsedTime = 0f;

        // Move weapon back to original position
        while (elapsedTime < 1f / returnSpeed)
        {
            elapsedTime += Time.deltaTime;
            transform.localPosition = Vector3.Lerp(transform.localPosition, originalPosition, elapsedTime * returnSpeed);
            yield return null;
        }

        transform.localPosition = originalPosition;
        isRecoiling = false;
    }

}
