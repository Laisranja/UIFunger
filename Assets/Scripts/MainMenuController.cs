using UnityEngine;
using UnityEngine.UIElements;

public class MainMenuController : MonoBehaviour
{
    [Header("Referências")]
    [SerializeField] private string newGameButtonName = "NewGameButton";
    [SerializeField] private GameObject classSelectObject;

    private Button _newGameButton;

    private void OnEnable()
    {
        var uiDocument = GetComponent<UIDocument>();
        if (uiDocument == null || uiDocument.rootVisualElement == null) return;

        _newGameButton = uiDocument.rootVisualElement.Q<Button>(newGameButtonName);
        if (_newGameButton != null)
        {
            _newGameButton.clicked += OnNewGameClicked;
        }
    }

    private void OnDisable()
    {
        if (_newGameButton != null)
        {
            _newGameButton.clicked -= OnNewGameClicked;
        }
    }

    private void OnNewGameClicked()
    {
        if (classSelectObject != null)
        {
            classSelectObject.SetActive(true);
        }

        gameObject.SetActive(false);
    }
}