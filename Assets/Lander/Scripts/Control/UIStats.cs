using System.Linq;

using TMPro;
using UnityEngine;
using Lander.Shared;

namespace Lander.Control
{
    [DisallowMultipleComponent]
    public class UIStats : MonoBehaviour
    {
        [SerializeField]
        private CraftManager Manager;

        private TextMeshProUGUI TextH;
        private TextMeshProUGUI TextFuel;
        private TextMeshProUGUI TextState;
        private TextMeshProUGUI TextV;
        private TextMeshProUGUI TextW;

        private void Start()
        {
            var text = gameObject.GetComponentsInChildren<TextMeshProUGUI>();
            TextH = text.First(s => s.name == "Text H");
            TextState = text.First(s => s.name == "Text State");
            TextV = text.First(s => s.name == "Text V");
            TextW = text.First(s => s.name == "Text W");
            TextFuel = text.First(s => s.name == "Text Fuel");
        }

        private void Update()
        {
            TextH.text = $"h = {Manager.State.Movement.Height.ShortFormat()}";
            TextState.text = $"State = {Manager.State.Name}";
            TextV.text = $"v = {Manager.State.Movement.Velocity.magnitude.ShortFormat()} {Manager.State.Movement.Velocity.ShortFormat()}";
            TextW.text = $"ω = {Manager.State.Movement.AngularVelocity.magnitude.ShortFormat()} {Manager.State.Movement.AngularVelocity.ShortFormat()}";
            TextFuel.text = $"Fuel = {1.00}";
        }
    }
}