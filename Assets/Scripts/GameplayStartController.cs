using System;
using Unity.Properties;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UIElements;

public class GameplayStartController : MonoBehaviour
{
    [SerializeField]
    private CharacterData[] characters = new CharacterData[]
    {
        new CharacterData { className = "Mercenary", phobia = "Phasmophobia" },
        new CharacterData { className = "Knight", phobia = "Teratophobia" },
        new CharacterData { className = "Dark priest", phobia = "Rhabdophobia" },
        new CharacterData { className = "Outlander", phobia = "Zoophobia" }
    };

    [SerializeField] private float decayInterval = 1.5f;

    private PlayerHudData _hud;
    private VisualElement _escMenu;
    private Button _eatBtn;
    private Button _restBtn;
    private float _timer;
    private bool _open;

    private void OnEnable()
    {
        var doc = GetComponent<UIDocument>();
        if (doc == null || doc.rootVisualElement == null) return;
        var root = doc.rootVisualElement;

        string chosen = string.IsNullOrEmpty(GameSession.SelectedClass) ? "Mercenary" : GameSession.SelectedClass;
        CharacterData active = Array.Find(characters, c => c.className == chosen) ?? characters[0];

        // Fundo que varia conforme o personagem (ragnbegin, enkibegin, darcebegin, caharabegin)
        var centerImg = root.Q<VisualElement>("CharacterCenterImage");
        if (centerImg != null && active != null)
        {
            Sprite bg = active.beginBackgroundSprite != null ? active.beginBackgroundSprite : active.characterSprite;
            if (bg != null)
                centerImg.style.backgroundImage = new StyleBackground(bg);
        }

        _escMenu = root.Q<VisualElement>("EscMenuPanel");
        if (_escMenu == null) return;
        _escMenu.style.display = DisplayStyle.None;

        // Sprite do personagem selecionado no retrato do menu Esc
        var portrait = _escMenu.Q<VisualElement>("EscPortrait");
        if (portrait != null && active != null)
        {
            Sprite portraitSpr = active.portraitSprite != null ? active.portraitSprite : active.characterSprite;
            if (portraitSpr != null)
                portrait.style.backgroundImage = new StyleBackground(portraitSpr);
        }

        var classLbl = _escMenu.Q<Label>("EscClassLabel");
        if (classLbl != null && active != null) classLbl.text = active.className;

        var phobiaLbl = _escMenu.Q<Label>("EscPhobiaLabel");
        if (phobiaLbl != null && active != null) phobiaLbl.text = active.phobia;

        _hud = new PlayerHudData();
        _escMenu.dataSource = _hud;

        BindLabel(_escMenu.Q<Label>("HungerLabel"), nameof(PlayerHudData.HungerText));
        BindLabel(_escMenu.Q<Label>("BodyLabel"), nameof(PlayerHudData.BodyText));
        BindLabel(_escMenu.Q<Label>("MindLabel"), nameof(PlayerHudData.MindText));

        _eatBtn = _escMenu.Q<Button>("EatButton");
        if (_eatBtn != null) _eatBtn.clicked += OnEat;

        _restBtn = _escMenu.Q<Button>("RestButton");
        if (_restBtn != null) _restBtn.clicked += OnRest;
    }

    private void OnDisable()
    {
        if (_eatBtn != null) _eatBtn.clicked -= OnEat;
        if (_restBtn != null) _restBtn.clicked -= OnRest;
    }

    private void Update()
    {
        if (Keyboard.current != null && Keyboard.current.escapeKey.wasPressedThisFrame)
        {
            _open = !_open;
            if (_escMenu != null)
                _escMenu.style.display = _open ? DisplayStyle.Flex : DisplayStyle.None;
        }

        if (_hud == null) return;
        _timer += Time.deltaTime;
        if (_timer >= decayInterval)
        {
            _timer = 0f;
            _hud.Hunger -= 2;
            _hud.Mind -= 1;
        }
    }

    private void BindLabel(Label lbl, string propName)
    {
        if (lbl == null) return;
        lbl.SetBinding("text", new DataBinding
        {
            dataSourcePath = new PropertyPath(propName),
            bindingMode = BindingMode.ToTarget
        });
    }

    private void OnEat()
    {
        if (_hud != null) _hud.Hunger += 20;
    }

    private void OnRest()
    {
        if (_hud != null) _hud.Mind += 20;
    }
}