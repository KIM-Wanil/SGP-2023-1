using UnityEngine;
using System.Collections;

public class PlayerControl : MonoBehaviour {

    public Transform FPScam;
    float mouseX, mouseY;
    float speed;

	void Start () 
    {
        Cursor.lockState = CursorLockMode.Locked;
        speed = 4f;
	}
	
	// Update is called once per frame
    void Update () 
    {
        Move();
        CamControl();
    }

    void Move()
    {
        Vector2 moveInput = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
        Vector3 lookForward = new Vector3(FPScam.transform.forward.x, 0, FPScam.transform.forward.z).normalized;
        Vector3 lookRight = new Vector3(FPScam.transform.right.x, FPScam.transform.forward.y, FPScam.transform.right.z).normalized;
        Vector3 moveDir = lookForward * moveInput.y + lookRight * moveInput.x;

        transform.forward = lookForward;
        if (transform.position.y < 0.0f)
            transform.position = new Vector3(transform.position.x, 0.0f, transform.position.z);
        transform.position += moveDir * Time.deltaTime * speed;
    }

    void CamControl()
    {
        mouseX += Input.GetAxis("Mouse X");
        mouseY -= Input.GetAxis("Mouse Y");

        mouseY = Mathf.Clamp(mouseY, -30, 30);
        FPScam.transform.eulerAngles = new Vector3(mouseY, mouseX, 0);
    }

}
