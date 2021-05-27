﻿using VRageMath;
using GSF.Config;

namespace GSF.Settings.Medium_Static_Pulse_Small
{
    //"Medium_Static_Pulse_Small == MSPS"
    /// <summary>
    /// Describes the basic behavior of the weapon such as :
    /// Ammo_Name, Class, Range, Power_Useage, Damage, Shield_Damage, Explosion_Radius and Barrel_Count
    /// </summary>
    public static class MSPS_Default_Settings
    {

        public static BeamWeaponDefaultInfo Medium_Static_Pulse_Small = new BeamWeaponDefaultInfo(
            "Class1LaserBeamCharge",	//ammoName
            false,	//Super Laser 
            1, 		//classes
            1200f,	//range 
            1500f,	//powerUsage 
            200f, 	//damage
            50f,	//Shield Damage 
            .05f,    //Explosion radius
            1       //barrel Count
            );

    }

    /// <summary>
    /// Describes the Color and also the beam width for both beams.
    /// Explosive_Beam, Explosive_Beam_Aux, Main, Aux, Color_Wide, Color_Wide_Aux
    /// </summary>
    public static class MSPS_Color_Settings
    {
        public static BeamWeaponColorInfo Medium_Static_Pulse_Small = new BeamWeaponColorInfo(
            Color.OrangeRed,            //Explosive Beam
            Color.Orange,               //Explosive Beam aux
            Color.BlueViolet,		    //Main color 
            Color.CadetBlue, 			//Aux color
            0.005f, 	                //Color wide try .005
            0.025f                      //Aux color wide try .0025
        );
    }

    /// <summary>
    /// Describes the fireing sequence of the turret.
    /// Max_Residual_Heat, Max_Heat, Residual_HPT, Heat_Per_Tick--"HPT", Residual_HDPT, HDPT, Res_HID, Res_HDD, HDD, Keep_At_Charge
    /// </summary>
    public static class MSPS_Heat_Settings
    {
        public static BeamWeaponExtendedHeatInfo Medium_Static_Pulse_Small = new BeamWeaponExtendedHeatInfo(
        1000f, //Max Residual Heat  " The Total ammount of heat a turret will accumulate before it begins the cooldown process "
        100f, 	//maxHeat   " The Total ammount of heat a turret can accumulate per Volley "

        10f,    //Residual Heat Per Tick " The ammount of residual heat a turret will accumulate per tick "
        10f, 	//heatPerTick " The ammount of volley heat a turret will accumulate per tick "

        2f,     //Residual Heat Dis Per TIck " The ammount of residual heat that will disipate per tick "
        5f, 	//heat Dis Per Tick  " The ammount of volley heat that will disipate per tick "

        10,      //res heat inc delay "This is the top end delay a turret will start accumulationg heat after this number is reached"
        250,    //res heat dis delay  // the lower the number the longer it takes to over heat you can hit a threshold where you wont accumulate any residual heat

        5,	        //heatDissipationDelay " the ammount of time it will take before the turret will start disipateing volley heat "
        10);        //keepAtCharge "Keep this at ten it gets wonkey when you mess with it this tells the power to keep at charge when it has this ammount of inventory "
    }

    /// <summary>
    /// Describes what you want disabled or enabled from the default UI for the Missile turret.
    /// 
    /// </summary>
    public static class MSPS_Default_Ui_Settings
    {

        public static BeamWeaponUiInfo Medium_Static_Pulse_Small = new BeamWeaponUiInfo(
            true,  //On Off Value this will grey out the control   control.Enabled 
            true,  //Control Panel                                 control.Visible
            true,  //Toolbar Config                                action.Enabled

            false,  //Shoot Value           control.Enabled
            false,  //Control Panel         control.Visible
            false,  //Toolbar Config        action.Enabled

            false,  //Shoot Once Value          
            false,  //Control Panel
            false,  //Toolbar Config

            false,  //Enable Idle Movement Value
            false,  //Control Panel
            false,  //Toolbar Config

            false,  //Target Missiles 
            false,  //Control Panel
            false,  //Toolbar Config 

            false,  //Target Meteors Value
            false,  //Control Panel
            false,  //Toolbar Config 

            false,  //Target Small Ships Value
            false,  //Control Panel
            false,  //Toolbar Config 

            false,  //Target Large Ships Value
            false,  //Control Panel
            false,  //Toolbar Config 

            false,  //Target Stations Value
            false,  //Control Panel
            false,  //Toolbar Config 

            true,  //Target Characters Value
            true,  //Control Panel
            true,  //Toolbar Config 

            true,  //Target Neutrals Value
            true,  //Control Panel
            true,  //Toolbar Config 

            false,  //Control Panel
            false  //Toolbar Config 
            );

    }

    /// <summary>
    /// Describes the custom Ui for all the laser turrets.
    /// </summary>
    public static class MSPS_Custom_Ui_Settings
    {
        public class Toggle
        {
            public static BeamWeaponCustomUiInfo.BeamWeaponToggle MSPS_PowerOveride = new BeamWeaponCustomUiInfo.BeamWeaponToggle(
                "OveridePower",  //internalName
                "Power Overide", //title
                false            //default to on and off
                );
        }

        public class Slider
        {
            public static BeamWeaponCustomUiInfo.BeamWeaponSlider MSPS_RangeSlider = new BeamWeaponCustomUiInfo.BeamWeaponSlider(
                "RangeSlider",  //internalName
                "Range", //title
                0,
                100,
                75
                );

            public static BeamWeaponCustomUiInfo.BeamWeaponSlider MSPS_CurrentSlider = new BeamWeaponCustomUiInfo.BeamWeaponSlider(
                "CurrentSlider",  //internalName
                "Damage", //title
                0,
                100,
                75
                );

            public static BeamWeaponCustomUiInfo.BeamWeaponSlider MSPS_ModulationSlider = new BeamWeaponCustomUiInfo.BeamWeaponSlider(
                "ModulationSlider",  //internalName
                "Shield Damage", //title
                0,
                200,
                0
                );

            public static BeamWeaponCustomUiInfo.BeamWeaponSlider MSPS_PowerSlider = new BeamWeaponCustomUiInfo.BeamWeaponSlider(
                "PowerSlider",  //internalName
                "Power", //title
                0,
                100,
                75
                );
        }

    }

    /// <summary>
    /// Describes each barrel individually via (Vector format)
    /// </summary>
    public static class MSPS_Barrel_Settings
    {
        public static class Medium_Static_Pulse_Small
        {
            public static BeamWeaponBarrelSubpartInfo B1 = new BeamWeaponBarrelSubpartInfo(
               1.22d,  //Forward Origin
               0.0d,  //Backward Origin
               0.0d,  //Left Origin
               0.0d,  //Right Origin

               0f,   //Left Offset
               0f,   //Right Offset
               0f,   //Up Offset
               .05f    //Down Offset
               );

        }
        public static class MSPS_Targeting_Beam
        {
            public static BeamWeaponBarrelSubpartInfo T_Beam = new BeamWeaponBarrelSubpartInfo(
            2.0d,  //Forward Origin
            0.0d,  //Backward Origin
            0.0d,  //Left Origin
            0.0d,  //Right Origin

            0f,   //Left Offset
            0f,   //Right Offset
            .09f,   //Up Offset
            0f    //Down Offset
            );
        }
    }
}