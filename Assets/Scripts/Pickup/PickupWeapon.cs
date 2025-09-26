using UnityEngine;
using System.Collections;

public class PickupWeapon : MonoBehaviour
{
    [SerializeField] private Weapon weapon;
    [SerializeField] private BoxCollider boxCollider;
    [SerializeField] private Transform player, weaponContainer, fpsCam;

    [SerializeField] private float dropForwardForce = 2f, dropUpwardForce = 1f, dropSpeed = 5f, pickupRange = 2f, groundOffset = 0.05f;

    [SerializeField] private bool equipped;
    public static bool slotFull;
    [SerializeField] private bool isDropping = false; // Prevents pickup during drop

    [SerializeField] private LayerMask groundLayer, obstacleLayer;

    private void Start()
    {
        if (!equipped)
        {
            weapon.enabled = false;
            boxCollider.isTrigger = false;
        }
        else
        {
            weapon.enabled = true;
            boxCollider.isTrigger = true;
            slotFull = true;
        }
    }

    private void Update()
    {
        Vector3 distanceToPlayer = player.position - transform.position;

        if (!equipped && !isDropping && distanceToPlayer.magnitude <= pickupRange && Input.GetKeyDown(KeyCode.E) && !slotFull)
        {
            Pickup();
        }

        if (equipped && Input.GetKeyDown(KeyCode.Q))
        {
            Drop();
        }
    }

    private void Pickup()
    {
        if (isDropping) return;

        equipped = true;
        slotFull = true;

        StopAllCoroutines();

        transform.SetParent(weaponContainer);
        transform.localPosition = Vector3.zero;
        transform.localRotation = Quaternion.Euler(Vector3.zero);
        transform.localScale = Vector3.one;

        boxCollider.isTrigger = true;
        weapon.enabled = true;
        SetLayerRecursively(gameObject, LayerMask.NameToLayer("Weapon"));
    }

    private void Drop()
    {
        if (isDropping) return;

        equipped = false;
        slotFull = false;
        isDropping = true;

        transform.SetParent(null);
        boxCollider.isTrigger = false;
        weapon.enabled = false;

        SetLayerRecursively(gameObject, LayerMask.NameToLayer("Default"));

        // Calculate initial drop direction
        Vector3 dropDirection = (fpsCam.forward * dropForwardForce) + (Vector3.up * dropUpwardForce);
        Vector3 targetPosition = transform.position + dropDirection;
        Quaternion targetRotation = transform.rotation;

        bool obstacleHit = Physics.Raycast(player.transform.position, player.transform.forward, out RaycastHit hitObstacle, dropDirection.magnitude, obstacleLayer);

        // Stop in front of the obstacle
        if (obstacleHit)
        {
            float xOffset = 0.8f;
            targetPosition = hitObstacle.point - (player.transform.forward * xOffset);
        }

        // Check if the adjusted targetPosition is above the ground
        if (Physics.Raycast(targetPosition, Vector3.down, out RaycastHit hitGround, 10f, groundLayer))
        {
            if(!obstacleHit) targetPosition = hitGround.point + (hitGround.normal * groundOffset);
            targetRotation = Quaternion.FromToRotation(Vector3.up, hitGround.normal) * Quaternion.Euler(0, 0, 90);
        }

        StartCoroutine(SmoothDrop(targetPosition, targetRotation));
    }



    private IEnumerator SmoothDrop(Vector3 targetPos, Quaternion targetRot)
    {
        while (Vector3.Distance(transform.position, targetPos) > 0.01f)
        {
            transform.position = Vector3.Lerp(transform.position, targetPos, dropSpeed * Time.deltaTime);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRot, dropSpeed * Time.deltaTime);
            yield return null;
        }

        transform.position = targetPos;
        transform.rotation = targetRot;
        isDropping = false;
    }

    void SetLayerRecursively(GameObject obj, int newLayer)
    {
        obj.layer = newLayer;
        foreach (Transform child in obj.transform)
        {
            SetLayerRecursively(child.gameObject, newLayer);
        }
    }

}
