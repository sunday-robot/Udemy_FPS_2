using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FpsController : MonoBehaviour
{
    /// <summary>
    /// 移動速度
    /// </summary>
    const float speed = 0.1F;

    /// <summary>
    /// 視線移動速度(マウス感度)
    /// </summary>
    const float mouseXSensitivity = 3F;
    const float mouseYSensitivity = 3F;

    // プレイヤーの位置
    Vector3 playerPosition;
    // プレイヤーの向き(左右方向)
    float playerDirection;
    // プレイヤーの向き(上下方向)
    float playerElevation;

    public GameObject cameraObject;
    //Quaternion cameraRotation;
    //Quaternion playerRotation;

    // Start is called before the first frame update
    void Start()
    {
//        cameraRotation = cameraObject.transform.localRotation;
        Cursor.lockState = CursorLockMode.Locked;   // マウスカーソルを非表示
    }

    // Update is called once per frame
    void Update()
    {
        Debug.Log("Update()");

        cameraObject.transform.localRotation = Quaternion.Euler(playerElevation, 0, 0);
        transform.localRotation = Quaternion.Euler(0, playerDirection, 0);
        transform.position = playerPosition;

        UpdateCursorLock();
    }

    /// <summary>
    /// 0.02秒毎に呼ばれるもの
    /// </summary>
    void FixedUpdate()
    {
        playerDirection = Mathf.Clamp(playerDirection + Input.GetAxis("Mouse X") * mouseXSensitivity, 0, Mathf.PI * 2);
        playerElevation = Mathf.Clamp(playerElevation + Input.GetAxis("Mouse Y") * mouseYSensitivity, -Mathf.PI / 2, Mathf.PI / 2);

        var z = Input.GetAxisRaw("Vertical");    // 前後移動("w", "s"キー)
        var x = Input.GetAxisRaw("Horizontal");    // 左右移動("a", "d"キー)
        var d = Mathf.Sqrt(z * z + x * x);
        var k = speed / d;
        z *= k;
        x *= k;

        var s = Mathf.Sin(playerDirection);
        var c = Mathf.Cos(playerDirection);

        var dx = c * x - s * z;
        var dz = s * x + c * z;

        playerPosition += new Vector3(dx, 0, dz);
    }

    Quaternion ClampRotation(Quaternion q)
    {
#if false
        var cq = q.normalized;
#else
        Quaternion cq;
        cq.x = q.x / q.w;
        cq.y = q.y / q.w;
        cq.z = q.z / q.w;
        cq.w = 1;
#endif
        var elevationAngle = Mathf.Atan(cq.x) * Mathf.Rad2Deg * 2f;
        elevationAngle = Mathf.Clamp(elevationAngle, -90f, 90f);
        cq.x = Mathf.Tan(elevationAngle * Mathf.Deg2Rad / 2f);
        return cq;
    }

    void UpdateCursorLock()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            // マウスカーソルをロックしない
            Cursor.lockState = CursorLockMode.None;
            // マウスカーソルが表示され、マウスカーソルの移動範囲が画面内に制限されるため、
            // FPSの視線移動にも正常にできなくなる。
        }
        else if (Input.GetMouseButton(0))  // 0:左ボタン
        {
            // マウスカーソルをロックする
            Cursor.lockState = CursorLockMode.Locked;
            // マウスカーソルが非表示となり、マウスカーソルの移動範囲が画面内に制限されなくなるため、
            // FPSの視線移動が正常に行われる
        }
    }

}
