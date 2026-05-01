using UnityEngine;

public class Shooter : MonoBehaviour
{
    [SerializeField] private Transform shootPoint;
    [SerializeField] private GameObject target;
    [SerializeField] private Rigidbody2D bulletPrefab;

    void Update()
    {
        Vector2 screenPos = Input.mousePosition;

        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(screenPos);
            Debug.DrawRay(ray.origin, ray.direction * 5f, Color.red, 5f);

            RaycastHit2D hit = Physics2D.GetRayIntersection(ray, Mathf.Infinity);

            if (hit.collider != null)
            {
                target.transform.position = new Vector2(hit.point.x, hit.point.y);
                Debug.Log($"Hit {hit.collider.gameObject.name}");

                Vector2 projectileVelocity = CalculateProjectileVelocity(
                    shootPoint.position,
                    hit.point,
                    1f
                );

                Rigidbody2D shootBullet = Instantiate(
                    bulletPrefab,
                    shootPoint.position,
                    Quaternion.identity
                );

                shootBullet.velocity = projectileVelocity;
            }
        }
    }

    Vector2 CalculateProjectileVelocity(Vector2 origin, Vector2 target, float time)
    {
        Vector2 direction = target - origin;

        return new Vector2(
            direction.x / time,
            (direction.y / time) + 0.5f * Mathf.Abs(Physics2D.gravity.y) * time
        );
    }
}