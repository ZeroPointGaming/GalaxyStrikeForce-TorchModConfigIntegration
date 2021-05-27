using VRageMath;
using GSF.Config;

namespace GSF.Settings.Small_Beam
{
    //"Small_Beam == SB"
    /// <summary>
    /// Describes the basic behavior of the weapon such as :
    /// Ammo_Name, Class, Range, Power_Useage, Damage, Shield_Damage, Explosion_Radius and Barrel_Count
    /// </summary>
    public static class SB_Default_Settings
    {

        public static BeamWeaponDefaultInfo Small_Beam = new BeamWeaponDefaultInfo(
            "Class1LaserBeamCharge",	//ammoName
            false,	//Super Laser 
            1, 		//classes
            800f,	//range 
            154.94f,	//powerUsage 
            164.22f, 	//damage
            0f,	//Shield Damage 
            .005f,    //Explosion radius
            1
            );

    }

    /// <summary>
    /// Describes the Color and also the beam width for both beams.
    /// Explosive_Beam, Explosive_Beam_Aux, Main, Aux, Color_Wide, Color_Wide_Aux
    /// </summary>
    public static class SB_Color_Settings
    {
        public static BeamWeaponColorInfo Small_Beam = new BeamWeaponColorInfo(
            Color.OrangeRed,            //Explosive Beam
            Color.Orange,               //Explosive Beam aux
            Color.DarkGoldenrod,		//Main color 
            Color.Gold, 				//Aux color
            .005f, 	                    //Color wide try .005
            .025f                      //Aux color wide try .0025
        );
    }

    /// <summary>
    /// Describes the fireing sequence of the turret.
    /// Max_Residual_Heat, Max_Heat, Residual_HPT, Heat_Per_Tick--"HPT", Residual_HDPT, HDPT, Res_HID, Res_HDD, HDD, Keep_At_Charge
    /// </summary>
    public static class SB_Heat_Settings
    {
        public static BeamWeaponExtendedHeatInfo Small_Beam = new BeamWeaponExtendedHeatInfo(
        2000f, //Max Residual Heat  " The Total ammount of heat a turret will accumulate before it begins the cooldown process "
        1000f, 	//maxHeat   " The Total ammount of heat a turret can accumulate per Volley "

        10f,    //Residual Heat Per Tick " The ammount of residual heat a turret will accumulate per tick "
        5f, 	//heatPerTick " The ammount of volley heat a turret will accumulate per tick "

        5f,     //Residual Heat Dis Per TIck " The ammount of residual heat that will disipate per tick "
        5f, 	//heat Dis Per Tick  " The ammount of volley heat that will disipate per tick "

        10,      //res heat inc delay "This is the top end delay a turret will start accumulationg heat after this number is reached"
        300,    //res heat dis delay  // the lower the number the longer it takes to over heat you can hit a threshold where you wont accumulate any residual heat

        5,	        //heatDissipationDelay " the ammount of time it will take before the turret will start disipateing volley heat "
        10); 	    //keepAtCharge "Keep this at ten it gets wonkey when you mess with it this tells the power to keep at charge when it has this ammount of inventory "
    }

    /// <summary>
    /// Describes what you want disabled or enabled from the default UI for the Missile turret.
    /// </summary>
    public static class SB_Default_Ui_Settings
    {
        public static BeamWeaponUiInfo Small_Beam = new BeamWeaponUiInfo(
            true,  //On Off Value this will grey out the control   control.Enabled 
            true,  //Control Panel                                 control.Visible
            true,  //Toolbar Config                                action.Enabled

            true,  //Shoot Value           control.Enabled
            true,  //Control Panel         control.Visible
            true,  //Toolbar Config        action.Enabled

            true,  //Shoot Once Value          
            true,  //Control Panel
            true,  //Toolbar Config

            true,  //Enable Idle Movement Value
            true,  //Control Panel
            true,  //Toolbar Config

            true,  //Target Missiles 
            true,  //Control Panel
            true,  //Toolbar Config 

            true,  //Target Meteors Value
            true,  //Control Panel
            true,  //Toolbar Config 

            true,  //Target Small Ships Value
            true,  //Control Panel
            true,  //Toolbar Config 

            true,  //Target Large Ships Value
            true,  //Control Panel
            true,  //Toolbar Config 

            true,  //Target Stations Value
            true,  //Control Panel
            true,  //Toolbar Config 

            true,  //Target Characters Value
            true,  //Control Panel
            true,  //Toolbar Config 

            true,  //Target Neutrals Value
            true,  //Control Panel
            true,  //Toolbar Config 

            true,  //Control Panel
            true  //Toolbar Config 
            );


    }

    /// <summary>
    /// Describes the custom Ui for all the laser turrets.
    /// </summary>
    public static class SB_Custom_Ui_Settings
    {
        public class Toggle
        {
            public static BeamWeaponCustomUiInfo.BeamWeaponToggle SB_PowerOveride = new BeamWeaponCustomUiInfo.BeamWeaponToggle(
                "OveridePower",  //internalName
                "Power Overide", //title
                false            //default to on and off
                );
        }

        public class Slider
        {
            public static BeamWeaponCustomUiInfo.BeamWeaponSlider SB_RangeSlider = new BeamWeaponCustomUiInfo.BeamWeaponSlider(
                "RangeSlider",  //internalName
                "Range", //title
                0,
                100,
                75
                );

            public static BeamWeaponCustomUiInfo.BeamWeaponSlider SB_CurrentSlider = new BeamWeaponCustomUiInfo.BeamWeaponSlider(
                "CurrentSlider",  //internalName
                "Damage", //title
                0,
                100,
                75
                );

            public static BeamWeaponCustomUiInfo.BeamWeaponSlider SB_ModulationSlider = new BeamWeaponCustomUiInfo.BeamWeaponSlider(
                "ModulationSlider",  //internalName
                "Shield Damage", //title
                0,
                200,
                0
                );

            public static BeamWeaponCustomUiInfo.BeamWeaponSlider SB_PowerSlider = new BeamWeaponCustomUiInfo.BeamWeaponSlider(
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
    public static class SB_Barrel_Settings
    {
        public static class Small_Beam
        {
            public static BeamWeaponBarrelSubpartInfo B1 = new BeamWeaponBarrelSubpartInfo(
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

        public static class SB_Targeting_Beam
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

