﻿using System;
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

namespace GSFWeapon.Control
{
    public class Slider<T> : BaseControl<T>
    {
        public float Min;
        public float Max;
        public float Standard;

        public Slider(
            IMyTerminalBlock block,
            string internalName,
            string title,
            float min = 0.0f,
            float max = 100.0f,
            float standard = 10.0f)
            : base(block, internalName, title)
        {
            Min = min;
            Max = max;
            Standard = standard;
            CreateUI();
        }

        public override void OnCreateUI()
        {
            var slider = MyAPIGateway.TerminalControls.CreateControl<IMyTerminalControlSlider, T>(InternalName);
            slider.Visible = ShowControl;
            //slider.SetLogLimits(Min, Max);
            slider.SetLimits(Min, Max);
            slider.Getter = Getter;
            slider.Setter = Setter;
            slider.Enabled = Enabled;
            slider.Writer = Writer;
            slider.Title = VRage.Utils.MyStringId.GetOrCompute(Title);
            MyAPIGateway.TerminalControls.AddControl<T>(slider);
        }

        public virtual void Writer(IMyTerminalBlock block, StringBuilder builder)
        {
            builder.Clear();
            builder.Append(Getter(block).ToString());
        }

        public virtual float Getter(IMyTerminalBlock block)
        {
            float value = Standard;
            if (MyAPIGateway.Utilities.GetVariable<float>(block.EntityId.ToString() + InternalName, out value))
            {
                return value;
            }
            return Standard;
        }

        public virtual void Setter(IMyTerminalBlock block, float value)
        {
            value = Math.Max(Math.Min(value, Max), Min);
            MyAPIGateway.Utilities.SetVariable<float>(block.EntityId.ToString() + InternalName, value);
        }
    }
}