using UnityEngine;
using UnityEngine.InputSystem; // ใช้ Input แบบใหม่โดยไม่ต้องไปตั้งค่า

public class Shooter : MonoBehaviour
{
    [SerializeField] private Transform shootPoint;     // จุดที่ยิงออก
    [SerializeField] private GameObject target;        // เป้าเล็ง / Crosshair
    [SerializeField] private Rigidbody2D bulletPrefab;

    void Start()
    {
    }

    void Update()
    {
        Vector2 screenPos = Mouse.current.position.ReadValue(); // อ่านตำแหน่งเมาส์

        if (Mouse.current.leftButton.wasPressedThisFrame)
        {
            // ยิง Ray ไปตามตำแหน่งเมาส์
            Ray ray = Camera.main.ScreenPointToRay(screenPos);
            Debug.DrawRay(ray.origin, ray.direction * 5f, Color.red, 5f);

            // ตรวจจับ Ray ชนอะไร
            RaycastHit2D hit = Physics2D.GetRayIntersection(ray, Mathf.Infinity);

            if (hit.collider != null)
            {
                target.transform.position = new Vector2(hit.point.x, hit.point.y);
                Debug.Log($"Hit {hit.collider.gameObject.name}");

                // คำนวณความเร็วกระสุน
                Vector2 projectileVelocity = CalculateProjectileVelocity(
                    shootPoint.position,
                    hit.point,
                    1f
                );

                // สร้างกระสุน
                Rigidbody2D shootBullet = Instantiate(
                    bulletPrefab,
                    shootPoint.position,
                    Quaternion.identity
                );

                // ใส่ความเร็วให้กระสุน
                shootBullet.velocity = projectileVelocity;
            }
        }
    }

    Vector2 CalculateProjectileVelocity(Vector2 origin, Vector2 target, float time)
    {
        // time = เวลาในการบิน
        Vector2 direction = target - origin;

        return new Vector2(
            direction.x / time,
            (direction.y / time) + 0.5f * Mathf.Abs(Physics2D.gravity.y) * time
        );
    }
}