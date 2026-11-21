using System;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public class ClassSelectController : MonoBehaviour
{
    [Serializable]
    public struct ClassInfo
    {
        public string elementName;
        public string className;
        [TextArea(2, 5)] public string description;
    }

    [SerializeField]
    private ClassInfo[] classes = new ClassInfo[]
    {
        new ClassInfo
        {
            elementName = "Cahara",
            className = "Mercenary",
            description = "Mercenary, thief, assassin... Whatever brings the silver to the table. Mercenary is known for his dirty tactics in battle and crafty ways of gaining the advantage."
        },
        new ClassInfo
        {
            elementName = "Darce",
            className = "Knight",
            description = "Knight with pure and righteous ways of the warrior. Having been trained for combat since a child, knight excels in close combat and with different weaponry."
        },
        new ClassInfo
        {
            elementName = "Enki",
            className = "Dark priest",
            description = "Bearing no burden on such things as morality and ethics, gives dark priest an edge in blood magic. However, devoting oneself to magic has left his physical body weak."
        },
        new ClassInfo
        {
            elementName = "Ragn",
            className = "Outlander",
            description = "Hardened in the freezing winds of the north, outlander is an epitome of survival. He knows all the tricks to stay alive even in the most impossible of situations."
        }
    };

    [Header("Confirmação")]
    [SerializeField] private string gameplaySceneName = "";

    public string SelectedClass { get; private set; }

    private Label _classNameLabel;
    private Label _classDescLabel;

    private void OnEnable()
    {
        var uiDocument = GetComponent<UIDocument>();
        if (uiDocument == null || uiDocument.rootVisualElement == null) return;

        var root = uiDocument.rootVisualElement;
        _classNameLabel = root.Q<Label>("ClassNameLabel");
        _classDescLabel = root.Q<Label>("ClassDescLabel");

        SelectedClass = null;

        foreach (var info in classes)
        {
            var portrait = root.Q<VisualElement>(info.elementName);
            if (portrait == null) continue;

            string clsName = info.className;
            string desc = info.description;

            portrait.RegisterCallback<ClickEvent>(_ =>
            {
                SelectedClass = clsName;
                if (_classNameLabel != null) _classNameLabel.text = clsName;
                if (_classDescLabel != null) _classDescLabel.text = desc;
            });
        }
    }

    private void Update()
    {
        if (string.IsNullOrEmpty(SelectedClass)) return;

        var keyboard = Keyboard.current;
        bool enterPressed = keyboard != null &&
            (keyboard.enterKey.wasPressedThisFrame || keyboard.numpadEnterKey.wasPressedThisFrame);

        if (enterPressed)
        {
            ConfirmSelection();
        }
    }

    private void ConfirmSelection()
    {
        if (string.IsNullOrEmpty(SelectedClass)) return;

        GameSession.SelectedClass = SelectedClass;
        Debug.Log($"Classe salva na sessão: {GameSession.SelectedClass}");

        if (!string.IsNullOrEmpty(gameplaySceneName))
        {
            SceneManager.LoadScene(gameplaySceneName);
        }
        else
        {
            gameObject.SetActive(false);
        }
    }
}