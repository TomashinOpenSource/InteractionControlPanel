using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;


[System.Serializable]
public class MenuJsonClass
{
    [System.Serializable]
    public class MenuPanelClass
    {
        [System.Serializable]
        public class MenuButtonClass
        {
            public string Text;
            public string Name;
            public bool isLocked;
            public bool isCorrect;
            [System.NonSerialized]
            public bool isPressed;
            [System.NonSerialized]
            public Transform ButtonObject;
        }

        public List<MenuButtonClass> Buttons;
        public string Name;
    }

    public List<MenuPanelClass> Panels;
}

public class MenuScript : MonoBehaviour
{
    [SerializeField]
    private Transform ButtonPrefab;
    [SerializeField]
    private List<Transform> PanelArray;
    [SerializeField]
    private List<Transform> ButtonArrayPlace;
    [SerializeField]
    private Transform PanelsGO;
    [SerializeField]
    private Transform ButtonPlacesGO;
    [SerializeField]
    private float SpaceBetweenButtons = 0.02f;
    [SerializeField]
    private string JsonFilePath;
    [SerializeField]
    private Material LockedButtonMaterial;

    private MenuJsonClass menu = null;
    void Start()
    {
        CreateMenu();
    }

    private void OnEnable()
    {
        EventManager.StartListening("ButtonDownEvent", ButtonDownHandler);
    }

    private void OnDisable()
    {
        EventManager.StopListening("ButtonDownEvent", ButtonDownHandler);
    }

    private void ButtonDownHandler(string _buttonName)
    {
        bool AllButtonsDown = true;
        foreach (MenuJsonClass.MenuPanelClass panel in menu.Panels)
        {
            foreach (MenuJsonClass.MenuPanelClass.MenuButtonClass button in panel.Buttons)
            {
                if (button.isCorrect && !button.isPressed)
                {
                    if (button.Name == _buttonName) button.isPressed = true;
                    else AllButtonsDown = false;
                }
            }
        }
        if (AllButtonsDown) gameObject.SetActive(false);
        //something code for each menu button
    }

    #region Json

    private void CreateMenu()
    {
        if (ButtonArrayPlace.Count != PanelArray.Count)
        {
            Debug.LogError("ButtonPlaces non-equals Panels!");
            return;
        }

        string jsonText = GetJsonStringFromFile(JsonFilePath);
        if (jsonText == "")
        {
            Debug.Log("Failed to load json!");
            return;
        }

        try
        {
            menu = JsonUtility.FromJson<MenuJsonClass>(jsonText);
        }
        catch (System.ArgumentException e)
        {
            Debug.LogError(e.Message);
            return;
        }

        int currentPanelIndex = 0;
        float delta = ButtonPrefab.localScale.y + SpaceBetweenButtons;
        foreach (MenuJsonClass.MenuPanelClass panel in menu.Panels)
        {
            if (currentPanelIndex == PanelsGO.childCount)
            {
                Debug.LogError("Too low count of panels!");
                break;
            }
            int count = panel.Buttons.Count;
            if (count != 0)
            {
                Transform MenuName = Instantiate(ButtonPrefab, ButtonPlacesGO.GetChild(currentPanelIndex));
                MenuName.localPosition = new Vector3(MenuName.localPosition.x, PanelArray[currentPanelIndex].localScale.y / 2 - delta, MenuName.localPosition.z);
                MenuName.GetComponent<Renderer>().enabled = false;
                MenuName.GetChild(0).GetComponent<TextMeshPro>().text = panel.Name;
                MenuName.GetChild(0).GetComponent<TextMeshPro>().fontStyle = FontStyles.Bold;
                int currentButtonIndex = 1;
                foreach (MenuJsonClass.MenuPanelClass.MenuButtonClass button in panel.Buttons)
                {
                    Transform NewButton = Instantiate(ButtonPrefab, ButtonPlacesGO.GetChild(currentPanelIndex));
                    NewButton.localPosition = new Vector3(NewButton.localPosition.x, currentButtonIndex * -delta + PanelArray[currentPanelIndex].localScale.y / 2 - delta, NewButton.localPosition.z);
                    NewButton.GetChild(0).GetComponent<TextMeshPro>().text = button.Text;
                    NewButton.name = button.Name;
                    if (button.isLocked)
                    {
                        NewButton.GetComponent<Renderer>().material = LockedButtonMaterial;
                        NewButton.GetComponent<Custom3DButtonEventReciever>().isLocked = true;
                        NewButton.tag = "Untagged";
                    }
                    button.ButtonObject = NewButton;
                    button.isPressed = false;
                    currentButtonIndex++;
                }
            }
            currentPanelIndex++;
        }
        Debug.Log("Menu successfully created!");
    }

    private string GetJsonStringFromFile(string path)
    {
        var x = Resources.Load<TextAsset>(path);
        if (x == null) return "";
        return x.text;
    }

    #endregion

    private void Update()
    {
        if (Input.GetKey(KeyCode.I) && Input.GetKey(KeyCode.O) && Input.GetKey(KeyCode.P))
            gameObject.SetActive(false);
    }
}
