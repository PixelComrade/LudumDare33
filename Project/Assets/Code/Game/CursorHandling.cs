using UnityEngine;
using System.Collections;

public class CursorHandling : MonoBehaviour {

//	public WinLose player = null;

	private CursorLockMode wantedMode;
	
	void SetCursorState (){
		Cursor.lockState = wantedMode;
		if (wantedMode == CursorLockMode.Locked) {
			Cursor.visible = false;
		} else {
			Cursor.visible = true;
		}
	}
	
	void FixedUpdate () {
//		if (player.inMenu == false) {
			if (Input.GetKeyDown(KeyCode.Escape) && wantedMode != CursorLockMode.None) {
				Cursor.lockState = wantedMode = CursorLockMode.None;
				SetCursorState();
			}
			if (Input.GetMouseButtonDown(0) && wantedMode != CursorLockMode.Locked) {
				Cursor.lockState = wantedMode = CursorLockMode.Locked;
				SetCursorState();
			}
//		} else if (wantedMode == CursorLockMode.Locked) {
//			Cursor.lockState = wantedMode = CursorLockMode.None;
//			SetCursorState();
//		}
	}
}
