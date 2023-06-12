using System;
using System.Linq;

using TMPro;
using UnityEngine;

using Lander.Shared;

namespace Lander.Control
{
    /// <summary>Компонент, отвечающий за отрисовку UI.</summary>
    [DisallowMultipleComponent]
    public class StatsDisplay : MonoBehaviour
    {
        [SerializeField]
        private CraftManager Manager;

        [SerializeField]
        private TimeSpan InfoDisplayTime = new TimeSpan(0, 0, 2);

        private TextMeshProUGUI TextH;
        private TextMeshProUGUI TextFuel;
        private TextMeshProUGUI TextState;
        private TextMeshProUGUI TextV;
        private TextMeshProUGUI TextW;

        private static TextMeshProUGUI TextInfo;
        private static DateTime infoDisplayedAt;

        private void Start()
        {
            var text = gameObject.GetComponentsInChildren<TextMeshProUGUI>();
            TextState = text.First(s => s.name == "Text State");
            TextFuel = text.First(s => s.name == "Text Fuel");
            TextH = text.First(s => s.name == "Text H");
            TextV = text.First(s => s.name == "Text V");
            TextW = text.First(s => s.name == "Text W");
            TextInfo = text.FirstOrDefault(s => s.name == "Text Info");
        }

        private void Update()
        {
            TextState.text = $"State = {Manager.State.Name}";
            TextFuel.text = $"Fuel = {1.00}";
            TextH.text = $"h = {Manager.State.Movement.Height.ShortFormat()}";
            TextV.text = $"v = {Manager.State.Movement.Velocity.magnitude.ShortFormat()} {Manager.State.Movement.Velocity.ShortFormat()}";
            TextW.text = $"ω = {Manager.State.Movement.AngularVelocity.magnitude.ShortFormat()} {Manager.State.Movement.AngularVelocity.ShortFormat()}";

            if (TextInfo != null && DateTime.Now - infoDisplayedAt > InfoDisplayTime)
            {
                TextInfo.text = "";
            }
        }

        public static void ShowInfo(string text)
        {
            if (TextInfo != null)
            {
                TextInfo.text = text;
                infoDisplayedAt = DateTime.Now;
            }
        }
    }
}