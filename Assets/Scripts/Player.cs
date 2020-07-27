using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
  public VariableJoystick joystick;
  private float speed = 0.05f;
	public static bool sick = true;
	private bool joystickChanged = false;

  // Start is called before the first frame update
  void Start() {
    joystick = GameObject.FindGameObjectWithTag("Joystick").GetComponent<VariableJoystick>();
		transform.position = new Vector3(0,-1.5f,0);
  }

  // Update is called once per frame
  void Update() {
    Move();

		if (joystick.Direction.x != 0 && !joystickChanged) {
			joystickChanged = true;
      joystick.SetMode(JoystickType.Dynamic);
			joystick.gameObject.transform.Find("Background").gameObject.SetActive(true);
		}
  }

  private void Move() {
		if ((joystick.Direction.x > 0 && transform.position.x < 2.8) || (joystick.Direction.x < 0 && transform.position.x > -2.8))
		transform.position = new Vector3(transform.position.x,-1.5f,transform.position.z);
		transform.rotation = Quaternion.identity;
		transform.Translate(Vector3.right * speed * joystick.Direction.x);
  }

  private void OnTriggerEnter2D(Collider2D other) {
   
  }
}