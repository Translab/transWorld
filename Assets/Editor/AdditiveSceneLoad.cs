//Create a new folder (Right click in the Assets folder, Create>Folder) and name it “Editor” if one doesn’t already exist
//Put this script in the folder

//This script creates a new menu (Examples) and item (Open Scene). If you choose this item in the Editor, the EditorSceneManager opens the Scene at the given directory (In this case, the “Scene2” Scene is located in the Assets folder). This allows you to open Scenes while still working with the Editor.

using UnityEngine;
using UnityEditor;
using UnityEditor.SceneManagement;

public class AdditiveSceneLoad : MonoBehaviour
{
	// Create a new drop-down menu in Editor named "Examples" and a new option called "Open Scene"
	[MenuItem("Custom/Open Scene")]
	static void OpenScene()
	{
		//Open the Scene in the Editor (do not enter Play Mode)
		EditorSceneManager.OpenScene("Assets/Scenes/Mengyu.unity", OpenSceneMode.Additive);
		//need to add more
	}
}