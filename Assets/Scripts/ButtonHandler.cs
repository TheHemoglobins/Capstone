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
    private GroupBox column3;
    private Button uploadButton;
    private Button exitButton;
    private Label label;

    public GameObject generatorDoc;

    private void Start() {
        FileBrowser.SetFilters(true, new FileBrowser.Filter( "Images", ".jpg", ".png" ));
    }
 
    void OnEnable()
    {
        var uiDocument = GetComponent<UIDocument>();
        var rootVisualElement = uiDocument.rootVisualElement;
 
        GetLabel(rootVisualElement);
      
        this.uploadButton.RegisterCallback<ClickEvent>(ev => {
            FileBrowser.ShowLoadDialog(
                (newPhotoPaths) => {OnSuccess(newPhotoPaths, rootVisualElement);},
				() => { Debug.Log( "Canceled" ); },
				FileBrowser.PickMode.Files, true, null, null, "Select Files", "Select" );
        });

        this.exitButton.RegisterCallback<ClickEvent>(ev => {
            rootVisualElement.style.display = DisplayStyle.None;
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
        this.uploadButton = column1.Q<Button>("UploadButton");

        this.column2 = frame.Q<GroupBox>("UploadedPhotosColumns");

        this.column3 = frame.Q<GroupBox>("InstructionsColumn");
        this.exitButton = column3.Q<Button>("ExitButton");

        Label label = new Label();
        label.AddToClassList("photoTitle");

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
                GeneratePagination(pagination, i);
            }
            this.column2.Add(label);
        };
        VisualElement forwardArrow = new VisualElement();

        //USS is drawing the wrong background image probably because of fileID/guid issues
        forwardArrow.AddToClassList("forwardArrow");
        pagination.Add(forwardArrow);

        this.column2.Add(pagination);
        List<GameObject> frameList = generateFrames();
        assignImages(this.uploadedPhotoPaths, frameList);
    }

    private void SetUploadPhotoColumn(VisualElement uploadLabel, string photoPath){
        this.label = uploadLabel as Label;

        var splicedPhotoPath = Path.GetFileName(photoPath);
        this.label.text = $"Uploaded: {splicedPhotoPath}";
    }

    public void assignImages(string[] paths, List<GameObject> frameList){

        byte[] image;

        for(int i = 0; i < paths.Length; i++){
            Texture2D tex = new Texture2D(2, 2);
            image = File.ReadAllBytes(paths[i]);
            ImageConversion.LoadImage(tex, image);
            frameList[i].GetComponent<Renderer>().material = new Material(Shader.Find("Standard"));
            frameList[i].GetComponent<Renderer>().material.mainTexture = tex;
        };
    }
    private List<GameObject> generateFrames() {

        var frameGenerator = (FrameGeneratorScript) generatorDoc.GetComponent(typeof(FrameGeneratorScript));
        frameGenerator.Generate(this.uploadedPhotoPaths);

        return frameGenerator.getFrameList();
    }
}

//Stretch Goal: display photo previews