using Scripts.Local.Helper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using UnityEngine;


[RequireComponent(typeof(Camera))]
public class CreatorCamera : MonoBehaviour
{
    Vector3 defaultPosition;
    Quaternion defaultRotation;

    public Transform target;
    public bool rotate = true, drag = false, smooth = true;
    public float distance = 5.0f;
    public float xSpeed = 120.0f, stateXSpeed = 60.0f;
    public float ySpeed = 120.0f, stateYSpeed = 0;
    public float scrollSpeed = 2f;
    public float height = 1;

    public float yMinLimit = -20f;
    public float yMaxLimit = 80f;

    public float distanceMin = .5f;
    public float distanceMax = 15f;

    public float smoothTime = 2f;

    float rotationYAxis = 0.0f;
    float rotationXAxis = 0.0f;

    float velocityX = 0.0f;
    float velocityY = 0.0f;

    // Use this for initialization
    void Start()
    {
        defaultPosition = transform.position;
        defaultRotation = transform.rotation;

        Vector3 angles = transform.eulerAngles;
        rotationYAxis = angles.y;
        rotationXAxis = angles.x;

        // Make the rigid body not change rotation
        if (GetComponent<Rigidbody>())
        {
            GetComponent<Rigidbody>().freezeRotation = true;
        }
    }

    void Update()
    {
        if (!target || !target.gameObject.activeInHierarchy)
        {
            var player = GameObjectHelper.FindObjectWithLayer("Local", "Player");
            if (player)
                target = player.transform;
        }

        else
        {
            drag = Input.GetMouseButton(1);
            if (Input.GetKeyDown(KeyCode.X))
                rotate = !rotate;
        }
    }

    void LateUpdate()
    {
        if (target)
        {
            if (drag)
            {
                velocityX += xSpeed * Input.GetAxis("Mouse X") * 0.02f;
                velocityY += ySpeed * Input.GetAxis("Mouse Y") * 0.02f;
            }
            else
            {
                velocityX += (rotate ? stateXSpeed : 0) * 0.02f;
                velocityY += (rotate ? stateYSpeed : 0) * 0.02f;
            }

            var scroll = Input.GetAxis("Mouse ScrollWheel");
            if (scroll != 0)
                distance -= scrollSpeed * scroll * (smooth ? Time.deltaTime : 1);
            distance = Mathf.Clamp(distance, distanceMin, distanceMax);

            rotationYAxis += velocityX * (smooth ? Time.deltaTime : 1);
            rotationXAxis -= velocityY * (smooth ? Time.deltaTime : 1);

            rotationXAxis = ClampAngle(rotationXAxis, yMinLimit, yMaxLimit);

            Quaternion fromRotation = Quaternion.Euler(transform.rotation.eulerAngles.x, transform.rotation.eulerAngles.y, 0);
            Quaternion toRotation = Quaternion.Euler(rotationXAxis, rotationYAxis, 0);
            Quaternion rotation = toRotation;

            Vector3 negDistance = new Vector3(0.0f, height, -distance);
            Vector3 position = rotation * negDistance + target.position;

            transform.rotation = rotation;
            transform.position = position;

            velocityX = Mathf.Lerp(velocityX, 0, Time.deltaTime * smoothTime);
            velocityY = Mathf.Lerp(velocityY, 0, Time.deltaTime * smoothTime);
        }
        else
        {
            transform.position = defaultPosition;
            transform.rotation = defaultRotation;
        }
    }

    public static float ClampAngle(float angle, float min, float max)
    {
        if (angle < -360F)
            angle += 360F;
        if (angle > 360F)
            angle -= 360F;
        return Mathf.Clamp(angle, min, max);
    }

    private Vector2 size = Vector2.zero;
    private GUIStyle style = null;
    const string content = "Press X to toggle camera auto-rotation!";

    private void OnGUI()
    {
        if (size == Vector2.zero)
            size = GUI.skin.label.CalcSize(new GUIContent(content));

        if(style == null)
        {
            style = new GUIStyle(GUI.skin.label);
            style.active.textColor = Color.black;
            style.focused.textColor = Color.black;
            style.normal.textColor = Color.black;
            style.hover.textColor = Color.black;
        }

        if(target != null)
            GUI.Label(new Rect(Screen.width - size.x, 0, size.x, size.y), content, style);
    }
}