using MedievalRising.Application;
using MedievalRising.Application.Persistence;
using MedievalRising.Infrastructure.Persistence;
using UnityEngine;
using UnityEngine.InputSystem;

namespace MedievalRising.Presentation
{
    public sealed class GameBootstrap : MonoBehaviour
    {
        private const string DefaultSlot = "starter";
        private const float GameMinutesPerRealSecond = 10f;
        private float _minuteAccumulator;
        private SaveService _saves;

        public GameSession Session { get; private set; }

        public string SaveStatus { get; private set; } = "Not saved";

        private void Awake()
        {
            _saves = new SaveService(new LocalJsonSaveRepository());
            Session = GameSessionFactory.CreateNew("Aldric");
        }

        private void Update()
        {
            _minuteAccumulator += Time.unscaledDeltaTime * GameMinutesPerRealSecond;
            int wholeMinutes = Mathf.FloorToInt(_minuteAccumulator);
            if (wholeMinutes > 0)
            {
                Session.AdvanceMinutes(wholeMinutes);
                _minuteAccumulator -= wholeMinutes;
            }

            Keyboard keyboard = Keyboard.current;
            if (keyboard == null)
            {
                return;
            }

            if (keyboard.f5Key.wasPressedThisFrame)
            {
                _saves.Save(DefaultSlot, Session.World);
                SaveStatus = "Saved";
            }

            if (keyboard.f9Key.wasPressedThisFrame)
            {
                if (_saves.Exists(DefaultSlot))
                {
                    Session = GameSessionFactory.CreateFromWorld(_saves.Load(DefaultSlot));
                    SaveStatus = "Loaded";
                }
                else
                {
                    SaveStatus = "No save found";
                }
            }
        }
    }
}
