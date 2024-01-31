using UnityEditor;
using UnityEngine;

public class ResetSaveData{
	[MenuItem("Window/Rai_Framework/Reset Save Data %#r")]
	private static void ResetSave (){				
		Reset ();
	}

	[MenuItem("Window/Rai_Framework/Open Save File %#o")]
	private static void OpenSave (){
		Application.OpenURL (Application.persistentDataPath);
	}

	public static void Reset(){
		Rai_SaveLoad.DeleteProgress();
		EditorUtility.DisplayDialog("Rai_Framework",
			"Save data reset successfull !", 
			"Ok");
	}
}

