using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FpsController : MonoBehaviour
{
    /// <summary>
    /// �ړ����x
    /// </summary>
    const float speed = 0.1F;

    /// <summary>
    /// �����ړ����x(�}�E�X���x)
    /// </summary>
    const float mouseXSensitivity = 3F;
    const float mouseYSensitivity = 3F;

    // �v���C���[�̈ʒu
    Vector3 playerPosition;
    // �v���C���[�̌���(���E����)
    float playerDirection;
    // �v���C���[�̌���(�㉺����)
    float playerElevation;

    public GameObject cameraObject;
    //Quaternion cameraRotation;
    //Quaternion playerRotation;

    // Start is called before the first frame update
    void Start()
    {
//        cameraRotation = cameraObject.transform.localRotation;
        Cursor.lockState = CursorLockMode.Locked;   // �}�E�X�J�[�\�����\��
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
    /// 0.02�b���ɌĂ΂�����
    /// </summary>
    void FixedUpdate()
    {
        playerDirection = Mathf.Clamp(playerDirection + Input.GetAxis("Mouse X") * mouseXSensitivity, 0, Mathf.PI * 2);
        playerElevation = Mathf.Clamp(playerElevation + Input.GetAxis("Mouse Y") * mouseYSensitivity, -Mathf.PI / 2, Mathf.PI / 2);

        var z = Input.GetAxisRaw("Vertical");    // �O��ړ�("w", "s"�L�[)
        var x = Input.GetAxisRaw("Horizontal");    // ���E�ړ�("a", "d"�L�[)
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
            // �}�E�X�J�[�\�������b�N���Ȃ�
            Cursor.lockState = CursorLockMode.None;
            // �}�E�X�J�[�\�����\������A�}�E�X�J�[�\���̈ړ��͈͂���ʓ��ɐ�������邽�߁A
            // FPS�̎����ړ��ɂ�����ɂł��Ȃ��Ȃ�B
        }
        else if (Input.GetMouseButton(0))  // 0:���{�^��
        {
            // �}�E�X�J�[�\�������b�N����
            Cursor.lockState = CursorLockMode.Locked;
            // �}�E�X�J�[�\������\���ƂȂ�A�}�E�X�J�[�\���̈ړ��͈͂���ʓ��ɐ�������Ȃ��Ȃ邽�߁A
            // FPS�̎����ړ�������ɍs����
        }
    }

}
