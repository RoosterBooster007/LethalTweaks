using LethalCompanyInputUtils.Api;
using LethalCompanyInputUtils.BindingPathEnums;
using UnityEngine.InputSystem;

namespace LethalTools
{
    public class InputActions : LcInputActions
    {
        [InputAction(KeyboardControl.F, Name = "Night Vision")]
        public InputAction NightVisionKey { get; set; }

        [InputAction(KeyboardControl.R, Name = "Inf. Stamina")]
        public InputAction StaminaKey { get; set; }

        [InputAction(KeyboardControl.H, Name = "Weightless")]
        public InputAction WeightlessKey { get; set; }

        [InputAction(KeyboardControl.Y, Name = "Speed Toggle")]
        public InputAction SpeedKey { get; set; }

        [InputAction(KeyboardControl.U, Name = "Climb Speed Toggle")]
        public InputAction ClimbSpeedKey { get; set; }

        [InputAction(KeyboardControl.I, Name = "Sink Toggle")]
        public InputAction SinkKey { get; set; }

        [InputAction(KeyboardControl.L, Name = "Disable Fall Damage")]
        public InputAction FallDamageKey { get; set; }

        [InputAction(KeyboardControl.J, Name = "Water Breathing")]
        public InputAction WaterBKey { get; set; }

        [InputAction(KeyboardControl.C, Name = "Cooldown Toggle")]
        public InputAction CooldownKey { get; set; }

        [InputAction(KeyboardControl.P, Name = "Battery Toggle")]
        public InputAction BatteryKey { get; set; }

        [InputAction(KeyboardControl.O, Name = "One-handed")]
        public InputAction OneHKey { get; set; }

        [InputAction(KeyboardControl.K, Name = "Suicide")]
        public InputAction SuicideKey { get; set; }

        [InputAction(KeyboardControl.T, Name = "Name Tags Toggle")]
        public InputAction NameTagKey { get; set; }

        [InputAction(KeyboardControl.N, Name = "Warp Ship")]
        public InputAction ShipKey { get; set; }

        [InputAction(KeyboardControl.M, Name = "Warp Entrance")]
        public InputAction EntranceKey { get; set; }

        [InputAction(KeyboardControl.Z, Name = "Start Game")]
        public InputAction StartKey { get; set; }
    }
}
