using System;
using Unity.Properties;
using UnityEngine.UIElements;

public class PlayerHudData : INotifyBindablePropertyChanged
{
    public event EventHandler<BindablePropertyChangedEventArgs> propertyChanged;

    private int _hunger = 100;
    private int _body = 100;
    private int _mind = 100;

    [CreateProperty] public string HungerText => $"Hunger   {_hunger}";
    [CreateProperty] public string BodyText => $"Body     {_body}";
    [CreateProperty] public string MindText => $"Mind     {_mind}";

    public int Hunger
    {
        get => _hunger;
        set
        {
            int v = Math.Clamp(value, 0, 100);
            if (_hunger == v) return;
            _hunger = v;
            Notify(nameof(HungerText));
        }
    }

    public int Body
    {
        get => _body;
        set
        {
            int v = Math.Clamp(value, 0, 100);
            if (_body == v) return;
            _body = v;
            Notify(nameof(BodyText));
        }
    }

    public int Mind
    {
        get => _mind;
        set
        {
            int v = Math.Clamp(value, 0, 100);
            if (_mind == v) return;
            _mind = v;
            Notify(nameof(MindText));
        }
    }

    private void Notify(string prop)
    {
        propertyChanged?.Invoke(this, new BindablePropertyChangedEventArgs(prop));
    }
}