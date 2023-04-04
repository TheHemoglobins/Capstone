using UnityEngine;
using UnityEngine.UIElements;
using SimpleFileBrowser;
using UnityEngine;
using System.Collections.Generic;
using System.Collections;

public class ButtonHandler : MonoBehaviour
{
    private List<string> uploadedPhotoPaths = new List<string>();
    private VisualElement frame;
    private GroupBox column;
    private Button button;

    private void Start() {
        FileBrowser.SetFilters(true, new FileBrowser.Filter( "Images", ".jpg", ".png" ));
    }
 
    void OnEnable()
    {
        var uiDocument = GetComponent<UIDocument>();
        var rootVisualElement = uiDocument.rootVisualElement;
 
        this.frame = rootVisualElement.Q<VisualElement>("Canvas");
        this.column = frame.Q<GroupBox>("FrontMatterColumn");
        this.button = column.Q<Button>("UploadButton");
 
        this.button.RegisterCallback<ClickEvent>(ev => openFileBrowser());
    }
 
    IEnumerator openFileBrowser()
    {
        yield return FileBrowser.WaitForLoadDialog(FileBrowser.PickMode.FilesAndFolders, true, null, null, "Load Files and Folders", "Load");
        if(FileBrowser.Success)
		{
            for(int i = 0; i < FileBrowser.Result.Length; i++){
                this.uploadedPhotoPaths.Add(FileBrowser.Result[i]);
            }
		};

    }
}