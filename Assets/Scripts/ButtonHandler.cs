using UnityEngine;
using UnityEngine.UIElements;
using SimpleFileBrowser;
using System.Collections.Generic;
using System.Collections;
using System.IO;

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
 
        GetLabel(rootVisualElement);
      
        this.button.RegisterCallback<ClickEvent>(ev => {
            FileBrowser.ShowLoadDialog(
                (newPhotoPaths) => {OnSuccess(newPhotoPaths, rootVisualElement);},
				() => { Debug.Log( "Canceled" ); },
				FileBrowser.PickMode.Files, true, null, null, "Select Files", "Select" );
        });
    }

    public GroupBox GeneratePaginationArrows(VisualElement root){
        this.frame = root.Q<VisualElement>("Canvas");
        this.column2 = frame.Q<GroupBox>("UploadedPhotosColumns");
        
        GroupBox pagination = new GroupBox();
        VisualElement backArrow = new VisualElement();
        
        backArrow.AddToClassList("backArrow");

        pagination.Add(backArrow);
        
        pagination.AddToClassList("pagination");

        column2.Add(pagination);

        return pagination;
    }

    public void GeneratePagination(GroupBox pag, int n){

        Button pageButton = new Button();
        pageButton.text = $"{n}";

        //Issues changing the page number to reflect 1 versus the number of interations
        if(n > 0){
            pageButton.text = $"{n-3}";
        };

        pag.Add(pageButton);

    }

    public VisualElement GetLabel(VisualElement root){

        this.frame = root.Q<VisualElement>("Canvas");
        this.column1 = frame.Q<GroupBox>("FrontMatterColumn");
        this.button = column1.Q<Button>("UploadButton");

        this.column2 = frame.Q<GroupBox>("UploadedPhotosColumns");

        Label label = new Label();
        label.AddToClassList("photoTitle");
        column2.Add(label);

        return label as VisualElement;
    }

    public void OnSuccess(string[] paths, VisualElement root){
        this.uploadedPhotoPaths = paths;

        List<VisualElement> labelList = new List<VisualElement>();

        GroupBox pagination = GeneratePaginationArrows(root);

        for(int i = 0; i < paths.Length; i++){
            VisualElement label = GetLabel(root);
            labelList.Add(label);
            SetUploadPhotoColumn(labelList[i], uploadedPhotoPaths[i]);
            if((i % 3) == 0){
                //Pagination is being generated before the labels thus placing it as the first element
                //unknown as to why this is being generated first
                GeneratePagination(pagination, i);
            }
        };
        VisualElement forwardArrow = new VisualElement();
        //USS is drawing the wrong background image probably because of fileID/guid issues
        forwardArrow.AddToClassList("forwardArrow");
        pagination.Add(forwardArrow);
    }

    public void SetUploadPhotoColumn(VisualElement uploadLabel, string photoPath){
        this.label = uploadLabel as Label;

        var splicedPhotoPath = Path.GetFileName(photoPath);
        this.label.text = $"Uploaded: {splicedPhotoPath}";
    }
}

//Stretch Goal: display photo previews