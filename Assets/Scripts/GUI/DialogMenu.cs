using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using TMPro;

public class DialogMenu : BaseMenu {

	public TextMeshProUGUI dialogText;
	public bool selectionYes = false;
	public GameObject selectionItemPrefab;
	public Transform selectionGroup;

	public override void InitMenu ()
	{
		base.InitMenu ();
		for (int i=0; i<2; i++) {
			GameObject g = Instantiate (selectionItemPrefab);
			g.transform.SetParent (selectionGroup);
			g.transform.localPosition = Vector3.zero;
			g.transform.localScale = Vector3.one;
			TextMeshProUGUI t = g.transform.GetChild (1).GetComponent<TextMeshProUGUI> ();
			Button b = g.GetComponent<Button>();
			b.onClick.RemoveAllListeners();
			if (i==0) {
				t.text = "Yes";
				b.onClick.AddListener(()=>SetSelection(true));
			} else {
				t.text = "No";
				b.onClick.AddListener(()=>SetSelection(false));
			}
			b.onClick.AddListener(()=>ShowOutro());

			OnEscapeAction += EscapePressed;
		}
	}

	public void ShowDialog(string textDialog)
	{
		dialogText.text = textDialog;
		ShowIntro ();
	}
	void SetSelection(bool select)
	{
		selectionYes = select;
	}

	void EscapePressed() {
		SetSelection (false);
		ShowOutro ();
	}
}
