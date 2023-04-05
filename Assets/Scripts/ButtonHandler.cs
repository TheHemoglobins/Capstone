using UnityEngine;
using UnityEngine.UIElements;
using SimpleFileBrowser;
using System.Collections.Generic;
using System.Collections;

public class ButtonHandler : MonoBehaviour
{
    private string[] uploadedPhotoPaths;
    private VisualElement frame;
    private GroupBox column1;
    private GroupBox column2;
    private Button button;
    private Label label;

    private void Start() {
        FileBrowser.SetFilters(true, new FileBrowser.Filter( "Images", ".jpg", ".png" ));
    }
 
    void OnEnable()
    {
        var uiDocument = GetComponent<UIDocument>();
        var rootVisualElement = uiDocument.rootVisualElement;
 
        List<VisualElement> labelList = GetListOfLabels(rootVisualElement);
      
        this.button.RegisterCallback<ClickEvent>(ev => {
            FileBrowser.ShowLoadDialog(
                (newPhotoPaths) => {OnSuccess(newPhotoPaths, labelList);},
				() => { Debug.Log( "Canceled" ); },
				FileBrowser.PickMode.Files, true, null, null, "Select Files", "Select" );
        });
    }
/* 
    private void Update() {
        for(int i = 0; i < this.uploadedPhotoPaths.Length; i++){
            this.newText = this.uploadedPhotoPaths[i];
            Debug.Log(newText);
            setText();
        }
    }
 */
    public List<VisualElement> GetListOfLabels(VisualElement rootVisualElement){

        this.frame = rootVisualElement.Q<VisualElement>("Canvas");
        this.column1 = frame.Q<GroupBox>("FrontMatterColumn");
        this.button = column1.Q<Button>("UploadButton");

        this.column2 = frame.Q<GroupBox>("UploadedPhotosColumns");

        List<VisualElement> labelList = column2.Query(className: "photoTitle").ToList();

        return labelList;
    }

    public void OnSuccess(string[] paths, List<VisualElement> labelList){
        this.uploadedPhotoPaths = paths; 
        for(var i = 0; i < this.uploadedPhotoPaths.Length; i++){
            setText(labelList[i], uploadedPhotoPaths[i]);
        };
    }

    public void setText(VisualElement uploadLabel, string photoPath){
        this.label = uploadLabel as Label;
        var splicedPhotoPath = photoPath;
        this.label.text = $"Uploaded: {splicedPhotoPath}";
    }
/*  
    IEnumerator openFileBrowserCoroutine()
    {
        yield return FileBrowser.WaitForLoadDialog(FileBrowser.PickMode.Files, true, null, null, "Load Files", "Select");
        if(FileBrowser.Success)
		{
            for(int i = 0; i < FileBrowser.Result.Length; i++){
                this.uploadedPhotoPaths.Add(FileBrowser.Result[i]);
            }
		};

    } */
}