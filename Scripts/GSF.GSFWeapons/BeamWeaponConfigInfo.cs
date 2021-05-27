using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VRageMath;

namespace GSF.Config
{
    public class BeamWeaponDefaultInfo
    {
        public float range;
        public float powerUsage;
        public float damage;
        public float SHIELD_DAMAGE;
        public string ammoName;
        public int classes;
        public bool issuper;
        public float expradius;
        public int barrels;

        public BeamWeaponDefaultInfo(string ammoName, bool issuper, int classes, float range, float powerUsage, float damage, float SHIELD_DAMAGE, float expradius, int barrels)
        {

            this.range = range;
            this.powerUsage = powerUsage;
            this.damage = damage;
            this.SHIELD_DAMAGE = SHIELD_DAMAGE;
            this.ammoName = ammoName;
            this.classes = classes;
            this.issuper = issuper;
            this.expradius = expradius;
            this.barrels = barrels;

        }
    }

    public class BeamWeaponBarrelInfo
    {
        public int Barrels;
        public float BarrelLRoffset;
        public float BarrelUDoffset;
        public double forwardOrigin;

        public BeamWeaponBarrelInfo(int Barrels, float BarrelLRoffset, float BarrelUDoffset, double forwardOrigin)
        {
            this.Barrels = Barrels;
            this.BarrelLRoffset = BarrelLRoffset;
            this.BarrelUDoffset = BarrelUDoffset;
            this.forwardOrigin = forwardOrigin;
        }
    }

    public class BeamWeaponHeatInfo
    {

        public float maxHeat;
        public float heatPerTick;
        public float heatDissipationPerTick;
        public int heatDissipationDelay;
        public int keepAtCharge;


        public BeamWeaponHeatInfo(float maxHeat, float heatPerTick, float heatDissipationPerTick, int heatDissipationDelay, int keepAtCharge)
        {
            this.maxHeat = maxHeat;
            this.heatPerTick = heatPerTick;
            this.heatDissipationPerTick = heatDissipationPerTick;
            this.heatDissipationDelay = heatDissipationDelay;
            this.keepAtCharge = keepAtCharge;
        }
    }

    public class BeamWeaponColorInfo
    {

        public Color explosiveBeam;
        public Color explosiveBeamAux;
        public Color color;
        public Color auxcolor;
        public float colorwide;
        public float auxcolorwide;

        public BeamWeaponColorInfo(Color explosiveBeam, Color explosiveBeamAux, Color color, Color auxcolor, float colorwide, float auxcolorwide)
        {

            this.explosiveBeam = explosiveBeam;
            this.explosiveBeamAux = explosiveBeamAux;
            this.color = color;
            this.auxcolor = auxcolor;
            this.colorwide = colorwide;
            this.auxcolorwide = auxcolorwide;

        }
    }

    public class BeamWeaponColor
    {


        public int r1;
        public int g1;
        public int b1;

        public int r2;
        public int g2;
        public int b2;

        public int r3;
        public int g3;
        public int b3;

        public int r4;
        public int g4;
        public int b4;

        public float colorwide;
        public float auxcolorwide;

        public BeamWeaponColor(int r1, int g1, int b1, int r2, int g2, int b2, int r3, int g3, int b3, int r4, int g4, int b4, float colorwide, float auxcolorwide)
        {


            this.r1 = r1;
            this.g1 = g1;
            this.b1 = b1;

            this.r2 = r2;
            this.g2 = g2;
            this.b2 = b2;

            this.r2 = r3;
            this.g2 = g3;
            this.b2 = b3;

            this.r2 = r4;
            this.g2 = g4;
            this.b2 = b4;

            this.colorwide = colorwide;
            this.auxcolorwide = auxcolorwide;

        }
    }

    public class BeamWeaponUiInfo
    {
        public bool OnOffValue;
        public bool OnOffControl;
        public bool OnOffConfig;

        public bool ShootValue;
        public bool ShootControl;
        public bool ShootConfig;

        public bool ShootOnceValue;
        public bool ShootOnceControl;
        public bool ShootOnceConfig;

        public bool EnableIdleMovementValue;
        public bool EnableIdleMovementControl;
        public bool EnableIdleMovementConfig;

        public bool TargetMissilesValue;
        public bool TargetMissilesControl;
        public bool TargetMissilesConfig;

        public bool TargetMeteorsValue;
        public bool TargetMeteorsControl;
        public bool TargetMeteorsConfig;

        public bool TargetSmallShipsValue;
        public bool TargetSmallShipsControl;
        public bool TargetSmallShipsConfig;

        public bool TargetLargeShipsValue;
        public bool TargetLargeShipsControl;
        public bool TargetLargeShipsConfig;

        public bool TargetStationsValue;
        public bool TargetStationsControl;
        public bool TargetStationsConfig;

        public bool TargetCharactersValue;
        public bool TargetCharactersControl;
        public bool TargetCharactersConfig;

        public bool TargetNeutralsValue;
        public bool TargetNeutralsControl;
        public bool TargetNeutralsConfig;

        public bool CanControlControl;
        public bool CanControlConfig;

        public BeamWeaponUiInfo
            (
            bool OnOffValue, bool OnOffControl, bool OnOffConfig,
            bool ShootValue, bool ShootControl, bool ShootConfig,
            bool ShootOnceValue, bool ShootOnceControl, bool ShootOnceConfig,
            bool EnableIdleMovementValue, bool EnableIdleMovementControl, bool EnableIdleMovementConfig,
            bool TargetMissilesValue, bool TargetMissilesControl, bool TargetMissilesConfig,

            bool TargetMeteorsValue, bool TargetMeteorsControl, bool TargetMeteorsConfig,
            bool TargetSmallShipsValue, bool TargetSmallShipsControl, bool TargetSmallShipsConfig,
            bool TargetLargeShipsValue, bool TargetLargeShipsControl, bool TargetLargeShipsConfig,
            bool TargetStationsValue, bool TargetStationsControl, bool TargetStationsConfig,
            bool TargetCharactersValue, bool TargetCharactersControl, bool TargetCharactersConfig,
            bool TargetNeutralsValue, bool TargetNeutralsControl, bool TargetNeutralsConfig,
            bool CanControlControl, bool CanControlConfig
            )
        {

            this.OnOffValue = OnOffValue;
            this.OnOffControl = OnOffControl;
            this.OnOffConfig = OnOffConfig;

            this.ShootValue = ShootValue;
            this.ShootControl = ShootControl;
            this.ShootConfig = ShootConfig;

            this.ShootOnceValue = ShootOnceValue;
            this.ShootOnceControl = ShootOnceControl;
            this.ShootOnceConfig = ShootOnceConfig;

            this.EnableIdleMovementValue = EnableIdleMovementValue;
            this.EnableIdleMovementControl = EnableIdleMovementControl;
            this.EnableIdleMovementConfig = EnableIdleMovementConfig;

            this.TargetMissilesValue = TargetMissilesValue;
            this.TargetMissilesControl = TargetMissilesControl;
            this.TargetMissilesConfig = TargetMissilesConfig;

            this.TargetMeteorsValue = TargetMeteorsValue;
            this.TargetMeteorsControl = TargetMeteorsControl;
            this.TargetMeteorsConfig = TargetMeteorsConfig;

            this.TargetSmallShipsValue = TargetSmallShipsValue;
            this.TargetSmallShipsControl = TargetSmallShipsControl;
            this.TargetSmallShipsConfig = TargetSmallShipsConfig;

            this.TargetLargeShipsValue = TargetLargeShipsValue;
            this.TargetLargeShipsControl = TargetLargeShipsControl;
            this.TargetLargeShipsConfig = TargetLargeShipsConfig;

            this.TargetStationsValue = TargetStationsValue;
            this.TargetStationsControl = TargetStationsControl;
            this.TargetStationsConfig = TargetStationsConfig;

            this.TargetCharactersValue = TargetCharactersValue;
            this.TargetCharactersControl = TargetCharactersControl;
            this.TargetCharactersConfig = TargetCharactersConfig;

            this.TargetNeutralsValue = TargetNeutralsValue;
            this.TargetNeutralsControl = TargetNeutralsControl;
            this.TargetNeutralsConfig = TargetNeutralsConfig;

            this.CanControlControl = CanControlControl;
            this.CanControlConfig = CanControlConfig;
        }
    }

    public class BeamWeaponCustomUiInfo
    {
        public class BeamWeaponControls
        {
            public bool PowerOveride;
            public bool Range;
            public bool Power;
            public bool Current;
            public bool Modulation;

            public BeamWeaponControls(bool PowerOveride, bool Range, bool Power, bool Current, bool Modulation)
            {
                this.PowerOveride = PowerOveride;
                this.Range = Range;
                this.Power = Power;
                this.Current = Current;
                this.Modulation = Modulation;
            }
        }

        public class BeamWeaponToggle
        {
            public string internalName;
            public string title;
            public bool defaultValue;

            public BeamWeaponToggle(string internalName, string title, bool defaultValue)
            {
                this.internalName = internalName;
                this.title = title;
                this.defaultValue = defaultValue;
            }
        }

        public class BeamWeaponSlider
        {
            public string internalName;
            public string title;
            public float min;
            public float max;
            public float standard;

            public BeamWeaponSlider(string internalName, string title, float min, float max, float standard)
            {
                this.internalName = internalName;
                this.title = title;
                this.min = min;
                this.max = max;
                this.standard = standard;
            }
        }

    }

    public class BeamWeaponSubpartInfo
    {
        public float barrelTravelDist;
        public float barrelPunchSpeed;
        public float barrelRestoreSpeed;

        public BeamWeaponSubpartInfo(float barrelTravelDist, float barrelPunchSpeed, float barrelRestoreSpeed)
        {

            this.barrelTravelDist = barrelTravelDist;
            this.barrelPunchSpeed = barrelPunchSpeed;
            this.barrelRestoreSpeed = barrelRestoreSpeed;


        }
    }

    public class BeamWeaponBarrelSubpartInfo
    {

        public double forwardOrigin;
        public double backwardOrigin;
        public double leftOrigin;
        public double rightOrigin;

        public float leftOffset;
        public float rightOffset;
        public float upOffset;
        public float downOffset;

        public BeamWeaponBarrelSubpartInfo(

            double forwardOrigin,
            double backwardOrigin,
            double leftOrigin,
            double rightOrigin,
            float leftOffset,
            float rightOffset,
            float upOffset,
            float downOffset
            )
        {

            this.forwardOrigin = forwardOrigin;
            this.leftOffset = leftOffset;
            this.rightOffset = rightOffset;
            this.upOffset = upOffset;
            this.downOffset = downOffset;



        }

    }

    public class BeamWeaponExtendedHeatInfo
    {
        public float ResidualMaxHeat; // the overall heat that a turret can reach before shutdown
        public float MaxHeat; // the max heat that a turret will reach on a per volley basis

        public float HeatPerTick;
        public float ResidualHeatPerTick; //each time the script runs every tick ++

        public float HeatDissipationPerTick;
        public float ResidualDissipationPerTick;

        public int ResidualHeatIncDelay;
        public int ResidualHeatDisDelay;

        public int HeatDisDelay;
        public int KeepAtCharge;

        public BeamWeaponExtendedHeatInfo(
            float ResidualMaxHeat,
            float MaxHeat,

            float ResidualHeatPerTick,
            float HeatPerTick,

            float ResidualDissipationPerTick,
            float HeatDissipationPerTick,


            int ResidualHeatIncDelay,
            int ResidualHeatDisDelay,

            int HeatDisDelay,
            int KeepAtCharge)
        {
            this.ResidualMaxHeat = ResidualMaxHeat;
            this.MaxHeat = MaxHeat;

            this.HeatPerTick = HeatPerTick;
            this.ResidualHeatPerTick = ResidualHeatPerTick;

            this.HeatDissipationPerTick = HeatDissipationPerTick;
            this.ResidualDissipationPerTick = ResidualDissipationPerTick;

            this.ResidualHeatIncDelay = ResidualHeatIncDelay;
            this.ResidualHeatDisDelay = ResidualHeatDisDelay;

            this.HeatDisDelay = HeatDisDelay;
            this.KeepAtCharge = KeepAtCharge;
        }
    }

}
