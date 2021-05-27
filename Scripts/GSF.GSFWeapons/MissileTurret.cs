using VRage.Game.Components;
using Sandbox.Common.ObjectBuilders;
using VRage.Game;
using VRage.Game.ObjectBuilders.Definitions;
using VRage.ObjectBuilders;
using VRage.Game.ModAPI;
using Sandbox.Game.EntityComponents;
using System.Collections.Generic;
using VRage.ModAPI;
using System.Text;
using VRage;
using Sandbox.ModAPI;
using Sandbox.Game.Entities;
using VRageMath;
using VRage.Game.Entity;
using VRage.Utils;
using VRage.Game.ModAPI.Interfaces;
using System;
using System.Linq;
using SpaceEngineers.Game.ModAPI;
using Sandbox.Game;
using ProtoBuf;
using Sandbox.ModAPI.Weapons;
using Sandbox.ModAPI.Interfaces.Terminal;
using System.Linq.Expressions;
using IMyCockpit = Sandbox.ModAPI.Ingame.IMyCockpit;
using TExtensions = Sandbox.ModAPI.Interfaces.TerminalPropertyExtensions;
using GSF.Config;
//using GSF.Settings;
using GSFWeapon.Control;

using GSF.Settings.Interior_Pulse;

using GSF.Settings.S_Small_Beam_Static;
using GSF.Settings.Small_Beam;
using GSF.Settings.Small_Pulse;

using GSF.Settings.Medium_Quad_Beam;
using GSF.Settings.Medium_Pulse;
using GSF.Settings.Medium_Pulse_Small;
using GSF.Settings.Medium_Static_Pulse_Large;
using GSF.Settings.Medium_Static_Pulse_Small;

using GSF.Settings.Aegis_Large_Beam_Base;
using GSF.Settings.Aegis_Medium_Beam_Base;
using GSF.Settings.Aegis_Small_Beam_Base;

using GSF.Settings.XLarge_Dual_Pulse;
using GSF.Settings.XLarge_Giga_Beam;
using GSF.Settings.XXLarge_Planitary_GigaBeam;

using GSF.Settings.Large_Dual_Beam;
using GSF.Settings.Large_Dual_Pulse;
using GSF.Settings.Large_Static_Large_Beam;
using GSF.Settings.Large_Static_Small_Beam;



namespace GSF.GSFWeapons
{
    class Globals
    {
        public static readonly bool Debug = false;
        public static readonly bool LockDebug = false;
    }

    [MyEntityComponentDescriptor(typeof(MyObjectBuilder_LargeMissileTurret), true, "SmallBeamBaseGTF_Large", "Interior_Pulse_Laser_Base_Large", "SmallPulseLaser_Base_Large", "MediumQuadBeamGTFBase_Large", "MPulseLaserBase_Large", "LDualPulseLaserBase_Large", "XLDualPulseLaserBase_Large", "LargeDualBeamGTFBase_Large", "XLGigaBeamGTFBase_Large", "MPulseLaserBase_Small", "StationBeamBase_Large", "AegisLargeBeamBase_Large", "AegisMediumeamBase_Large", "AegisSmallBeamBase_Large")]

    public class MissileTurret : MyGameLogicComponent
    {
        /// <summary>
        /// holds all of the pre defined variables needed to make the script function properly
        /// </summary>
        #region  Weapon Variables

        MyDefinitionId electricityDefinition = new MyDefinitionId(typeof(MyObjectBuilder_GasProperties), "Electricity");
        MyResourceSinkComponent resourceSink;

        MyObjectBuilder_EntityBase objectBuilder = null;
        MyEntity3DSoundEmitter e;

        List<MyObjectBuilder_AmmoMagazine> chargeObjectBuilders;
        List<SerializableDefinitionId> chargeDefinitionIds;
        List<Beam> beamList = new List<Beam>();

        IMyCubeBlock cubeBlock = null;
        IMyInventory m_inventory;
        IMyFunctionalBlock functionalBlock = null;
        IMyTerminalBlock terminalBlock;
        IMyCubeBlock m_turret;
        IMyEntity _ownerEntity;

        #region Beam Weapon Info
        
        BeamWeaponColorInfo beamWeaponColorInfo;
        BeamWeaponSubpartInfo beamWeaponSubpartInfo;
        BeamWeaponDefaultInfo beamWeaponDefaultInfo;
        BeamWeaponUiInfo beamWeaponUiInfo;
        BeamWeaponExtendedHeatInfo beamWeaponExtendedHeatInfo;
        BeamWeaponBarrelSubpartInfo beamWeaponBarrel;
        BeamWeaponBarrelSubpartInfo beamWeaponTargetingLaser;

        List<BeamWeaponBarrelSubpartInfo> BarrelList = new List<BeamWeaponBarrelSubpartInfo>();

        BeamWeaponCustomUiInfo.BeamWeaponToggle Toggle;
        BeamWeaponCustomUiInfo.BeamWeaponSlider powerSlider;
        BeamWeaponCustomUiInfo.BeamWeaponSlider rangeSlider;
        BeamWeaponCustomUiInfo.BeamWeaponSlider currentSlider;
        BeamWeaponCustomUiInfo.BeamWeaponSlider modulationSlider;

        #endregion

        #region Controls
        Slider<Sandbox.ModAPI.Ingame.IMyLargeTurretBase> _RangeSlider;
        Slider<Sandbox.ModAPI.Ingame.IMyLargeTurretBase> _PowerSlider;
        Slider<Sandbox.ModAPI.Ingame.IMyLargeTurretBase> _CurrentSlider;
        Slider<Sandbox.ModAPI.Ingame.IMyLargeTurretBase> _ModulationSlider;
        Checkbox<Sandbox.ModAPI.Ingame.IMyLargeTurretBase> _PowerOveride;
        ColorSlider<Sandbox.ModAPI.Ingame.IMyLargeTurretBase> _PrimaryBeamColor;
        ColorSlider<Sandbox.ModAPI.Ingame.IMyLargeTurretBase> _SecondaryBeamColor;
        #endregion

        public bool Initialized = true;
        private bool _anim_init = false;
        private float anim_step = 0f;

        #region Turret Variables
        string subtypeName;
        float powerConsumption;
        float setPowerConsumption;
        public float currentHeat;
        public float currentHeatResidual;
        bool overheatedResidual = false;
        bool overheated = false;
        bool hitBool = false;
        long lastShootTime;
        int lastShootTimeTicks;
        int ticks = 0;
        int shotCount = 1;
        //int residualCounter = 0;
        int damageUpgrades = 0;
        #endregion

        #region Targeting
        bool hitPoint;
        bool hitCenter = false;
        #endregion

        #region Particle Effects

        int barrelEff = 17;
        int barrelEff2 = 22;
        int toTargetEff = 51;

        private MyParticleEffect m_particleEffectBL1;
        private MyParticleEffect m_particleEffectBR1;
        private MyParticleEffect m_particleEffectBL;
        private MyParticleEffect m_particleEffectBR;
        private MyParticleEffect m_particleEffectToTarget;

        private void StartTargetEffects()
        {
            if (MyAPIGateway.Session.Player == null)
                return;

            if (!(functionalBlock as IMyFunctionalBlock).IsWorking)
                return;


            var toTargetMatrix = MatrixD.CreateTranslation(toTarget);



            if (m_particleEffectToTarget != null)
            {
                m_particleEffectToTarget.WorldMatrix = toTargetMatrix;
                return;
            }


            MyParticlesManager.TryCreateParticleEffect((int)toTargetEff, out m_particleEffectToTarget);



            if (m_particleEffectToTarget != null)
            {
                m_particleEffectToTarget.UserScale = m_turret == null ? 1f : 1f;
                m_particleEffectToTarget.WorldMatrix = toTargetMatrix;
                var dur = m_particleEffectToTarget.Duration;
                m_particleEffectToTarget.DurationMax = dur / 2f;
                m_particleEffectToTarget.Play();

            }

        }

        protected void PauseTargetEffects()
        {
            if (MyAPIGateway.Session.Player == null)
                return;

            if (m_particleEffectToTarget != null)
            {
                m_particleEffectToTarget.Pause();

            }
        }

        protected void StopTargetEffects()
        {
            if (MyAPIGateway.Session.Player == null)
                return;

            if (m_particleEffectToTarget != null)
            {
                m_particleEffectToTarget.Stop();
                m_particleEffectToTarget = null;
            }
        }


        #endregion

        #region Weapon Control Variables and functions

        public float m_requiredInput()
        {
            float requiredInput = (getRange() + getCurrent() + getModulation());
            return requiredInput;
        }
        public float m_maximumInput()
        {
            float maximumInput = ((MaxPower() / 100) * (beamWeaponDefaultInfo.powerUsage));

            if (getPowerOveride())
            {
                maximumInput = (MaxPower() / 100) * (beamWeaponDefaultInfo.powerUsage * 2);
            }

            if (getPowerOveride() && beamWeaponDefaultInfo.issuper)
            {
                maximumInput = (MaxPower() / 100) * (beamWeaponDefaultInfo.powerUsage * 4);
            }

            return maximumInput;
        }
        public float getRange()
        {
            float range = (MaxRange() / 100) * (beamWeaponDefaultInfo.range);

            if (getPowerOveride())
            {
                range = ((MaxRange() / 100) * (2 * beamWeaponDefaultInfo.range));
            }

            if (getPowerOveride() && beamWeaponDefaultInfo.issuper)
            {
                range = ((MaxRange() / 100) * (2 * beamWeaponDefaultInfo.range));
            }
            return range;
        }
        public float getCurrent()
        {
            float current = (MaxCurrent() / 100) * (beamWeaponDefaultInfo.damage);

            if (getPowerOveride())
            {
                current = ((MaxCurrent() / 100) * (2 * beamWeaponDefaultInfo.damage));
            }

            if (getPowerOveride() && beamWeaponDefaultInfo.issuper)
            {
                current = ((MaxCurrent() / 100) * (4 * beamWeaponDefaultInfo.damage));
            }

            return current;
        }
        public float getModulation()
        {
            float modulation = (MaxModulation() / 100) * (beamWeaponDefaultInfo.SHIELD_DAMAGE);

            if (getPowerOveride())
            {
                modulation = ((MaxModulation() / 100) * (2 * beamWeaponDefaultInfo.SHIELD_DAMAGE));
            }

            if (getPowerOveride() && beamWeaponDefaultInfo.issuper)
            {
                modulation = ((MaxModulation() / 100) * (4 * beamWeaponDefaultInfo.SHIELD_DAMAGE));
            }

            return modulation;
        }
        public float getHeat()
        {
            float heat = currentHeat;
            return heat;
        }

        public float getResidualHeat()
        {
            float Rheat = currentHeatResidual;
            return Rheat;
        }

        public static string _tName = "default";
        private string m_tName = _tName;
        public string TurretName
        {
            get { return (cubeBlock != null ? m_tName : _tName); }
            set { m_tName = cubeBlock.Name; }
        }

        public float MaxRange()
        {
            float range = 0f;
            if (!Initialized && cubeBlock.IsWorking)
            {
                range = _RangeSlider.Getter((IMyFunctionalBlock)cubeBlock);
            }
            return range;
        }

        public float MaxPower()
        {
            float power = 0f;
            if (!Initialized && cubeBlock.IsWorking)
            {
                power = _PowerSlider.Getter((IMyFunctionalBlock)cubeBlock);
            }
            return power;
        }

        public float MaxCurrent()
        {
            float current = 0f;
            if (!Initialized && cubeBlock.IsWorking)
            {
                current = _CurrentSlider.Getter((IMyFunctionalBlock)cubeBlock);
            }
            return current;
        }

        public float MaxModulation()
        {
            float modulation = 0f;
            if (!Initialized && cubeBlock.IsWorking)
            {
                modulation = _ModulationSlider.Getter((IMyFunctionalBlock)cubeBlock);
            }
            return modulation;
        }

        public Color PrimaryBeamColor()
        {
            Color color = beamWeaponColorInfo.color;
            if (!Initialized && cubeBlock.IsWorking)
            {
                color = _PrimaryBeamColor.Getter((IMyFunctionalBlock)cubeBlock);
            }
            return color;
        }

        public Color SecondaryBeamColor()
        {
            Color color = beamWeaponColorInfo.auxcolor;
            if (!Initialized && cubeBlock.IsWorking)
            {
                color = _SecondaryBeamColor.Getter((IMyFunctionalBlock)cubeBlock);
            }
            return color;
        }

        public float CalcRequiredPower()
        {

            float power = 0.0001f;
            if (!Initialized && cubeBlock.IsWorking)
            {
               // var radius = Slider.Getter((IMyFunctionalBlock)cubeBlock);
                //power = (float)(4.0 * Math.PI * Math.Pow(radius, 3) / 3.0 / 1000.0 / 1000.0);
            }
            return power;
        }

        public bool getPowerOveride()
        {
            bool check = false;
            if (!Initialized && cubeBlock.IsWorking)
            {
                check = _PowerOveride.Getter((IMyFunctionalBlock)cubeBlock);

            }
            return check;
        }
        #endregion

        #region Beam endpoint Vectors

        #region From points
        Vector3D barrelTip = new VRageMath.Vector3D();
        Vector3D from = new Vector3D();
        #endregion

        #region To Points
        Vector3D beamEndpoint = new VRageMath.Vector3D();
        Vector3D to = new Vector3D();
        #endregion

        #region ToTarget Points
        Vector3D beamTarget = new Vector3D();
        Vector3D toTarget = new Vector3D();
        #endregion


        #endregion

        #endregion

        #region Turret Degridation
        /// <summary>
        /// applys intentional damage to the turret when power overide is enabled
        /// </summary>
        public void DoTurretDegridation()
        {

            if (m_requiredInput() > beamWeaponDefaultInfo.powerUsage)
            {

                cubeBlock.SlimBlock.DoDamage(GetTurretDegFactor(), MyStringHash.GetOrCompute("Laser"), false, default(MyHitInfo), cubeBlock.EntityId);
            }
        }

        /// <summary>
        /// returns the percentage of damage in which the turrets ensue due to overchargeing the turret 
        /// </summary>
        /// <returns></returns>
        public float GetTurretDegFactor()
        {
            float factor = (m_requiredInput() - beamWeaponDefaultInfo.powerUsage) * .1f;
            if (factor < 0.0f)
            {
                factor = 0.0f;
            }
            return factor;
        }

        /// <summary>
        /// returns the maximum integrity of the Turret
        /// </summary>
        /// <returns></returns>
        public float GetTurretMaxInteg()
        {
            float maxinteg = cubeBlock.SlimBlock.MaxIntegrity;
            return maxinteg;
        }

        /// <summary>
        /// returns the integrity / health info of the Turret
        /// </summary>
        /// <returns></returns>
        public float GetTurretInteg()
        {
            float integ = cubeBlock.SlimBlock.Integrity;
            return integ;
        }

        /// <summary>
        /// returns the current build integrity of the turret - integrity
        /// </summary>
        /// <returns></returns>
        public float getTurretCurrentDamage()
        {
            float integ = cubeBlock.SlimBlock.CurrentDamage;
            return integ;
        }

        /// <summary>
        /// returns the current accumulated damage of the turret
        /// </summary>
        /// <returns></returns>
        public float getTurretCurrentAccumDamage()
        {
            float integ = cubeBlock.SlimBlock.AccumulatedDamage;
            return integ;
        }

        /// <summary>
        /// returns a detailed list of damaged parts inside the turret
        /// </summary>
        /// <returns></returns>
        public List<string> getTurretMissingComponents()
        {
            string partName = "";
            List<string> componentList = new List<string>();
            var components = new Dictionary<string, int>();
            cubeBlock.SlimBlock.GetMissingComponents(components);
            foreach (var part in components)
            {
                partName = part.ToString();
                componentList.Add(partName);
            }
            return componentList;
        }

        /// <summary>
        /// returns the ammount of missing Components inside the turret. 
        /// not individual components but by type.
        /// for example if you had 100 steel plate and 100 interior plates missing
        /// it would only return 2.
        /// </summary>
        /// <returns></returns>
        public long getAmmountOfMissingComponents()
        {
            long partName = 0;
            var components = new Dictionary<string, int>();
            cubeBlock.SlimBlock.GetMissingComponents(components);
            partName = components.LongCount();
            return partName;
        }

        /// <summary>
        /// returns the integrity of the ship in a percentage format
        /// </summary>
        /// <returns></returns>
        public float getTurretDamagePercentage()
        {
            float percentage = 0.0f;
            percentage = 100 - (100.0f * Math.Abs(GetTurretMaxInteg() - GetTurretInteg()) / (float)Math.Max(GetTurretMaxInteg(), GetTurretInteg()));
            return percentage;
        }

        /// <summary>
        /// this is experimental bullshit
        /// </summary>
        /// <returns></returns>
        public int getTurretComponentAmmount()
        {
            //var components = new 
            int ammount = cubeBlock.SlimBlock.GetConstructionStockpileItemAmount(electricityDefinition);
            return ammount;
        } 
        #endregion

        /// <summary>
        /// This meathod actually draws the lasers if you want to adjust them look no further 
        /// </summary>
        #region VRAGE Draw Laser Config

        public void DrawToCenterLasers()
        {

                var maincolor = PrimaryBeamColor().ToVector4();
                var auxcolor = SecondaryBeamColor().ToVector4();
                var material = MyStringId.GetOrCompute("WeaponLaser");

                VRage.Game.MySimpleObjectDraw.DrawLine(from, to, material, ref auxcolor, beamWeaponColorInfo.auxcolorwide  * MaxModulation());
                VRage.Game.MySimpleObjectDraw.DrawLine(from, to, material, ref maincolor, beamWeaponColorInfo.colorwide *  MaxCurrent());

        }
        public void DrawToCenterTargetLasers()
        {

                var maincolor = PrimaryBeamColor().ToVector4();
                var auxcolor = SecondaryBeamColor().ToVector4();
                var material = MyStringId.GetOrCompute("WeaponLaser");

                VRage.Game.MySimpleObjectDraw.DrawLine(from, toTarget, material, ref auxcolor, beamWeaponColorInfo.auxcolorwide * MaxModulation());
                VRage.Game.MySimpleObjectDraw.DrawLine(from, toTarget, material, ref maincolor, beamWeaponColorInfo.colorwide * MaxCurrent());
        }

        public void DrawToLaser(Vector3D from, Vector3D to)
        {

            var maincolor = PrimaryBeamColor().ToVector4();
            var auxcolor = SecondaryBeamColor().ToVector4();
            var material = MyStringId.GetOrCompute("WeaponLaser");

            VRage.Game.MySimpleObjectDraw.DrawLine(from, to, material, ref auxcolor, beamWeaponColorInfo.auxcolorwide * MaxModulation());
            VRage.Game.MySimpleObjectDraw.DrawLine(from, to, material, ref maincolor, beamWeaponColorInfo.colorwide * MaxCurrent());

        }
        public void DrawToTargetLaser(Vector3D from, Vector3D toTarget)
        {

            var maincolor = PrimaryBeamColor().ToVector4();
            var auxcolor = SecondaryBeamColor().ToVector4();
            var material = MyStringId.GetOrCompute("WeaponLaser");

            VRage.Game.MySimpleObjectDraw.DrawLine(from, toTarget, material, ref auxcolor, beamWeaponColorInfo.auxcolorwide * MaxModulation());
            VRage.Game.MySimpleObjectDraw.DrawLine(from, toTarget, material, ref maincolor, beamWeaponColorInfo.colorwide * MaxCurrent());
        }


        #endregion

        /// <summary>
        /// contains all the meathods needed to scann for objects along the beams
        /// </summary>
        /// <returns></returns>
        #region Scaning Logic

        public LineD OriginalRayTest()
        {
            LineD testRay = new LineD(from, to);
            return testRay;
        }
        public List<MyLineSegmentOverlapResult<MyEntity>> OriginalSurfaceScan()
        {
            LineD testRay = OriginalRayTest();
            List<MyLineSegmentOverlapResult<MyEntity>> result = new List<MyLineSegmentOverlapResult<MyEntity>>();
            MyGamePruningStructure.GetTopmostEntitiesOverlappingRay(ref testRay, result);
            return result;
        }

        public LineD RayTest()
        {
            LineD testRay = new LineD(barrelTip, beamEndpoint);
            return testRay;
        }
        public List<MyLineSegmentOverlapResult<MyEntity>> SurfaceScan()
        {
            LineD testRay = RayTest();
            List<MyLineSegmentOverlapResult<MyEntity>> result = new List<MyLineSegmentOverlapResult<MyEntity>>();
            MyGamePruningStructure.GetTopmostEntitiesOverlappingRay(ref testRay, result);
            return result;
        }

        public List<IHitInfo> RayCast()
        {
            var hitinfos = new List<IHitInfo>();
            MyAPIGateway.Physics.CastRay(beamEndpoint, beamEndpoint, hitinfos, 1);
            return hitinfos;
        }
        public List<IHitInfo> RayCastCenter()
        {
            var hitinfos = new List<IHitInfo>();
            MyAPIGateway.Physics.CastRay(from, to, hitinfos, 1);
            return hitinfos;
        }

        #endregion

        /// <summary>
        /// preforms all the loops to check for entities cubegrids and vodels and does the damage
        /// </summary>
        #region Damage Logic

        public void DoTargetingBarelDamage()
        {
            foreach (var resultItem in OriginalSurfaceScan())
            {
                LineD testRay = OriginalRayTest();
                IMyCubeGrid grid = resultItem.Element as IMyCubeGrid;
                IMyDestroyableObject destroyableEntity = resultItem.Element as IMyDestroyableObject;
                if (grid != null)
                {
                    IMySlimBlock slimblock;
                    double hitd;
                    Vector3D? resultVec = grid.GetLineIntersectionExactAll(ref testRay, out hitd, out slimblock);

                    if (resultVec != null)
                    {
                        if (grid.Physics != null)
                        {
                            hitCenter = true;
                            toTarget = from + DefineTargetingBeam().WorldMatrix.Forward * hitd;

                            MyAPIGateway.Utilities.SendModMessage(920304543, new KeyValuePair<IMyCubeGrid, float>(slimblock.CubeGrid, getModulation()));

                            if (!MyAPIGateway.Session.CreativeMode)//if not creative mode
                            {
                                if (beamWeaponDefaultInfo.issuper == true) //if is a super weapon
                                {
                                    DoCenterExplosiveDamage(beamWeaponDefaultInfo.expradius);
                                }
                                else
                                {
                                    slimblock.DoDamage(getCurrent(), MyStringHash.GetOrCompute("Laser"), false, default(MyHitInfo), cubeBlock.EntityId);
                                }
                            }
                            else//<---------Creative
                            {
                                if (beamWeaponDefaultInfo.issuper == true)
                                {
                                    DoCenterExplosiveDamage(beamWeaponDefaultInfo.expradius);
                                }
                                else
                                {
                                    slimblock.DoDamage(getCurrent(), MyStringHash.GetOrCompute("Laser"), false, default(MyHitInfo), cubeBlock.EntityId);
                                }
                            }
                        }
                    }
                }
                else if (destroyableEntity != null)
                {
                    IMyEntity ent = (IMyEntity)destroyableEntity;
                    double hitd = (from - ent.WorldMatrix.Translation).Length();

                    if (ent is IMyCharacter)
                    {
                        toTarget = from + DefineTargetingBeam().WorldMatrix.Forward * hitd;
                        hitCenter = true;
                        if (!MyAPIGateway.Session.CreativeMode)
                        {
                            if (beamWeaponDefaultInfo.issuper == true)
                            {
                                DoCenterExplosiveDamage(beamWeaponDefaultInfo.expradius);
                            }
                            else
                            {
                                destroyableEntity.DoDamage(getCurrent(), MyStringHash.GetOrCompute("Laser"), false, default(MyHitInfo), cubeBlock.EntityId);
                            }
                        }
                        else//<------------Creative
                        {
                            if (beamWeaponDefaultInfo.issuper == true)
                            {
                                DoCenterExplosiveDamage(beamWeaponDefaultInfo.expradius);
                            }
                            else
                            {
                                destroyableEntity.DoDamage(getCurrent(), MyStringHash.GetOrCompute("Laser"), false, default(MyHitInfo), cubeBlock.EntityId);
                            }
                        }
                    }
                }
            }
        }
        public void DoDamage()
        {
            foreach (var resultItem in SurfaceScan())
            {
                LineD testRay = RayTest();
                IMyCubeGrid grid = resultItem.Element as IMyCubeGrid;
                IMyDestroyableObject destroyableEntity = resultItem.Element as IMyDestroyableObject;
                if (grid != null)
                {
                    IMySlimBlock slimblock;
                    double hitd;
                    Vector3D? resultVec = grid.GetLineIntersectionExactAll(ref testRay, out hitd, out slimblock);

                    if (resultVec != null)
                    {
                        if (grid.Physics != null)
                        {

                            hitPoint = true;
                            beamTarget = barrelTip + DB().WorldMatrix.Forward * hitd;
                            //MyAPIGateway.Utilities.SendModMessage(920304543, new KeyValuePair<IMyCubeGrid, float>(slimblock.CubeGrid, getModulation()));

                            if (!MyAPIGateway.Session.CreativeMode)//if not creative mode
                            {
                                if (beamWeaponDefaultInfo.issuper == true) //if is a super weapon
                                {
                                    DoCenterExplosiveDamage(beamWeaponDefaultInfo.expradius);
                                }
                                else
                                {
                                    slimblock.DoDamage(getCurrent() / beamWeaponDefaultInfo.barrels, MyStringHash.GetOrCompute("Laser"), false, default(MyHitInfo), cubeBlock.EntityId);
                                }
                            }
                            else//<---------Creative
                            {
                                if (beamWeaponDefaultInfo.issuper == true)
                                {
                                    DoExplosiveDamage(beamWeaponDefaultInfo.expradius);
                                }
                                else
                                {
                                    slimblock.DoDamage(getCurrent() / beamWeaponDefaultInfo.barrels, MyStringHash.GetOrCompute("Laser"), false, default(MyHitInfo), cubeBlock.EntityId);
                                }
                            }
                        }
                    }
                }
                else if (destroyableEntity != null)
                {
                    IMyEntity ent = (IMyEntity)destroyableEntity;
                    double hitd = (barrelTip - ent.WorldMatrix.Translation).Length();

                    if (ent is IMyCharacter)
                    {
                        beamTarget = barrelTip + DB().WorldMatrix.Forward * hitd;
                        
                        hitPoint = true;
                        if (!MyAPIGateway.Session.CreativeMode)
                        {
                            if (beamWeaponDefaultInfo.issuper == true)
                            {
                                    DoExplosiveDamage(beamWeaponDefaultInfo.expradius);
                            }
                            else
                            {
                                destroyableEntity.DoDamage(getCurrent() / beamWeaponDefaultInfo.barrels, MyStringHash.GetOrCompute("Laser"), false, default(MyHitInfo), cubeBlock.EntityId);

                            }
                        }
                        else//<------------Creative
                        {
                            if (beamWeaponDefaultInfo.issuper == true)
                            {
                                DoCenterExplosiveDamage(beamWeaponDefaultInfo.expradius);
                            }
                            else
                            {
                                destroyableEntity.DoDamage(getCurrent() / beamWeaponDefaultInfo.barrels, MyStringHash.GetOrCompute("Laser"), false, default(MyHitInfo), cubeBlock.EntityId);
                            }
                        }
                    }
                }
                var hitinfos = new List<IHitInfo>();
                MyAPIGateway.Physics.CastRay(barrelTip, beamEndpoint, hitinfos, 1);
                foreach (var hitinfo in hitinfos)
                {
                    var Obstruction = hitinfos.First();
                    if (hitinfo.HitEntity is IMyVoxelBase && hitinfo == Obstruction) // if the entities in  the raycast is the first object in the list of voxels if not you shoot straigt through the rock
                    {
                        if (hitinfo != null)
                        {
                            beamTarget = hitinfo.Position;
                            hitPoint = true;
                            if (MyAPIGateway.Multiplayer.IsServer != true)
                            {

                                if (beamWeaponDefaultInfo.issuper == true)
                                {
                                    DoExplosiveDamage(beamWeaponDefaultInfo.expradius);

                                }
                                else
                                {

                                    //DoExplosiveDamage(beamWeaponDefaultInfo.expradius);

                                    //nothing yet hope to do other shit here
                                }

                            }
                        }
                        else
                        {
                            hitPoint = false;
                        }
                    }
                }
            }

        }

        #endregion

        public Beam Beam(Vector3D from, Vector3D to)
        {
            return Beam(from, to);
        }

        /// <summary>
        /// Defines the beginning and end points of the default raycast before targeting
        /// </summary>
        /// <returns></returns>
        /// 
        public MyEntitySubpart DefineBeam(double forwardOrigin, float LeftOffset, float RightOffset, float UpOffset, float DownOffset)
        {
            MyEntitySubpart tBase = cubeBlock.GetSubpart("MissileTurretBase1");
            MyEntitySubpart barrel = tBase.GetSubpart("MissileTurretBarrels");

            barrelTip = barrel.WorldMatrix.Translation +
                (barrel.WorldMatrix.Forward * forwardOrigin) +
                (barrel.WorldMatrix.Right * RightOffset) +
                (barrel.WorldMatrix.Left * LeftOffset) +
                (barrel.WorldMatrix.Up * UpOffset) +
                (barrel.WorldMatrix.Down * DownOffset);

            beamEndpoint = barrel.WorldMatrix.Translation +
                (barrel.WorldMatrix.Forward * getRange()) +
                (barrel.WorldMatrix.Right * RightOffset) +
                (barrel.WorldMatrix.Left * LeftOffset) +
                (barrel.WorldMatrix.Up * UpOffset) +
                (barrel.WorldMatrix.Down * DownOffset);

            return barrel;
        }
        public MyEntitySubpart DB()
        {
            MyEntitySubpart tBase = cubeBlock.GetSubpart("MissileTurretBase1");
            MyEntitySubpart barrel = tBase.GetSubpart("MissileTurretBarrels");
            return barrel;
        }
        public MyEntitySubpart DefineTargetingBeam()
        {
            #region Beam Orientation
            MyEntitySubpart subpart1 = cubeBlock.GetSubpart("MissileTurretBase1");
            MyEntitySubpart subpart2 = subpart1.GetSubpart("MissileTurretBarrels");
            MyEntitySubpart sub2 = subpart2;
            from = subpart2.WorldMatrix.Translation + (subpart2.WorldMatrix.Forward * beamWeaponTargetingLaser.forwardOrigin) + subpart2.WorldMatrix.Up * beamWeaponTargetingLaser.upOffset;
            to = subpart2.WorldMatrix.Translation + subpart2.WorldMatrix.Forward * (getRange());
            return sub2;
            #endregion
        }

        /// <summary>
        /// Use these functions to emulate damage on voxels or when the Super laser check in BWM config is checked
        /// In the future this will be more robust.
        /// </summary>
        #region ExpDamage
        public void DoExplosiveDamage(float expRad)
        {

                MyExplosionInfo exp = new MyExplosionInfo(getModulation(), getCurrent(), new BoundingSphereD(beamTarget, expRad), Sandbox.Game.MyExplosionTypeEnum.GRID_DESTRUCTION, true, true)
                {
                    OwnerEntity = _ownerEntity as MyEntity,
                    VoxelExplosionCenter = beamTarget
                };
                MyExplosions.AddExplosion(ref exp, true);

        }
        public void DoCenterExplosiveDamage(float expRad)
        {
            if (beamWeaponDefaultInfo.barrels == 2)
            {
                MyExplosionInfo exp = new MyExplosionInfo(getModulation(), getCurrent(), new BoundingSphereD(toTarget, expRad), Sandbox.Game.MyExplosionTypeEnum.GRID_DESTRUCTION, true, true)
                {
                    OwnerEntity = _ownerEntity as MyEntity,
                    VoxelExplosionCenter = toTarget
                };
                MyExplosions.AddExplosion(ref exp, true);

            }

        }
        #endregion

        public void PowerLogic()
        {
            terminalBlock.RefreshCustomInfo();
            if (m_requiredInput() <= m_maximumInput())
            {
            int chargesInInventory = (int)m_inventory.GetItemAmount(chargeDefinitionIds[damageUpgrades]);
            if (chargesInInventory < beamWeaponExtendedHeatInfo.KeepAtCharge)
            {
                if (resourceSink.RequiredInputByType(electricityDefinition) != m_maximumInput())
                {
                    resourceSink.SetRequiredInputByType(electricityDefinition, m_maximumInput());

                    setPowerConsumption = (m_maximumInput());
                    
                    powerConsumption = (m_maximumInput());
                }
                else
                {
                    if (!functionalBlock.Enabled)
                    {
                        
                        powerConsumption = 0.0001f;
                        ///StopLeftBarrelEffects1();
                        ///StopRightBarrelEffects1();
                        ///StopLeftBarrelEffects();
                        ///StopRightBarrelEffects();
                        ///StopTargetEffects();
                    }


                }

                if (resourceSink.CurrentInputByType(electricityDefinition) == m_maximumInput())
                {

                    if (!overheated)
                    {
                        m_inventory.AddItems((MyFixedPoint)(beamWeaponExtendedHeatInfo.KeepAtCharge - chargesInInventory), chargeObjectBuilders[damageUpgrades]);
                       
                       /// StartLeftBarrelEffects1();
                        ///StartRightBarrelEffects1();
                        ///StartLeftBarrelEffects();
                        ///StartRightBarrelEffects();
                       /// StartTargetEffects();
                    }
                }
            }
            else if (chargesInInventory > beamWeaponExtendedHeatInfo.KeepAtCharge)
            {
                m_inventory.RemoveItemsOfType((MyFixedPoint)(chargesInInventory - beamWeaponExtendedHeatInfo.KeepAtCharge), chargeObjectBuilders[damageUpgrades]);
            }
            else
            {
                if (setPowerConsumption != 0.0001f)
                {
                    resourceSink.SetRequiredInputByType(electricityDefinition, 0.0001f);
                    setPowerConsumption = 0.0001f;
                    powerConsumption = 0.0001f;

                    ///StopLeftBarrelEffects1();
                    ///StopRightBarrelEffects1();
                    ///StopLeftBarrelEffects();
                    ///StopRightBarrelEffects();
                    ///StopTargetEffects();
                }
            }
            }
            else
            {
                int chargesInInventory = (int)m_inventory.GetItemAmount(chargeDefinitionIds[damageUpgrades]);
                if (chargesInInventory < beamWeaponExtendedHeatInfo.KeepAtCharge)
                {
                    if (resourceSink.RequiredInputByType(electricityDefinition) != 0.0001f)
                    {
                        resourceSink.SetRequiredInputByType(electricityDefinition, 0.0001f);

                        setPowerConsumption = (0.0001f);

                        powerConsumption = (0.0001f);
                    }
                    else
                    {
                        if (!functionalBlock.Enabled)
                        {

                            powerConsumption = 0.0001f;
                            ///StopLeftBarrelEffects1();
                            ///StopRightBarrelEffects1();
                            ///StopLeftBarrelEffects();
                            ///StopRightBarrelEffects();
                            ///StopTargetEffects();
                        }


                    }

                    if (resourceSink.CurrentInputByType(electricityDefinition) == m_maximumInput())
                    {

                        if (!overheated)
                        {
                            m_inventory.AddItems((MyFixedPoint)(beamWeaponExtendedHeatInfo.KeepAtCharge - chargesInInventory), chargeObjectBuilders[damageUpgrades]);

                            /// StartLeftBarrelEffects1();
                            ///StartRightBarrelEffects1();
                            ///StartLeftBarrelEffects();
                            ///StartRightBarrelEffects();
                            /// StartTargetEffects();
                        }
                    }
                }
                else if (chargesInInventory > beamWeaponExtendedHeatInfo.KeepAtCharge)
                {
                    m_inventory.RemoveItemsOfType((MyFixedPoint)(chargesInInventory - beamWeaponExtendedHeatInfo.KeepAtCharge), chargeObjectBuilders[damageUpgrades]);
                }
                else
                {


                    if (setPowerConsumption != 0.0001f)
                    {
                        resourceSink.SetRequiredInputByType(electricityDefinition, 0.0001f);
                        setPowerConsumption = 0.0001f;
                        powerConsumption = 0.0001f;

                        ///StopLeftBarrelEffects1();
                        ///StopRightBarrelEffects1();
                        ///StopLeftBarrelEffects();
                        ///StopRightBarrelEffects();
                        ///StopTargetEffects();
                    }
                }
            }



        }

        public void HeatIncrimentationLogic()
        {
            if (!overheatedResidual)
            {
                currentHeat += beamWeaponExtendedHeatInfo.HeatPerTick;

                if (currentHeat > beamWeaponExtendedHeatInfo.MaxHeat)
                {

                    shotCount++;
                    currentHeat = beamWeaponExtendedHeatInfo.MaxHeat;
                    overheated = true;

                }
            }
            else
            {
                overheated = true;
            }
         

        }

        public void HeatDisipationLogic() 
        {
            if (!overheatedResidual)
            {
                if (currentHeat > 0f)
                {
                    if ((ticks - lastShootTimeTicks) > beamWeaponExtendedHeatInfo.HeatDisDelay)
                    {
                    
                        currentHeat -= beamWeaponExtendedHeatInfo.HeatDissipationPerTick;
                        if (currentHeat <= 0f)
                        {

                            currentHeat = 0f;
                            overheated = false;

                        }
                    }
                }

            }

        }

        public void ResidualHeatIncrimentationLogic()
        {
            //you can tell the residual heat when to start increasing in value
            if (currentHeat > beamWeaponExtendedHeatInfo.ResidualHeatIncDelay)
            {
                currentHeatResidual += beamWeaponExtendedHeatInfo.ResidualHeatPerTick;
            }
            if (currentHeatResidual > beamWeaponExtendedHeatInfo.ResidualMaxHeat)
            {
                currentHeatResidual = beamWeaponExtendedHeatInfo.ResidualMaxHeat;
                overheatedResidual = true;
            }

        }

        public void ResidualHeatDisipationLogic()
        {
            if ((overheated) && (ticks - lastShootTimeTicks) > beamWeaponExtendedHeatInfo.ResidualHeatDisDelay)
            {
                currentHeatResidual -= beamWeaponExtendedHeatInfo.ResidualDissipationPerTick;
                if (currentHeatResidual <= 0f)
                {
                    shotCount = 1;
                    currentHeatResidual = 0f;
                    overheatedResidual = false;

                }

            }

        }

        public long CurrentShootTime()
        {
            IMyCubeBlock cube = Entity as IMyCubeBlock;
            long currentShootTime = ((MyObjectBuilder_LargeMissileTurret)cube.GetObjectBuilderCubeBlock()).GunBase.LastShootTime;
            return currentShootTime;
        }

        public void FireTimeingUpdate()
        {

            lastShootTime = CurrentShootTime();
            lastShootTimeTicks = ticks;

        }

        public void BeamShow2(Beam beam)
        {

            if (ticks - lastShootTimeTicks < 4)
            {
                if (!MyAPIGateway.Utilities.IsDedicated)
                {
                    DrawToLaser(beam.Origin, beam.End);
                }
            }
        }

        public void BeamShowTarget(Vector3D from, Vector3D toTarget)
        {
            if (ticks - lastShootTimeTicks < 3)
            {
                if (!MyAPIGateway.Utilities.IsDedicated)
                {
                    DrawToTargetLaser(from, toTarget);
                }
            }
        }

        public void BeamShow(Vector3D from, Vector3D to)
        {
            if (ticks - lastShootTimeTicks < 3)
            {
                if (!MyAPIGateway.Utilities.IsDedicated)
                {
                    DrawToLaser(from, to);
                }
            }
        }

        public override void Init(MyObjectBuilder_EntityBase objectBuilder)
        {

            base.Init(objectBuilder);
            this.objectBuilder = objectBuilder;

            Entity.NeedsUpdate |= MyEntityUpdateEnum.BEFORE_NEXT_FRAME | MyEntityUpdateEnum.EACH_FRAME;
            functionalBlock = Entity as Sandbox.ModAPI.IMyFunctionalBlock;
            cubeBlock = Entity as IMyCubeBlock;
            terminalBlock = Entity as Sandbox.ModAPI.IMyTerminalBlock;
            IMyCubeBlock cube = Entity as IMyCubeBlock;

            subtypeName = functionalBlock.BlockDefinition.SubtypeName;

            getBeamWeaponInfo(subtypeName);

            InitCharges();

            
            terminalBlock.AppendingCustomInfo += appendCustomInfo;

            lastShootTime = ((MyObjectBuilder_LargeMissileTurret)cube.GetObjectBuilderCubeBlock()).GunBase.LastShootTime;

        }

        public override MyObjectBuilder_EntityBase GetObjectBuilder(bool copy = false)
        {
            return objectBuilder;
        }

        public void appendCustomInfo(Sandbox.ModAPI.IMyTerminalBlock block, StringBuilder info)
        {

            info.AppendLine(cubeBlock.DefinitionDisplayNameText);
            info.AppendLine("Weapon Integrity: " + (getTurretDamagePercentage()).ToString("N") + "%");

            if (getPowerOveride())
            {

                info.AppendLine("Weapon Degridation Factor: " + (GetTurretDegFactor()).ToString("N") + "%");
                info.AppendLine(" ");
            }

            if (overheatedResidual)
            {
                info.AppendLine(" ");
                info.AppendLine("ResidualHeat: " + (getResidualHeat()).ToString("N") + "/" + (beamWeaponExtendedHeatInfo.ResidualMaxHeat).ToString("N") + "C");
                info.AppendLine(" ");
                info.AppendLine("Warning");
                info.AppendLine(" ");
                info.AppendLine("Weapon Cooldown Sequence");
                info.AppendLine(" ");
                info.AppendLine("Warning");
            }
            else
            {
                info.AppendLine("Power Overide: " + getPowerOveride());
                info.AppendLine("Maximum Input: " + (m_maximumInput()).ToString("N") + "MW");
                info.AppendLine("Required Input: " + (m_requiredInput()).ToString("N") + "MW");
                info.AppendLine(" ");
                info.AppendLine("ResidualHeat: " + (getResidualHeat()).ToString("N") + "/" + (beamWeaponExtendedHeatInfo.ResidualMaxHeat).ToString("N") + "C");
                info.AppendLine("Volley: " + (shotCount/10).ToString());
                info.AppendLine("Range: " + (getRange()).ToString("N") + "/Meters");
                info.AppendLine("Current: " + (getCurrent()).ToString("N") + "/Jouls");
                info.AppendLine("Modulation: " + (getModulation()).ToString("N") + "/M.M.F.");
                if (getPowerOveride())
                {
                    info.AppendLine(" ");
                    info.AppendLine("Component Degridation Values:");
                    if (getAmmountOfMissingComponents() == 1)
                    {
                        info.AppendLine(" " + getTurretMissingComponents().ElementAt(0));
                    }
                    else if (getAmmountOfMissingComponents() == 2)
                    {
                        info.AppendLine(" " + getTurretMissingComponents().ElementAt(0));
                        info.AppendLine(" " + getTurretMissingComponents().ElementAt(1));
                    }
                    else if (getAmmountOfMissingComponents() == 3)
                    {
                        info.AppendLine(" " + getTurretMissingComponents().ElementAt(0));
                        info.AppendLine(" " + getTurretMissingComponents().ElementAt(1));
                        info.AppendLine(" " + getTurretMissingComponents().ElementAt(2));
                    }
                    else if (getAmmountOfMissingComponents() == 4)
                    {
                        info.AppendLine(" " + getTurretMissingComponents().ElementAt(0));
                        info.AppendLine(" " + getTurretMissingComponents().ElementAt(1));
                        info.AppendLine(" " + getTurretMissingComponents().ElementAt(2));
                        info.AppendLine(" " + getTurretMissingComponents().ElementAt(3));

                    }
                    else if (getAmmountOfMissingComponents() == 5)
                    {
                        info.AppendLine(" " + getTurretMissingComponents().ElementAt(0));
                        info.AppendLine(" " + getTurretMissingComponents().ElementAt(1));
                        info.AppendLine(" " + getTurretMissingComponents().ElementAt(2));
                        info.AppendLine(" " + getTurretMissingComponents().ElementAt(3));
                        info.AppendLine(" " + getTurretMissingComponents().ElementAt(4));
                    }
                    info.AppendLine(" ");
                }

            }


        }

        private void InitCharges()
        {
            chargeObjectBuilders = new List<MyObjectBuilder_AmmoMagazine>();
            chargeObjectBuilders.Add(new MyObjectBuilder_AmmoMagazine() { SubtypeName = "" + beamWeaponDefaultInfo.ammoName });

            chargeDefinitionIds = new List<SerializableDefinitionId>();
            chargeDefinitionIds.Add(new SerializableDefinitionId(typeof(MyObjectBuilder_AmmoMagazine), "" + beamWeaponDefaultInfo.ammoName));
        }

        private void getBeamWeaponInfo(string name)
        {
            //loads the Custom Ui Data no matter what turret it is.
            if (subtypeName == "SmallBeamBaseGTF_Large")
            {
                #region SmallBeamGTF
                Logging.writeLine(String.Format("{0} - " + subtypeName, DateTime.Now));

                beamWeaponDefaultInfo = SB_Default_Settings.Small_Beam;
                beamWeaponUiInfo = SB_Default_Ui_Settings.Small_Beam;
                beamWeaponExtendedHeatInfo = SB_Heat_Settings.Small_Beam;
                beamWeaponColorInfo = SB_Color_Settings.Small_Beam;

                beamWeaponTargetingLaser = SB_Barrel_Settings.SB_Targeting_Beam.T_Beam;
                for (int i = 1; i < beamWeaponDefaultInfo.barrels + 1; i++)
                {

                    if (i == 1)
                    {
                        beamWeaponBarrel = SB_Barrel_Settings.Small_Beam.B1;
                        BarrelList.Add(beamWeaponBarrel);
                    }


                }

                Toggle = SB_Custom_Ui_Settings.Toggle.SB_PowerOveride;
                rangeSlider = SB_Custom_Ui_Settings.Slider.SB_RangeSlider;
                powerSlider = SB_Custom_Ui_Settings.Slider.SB_PowerSlider;
                currentSlider = SB_Custom_Ui_Settings.Slider.SB_CurrentSlider;
                modulationSlider = SB_Custom_Ui_Settings.Slider.SB_ModulationSlider; 
                #endregion
            }
            else if (subtypeName == "Interior_Pulse_Laser_Base_Large")
            {
                #region InteriorPulseGTF
                Logging.writeLine(String.Format("{0} - " + subtypeName, DateTime.Now));
                beamWeaponDefaultInfo = IP_Default_Settings.Interior_Pulse;
                beamWeaponUiInfo = IP_Default_Ui_Settings.Interior_Pulse;
                beamWeaponExtendedHeatInfo = IP_Heat_Settings.Interior_Pulse;
                beamWeaponColorInfo = IP_Color_Settings.Interior_Pulse;

                beamWeaponTargetingLaser = IP_Barrel_Settings.IP_Targeting_Beam.T_Beam;
                for (int i = 1; i < beamWeaponDefaultInfo.barrels + 1; i++)
                {

                    if (i == 1)
                    {
                        beamWeaponBarrel = IP_Barrel_Settings.Interior_Pulse.B1;
                        BarrelList.Add(beamWeaponBarrel);
                    }


                }

                Toggle = IP_Custom_Ui_Settings.Toggle.IP_PowerOveride;
                rangeSlider = IP_Custom_Ui_Settings.Slider.IP_RangeSlider;
                powerSlider = IP_Custom_Ui_Settings.Slider.IP_PowerSlider;
                currentSlider = IP_Custom_Ui_Settings.Slider.IP_CurrentSlider;
                modulationSlider = IP_Custom_Ui_Settings.Slider.IP_ModulationSlider; 
                #endregion
            }
            else if (subtypeName == "SmallPulseLaser_Base_Large")
            {
                #region SmallPulseGTF
                Logging.writeLine(String.Format("{0} - " + subtypeName, DateTime.Now));
                beamWeaponDefaultInfo = SP_Default_Settings.Small_Pulse;
                beamWeaponUiInfo = SP_Default_Ui_Settings.Small_Pulse;
                beamWeaponColorInfo = SP_Color_Settings.Small_Pulse;
                beamWeaponExtendedHeatInfo = SP_Heat_Settings.Small_Pulse;


                beamWeaponTargetingLaser = SP_Barrel_Settings.SP_Targeting_Beam.T_Beam;
                for (int i = 1; i < beamWeaponDefaultInfo.barrels + 1; i++)
                {

                    if (i == 1)
                    {
                        beamWeaponBarrel = SP_Barrel_Settings.Small_Pulse.B1;
                        BarrelList.Add(beamWeaponBarrel);
                    }

                }

                Toggle = SP_Custom_Ui_Settings.Toggle.SP_PowerOveride;
                rangeSlider = SP_Custom_Ui_Settings.Slider.SP_RangeSlider;
                powerSlider = SP_Custom_Ui_Settings.Slider.SP_PowerSlider;
                currentSlider = SP_Custom_Ui_Settings.Slider.SP_CurrentSlider;
                modulationSlider = SP_Custom_Ui_Settings.Slider.SP_ModulationSlider; 
                #endregion
            }
            else if (subtypeName == "MediumQuadBeamGTFBase_Large")
            {
                #region MediumQuadBeamGTF
                Logging.writeLine(String.Format("{0} - " + subtypeName, DateTime.Now));
                beamWeaponDefaultInfo = MQB_Default_Settings.Medium_Quad_Beam;
                beamWeaponUiInfo = MQB_Default_Ui_Settings.Medium_Quad_Beam;
                beamWeaponColorInfo = MQB_Color_Settings.Medium_Quad_Beam;
                beamWeaponExtendedHeatInfo = MQB_Heat_Settings.Medium_Quad_Beam;

                beamWeaponTargetingLaser = MQB_Barrel_Settings.MQB_Targeting_Beam.T_Beam;
                for (int i = 1; i < beamWeaponDefaultInfo.barrels + 1; i++)
                {

                    if (i == 1)
                    {
                        beamWeaponBarrel = MQB_Barrel_Settings.Medium_Quad_Beam.B1;
                        BarrelList.Add(beamWeaponBarrel);
                    }
                    else if (i == 2)
                    {
                        beamWeaponBarrel = MQB_Barrel_Settings.Medium_Quad_Beam.B2;
                        BarrelList.Add(beamWeaponBarrel);
                    }

                    if (i == 3)
                    {
                        beamWeaponBarrel = MQB_Barrel_Settings.Medium_Quad_Beam.B3;
                        BarrelList.Add(beamWeaponBarrel);
                    }
                    else if (i == 4)
                    {
                        beamWeaponBarrel = MQB_Barrel_Settings.Medium_Quad_Beam.B4;
                        BarrelList.Add(beamWeaponBarrel);
                    }

                }

                Toggle = MQB_Custom_Ui_Settings.Toggle.MQB_PowerOveride;
                rangeSlider = MQB_Custom_Ui_Settings.Slider.MQB_RangeSlider;
                powerSlider = MQB_Custom_Ui_Settings.Slider.MQB_PowerSlider;
                currentSlider = MQB_Custom_Ui_Settings.Slider.MQB_CurrentSlider;
                modulationSlider = MQB_Custom_Ui_Settings.Slider.MQB_ModulationSlider;
                #endregion
            }
            else if (subtypeName == "MPulseLaserBase_Large")
            {
                #region MediumPulseGTF
                beamWeaponDefaultInfo = MP_Default_Settings.Medium_Pulse;
                beamWeaponUiInfo = MP_Default_Ui_Settings.Medium_Pulse;
                beamWeaponColorInfo = MP_Color_Settings.Medium_Pulse;
                beamWeaponExtendedHeatInfo = MP_Heat_Settings.Medium_Pulse;

                beamWeaponTargetingLaser = MP_Barrel_Settings.MP_Targeting_Beam.T_Beam;
                for (int i = 1; i < beamWeaponDefaultInfo.barrels + 1; i++)
                {

                    if (i == 1)
                    {
                        beamWeaponBarrel = MP_Barrel_Settings.Medium_Pulse.B1;
                        BarrelList.Add(beamWeaponBarrel);
                    }

                }

                Toggle = MP_Custom_Ui_Settings.Toggle.MP_PowerOveride;
                rangeSlider = MP_Custom_Ui_Settings.Slider.MP_RangeSlider;
                powerSlider = MP_Custom_Ui_Settings.Slider.MP_PowerSlider;
                currentSlider = MP_Custom_Ui_Settings.Slider.MP_CurrentSlider;
                modulationSlider = MP_Custom_Ui_Settings.Slider.MP_ModulationSlider; 
                #endregion
            }
            else if (subtypeName == "LargeDualBeamGTFBase_Large")
            {
                #region LargeDualBeam
                beamWeaponDefaultInfo = LDB_Default_Settings.Large_Dual_Beam;
                beamWeaponUiInfo = LDB_Default_Ui_Settings.Large_Dual_Beam;
                beamWeaponColorInfo = LDB_Color_Settings.Large_Dual_Beam;
                beamWeaponExtendedHeatInfo = LDB_Heat_Settings.Large_Dual_Beam;

                beamWeaponTargetingLaser = LDB_Barrel_Settings.LDB_Targeting_Beam.T_Beam;
                for (int i = 1; i < beamWeaponDefaultInfo.barrels + 1; i++)
                {

                    if (i == 1)
                    {
                        beamWeaponBarrel = LDB_Barrel_Settings.Large_Dual_Beam.B1;
                        BarrelList.Add(beamWeaponBarrel);
                    }
                    else if (i == 2)
                    {
                        beamWeaponBarrel = LDB_Barrel_Settings.Large_Dual_Beam.B2;
                        BarrelList.Add(beamWeaponBarrel);
                    }

                }

                Toggle = LDB_Custom_Ui_Settings.Toggle.LDB_PowerOveride;
                rangeSlider = LDB_Custom_Ui_Settings.Slider.LDB_RangeSlider;
                powerSlider = LDB_Custom_Ui_Settings.Slider.LDB_PowerSlider;
                currentSlider = LDB_Custom_Ui_Settings.Slider.LDB_CurrentSlider;
                modulationSlider = LDB_Custom_Ui_Settings.Slider.LDB_ModulationSlider; 
                #endregion
            }
            else if (subtypeName == "LDualPulseLaserBase_Large")
            {
                #region LargeDualPulseGTF
                beamWeaponDefaultInfo = LDP_Default_Settings.Large_Dual_Pulse;
                beamWeaponUiInfo = LDP_Default_Ui_Settings.Large_Dual_Pulse;
                beamWeaponColorInfo = LDP_Color_Settings.Large_Dual_Pulse;
                beamWeaponExtendedHeatInfo = LDP_Heat_Settings.Large_Dual_Pulse;

                beamWeaponTargetingLaser = LDP_Barrel_Settings.LDP_Targeting_Beam.T_Beam;
                for (int i = 1; i < beamWeaponDefaultInfo.barrels + 1; i++)
                {

                    if (i == 1)
                    {
                        beamWeaponBarrel = LDP_Barrel_Settings.Large_Dual_Pulse.B1;
                        BarrelList.Add(beamWeaponBarrel);
                    }
                    else if (i == 2)
                    {
                        beamWeaponBarrel = LDP_Barrel_Settings.Large_Dual_Pulse.B2;
                        BarrelList.Add(beamWeaponBarrel);
                    }

                }

                Toggle = LDP_Custom_Ui_Settings.Toggle.LDP_PowerOveride;
                rangeSlider = LDP_Custom_Ui_Settings.Slider.LDP_RangeSlider;
                powerSlider = LDP_Custom_Ui_Settings.Slider.LDP_PowerSlider;
                currentSlider = LDP_Custom_Ui_Settings.Slider.LDP_CurrentSlider;
                modulationSlider = LDP_Custom_Ui_Settings.Slider.LDP_ModulationSlider; 
                #endregion
            }
            else if (subtypeName == "XLDualPulseLaserBase_Large")
            {
                #region XLargeDualPulse
                beamWeaponDefaultInfo = XLDP_Default_Settings.XLarge_Dual_Pulse;
                beamWeaponUiInfo = XLDP_Default_Ui_Settings.XLarge_Dual_Pulse;
                beamWeaponColorInfo = XLDP_Color_Settings.XLarge_Dual_Pulse;
                beamWeaponExtendedHeatInfo = XLDP_Heat_Settings.XLarge_Dual_Pulse;

                beamWeaponTargetingLaser = XLDP_Barrel_Settings.XLDP_Targeting_Beam.T_Beam;
                for (int i = 1; i < beamWeaponDefaultInfo.barrels + 1; i++)
                {

                    if (i == 1)
                    {
                        beamWeaponBarrel = XLDP_Barrel_Settings.XLarge_Dual_Pulse.B1;
                        BarrelList.Add(beamWeaponBarrel);
                    }
                    else if (i == 2)
                    {
                        beamWeaponBarrel = XLDP_Barrel_Settings.XLarge_Dual_Pulse.B2;
                        BarrelList.Add(beamWeaponBarrel);
                    }

                }

                Toggle = XLDP_Custom_Ui_Settings.Toggle.XLDP_PowerOveride;
                rangeSlider = XLDP_Custom_Ui_Settings.Slider.XLDP_RangeSlider;
                powerSlider = XLDP_Custom_Ui_Settings.Slider.XLDP_PowerSlider;
                currentSlider = XLDP_Custom_Ui_Settings.Slider.XLDP_CurrentSlider;
                modulationSlider = XLDP_Custom_Ui_Settings.Slider.XLDP_ModulationSlider; 
                #endregion
            }
            else if (subtypeName == "XLGigaBeamGTFBase_Large")
            {
                #region XLGigaBeamGTF
                beamWeaponDefaultInfo = XLGB_Default_Settings.XLarge_Giga_Beam;
                beamWeaponUiInfo = XLGB_Default_Ui_Settings.XLarge_Giga_Beam;
                beamWeaponColorInfo = XLGB_Color_Settings.XLarge_Giga_Beam;
                beamWeaponExtendedHeatInfo = XLGB_Heat_Settings.XLarge_Giga_Beam;

                beamWeaponTargetingLaser = XLGB_Barrel_Settings.XLGB_Targeting_Beam.T_Beam;
                for (int i = 1; i < beamWeaponDefaultInfo.barrels + 1; i++)
                {

                    if (i == 1)
                    {
                        beamWeaponBarrel = XLGB_Barrel_Settings.XLarge_Giga_Beam.B1;
                        BarrelList.Add(beamWeaponBarrel);
                    }
                    else if (i == 2)
                    {
                        beamWeaponBarrel = XLGB_Barrel_Settings.XLarge_Giga_Beam.B2;
                        BarrelList.Add(beamWeaponBarrel);
                    }

                }

                Toggle = XLGB_Custom_Ui_Settings.Toggle.XLGB_PowerOveride;
                rangeSlider = XLGB_Custom_Ui_Settings.Slider.XLGB_RangeSlider;
                powerSlider = XLGB_Custom_Ui_Settings.Slider.XLGB_PowerSlider;
                currentSlider = XLGB_Custom_Ui_Settings.Slider.XLGB_CurrentSlider;
                modulationSlider = XLGB_Custom_Ui_Settings.Slider.XLGB_ModulationSlider; 
                #endregion
            }
            else if (subtypeName == "MPulseLaserBase_Small")
            {
                #region MediumPulseSmallGTF
                beamWeaponDefaultInfo = MPS_Default_Settings.Medium_Pulse_Small;
                beamWeaponUiInfo = MPS_Default_Ui_Settings.Medium_Pulse_Small;

                beamWeaponColorInfo = MPS_Color_Settings.Medium_Pulse_Small;
                beamWeaponExtendedHeatInfo = MPS_Heat_Settings.Medium_Pulse_Small;

                beamWeaponTargetingLaser = MPS_Barrel_Settings.MPS_Targeting_Beam.T_Beam;
                for (int i = 1; i < beamWeaponDefaultInfo.barrels + 1; i++)
                {

                    if (i == 1)
                    {
                        beamWeaponBarrel = MPS_Barrel_Settings.Medium_Pulse_Small.B1;
                        BarrelList.Add(beamWeaponBarrel);
                    }


                }

                Toggle = MPS_Custom_Ui_Settings.Toggle.MPS_PowerOveride;
                rangeSlider = MPS_Custom_Ui_Settings.Slider.MPS_RangeSlider;
                powerSlider = MPS_Custom_Ui_Settings.Slider.MPS_PowerSlider;
                currentSlider = MPS_Custom_Ui_Settings.Slider.MPS_CurrentSlider;
                modulationSlider = MPS_Custom_Ui_Settings.Slider.MPS_ModulationSlider; 
                #endregion
            }
            
            else if (subtypeName == "StationBeamBase_Large")
            {
                #region XXLPlanitaryGigaBeamAEGIS
                beamWeaponDefaultInfo = XXLPGB_Default_Settings.XXLarge_Planitary_GigaBeam;
                beamWeaponUiInfo = XXLPGB_Default_Ui_Settings.XXLarge_Planitary_GigaBeam;

                beamWeaponColorInfo = XXLPGB_Color_Settings.XXLarge_Planitary_GigaBeam;
                beamWeaponExtendedHeatInfo = XXLPGB_Heat_Settings.XXLarge_Planitary_GigaBeam;
                beamWeaponTargetingLaser = XXLPGB_Barrel_Settings.XXLPGB_Targeting_Beam.T_Beam;
                for (int i = 1; i < beamWeaponDefaultInfo.barrels + 1; i++)
                {

                    if (i == 1)
                    {
                        beamWeaponBarrel = XXLPGB_Barrel_Settings.XXLarge_Planitary_GigaBeam.B1;
                        BarrelList.Add(beamWeaponBarrel);
                    }


                }

                Toggle = XXLPGB_Custom_Ui_Settings.Toggle.XXLPGB_PowerOveride;
                rangeSlider = XXLPGB_Custom_Ui_Settings.Slider.XXLPGB_RangeSlider;
                powerSlider = XXLPGB_Custom_Ui_Settings.Slider.XXLPGB_PowerSlider;
                currentSlider = XXLPGB_Custom_Ui_Settings.Slider.XXLPGB_CurrentSlider;
                modulationSlider = XXLPGB_Custom_Ui_Settings.Slider.XXLPGB_ModulationSlider; 
                #endregion
            }
            else if (subtypeName == "AegisLargeBeamBase_Large")
            {
                #region AegisLargeBeamBase
                Logging.writeLine(String.Format("{0} - " + subtypeName, DateTime.Now));
                beamWeaponDefaultInfo = ALBB_Default_Settings.AegisLargeBeamBase;
                beamWeaponUiInfo = ALBB_Default_Ui_Settings.AegisLargeBeamBase;

                beamWeaponColorInfo = ALBB_Color_Settings.AegisLargeBeamBase;
                beamWeaponExtendedHeatInfo = ALBB_Heat_Settings.AegisLargeBeamBase;
                beamWeaponTargetingLaser = ALBB_Barrel_Settings.ALBB_Targeting_Beam.T_Beam;
                for (int i = 1; i < beamWeaponDefaultInfo.barrels + 1; i++)
                {

                    if (i == 1)
                    {
                        beamWeaponBarrel = ALBB_Barrel_Settings.AegisLargeBeamBase.B1;
                        BarrelList.Add(beamWeaponBarrel);
                    }
                    else if (i == 2)
                    {
                        beamWeaponBarrel = ALBB_Barrel_Settings.AegisLargeBeamBase.B2;
                        BarrelList.Add(beamWeaponBarrel);
                    }
                    else if (i == 3)
                    {
                        beamWeaponBarrel = ALBB_Barrel_Settings.AegisLargeBeamBase.B3;
                        BarrelList.Add(beamWeaponBarrel);
                    }
                    else if (i == 4)
                    {
                        beamWeaponBarrel = ALBB_Barrel_Settings.AegisLargeBeamBase.B4;
                        BarrelList.Add(beamWeaponBarrel);
                    }
                    else if (i == 5)
                    {
                        beamWeaponBarrel = ALBB_Barrel_Settings.AegisLargeBeamBase.B5;
                        BarrelList.Add(beamWeaponBarrel);
                    }
                    else if (i == 6)
                    {
                        beamWeaponBarrel = ALBB_Barrel_Settings.AegisLargeBeamBase.B6;
                        BarrelList.Add(beamWeaponBarrel);
                    }

                }

                Toggle = ALBB_Custom_Ui_Settings.Toggle.ALBB_PowerOveride;
                rangeSlider = ALBB_Custom_Ui_Settings.Slider.ALBB_RangeSlider;
                powerSlider = ALBB_Custom_Ui_Settings.Slider.ALBB_PowerSlider;
                currentSlider = ALBB_Custom_Ui_Settings.Slider.ALBB_CurrentSlider;
                modulationSlider = ALBB_Custom_Ui_Settings.Slider.ALBB_ModulationSlider;
                #endregion
            }
            else if (subtypeName == "AegisMediumeamBase_Large")
            {
                #region AegisMediumeamBase
                Logging.writeLine(String.Format("{0} - " + subtypeName, DateTime.Now));
                beamWeaponDefaultInfo = AMBB_Default_Settings.Aegis_Medium_Beam_Base;
                beamWeaponUiInfo = AMBB_Default_Ui_Settings.Aegis_Medium_Beam_Base;

                beamWeaponColorInfo = AMBB_Color_Settings.Aegis_Medium_Beam_Base;
                beamWeaponExtendedHeatInfo = AMBB_Heat_Settings.Aegis_Medium_Beam_Base;

                beamWeaponTargetingLaser = AMBB_Barrel_Settings.AMBB_Targeting_Beam.T_Beam;
                for (int i = 1; i < beamWeaponDefaultInfo.barrels + 1; i++)
                {

                    if (i == 1)
                    {
                        beamWeaponBarrel = AMBB_Barrel_Settings.Aegis_Medium_Beam_Base.B1;
                        BarrelList.Add(beamWeaponBarrel);
                    }
                    else if (i == 2)
                    {
                        beamWeaponBarrel = AMBB_Barrel_Settings.Aegis_Medium_Beam_Base.B2;
                        BarrelList.Add(beamWeaponBarrel);
                    }
                    else if (i == 3)
                    {
                        beamWeaponBarrel = AMBB_Barrel_Settings.Aegis_Medium_Beam_Base.B3;
                        BarrelList.Add(beamWeaponBarrel);
                    }
                    else if (i == 4)
                    {
                        beamWeaponBarrel = AMBB_Barrel_Settings.Aegis_Medium_Beam_Base.B4;
                        BarrelList.Add(beamWeaponBarrel);
                    }

                }

                Toggle = AMBB_Custom_Ui_Settings.Toggle.AMBB_PowerOveride;
                rangeSlider = AMBB_Custom_Ui_Settings.Slider.AMBB_RangeSlider;
                powerSlider = AMBB_Custom_Ui_Settings.Slider.AMBB_PowerSlider;
                currentSlider = AMBB_Custom_Ui_Settings.Slider.AMBB_CurrentSlider;
                modulationSlider = AMBB_Custom_Ui_Settings.Slider.AMBB_ModulationSlider;
                #endregion
            }
            else if (subtypeName == "AegisSmallBeamBase_Large")
            {
                #region AegisSmallBeamBase
                Logging.writeLine(String.Format("{0} - " + subtypeName, DateTime.Now));
                beamWeaponDefaultInfo = ASBB_Default_Settings.Aegis_Small_Beam_Base;
                beamWeaponUiInfo = ASBB_Default_Ui_Settings.Aegis_Small_Beam_Base;

                beamWeaponColorInfo = ASBB_Color_Settings.Aegis_Small_Beam_Base;
                beamWeaponExtendedHeatInfo = ASBB_Heat_Settings.Aegis_Small_Beam_Base;

                beamWeaponTargetingLaser = ASBB_Barrel_Settings.ASBB_Targeting_Beam.T_Beam;
                for (int i = 1; i < beamWeaponDefaultInfo.barrels + 1; i++)
                {

                    if (i == 1)
                    {
                        beamWeaponBarrel = ASBB_Barrel_Settings.Aegis_Small_Beam_Base.B1;
                        BarrelList.Add(beamWeaponBarrel);
                    }
                    else if (i == 2)
                    {
                        beamWeaponBarrel = ASBB_Barrel_Settings.Aegis_Small_Beam_Base.B2;
                        BarrelList.Add(beamWeaponBarrel);
                    }

                }

                Toggle = ASBB_Custom_Ui_Settings.Toggle.ASBB_PowerOveride;
                rangeSlider = ASBB_Custom_Ui_Settings.Slider.ASBB_RangeSlider;
                powerSlider = ASBB_Custom_Ui_Settings.Slider.ASBB_PowerSlider;
                currentSlider = ASBB_Custom_Ui_Settings.Slider.ASBB_CurrentSlider;
                modulationSlider = ASBB_Custom_Ui_Settings.Slider.ASBB_ModulationSlider;
                #endregion
            }

        }

        public override void UpdateOnceBeforeFrame()
        {
            resourceSink = Entity.Components.Get<MyResourceSinkComponent>();
            resourceSink.SetRequiredInputByType(electricityDefinition, 0.0001f);
            setPowerConsumption = .0001f;  
            m_inventory = ((Sandbox.ModAPI.Ingame.IMyTerminalBlock)Entity).GetInventory(0) as IMyInventory;
            terminalBlock.RefreshCustomInfo();
        }

        public override void UpdateBeforeSimulation()
        {
            if (Initialized)
            {
                Logging.writeLine(String.Format("{0} - Create UI {1}", DateTime.Now.ToString("MM-dd-yy_HH-mm-ss-fff"), ticks));
                CreateUI();
                //((IMyFunctionalBlock)cubeBlock).AppendingCustomInfo += appendCustomInfo;
                terminalBlock.RefreshCustomInfo(); //Check
                Initialized = false;
            }

            HeatDisipationLogic();
            ResidualHeatDisipationLogic();

            if (CurrentShootTime() != lastShootTime)
            {
                hitCenter = false;
                if (beamWeaponDefaultInfo.barrels > 1)
                {
                    DefineTargetingBeam();
                    DoTargetingBarelDamage();
                }


                for (int i = 0; i < BarrelList.Count ; i++)
                {

                    hitPoint = false;

                    beamWeaponBarrel = BarrelList.ElementAt(i);

                    DefineBeam(
                    beamWeaponBarrel.forwardOrigin,
                    beamWeaponBarrel.leftOffset,
                    beamWeaponBarrel.rightOffset,
                    beamWeaponBarrel.upOffset,
                    beamWeaponBarrel.downOffset
                    );

                    DoDamage();

                    if (hitPoint == false)
                    {
                        Beam Beam = new Beam(barrelTip, beamEndpoint);
                        beamList.Add(Beam);
                    }
                    else
                    {
                        Beam Beam = new Beam(barrelTip, beamTarget);
                        beamList.Add(Beam);
                    }

                }

                DoTurretDegridation();
                FireTimeingUpdate();
                HeatIncrimentationLogic();
                ResidualHeatIncrimentationLogic();
            }



            for (int i = 0; i < beamList.Count; i++)
            {
                BeamShow2(beamList.ElementAt(i));
            }
            beamList.Clear();

            if (Globals.Debug)
            {
                if (hitCenter == false)
                {
                    BeamShow(from, to);
                }
                else
                {
                    BeamShowTarget(from, toTarget);
                }
            }


            PowerLogic();
            terminalBlock.RefreshCustomInfo();

            ticks++;

        }

        #region Create UI

        void ReconfigureDefaultUI()
        {
            MyAPIGateway.TerminalControls.CustomControlGetter -= TerminalControls_CustomControlGetter;
            MyAPIGateway.TerminalControls.CustomActionGetter -= TerminalControls_CustomActionGetter;

            MyAPIGateway.TerminalControls.CustomControlGetter += TerminalControls_CustomControlGetter;
            MyAPIGateway.TerminalControls.CustomActionGetter += TerminalControls_CustomActionGetter;

        }

        static void TerminalControls_CustomActionGetter(Sandbox.ModAPI.IMyTerminalBlock block, List<IMyTerminalAction> actions)
        {
            if (block is IMyLargeTurretBase)
            {
                string subtype = (block as IMyLargeTurretBase).BlockDefinition.SubtypeId;
                var itemsToRemove = new List<IMyTerminalAction>();

                foreach (var action in actions)
                {
                    //Logger.Instance.LogMessage("Action: " + action.Id);
                    Logging.writeLine(String.Format("Action" + action.Id, DateTime.Now));

                    if (action.Id.StartsWith("OnOff"))
                            {

                                action.Enabled = b => b.GameLogic.GetAs<MissileTurret>()?.beamWeaponUiInfo?.OnOffConfig ?? true;

                                break;
                            }
                            if (action.Id.StartsWith("Shoot"))
                            {

                                action.Enabled = b => b.GameLogic.GetAs<MissileTurret>()?.beamWeaponUiInfo?.ShootConfig ?? true;
                                break;
                            }
                            if (action.Id.StartsWith("ShootOnce"))
                            {

                                action.Enabled = b => b.GameLogic.GetAs<MissileTurret>()?.beamWeaponUiInfo?.ShootOnceConfig ?? true;
                                break;
                            }
                            if (action.Id.StartsWith("EnableIdleMovement"))
                            {

                                action.Enabled = b => b.GameLogic.GetAs<MissileTurret>()?.beamWeaponUiInfo?.EnableIdleMovementConfig ?? true;
                                break;
                            }
                            if (action.Id.StartsWith("TargetMissiles"))
                            {
                                //MyAPIGateway.Utilities.SetVariable<bool>(block.EntityId.ToString() + "", GSFWeaponUiManager.LargeDualBeamGTF.TargetMeteorsValue);
                                action.Enabled = b => b.GameLogic.GetAs<MissileTurret>()?.beamWeaponUiInfo?.TargetMissilesConfig ?? true;
                                break;
                            }
                            if (action.Id.StartsWith("TargetMeteors"))
                            {

                                action.Enabled = b => b.GameLogic.GetAs<MissileTurret>()?.beamWeaponUiInfo?.TargetMeteorsConfig ?? true;
                                break;
                            }
                            if (action.Id.StartsWith("TargetSmallShips"))
                            {

                                action.Enabled = b => b.GameLogic.GetAs<MissileTurret>()?.beamWeaponUiInfo?.TargetSmallShipsConfig ?? true;
                                break;
                            }
                            if (action.Id.StartsWith("TargetLargeShips"))
                            {

                                action.Enabled = b => b.GameLogic.GetAs<MissileTurret>()?.beamWeaponUiInfo?.TargetLargeShipsConfig ?? true;
                                break;
                            }
                            if (action.Id.StartsWith("TargetStations"))
                            {

                                action.Enabled = b => b.GameLogic.GetAs<MissileTurret>()?.beamWeaponUiInfo?.TargetStationsConfig ?? true;
                                break;
                            }
                            if (action.Id.StartsWith("TargetCharacters"))
                            {

                                action.Enabled = b => b.GameLogic.GetAs<MissileTurret>()?.beamWeaponUiInfo?.TargetCharactersConfig ?? true;
                                break;
                            }
                            if (action.Id.StartsWith("TargetNeutrals"))
                            {

                                action.Enabled = b => b.GameLogic.GetAs<MissileTurret>()?.beamWeaponUiInfo?.TargetNeutralsConfig ?? true;
                                break;
                            }
                            if (action.Id.StartsWith("Control"))
                            {

                                action.Enabled = b => b.GameLogic.GetAs<MissileTurret>()?.beamWeaponUiInfo?.CanControlConfig ?? true;
                                break;
                            }

                            else itemsToRemove.Add(action);
                            break;
                   

                }

                foreach (var action in itemsToRemove)
                {
                    actions.Remove(action);
                }
            }
        }

        static void TerminalControls_CustomControlGetter(Sandbox.ModAPI.IMyTerminalBlock block, List<IMyTerminalControl> controls)
        {
            if (block is IMyLargeTurretBase)
            {
                string subtype = (block as IMyLargeTurretBase).BlockDefinition.SubtypeId;
                var itemsToRemove = new List<IMyTerminalControl>();
                int separatorsToKeep = 2;

                foreach (var control in controls)
                {
                    //Logger.Instance.LogMessage("Control: " + control.Id);
                    Logging.writeLine(String.Format("Control:" + control.Id, DateTime.Now));
                    switch (control.Id)
                            {

                                case "ShowInTerminal":
                                case "ShowInToolbarConfig":
                                case "ShowOnHUD":
                                case "Name":

                                    break;
                                default:
                                    if (control.Id.StartsWith("OnOff"))
                                    {
                                        control.Visible = b => b.GameLogic.GetAs<MissileTurret>()?.beamWeaponUiInfo?.OnOffControl ?? true;
                                        control.Enabled = b => b.GameLogic.GetAs<MissileTurret>()?.beamWeaponUiInfo?.OnOffValue ?? true;
                                        break;
                                    }
                                    if (control.Id.StartsWith("Shoot"))
                                    {
                                        control.Visible = b => b.GameLogic.GetAs<MissileTurret>()?.beamWeaponUiInfo?.ShootControl ?? true;
                                        control.Enabled = b => b.GameLogic.GetAs<MissileTurret>()?.beamWeaponUiInfo?.ShootValue ?? true;
                                        break;
                                    }
                                    if (control.Id.StartsWith("ShootOnce"))
                                    {
                                        control.Visible = b => b.GameLogic.GetAs<MissileTurret>()?.beamWeaponUiInfo?.ShootOnceControl ?? true;
                                        //control.Enabled = b => b.GameLogic.GetAs<MissileTurret>()?.beamWeaponUiInfo?.ShootOnceValue ?? true;
                                        break;
                                    }
                                    if (control.Id.StartsWith("EnableIdleMovement"))
                                    {
                                        control.Visible = b => b.GameLogic.GetAs<MissileTurret>()?.beamWeaponUiInfo?.EnableIdleMovementControl ?? true;
                                        control.Enabled = b => b.GameLogic.GetAs<MissileTurret>()?.beamWeaponUiInfo?.EnableIdleMovementValue ?? true;
                                        break;
                                    }
                                    if (control.Id.StartsWith("TargetMissiles"))
                                    {
                                        control.Visible = b => b.GameLogic.GetAs<MissileTurret>()?.beamWeaponUiInfo?.TargetMissilesControl ?? true;
                                        control.Enabled = b => b.GameLogic.GetAs<MissileTurret>()?.beamWeaponUiInfo?.TargetMissilesValue ?? true;

                                        break;
                                    }
                                    if (control.Id.StartsWith("TargetMeteors"))
                                    {
                                        control.Visible = b => b.GameLogic.GetAs<MissileTurret>()?.beamWeaponUiInfo?.TargetMeteorsControl ?? true;
                                        control.Enabled = b => b.GameLogic.GetAs<MissileTurret>()?.beamWeaponUiInfo?.TargetMeteorsValue ?? true;
                                        break;
                                    }
                                    if (control.Id.StartsWith("TargetSmallShips"))
                                    {
                                        control.Visible = b => b.GameLogic.GetAs<MissileTurret>()?.beamWeaponUiInfo?.TargetSmallShipsControl ?? true;
                                        control.Enabled = b => b.GameLogic.GetAs<MissileTurret>()?.beamWeaponUiInfo?.TargetSmallShipsValue ?? true;
                                        break;
                                    }
                                    if (control.Id.StartsWith("TargetLargeShips"))
                                    {
                                        control.Visible = b => b.GameLogic.GetAs<MissileTurret>()?.beamWeaponUiInfo?.TargetLargeShipsControl ?? true;
                                        control.Enabled = b => b.GameLogic.GetAs<MissileTurret>()?.beamWeaponUiInfo?.TargetLargeShipsValue ?? true;
                                        break;
                                    }
                                    if (control.Id.StartsWith("TargetStations"))
                                    {
                                        control.Visible = b => b.GameLogic.GetAs<MissileTurret>()?.beamWeaponUiInfo?.TargetStationsControl ?? true;
                                        control.Enabled = b => b.GameLogic.GetAs<MissileTurret>()?.beamWeaponUiInfo?.TargetStationsValue ?? true;
                                        break;
                                    }
                                    if (control.Id.StartsWith("TargetCharacters"))
                                    {
                                        control.Visible = b => b.GameLogic.GetAs<MissileTurret>()?.beamWeaponUiInfo?.TargetCharactersControl ?? true;
                                        control.Enabled = b => b.GameLogic.GetAs<MissileTurret>()?.beamWeaponUiInfo?.TargetCharactersValue ?? true;
                                        break;
                                    }
                                    if (control.Id.StartsWith("TargetNeutrals"))
                                    {
                                        control.Visible = b => b.GameLogic.GetAs<MissileTurret>()?.beamWeaponUiInfo?.TargetNeutralsControl ?? true;
                                        control.Enabled = b => b.GameLogic.GetAs<MissileTurret>()?.beamWeaponUiInfo?.TargetNeutralsValue ?? true;
                                        break;
                                    }

                                    if (control.Id.StartsWith("Control"))
                                    {
                                        control.Visible = b => b.GameLogic.GetAs<MissileTurret>()?.beamWeaponUiInfo?.CanControlControl ?? true;
                                        break;
                                    }

                                    if (control.Id.StartsWith("OveridePower"))
                                        break;
                                    if (control.Id.StartsWith("RangeSlider"))
                                        break;
                                    if (control.Id.StartsWith("CurrentSlider"))
                                        break;
                                    if (control.Id.StartsWith("ModulationSlider"))
                                        break;
                                    if (control.Id.StartsWith("PowerSlider"))
                                        break;
                                    if (control.Id.StartsWith("PrimaryColor"))
                                        break;
                                    if (control.Id.StartsWith("SecondaryColor"))
                                        break;

                                    if (control is IMyTerminalControlSeparator && separatorsToKeep-- > 0)
                                        break;
                                    itemsToRemove.Add(control);
                                    break;
                            
                            
                    }

                }

                foreach (var action in itemsToRemove)
                {
                    controls.Remove(action);
                }
            }
        }

        public void CreateUI()
        {
            ReconfigureDefaultUI();
           

            _PowerOveride = new Checkbox<Sandbox.ModAPI.Ingame.IMyLargeTurretBase>((IMyTerminalBlock)
                cubeBlock,
                Toggle.internalName,
                Toggle.title,
                Toggle.defaultValue);

            _PowerSlider = new Slider<Sandbox.ModAPI.Ingame.IMyLargeTurretBase>((IMyFunctionalBlock)
                cubeBlock,
                powerSlider.internalName,
                powerSlider.title,
                powerSlider.min,
                powerSlider.max,
                powerSlider.standard);

            _RangeSlider = new Slider<Sandbox.ModAPI.Ingame.IMyLargeTurretBase>((IMyFunctionalBlock)
                cubeBlock,
                rangeSlider.internalName,
                rangeSlider.title,
                rangeSlider.min,
                rangeSlider.max,
                rangeSlider.standard);

            _CurrentSlider = new Slider<Sandbox.ModAPI.Ingame.IMyLargeTurretBase>((IMyFunctionalBlock)
                cubeBlock,
                currentSlider.internalName,
                currentSlider.title,
                currentSlider.min,
                currentSlider.max,
                currentSlider.standard);

            _ModulationSlider = new Slider<Sandbox.ModAPI.Ingame.IMyLargeTurretBase>((IMyFunctionalBlock)
                cubeBlock,
                modulationSlider.internalName,
                modulationSlider.title,
                modulationSlider.min,
                modulationSlider.max,
                modulationSlider.standard);

            _PrimaryBeamColor = new ColorSlider<Sandbox.ModAPI.Ingame.IMyLargeTurretBase>((IMyFunctionalBlock)
                cubeBlock,
                "PrimaryColor",
                "Damage",
                beamWeaponColorInfo.color);

            _SecondaryBeamColor = new ColorSlider<Sandbox.ModAPI.Ingame.IMyLargeTurretBase>((IMyFunctionalBlock)
                cubeBlock,
                "SecondaryColor",
                "Shield",
                beamWeaponColorInfo.auxcolor);
        }
        #endregion

        public override void MarkForClose()
        {
            if (m_inventory != null)
            {

                for (int i = 0; i < beamWeaponDefaultInfo.classes; i++)
                {
                    m_inventory.RemoveItemsOfType(m_inventory.GetItemAmount(chargeDefinitionIds[i]), chargeObjectBuilders[i]);
                }
            }

            
            base.MarkForClose();
            
        }

    }

    public class Checkbox<T> : GSFWeapon.Control.Checkbox<T>
    {
        public Checkbox(IMyTerminalBlock block,
            string internalName,
            string title,
            bool defaultValue = true)
            : base(block, internalName, title, defaultValue)
        {
            //CreateUI(); //Check
        }
        public override void Setter(IMyTerminalBlock block, bool newState)
        {
            base.Setter(block, newState);


        }
    }

    public class Slider<T> : GSFWeapon.Control.Slider<T>
    {

        public Slider(
            IMyTerminalBlock block,
            string internalName,
            string title,
            float min = 1.0f,
            float max = 300.0f,
            float standard = 10.0f)
            : base(block, internalName, title, min, max, standard)
        {
        }

        public override void Writer(IMyTerminalBlock block, StringBuilder builder)
        {
            try
            {
                builder.Clear();
                var distanceString = Getter(block).ToString("0") + "%";
                builder.Append(distanceString);
                block.RefreshCustomInfo();
            }
            catch (Exception ex)
            {
                Logging.writeLine(String.Format("{0} - Exception in Writer", DateTime.Now));
                Logging.writeLine(String.Format("{0} - {1}", DateTime.Now, ex));
            }
        }

        public void SetterOutside(IMyTerminalBlock block, float value)
        {
            base.Setter(block, value);

        }

        public override void Setter(IMyTerminalBlock block, float value)
        {
            base.Setter(block, value);

        }
    }

    public class ColorSlider<T> : GSFWeapon.Control.ColorSlider<T>
    {

        public ColorSlider(
            IMyTerminalBlock block,
            string internalName,
            string title,
            Color min)
            : base(block, internalName, title, min)
        {
        }

        public void SetterOutside(IMyTerminalBlock block, Color value)
        {
            base.Setter(block, value);

        }

        public override void Setter(IMyTerminalBlock block, Color value)
        {
            base.Setter(block, value);
        }
    }

    public class Beam
    {
        public Vector3D Origin;
        public Vector3D End;

        public Beam(Vector3D origin, Vector3D end)
        {
            Origin = origin;
            End = end;
        }
    }


}


