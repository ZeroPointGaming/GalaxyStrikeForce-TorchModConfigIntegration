using System;
using System.Collections.Generic;
using System.Text;
using Sandbox.ModAPI;
using VRage.ObjectBuilders;
using Sandbox.ModAPI.Interfaces.Terminal;
using GSF.GSFWeapons;

namespace GSFWeapon.Control
{
    public class Checkbox<T> : BaseControl<T>
    {
        public bool DefaultValue;

        public Checkbox(
            IMyTerminalBlock block,
            string internalName,
            string title,
            bool defaultValue = true)
            : base(block, internalName, title)
        {
            DefaultValue = defaultValue;
            CreateUI(); // Check This was not here and instead was in the RefreshCheckBox class... not sure why  // Maybe fails on first use?

            bool temp;
            if (!MyAPIGateway.Utilities.GetVariable<bool>(block.EntityId.ToString() + InternalName, out temp))
            {
                MyAPIGateway.Utilities.SetVariable<bool>(block.EntityId.ToString() + InternalName, defaultValue);
            }
        }

        public override void OnCreateUI()
        {
            var checkbox = MyAPIGateway.TerminalControls.CreateControl<IMyTerminalControlCheckbox, T>(InternalName);
            checkbox.Visible = ShowControl;
            checkbox.Getter = Getter;
            checkbox.Setter = Setter;
            checkbox.Title = VRage.Utils.MyStringId.GetOrCompute(Title);
            MyAPIGateway.TerminalControls.AddControl<T>(checkbox);
        }

        public virtual bool Getter(IMyTerminalBlock block)
        {

            bool value = DefaultValue;
            MyAPIGateway.Utilities.GetVariable<bool>(block.EntityId.ToString() + InternalName, out value);
            Logging.writeLine(String.Format("{0} - Checkbox value is {1}", DateTime.Now.ToString("MM-dd-yy_HH-mm-ss-fff"), value));
            return value;
        }

        public virtual void Setter(IMyTerminalBlock block, bool newState)
        {
            MyAPIGateway.Utilities.SetVariable<bool>(block.EntityId.ToString() + InternalName, newState);
        }

    }
}