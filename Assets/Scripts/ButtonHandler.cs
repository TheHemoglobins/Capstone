using UnityEngine;
using UnityEngine.UIElements;
using SimpleFileBrowser;
using System.Collections.Generic;
using System.Collections;

public class ButtonHandler : MonoBehaviour
{
    private List<string> uploadedPhotoPaths = new List<string>();
    private VisualElement frame;
    private GroupBox column1;
    private GroupBox column2;
    private Button button;
    private Label label;
    private string newText;

    private void Start() {
        FileBrowser.SetFilters(true, new FileBrowser.Filter( "Images", ".jpg", ".png" ));
    }
 
    void OnEnable()
    {
        var uiDocument = GetComponent<UIDocument>();
        var rootVisualElement = uiDocument.rootVisualElement;
 
        this.frame = rootVisualElement.Q<VisualElement>("Canvas");
        this.column1 = frame.Q<GroupBox>("FrontMatterColumn");
        this.button = column1.Q<Button>("UploadButton");

        this.column2 = frame.Q<GroupBox>("UploadedPhotosColumns");
        this.label = column2.Q<Label>("UploadedPhotoTitle");
 
        this.button.RegisterCallback<ClickEvent>(ev => {
            FileBrowser.ShowLoadDialog( ( uploadedPhotoPaths ) => { Debug.Log( "Selected: " + uploadedPhotoPaths[0] ); },
								   () => { Debug.Log( "Canceled" ); },
								   FileBrowser.PickMode.Files, true, null, null, "Select Files", "Select" );
        });
    }

    private void Update() {
        for(int i = 0; i < FileBrowser.Result.Length; i++){
            this.newText = uploadedPhotoPaths[i];
            Debug.Log(newText);
            setText();
        }
    }


    public void setText(){
        this.label.text = this.newText;
    }
 
    IEnumerator openFileBrowserCoroutine()
    {
        yield return FileBrowser.WaitForLoadDialog(FileBrowser.PickMode.Files, true, null, null, "Load Files", "Select");
        if(FileBrowser.Success)
		{
            for(int i = 0; i < FileBrowser.Result.Length; i++){
                this.uploadedPhotoPaths.Add(FileBrowser.Result[i]);
            }
		};

    }
}