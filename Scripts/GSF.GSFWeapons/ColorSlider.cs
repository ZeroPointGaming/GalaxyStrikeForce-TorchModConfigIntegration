using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using System.Runtime.InteropServices;
using System.Collections;
using Sandbox.ModAPI;
using VRage.ObjectBuilders;
using Sandbox.ModAPI.Interfaces.Terminal;
using VRage.ModAPI;
using GSF.GSFWeapons;
using VRageMath;


namespace GSFWeapon.Control
{
    public class ColorSlider<T> : BaseControl<T>
    {
        public Color Min;

        public ColorSlider(
            IMyTerminalBlock block,
            string internalName,
            string title,
            Color min,
            float standard = 10.0f)
            : base(block, internalName, title)
        {
            Min = min;
            CreateUI();
        }

        public override void OnCreateUI()
        {
            var slider = MyAPIGateway.TerminalControls.CreateControl<IMyTerminalControlColor, T>(InternalName);
            slider.Visible = ShowControl;
            slider.Getter = Getter;
            slider.Setter = Setter;
            slider.Enabled = Enabled;
            slider.Title = VRage.Utils.MyStringId.GetOrCompute(Title);
            MyAPIGateway.TerminalControls.AddControl<T>(slider);
        }

        public virtual Color Getter(IMyTerminalBlock block)
        {
            Color value = Min;
            if (MyAPIGateway.Utilities.GetVariable<Color>(block.EntityId.ToString() + InternalName, out value))
            {
                return value;
            }
            return Min;
        }

        public virtual void Setter(IMyTerminalBlock block, Color value)
        {
           
            MyAPIGateway.Utilities.SetVariable<Color>(block.EntityId.ToString() + InternalName, value);
        }
    }
}