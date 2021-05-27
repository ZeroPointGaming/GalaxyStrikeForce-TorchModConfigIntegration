﻿//-----=====File Information=====-----
///File generated by Justin's SE Weapon Template Manager
///File Type: Beam Weapon Manager
///To donate or contact author of this software contact via discord @ Justin#1619

using VRageMath;
using GSF.Config;

namespace DefaultTurret
{
    public static class DT_Default_Settings
    {
        public static BeamWeaponDefaultInfo DefaultTurret = new BeamWeaponDefaultInfo(
        "DefaultAmmoClass",
        false,
        1,
        200f,
        250f,
        10f,
        10f,
        .05f,
        1
        );
    }

    public static class DT_Color_Settings
    {
        public static BeamWeaponColor DefaultTurret = new BeamWeaponColor(
        255, //Explosive Beam Color RED
        69, //Explosive Beam Color BLUE
        0, //Explosive Beam Color GREEN
        255, //Explosive Beam Aux RED
        165, //Explosive Beam Aux BLUE
        0, //Explosive Beam Aux GREEN
        184, //Main Color RED
        134, //Main Color BLUE
        11, //Main Color GREEN
        255, //Aux Color RED
         215, //Aux Color BLUE
         0, //Aux Color GREEN
        .005f,
        .025f
        );
    }

    public static class DT_Heat_Settings
    {
        public static BeamWeaponExtendedHeatInfo DefaultTurret = new BeamWeaponExtendedHeatInfo(
        2000f,
        1000f,
        10f,
        5f,
        5f,
        5f,
        10,
        300,
        5,
        10
        );
    }

    public static class DT_Default_Ui_Settings
    {
        public static BeamWeaponUiInfo DefaultTurret = new BeamWeaponUiInfo(
        true,
        true,
        true,
        true,
        true,
        true,
        true,
        true,
        true,
        true,
        true,
        true,
        true,
        true,
        true,
        true,
        true,
        true,
        true,
        true,
        true,
        true,
        true,
        true,
        true,
        true,
        true,
        true,
        true,
        true,
        true,
        true,
        true,
        true,
        true
        );
    }

    public static class DT_Custom_Ui_Settings
    {
        public class ActiveControls
        {
            public static BeamWeaponCustomUiInfo.BeamWeaponControls DT_ActiveControls = new BeamWeaponCustomUiInfo.BeamWeaponControls(
            false,
            false,
            false,
            false,
            false
            );
        }

        public class Toggle
        {
            public static BeamWeaponCustomUiInfo.BeamWeaponToggle DT_PowerOverride = new BeamWeaponCustomUiInfo.BeamWeaponToggle(
            "OverridePower",
            "Power Override",
            false
            );
        }

        public class Slider
        {
            public static BeamWeaponCustomUiInfo.BeamWeaponSlider DT_RangeSlider = new BeamWeaponCustomUiInfo.BeamWeaponSlider(
            "RangeSlider",
            "Range",
            0,
            100,
            75
            );

            public static BeamWeaponCustomUiInfo.BeamWeaponSlider DT_CurrentSlider = new BeamWeaponCustomUiInfo.BeamWeaponSlider(
            "CurrentSlider",
            "Damage",
            0,
            100,
            75
            );

            public static BeamWeaponCustomUiInfo.BeamWeaponSlider DT_ModulationSlider = new BeamWeaponCustomUiInfo.BeamWeaponSlider(
            "ModulationSlider",
            "Shield Damage",
            0,
            200,
            0
            );

            public static BeamWeaponCustomUiInfo.BeamWeaponSlider DT_PowerSlider = new BeamWeaponCustomUiInfo.BeamWeaponSlider(
            "PowerSlider",
            "Power",
            0,
            100,
            75
            );
        }
    }

    public static class DT_Barrel_Settings
    {
        public static class DT_Barrel1
        {
            public static BeamWeaponBarrelSubpartInfo Barrel1 = new BeamWeaponBarrelSubpartInfo(
           2.0,
           0d, 0d, 0d,
           -10f,
           0f,
           10f,
           0f
           );
        }
        public static class DT_Barrel2
        {
            public static BeamWeaponBarrelSubpartInfo Barrel2 = new BeamWeaponBarrelSubpartInfo(
           2.0,
           0d, 0d, 0d,
           -10f,
           0f,
           10f,
           0f
           );
        }
        public static class DT_Barrel3
        {
            public static BeamWeaponBarrelSubpartInfo Barrel3 = new BeamWeaponBarrelSubpartInfo(
           2.0,
           0d, 0d, 0d,
           -10f,
           0f,
           10f,
           0f
           );
        }
        public static class DT_Barrel4
        {
            public static BeamWeaponBarrelSubpartInfo Barrel4 = new BeamWeaponBarrelSubpartInfo(
           2.0,
           0d, 0d, 0d,
           -10f,
           0f,
           10f,
           0f
           );
        }
        public static class DT_Barrel5
        {
            public static BeamWeaponBarrelSubpartInfo Barrel5 = new BeamWeaponBarrelSubpartInfo(
           2.0,
           0d, 0d, 0d,
           -10f,
           0f,
           10f,
           0f
           );
        }
        public static class DT_Barrel6
        {
            public static BeamWeaponBarrelSubpartInfo Barrel6 = new BeamWeaponBarrelSubpartInfo(
           2.0,
           0d, 0d, 0d,
           -10f,
           0f,
           10f,
           0f
           );
        }
        public static class DT_Barrel7
        {
            public static BeamWeaponBarrelSubpartInfo Barrel7 = new BeamWeaponBarrelSubpartInfo(
           2.0,
           0d, 0d, 0d,
           -10f,
           0f,
           10f,
           0f
           );
        }
        public static class DT_Barrel8
        {
            public static BeamWeaponBarrelSubpartInfo Barrel8 = new BeamWeaponBarrelSubpartInfo(
           2.0,
           0d, 0d, 0d,
           -10f,
           0f,
           10f,
           0f
           );
        }
        public static class DT_Barrel9
        {
            public static BeamWeaponBarrelSubpartInfo Barrel9 = new BeamWeaponBarrelSubpartInfo(
           2.0,
           0d, 0d, 0d,
           -10f,
           0f,
           10f,
           0f
           );
        }
        public static class DT_Barrel10
        {
            public static BeamWeaponBarrelSubpartInfo Barrel10 = new BeamWeaponBarrelSubpartInfo(
           2.0,
           0d, 0d, 0d,
           -10f,
           0f,
           10f,
           0f
           );
        }
        public static class DT_Barrel11
        {
            public static BeamWeaponBarrelSubpartInfo Barrel11 = new BeamWeaponBarrelSubpartInfo(
           2.0,
           0d, 0d, 0d,
           -10f,
           0f,
           10f,
           0f
           );
        }
