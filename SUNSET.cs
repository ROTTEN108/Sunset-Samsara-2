using HutongGames.PlayMaker.Actions;
using Modding;
using System;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;
using Vasi;
using System.Collections.Generic;
using HKMirror.Reflection;
using System.Collections;
using System.IO;
using System.Reflection;
using WavLib;
using TMPro;

namespace SUNSET
{
    public class Settings
    {
        public bool on = true;
    }
    public class SUNSET : Mod, IMod, Modding.ILogger, IGlobalSettings<Settings>, IMenuMod
    {
        public override string GetVersion()
        {
            return "0.0.2.0";
        }
        public static Settings settings_ = new Settings();
        public static Settings settings_Pt_ = new Settings();
        public bool ToggleButtonInsideMenu => true;
        public List<IMenuMod.MenuEntry> GetMenuData(IMenuMod.MenuEntry? toggleButtonEntry)
        {
            List<IMenuMod.MenuEntry> menus = new List<IMenuMod.MenuEntry>();
            if (toggleButtonEntry != null)
            {
                menus.Add(toggleButtonEntry.Value);
            }
            menus.Add(new IMenuMod.MenuEntry
            {
                Name = this.OtherLanguage("召唤残阳", "Summon Sunset"),
                Description = this.OtherLanguage("古老的太阳以光之神形体逼近现世。再次进入世界生效。", "The ancient sun approached this world in the form of the God of light. Re-enter the world effective."),
                Values = new string[]
                {
                    Language.Language.Get("MOH_ON", "MainMenu"),
                    Language.Language.Get("MOH_OFF", "MainMenu")
                },
                Loader = (() => (!settings_.on) ? 1 : 0),
                Saver = delegate (int i)
                {
                    settings_.on = (i == 0);
                }
            });
            menus.Add(new IMenuMod.MenuEntry
            {
                Name = this.OtherLanguage("去除部分粒子效果", "Removes some particle effects"),
                Description = this.OtherLanguage("降低演出效果，提高战斗体验", "Reduce performance and improve combat experience"),
                Values = new string[]
                {
                    Language.Language.Get("MOH_OFF", "MainMenu"),
                    Language.Language.Get("MOH_ON", "MainMenu")
                },
                Loader = (() => (!settings_Pt_.on) ? 1 : 0),
                Saver = delegate (int i)
                {
                    settings_Pt_.on = (i == 0);
                }
            });

            return menus;
        }
        public void OnLoadGlobal(Settings settings) => settings_ = settings;

        public Settings OnSaveGlobal() => settings_;
        private string OtherLanguage(string chinese, string english)
        {
            if (Language.Language.CurrentLanguage() == Language.LanguageCode.ZH)
            {
                return chinese;
            }
            return english;
        }
        static System.Random random = new System.Random();
        public static double RadiansToDegrees(double radians)
        {
            return radians * (180 / Math.PI);
        }
        public static double DegreesToRadians(double Degrees)
        {
            return Degrees * (Math.PI / 180);
        }
        static double RX0 => random.NextDouble();
        static double RY0 => random.NextDouble();
        static double RZ0 => random.NextDouble();
        public static float SIGHX => (random.Next(0, 2) * 2 - 1);
        public static float SIGHY => (random.Next(0, 2) * 2 - 1);
        public static float SIGHZ => (random.Next(0, 2) * 2 - 1);
        public static float RX => (float)(RX0 * SIGHX);
        public static float RY => (float)(RY0 * SIGHY);
        public static float RZ => (float)(RZ0 * SIGHZ);
        public float CHOICE => random.Next(1, 6);
        public static int HardMode = 0;
        public static int HaloConut = 1;
        public static int P4Hp = 1020 + GameManager.instance.playerData.nailDamage * 20;
        public static float HaloScaleFactor = 1f;
        public static GameObject BOSS;
        public static GameObject BEAM;
        public static GameObject NAIL;
        public static GameObject ORB;
        public static GameObject BLASTPT;
        public static GameObject DREAMPT;
        public static GameObject DREAMPTCHARG;
        public static GameObject ChargePt;
        public static GameObject DREAMPTCHARG2;
        public static GameObject DREAMPTBLOCK;
        public static GameObject HALO;
        public static GameObject HALO_prefab;
        public static GameObject ANOTHERHALO_prefab;
        public static GameObject HALO1;
        public static GameObject HALO2;
        public static GameObject HALO3;

        public static GameObject DREAMVER;
        public static GameObject STATUS;
        public static GameObject Camera;
        public static GameObject BLACKKNIGHT_Prefab;
        public static GameObject GATE_Prefab;
        public static GameObject GLOW_Prefab;
        public static GameObject WAVE_Prefab;
        public static GameObject GLOW;
        public static GameObject WAVE;
        public static GameObject WAVE1;
        public static GameObject PLAT_Prefab;
        public static GameObject HALO_WORKSHOP;
        public static GameObject HALO_WORKSHOP0;

        public static AudioClip NAILSHOT;
        public static AudioClip NAILCHARGE;
        public static AudioClip ORBIMPACT;
        public static AudioClip BeamCannon;
        public static AudioClip BeamCannon_H;
        public static AudioClip Tik;
        public static AudioClip Metal;
        public static AudioClip Metal2;
        public static AudioClip ShieldBreak;
        public static AudioClip BGM1;
        public static AudioClip BGM2;
        public static bool ZH;
        public static bool DREAMSWITCH = false;
        public static bool DREAMcheck = false;
        public static bool ShieldOn = false;
        public static bool ShieldNeedRecharge = true;
        public static bool P1Start = false;
        public override List<(string, string)> GetPreloadNames()
        {
            return new List<(string, string)>
            {
                ("GG_Workshop","GG_Statue_Radiance/Spotlight/Glow Response statue_beam/light_beam_particles 3"),
                ("GG_Workshop","gg_radiant_gate (27)"),
                ("GG_Radiance","Boss Control"),
                ("GG_Radiance","Boss Control/Plat Sets/Ascend Set/Radiant Plat Small (10)"),
                ("Dream_Final_Boss","Boss Control"),
                ("GG_Soul_Master","Mage Lord"),
                ("GG_Watcher_Knights","Battle Control/Black Knight 1")
            };
        }
        public override void Initialize(Dictionary<string, Dictionary<string, GameObject>> preloadedObjects)
        {
            var radiance = preloadedObjects["GG_Radiance"]["Boss Control"].transform.Find("Absolute Radiance").gameObject;
            var burst = radiance.transform.Find("Eye Beam Glow").gameObject.transform.Find("Burst 1").gameObject;
            BEAM = burst.transform.Find("Radiant Beam").gameObject;
            DREAMPT = radiance.transform.Find("Eye Final Pt").gameObject;
            HALO_prefab = radiance.transform.Find("Halo").gameObject;
            var radiance_1 = preloadedObjects["Dream_Final_Boss"]["Boss Control"].transform.Find("Radiance").gameObject;
            ANOTHERHALO_prefab = radiance_1.transform.Find("Halo").gameObject;


            Texture2D halo00 = LoadPng("Sunset.Resources.Halo0.png");
            HALO_prefab.GetComponent<SpriteRenderer>().sprite = Sprite.Create(halo00, new Rect(0f, 0f, (float)halo00.width, (float)halo00.height), new Vector2(0.5f, 0.5f), HALO_prefab.GetComponent<SpriteRenderer>().sprite.pixelsPerUnit);
            
            Texture2D halo11 = LoadPng("Sunset.Resources.Halo1.png");
            ANOTHERHALO_prefab.GetComponent<SpriteRenderer>().sprite = Sprite.Create(halo11, new Rect(0f, 0f, (float)halo11.width, (float)halo11.height), new Vector2(0.5f, 0.5f), ANOTHERHALO_prefab.GetComponent<SpriteRenderer>().sprite.pixelsPerUnit);


            GATE_Prefab = preloadedObjects["GG_Workshop"]["gg_radiant_gate (27)"].gameObject;
            var ebg = radiance.transform.Find("Eye Beam Glow").gameObject;
            GLOW_Prefab = ebg.transform.Find("Sprite").gameObject;
            WAVE_Prefab = radiance.transform.Find("Roar Wave Stun").gameObject;

            var MageLord = preloadedObjects["GG_Soul_Master"]["Mage Lord"].gameObject;
            var audioclip1 = MageLord.LocateMyFSM("Mage Lord").GetState("HS Ret Left").GetAction<AudioPlaySimple>().oneShotClip.Value as AudioClip;
            var audioclip2 = MageLord.LocateMyFSM("Mage Lord").GetState("Teleport").GetAction<AudioPlayerOneShotSingle>().audioClip.Value as AudioClip;
            NAILSHOT = audioclip1;
            NAILCHARGE = audioclip2;

            BLACKKNIGHT_Prefab = preloadedObjects["GG_Watcher_Knights"]["Battle Control/Black Knight 1"].gameObject;
            PLAT_Prefab = preloadedObjects["GG_Radiance"]["Boss Control/Plat Sets/Ascend Set/Radiant Plat Small (10)"].gameObject;

            var Pt1 = radiance.transform.Find("Pt Tele Out").gameObject;
            Pt1.gameObject.GetComponent<Transform>().localScale = new Vector3(6f, 6f, 1.3954f);
            Pt1.gameObject.GetComponent<ParticleSystem>().emissionRate = 10;
            Pt1.gameObject.GetComponent<ParticleSystem>().maxParticles = 9999;
            Pt1.gameObject.GetComponent<ParticleSystem>().startLifetime = 1f;
            Pt1.gameObject.GetComponent<ParticleSystem>().startSize = 3f;
            Pt1.gameObject.GetComponent<ParticleSystem>().startSpeed = 0f;
            BLASTPT = Pt1;

            var Pt2 = radiance.transform.Find("Shot Charge").gameObject;
            Pt2.transform.GetComponent<ParticleSystem>().emissionRate = 200f;
            Pt2.transform.GetComponent<ParticleSystem>().maxParticles = 9999;
            Pt2.transform.GetComponent<ParticleSystem>().startSpeed = -24f;
            Pt2.transform.GetComponent<ParticleSystem>().startColor = new Color(1f, 0.9f, 0.73f, 1f);
            Pt2.transform.GetComponent<ParticleSystem>().startSize = 1.5f;
            //Pt2.transform.GetComponent<ParticleSystem>().startLifetime = 1f;
            Pt2.transform.GetComponent<Behaviour>().enabled = true;
            Pt2.transform.localPosition = new Vector3(0, 0, 0);
            Pt2.transform.localScale = new Vector3(3f, 3f, Pt2.transform.localScale.z);
            DREAMPTCHARG = Pt2;

            var Pt3 = radiance.gameObject.transform.Find("Eye Final Pt").gameObject;
            //Pt3.transform.GetComponent<ParticleSystem>().enableEmission = false;
            Pt3.transform.GetComponent<ParticleSystem>().emissionRate = 60f;
            Pt3.transform.GetComponent<ParticleSystem>().startLifetime = 1f;
            //Pt3.transform.GetComponent<ParticleSystem>().startColor = new Color(1, 1, 1, 1);
            Pt3.transform.GetComponent<ParticleSystem>().startSpeed = 60f;
            Pt3.transform.GetComponent<Transform>().localPosition = new Vector3(0, 0, 0);
            Pt3.transform.GetComponent<Transform>().localScale = new Vector3(0.2f, 0.3f, 1f);
            DREAMPTBLOCK = Pt3;

            BeamCannon = LoadAudioClip("Sunset.Resources.BeamCannon.wav");
            BeamCannon_H = LoadAudioClip("Sunset.Resources.BeamCannon_H.wav");
            Tik = LoadAudioClip("Sunset.Resources.tik.wav");
            Metal = LoadAudioClip("Sunset.Resources.metal.wav");
            Metal2 = LoadAudioClip("Sunset.Resources.metal_2.wav");
            ShieldBreak = LoadAudioClip("Sunset.Resources.ShieldBreak.wav");
            BGM1 = LoadAudioClip("Sunset.Resources.bgm1.wav");
            BGM2 = LoadAudioClip("Sunset.Resources.bgm2.wav");

            On.GameManager.LoadGame += GameManager_LoadGame;
        }

        private void GameManager_LoadGame(On.GameManager.orig_LoadGame orig, GameManager self, int saveSlot, Action<bool> callback)
        {
            if(settings_.on)
            {
                ModHooks.LanguageGetHook += ModHooks_LanguageGetHook;
                ModHooks.HeroUpdateHook += ModHooks_HeroUpdateHook;
                On.PlayMakerFSM.Start += PlayMakerFSM_Start;
                On.HealthManager.TakeDamage += HealthManager_TakeDamage;
                On.BossStatueDreamToggle.OnTriggerEnter2D += BossStatueDreamToggle_OnTriggerEnter2D;
            }
            else
            {
                if(HALO_WORKSHOP != null)
                {
                    HALO_WORKSHOP.Recycle();
                }
                if(HALO_WORKSHOP0 != null)
                {
                    HALO_WORKSHOP0.Recycle();
                }
            }
            orig(self, saveSlot, callback);
        }
        private void HealthManager_TakeDamage(On.HealthManager.orig_TakeDamage orig, HealthManager self, HitInstance hitInstance)
        {
            orig(self, hitInstance);
            BOSS.GetComponent<BossShieldControl>().HitShield();
        }

        public string ModHooks_LanguageGetHook(string key, string sheet, string text)
        {
            if (settings_.on == true)
            {
                if (HardMode == 0)
                {
                    if (key == "NAME_FINAL_BOSS")
                    {
                        if (Language.Language.CurrentLanguage() == Language.LanguageCode.ZH)
                        {
                            text = "残阳";
                        }
                        else
                        {
                            text = "SUNSET";
                        }
                    }
                    if (key == "ABSOLUTE_RADIANCE_MAIN" && sheet == "Titles")
                    {
                        if (Language.Language.CurrentLanguage() == Language.LanguageCode.ZH)
                        {
                            text = "残阳";
                        }
                        else
                        {
                            text = "SUNSET";
                        }
                    }
                    if (key == "ABSOLUTE_RADIANCE_SUPER" && sheet == "Titles")
                    {
                        if (Language.Language.CurrentLanguage() == Language.LanguageCode.ZH)
                        {
                            text = "现世将尽";
                        }
                        else
                        {
                            text = "DOOMSDAY";
                        }
                    }
                    if (key == "GG_S_RADIANCE" && sheet == "CP3")
                    {
                        if (Language.Language.CurrentLanguage() == Language.LanguageCode.ZH)
                        {
                            text = "旧时代湮灭的余烬";
                        }
                        else
                        {
                            text = "Ember of former era";
                        }
                    }
                    return text;
                }
                else
                {

                    if (key == "NAME_FINAL_BOSS")
                    {
                        if (Language.Language.CurrentLanguage() == Language.LanguageCode.ZH)
                        {
                            text = "旧日·残阳";
                        }
                        else
                        {
                            text = "FORMER SUN";
                        }
                    }
                    if (key == "ABSOLUTE_RADIANCE_MAIN" && sheet == "Titles")
                    {
                        if (Language.Language.CurrentLanguage() == Language.LanguageCode.ZH)
                        {
                            text = "旧日·残阳";
                        }
                        else
                        {
                            text = "FORMER SUN";
                        }
                    }
                    if (key == "ABSOLUTE_RADIANCE_SUPER" && sheet == "Titles")
                    {
                        if (Language.Language.CurrentLanguage() == Language.LanguageCode.ZH)
                        {
                            text = "万古伊始";
                        }
                        else
                        {
                            text = "BEGINNING OF ETERNITY";
                        }
                    }
                    if (key == "GG_S_RADIANCE" && sheet == "CP3")
                    {
                        if (Language.Language.CurrentLanguage() == Language.LanguageCode.ZH)
                        {
                            text = "铸造与毁灭旧时代之神";
                        }
                        else
                        {
                            text = "The god who forged and destroyed the former era";
                        }
                    }
                    return text;
                }
            }
            else
                return @text;
        }
        public void BossStatueDreamToggle_OnTriggerEnter2D(On.BossStatueDreamToggle.orig_OnTriggerEnter2D orig, BossStatueDreamToggle self, Collider2D collision)
        {
            var this_ = self.Reflect();
            if (!self.gameObject.activeInHierarchy || !this_.canToggle)
            {
                return;
            }
            if (this_.bossStatue && collision.tag == "Dream Attack")
            {
                bool flag;
                if (this_.bossStatue.name == "GG_Statue_Radiance")
                {
                    flag = !DREAMSWITCH;
                    DREAMSWITCH = !DREAMSWITCH;

                    Camera.GetComponent<SceneSwitchDetector>().SwitchDetectAndChange();

                    if (this_.bossStatue.GetComponent<BossStatue>().UsingDreamVersion == true)
                    {
                        this_.bossStatue.SetDreamVersion(false, false, true);
                    }
                    if(DREAMSWITCH)
                    {
                        HALO_WORKSHOP.GetComponent<HaloColorChange_SmallWorkshop>().defaultColora = 1f;
                        HALO_WORKSHOP.GetComponent<HaloRotateChange_SmallWorkshop>().defaultSpeed = -1f;
                    }
                    else
                    {
                        HALO_WORKSHOP.GetComponent<HaloColorChange_SmallWorkshop>().defaultColora = 0.5f;
                        HALO_WORKSHOP.GetComponent<HaloRotateChange_SmallWorkshop>().defaultSpeed = -0.5f;
                    }
                    HALO_WORKSHOP.GetComponent<HaloRotateChange_SmallWorkshop>().Jump();
                }
                else
                {
                    flag = !this_.bossStatue.UsingDreamVersion;
                    this_.bossStatue.SetDreamVersion(flag, false, true);
                }
                if (this_.dreamImpactPoint && this_.dreamImpactPrefab)
                {
                    this_.dreamImpactPrefab.Spawn(this_.dreamImpactPoint.position).transform.localScale = this_.dreamImpactScale;
                }
                if (this_.dreamBurstEffect)
                {
                    this_.dreamBurstEffect.SetActive(flag);
                }
                if (this_.dreamBurstEffectOff)
                {
                    this_.dreamBurstEffectOff.SetActive(!flag);
                }
                self.StartCoroutine(this_.Fade(flag));
            }
        }
        public static void ModHooks_HeroUpdateHook()
        {
            if(settings_.on)
            {
                GameObject GC = GameCameras.instance.gameObject;
                GameObject CP = GC.transform.Find("CameraParent").gameObject;
                Camera = CP.transform.Find("tk2dCamera").gameObject;
                if (Camera != null)
                {
                    if (Camera.GetComponent<CameraControl>() == null)
                    {
                        Camera.AddComponent<SceneSwitchDetector>();
                        Camera.AddComponent<ChangeColor>();
                        Camera.AddComponent<CameraControl>();
                        Camera.AddComponent<Dreamcheck>();
                        Camera.AddComponent<Dreamcheck2>();
                        if (Camera.GetComponent<SceneSwitchDetector>() == null)
                        {
                            Camera.AddComponent<SceneSwitchDetector>();
                        }
                        ModHooks.HeroUpdateHook -= ModHooks_HeroUpdateHook;
                    }
                }
            }
        }
        private void PlayMakerFSM_Start(On.PlayMakerFSM.orig_Start orig, PlayMakerFSM self)
        {
            orig(self);
            if (self.gameObject.name == "Halo" && self.FsmName == "Rotation")
            {
                self.enabled = false;
            }
            if (self.gameObject.name == "Absolute Radiance" && self.FsmName == "Control")
            {
                ShieldOn = false;
                ShieldNeedRecharge = true;
                var bc = self.transform.parent;
                bc.transform.Find("Spike Control").gameObject.Recycle();
                self.transform.Find("Shot Charge").gameObject.Recycle();
                self.transform.Find("Shot Charge 2").gameObject.Recycle();

                self.GetState("Set Arena 1").AddMethod(() =>
                {
                    GameObject[] allGameObjects = GameObject.FindObjectsOfType<GameObject>();
                    foreach (GameObject go in allGameObjects)
                    {
                        if (go.name.Contains("SceneBorder(Clone)"))
                        {
                            go.Recycle();
                        }
                    }
                });

                var RG = self.CopyState("Rage Comb", "RG");
                RG.RemoveAction<SpawnObjectFromGlobalPool>();
                self.ChangeTransition("Rage1 Start", "FINISHED", "RG");
                self.ChangeTransition("Rage1 Loop", "FINISHED", "RG");
                self.GetState("Rage1 Start").AddMethod(() =>
                {
                    if (HardMode == 0)
                    {
                        self.GetComponent<BeamSkillRage>().On(0.2f, 1.5f);
                    }
                    else
                    {
                        self.GetComponent<BeamSkillRage>().On(0.125f, 1.5f);
                    }
                    self.GetComponent<BossShieldControl>().ReChargAnim();
                });
                self.GetState("Arena 1 Start").AddMethod(() =>
                {
                    self.GetComponent<BossShieldControl>().ReChargAnim();
                });
                //self.GetState("Arena 1 Start").GetAction<SendEventByName>().delay = 2f;

                self.ChangeTransition("Tele Cast?", "FINISHED", "A2 Cast Antic");

                self.GetState("Stun1 Start").AddMethod(() =>
                {
                    Camera.gameObject.GetComponent<ChangeColor>().Change2();
                    self.GetComponent<BeamSkillRage>().Off();
                    self.GetComponent<CombL>().p2 = true;
                    self.GetComponent<CombR>().p2 = true;
                    self.GetComponent<BeamTop>().p2 = true;
                    self.GetComponent<BeamSkill6>().p2 = true;
                    HALO.GetComponent<HaloColorChange>().Disappear();
                });
                self.GetState("Stun1 Out").AddMethod(() =>
                {
                    self.GetComponent<BeamTop>().On(0.25f, 5.1f, 1.5f);
                });
                self.GetState("Arena 1 Start").AddMethod(() =>
                {
                    HALO.GetComponent<HaloColorChange>().Appear();
                });
                self.GetState("Ascend Tele").AddMethod(() =>
                {
                    Camera.gameObject.GetComponent<ChangeColor>().Change3();
                    if (HardMode == 0)
                    {
                        self.GetComponent<OrbSkillP3>().On(0.075f, 99999f);
                    }
                    else
                    {
                        self.GetComponent<NailSkillRage>().On(0.1f, 0.3f);
                    }
                });
                self.GetState("Scream").AddMethod(() =>
                {
                    if (HardMode == 0)
                    {
                        self.GetComponent<FinalPlatsSet>().Set();
                        self.GetComponent<FinalPlatsSet>().DelayAcitve();
                        self.GetComponent<OrbSkillP3>().SummonLoopEnd();
                    }
                    else
                    {
                        self.GetComponent<FinalPlatsSet>().Set();
                        self.GetComponent<FinalPlatsSet>().DelayAcitve();
                        self.GetComponent<NailSkillRage>().SummonEnd();
                    }
                });

                self.GetState("Set Dest").AddMethod(() =>
                {
                    if (ShieldNeedRecharge)
                    {
                        self.SendEvent("CAST");
                    }
                });
                self.GetState("Tele Cast?").AddMethod(() =>
                {
                    if (ShieldNeedRecharge)
                    {
                        self.SendEvent("CAST");
                    }
                });
                self.GetState("Tele SFX").AddMethod(() =>
                {
                    if (ShieldNeedRecharge)
                    {
                        self.SendEvent("CAST");
                    }
                });
                self.GetState("Final Impact").AddMethod(() =>
                {
                    GameObject[] allGameObjects = GameObject.FindObjectsOfType<GameObject>();
                    foreach (GameObject go in allGameObjects)
                    {
                        if (go.name.Contains("Orb") || go.name.Contains("Nail") || go.name.Contains("Beam"))
                        {
                            go.LocateMyFSM("Orb Control").SetState("Recycle");
                        }
                    }
                });
                self.gameObject.AddComponent<Audios>();
                self.gameObject.AddComponent<HeroAngle>();
                self.gameObject.AddComponent<BossDash>();
                self.gameObject.AddComponent<SkillsWaitTimeChange>();
                self.gameObject.AddComponent<HaloControl>();
                self.gameObject.AddComponent<HaloControl_Small>();
                self.gameObject.AddComponent<OrbBlast>();
                self.gameObject.AddComponent<OrbImpact>();
                self.gameObject.AddComponent<OrbSummonLoop>();
                self.gameObject.AddComponent<SolarFlareSkill>();
                self.gameObject.AddComponent<BeamSight_1>();
                self.gameObject.AddComponent<BossShieldControl>();
                self.gameObject.AddComponent<FinalPlatsSet>();

                self.gameObject.AddComponent<BeamSkill1>();
                self.gameObject.AddComponent<BeamSkill2>();
                self.gameObject.AddComponent<BeamSkill3>();
                self.gameObject.AddComponent<BeamSkill4>();
                self.gameObject.AddComponent<BeamSkill5>();
                self.gameObject.AddComponent<BeamSkill6>();
                self.gameObject.AddComponent<BeamSkillP4>();

                self.gameObject.AddComponent<OrbSkill1>();
                self.gameObject.AddComponent<OrbSkill2>();
                self.gameObject.AddComponent<OrbSkill3>();
                self.gameObject.AddComponent<OrbSkill4>();
                self.gameObject.AddComponent<OrbSkillP3>();

                self.gameObject.AddComponent<NailSkill1>();
                self.gameObject.AddComponent<NailSkill2>();
                self.gameObject.AddComponent<NailSkill3>();
                self.gameObject.AddComponent<NailSkill4>();
                self.gameObject.AddComponent<NailSkill5>();
                self.gameObject.AddComponent<NailSkill6>();
                self.gameObject.AddComponent<NailSkill6_H>();

                self.gameObject.AddComponent<CombL>();
                self.gameObject.AddComponent<CombR>();
                self.gameObject.AddComponent<BeamTop>();
                self.gameObject.AddComponent<BeamSkillRage>();
                self.gameObject.AddComponent<NailSkillRage>();

                self.gameObject.AddComponent<BossSkillChoice>();



                self.GetState("Scream").GetAction<SetHP>().hp = P4Hp;
                self.GetState("Scream").AddMethod(() =>
                {
                    BOSS.GetComponent<BossShieldControl>().nextHp = 0;
                });

                self.GetComponent<HealthManager>().hp = 4000;
                self.gameObject.LocateMyFSM("Phase Control").GetState("Check 1").GetAction<IntCompare>().integer2.RawValue = 2500;
                self.gameObject.LocateMyFSM("Phase Control").GetState("Check 2").GetAction<IntCompare>().integer2.RawValue = 2500;
                self.gameObject.LocateMyFSM("Phase Control").GetState("Check 3").GetAction<IntCompare>().integer2.RawValue = 2000;
                self.gameObject.LocateMyFSM("Phase Control").GetState("Check 4").GetAction<IntCompare>().integer2.RawValue = 500;

                self.GetState("Final Antic").AddMethod(() =>
                {
                    self.GetComponent<BossShieldControl>().finalStart = true;
                    if (ShieldOn)
                    {
                        if (HardMode == 0)
                        {
                            double num = RX0;
                            if (num <= 0.33333d)
                            {
                                self.GetState("Final Idle").GetAction<WaitRandom>().timeMax = 2.5f;
                                self.GetState("Final Idle").GetAction<WaitRandom>().timeMin = 2.5f;
                                if (self.GetComponent<BeamSkillP4>() == null)
                                {
                                    self.gameObject.AddComponent<BeamSkillP4>();
                                }
                                self.GetComponent<BeamSkillP4>().On(0.25f, 1.8f, 0.8f, false);
                            }
                            else if (num <= 0.66667d)
                            {
                                self.GetState("Final Idle").GetAction<WaitRandom>().timeMax = 2.5f;
                                self.GetState("Final Idle").GetAction<WaitRandom>().timeMin = 2.5f;
                                if (self.GetComponent<OrbSkillP4>() == null)
                                {
                                    self.gameObject.AddComponent<OrbSkillP4>();
                                }
                                self.GetComponent<OrbSkillP4>().On(3, 0.3f);
                            }
                            else
                            {
                                self.GetState("Final Idle").GetAction<WaitRandom>().timeMax = 2.5f;
                                self.GetState("Final Idle").GetAction<WaitRandom>().timeMin = 2.5f;
                                self.GetComponent<NailSkill4>().On(0.15f, 2.2f);
                            }
                        }
                        else
                        {
                            double num = RX0;
                            if (num <= 0.33333d)
                            {
                                self.GetState("Final Idle").GetAction<WaitRandom>().timeMax = 2.5f;
                                self.GetState("Final Idle").GetAction<WaitRandom>().timeMin = 2.5f;
                                if (self.GetComponent<BeamSkillP4>() == null)
                                {
                                    self.gameObject.AddComponent<BeamSkillP4>();
                                }
                                self.GetComponent<BeamSkillP4>().On(0.2f, 2.2f, 0.8f, false);
                            }
                            else if (num <= 0.66667d)
                            {
                                self.GetState("Final Idle").GetAction<WaitRandom>().timeMax = 2.5f;
                                self.GetState("Final Idle").GetAction<WaitRandom>().timeMin = 2.5f;
                                if (self.GetComponent<OrbSkillP4>() == null)
                                {
                                    self.gameObject.AddComponent<OrbSkillP4>();
                                }
                                self.GetComponent<OrbSkillP4>().On(5, 0.3f);
                            }
                            else
                            {
                                self.GetState("Final Idle").GetAction<WaitRandom>().timeMax = 2.5f;
                                self.GetState("Final Idle").GetAction<WaitRandom>().timeMin = 2.5f;
                                self.GetComponent<NailSkill4>().On(0.1f, 2.2f);
                            }
                        }
                    }
                });

                self.GetState("A1 Tele").AddMethod(() =>
                {
                    self.GetComponent<HaloControl>().Fade();
                    self.GetComponent<HaloControl_Small>().Fade();
                });
                self.GetState("A1 Tele 2").AddMethod(() =>
                {
                    self.GetComponent<HaloControl>().Fade();
                    self.GetComponent<HaloControl_Small>().Fade();
                });
                self.GetState("Tele 1").AddMethod(() =>
                {
                    self.GetComponent<HaloControl>().Fade();
                    self.GetComponent<HaloControl_Small>().Fade();
                });
                self.GetState("Tele 2").AddMethod(() =>
                {
                    self.GetComponent<HaloControl>().Fade();
                    self.GetComponent<HaloControl_Small>().Fade();
                });
                self.GetState("Tele 3").AddMethod(() =>
                {
                    self.GetComponent<HaloControl>().Fade();
                    self.GetComponent<HaloControl_Small>().Fade();
                });
                self.GetState("Tele 4").AddMethod(() =>
                {
                    self.GetComponent<HaloControl>().Fade();
                    self.GetComponent<HaloControl_Small>().Fade();
                });
                self.GetState("Tele 5").AddMethod(() =>
                {
                    self.GetComponent<HaloControl>().Fade();
                    self.GetComponent<HaloControl_Small>().Fade();
                });
                self.GetState("Tele 6").AddMethod(() =>
                {
                    self.GetComponent<HaloControl>().Fade();
                    self.GetComponent<HaloControl_Small>().Fade();
                });
                self.GetState("Tele 7").AddMethod(() =>
                {
                    self.GetComponent<HaloControl>().Fade();
                    self.GetComponent<HaloControl_Small>().Fade();
                });
                self.GetState("Tele 8").AddMethod(() =>
                {
                    self.GetComponent<HaloControl>().Fade();
                    self.GetComponent<HaloControl_Small>().Fade();
                });
                self.GetState("Tele 9").AddMethod(() =>
                {
                    self.GetComponent<HaloControl>().Fade();
                    self.GetComponent<HaloControl_Small>().Fade();
                });
                self.GetState("Tele 10").AddMethod(() =>
                {
                    self.GetComponent<HaloControl>().Fade();
                    self.GetComponent<HaloControl_Small>().Fade();
                });
                self.GetState("Tele 11").AddMethod(() =>
                {
                    self.GetComponent<HaloControl>().Fade();
                    self.GetComponent<HaloControl_Small>().Fade();
                });
                self.GetState("Tele 12").AddMethod(() =>
                {
                    self.GetComponent<HaloControl>().Fade();
                    self.GetComponent<HaloControl_Small>().Fade();
                });
                self.GetState("Tele 13").AddMethod(() =>
                {
                    self.GetComponent<HaloControl>().Fade();
                    self.GetComponent<HaloControl_Small>().Fade();
                });
                self.GetState("Ascend Tele").AddMethod(() =>
                {
                    self.GetComponent<HaloControl>().Fade();
                    self.GetComponent<HaloControl_Small>().Fade();
                });
                self.GetState("Ascend Cast").AddMethod(() =>
                {
                    self.GetComponent<BossShieldControl>().ReChargAnim();
                });
            }
            if (self.gameObject.name == "Boss Control" && self.FsmName == "Control")
            {
                MusicCue obj = FsmUtil.GetAction<ApplyMusicCue>(self, "Title Up", 3).musicCue.Value as MusicCue;
                MusicCue.MusicChannelInfo musicChannelInfo = ReflectionHelper.GetField<MusicCue, MusicCue.MusicChannelInfo[]>(obj, "channelInfos")[0];
                if (BGM1 != null && HardMode == 0)
                {
                    ReflectionHelper.SetField(musicChannelInfo, "clip", BGM1);
                }
                if (BGM2 != null && HardMode == 1)
                {
                    ReflectionHelper.SetField(musicChannelInfo, "clip", BGM2);
                }
                var title = self.transform.Find("Boss Title").gameObject;
                var title1 = title.transform.Find("Boss Title (1)").gameObject;
                title.GetComponent<TextMeshPro>().color = new Color(0.7147f, 0.2196f, 0.1264f, 1f);
                title1.GetComponent<TextMeshPro>().color = new Color(0.5347f, 0.1596f, 0.0864f, 1f);

            }
            if (self.gameObject.name == "Radiant Beam" && self.FsmName == "Control")
            {
                self.Recycle();
            }
            if (self.gameObject.name == "Radiant Beam (1)" && self.FsmName == "Control")
            {
                self.Recycle();
            }
            if (self.gameObject.name == "Radiant Beam (2)" && self.FsmName == "Control")
            {
                self.Recycle();
            }
            if (self.gameObject.name == "Radiant Beam (3)" && self.FsmName == "Control")
            {
                self.Recycle();
            }
            if (self.gameObject.name == "Radiant Beam (4)" && self.FsmName == "Control")
            {
                self.Recycle();
            }
            if (self.gameObject.name == "Radiant Beam (5)" && self.FsmName == "Control")
            {
                self.Recycle();
            }
            if (self.gameObject.name == "Radiant Beam (6)" && self.FsmName == "Control")
            {
                self.Recycle();
            }
            if (self.gameObject.name == "Radiant Beam (7)" && self.FsmName == "Control")
            {
                self.Recycle();
            }
            if (self.gameObject.name == "Radiant Beam (8)" && self.FsmName == "Control")
            {
                self.Recycle();
            }
            if (self.gameObject.name == "Absolute Radiance" && self.FsmName == "Attack Choices")
            {
                var Pt4 = self.gameObject.transform.Find("Shot Charge").gameObject;
                Pt4.transform.GetComponent<ParticleSystem>().emissionRate = 180f;
                Pt4.transform.GetComponent<ParticleSystem>().maxParticles = 9999;
                Pt4.transform.GetComponent<ParticleSystem>().startSpeed = -24f;
                Pt4.transform.GetComponent<ParticleSystem>().startColor = new Color(1f, 0.9f, 0.73f, 1f);
                Pt4.transform.GetComponent<ParticleSystem>().startSize = 1.5f;
                Pt4.transform.GetComponent<Behaviour>().enabled = true;
                Pt4.transform.localPosition = new Vector3(0, 0, 0);
                Pt4.transform.localScale = new Vector3(3f, 3f, Pt4.transform.localScale.z);
                DREAMPTCHARG2 = Pt4;

                self.GetState("Nail Top Sweep").GetAction<Wait>().time = 1f;
                self.GetState("Beam Sweep R").RemoveAction<SendEventByName>();
                self.GetState("Beam Sweep L").RemoveAction<SendEventByName>();
                self.GetState("Beam Sweep R 2").RemoveAction<SendEventByName>();
                self.GetState("Beam Sweep L 2").RemoveAction<SendEventByName>();
                self.GetState("Nail L Sweep 2").GetAction<SendEventByName>(0).sendEvent.Clear();
                self.GetState("Nail L Sweep 2").GetAction<SendEventByName>(1).sendEvent.Clear();
                self.GetState("Nail L Sweep 2").GetAction<SendEventByName>(2).sendEvent.Clear();
                self.GetState("Nail L Sweep 2").GetAction<SendEventByName>(3).sendEvent.Clear();
                self.GetState("Nail R Sweep 2").GetAction<SendEventByName>(0).sendEvent.Clear();
                self.GetState("Nail R Sweep 2").GetAction<SendEventByName>(1).sendEvent.Clear();
                self.GetState("Nail R Sweep 2").GetAction<SendEventByName>(2).sendEvent.Clear();
                self.GetState("Nail R Sweep 2").GetAction<SendEventByName>(3).sendEvent.Clear();

                self.GetState("Nail L Sweep 2").RemoveAction<SpawnObjectFromGlobalPool>();
                self.GetState("Nail L Sweep 2").AddMethod(() =>
                {
                    if (HardMode == 0)
                    {
                        if (CHOICE <= 2)
                        {
                            self.GetComponent<NailSkill5>().On(0.06f, 1.6f);
                        }
                        else
                        {
                            self.GetComponent<CombR>().On(2f, 0.2f, 20, 1f);
                            self.GetComponent<BeamSkill6>().On(0.5f, 1.4f, 1f, false);
                        }
                    }
                    else
                    {
                        if (CHOICE <= 2)
                        {
                            self.GetComponent<NailSkill5>().On(0.05f, 2f);
                        }
                        else
                        {
                            self.GetComponent<CombR>().On(1.5f, 0.2f, 20, 1.2f);
                            self.GetComponent<BeamSkill6>().On(0.5f, 1.4f, 1f, false);
                        }
                    }
                });
                self.GetState("Nail R Sweep 2").RemoveAction<SpawnObjectFromGlobalPool>();
                self.GetState("Nail R Sweep 2").AddMethod(() =>
                {
                    if (HardMode == 0)
                    {
                        if (CHOICE <= 2)
                        {
                            self.GetComponent<NailSkill5>().On(0.06f, 1.6f);
                        }
                        else
                        {
                            self.GetComponent<CombL>().On(2f, 0.2f, 20, 1f);
                            self.GetComponent<BeamSkill6>().On(0.5f, 1.4f, 1f, false);
                        }
                    }
                    else
                    {
                        if (CHOICE <= 2)
                        {
                            self.GetComponent<NailSkill5>().On(0.05f, 2f);
                        }
                        else
                        {
                            self.GetComponent<CombL>().On(1.5f, 0.2f, 20, 1.2f);
                            self.GetComponent<BeamSkill6>().On(0.5f, 1.4f, 1f, false);
                        }
                    }
                });
                self.GetState("Beam Sweep R").AddMethod(() =>
                {
                    if (HardMode == 0)
                    {
                        self.GetComponent<BeamSight_1>().On();
                    }
                    else
                    {
                        self.GetComponent<BeamSight_2>().On();
                    }
                });
                self.GetState("Beam Sweep L").AddMethod(() =>
                {
                    if (HardMode == 0)
                    {
                        self.GetComponent<BeamSight_1>().On();
                    }
                    else
                    {
                        self.GetComponent<BeamSight_2>().On();
                    }
                });
                self.GetState("Beam Sweep R 2").AddMethod(() =>
                {
                    if (HardMode == 0)
                    {
                        self.GetComponent<BeamSight_1>().On();
                    }
                    else
                    {
                        self.GetComponent<BeamSight_2>().On();
                    }
                });
                self.GetState("Beam Sweep L 2").AddMethod(() =>
                {
                    if (HardMode == 0)
                    {
                        self.GetComponent<BeamSight_1>().On();
                    }
                    else
                    {
                        self.GetComponent<BeamSight_2>().On();
                    }
                });
            }
            if (self.gameObject.name == "Absolute Radiance" && self.FsmName == "Attack Commands")
            {
                self.GetComponent<tk2dSprite>().Collection.materials[0].mainTexture = LoadPng("Sunset.Resources.skin0.png");
                self.GetComponent<tk2dSprite>().Collection.materials[1].mainTexture = LoadPng("Sunset.Resources.skin1.png");
                self.GetComponent<tk2dSprite>().Collection.materials[2].mainTexture = LoadPng("Sunset.Resources.skin2.png");
                BOSS = self.gameObject;
                NAIL = self.GetState("CW Spawn").GetAction<SpawnObjectFromGlobalPool>().gameObject.Value;
                ORB = self.GetState("Spawn Fireball").GetAction<SpawnObjectFromGlobalPool>().gameObject.Value;

                self.GetState("CW Fire").RemoveAction<AudioPlayerOneShotSingle>();
                self.GetState("CCW Fire").RemoveAction<AudioPlayerOneShotSingle>();

                self.GetState("Comb Top 2").RemoveAction<SpawnObjectFromGlobalPool>();
                self.GetState("Comb Top").RemoveAction<SpawnObjectFromGlobalPool>();

                var shieldRecharge = self.CopyState("Orb Summon", "ShieldRecharge");
                self.ChangeTransition("ShieldRecharge", "FINISHED", "Orb End");
                self.GetState("ShieldRecharge").AddMethod(() =>
                {
                    self.GetComponent<BossShieldControl>().ReChargAnim();
                });
                self.GetState("ShieldRecharge").GetAction<Wait>().time = 1.8f;
                self.ChangeTransition("Orb Summon", "FINISHED", "Orb End");
                self.GetState("Orb Summon").GetAction<SetParticleEmission>(4).emission = false;
                self.GetState("Orb Summon").GetAction<SetParticleEmission>(5).emission = false;
                self.GetState("Orb Summon").RemoveAction<AudioPlayerOneShotSingle>();

                self.ChangeTransition("NF Glow", "FINISHED", "EB Glow End");
                self.GetState("NF Glow").GetAction<Wait>().time.Value = 3.3f;
                self.GetState("NF Glow").RemoveAction<ActivateGameObject>();
                self.GetState("Nail Fan").RemoveAction<ActivateGameObject>();

                self.GetState("CW Spawn").RemoveAction<SpawnObjectFromGlobalPool>();
                self.GetState("CCW Spawn").RemoveAction<SpawnObjectFromGlobalPool>();
                self.GetState("CW Restart").GetAction<Wait>().time.Value = 1f;
                self.GetState("CCW Restart").GetAction<Wait>().time.Value = 1f;
                self.GetState("Nail Fan").GetAction<Wait>().time.Value = 3f;


                self.GetState("Orb Antic").AddMethod(() =>
                {
                    if (ShieldOn)
                    {
                        BOSS.GetComponent<BossSkillChoice>().OrbSkill();
                    }
                    else
                    {
                        if (ShieldNeedRecharge)
                        {
                            self.SetState("ShieldRecharge");
                        }
                        else
                        {
                            self.SetState("Orb End");
                        }
                    }
                });
                self.GetState("NF Glow").AddMethod(() =>
                {
                    if (ShieldOn)
                    {
                        BOSS.GetComponent<BossSkillChoice>().BeamSkill();
                    }
                    else
                    {
                        if (ShieldNeedRecharge)
                        {
                            self.SetState("ShieldRecharge");
                            self.transform.Find("Eye Beam Glow").gameObject.SetActive(false);
                        }
                        else
                        {
                            self.SetState("EB End");
                        }
                    }
                });
                self.GetState("Nail Fan").AddMethod(() =>
                {
                    if (ShieldOn)
                    {
                        BOSS.GetComponent<BossSkillChoice>().NailSkill();
                    }
                    else
                    {
                        if (ShieldNeedRecharge)
                        {
                            self.SetState("ShieldRecharge");
                        }
                        else
                        {
                            self.SetState("End");
                        }
                    }
                });

                self.GetState("Comb L").RemoveAction<SpawnObjectFromGlobalPool>();
                self.GetState("Comb L").AddMethod(() =>
                {
                    BOSS.GetComponent<BossSkillChoice>().NailSweepLP1();
                });
                self.GetState("Comb R").RemoveAction<SpawnObjectFromGlobalPool>();
                self.GetState("Comb R").AddMethod(() =>
                {
                    BOSS.GetComponent<BossSkillChoice>().NailSweepRP1();
                });
                self.GetState("Comb Top").AddMethod(() =>
                {
                    self.GetComponent<BeamTop>().On(0.4f, 3.1f, 1.2f);
                });
                self.GetState("Comb Top 2").AddMethod(() =>
                {
                    self.GetComponent<BeamTop>().On(0.5f, 3.1f, 1.2f);
                });
                var Phase3AB = self.CopyState("AB Start", "Phase3AB1");
                self.ChangeTransition("AB Start", "FINISHED", "Phase3AB1");
                self.ChangeTransition("Phase3AB1", "FINISHED", "AB Start");
                self.GetState("AB Start").AddMethod(() =>
                {
                });
            }
        }

        public static DamageEnemies SetDamageEnemy(GameObject go, int value = 0, float angle = 0f, float magnitudeMult = 0f, AttackTypes type = AttackTypes.Nail)
        {
            DamageEnemies damageEnemies = go.GetComponent<DamageEnemies>() ?? go.AddComponent<DamageEnemies>();
            damageEnemies.attackType = type;
            damageEnemies.circleDirection = false;
            damageEnemies.damageDealt = value;
            damageEnemies.direction = angle;
            damageEnemies.ignoreInvuln = false;
            damageEnemies.magnitudeMult = magnitudeMult;
            damageEnemies.moveDirection = false;
            damageEnemies.specialType = SpecialTypes.None;
            return damageEnemies;
        }
        public static AudioClip LoadAudioClip(string path)
        {
            Stream stream = Assembly.GetExecutingAssembly().GetManifestResourceStream(path);
            WavData wavData = new WavData();
            wavData.Parse(stream, null);
            stream.Close();
            float[] samples = wavData.GetSamples();
            AudioClip audioClip = AudioClip.Create("audio", samples.Length / wavData.FormatChunk.NumChannels, wavData.FormatChunk.NumChannels, (int)wavData.FormatChunk.SampleRate, false);
            audioClip.SetData(samples, 0);
            return audioClip;
        }
        public static Texture2D LoadPng(string path)
        {
            Stream stream = Assembly.GetExecutingAssembly().GetManifestResourceStream(path);
            Texture2D texture2D;
            MemoryStream memoryStream = new MemoryStream((int)stream.Length);
            stream.CopyTo(memoryStream);
            stream.Close();
            var bytes = memoryStream.ToArray();
            memoryStream.Close();
            texture2D = new Texture2D(1, 1);
            texture2D.LoadImage(bytes, true);
            return texture2D;
        }
        public class SceneSwitchDetector : MonoBehaviour
        {
            bool summoned = false;
            bool changed = false;
            bool detect = false;
            void HaloSummon()
            {
                if(HALO_WORKSHOP0 == null)
                {
                    HALO_WORKSHOP0 = Instantiate(HALO_prefab, new Vector3(250.03f, 40.57f, 2.77f), Quaternion.Euler(0, 0, 0), HeroController.instance.transform);
                    HALO_WORKSHOP0.transform.SetParent(null);
                    HALO_WORKSHOP0.transform.localScale = new Vector3(1f, 1f, 1f);
                    HALO_WORKSHOP0.GetComponent<SpriteRenderer>().enabled = true;
                    HALO_WORKSHOP0.GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, 1f);
                    HALO_WORKSHOP0.name = "Halo0";
                    HALO_WORKSHOP0.gameObject.LocateMyFSM("Fader").enabled = false;
                    HALO_WORKSHOP0.gameObject.LocateMyFSM("Rotation").enabled = false;
                    HALO_WORKSHOP0.AddComponent<HaloRotateChange>();
                }
                if(HALO_WORKSHOP == null)
                {
                    HALO_WORKSHOP = Instantiate(ANOTHERHALO_prefab, new Vector3(250.03f, 40.57f, 2.77f), Quaternion.Euler(0, 0, 0), HeroController.instance.transform);
                    HALO_WORKSHOP.transform.SetParent(null);
                    HALO_WORKSHOP.transform.localScale = new Vector3(1f, 1f, 1f);
                    HALO_WORKSHOP.GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, 1);
                    HALO_WORKSHOP.name = "Halo1";
                    HALO_WORKSHOP.gameObject.LocateMyFSM("Fader").enabled = false;
                    HALO_WORKSHOP.gameObject.LocateMyFSM("Rotation").enabled = false;
                    HALO_WORKSHOP.AddComponent<HaloColorChange_SmallWorkshop>();
                    HALO_WORKSHOP.AddComponent<HaloRotateChange_SmallWorkshop>();
                }
            }
            public void Repeat()
            {
                GameObject ST = GameObject.Find("GG_Statue_Radiance").gameObject;
                GameObject SW = ST.transform.Find("dream_version_switch").gameObject;
                var LP = SW.transform.Find("lit_pieces").gameObject;
                var haze = LP.transform.Find("haze").gameObject;
                var glow = LP.transform.Find("plinth_glow").gameObject;
                var guy = LP.transform.Find("dream_glowy_guy").gameObject;
                haze.GetComponent<SpriteRenderer>().color = new Color(1f, 0.26f, 0.2f, 1f);
                glow.GetComponent<SpriteRenderer>().color = new Color(1f, 0.67f, 0.71f, 1f);
                guy.GetComponent<SpriteRenderer>().color = new Color(1f, 0.87f, 1f, 1f);
                if (guy.GetComponent<SpriteRenderer>().color != new Color(1f, 0.87f, 1f, 1f) || glow.GetComponent<SpriteRenderer>().color != new Color(1f, 0.67f, 0.71f, 1f) || haze.GetComponent<SpriteRenderer>().color != new Color(1f, 0.45f, 0.71f, 1f))
                {
                    Invoke("Change", 0.5f);
                }
                if(!summoned)
                {
                    HaloSummon();
                    summoned = true;
                }
            }
            public void SwitchDetectAndChange()
            {
                GameObject ST = GameObject.Find("GG_Statue_Radiance").gameObject;
                GameObject SW = ST.transform.Find("dream_version_switch").gameObject;

                GameObject LIGHT = GameObject.Find("Fade Sprite").gameObject;

                GameObject SP = SW.transform.Find("Statue Pt").gameObject;

                GameObject BEAM = GameObject.Find("dream_beam_animation").gameObject;
                var beam1 = BEAM.transform.Find("dream_nail_base").gameObject;
                var beam2 = BEAM.transform.Find("cd_room_beam_glow").gameObject;
                var beam3 = BEAM.transform.Find("dream_beam").gameObject;
                var beam4 = BEAM.transform.Find("dream_beam (1)").gameObject;
                var LP = SW.transform.Find("lit_pieces").gameObject;
                DREAMVER = LP;

                var haze = LP.transform.Find("haze").gameObject;
                var glow = LP.transform.Find("plinth_glow").gameObject;
                var guy = LP.transform.Find("dream_glowy_guy").gameObject;
                var pt = SW.transform.Find("Statue Pt").gameObject;

                if (guy == null || glow == null || haze == null)
                {
                    Invoke("SwitchDetectAndChange", 0.1f);
                    return;
                }

                haze.GetComponent<SpriteRenderer>().color = new Color(1f, 0.26f, 0.2f, 1f);
                glow.GetComponent<SpriteRenderer>().color = new Color(1f, 0.67f, 0.71f, 1f);
                guy.GetComponent<SpriteRenderer>().color = new Color(1f, 1f, 1f, 1f);

                if (!changed)
                {
                    changed = true;
                    STATUS = ST;
                    SW.AddComponent<StatueDeleter>();
                    SW.SetActive(true);
                    SW.transform.Find("GG_statue_plinth_dream").gameObject.SetActive(true);
                    SW.transform.Find("GG_statue_plinth_orb_off").gameObject.SetActive(true);
                    SP.GetComponent<ParticleSystem>().startColor = new Color(1, 1f, 1f, 1f);
                    SP.GetComponent<ParticleSystem>().maxParticles = 5000;
                    SP.GetComponent<ParticleSystem>().startLifetime = 7f;
                    SP.GetComponent<ParticleSystem>().startSize = 2.4f;
                    var toggle = ST.GetComponentInChildren<BossStatueDreamToggle>(true);
                    toggle.SetState(true);
                    Modding.ReflectionHelper.SetField
                    (
                        toggle,
                        "colorFaders",
                        toggle.litPieces.GetComponentsInChildren<ColorFader>(true)
                    );
                    var bs = ST.GetComponent<BossStatue>();
                    toggle.SetOwner(bs);
                    var scene1 = ScriptableObject.CreateInstance<BossScene>();
                    scene1.sceneName = "GG_Radiance";
                    bs.dreamBossScene = scene1;
                    bs.dreamStatueStatePD = "statueStateRadiance";
                    var details = new BossStatue.BossUIDetails();
                    details.nameKey = details.nameSheet = "NAME_FINAL_BOSS";
                    details.descriptionKey = "GG_S_RADIANCE";
                    details.descriptionSheet = "CP3";
                    bs.dreamBossDetails = details;
                    var Base = ST.transform.Find("Base").gameObject;
                    if (Base.transform.Find("Plaque").gameObject != null)
                    {
                        Base.transform.Find("Plaque").gameObject.Recycle();
                    }

                    var Switch = ST.transform.Find("dream_version_switch").gameObject;
                    Switch.transform.Find("GG_statue_plinth_dream").gameObject.SetActive(false);
                    Switch.transform.position = new Vector3(247.24f, 35.89f, 0.24f);
                    var lit = Switch.transform.Find("lit_pieces").gameObject;
                    lit.transform.Find("plinth_glow").gameObject.GetComponent<SpriteRenderer>().color = new Color(0, 0, 0, 0);
                    lit.transform.Find("Base Glow").transform.position = new Vector3(249.9916f, 36.0137f, 2.51f);
                    lit.transform.Find("Base Glow").GetComponent<tk2dSprite>().color = new Color(0.85f, 0.85f, 1, 1);

                    pt.transform.position = new Vector3(249.9916f, 36.0137f, 2.51f);
                    pt.transform.localScale = new Vector3(3.7f, 1.1f, 1f);
                    LIGHT.SetActive(false);

                    if(HardMode == 1)
                    {
                        Switch.SetActive(true);
                    }

                }
                if (!DREAMSWITCH)
                {
                    GameObject HAZE2 = GameObject.Find("haze2").gameObject;
                    HAZE2.GetComponent<SpriteRenderer>().color = new Color(0.5f, 0.45f, 0.5f, 0.7f);
                    beam1.GetComponent<SpriteRenderer>().color = new Color(0.85f, 0.85f, 1f, 1f);
                    beam2.GetComponent<SpriteRenderer>().color = new Color(1f, 0.85f, 1f, 0.4f);
                    beam3.GetComponent<SpriteRenderer>().color = new Color(1f, 0.85f, 1f, 0f);
                    beam4.GetComponent<SpriteRenderer>().color = new Color(1f, 0.85f, 1f, 0f);
                }
                else
                {
                    LP.transform.Find("plinth_glow").gameObject.GetComponent<SpriteRenderer>().color = new Color(0, 0, 0, 0);
                    GameObject HAZE2 = GameObject.Find("haze2").gameObject;
                    LP.SetActive(true);
                    haze.GetComponent<ColorFader>().Fade(true);
                    glow.GetComponent<ColorFader>().Fade(true);
                    guy.GetComponent<ColorFader>().Fade(true);
                    pt.GetComponent<ParticleSystem>().enableEmission = true;
                    HAZE2.GetComponent<SpriteRenderer>().color = new Color(1f, 0.3f, 0.2f, 0.85f);
                    beam1.GetComponent<SpriteRenderer>().color = new Color(1f, 0.85f, 1f, 1f);
                    beam2.GetComponent<SpriteRenderer>().color = new Color(1f, 0.7f, 0.7f, 0.4f);
                    beam3.GetComponent<SpriteRenderer>().color = new Color(1f, 0.85f, 1f, 0f);
                    beam4.GetComponent<SpriteRenderer>().color = new Color(1f, 0.85f, 1f, 0f);
                }
                StartCoroutine(DelayedExecution(0.2f));
                IEnumerator DelayedExecution(float time)
                {
                    yield return new WaitForSeconds(time);
                    if (!DREAMSWITCH)
                    {
                        GameObject HAZE2 = GameObject.Find("haze2").gameObject;
                        HAZE2.GetComponent<SpriteRenderer>().color = new Color(0.5f, 0.45f, 0.5f, 0.7f);
                        beam1.GetComponent<SpriteRenderer>().color = new Color(0.85f, 0.85f, 1f, 1f);
                        beam2.GetComponent<SpriteRenderer>().color = new Color(1f, 0.85f, 1f, 0.4f);
                        beam3.GetComponent<SpriteRenderer>().color = new Color(1f, 0.85f, 1f, 0f);
                        beam4.GetComponent<SpriteRenderer>().color = new Color(1f, 0.85f, 1f, 0f);
                    }
                    else
                    {
                        GameObject HAZE2 = GameObject.Find("haze2").gameObject;
                        LP.SetActive(true);
                        haze.GetComponent<ColorFader>().Fade(true);
                        glow.GetComponent<ColorFader>().Fade(true);
                        guy.GetComponent<ColorFader>().Fade(true);
                        HAZE2.GetComponent<SpriteRenderer>().color = new Color(1f, 0.3f, 0.2f, 0.85f);
                        beam1.GetComponent<SpriteRenderer>().color = new Color(1f, 0.85f, 1f, 1f);
                        beam2.GetComponent<SpriteRenderer>().color = new Color(1f, 0.7f, 0.7f, 0.4f);
                        beam3.GetComponent<SpriteRenderer>().color = new Color(1f, 0.85f, 1f, 0f);
                        beam4.GetComponent<SpriteRenderer>().color = new Color(1f, 0.85f, 1f, 0f);
                        pt.GetComponent<ParticleSystem>().enableEmission = true;
                    }
                }
                if (!summoned)
                {
                    HaloSummon();
                    summoned = true;
                }
            }
            void Update()
            {
                if(detect)
                {

                }
            }
            void OnEnable()
            {
                UnityEngine.SceneManagement.SceneManager.sceneLoaded += OnSceneLoaded;
            }
            void OnDisable()
            {
                UnityEngine.SceneManagement.SceneManager.sceneLoaded -= OnSceneLoaded;
            }
            void OnSceneLoaded(Scene scene, LoadSceneMode mode)
            {
                if (scene.name != "GG_Radiance")
                {
                    if (Camera.GetComponent<tk2dCamera>().ZoomFactor < 0.9)
                    {
                        Camera.GetComponent<tk2dCamera>().ZoomFactor = 1;
                        if (Camera.GetComponent<ChangeColor>() != null)
                        {
                            Destroy(Camera.GetComponent<ChangeColor>());
                        }
                        if (Camera.GetComponent<Away>() != null)
                        {
                            Destroy(Camera.GetComponent<Away>());
                        }
                    }
                }
                else
                {
                    if (HardMode == 0)
                    {
                        P4Hp = 1020 + GameManager.instance.playerData.nailDamage * 20;
                    }
                    else
                    {
                        P4Hp = 1020 + GameManager.instance.playerData.nailDamage * 30;
                    }
                    GameObject.Find("Cloud Hazard").LocateMyFSM("damages_hero").FsmVariables.FindFsmInt("damageDealt").RawValue = 2;
                    var bc = GameObject.Find("Boss Control").gameObject;
                    var abyss = bc.transform.Find("Abyss Pit").gameObject;
                    abyss.transform.Find("Spike Collider").GetComponent<DamageHero>().damageDealt = 2;
                    if(Camera.GetComponent<ChangeColor>() == null)
                    {
                        Camera.AddComponent<ChangeColor>();
                    }

                    if (Camera.GetComponent<Away>() == null)
                    {
                        Camera.AddComponent<Away>();
                    }
                    if (settings_.on == true)
                    {
                        Camera.GetComponent<Away>().enabled = true;
                        Camera.GetComponent<ChangeColor>().ChangeStart();

                        if (DREAMVER.activeSelf == false)
                            HardMode = 0;
                        else
                            HardMode = 1;
                    }
                }

                if (scene.name == "GG_Workshop")
                {
                    changed = false;
                    SwitchDetectAndChange();
                    Invoke("HaloSummon",2f);
                }
            }
        }
        public class StatueDeleter : MonoBehaviour
        {
            void Start()
            {
                Invoke("Delete", 0.5f);
                Invoke("Delete", 0.6f);
                Invoke("Delete", 0.7f);
                Invoke("Delete", 0.8f);
                Invoke("Delete", 0.9f);
                Invoke("Delete", 1.0f);
                Invoke("Delete", 1.1f);
            }
            void Delete()
            {
                var sta = GameObject.Find("GG_statues_0006_5 (1)").gameObject;
                sta.Recycle();
            }
        }
        public class Dreamcheck : MonoBehaviour
        {
            public bool Done = true;
            public void Check()
            {
                Done = false;
            }
            public void Update()
            {
                if (DREAMSWITCH == false && Done == false)
                {
                    Done = true;
                }
                else if (DREAMVER != null && Done == false)
                {
                    DREAMVER.SetActive(true);
                    Done = true;
                }
            }
        }
        public class Dreamcheck2 : MonoBehaviour
        {
            public void Update()
            {
                if (STATUS.GetComponent<BossStatue>().UsingDreamVersion == true)
                {
                    STATUS.GetComponent<BossStatue>().SetDreamVersion(false, false, true);
                }
                else
                {
                    Destroy(Camera.GetComponent<Dreamcheck2>());
                }
            }
        }
        public class Away : MonoBehaviour
        {
            bool Stop = false;
            float Timer;
            public void Start()
            {
                Timer = 0f;
                Stop = false;
            }
            public void FixedUpdate()
            {
                if (Stop == false && Timer <= 4)
                {
                    Timer += Time.deltaTime;
                    float Speed = 0f;
                    Speed += (Camera.GetComponent<tk2dCamera>().ZoomFactor - 0.65f) / 20 * Time.deltaTime / 0.02f;
                    Camera.GetComponent<tk2dCamera>().ZoomFactor -= Speed;
                }
                else
                {
                    gameObject.GetComponent<Away>().enabled = false;
                }
            }
        }
        public class CameraControl : MonoBehaviour
        {
            public float Timer = 0f;
            public bool Start = false;
            public bool DREA;
            public void StartControl()
            {
                GameObject GC = GameCameras.instance.gameObject;
                GameObject CP = GC.transform.Find("CameraParent").gameObject;
                Camera = CP.transform.Find("tk2dCamera").gameObject;
                if (Camera.GetComponent<SceneSwitchDetector>() == null)
                {
                    Camera.AddComponent<SceneSwitchDetector>();
                }
            }
        }
        public class ChangeColor : MonoBehaviour
        {
            public static List<GameObject> PILLARS = new List<GameObject>();
            public static List<GameObject> CLOUDS = new List<GameObject>();
            public static GameObject Haze1;
            public static Color Haze1color1 = new Color(0.74f, 0.66f, 0.56f, 1f);
            public static Color Haze1color2 = new Color(0.7f, 0.5f, 0.42f, 1f);
            public static Color Haze1color3 = new Color(0.6f, 0.32f, 0.27f, 1f);
            public static Color Pillarcolor1 = new Color(1f, 0.77f, 0.67f, 1f);
            public static Color Pillarcolor2 = new Color(0.77f, 0.65f, 0.49f, 1f);
            public static Color Pillarcolor3 = new Color(0.57f, 0.34f, 0.24f, 1f);
            public static float BGcolorR = 0.1f;
            public static float BGcolorG = 0.03f;
            public static float BGcolorB = 0.14f;
            public static float CLcolorR = 0.45f;
            public static float CLcolorG = 0.28f;
            public static float CLcolorB = 0.2f;
            public static float STcolorR = 1f;
            public static float STcolorG = 0.78f;
            public static float STcolorB = 0.82f;
            public static float STcolorA = 0.42f;
            public static float Timer = 0;
            public static bool Started1 = false;
            public static bool Started2 = false;
            public static bool Started3 = false;
            public static int UnDone = 0;
            public void ChangeStart()
            {
                Started1 = true;
                Started2 = false;
                Started3 = false;
                if (Started1 == true)
                {
                    var gate1 = Instantiate(GATE_Prefab, new Vector3(75.234f, 16.8705f, 0.1591f), Quaternion.Euler(0, 0, 0));
                    var gate2 = Instantiate(GATE_Prefab, new Vector3(45.5522f, 16.8705f, 0.1591f), Quaternion.Euler(0, 0, 180));
                    UnDone = 0;
                    GameObject GG_Arena_Prefab = GameObject.Find("GG_Arena_Prefab");
                    GameObject bg = GG_Arena_Prefab.transform.Find("BG").gameObject;
                    GameObject throne = bg.transform.Find("throne").gameObject;
                    throne.transform.position += new Vector3(3f, 0f, 0f);
                    throne.transform.eulerAngles += new Vector3(0f, 0f, -15f);
                    GameObject PL = GameObject.Find("GG_pillar_top").gameObject;
                    PL.transform.position += new Vector3(1f, 0f, 0f);
                    PL.transform.eulerAngles += new Vector3(0f, 0f, -15f);
                    GameObject GodSeeker = GameObject.Find("Godseeker Crowd").gameObject;
                    GodSeeker.SetActive(false);

                    List<GameObject> OBJECTS = new List<GameObject>();
                    for (int i = 0; i < bg.transform.childCount; i++)
                    {
                        OBJECTS.Add(bg.transform.GetChild(i).gameObject);
                    }
                    foreach (GameObject g in OBJECTS)
                    {
                        if (g.name.Contains("haze"))
                        {
                            SpriteRenderer render = g.GetComponent<SpriteRenderer>();
                            bool succeed = render != null;
                            if (succeed)
                            {
                                g.GetComponent<SpriteRenderer>().color = new Color(BGcolorR, BGcolorG, BGcolorB, g.GetComponent<SpriteRenderer>().color.a);
                                if (g.name.Contains("haze2 (1)"))
                                {
                                    g.GetComponent<SpriteRenderer>().color = Haze1color1;
                                    Haze1 = g;
                                }
                                if (g.name.Contains("haze2 (5)"))
                                    g.GetComponent<SpriteRenderer>().color = new Color(0.5f, 0.8f, 1f, 1f);
                            }
                            else
                            {
                                Invoke("ChangeStart", 0.1f);
                                return;
                            }
                        }
                        if (g.name.Contains("GG_scenery_0004_17"))
                        {
                            SpriteRenderer render = g.GetComponent<SpriteRenderer>();
                            bool succeed = render != null;
                            if (succeed)
                            {
                                g.GetComponent<SpriteRenderer>().color = new Color(CLcolorR, CLcolorG, CLcolorB, g.GetComponent<SpriteRenderer>().color.a);
                                CLOUDS.Add(g);
                            }
                            else
                            {
                                Invoke("ChangeStart", 0.1f);
                                return;
                            }
                        }
                        if (g.name.Contains("ray"))
                        {
                            for (int i = 0; i < g.transform.childCount; i++)
                            {
                                if (g.transform.GetChild(i).gameObject.GetComponent<SpriteRenderer>() != null)
                                {
                                    g.transform.GetChild(i).gameObject.GetComponent<SpriteRenderer>().color = new Color(STcolorR, STcolorG, STcolorB, STcolorA);
                                }
                            }
                        }
                        if (g.name.Contains("pillar"))
                        {
                            for (int i = 0; i < g.transform.childCount; i++)
                            {
                                if (g.transform.GetChild(i).gameObject.GetComponent<SpriteRenderer>() != null)
                                {
                                    g.transform.GetChild(i).gameObject.GetComponent<SpriteRenderer>().color = Pillarcolor1;
                                    PILLARS.Add(g.transform.GetChild(i).gameObject);
                                }
                            }
                        }
                    }/*
                        if (UnDone >= 1)
                        {
                            Camera.GetComponent<ChangeWait>().Repeat();
                        }
                        */

                }
            }
            public void Change2()
            {
                Started2 = true;
                Timer = 0;
            }
            public void Change3()
            {
                Started3 = true;
                Timer = 0;
            }
            public void FixedUpdate()
            {
                if (Started2 == true)
                {
                    if (Timer <= 5)
                    {
                        Timer += Time.deltaTime;
                        Haze1.GetComponent<SpriteRenderer>().color += (Haze1color2 - Haze1.GetComponent<SpriteRenderer>().color) / 25 * Time.deltaTime / 0.02f;
                        foreach (var p in PILLARS)
                        {
                            p.GetComponent<SpriteRenderer>().color += (Pillarcolor2 - p.GetComponent<SpriteRenderer>().color) / 25 * Time.deltaTime / 0.02f;
                        }
                    }
                }
                if (Started3 == true)
                {
                    if (Timer <= 6)
                    {
                        Timer += Time.deltaTime;
                        Haze1.GetComponent<SpriteRenderer>().color += (Haze1color3 - Haze1.GetComponent<SpriteRenderer>().color) / 30 * Time.deltaTime / 0.02f;
                        foreach (var p in PILLARS)
                        {
                            p.GetComponent<SpriteRenderer>().color += (Pillarcolor3 - p.GetComponent<SpriteRenderer>().color) / 25 * Time.deltaTime / 0.02f;
                        }
                    }
                }
            }
        }
        public class SkillsWaitTimeChange : MonoBehaviour
        {
            public void ChangeOrb(float time)
            {
                BOSS.LocateMyFSM("Attack Commands").GetState("Orb Antic").GetAction<Wait>().time = time;
                BOSS.LocateMyFSM("Attack Commands").GetState("Orb Summon").GetAction<Wait>().time = time / 4f;
            }
            public void ChangeBeam(float time)
            {
                BOSS.LocateMyFSM("Attack Commands").GetState("NF Glow").GetAction<Wait>().time = time;
            }
            public void ChangeNail(float time)
            {
                BOSS.LocateMyFSM("Attack Commands").GetState("Nail Fan").GetAction<Wait>().time = time;
            }
        }
        public class Audios:MonoBehaviour
        {
            public float volume1 = 1f;
            public float volume2 = 0.4f;
            public float volume3 = 0.6f;
            public float volume4 = 0.7f;
            public float tikSpeed = 0.5f;
            public float tikTimer = 1f;
            bool timeUp = true;
            public void Init()
            {
                //HALO1.GetComponent<AudioSource>().clip = Metal;
            }
            public void Play(int soundType)
            {
                HALO1.GetComponent<AudioSource>().clip = Metal;
                if (soundType == 1)
                {
                    BOSS.GetComponent<AudioSource>().PlayOneShot(BeamCannon, volume1);
                }
                if(soundType == 2)
                {
                    BOSS.GetComponent<AudioSource>().PlayOneShot(Tik, volume2);
                }
                if(soundType == 3)
                {
                    BOSS.GetComponent<AudioSource>().PlayOneShot(Metal, volume3);
                }
                if(soundType == 4)
                {
                    BOSS.GetComponent<AudioSource>().PlayOneShot(Metal2, volume4);
                }
                if(soundType == 5)
                {
                    BOSS.GetComponent<AudioSource>().PlayOneShot(BeamCannon_H, volume1);
                }
            }
        }
        public class HeroAngle : MonoBehaviour
        {
            public float Angle;
            float ADDANGLE;
            void FixedUpdate()
            {
                if (Math.Sign(BOSS.transform.position.x - HeroController.instance.gameObject.transform.position.x) < 0)
                {
                    ADDANGLE = 180;
                }
                else
                {
                    ADDANGLE = 0;
                }
                Angle = (float)RadiansToDegrees(Math.Atan((BOSS.transform.position.y - HeroController.instance.gameObject.transform.position.y + 2.5) / (BOSS.transform.position.x - HeroController.instance.gameObject.transform.position.x))) + ADDANGLE + 90;
            }
        }
        public class ObjRecycle : MonoBehaviour
        {
            void Awake()
            {
                Invoke("Recycle", 4.5f);
            }
            void Recycle()
            {
                gameObject.Recycle();
            }
        }
        public class BossDash : MonoBehaviour
        {
            bool dashing;
            float angle;
            float speed;
            public void DashAuto()
            {
                float distance = Vector2.Distance(BOSS.transform.position, HeroController.instance.gameObject.transform.position);
                if (BOSS.transform.position.x - HeroController.instance.gameObject.transform.position.x < 0)
                {
                    angle = (float)RadiansToDegrees(Math.Atan((BOSS.transform.position.y - HeroController.instance.gameObject.transform.position.y + 8f) / (BOSS.transform.position.x - HeroController.instance.gameObject.transform.position.x))) + 180;
                }
                else
                {
                    angle = (float)RadiansToDegrees(Math.Atan((BOSS.transform.position.y - HeroController.instance.gameObject.transform.position.y + 8f) / (BOSS.transform.position.x - HeroController.instance.gameObject.transform.position.x)));
                }
                speed = (2f + 15f / (3f + distance)) * 6f;
                dashing = true;
            }
            public void Dash(float DashAngle, float DashSpeedFactor)
            {
                angle = DashAngle;
                speed = DashSpeedFactor * 3f;
                dashing = true;
            }
            void DashEnd()
            {
                dashing = false;
            }
            void FixedUpdate()
            {
                if(dashing)
                {
                    float x = (float)Math.Cos(DegreesToRadians(angle)) * Time.deltaTime * speed;
                    float y = (float)Math.Sin(DegreesToRadians(angle)) * Time.deltaTime * speed;
                    gameObject.transform.position += new Vector3(x, y, 0);
                    speed -= speed * Time.deltaTime * 5f;
                }
            }
        }
        public class HaloControl : MonoBehaviour
        {
            float factor1 = 1;
            float factor2 = 1;
            bool eye = false;
            void Start()
            {
                HALO = BOSS.transform.Find("Halo").gameObject;
                HALO.gameObject.AddComponent<HaloColorChange>();
                HALO.gameObject.AddComponent<HaloRotateChange>();
                HALO.gameObject.AddComponent<HaloScaleChange>();
                HALO.gameObject.AddComponent<HaloAround>();

                HALO.gameObject.LocateMyFSM("Fader").enabled = false;
                HALO.gameObject.LocateMyFSM("Rotation").enabled = false;
                HALO.GetComponent<SpriteRenderer>().enabled = true;

                Texture2D halo1 = LoadPng("Sunset.Resources.Halo0.png");
                HALO.GetComponent<SpriteRenderer>().sprite = Sprite.Create(halo1, new Rect(0f, 0f, (float)halo1.width, (float)halo1.height), new Vector2(0.5f, 0.5f), HALO.GetComponent<SpriteRenderer>().sprite.pixelsPerUnit);
            }
            public void Control(float delayTime, float animScaleFactor, float animRotateFactor, bool MoveToEye)
            {
                factor1 = animScaleFactor;
                factor2 = animRotateFactor;
                eye = MoveToEye;
                Invoke("Begin", delayTime);
            }
            void Begin()
            {
                HALO.GetComponent<HaloColorChange>().Change();
                HALO.GetComponent<HaloScaleChange>().Change(factor1);
                HALO.GetComponent<HaloRotateChange>().Change(factor2);
                if(eye)
                {
                    HALO.GetComponent<HaloAround>().MoveToEye();
                }
            }
            public void ControlEnd(float delayTime)
            {
                Invoke("End", delayTime);
            }
            public void Fade()
            {
                HALO.GetComponent<HaloColorChange>().TeleChange();
                HALO1.GetComponent<HaloColorChange_Small>().TeleChange();
            }
            public void Stun()
            {
                HALO.GetComponent<HaloColorChange>().Disappear();
                HALO1.GetComponent<HaloColorChange_Small>().Disappear();
            }
            public void StunEnd()
            {
                HALO.GetComponent<HaloColorChange>().Appear();
                HALO1.GetComponent<HaloColorChange_Small>().Appear();
            }
            void End()
            {
                HALO.GetComponent<HaloColorChange>().Recover();
                HALO.GetComponent<HaloScaleChange>().Recover();
                HALO.GetComponent<HaloRotateChange>().Recover();
                HALO.GetComponent<HaloAround>().SpinEnd();
            }
        }
        public class HaloColorChange : MonoBehaviour
        {
            bool enable = false;
            bool Tele = false;
            public void Change()
            {
                enable = true;
            }
            public void Recover()
            {
                enable = false;
            }
            public void TeleChange()
            {
                Tele = true;
                Invoke("TeleChangeEnd", 0.5f);
            }
            void TeleChangeEnd()
            {
                Tele = false;
            }
            public void Disappear()
            {
                Tele = true;
            }
            public void Appear()
            {
                Tele = false;
            }
            void FixedUpdate()
            {
                if (enable)
                {
                    gameObject.GetComponent<SpriteRenderer>().color += new Color(0, 0, 0, (1f - gameObject.GetComponent<SpriteRenderer>().color.a) * Time.deltaTime * 3f);
                }
                else
                {
                    if(Tele)
                    {
                        gameObject.GetComponent<SpriteRenderer>().color += new Color(0, 0, 0, -1f) * Time.deltaTime;
                    }
                    else
                    {
                        gameObject.GetComponent<SpriteRenderer>().color += new Color(0, 0, 0, (0.3f - gameObject.GetComponent<SpriteRenderer>().color.a) * Time.deltaTime * 3f);
                    }
                }
            }
        }
        public class HaloScaleChange : MonoBehaviour
        {
            bool enable = false;
            bool quick = false;
            float factor = 1f;
            float flashfactor = 1f;
            public void Change(float animScaleFactor)
            {
                factor = animScaleFactor;
                enable = true;
            }
            public void Recover()
            {
                enable = false;
            }
            public void ShieldRecover()
            {
                flashfactor = 1.2f;
                quick = true;
                Invoke("QuickDone", 0.2f);
            }
            void QuickDone()
            {
                flashfactor = 1f;
                quick = false;
            }
            void FixedUpdate()
            {
                if (enable)
                {
                    gameObject.transform.localScale += (new Vector3(3f, 3f, 1) * factor - gameObject.transform.localScale) * Time.deltaTime * 3;
                }
                else
                {
                    if (quick)
                    {
                        gameObject.transform.localScale += (new Vector3(3f, 3f, 1) * flashfactor - gameObject.transform.localScale) * Time.deltaTime * 20;
                    }
                    else
                    {
                        gameObject.transform.localScale += (new Vector3(3f, 3f, 1) * flashfactor - gameObject.transform.localScale) * Time.deltaTime * 2.5f;
                    }
                }
            }
        }
        public class HaloRotateChange : MonoBehaviour
        {
            float SpinSpeed = 0f;
            bool enable = false;
            float factor = 1f;
            public void Change(float animSpeedFactor)
            {
                factor = animSpeedFactor;
                enable = true;
            }
            public void Recover()
            {
                enable = false;
            }
            void FixedUpdate()
            {
                if (enable)
                {
                    SpinSpeed += ((0.6f * factor) - SpinSpeed) * Time.deltaTime;
                }
                else
                {
                    SpinSpeed += (0.3f - SpinSpeed) * Time.deltaTime;
                }
                gameObject.transform.eulerAngles += new Vector3(0, 0, SpinSpeed);
            }
        }
        public class HaloAround : MonoBehaviour
        {
            float SIGHX => (random.Next(0, 2) * 2 - 1);
            float angle = 0f;
            float angleSpeed = 0f;
            float angleSpeedMax = 350f * Time.deltaTime;
            float DefaultangleSpeedMax = 350f * Time.deltaTime;
            float R = 0f;
            float RFactor = 0f;
            float side = 1f;
            Vector3 LocalPosition = new Vector3(-0.15f, 1.54f, 0.002f);
            Vector3 DefaultLocalPosition = new Vector3(-0.15f, 1.54f, 0.002f);
            Vector3 EyeLocalPosition = new Vector3(0f, 1f, 0f);
            bool enable = false;
            bool eye = false;
            public void SpinStart(float speedFactor, float rFactor)
            {
                angleSpeedMax = DefaultangleSpeedMax * speedFactor;
                RFactor = rFactor;
                side = SIGHX;
                enable = true;
                //angleSpeedAdd = 5400f * Time.deltaTime * speedFactor;
                //angleSpeedMax = 800f * Time.deltaTime * speedFactor;
            }
            public void SpinEnd()
            {
                enable = false;
                eye = false;
            }
            public void MoveToEye()
            {
                eye = true;
            }
            void FixedUpdate()
            {
                if (eye)
                {
                    LocalPosition += (DefaultLocalPosition + EyeLocalPosition - LocalPosition) * Time.deltaTime * 3.5f;
                }
                else
                {
                    LocalPosition += (DefaultLocalPosition - LocalPosition) * Time.deltaTime * 3.5f;
                }
                if (enable)
                {
                    angleSpeed += (angleSpeedMax - angleSpeed) * Time.deltaTime * 3.5f;
                    angle += side * angleSpeed;
                    R += ((8 * RFactor) - R) * Time.deltaTime * 5f;
                    Vector3 Halo1Position = LocalPosition + new Vector3((float)Math.Cos(DegreesToRadians(angle)) * R, (float)Math.Sin(DegreesToRadians(angle)) * R, 0);

                    HALO.transform.localPosition = Halo1Position;
                }
                else
                {
                    angleSpeed += (0 - angleSpeed) * Time.deltaTime * 3.5f;
                    angle += side * angleSpeed;
                    R += (0 - R) * Time.deltaTime * 5f;
                    Vector3 Halo1Position = LocalPosition + new Vector3((float)Math.Cos(DegreesToRadians(angle)) * R, (float)Math.Sin(DegreesToRadians(angle)) * R, 0);
                    HALO.transform.localPosition = Halo1Position;
                }
            }
        }
        public class HaloControl_Small:MonoBehaviour
        {
            float factor1 = 1;
            float factor2 = 1;
            float factor3 = 1;
            float RFactor = 1;
            bool glow = false;
            bool jump = false;
            bool eye = false;
            void Start()
            {
                if(HALO1 == null)
                {
                    HALO1 = Instantiate(ANOTHERHALO_prefab, BOSS.transform.Find("Halo").position, Quaternion.Euler(0, 0, 90), BOSS.transform);
                    HALO1.transform.localScale = new Vector3(0f, 0f, 1f);
                    HALO1.transform.localPosition = BOSS.transform.Find("Halo").localPosition + new Vector3(0, 0, -0.001f);
                    HALO1.GetComponent<SpriteRenderer>().color -= new Color(0, 0, 0, HALO1.GetComponent<SpriteRenderer>().color.a);
                    HALO1.name = "Halo1";
                    HALO1.AddComponent<HaloColorChange_Small>();
                    HALO1.AddComponent<HaloScaleChange_Small>();
                    HALO1.AddComponent<HaloRotateChange_Small>();
                    HALO1.AddComponent<HaloAround_Small>();
                    HALO1.AddComponent<AudioSource>();
                    HALO1.GetComponent<AudioSource>().minDistance = 40f;
                    HALO1.GetComponent<AudioSource>().playOnAwake = false;
                    HALO1.gameObject.LocateMyFSM("Fader").enabled = false;
                    HALO1.gameObject.LocateMyFSM("Rotation").enabled = false;
                    Texture2D halo1 = LoadPng("Sunset.Resources.Halo1.png");
                    HALO1.GetComponent<SpriteRenderer>().sprite = Sprite.Create(halo1, new Rect(0f, 0f, (float)halo1.width, (float)halo1.height), new Vector2(0.5f, 0.5f), HALO.GetComponent<SpriteRenderer>().sprite.pixelsPerUnit);
                    var glow = Instantiate(GLOW_Prefab, HALO1.transform.position, Quaternion.Euler(0, 0, 90), HALO1.transform);
                    glow.transform.localScale = new Vector3(0.5f, 0.5f, 1.5f);
                    glow.transform.localPosition += new Vector3(0f, 0f, -0.004f);
                    GLOW = glow;
                    Close();
                }
            }
            public void Control(float delayTime, float animScaleFactor, float animRotateFactor, float animAroundFactor, float rFactor, bool isGlow, bool isJump, bool MoveToEye)
            {
                factor1 = animScaleFactor;
                factor2 = animRotateFactor;
                factor3 = animAroundFactor;
                RFactor = rFactor;
                glow = isGlow;
                jump = isJump;
                eye = MoveToEye;
                Invoke("Begin", delayTime);
            }
            void Begin()
            {
                if (ShieldOn)
                {
                    if (glow)
                    {
                        HALO1.GetComponent<HaloColorChange_Small>().Glow();
                    }
                    if (jump)
                    {
                        HALO1.GetComponent<HaloRotateChange_Small>().Jump();
                    }
                    if (eye)
                    {
                        HALO1.GetComponent<HaloAround_Small>().MoveToEye();
                    }
                    HALO1.GetComponent<HaloColorChange_Small>().Change();
                    HALO1.GetComponent<HaloScaleChange_Small>().Change(factor1);
                    HALO1.GetComponent<HaloRotateChange_Small>().Change(factor2);
                    HALO1.GetComponent<HaloAround_Small>().SpinStart(factor3, RFactor);
                    //BOSS.GetComponent<BossShieldControl>().TempClose();
                }
            }
            public void ControlEnd(float delayTime)
            {
                Invoke("End", delayTime);
            }
            void End()
            {
                if (ShieldOn)
                {
                    HALO1.GetComponent<HaloColorChange_Small>().Recover();
                    HALO1.GetComponent<HaloScaleChange_Small>().Recover();
                    HALO1.GetComponent<HaloRotateChange_Small>().Recover();
                    HALO1.GetComponent<HaloAround_Small>().SpinEnd();
                    HALO1.GetComponent<HaloColorChange_Small>().GLowEnd();
                    //BOSS.GetComponent<BossShieldControl>().TempCloseEnd();
                }
            }
            public void Flash()
            {
                HALO1.GetComponent<HaloColorChange_Small>().Flash();
                HALO1.GetComponent<HaloScaleChange_Small>().Flash();
            }
            public void Close()
            {
                HALO1.GetComponent<HaloColorChange_Small>().Recover();
                HALO1.GetComponent<HaloScaleChange_Small>().Change(0);
                HALO1.GetComponent<HaloRotateChange_Small>().Change(0);
            }
            public void ReCharge()
            {
                HALO1.GetComponent<HaloColorChange_Small>().Recover();
                HALO1.GetComponent<HaloScaleChange_Small>().Recover();
                HALO1.GetComponent<HaloRotateChange_Small>().Recover();
                HALO1.GetComponent<HaloAround_Small>().SpinEnd();
            }
            public void Fade()
            {
            }
        }
        public class HaloColorChange_Small:MonoBehaviour
        {
            bool enable = false;
            bool quick = false;
            bool Tele = false;
            bool glow = false;
            public float defaultColora = 0.5f;
            public void Change()
            {
                enable = true;
            }
            public void Recover()
            {
                enable = false;
            }
            public void TeleChange()
            {
                Tele = true;
                Invoke("TeleChangeEnd", 0.5f);
            }
            void TeleChangeEnd()
            {
                Tele = false;
            }
            public void Disappear()
            {
                Tele = true;
            }
            public void Appear()
            {
                Tele = false;
            }
            void RecoverQuickly()
            {
                quick = true;
                enable = false;
                Invoke("QuickDone", 0.3f);
            }
            void QuickDone()
            {
                quick = false;
            }
            public void Flash()
            {
                if(!enable)
                {
                    gameObject.GetComponent<SpriteRenderer>().color += new Color(0, 0, 0, (1f - gameObject.GetComponent<SpriteRenderer>().color.a));
                    RecoverQuickly();
                }
            }
            public void Glow()
            {
                glow = true;
            }
            public void GLowEnd()
            {
                glow = false;
            }
            void FixedUpdate()
            {
                if (glow)
                {
                    GLOW.GetComponent<SpriteRenderer>().color += new Color(0, 0, 0, (1f - GLOW.GetComponent<SpriteRenderer>().color.a) * Time.deltaTime * 3f);
                }
                else
                {
                    GLOW.GetComponent<SpriteRenderer>().color += new Color(0, 0, 0, (0f - GLOW.GetComponent<SpriteRenderer>().color.a) * Time.deltaTime * 3f);
                }
                if (enable)
                {
                    gameObject.GetComponent<SpriteRenderer>().color += new Color(0, 0, 0, (1f - gameObject.GetComponent<SpriteRenderer>().color.a) * Time.deltaTime * 3f);
                }
                else
                {
                    if(Tele)
                    {
                        gameObject.GetComponent<SpriteRenderer>().color += new Color(0, 0, 0, -1f) * Time.deltaTime;
                    }
                    else
                    {
                        if (quick)
                        {
                            gameObject.GetComponent<SpriteRenderer>().color += new Color(0, 0, 0, (0.5f - gameObject.GetComponent<SpriteRenderer>().color.a) * Time.deltaTime * 8f);
                        }
                        else
                        {
                            gameObject.GetComponent<SpriteRenderer>().color += new Color(0, 0, 0, (defaultColora - gameObject.GetComponent<SpriteRenderer>().color.a) * Time.deltaTime * 3f);
                        }
                    }
                }
            }
        }
        public class HaloScaleChange_Small:MonoBehaviour
        {
            int num = 0;
            bool flag1 = true;
            bool flag2 = false;
            bool enable = false;
            bool quick = false;
            float factor = 1f;
            Vector3 flashScale;
            Vector3 lastScale;
            public void Change(float animScaleFactor)
            {
                factor = animScaleFactor;
                enable = true;
            }
            public void Recover()
            {
                enable = false;
            }
            public void Flash()
            {
                flashScale = gameObject.transform.localScale * 0.95f;
                quick = true;
                Invoke("QuickDone", 0.1f);
            }
            public void ShieldRecover()
            {
                flashScale = new Vector3(3f, 3f, 1);
                quick = true;
                Invoke("QuickDone", 0.2f);
            }
            void QuickDone()
            {
                quick = false;
            }
            void FixedUpdate()
            {
                num = 0;
                for (int i = 0;i <= 100;i++)
                {
                    if(gameObject.transform.localScale.x <= 2f + i * 0.05f)
                    {
                        flag1 = true;
                    }
                    else
                    {
                        flag1 = false;
                    }
                    if(lastScale.x <= 2f + i * 0.05f)
                    {
                        flag2 = true;
                    }
                    else
                    {
                        flag2 = false;
                    }
                    if(flag1 && !flag2)
                    {
                        num++;
                    }
                    if(!flag1 && flag2)
                    {
                        num++;
                    }
                }
                if(num >= 1)
                {
                    BOSS.GetComponent<Audios>().Play(2);
                }
                lastScale = gameObject.transform.localScale;
                if (enable)
                {
                    gameObject.transform.localScale += (new Vector3(3f, 3f, 1) * factor - gameObject.transform.localScale) * Time.deltaTime * 3;
                }
                else
                {
                    if (quick)
                    {
                        gameObject.transform.localScale += (flashScale - gameObject.transform.localScale) * Time.deltaTime * 30;
                    }
                    else
                    {
                        gameObject.transform.localScale += (new Vector3(3f, 3f, 1) - gameObject.transform.localScale) * Time.deltaTime * 3;
                    }
                }
            }
        }
        public class HaloRotateChange_Small:MonoBehaviour
        {
            float SpinSpeed = 0f;
            bool enable = false;
            bool quick = false;
            float factor = 1f;
            float jumpTimeFactor = 1f;
            float defaultAngle = 0f;
            float lastAngle = 0f;
            int SoundAngle = 20;
            void Start()
            {
                defaultAngle = gameObject.transform.eulerAngles.z;
            }
            public void Change(float animSpeedFactor)
            {
                factor = animSpeedFactor;
                enable = true;
            }
            public void Recover()
            {
                enable = false;
            }
            public void Flash()
            {
                quick = true;
                Invoke("QuickDone", 0.1f);
            }
            void QuickDone()
            {
                quick = false;
            }
            public void Jump()
            {
                jumpTimeFactor = 3;
                Invoke("JumpEnd", 0.2f);
            }
            void JumpEnd()
            {
                jumpTimeFactor = 1f;
            }
            void FixedUpdate()
            {
                lastAngle = gameObject.transform.eulerAngles.z;
                if (gameObject.transform.eulerAngles.z >= 360)
                {
                    gameObject.transform.eulerAngles += new Vector3(0, 0, -360);
                }
                if (gameObject.transform.eulerAngles.z <= -360)
                {
                    gameObject.transform.eulerAngles += new Vector3(0, 0, 360);
                }
                if (defaultAngle >= 360)
                {
                    defaultAngle += -360;
                }
                if (defaultAngle <= -360)
                {
                    defaultAngle += 360;
                }
                if (enable)
                {
                    if(quick)
                    {
                        SpinSpeed += ((-4f * factor * jumpTimeFactor) - SpinSpeed) * Time.deltaTime * 30f * jumpTimeFactor;
                    }    
                    else
                    {
                        SpinSpeed += ((-1f * factor * jumpTimeFactor) - SpinSpeed) * Time.deltaTime * 3f * jumpTimeFactor;
                    }
                }
                else
                {
                    SpinSpeed += ((-1f * 1) - SpinSpeed) * Time.deltaTime * 3f;
                }
                gameObject.transform.eulerAngles += new Vector3(0, 0, SpinSpeed);
                defaultAngle += -3f * Time.deltaTime;
                bool flag1;
                bool flag2;
                int num = 0;
                for (int i = -(360 / SoundAngle); i < (360 / SoundAngle); i++)
                {
                    if (gameObject.transform.eulerAngles.z >= defaultAngle + SoundAngle * i)
                    {
                        flag1 = true;
                    }
                    else
                    {
                        flag1 = false;
                    }
                    if (lastAngle >= defaultAngle + SoundAngle * i)
                    {
                        flag2 = true;
                    }
                    else
                    {
                        flag2 = false;
                    }
                    if (flag1 && !flag2)
                    {
                        num++;
                    }
                    if (!flag1 && flag2)
                    {
                        num++;
                    }
                }
                if (num >= 1)
                {
                    BOSS.GetComponent<Audios>().Play(3);
                }
            }
        }
        public class HaloRotateChange_SmallWorkshop : MonoBehaviour
        {
            float SpinSpeed = 0f;
            bool quick = false;
            float jumpTimeFactor = 1f;
            public float defaultSpeed = -0.5f;
            void QuickDone()
            {
                quick = false;
            }
            public void Jump()
            {
                quick = true;
                jumpTimeFactor = 3;
                Invoke("JumpEnd", 0.2f);
                Invoke("QuickDone", 0.2f);
            }
            void JumpEnd()
            {
                jumpTimeFactor = 1f;
            }
            void FixedUpdate()
            {
                if (quick)
                {
                    SpinSpeed += ((-4f * jumpTimeFactor) - SpinSpeed) * Time.deltaTime * 30f * jumpTimeFactor;
                }
                else
                {
                    SpinSpeed += ((defaultSpeed * 1) - SpinSpeed) * Time.deltaTime * 3f;
                }
                gameObject.transform.eulerAngles += new Vector3(0, 0, SpinSpeed);
            }
        }
        public class HaloColorChange_SmallWorkshop : MonoBehaviour
        {
            public float defaultColora = 0.5f;
            void FixedUpdate()
            {
                gameObject.GetComponent<SpriteRenderer>().color += new Color(0, 0, 0, (defaultColora - gameObject.GetComponent<SpriteRenderer>().color.a) * Time.deltaTime * 10f);
            }
        }
        public class HaloAround_Small : MonoBehaviour
        {
            float SIGHX => (random.Next(0, 2) * 2 - 1);
            public float angle = 0f;
            float angleSpeed = 0f;
            float angleSpeedMax = 350f * Time.deltaTime;
            float DefaultangleSpeedMax = 350f * Time.deltaTime;
            float R = 0f;
            float RFactor = 0f;
            float side = 1f;
            Vector3 LocalPosition = new Vector3(-0.15f, 1.54f, 0.002f);
            Vector3 DefaultLocalPosition = new Vector3(-0.15f, 1.54f, 0.002f);
            Vector3 EyeLocalPosition = new Vector3(0f, 1.0f, 0f);
            bool enable = false;
            bool eye = false;
            bool quick = false;
            bool lockHero = false;
            public void SpinStart(float speedFactor, float rFactor)
            {
                angleSpeedMax = DefaultangleSpeedMax * speedFactor;
                RFactor = rFactor;
                side = SIGHX;
                enable = true;
            }
            public void SpinEnd()
            {
                enable = false;
                eye = false;
            }
            public void MoveToEye()
            {
                eye = true;
            }
            public void LockHero(float endTime)
            {
                lockHero = true;
                Invoke("LockHeroEnd", endTime);
            }
            public void Flash()
            {
                quick = true;
                Invoke("QuickDone", 0.1f);
            }
            void QuickDone()
            {
                quick = false;
            }
            void LockHeroEnd()
            {
                lockHero = false;
            }
            void FixedUpdate()
            {
                if (eye)
                {
                    LocalPosition += (DefaultLocalPosition + EyeLocalPosition - LocalPosition) * Time.deltaTime * 3.5f;
                }
                else
                {
                    LocalPosition += (DefaultLocalPosition - LocalPosition) * Time.deltaTime * 3.5f;
                }
                if (enable)
                {
                    if (quick)
                    {
                        R += (3 - R) * Time.deltaTime * 40f;
                    }
                    else
                    {
                        R += ((8 * RFactor) - R) * Time.deltaTime * 5f;
                    }
                    if (lockHero)
                    {
                        if (BOSS.transform.position.x - HeroController.instance.gameObject.transform.position.x < 0)
                        {
                            angle = (float)RadiansToDegrees(Math.Atan((BOSS.transform.position.y + 2.5f - HeroController.instance.gameObject.transform.position.y) / (BOSS.transform.position.x - HeroController.instance.gameObject.transform.position.x))) + 180;
                        }
                        else
                        {
                            angle = (float)RadiansToDegrees(Math.Atan((BOSS.transform.position.y + 2.5f - HeroController.instance.gameObject.transform.position.y) / (BOSS.transform.position.x - HeroController.instance.gameObject.transform.position.x)));
                        }
                        Vector3 Halo1Position = LocalPosition + new Vector3((float)Math.Cos(DegreesToRadians(angle)) * R, (float)Math.Sin(DegreesToRadians(angle)) * R, 0);
                        HALO1.transform.localPosition = Halo1Position;
                    }
                    else
                    {
                        angleSpeed += (angleSpeedMax - angleSpeed) * Time.deltaTime * 3.5f;
                        angle += side * angleSpeed;
                        Vector3 Halo1Position = LocalPosition + new Vector3((float)Math.Cos(DegreesToRadians(angle)) * R, (float)Math.Sin(DegreesToRadians(angle)) * R, 0);
                        HALO1.transform.localPosition = Halo1Position;
                    }

                }
                else
                {
                    angleSpeed += (0 - angleSpeed) * Time.deltaTime * 3.5f;
                    angle += side * angleSpeed;
                    R += (0 - R) * Time.deltaTime * 5f;
                    Vector3 Halo1Position = LocalPosition + new Vector3((float)Math.Cos(DegreesToRadians(angle)) * R, (float)Math.Sin(DegreesToRadians(angle)) * R, 0);
                    HALO1.transform.localPosition = Halo1Position;
                }
            }
        }

        public class BossSkillChoice : MonoBehaviour
        {
            private static int[] OrbChoice = { 1, 2, 3, 4 };
            private static int[] BeamChoice = { 1, 2, 3, 4, 5 };
            private static int[] NailChoice = { 1, 2, 3, 4, 5 };
            int[] shuffledArray1 = OrbChoice;
            int[] shuffledArray2 = BeamChoice;
            int[] shuffledArray3 = NailChoice;
            int numOrb = 10;
            int numBeam = 10;
            int numNail = 10;
            public void OrbSkill()
            {
                numOrb++;
                if (numOrb >= 4)
                {
                    shuffledArray1 = OrbChoice.OrderBy(x => Guid.NewGuid()).ToArray();
                    numOrb = 0;
                }
                switch (shuffledArray1[numOrb])
                {
                    case 1:
                        {
                            if (HardMode == 0)
                            {
                                BOSS.GetComponent<SkillsWaitTimeChange>().ChangeOrb(3f);
                                HaloScaleFactor = 1.3f + RX * 0.3f;
                                float HaloAroundFactor = 1.4f + RY * 0.5f;
                                BOSS.GetComponent<OrbSkill3>().On(HaloScaleFactor / 1.4f, 2.6f, 0.25f);
                                BOSS.GetComponent<HaloControl>().Control(0.3f, HaloScaleFactor / 1.45f, 1.5f, false);
                                BOSS.GetComponent<HaloControl>().ControlEnd(3);
                                BOSS.GetComponent<HaloControl_Small>().Control(0f, 0.8f, 4f, HaloAroundFactor, HaloScaleFactor, false, true, false);
                                BOSS.GetComponent<HaloControl_Small>().ControlEnd(3f);
                            }
                            else
                            {
                                BOSS.GetComponent<SkillsWaitTimeChange>().ChangeOrb(3f);
                                HaloScaleFactor = 1.6f + RX * 0.3f;
                                float HaloAroundFactor = 1.7f + RY * 0.6f;
                                BOSS.GetComponent<OrbSkill3>().On(HaloScaleFactor / 1.5f, 2.6f, 0.15f);
                                BOSS.GetComponent<HaloControl>().Control(0.3f, HaloScaleFactor / 1.45f, 1.5f, false);
                                BOSS.GetComponent<HaloControl>().ControlEnd(3);
                                BOSS.GetComponent<HaloControl_Small>().Control(0f, 0.8f, 4f, HaloAroundFactor, HaloScaleFactor, false, true, false);
                                BOSS.GetComponent<HaloControl_Small>().ControlEnd(3f);
                            }
                            return;
                        }
                    case 2:
                        {

                            if (HardMode == 0)
                            {
                                float time = 2f;
                                BOSS.GetComponent<SkillsWaitTimeChange>().ChangeOrb(time);
                                BOSS.GetComponent<OrbSkill1>().On(9, 0.15f);
                                BOSS.GetComponent<HaloControl>().Control(0, 0.8f, 1.2f, false);
                                BOSS.GetComponent<HaloControl>().ControlEnd(time);
                                BOSS.GetComponent<HaloControl_Small>().Control(0f, 1.1f, 3f, 0f, 0f, false, true, false);
                                BOSS.GetComponent<HaloControl_Small>().ControlEnd(time);
                            }
                            else
                            {
                                float time = 2.5f;
                                BOSS.GetComponent<SkillsWaitTimeChange>().ChangeOrb(time);
                                BOSS.GetComponent<OrbSkill1>().On(15, 0.15f);
                                BOSS.GetComponent<HaloControl>().Control(0, 0.8f, 1.2f, false);
                                BOSS.GetComponent<HaloControl>().ControlEnd(time);
                                BOSS.GetComponent<HaloControl_Small>().Control(0f, 1.1f, 3f, 0f, 0f, false, true, false);
                                BOSS.GetComponent<HaloControl_Small>().ControlEnd(time);
                            }
                            return;
                        }
                    case 3:
                        {
                            if (HardMode == 0)
                            {
                                BOSS.GetComponent<SkillsWaitTimeChange>().ChangeOrb(2.5f);
                                HaloScaleFactor = 1.4f + RX * 0.3f;
                                float HaloAroundFactor = 1.5f + RY * 0.3f;
                                BOSS.GetComponent<OrbSkill2>().On();
                                BOSS.GetComponent<HaloControl>().Control(0.3f, HaloScaleFactor / 1.45f, 1.5f, false);
                                BOSS.GetComponent<HaloControl>().ControlEnd(2.5f);
                                BOSS.GetComponent<HaloControl_Small>().Control(0f, 1.2f, 4f, HaloAroundFactor, HaloScaleFactor, false, true, false);
                                BOSS.GetComponent<HaloControl_Small>().ControlEnd(2.5f);
                            }
                            else
                            {
                                BOSS.GetComponent<SkillsWaitTimeChange>().ChangeOrb(2.5f);
                                HaloScaleFactor = 1.6f + RX * 0.3f;
                                float HaloAroundFactor = 2f + RY * 0.3f;
                                BOSS.GetComponent<OrbSkill2>().On();
                                BOSS.GetComponent<HaloControl>().Control(0.3f, HaloScaleFactor / 1.45f, 1.5f, false);
                                BOSS.GetComponent<HaloControl>().ControlEnd(2.5f);
                                BOSS.GetComponent<HaloControl_Small>().Control(0f, 1.2f, 4f, HaloAroundFactor, HaloScaleFactor, false, true, false);
                                BOSS.GetComponent<HaloControl_Small>().ControlEnd(2.5f);
                            }
                            return;
                        }
                    case 4:
                        {
                            if (HardMode == 0)
                            {
                                OrbSkill();
                                /*
                                BOSS.GetComponent<SkillsWaitTimeChange>().ChangeOrb(5.4f);
                                BOSS.GetComponent<OrbSkill4>().On(0.075f, 5.4f);
                                BOSS.GetComponent<HaloControl>().Control(0f, 1.2f, 1.5f, true);
                                BOSS.GetComponent<HaloControl>().ControlEnd(5.4f);
                                BOSS.GetComponent<HaloControl_Small>().Control(0f, 1.75f, 6f, 0f, 0f, true, false, true);
                                BOSS.GetComponent<HaloControl_Small>().ControlEnd(5.4f);
                                */
                            }
                            else
                            {
                                BOSS.GetComponent<SkillsWaitTimeChange>().ChangeOrb(5.4f);
                                BOSS.GetComponent<OrbSkill4>().On(0.05f, 5.4f);
                                BOSS.GetComponent<HaloControl>().Control(0f, 1.2f, 1.5f, true);
                                BOSS.GetComponent<HaloControl>().ControlEnd(5.4f);
                                BOSS.GetComponent<HaloControl_Small>().Control(0f, 1.75f, 6f, 0f, 0f, true, false, true);
                                BOSS.GetComponent<HaloControl_Small>().ControlEnd(5.4f);
                            }
                            return;
                        }
                }
            }
            public void BeamSkill()
            {
                numBeam++;
                if (numBeam >= 5)
                {
                    shuffledArray2 = BeamChoice.OrderBy(x => Guid.NewGuid()).ToArray();
                    numBeam = 0;
                }
                switch (shuffledArray2[numBeam])
                {
                    case 1:
                        {
                            if (HardMode == 0)
                            {
                                BOSS.GetComponent<SkillsWaitTimeChange>().ChangeBeam(3f);
                                BOSS.GetComponent<BeamSkill3>().On(3.5f, 2.7f, 3f);
                                HaloScaleFactor = 0f;
                                float HaloAroundFactor = 0f;
                                BOSS.GetComponent<HaloControl>().Control(0f, 0.75f, 1.5f, true);
                                BOSS.GetComponent<HaloControl>().ControlEnd(3);
                                BOSS.GetComponent<HaloControl_Small>().Control(0f, 1.5f, 6f, 0f, 0f, true, false, true);
                                BOSS.GetComponent<HaloControl_Small>().ControlEnd(3f);
                            }
                            else
                            {
                                BOSS.GetComponent<SkillsWaitTimeChange>().ChangeBeam(4f);
                                BOSS.GetComponent<BeamSkill3>().On(5.1f, 3.5f, 4f);
                                HaloScaleFactor = 0f;
                                float HaloAroundFactor = 0f;
                                BOSS.GetComponent<HaloControl>().Control(0f, 0.75f, 1.5f, true);
                                BOSS.GetComponent<HaloControl>().ControlEnd(4);
                                BOSS.GetComponent<HaloControl_Small>().Control(0f, 1.75f, 6f, 0f, 0f, true, false, true);
                                BOSS.GetComponent<HaloControl_Small>().ControlEnd(4f);
                            }
                            return;
                        }
                    case 2:
                        {

                            if (HardMode == 0)
                            {
                                BOSS.GetComponent<SkillsWaitTimeChange>().ChangeBeam(4f);
                                BOSS.GetComponent<SolarFlareSkill>().On(0.045f, 3f);
                                HaloScaleFactor = 0f;
                                float HaloAroundFactor = 0f;
                                BOSS.GetComponent<HaloControl>().Control(0f, 0.75f, 1.5f, true);
                                BOSS.GetComponent<HaloControl>().ControlEnd(4f);
                                BOSS.GetComponent<HaloControl_Small>().Control(0f, 1.2f, 3f, HaloAroundFactor, HaloScaleFactor, true, true, true);
                                BOSS.GetComponent<HaloControl_Small>().ControlEnd(4f);
                            }
                            else
                            {
                                BOSS.GetComponent<SkillsWaitTimeChange>().ChangeBeam(4f);
                                BOSS.GetComponent<SolarFlareSkill>().On(0.03f, 3f);
                                HaloScaleFactor = 0f;
                                float HaloAroundFactor = 0f;
                                BOSS.GetComponent<HaloControl>().Control(0f, 0.75f, 1.5f, true);
                                BOSS.GetComponent<HaloControl>().ControlEnd(4f);
                                BOSS.GetComponent<HaloControl_Small>().Control(0f, 1.2f, 3f, HaloAroundFactor, HaloScaleFactor, true, true, true);
                                BOSS.GetComponent<HaloControl_Small>().ControlEnd(4f);
                            }
                            return;
                        }
                    case 3:
                        {
                            if (HardMode == 0)
                            {
                                BOSS.GetComponent<SkillsWaitTimeChange>().ChangeNail(3.3f);
                                BOSS.GetComponent<BeamSkill2>().On(1.1f, 0.8f);
                                HaloScaleFactor = 1.2f;
                                float HaloAroundFactor = 0.5f + RY * 0.2f;
                                BOSS.GetComponent<HaloControl>().Control(0f, HaloScaleFactor / 1.45f, 1.5f, false);
                                BOSS.GetComponent<HaloControl>().ControlEnd(3);
                                BOSS.GetComponent<HaloControl_Small>().Control(0f, 0.8f, 6f, HaloAroundFactor, HaloScaleFactor, true, true, false);
                                BOSS.GetComponent<HaloControl_Small>().ControlEnd(3.3f);
                            }
                            else
                            {
                                BOSS.GetComponent<SkillsWaitTimeChange>().ChangeNail(3.3f);
                                BOSS.GetComponent<BeamSkill2>().On(1.4f, 1.1f);
                                HaloScaleFactor = 1.2f;
                                float HaloAroundFactor = 0.8f + RY * 0.3f;
                                BOSS.GetComponent<HaloControl>().Control(0f, HaloScaleFactor / 1.45f, 1.5f, false);
                                BOSS.GetComponent<HaloControl>().ControlEnd(3);
                                BOSS.GetComponent<HaloControl_Small>().Control(0f, 0.8f, 6f, HaloAroundFactor, HaloScaleFactor, true, true, false);
                                BOSS.GetComponent<HaloControl_Small>().ControlEnd(3.3f);
                            }
                            return;
                        }
                    case 4:
                        {
                            if (HardMode == 0)
                            {
                                float time = 3.3f;
                                BOSS.GetComponent<SkillsWaitTimeChange>().ChangeOrb(time);
                                BOSS.GetComponent<BeamSkill6>().On(0.25f, time, 1f, true);
                                HaloScaleFactor = 0f;
                                float HaloAroundFactor = 0f;
                                BOSS.GetComponent<HaloControl>().Control(0f, 0.75f, 1.5f, true);
                                BOSS.GetComponent<HaloControl>().ControlEnd(time);
                                BOSS.GetComponent<HaloControl_Small>().Control(0f, 1.2f, 3f, HaloAroundFactor, HaloScaleFactor, true, true, true);
                                BOSS.GetComponent<HaloControl_Small>().ControlEnd(time);
                            }
                            else
                            {
                                float time = 3.3f;
                                BOSS.GetComponent<SkillsWaitTimeChange>().ChangeOrb(time);
                                BOSS.GetComponent<BeamSkill6>().On(0.15f, time, 1.25f, true);
                                HaloScaleFactor = 0f;
                                float HaloAroundFactor = 0f;
                                BOSS.GetComponent<HaloControl>().Control(0f, 0.75f, 1.5f, true);
                                BOSS.GetComponent<HaloControl>().ControlEnd(time);
                                BOSS.GetComponent<HaloControl_Small>().Control(0f, 1.2f, 3f, HaloAroundFactor, HaloScaleFactor, true, true, true);
                                BOSS.GetComponent<HaloControl_Small>().ControlEnd(time);
                            }
                            return;
                        }
                    case 5:
                        {
                            if (HardMode == 0)
                            {
                                BOSS.GetComponent<SkillsWaitTimeChange>().ChangeBeam(3.3f);
                                BOSS.GetComponent<BeamSkill4>().On(0);
                                HaloScaleFactor = 1.4f;
                                float HaloAroundFactor = 0.5f + RY * 0.2f;
                                BOSS.GetComponent<HaloControl>().Control(0f, HaloScaleFactor / 1.45f, 1.5f, false);
                                BOSS.GetComponent<HaloControl>().ControlEnd(3);
                                BOSS.GetComponent<HaloControl_Small>().Control(0f, 0.8f, 6f, HaloAroundFactor, HaloScaleFactor, true, true, false);
                                BOSS.GetComponent<HaloControl_Small>().ControlEnd(3.3f);
                            }
                            else
                            {
                                BOSS.GetComponent<SkillsWaitTimeChange>().ChangeBeam(3.3f);
                                BOSS.GetComponent<BeamSkill4>().On(2);
                                HaloScaleFactor = 1.4f;
                                float HaloAroundFactor = 0.5f + RY * 0.2f;
                                BOSS.GetComponent<HaloControl>().Control(0f, HaloScaleFactor / 1.45f, 1.5f, false);
                                BOSS.GetComponent<HaloControl>().ControlEnd(3);
                                BOSS.GetComponent<HaloControl_Small>().Control(0f, 0.8f, 6f, HaloAroundFactor, HaloScaleFactor, true, true, false);
                                BOSS.GetComponent<HaloControl_Small>().ControlEnd(3.3f);
                            }
                            return;
                        }
                    case 6:
                        {
                            if (HardMode == 0)
                            {
                                BOSS.GetComponent<SkillsWaitTimeChange>().ChangeBeam(3f);
                                BOSS.GetComponent<BeamSkill1>().On(1.15f);
                                HaloScaleFactor = 0f;
                                float HaloAroundFactor = 0f;
                                BOSS.GetComponent<HaloControl>().Control(0f, 0.75f, 1.5f, true);
                                BOSS.GetComponent<HaloControl>().ControlEnd(3);
                                BOSS.GetComponent<HaloControl_Small>().Control(0f, 1.2f, 3f, HaloAroundFactor, HaloScaleFactor, true, true, true);
                                BOSS.GetComponent<HaloControl_Small>().ControlEnd(3f);
                            }
                            else
                            {
                                BOSS.GetComponent<SkillsWaitTimeChange>().ChangeBeam(3f);
                                BOSS.GetComponent<BeamSkill1>().On(1f);
                                HaloScaleFactor = 0f;
                                float HaloAroundFactor = 0f;
                                BOSS.GetComponent<HaloControl>().Control(0f, 0.75f, 1.5f, true);
                                BOSS.GetComponent<HaloControl>().ControlEnd(3);
                                BOSS.GetComponent<HaloControl_Small>().Control(0f, 1.2f, 3f, HaloAroundFactor, HaloScaleFactor, true, true, true);
                                BOSS.GetComponent<HaloControl_Small>().ControlEnd(3f);
                            }
                            return;
                        }
                }
            }
            public void NailSkill()
            {
                numNail++;
                if (numNail >= 5)
                {
                    shuffledArray3 = NailChoice.OrderBy(x => Guid.NewGuid()).ToArray();
                    numNail = 0;
                }
                switch (shuffledArray3[numNail])
                {
                    case 1:
                        {
                            if (HardMode == 0)
                            {
                                BOSS.GetComponent<SkillsWaitTimeChange>().ChangeNail(3.5f);
                                BOSS.GetComponent<NailSkill1>().On(0.06f, 0.04f);
                                BOSS.GetComponent<HaloControl>().Control(0f, 1f, 1.5f, false);
                                BOSS.GetComponent<HaloControl>().ControlEnd(3.5f);
                                BOSS.GetComponent<HaloControl_Small>().Control(0f, 1f, 3f, 1f, 0f, false, true, false);
                                BOSS.GetComponent<HaloControl_Small>().ControlEnd(3.5f);
                            }
                            else
                            {
                                BOSS.GetComponent<SkillsWaitTimeChange>().ChangeNail(2.4f);
                                BOSS.GetComponent<NailSkill1>().On(0.045f, 0.03f);
                                BOSS.GetComponent<HaloControl>().Control(0f, 1f, 1.5f, false);
                                BOSS.GetComponent<HaloControl>().ControlEnd(2.4f);
                                BOSS.GetComponent<HaloControl_Small>().Control(0f, 1f, 3f, 1f, 0f, false, true, false);
                                BOSS.GetComponent<HaloControl_Small>().ControlEnd(2.4f);
                            }
                            return;
                        }
                    case 2:
                        {
                            if (HardMode == 0)
                            {
                                BOSS.GetComponent<SkillsWaitTimeChange>().ChangeNail(3.3f);
                                BOSS.GetComponent<NailSkill6>().On();
                                HaloScaleFactor = 0f;
                                float HaloAroundFactor = 0f;
                                BOSS.GetComponent<HaloControl>().Control(0f, 0.9f / 1.45f, 1.5f, false);
                                BOSS.GetComponent<HaloControl>().ControlEnd(5f);
                                BOSS.GetComponent<HaloControl_Small>().Control(0f, 0.8f, 1f - HaloAroundFactor, HaloAroundFactor, HaloScaleFactor, true, true, true);
                                BOSS.GetComponent<HaloControl_Small>().ControlEnd(3.3f);
                            }
                            else
                            {
                                BOSS.GetComponent<SkillsWaitTimeChange>().ChangeNail(3.3f);
                                BOSS.GetComponent<NailSkill6_H>().On();
                                HaloScaleFactor = 0f;
                                float HaloAroundFactor = 0f;
                                BOSS.GetComponent<HaloControl>().Control(0f, 0.9f / 1.45f, 1.5f, false);
                                BOSS.GetComponent<HaloControl>().ControlEnd(5f);
                                BOSS.GetComponent<HaloControl_Small>().Control(0f, 0.8f, 1f - HaloAroundFactor, HaloAroundFactor, HaloScaleFactor, true, true, true);
                                BOSS.GetComponent<HaloControl_Small>().ControlEnd(3.3f);
                            }
                            return;
                        }
                    case 3:
                        {
                            if (HardMode == 0)
                            {
                                BOSS.GetComponent<SkillsWaitTimeChange>().ChangeNail(3f);
                                HaloScaleFactor = 1.3f + RX * 0.4f;
                                float HaloAroundFactor = 1f + RY * 0.4f;
                                BOSS.GetComponent<NailSkill3>().On(0.25f, 3.5f);
                                BOSS.GetComponent<HaloControl>().Control(0f, HaloScaleFactor / 1.45f, 1.5f, false);
                                BOSS.GetComponent<HaloControl>().ControlEnd(3);
                                BOSS.GetComponent<HaloControl_Small>().Control(0f, 0.8f, 4f, HaloAroundFactor, HaloScaleFactor, false, true, false);
                                BOSS.GetComponent<HaloControl_Small>().ControlEnd(3f);
                            }
                            else
                            {
                                BOSS.GetComponent<SkillsWaitTimeChange>().ChangeNail(3f);
                                HaloScaleFactor = 1.3f + RX * 0.4f;
                                float HaloAroundFactor = 1f + RY * 0.4f;
                                BOSS.GetComponent<NailSkill3>().On(0.1f, 3.5f);
                                BOSS.GetComponent<HaloControl>().Control(0f, HaloScaleFactor / 1.45f, 1.5f, false);
                                BOSS.GetComponent<HaloControl>().ControlEnd(3);
                                BOSS.GetComponent<HaloControl_Small>().Control(0f, 0.8f, 4f, HaloAroundFactor, HaloScaleFactor, false, true, false);
                                BOSS.GetComponent<HaloControl_Small>().ControlEnd(3f);
                            }
                            return;
                        }
                    case 4:
                        {
                            if (HardMode == 0)
                            {
                                BOSS.GetComponent<SkillsWaitTimeChange>().ChangeNail(3.5f);
                                HaloScaleFactor = 1.3f + RX * 0.4f;
                                float HaloAroundFactor = 1f + RY * 0.4f;
                                BOSS.GetComponent<NailSkill4>().On(0.3f, 3.5f);
                                BOSS.GetComponent<HaloControl>().Control(0f, 1.2f / 1.45f, 1.5f, false);
                                BOSS.GetComponent<HaloControl>().ControlEnd(3.5f);
                                BOSS.GetComponent<HaloControl_Small>().Control(0f, 0.8f, 4f, HaloAroundFactor, HaloScaleFactor, false, true, false);
                                BOSS.GetComponent<HaloControl_Small>().ControlEnd(3.5f);
                            }
                            else
                            {
                                BOSS.GetComponent<SkillsWaitTimeChange>().ChangeNail(3.5f);
                                HaloScaleFactor = 1.3f + RX * 0.4f;
                                float HaloAroundFactor = 1f + RY * 0.4f;
                                BOSS.GetComponent<NailSkill4>().On(0.15f, 3.5f);
                                BOSS.GetComponent<HaloControl>().Control(0f, 1.2f / 1.45f, 1.5f, false);
                                BOSS.GetComponent<HaloControl>().ControlEnd(3.5f);
                                BOSS.GetComponent<HaloControl_Small>().Control(0f, 0.8f, 4f, HaloAroundFactor, HaloScaleFactor, false, true, false);
                                BOSS.GetComponent<HaloControl_Small>().ControlEnd(3.5f);
                            }
                            return;
                        }
                    case 5:
                        {
                            if (HardMode == 0)
                            {
                                BOSS.GetComponent<SkillsWaitTimeChange>().ChangeNail(3f);
                                HaloScaleFactor = 0f;
                                float HaloAroundFactor = 0f;
                                BOSS.GetComponent<NailSkill5>().On(0.045f, 3f);
                                BOSS.GetComponent<HaloControl>().Control(0.3f, 1.2f / 1.45f, 1.5f, false);
                                BOSS.GetComponent<HaloControl>().ControlEnd(3);
                                BOSS.GetComponent<HaloControl_Small>().Control(0f, 0.8f, 4f, HaloAroundFactor, HaloScaleFactor, false, true, false);
                                BOSS.GetComponent<HaloControl_Small>().ControlEnd(3f);
                            }
                            else
                            {
                                BOSS.GetComponent<SkillsWaitTimeChange>().ChangeNail(3f);
                                HaloScaleFactor = 0f;
                                float HaloAroundFactor = 0f;
                                BOSS.GetComponent<NailSkill5>().On(0.032f, 3f);
                                BOSS.GetComponent<HaloControl>().Control(0.3f, 1.2f / 1.45f, 1.5f, false);
                                BOSS.GetComponent<HaloControl>().ControlEnd(3);
                                BOSS.GetComponent<HaloControl_Small>().Control(0f, 0.8f, 4f, HaloAroundFactor, HaloScaleFactor, false, true, false);
                                BOSS.GetComponent<HaloControl_Small>().ControlEnd(3f);
                            }
                            return;
                        }
                }
            }
            public void NailSweepRP1()
            {
                float num1 = RX;
                float num2 = RY;
                if (HardMode == 0)
                {
                    if (num1 <= 0)
                    {
                        BOSS.GetComponent<NailSkill5>().On(0.06f, 1.6f);
                    }
                    else
                    {
                        BOSS.GetComponent<CombR>().On(2f, 0.2f, 20, 1f);
                        if (num2 <= -0.2f)
                        {
                            BOSS.GetComponent<BeamSkill6>().On(0.4f, 1.4f, 1f, false);
                        }
                        else if(num2 <= 0.6f)
                        {
                            BOSS.GetComponent<BeamSight_1>().On();
                        }
                        else
                        {
                            BOSS.GetComponent<BeamSkill5>().On(0);
                        }
                    }

                }
                else
                {
                    if (num1 <= 0)
                    {
                        BOSS.GetComponent<NailSkill5>().On(0.05f, 2f);
                    }
                    else
                    {
                        BOSS.GetComponent<CombR>().On(1.5f, 0.2f, 20, 1.2f);
                        if (RX >= 0)
                        {
                            BOSS.GetComponent<BeamSkill6>().On(0.5f, 1.4f, 1f, false);
                        }
                        else if (num2 <= 0.6f)
                        {
                            BOSS.GetComponent<BeamSight_1>().On();
                        }
                        else
                        {
                            BOSS.GetComponent<BeamSkill5>().On(2);
                        }
                    }
                }
            }
            public void NailSweepLP1()
            {
                float num1 = RX;
                float num2 = RY;
                if (HardMode == 0)
                {
                    if (num1 <= 0)
                    {
                        BOSS.GetComponent<NailSkill5>().On(0.06f, 1.6f);
                    }
                    else
                    {
                        BOSS.GetComponent<CombL>().On(2f, 0.2f, 20, 1f);
                        if (num2 <= -0.2f)
                        {
                            BOSS.GetComponent<BeamSkill6>().On(0.4f, 1.4f, 1f, false);
                        }
                        else if(num2 <= 1.6f)
                        {
                            BOSS.GetComponent<BeamSight_1>().On();
                        }
                        else
                        {
                            BOSS.GetComponent<BeamSkill5>().On(0);
                        }
                    }

                }
                else
                {
                    if (num1 <= 0)
                    {
                        BOSS.GetComponent<NailSkill5>().On(0.05f, 2f);
                    }
                    else
                    {
                        BOSS.GetComponent<CombL>().On(1.5f, 0.2f, 20, 1.2f);
                        if (RX >= 0)
                        {
                            BOSS.GetComponent<BeamSkill6>().On(0.5f, 1.4f, 1f, false);
                        }
                        else if (num2 <= 1.6f)
                        {
                            BOSS.GetComponent<BeamSight_1>().On();
                        }
                        else
                        {
                            BOSS.GetComponent<BeamSkill5>().On(2);
                        }
                    }
                }
            }
        }
        /*
        public class HaloControl1 : MonoBehaviour
        {
            void Start()
            {
            }
            public void On(float speedFactor1, float scaleFactor1, float speedFactor2, float scaleFactor2, float speedFactor3, float scaleFactor3, float r1, float r2, float r3)
            {
                if (HaloConut == 1)
                {
                    HALO1.GetComponent<HaloReduce1>().ReduceStart(scaleFactor1);
                    HALO1.GetComponent<HaloRotate1>().SpinStart();
                    HALO1.GetComponent<HaloAround1>().SpinStart(speedFactor1, r1);
                }
                if (HaloConut == 2)
                {
                    HALO1.GetComponent<HaloReduce1>().ReduceStart(scaleFactor1);
                    HALO1.GetComponent<HaloRotate1>().SpinStart();
                    HALO1.GetComponent<HaloAround1>().SpinStart(speedFactor1, r1);
                    HALO2.GetComponent<HaloReduce1>().ReduceStart(scaleFactor2);
                    HALO2.GetComponent<HaloRotate1>().SpinStart();
                    //HALO2.GetComponent<HaloAround1>().SpinStart(speedFactor2, r2);
                }
                if (HaloConut == 3)
                {
                    HALO1.GetComponent<HaloReduce1>().ReduceStart(scaleFactor1);
                    HALO1.GetComponent<HaloRotate1>().SpinStart();
                    HALO1.GetComponent<HaloAround1>().SpinStart(speedFactor1, r1);
                    HALO2.GetComponent<HaloReduce1>().ReduceStart(scaleFactor2);
                    HALO2.GetComponent<HaloRotate1>().SpinStart();
                    //HALO2.GetComponent<HaloAround1>().SpinStart(speedFactor2, r2);
                    HALO3.GetComponent<HaloReduce1>().ReduceStart(scaleFactor3);
                    HALO3.GetComponent<HaloRotate1>().SpinStart();
                    //HALO3.GetComponent<HaloAround1>().SpinStart(speedFactor3, r3);
                }
            }
            public void Off()
            {
                HALO1.GetComponent<HaloReduce1>().ReduceEnd();
                HALO1.GetComponent<HaloRotate1>().SpinEnd();
                HALO1.GetComponent<HaloAround1>().SpinEnd();
                HALO2.GetComponent<HaloReduce1>().ReduceEnd();
                HALO2.GetComponent<HaloRotate1>().SpinEnd();
                //HALO2.GetComponent<HaloAround1>().SpinEnd();
                HALO3.GetComponent<HaloReduce1>().ReduceEnd();
                HALO3.GetComponent<HaloRotate1>().SpinEnd();
                //HALO3.GetComponent<HaloAround1>().SpinEnd();
            }
        }
        public class HaloRotate1 : MonoBehaviour
        {
            public float SIGHX => (random.Next(0, 2) * 2 - 1);
            float SpinSpeed = -4f;
            float SpinSpeedMax = -8f;
            float SpinSpeedMin = -3f;
            bool enable = false;
            public float side = 1;
            
            public void SpinStart()
            {
                enable = true;
                side = SIGHX;
            }
            public void SpinEnd()
            {
                enable = false;
            }
            void FixedUpdate()
            {
                if (enable)
                {
                    gameObject.GetComponent<SpriteRenderer>().color += new Color(0, 0, 0, (1f - gameObject.GetComponent<SpriteRenderer>().color.a) * Time.deltaTime * 2.5f);
                    SpinSpeed += (SpinSpeedMin - SpinSpeed) * Time.deltaTime;
                }
                else
                {
                    gameObject.GetComponent<SpriteRenderer>().color += new Color(0, 0, 0, (0f - gameObject.GetComponent<SpriteRenderer>().color.a) * Time.deltaTime * 2.5f);
                    SpinSpeed += (SpinSpeedMax - SpinSpeed) * Time.deltaTime;
                }
                gameObject.transform.eulerAngles += new Vector3(0, 0, side * SpinSpeed);
            }
        }
        public class HaloReduce1 : MonoBehaviour
        {
            bool enable = false;
            float factor = 0.75f;
            public void ReduceStart(float scaleFactor)
            {
                factor = scaleFactor;
                enable = true;
            }
            public void ReduceEnd()
            {
                enable = false;
            }
            void FixedUpdate()
            {
                if (enable)
                {
                    gameObject.transform.localScale += (new Vector3(2.6f, 2.6f, 1) * factor - gameObject.transform.localScale) * Time.deltaTime * 2;
                }
                else
                {
                    gameObject.transform.localScale += (new Vector3(2.6f, 2.6f, 1) - gameObject.transform.localScale) * Time.deltaTime * 2;
                }
            }
        }
        */
        public class BossShieldControl : MonoBehaviour
        {
            public int nextHp;
            public bool finalStart = false;
            GameObject blackKnight;

            void Start()
            {
                ShieldNeedRecharge = true;
                ChargePt = Instantiate(DREAMPTCHARG, gameObject.transform.position + new Vector3(0, 2f, 0), new Quaternion(), gameObject.transform);
                ChargePt.gameObject.SetActive(true);
                WAVE = Instantiate(WAVE_Prefab, gameObject.transform.position + new Vector3(0, 2f, 0), new Quaternion(), gameObject.transform);
                WAVE.transform.Find("wave 2").gameObject.SetActive(false);
                //Invoke("ReChargAnim", 2f);
                blackKnight = Instantiate(BLACKKNIGHT_Prefab, gameObject.transform.position, new Quaternion(), gameObject.transform);
                blackKnight.GetComponent<MeshRenderer>().enabled = false;
                blackKnight.GetComponent<tk2dSprite>().enabled = false;
                blackKnight.GetComponent<tk2dSpriteAnimator>().enabled = false;
                blackKnight.GetComponent<SetZ>().enabled = false;
                blackKnight.GetComponent<PlayMakerFixedUpdate>().enabled = false;
                blackKnight.GetComponent<BoxCollider2D>().enabled = false;
                blackKnight.GetComponent<HealthManager>().enabled = false;
                blackKnight.GetComponent<DamageHero>().enabled = false;
                foreach(Transform tra in blackKnight.transform)
                {
                    tra.gameObject.Recycle();
                }
            }
            public void FinalRecharge()
            {
                On();
                nextHp = 720;
                if (HardMode == 0)
                {
                    BOSS.GetComponent<HealthManager>().hp = 1020 + GameManager.instance.playerData.nailDamage * 30;
                }
                else
                {
                    BOSS.GetComponent<HealthManager>().hp = 1020 + GameManager.instance.playerData.nailDamage * 40;
                }
                HALO1.GetComponent<HaloScaleChange_Small>().ShieldRecover();
                ShieldNeedRecharge = false;
            }
            public void ReChargAnim()
            {
                Invoke("PtStart", 0f);
                Invoke("PtEnd", 0.5f);
                Invoke("ReCharge", 1f);
            }
            public void ReChargAnimRage()
            {
                Invoke("PtStart", 0f);
                Invoke("PtEnd", 0.5f);
                Invoke("ReCharge", 1f);
            }
            void PtStart()
            {
                ChargePt.gameObject.GetComponent<ParticleSystem>().enableEmission = true;
            }
            void PtEnd()
            {
                ChargePt.gameObject.GetComponent<ParticleSystem>().enableEmission = false;
            }
            void NeedRecharge()
            {
                ShieldNeedRecharge = true;
            }
            public void ReCharge()
            {
                if(!ShieldOn)
                {
                    On();
                    nextHp = BOSS.GetComponent<HealthManager>().hp;
                    if (HardMode == 0)
                    {
                        BOSS.GetComponent<HealthManager>().hp += 300 + GameManager.instance.playerData.nailDamage * 30;
                    }
                    else
                    {
                        BOSS.GetComponent<HealthManager>().hp += 300 + GameManager.instance.playerData.nailDamage * 40;
                    }
                    BOSS.GetComponent<SpriteFlash>().flashDreamImpact();
                    HALO1.GetComponent<HaloScaleChange_Small>().ShieldRecover();
                    ShieldNeedRecharge = false;
                    BOSS.GetComponent<AudioSource>().PlayOneShot(ORBIMPACT, 1f);
                    WAVE.SetActive(true);
                    Invoke("WaveClose", 0.3f);
                }
            }
            public void ReChargeRage()
            {
                On();
                nextHp = BOSS.GetComponent<HealthManager>().hp;
                if(HardMode == 0)
                {
                    BOSS.GetComponent<HealthManager>().hp = 2000 + 300 + GameManager.instance.playerData.nailDamage * 30;
                }
                else
                {
                    BOSS.GetComponent<HealthManager>().hp = 2000 + 300 + GameManager.instance.playerData.nailDamage * 40;
                }
                BOSS.GetComponent<SpriteFlash>().flashDreamImpact();
                HALO1.GetComponent<HaloScaleChange_Small>().ShieldRecover();
                ShieldNeedRecharge = false;
                BOSS.GetComponent<AudioSource>().PlayOneShot(ORBIMPACT, 1f);
                WAVE.SetActive(true);
                Invoke("WaveClose", 0.3f);
            }
            void WaveClose()
            {
                WAVE.SetActive(false);
            }
            void Break()
            {
                ShieldNeedRecharge = false;
                if (finalStart)
                {
                    BOSS.GetComponent<HealthManager>().hp = 0;
                }
                Off();
                Invoke("NeedRecharge", 5f);
                BOSS.GetComponent<AudioSource>().PlayOneShot(ShieldBreak, 1f);
                BOSS.GetComponent<SpriteFlash>().flashArmoured();
                HALO1.GetComponent<HaloColorChange_Small>().GLowEnd();
                HALO1.GetComponent<HaloAround_Small>().SpinEnd();
                GameManager.instance.FreezeMoment(0);
                var activeStateName = BOSS.LocateMyFSM("Attack Commands").ActiveStateName;
                if (activeStateName != "Init" && activeStateName != "Idle")
                {
                    BOSS.LocateMyFSM("Attack Commands").SetState("End");
                }

                WAVE.SetActive(true);
                Invoke("WaveClose", 0.3f);

                GameObject[] allGameObjects = FindObjectsOfType<GameObject>();
                foreach (GameObject go in allGameObjects)
                {
                    if(go.name.Contains("Orb"))
                    {
                        go.LocateMyFSM("Orb Control").SetState("Recycle");
                    }
                    else if(go.name.Contains("Nail"))
                    {
                        go.LocateMyFSM("Control").SetState("Recycle");
                    }
                    else if(go.name.Contains("Radiant Beam"))
                    {
                        go.LocateMyFSM("Control").SendEvent("FIRE");
                        go.LocateMyFSM("Control").SendEvent("END");
                    }
                }
                Invoke("ObjClear", 0.1f);
                if(HardMode == 1)
                {
                    BOSS.GetComponent<BeamSkill6>().On(0.15f, 3f, 1.2f, false);
                }
            }
            void ObjClear()
            {
                GameObject[] allGameObjects = FindObjectsOfType<GameObject>();
                foreach (GameObject go in allGameObjects)
                {
                    if (go.name.Contains("Orb"))
                    {
                        go.LocateMyFSM("Orb Control").SetState("Recycle");
                    }
                    else if (go.name.Contains("Nail"))
                    {
                        go.LocateMyFSM("Control").SetState("Recycle");
                    }
                    else if (go.name.Contains("Radiant Beam"))
                    {
                        go.LocateMyFSM("Control").SendEvent("FIRE");
                        go.LocateMyFSM("Control").SendEvent("END");
                    }
                }
            }
            public void On()
            {
                ShieldOn = true;
                BOSS.GetComponent<EnemyHitEffectsGhost>().enabled = false;
                BOSS.GetComponent<HealthManager>().Reflect().enemyType = 3;
                BOSS.GetComponent<HaloControl_Small>().ReCharge();
            }
            public void Off()
            {
                ShieldOn = false;
                BOSS.GetComponent<EnemyHitEffectsGhost>().enabled = true;
                BOSS.GetComponent<HealthManager>().Reflect().enemyType = 2;
                BOSS.GetComponent<HaloControl_Small>().Close();
            }
            public void HitShield()
            {
                if (ShieldOn)
                {
                    BOSS.GetComponent<SpriteFlash>().flashShadeGet();
                    BOSS.GetComponent<HaloControl_Small>().Flash();
                    blackKnight.GetComponent<EnemyHitEffectsBlackKnight>().enemyDamage.SpawnAndPlayOneShot(blackKnight.GetComponent<EnemyHitEffectsBlackKnight>().audioPlayerPrefab, transform.position);
                    if (BOSS.GetComponent<HealthManager>().hp <= nextHp)
                    {
                        BOSS.GetComponent<HealthManager>().hp = nextHp;
                        BOSS.GetComponent<BossShieldControl>().Break();
                    }
                }
                else
                {
                    BOSS.GetComponent<HaloControl>().ControlEnd(0f);
                    BOSS.GetComponent<HaloControl_Small>().ControlEnd(0f);
                }
            }
            public void CancelInvokeOn()
            {
                CancelInvoke("On");
            }
        }
        public class NailBlock : MonoBehaviour
        {
            double RX0 => random.NextDouble();
            double RY0 => random.NextDouble();
            public float SIGHX => (random.Next(0, 2) * 2 - 1);
            public float SIGHY => (random.Next(0, 2) * 2 - 1);
            public float RX => (float)(RX0 * SIGHX);
            public float RY => (float)(RY0 * SIGHY);  
            void Fire()
            {
                if (gameObject.LocateMyFSM("Control").ActiveStateName != "Fire CW")
                {
                    gameObject.LocateMyFSM("Control").SetState("Fire CW");
                }
                if (gameObject.GetComponent<NailSpeedDown>() != null)
                {
                    gameObject.GetComponent<NailSpeedDown>().enabled = false;
                }
                if (gameObject.GetComponent<NailSpeedUp>() != null)
                {
                    gameObject.GetComponent<NailSpeedUp>().enabled = false;
                }

                float speed = 120f;
                float speednail = gameObject.LocateMyFSM("Control").GetState("Fire CW").GetAction<SetVelocityAsAngle>().speed.Value;
                if (speednail >= 200)
                {
                    speed = speednail;
                }
                gameObject.LocateMyFSM("Control").GetState("Fire CW").GetAction<SetVelocityAsAngle>().speed.Value = speed;
            }
            public void OnTriggerEnter2D(Collider2D collision)
            {
                var nail = gameObject;
                var receiver = collision.gameObject;
                if (receiver.gameObject.name == "Slash" || receiver.gameObject.name == "AltSlash" || receiver.gameObject.name == "Great Slash")
                {
                    Fire();
                    nail.GetComponent<DamageHero>().damageDealt = 0;
                    float angle = 90f + 90f * HeroController.instance.transform.localScale.x + RX * 10f;
                    SetDamageEnemy(nail, 50, angle - 90f, 1f);
                    nail.LocateMyFSM("Control").FsmVariables.FindFsmFloat("Angle").Value = angle;
                    nail.transform.eulerAngles = new Vector3(0, 0, angle - 90f);
                    //nail.LocateMyFSM("Control").GetState("Fire CW").GetAction<SetVelocityAsAngle>().speed.Value = Math.Sign(gameObject.LocateMyFSM("Control").GetState("Fire CW").GetAction<SetVelocityAsAngle>().speed.Value) * speed;
                    var Pt1 = GameObject.Instantiate(DREAMPTBLOCK, nail.transform.position, Quaternion.Euler(0, 0, nail.transform.eulerAngles.z + 90));
                    Pt1.gameObject.SetActive(true);
                    Pt1.GetComponent<ParticleSystem>().Play();
                    Pt1.AddComponent<ObjRecycle>();
                    var Pt2 = Instantiate(DREAMPTBLOCK, nail.transform.position, Quaternion.Euler(0, 0, nail.transform.eulerAngles.z - 90));
                    Pt2.gameObject.SetActive(true);
                    Pt2.GetComponent<ParticleSystem>().Play();
                    Pt2.AddComponent<ObjRecycle>();
                }
                else if (receiver.gameObject.name == "UpSlash")
                {
                    Fire();
                    nail.GetComponent<DamageHero>().damageDealt = 0;
                    float angle = 90f + RX * 10f;
                    nail.LocateMyFSM("Control").FsmVariables.FindFsmFloat("Angle").Value = angle;
                    SetDamageEnemy(nail, 50, angle - 90f, 1f);
                    nail.transform.eulerAngles = new Vector3(0, 0, angle - 90f);
                    //nail.LocateMyFSM("Control").GetState("Fire CW").GetAction<SetVelocityAsAngle>().speed.Value = Math.Sign(nail.LocateMyFSM("Control").GetState("Fire CW").GetAction<SetVelocityAsAngle>().speed.Value) * speed;
                    var Pt1 = Instantiate(DREAMPTBLOCK, nail.transform.position, Quaternion.Euler(0, 0, nail.transform.eulerAngles.z + 90));
                    Pt1.gameObject.SetActive(true);
                    Pt1.GetComponent<ParticleSystem>().Play();
                    Pt1.AddComponent<ObjRecycle>();
                    var Pt2 = GameObject.Instantiate(DREAMPTBLOCK, nail.transform.position, Quaternion.Euler(0, 0, nail.transform.eulerAngles.z - 90));
                    Pt2.gameObject.SetActive(true);
                    Pt2.GetComponent<ParticleSystem>().Play();
                    Pt2.AddComponent<ObjRecycle>();
                }
                else if (receiver.gameObject.name == "DownSlash")
                {
                    Fire();
                    nail.GetComponent<DamageHero>().damageDealt = 0;
                    float angle = -90f + RX * 10f;
                    nail.LocateMyFSM("Control").FsmVariables.FindFsmFloat("Angle").Value = angle;
                    SetDamageEnemy(nail, 50, angle - 90f, 1f);
                    nail.transform.eulerAngles = new Vector3(0, 0, angle - 90f);
                    //nail.LocateMyFSM("Control").GetState("Fire CW").GetAction<SetVelocityAsAngle>().speed.Value = Math.Sign(nail.LocateMyFSM("Control").GetState("Fire CW").GetAction<SetVelocityAsAngle>().speed.Value) * speed;
                    var Pt1 = GameObject.Instantiate(DREAMPTBLOCK, nail.transform.position, Quaternion.Euler(0, 0, nail.transform.eulerAngles.z + 90));
                    Pt1.gameObject.SetActive(true);
                    Pt1.GetComponent<ParticleSystem>().Play();
                    Pt1.AddComponent<ObjRecycle>();
                    var Pt2 = GameObject.Instantiate(DREAMPTBLOCK, nail.transform.position, Quaternion.Euler(0, 0, nail.transform.eulerAngles.z - 90));
                    Pt2.gameObject.SetActive(true);
                    Pt2.GetComponent<ParticleSystem>().Play();
                    Pt2.AddComponent<ObjRecycle>();
                }
                else if (receiver.gameObject.name == "Hit L")
                {
                    Fire();
                    nail.GetComponent<DamageHero>().damageDealt = 0;
                    float angle = 180f + RX * 10f;
                    nail.LocateMyFSM("Control").FsmVariables.FindFsmFloat("Angle").Value = angle;
                    SetDamageEnemy(nail, 50, angle - 90f, 1f);
                    nail.transform.eulerAngles = new Vector3(0, 0, angle - 90f);
                    //nail.LocateMyFSM("Control").GetState("Fire CW").GetAction<SetVelocityAsAngle>().speed.Value = Math.Sign(nail.LocateMyFSM("Control").GetState("Fire CW").GetAction<SetVelocityAsAngle>().speed.Value) * speed;
                    var Pt1 = GameObject.Instantiate(DREAMPTBLOCK, nail.transform.position, Quaternion.Euler(0, 0, nail.transform.eulerAngles.z + 90));
                    Pt1.gameObject.SetActive(true);
                    Pt1.GetComponent<ParticleSystem>().Play();
                    Pt1.AddComponent<ObjRecycle>();
                    var Pt2 = GameObject.Instantiate(DREAMPTBLOCK, nail.transform.position, Quaternion.Euler(0, 0, nail.transform.eulerAngles.z - 90));
                    Pt2.gameObject.SetActive(true);
                    Pt2.GetComponent<ParticleSystem>().Play();
                    Pt2.AddComponent<ObjRecycle>();
                }
                else if (receiver.gameObject.name == "Hit R")
                {
                    Fire();
                    nail.GetComponent<DamageHero>().damageDealt = 0;
                    float angle = 0f + RX * 10f;
                    nail.LocateMyFSM("Control").FsmVariables.FindFsmFloat("Angle").Value = angle;
                    SetDamageEnemy(nail, 50, angle - 90f, 1f);
                    nail.transform.eulerAngles = new Vector3(0, 0, angle - 90f);
                    //nail.LocateMyFSM("Control").GetState("Fire CW").GetAction<SetVelocityAsAngle>().speed.Value = Math.Sign(nail.LocateMyFSM("Control").GetState("Fire CW").GetAction<SetVelocityAsAngle>().speed.Value) * speed;
                    var Pt1 = GameObject.Instantiate(DREAMPTBLOCK, nail.transform.position, Quaternion.Euler(0, 0, nail.transform.eulerAngles.z + 90));
                    Pt1.gameObject.SetActive(true);
                    Pt1.GetComponent<ParticleSystem>().Play();
                    Pt1.AddComponent<ObjRecycle>();
                    var Pt2 = GameObject.Instantiate(DREAMPTBLOCK, nail.transform.position, Quaternion.Euler(0, 0, nail.transform.eulerAngles.z - 90));
                    Pt2.gameObject.SetActive(true);
                    Pt2.GetComponent<ParticleSystem>().Play();
                    Pt2.AddComponent<ObjRecycle>();
                }
            }
        }
        public class FinalPlatsSet : MonoBehaviour
        {
            GameObject obj1;
            GameObject obj2;
            public void Set()
            {
                obj1 = Instantiate(PLAT_Prefab, new Vector3(77.582f, 151.82f, 0f), new Quaternion());
                obj2 = Instantiate(PLAT_Prefab, new Vector3(48.8482f, 151.82f, 0f), new Quaternion());
                Invoke("DelayAcitve", 2f);
            }
            public void DelayAcitve()
            {
                if(obj1 == null || obj2 == null)
                {
                    Set();
                }
                obj1.SetActive(true);
                obj2.SetActive(true);
                obj1.GetComponent<MeshRenderer>().enabled = true;
                obj2.GetComponent<MeshRenderer>().enabled = true;
                obj1.transform.Find("Colliders").gameObject.SetActive(true);
                obj2.transform.Find("Colliders").gameObject.SetActive(true);
            }
        }
        public class CombL : MonoBehaviour
        {
            Vector3 position = new Vector3(38, 21, 0);
            float y = 0f;
            float factor1 = 1f;
            bool coolDown = true;
            public bool p2 = false;
            int count = 0;
            int countMax = 10;

            int num1 = random.Next(0, 3);
            int num2 = random.Next(3, 6);
            int num3 = random.Next(6, 9);
            int num4 = random.Next(9, 12);
            int num5 = random.Next(12, 15);
            int num6 = random.Next(15, 18);
            int num7 = random.Next(18, 21);
            int num8 = random.Next(21, 24);
            public void On(float l, float loopTime, int nailCount, float AccerateFactor)
            {
                if(count == 0 && coolDown)
                {
                    factor1 = AccerateFactor;
                    coolDown = false;
                    y = l;
                    SummonLoop();
                    if(p2)
                    {
                        position = new Vector3(33, 32, 0);
                    }
                    else
                    {
                        position = new Vector3(38, 21, 0);
                    }
                    countMax = nailCount;
                    Invoke("CoolDown", 2f);
                }
            }
            void CoolDown()
            {
                coolDown = true;
            }
            void SummonLoop()
            {
                int num11 = num1;
                int num22 = num2;
                int num33 = num3;
                int num44 = num4;
                int num55 = num5;
                int num66 = num6;
                int num77 = num7;
                int num88 = num8;
                for (int i = 0;i <= countMax;i++)
                {
                    if(i != num11 && i != num22 && i != num33 && i != num44 && i != num55 && i != num66 && i != num77 && i != num88)
                    {
                        NailSummon(position, 30 - i * 1f);
                    }
                    position += new Vector3(0, y, 0);
                }
                BOSS.GetComponent<Audios>().Play(4);
            } 
            void NailSummon(Vector3 pos, float speed)
            {
                var nail = Instantiate(NAIL, pos, Quaternion.Euler(0, 0, -90));
                //nail.GetComponent<DamageHero>().damageDealt = 0;
                nail.SetActive(true);
                nail.LocateMyFSM("Control").GetState("Set Collider").AddMethod(() =>
                {
                    nail.LocateMyFSM("Control").FsmVariables.FindFsmFloat("Angle").Value = nail.transform.eulerAngles.z + 90;
                    nail.LocateMyFSM("Control").GetState("Fire CW").RemoveAction<FaceAngle>();
                    nail.LocateMyFSM("Control").GetState("Fire CW").GetAction<FloatAdd>().add.Value = 0f;
                    nail.LocateMyFSM("Control").GetState("Fire CW").GetAction<SetVelocityAsAngle>().speed.Value = 0f;
                    nail.LocateMyFSM("Control").GetState("Fire CW").GetAction<Wait>().time.Value = 8f;
                    nail.LocateMyFSM("Control").SetState("Fire CW");
                    nail.AddComponent<NailSpeedUp>();
                    nail.GetComponent<NailSpeedUp>().NailSpeedMax = speed;
                    nail.GetComponent<NailSpeedUp>().accerateFactor = 0.75f * factor1;
                    nail.AddComponent<NailBlock>();
                });
            }
        }
        public class CombR : MonoBehaviour
        {
            Vector3 position = new Vector3(83, 21, 0);
            float y = 0f;
            float factor1 = 1f;
            bool coolDown = true;
            public bool p2 = false;
            int count = 0;
            int countMax = 10;
            int num1 = random.Next(0, 3);
            int num2 = random.Next(3, 6);
            int num3 = random.Next(6, 9);
            int num4 = random.Next(9, 12);
            int num5 = random.Next(12, 15);
            int num6 = random.Next(15, 18);
            int num7 = random.Next(18, 21);
            int num8 = random.Next(21, 24);
            public void On(float l, float loopTime, int nailCount, float AccerateFactor)
            {
                if(count == 0 && coolDown)
                {
                    factor1 = AccerateFactor;
                    coolDown = false;
                    y = l;
                    SummonLoop();
                    if(p2)
                    {
                        position = new Vector3(83, 32, 0);
                    }
                    else
                    {
                        position = new Vector3(86, 21, 0);
                    }
                    countMax = nailCount;
                    Invoke("CoolDown", 2f);
                }
            }
            void CoolDown()
            {
                coolDown = true;
            }
            void SummonLoop()
            {
                int num11 = num1;
                int num22 = num2;
                int num33 = num3;
                int num44 = num4;
                int num55 = num5;
                int num66 = num6;
                int num77 = num7;
                int num88 = num8;
                for (int i = 0; i <= countMax; i++)
                {
                    if (i != num11 && i != num22 && i != num33 && i != num44 && i != num55 && i != num66 && i != num77 && i != num88)
                    {
                        NailSummon(position, 30 - i * 1f);
                    }
                    position += new Vector3(0, y, 0);
                }
                BOSS.GetComponent<Audios>().Play(4);
            } 
            void NailSummon(Vector3 pos, float speed)
            {
                var nail = Instantiate(NAIL, pos, Quaternion.Euler(0, 0, 90));
                //nail.GetComponent<DamageHero>().damageDealt = 0;
                nail.SetActive(true);
                nail.LocateMyFSM("Control").GetState("Set Collider").AddMethod(() =>
                {
                    nail.LocateMyFSM("Control").FsmVariables.FindFsmFloat("Angle").Value = nail.transform.eulerAngles.z + 90;
                    nail.LocateMyFSM("Control").GetState("Fire CW").RemoveAction<FaceAngle>();
                    nail.LocateMyFSM("Control").GetState("Fire CW").GetAction<FloatAdd>().add.Value = 0f;
                    nail.LocateMyFSM("Control").GetState("Fire CW").GetAction<SetVelocityAsAngle>().speed.Value = 0f;
                    nail.LocateMyFSM("Control").GetState("Fire CW").GetAction<Wait>().time.Value = 8f;
                    nail.LocateMyFSM("Control").SetState("Fire CW");
                    nail.AddComponent<NailSpeedUp>();
                    nail.GetComponent<NailSpeedUp>().NailSpeedMax = speed;
                    nail.GetComponent<NailSpeedUp>().accerateFactor = 0.75f * factor1;
                    nail.AddComponent<NailBlock>();
                });
            }
        }
        public class BeamTop : MonoBehaviour
        {
            double RX0 => random.NextDouble();
            double RY0 => random.NextDouble();
            public float SIGHX => (random.Next(0, 2) * 2 - 1);
            public float SIGHY => (random.Next(0, 2) * 2 - 1);
            public float RX => (float)(RX0 * SIGHX);
            public float RY => (float)(RY0 * SIGHY);
            float Factor1 = 1;
            float Factor2 = 1;
            float LoopTime = 1;
            float X = 1;
            public bool p2 = false;
            Vector3 pos1 = new Vector3(41f, 35.2f, 0f);
            Vector3 pos2 = new Vector3(43.5f, 35.2f, 0f);

            Vector3 pos3 = new Vector3(46f, 42.3f, 0f);
            Vector3 pos4 = new Vector3(48f, 42.3f, 0f);

            Vector3 pos5 = new Vector3(49.7f, 36.1f, 0f);
            Vector3 pos6 = new Vector3(53.5f, 36.1f, 0f);

            Vector3 pos7 = new Vector3(57.6f, 44.7f, 0f);
            Vector3 pos8 = new Vector3(61.7f, 44.7f, 0f);

            Vector3 pos9 = new Vector3(61.7f, 33.3f, 0f);
            Vector3 pos10 = new Vector3(62.3f, 33.3f, 0f);

            Vector3 pos11 = new Vector3(66.7f, 37.8f, 0f);
            Vector3 pos12 = new Vector3(69.0f, 37.8f, 0f);

            Vector3 pos13 = new Vector3(71.5f, 43.8f, 0f);
            Vector3 pos14 = new Vector3(74.1f, 43.8f, 0f);
            public void On(float loopTime, float endTime, float beamScaleFactor)
            {
                Factor1 = beamScaleFactor;
                LoopTime = loopTime;
                SummonLoop();
                Invoke("SummonEnd", endTime);
            }
            void SummonLoop()
            {
                float summonX = 60 + 17 * RX;
                float summonY = 20;
                float scaleFactor = 3f * Factor1;
                if (p2)
                {
                    if (summonX > pos1.x && summonX < pos2.x)
                    {
                        summonY = pos1.y;
                    }
                    else if (summonX > pos3.x && summonX < pos4.x)
                    {
                        summonY = pos3.y;
                    }
                    else if (summonX > pos5.x && summonX < pos6.x)
                    {
                        summonY = pos5.y;
                    }
                    else if (summonX > pos7.x && summonX < pos8.x)
                    {
                        summonY = pos7.y;
                    }
                    else if (summonX > pos9.x && summonX < pos10.x)
                    {
                        summonY = pos9.y;
                    }
                    else if (summonX > pos11.x && summonX < pos12.x)
                    {
                        summonY = pos11.y;
                    }
                    else if (summonX > pos13.x && summonX < pos14.x)
                    {
                        summonY = pos13.y;
                    }
                }
                var beam = Instantiate(BEAM, new Vector3(summonX, summonY, 0), Quaternion.Euler(0, 0, 90));
                beam.transform.localScale *= Factor1;
                beam.AddComponent<BeamAuto_Small>();
                StartCoroutine(DelayedExecution(0.75f, scaleFactor));
                IEnumerator DelayedExecution(float time, float impactScaleFactor)
                {
                    yield return new WaitForSeconds(time);
                    Impact(summonX, summonY, impactScaleFactor);
                }
                Invoke("SummonLoop", LoopTime);
            }
            void SummonEnd()
            {
                CancelInvoke("SummonLoop");
            }
            void Impact(float x, float y, float impactScaleFactor)
            {
                var orb = Instantiate(ORB, new Vector3(x, y, -0.001f), Quaternion.Euler(0, 0, RY * 180f));
                var pt = orb.transform.Find("Impact Particles").gameObject;
                pt.transform.SetParent(null);
                pt.GetComponent<ParticleSystem>().emissionRate = 2000;
                pt.GetComponent<ParticleSystem>().startSpeed = 150;
                pt.AddComponent<ObjRecycle>();
                var heroHurter = orb.transform.Find("Hero Hurter").gameObject;
                heroHurter.SetActive(false);
                heroHurter.transform.SetParent(pt.transform);
                var impact = orb.transform.Find("Impact").gameObject;
                impact.GetComponent<RandomScale>().enabled = false;
                impact.transform.SetParent(pt.transform);
                orb.transform.position -= new Vector3(0, 50, 0);
                orb.LocateMyFSM("Orb Control").SetState("Recycle");
                pt.transform.localScale *= 1.75f;
                pt.AddComponent<OrbBlast_Quick>();
                pt.transform.localScale *= Factor2;
                impact.transform.localScale *= Factor2;
                heroHurter.transform.localScale *= Factor2;
            }
        }
        public class OrbSummonLoop : MonoBehaviour
        {
            double RX0 => random.NextDouble();
            double RY0 => random.NextDouble();
            public float SIGHX => (random.Next(0, 2) * 2 - 1);
            public float SIGHY => (random.Next(0, 2) * 2 - 1);
            public float RX => (float)(RX0 * SIGHX);
            public float RY => (float)(RY0 * SIGHY);
        }
        public class OrbBlast : MonoBehaviour
        {
            double RX0 => random.NextDouble();
            double RY0 => random.NextDouble();
            public float SIGHX => (random.Next(0, 2) * 2 - 1);
            public float SIGHY => (random.Next(0, 2) * 2 - 1);
            public float RX => (float)(RX0 * SIGHX);
            public float RY => (float)(RY0 * SIGHY);
            float angle = 0f;
            float R = 0f;
            public void On(float r, float time)
            {
                SummonBlastLoop();
                R = r;
                Invoke("StopLoop", time);
            }
            void SummonBlastLoop()
            {
                float angle = RX * (float)Math.PI;
                float random = (RY + 1) / 2;
                Vector3 SummonPosition = new Vector3((float)Math.Cos(angle) * R * random, (float)Math.Sin(angle) * R * random, 0);
                var orb = Instantiate(ORB, HALO1.transform.position + SummonPosition, Quaternion.Euler(0, 0, RY * 180f));
                var pt = orb.transform.Find("Impact Particles").gameObject;
                pt.transform.SetParent(null);
                pt.GetComponent<ParticleSystem>().emissionRate = 1000;
                pt.GetComponent<ParticleSystem>().startSpeed = 100;
                pt.AddComponent<ObjRecycle>();
                var heroHurter = orb.transform.Find("Hero Hurter").gameObject;
                heroHurter.SetActive(false);
                heroHurter.transform.SetParent(pt.transform);
                var impact = orb.transform.Find("Impact").gameObject;
                impact.GetComponent<RandomScale>().enabled = false;
                impact.transform.SetParent(pt.transform);
                orb.transform.position -= new Vector3(0, 50, 0);
                orb.LocateMyFSM("Orb Control").SetState("Recycle");
                pt.transform.localScale *= 1.75f;
                pt.AddComponent<OrbBlast_Quick>();

                if (ShieldOn)
                {
                    Invoke("SummonBlastLoop", 0.06f);
                }
            }
            void StopLoop()
            {
                CancelInvoke("SummonBlastLoop");
            }
        }
        public class OrbBlast_Quick : MonoBehaviour
        {
            void Start()
            {
                gameObject.transform.Find("Hero Hurter").gameObject.SetActive(true);
                gameObject.transform.Find("Impact").gameObject.SetActive(true);
                gameObject.GetComponent<ParticleSystem>().Play();
                BOSS.GetComponent<AudioSource>().PlayOneShot(ORBIMPACT, 1f);
                Invoke("End", 0.1f);
            }
            void End()
            {
                gameObject.transform.Find("Hero Hurter").gameObject.Recycle();
            }
        }
        public class OrbImpact : MonoBehaviour
        {
            public void Impact()
            {
                BOSS.transform.Find("Hero Hurter").gameObject.SetActive(true);
                BOSS.transform.Find("Impact").gameObject.SetActive(true);
                BOSS.transform.Find("Impact Particles").GetComponent<ParticleSystem>().Play();
                BOSS.GetComponent<AudioSource>().PlayOneShot(ORBIMPACT, 1f);
                Invoke("End", 0.1f);
            }
            void End()
            {
                BOSS.transform.Find("Hero Hurter").gameObject.SetActive(false);
            }
            void Start()
            {
                var orb = Instantiate(ORB, BOSS.transform.position + new Vector3(-0.1f, 2f, 0f), new Quaternion(0, 0, 0, 0));
                ORBIMPACT = orb.LocateMyFSM("Orb Control").GetState("Impact").GetAction<AudioPlaySimple>().oneShotClip.Value as AudioClip;
                var heroHurter = orb.transform.Find("Hero Hurter").gameObject;
                heroHurter.SetActive(false);
                heroHurter.transform.localScale = new Vector3(2, 2, 1);
                heroHurter.transform.SetParent(BOSS.transform);
                var impact = orb.transform.Find("Impact").gameObject;
                impact.GetComponent<RandomScale>().enabled = false;
                impact.transform.localScale = new Vector3(2, 2, 1);
                impact.transform.SetParent(BOSS.transform);
                var pt = orb.transform.Find("Impact Particles").gameObject;
                if(settings_Pt_.on)
                {
                    pt.GetComponent<ParticleSystem>().emissionRate = 1500;
                }
                else
                {
                    pt.GetComponent<ParticleSystem>().emissionRate = 5000;
                }
                pt.GetComponent<ParticleSystem>().startSpeed = 100;
                pt.transform.SetParent(BOSS.transform);
                orb.transform.position -= new Vector3(0, 30, 0);
                orb.LocateMyFSM("Orb Control").SetState("Recycle");
            }
        }
        public class BeamAuto_Small : MonoBehaviour
        {
            void Awake()
            {
                gameObject.LocateMyFSM("Control").SetState("Antic");
                gameObject.LocateMyFSM("Control").SendEvent("ANTIC");
                Invoke("Fire", 0.75f);
                Invoke("End", 0.85f);
            }
            void Fire()
            {
                gameObject.LocateMyFSM("Control").SendEvent("FIRE");
            }
            void End()
            {
                gameObject.LocateMyFSM("Control").SendEvent("END");
                gameObject.AddComponent<ObjRecycle>();
            }
        }
        public class BeamAuto_Big : MonoBehaviour
        {
            void Awake()
            {
                gameObject.LocateMyFSM("Control").SetState("Antic");
                gameObject.LocateMyFSM("Control").SendEvent("ANTIC");
                Invoke("Fire", 1f);
                Invoke("End", 1.1f);
            }
            void Fire()
            {
                gameObject.LocateMyFSM("Control").SendEvent("FIRE");
            }
            void End()
            {
                gameObject.LocateMyFSM("Control").SendEvent("END");
                gameObject.AddComponent<ObjRecycle>();
            }
        }
        public class BeamAuto_flower : MonoBehaviour
        {
            float r = 0;
            float x = 0;
            float y = 0;
            Vector3 origPosition = new Vector3(0, 0, 0);
            public void SetR(float R)
            {
                r = R;
            }
            void Start()
            {
                origPosition = gameObject.transform.position;
                float angle = gameObject.transform.eulerAngles.z + 90;
                x = (float)Math.Cos(DegreesToRadians(angle));
                y = (float)Math.Sin(DegreesToRadians(angle));
                gameObject.LocateMyFSM("Control").SetState("Antic");
                gameObject.LocateMyFSM("Control").SendEvent("ANTIC");
                Invoke("Fire", 1.5f);
                Invoke("End", 1.6f);
            }
            void Fire()
            {
                gameObject.LocateMyFSM("Control").SendEvent("FIRE");
            }
            void End()
            {
                gameObject.LocateMyFSM("Control").SendEvent("END");
                gameObject.AddComponent<ObjRecycle>();
            }
            void FixedUpdate()
            {
                gameObject.transform.position += (origPosition + new Vector3(x * r, y * r, 0) - gameObject.transform.position) * Time.deltaTime * 3.5f;
            }
        }
        public class BeamAuto_flower_Reverse : MonoBehaviour
        {
            float r = 0;
            float x = 0;
            float y = 0;
            Vector3 origPosition = new Vector3(0, 0, 0);
            public void SetR(float R)
            {
                r = R;
            }
            void Start()
            {
                origPosition = gameObject.transform.position;
                float angle = gameObject.transform.eulerAngles.z - 90f;
                x = (float)Math.Cos(DegreesToRadians(angle));
                y = (float)Math.Sin(DegreesToRadians(angle));
                gameObject.LocateMyFSM("Control").SetState("Antic");
                gameObject.LocateMyFSM("Control").SendEvent("ANTIC");
                Invoke("Fire", 1.5f);
                Invoke("End", 1.6f);
            }
            void Fire()
            {
                gameObject.LocateMyFSM("Control").SendEvent("FIRE");
            }
            void End()
            {
                gameObject.LocateMyFSM("Control").SendEvent("END");
                gameObject.AddComponent<ObjRecycle>();
            }
            void FixedUpdate()
            {
                gameObject.transform.position += (origPosition + new Vector3(x * r, y * r, 0) - gameObject.transform.position) * Time.deltaTime * 3.5f;
            }
        }
        /*
        public class BeamSkillRage : MonoBehaviour
        {
            public static double R => random.NextDouble();
            public static float SIGH => (random.Next(0, 2) * 2 - 1);
            public static float RX => (float)(R * SIGH);
            bool turnOff = false;
            bool coolDown = true;
            public void On()
            {
                if (coolDown)
                {
                    coolDown = false;
                    Invoke("BeamsSummon1", 0f);
                    Invoke("BeamsSummon2", 1.1f);
                    Invoke("BeamsSummon3", 2.2f);
                    Invoke("CoolDown", 3.7f);
                }
            }
            void CoolDown()
            {
                coolDown = true;
            }
            public void Off()
            {
                turnOff = true;
            }
            void BeamsSummon1()
            {
                if (!turnOff)
                {
                    float factor = 1.3f;
                    var beam0 = Instantiate(BEAM, new Vector3(HeroController.instance.gameObject.transform.position.x + RX / 2 - 8 * factor, 20, 0), Quaternion.Euler(0, 0, 90));
                    var beam1 = Instantiate(BEAM, new Vector3(HeroController.instance.gameObject.transform.position.x + RX / 2 - 6 * factor, 20, 0), Quaternion.Euler(0, 0, 90));
                    var beam2 = Instantiate(BEAM, new Vector3(HeroController.instance.gameObject.transform.position.x + RX / 2 - 4 * factor, 20, 0), Quaternion.Euler(0, 0, 90));
                    var beam3 = Instantiate(BEAM, new Vector3(HeroController.instance.gameObject.transform.position.x + RX / 2 - 2 * factor, 20, 0), Quaternion.Euler(0, 0, 90));
                    var beam4 = Instantiate(BEAM, new Vector3(HeroController.instance.gameObject.transform.position.x + RX / 2 - 0 * factor, 20, 0), Quaternion.Euler(0, 0, 90));
                    var beam5 = Instantiate(BEAM, new Vector3(HeroController.instance.gameObject.transform.position.x + RX / 2 + 2 * factor, 20, 0), Quaternion.Euler(0, 0, 90));
                    var beam6 = Instantiate(BEAM, new Vector3(HeroController.instance.gameObject.transform.position.x + RX / 2 + 4 * factor, 20, 0), Quaternion.Euler(0, 0, 90));
                    var beam7 = Instantiate(BEAM, new Vector3(HeroController.instance.gameObject.transform.position.x + RX / 2 + 6 * factor, 20, 0), Quaternion.Euler(0, 0, 90));
                    var beam8 = Instantiate(BEAM, new Vector3(HeroController.instance.gameObject.transform.position.x + RX / 2 + 8 * factor, 20, 0), Quaternion.Euler(0, 0, 90));
                    beam0.AddComponent<BeamAuto_Small>();
                    beam1.AddComponent<BeamAuto_Small>();
                    beam2.AddComponent<BeamAuto_Small>();
                    beam3.AddComponent<BeamAuto_Small>();
                    beam4.AddComponent<BeamAuto_Small>();
                    beam5.AddComponent<BeamAuto_Small>();
                    beam6.AddComponent<BeamAuto_Small>();
                    beam7.AddComponent<BeamAuto_Small>();
                    beam8.AddComponent<BeamAuto_Small>();
                    Invoke("AnticAudio", 0f);
                    Invoke("FireAudio", 0.75f);
                }
            }
            void BeamsSummon2()
            {
                if (!turnOff)
                {
                    float factor = 1.3f;
                    var beam0 = Instantiate(BEAM, new Vector3(HeroController.instance.gameObject.transform.position.x + RX / 2 - 8 * factor, 20, 0), Quaternion.Euler(0, 0, 90));
                    var beam1 = Instantiate(BEAM, new Vector3(HeroController.instance.gameObject.transform.position.x + RX / 2 - 6 * factor, 20, 0), Quaternion.Euler(0, 0, 90));
                    var beam2 = Instantiate(BEAM, new Vector3(HeroController.instance.gameObject.transform.position.x + RX / 2 - 4 * factor, 20, 0), Quaternion.Euler(0, 0, 90));
                    var beam3 = Instantiate(BEAM, new Vector3(HeroController.instance.gameObject.transform.position.x + RX / 2 - 2 * factor, 20, 0), Quaternion.Euler(0, 0, 90));
                    var beam4 = Instantiate(BEAM, new Vector3(HeroController.instance.gameObject.transform.position.x + RX / 2 - 0 * factor, 20, 0), Quaternion.Euler(0, 0, 90));
                    var beam5 = Instantiate(BEAM, new Vector3(HeroController.instance.gameObject.transform.position.x + RX / 2 + 2 * factor, 20, 0), Quaternion.Euler(0, 0, 90));
                    var beam6 = Instantiate(BEAM, new Vector3(HeroController.instance.gameObject.transform.position.x + RX / 2 + 4 * factor, 20, 0), Quaternion.Euler(0, 0, 90));
                    var beam7 = Instantiate(BEAM, new Vector3(HeroController.instance.gameObject.transform.position.x + RX / 2 + 6 * factor, 20, 0), Quaternion.Euler(0, 0, 90));
                    var beam8 = Instantiate(BEAM, new Vector3(HeroController.instance.gameObject.transform.position.x + RX / 2 + 8 * factor, 20, 0), Quaternion.Euler(0, 0, 90));
                    beam0.AddComponent<BeamAuto_Small>();
                    beam1.AddComponent<BeamAuto_Small>();
                    beam2.AddComponent<BeamAuto_Small>();
                    beam3.AddComponent<BeamAuto_Small>();
                    beam4.AddComponent<BeamAuto_Small>();
                    beam5.AddComponent<BeamAuto_Small>();
                    beam6.AddComponent<BeamAuto_Small>();
                    beam7.AddComponent<BeamAuto_Small>();
                    beam8.AddComponent<BeamAuto_Small>();
                    Invoke("AnticAudio", 0f);
                    Invoke("FireAudio", 0.75f);
                }
            }
            void BeamsSummon3()
            {
                if (!turnOff)
                {
                    float factor = 1.6f;
                    var beam0 = Instantiate(BEAM, new Vector3(HeroController.instance.gameObject.transform.position.x + RX / 2 - 8 * factor, 20, 0), Quaternion.Euler(0, 0, 90));
                    var beam1 = Instantiate(BEAM, new Vector3(HeroController.instance.gameObject.transform.position.x + RX / 2 - 6 * factor, 20, 0), Quaternion.Euler(0, 0, 90));
                    var beam2 = Instantiate(BEAM, new Vector3(HeroController.instance.gameObject.transform.position.x + RX / 2 - 4 * factor, 20, 0), Quaternion.Euler(0, 0, 90));
                    var beam3 = Instantiate(BEAM, new Vector3(HeroController.instance.gameObject.transform.position.x + RX / 2 - 2 * factor, 20, 0), Quaternion.Euler(0, 0, 90));
                    var beam4 = Instantiate(BEAM, new Vector3(HeroController.instance.gameObject.transform.position.x + RX / 2 - 0 * factor, 20, 0), Quaternion.Euler(0, 0, 90));
                    var beam5 = Instantiate(BEAM, new Vector3(HeroController.instance.gameObject.transform.position.x + RX / 2 + 2 * factor, 20, 0), Quaternion.Euler(0, 0, 90));
                    var beam6 = Instantiate(BEAM, new Vector3(HeroController.instance.gameObject.transform.position.x + RX / 2 + 4 * factor, 20, 0), Quaternion.Euler(0, 0, 90));
                    var beam7 = Instantiate(BEAM, new Vector3(HeroController.instance.gameObject.transform.position.x + RX / 2 + 6 * factor, 20, 0), Quaternion.Euler(0, 0, 90));
                    var beam8 = Instantiate(BEAM, new Vector3(HeroController.instance.gameObject.transform.position.x + RX / 2 + 8 * factor, 20, 0), Quaternion.Euler(0, 0, 90));
                    beam0.transform.localScale *= 2.5f;
                    beam1.transform.localScale *= 2.5f;
                    beam2.transform.localScale *= 2.5f;
                    beam3.transform.localScale *= 2.5f;
                    beam4.transform.localScale *= 2.5f;
                    beam5.transform.localScale *= 2.5f;
                    beam6.transform.localScale *= 2.5f;
                    beam7.transform.localScale *= 2.5f;
                    beam8.transform.localScale *= 2.5f;
                    beam0.AddComponent<BeamAuto_Big>();
                    beam1.AddComponent<BeamAuto_Big>();
                    beam2.AddComponent<BeamAuto_Big>();
                    beam3.AddComponent<BeamAuto_Big>();
                    beam4.AddComponent<BeamAuto_Big>();
                    beam5.AddComponent<BeamAuto_Big>();
                    beam6.AddComponent<BeamAuto_Big>();
                    beam7.AddComponent<BeamAuto_Big>();
                    beam8.AddComponent<BeamAuto_Big>();
                    Invoke("Impact", 0f);
                    Invoke("AnticAudio", 0f);
                    Invoke("FireAudio", 1f);
                }
            }
            void Impact()
            {
                BOSS.GetComponent<OrbImpact>().Impact();
            }
            void AnticAudio()
            {
                AudioClip Antic = BOSS.LocateMyFSM("Attack Commands").GetState("EB 1").GetAction<AudioPlaySimple>().oneShotClip.Value as AudioClip;
                BOSS.GetComponent<AudioSource>().PlayOneShot(Antic, 1f);
            }
            void FireAudio()
            {
                AudioClip Fire = BOSS.LocateMyFSM("Attack Commands").GetState("EB 1").GetAction<AudioPlayerOneShotSingle>().audioClip.Value as AudioClip;
                BOSS.GetComponent<AudioSource>().PlayOneShot(Fire, 1f);
            }
        }
        */
        public class Beam3Blast : MonoBehaviour
        {
            float R = 0;
            public void On(float r)
            {
                SummonBlastLoop();
                R = r;
            }
            void SummonBlastLoop()
            {
                float angle = RX * (float)Math.PI;
                float random = (RY + 1) / 2;
                Vector3 SummonPosition = new Vector3((float)Math.Cos(angle) * R * random, (float)Math.Sin(angle) * R * random, 0);
                var orb = Instantiate(ORB, gameObject.transform.position + SummonPosition, Quaternion.Euler(0, 0, RY * 180f));
                var pt = orb.transform.Find("Impact Particles").gameObject;
                pt.transform.SetParent(null);
                pt.GetComponent<ParticleSystem>().emissionRate = 1000;
                pt.GetComponent<ParticleSystem>().startSpeed = 100;
                pt.AddComponent<ObjRecycle>();
                var heroHurter = orb.transform.Find("Hero Hurter").gameObject;
                heroHurter.SetActive(false);
                heroHurter.transform.SetParent(pt.transform);
                var impact = orb.transform.Find("Impact").gameObject;
                impact.GetComponent<RandomScale>().enabled = false;
                impact.transform.SetParent(pt.transform);
                orb.transform.position -= new Vector3(0, 50, 0);
                orb.LocateMyFSM("Orb Control").SetState("Recycle");
                pt.transform.localScale *= 0.8f;
                pt.AddComponent<OrbBlast_Quick>();

                if (ShieldOn)
                {
                    Invoke("SummonBlastLoop", 0.1f);
                }
            }
            public void StopLoop()
            {
                CancelInvoke("SummonBlastLoop");
            }
        }
        public class BeamSkill1: MonoBehaviour
        {
            public static double R => random.NextDouble();
            public static float SIGH => (random.Next(0, 2) * 2 - 1);
            public static float RX => (float)(R * SIGH);
            float Xfactor = 1f;
            public void On(float xFactor)
            {
                Xfactor = xFactor;
                Invoke("BeamsSummon1", 0f);
                Invoke("BeamsSummon2", 1.1f);
                Invoke("BeamsSummon3", 2.2f);
            }
            void BeamsSummon1()
            {
                if (ShieldOn)
                {
                    float factor = 1.3f * Xfactor;
                    var beam0 = Instantiate(BEAM, new Vector3(HeroController.instance.gameObject.transform.position.x + RX / 2 - 8 * factor, 20, 0), Quaternion.Euler(0, 0, 90));
                    var beam1 = Instantiate(BEAM, new Vector3(HeroController.instance.gameObject.transform.position.x + RX / 2 - 6 * factor, 20, 0), Quaternion.Euler(0, 0, 90));
                    var beam2 = Instantiate(BEAM, new Vector3(HeroController.instance.gameObject.transform.position.x + RX / 2 - 4 * factor, 20, 0), Quaternion.Euler(0, 0, 90));
                    var beam3 = Instantiate(BEAM, new Vector3(HeroController.instance.gameObject.transform.position.x + RX / 2 - 2 * factor, 20, 0), Quaternion.Euler(0, 0, 90));
                    var beam4 = Instantiate(BEAM, new Vector3(HeroController.instance.gameObject.transform.position.x + RX / 2 - 0 * factor, 20, 0), Quaternion.Euler(0, 0, 90));
                    var beam5 = Instantiate(BEAM, new Vector3(HeroController.instance.gameObject.transform.position.x + RX / 2 + 2 * factor, 20, 0), Quaternion.Euler(0, 0, 90));
                    var beam6 = Instantiate(BEAM, new Vector3(HeroController.instance.gameObject.transform.position.x + RX / 2 + 4 * factor, 20, 0), Quaternion.Euler(0, 0, 90));
                    var beam7 = Instantiate(BEAM, new Vector3(HeroController.instance.gameObject.transform.position.x + RX / 2 + 6 * factor, 20, 0), Quaternion.Euler(0, 0, 90));
                    var beam8 = Instantiate(BEAM, new Vector3(HeroController.instance.gameObject.transform.position.x + RX / 2 + 8 * factor, 20, 0), Quaternion.Euler(0, 0, 90));
                    beam0.AddComponent<BeamAuto_Small>();
                    beam1.AddComponent<BeamAuto_Small>();
                    beam2.AddComponent<BeamAuto_Small>();
                    beam3.AddComponent<BeamAuto_Small>();
                    beam4.AddComponent<BeamAuto_Small>();
                    beam5.AddComponent<BeamAuto_Small>();
                    beam6.AddComponent<BeamAuto_Small>();
                    beam7.AddComponent<BeamAuto_Small>();
                    beam8.AddComponent<BeamAuto_Small>();
                    Invoke("AnticAudio", 0f);
                    Invoke("FireAudio", 0.75f);
                }
            }
            void BeamsSummon2()
            {
                if (ShieldOn)
                {
                    float factor = 1.3f * Xfactor;
                    var beam0 = Instantiate(BEAM, new Vector3(HeroController.instance.gameObject.transform.position.x + RX / 2 - 8 * factor, 20, 0), Quaternion.Euler(0, 0, 90));
                    var beam1 = Instantiate(BEAM, new Vector3(HeroController.instance.gameObject.transform.position.x + RX / 2 - 6 * factor, 20, 0), Quaternion.Euler(0, 0, 90));
                    var beam2 = Instantiate(BEAM, new Vector3(HeroController.instance.gameObject.transform.position.x + RX / 2 - 4 * factor, 20, 0), Quaternion.Euler(0, 0, 90));
                    var beam3 = Instantiate(BEAM, new Vector3(HeroController.instance.gameObject.transform.position.x + RX / 2 - 2 * factor, 20, 0), Quaternion.Euler(0, 0, 90));
                    var beam4 = Instantiate(BEAM, new Vector3(HeroController.instance.gameObject.transform.position.x + RX / 2 - 0 * factor, 20, 0), Quaternion.Euler(0, 0, 90));
                    var beam5 = Instantiate(BEAM, new Vector3(HeroController.instance.gameObject.transform.position.x + RX / 2 + 2 * factor, 20, 0), Quaternion.Euler(0, 0, 90));
                    var beam6 = Instantiate(BEAM, new Vector3(HeroController.instance.gameObject.transform.position.x + RX / 2 + 4 * factor, 20, 0), Quaternion.Euler(0, 0, 90));
                    var beam7 = Instantiate(BEAM, new Vector3(HeroController.instance.gameObject.transform.position.x + RX / 2 + 6 * factor, 20, 0), Quaternion.Euler(0, 0, 90));
                    var beam8 = Instantiate(BEAM, new Vector3(HeroController.instance.gameObject.transform.position.x + RX / 2 + 8 * factor, 20, 0), Quaternion.Euler(0, 0, 90));
                    beam0.AddComponent<BeamAuto_Small>();
                    beam1.AddComponent<BeamAuto_Small>();
                    beam2.AddComponent<BeamAuto_Small>();
                    beam3.AddComponent<BeamAuto_Small>();
                    beam4.AddComponent<BeamAuto_Small>();
                    beam5.AddComponent<BeamAuto_Small>();
                    beam6.AddComponent<BeamAuto_Small>();
                    beam7.AddComponent<BeamAuto_Small>();
                    beam8.AddComponent<BeamAuto_Small>();
                    Invoke("AnticAudio", 0f);
                    Invoke("FireAudio", 0.75f);
                }
            }
            void BeamsSummon3()
            {
                if (ShieldOn)
                {
                    float factor = 1.6f * Xfactor;
                    var beam0 = Instantiate(BEAM, new Vector3(HeroController.instance.gameObject.transform.position.x + RX / 2 - 8 * factor, 20, 0), Quaternion.Euler(0, 0, 90));
                    var beam1 = Instantiate(BEAM, new Vector3(HeroController.instance.gameObject.transform.position.x + RX / 2 - 6 * factor, 20, 0), Quaternion.Euler(0, 0, 90));
                    var beam2 = Instantiate(BEAM, new Vector3(HeroController.instance.gameObject.transform.position.x + RX / 2 - 4 * factor, 20, 0), Quaternion.Euler(0, 0, 90));
                    var beam3 = Instantiate(BEAM, new Vector3(HeroController.instance.gameObject.transform.position.x + RX / 2 - 2 * factor, 20, 0), Quaternion.Euler(0, 0, 90));
                    var beam4 = Instantiate(BEAM, new Vector3(HeroController.instance.gameObject.transform.position.x + RX / 2 - 0 * factor, 20, 0), Quaternion.Euler(0, 0, 90));
                    var beam5 = Instantiate(BEAM, new Vector3(HeroController.instance.gameObject.transform.position.x + RX / 2 + 2 * factor, 20, 0), Quaternion.Euler(0, 0, 90));
                    var beam6 = Instantiate(BEAM, new Vector3(HeroController.instance.gameObject.transform.position.x + RX / 2 + 4 * factor, 20, 0), Quaternion.Euler(0, 0, 90));
                    var beam7 = Instantiate(BEAM, new Vector3(HeroController.instance.gameObject.transform.position.x + RX / 2 + 6 * factor, 20, 0), Quaternion.Euler(0, 0, 90));
                    var beam8 = Instantiate(BEAM, new Vector3(HeroController.instance.gameObject.transform.position.x + RX / 2 + 8 * factor, 20, 0), Quaternion.Euler(0, 0, 90));
                    beam0.transform.localScale *= 2f;
                    beam1.transform.localScale *= 2f;
                    beam2.transform.localScale *= 2f;
                    beam3.transform.localScale *= 2f;
                    beam4.transform.localScale *= 2f;
                    beam5.transform.localScale *= 2f;
                    beam6.transform.localScale *= 2f;
                    beam7.transform.localScale *= 2f;
                    beam8.transform.localScale *= 2f;
                    beam0.AddComponent<BeamAuto_Big>();
                    beam1.AddComponent<BeamAuto_Big>();
                    beam2.AddComponent<BeamAuto_Big>();
                    beam3.AddComponent<BeamAuto_Big>();
                    beam4.AddComponent<BeamAuto_Big>();
                    beam5.AddComponent<BeamAuto_Big>();
                    beam6.AddComponent<BeamAuto_Big>();
                    beam7.AddComponent<BeamAuto_Big>();
                    beam8.AddComponent<BeamAuto_Big>();
                    Invoke("Impact", 0f);
                    Invoke("AnticAudio", 0f);
                    Invoke("FireAudio", 1f);
                }
            }
            void Impact()
            {
                BOSS.GetComponent<OrbImpact>().Impact();
            }
            void AnticAudio()
            {
                AudioClip Antic = BOSS.LocateMyFSM("Attack Commands").GetState("EB 1").GetAction<AudioPlaySimple>().oneShotClip.Value as AudioClip;
                BOSS.GetComponent<AudioSource>().PlayOneShot(Antic, 1f);
            }
            void FireAudio()
            {
                AudioClip Fire = BOSS.LocateMyFSM("Attack Commands").GetState("EB 1").GetAction<AudioPlayerOneShotSingle>().audioClip.Value as AudioClip;
                BOSS.GetComponent<AudioSource>().PlayOneShot(Fire, 1f);
            }
        }
        public class BeamSkill3 : MonoBehaviour
        {
            GameObject Beam;
            GameObject Beam1;
            GameObject Beam2;
            GameObject Beam3;
            GameObject Beam4;
            GameObject ShotPt;
            public float Angle;
            float BeamScaleY = 2.1f;
            float BeamScaleYMax = 3.2f;
            float BeamScaleYMin = 2.3f;
            float ADDANGLE;
            float angleSpeed = 0f;
            float angleSpeedMinusFactor = 0.8f;
            float angleSpeedAddMax = 0.6f;
            float angleSpeedAddFactor = 0.05f;
            float backSpeed = 0f;
            float backSpeedAdd = 0.1f;

            float x;
            float y;
            bool fireOn = false;
            void Start()
            {
                BeamSummon();
            }
            void BeamSummon()
            {
                Beam = Instantiate(BEAM, BOSS.transform.position, Quaternion.Euler(0, 0, 90),BOSS.transform);
                Beam.transform.localScale = new Vector3(Beam.transform.localScale.x, Beam.transform.localScale.y * 0.8f, Beam.transform.localScale.z);
                Beam.transform.localPosition = new Vector3(0, 2.5f, 0);
                Beam.AddComponent<Beam3Blast>();

                Beam.GetComponent<DamageHero>().damageDealt = 4;

                Beam1 = Instantiate(BEAM, BOSS.transform.position, Quaternion.Euler(0, 0, 90), BOSS.transform);
                Beam1.transform.localScale = new Vector3(Beam.transform.localScale.x, Beam.transform.localScale.y * 0.8f, Beam.transform.localScale.z);
                Beam1.transform.eulerAngles = new Vector3(0, 0, 90f);
                Beam1.transform.localPosition = new Vector3(0, 2.5f, 0);
                Beam2 = Instantiate(BEAM, BOSS.transform.position, Quaternion.Euler(0, 0, 90), BOSS.transform);
                Beam2.transform.localScale = new Vector3(Beam.transform.localScale.x, Beam.transform.localScale.y * 0.8f, Beam.transform.localScale.z);
                Beam2.transform.eulerAngles = new Vector3(0, 0, -90f);
                Beam2.transform.localPosition = new Vector3(0, 2.5f, 0);
                Beam3 = Instantiate(BEAM, BOSS.transform.position, Quaternion.Euler(0, 0, 90), BOSS.transform);
                Beam3.transform.localScale = new Vector3(Beam.transform.localScale.x, Beam.transform.localScale.y * 0.8f, Beam.transform.localScale.z);
                Beam3.transform.eulerAngles = new Vector3(0, 0, 45f);
                Beam3.transform.localPosition = new Vector3(0, 2.5f, 0);
                Beam4 = Instantiate(BEAM, BOSS.transform.position, Quaternion.Euler(0, 0, 90), BOSS.transform);
                Beam4.transform.localScale = new Vector3(Beam.transform.localScale.x, Beam.transform.localScale.y * 0.8f, Beam.transform.localScale.z);
                Beam4.transform.eulerAngles = new Vector3(0, 0, -45f);
                Beam4.transform.localPosition = new Vector3(0, 2.5f, 0);
                Beam1.AddComponent<BeamSmallAuto>();
                Beam2.AddComponent<BeamSmallAuto>();
                Beam3.AddComponent<BeamSmallAuto>();
                Beam4.AddComponent<BeamSmallAuto>();
                Beam1.GetComponent<BeamSmallAuto>().SetAngle(90f);
                Beam2.GetComponent<BeamSmallAuto>().SetAngle(-90f);
                Beam3.GetComponent<BeamSmallAuto>().SetAngle(45f);
                Beam4.GetComponent<BeamSmallAuto>().SetAngle(-45f);
                ShotPt = Instantiate(DREAMPTBLOCK, Beam.transform.position, Quaternion.Euler(0, 0, 0), Beam.transform);
                ShotPt.transform.GetComponent<ParticleSystem>().enableEmission = false;
                if (settings_Pt_.on)
                {
                    ShotPt.transform.GetComponent<ParticleSystem>().emissionRate = 150f;
                }
                else
                {
                    ShotPt.transform.GetComponent<ParticleSystem>().emissionRate = 300f;
                }
                ShotPt.transform.GetComponent<ParticleSystem>().startLifetime = 1f;
                ShotPt.transform.GetComponent<ParticleSystem>().startSpeed = 400f;
                ShotPt.transform.GetComponent<ParticleSystem>().loop = true;
                ShotPt.transform.GetComponent<ParticleSystem>().Play();
                ShotPt.transform.GetComponent<Transform>().localPosition = new Vector3(0, 0, 0);
                ShotPt.transform.GetComponent<Transform>().localScale = new Vector3(1f, 1f, 1f);
            }
            public void On(float scaleYmax, float scaleYmin, float endTime)
            {
                BeamScaleYMax = scaleYmax;
                BeamScaleYMin = scaleYmin;
                Invoke("Antic", 0f);
                Invoke("Fire", 1f);
                Invoke("End", endTime);
            }
            void Antic()
            {
                BOSS.GetComponent<BossDash>().Dash(Angle + 180f, 6f);
                Beam.transform.eulerAngles = new Vector3(0, 0, Angle);
                Beam1.transform.eulerAngles = new Vector3(0, 0, Angle + 90);
                Beam2.transform.eulerAngles = new Vector3(0, 0, Angle - 90);
                Beam3.transform.eulerAngles = new Vector3(0, 0, Angle + 45);
                Beam4.transform.eulerAngles = new Vector3(0, 0, Angle - 45);
                Beam.LocateMyFSM("Control").SetState("Antic");
                Beam.LocateMyFSM("Control").SendEvent("ANTIC");

                Beam1.GetComponent<BeamSmallAuto>().Antic();
                Beam2.GetComponent<BeamSmallAuto>().Antic();
                Beam3.GetComponent<BeamSmallAuto>().Antic();
                Beam4.GetComponent<BeamSmallAuto>().Antic();
                AnticAudio();
                PtStart();
                Invoke("PtEnd", 0.7f);
            }
            void Fire()
            {
                if (ShieldOn)
                {
                    BOSS.GetComponent<Audios>().Play(5);
                    angleSpeed = 0f;
                    BeamScaleY = BeamScaleYMax;
                    fireOn = true;
                    Beam.LocateMyFSM("Control").SendEvent("FIRE");
                    FireAudio();
                    backSpeed = 0f;
                    ShotPt.transform.GetComponent<ParticleSystem>().enableEmission = true;
                    Beam.GetComponent<Beam3Blast>().On(BeamScaleYMin);
                }
                else
                {

                    Beam.LocateMyFSM("Control").SendEvent("FIRE");
                    Beam.LocateMyFSM("Control").SendEvent("END");
                    fireOn = false;
                    ShotPt.transform.GetComponent<ParticleSystem>().enableEmission = false;
                }
            }
            void End()
            {
                Beam.LocateMyFSM("Control").SendEvent("END");
                fireOn = false;
                ShotPt.transform.GetComponent<ParticleSystem>().enableEmission = false;
                Beam.GetComponent<Beam3Blast>().StopLoop();
            }
            void AnticAudio()
            {
                AudioClip Antic = BOSS.LocateMyFSM("Attack Commands").GetState("EB 1").GetAction<AudioPlaySimple>().oneShotClip.Value as AudioClip;
                BOSS.GetComponent<AudioSource>().PlayOneShot(Antic, 1f);
            }
            void FireAudio()
            {
                AudioClip Fire = BOSS.LocateMyFSM("Attack Commands").GetState("EB 1").GetAction<AudioPlayerOneShotSingle>().audioClip.Value as AudioClip;
                BOSS.GetComponent<AudioSource>().PlayOneShot(Fire, 0.7f);
            }
            void PtStart()
            {
                ChargePt.gameObject.GetComponent<ParticleSystem>().enableEmission = true;
            }
            void PtEnd()
            {
                ChargePt.gameObject.GetComponent<ParticleSystem>().enableEmission = false;
            }
            void FixedUpdate()
            {
                BeamScaleY += (BeamScaleYMin - BeamScaleY) * Time.deltaTime * 3.5f;
                Beam.transform.localScale = new Vector3(40f, 1.6f * BeamScaleY, 1.5f);
                if (Math.Sign(Beam.transform.position.x - HeroController.instance.gameObject.transform.position.x) < 0)
                {
                    ADDANGLE = 180;
                }
                else
                {
                    ADDANGLE = 0;
                }
                //Vector3 forwardInWorld = Beam.transform.InverseTransformDirection(HeroController.instance.transform.forward);
                //Angle = Vector3.Angle(forwardInWorld, Vector3.forward);;
                if (Beam.transform.position.x - HeroController.instance.gameObject.transform.position.x < 0)
                {
                    Angle = (float)RadiansToDegrees(Math.Atan((Beam.transform.position.y - HeroController.instance.gameObject.transform.position.y) / (Beam.transform.position.x - HeroController.instance.gameObject.transform.position.x)));
                }
                else
                {
                    Angle = (float)RadiansToDegrees(Math.Atan((Beam.transform.position.y - HeroController.instance.gameObject.transform.position.y) / (Beam.transform.position.x - HeroController.instance.gameObject.transform.position.x))) + 180;
                }
                float angle = Angle - Beam.transform.eulerAngles.z;
                float angleSpeedAdd;

                if (fireOn)
                {
                    if (angle > 180f)
                    {
                        angleSpeedAdd = Math.Sign(angle - 360) * (float)Math.Min((float)Math.Abs((angle - 360) * Time.deltaTime * angleSpeedAddFactor), angleSpeedAddMax * Time.deltaTime * 4);
                        angleSpeed += angleSpeedAdd - angleSpeed * angleSpeedMinusFactor * Time.deltaTime;
                        //Beam.transform.eulerAngles = new Vector3(0, 0, angle - 360) * Time.deltaTime * 3;
                    }
                    else if (angle < -180f)
                    {
                        angleSpeedAdd = Math.Sign(angle + 360) * (float)Math.Min((float)Math.Abs((angle + 360) * Time.deltaTime * angleSpeedAddFactor), angleSpeedAddMax * Time.deltaTime * 4);
                        angleSpeed += angleSpeedAdd - angleSpeed * angleSpeedMinusFactor * Time.deltaTime;
                        //Beam.transform.eulerAngles += new Vector3(0, 0, angle + 360) * Time.deltaTime * 3;
                    }
                    else
                    {
                        angleSpeedAdd = Math.Sign(angle) * (float)Math.Min((float)Math.Abs((angle) * Time.deltaTime * angleSpeedAddFactor), angleSpeedAddMax * Time.deltaTime * 4);
                        angleSpeed += angleSpeedAdd - angleSpeed * angleSpeedMinusFactor * Time.deltaTime;
                        //Beam.transform.eulerAngles += new Vector3(0, 0, angle) * Time.deltaTime * 3;
                    }
                    Beam.transform.Rotate(0, 0, angleSpeed, Space.Self);

                    backSpeed += backSpeedAdd * Time.deltaTime;
                    x = (float)Math.Cos(DegreesToRadians(Beam.transform.eulerAngles.z + 180f));
                    y = (float)Math.Sin(DegreesToRadians(Beam.transform.eulerAngles.z + 180f));
                }
                gameObject.transform.position += new Vector3(x, y, 0) * backSpeed;
                backSpeed /= 1f + Time.deltaTime * 3f;
            }
        }
        public class BeamSmallAuto : MonoBehaviour
        {
            float OrigAngle;
            bool antic = false;
            public void SetAngle(float angle)
            {
                OrigAngle = angle;
            }
            public void Antic()
            {
                antic = true;
                gameObject.SetActive(true);
                gameObject.LocateMyFSM("Control").SetState("Antic");
                gameObject.LocateMyFSM("Control").SendEvent("ANTIC");
                Invoke("End", 1f);
            }
            void End()
            {
                antic = false;
                gameObject.transform.localEulerAngles = new Vector3(0, 0, OrigAngle);
                gameObject.SetActive(false);
            }
            void FixedUpdate()
            {
                if(antic)
                {
                    gameObject.transform.Rotate(0, 0, -OrigAngle * Time.deltaTime);
                }
            }
        }
        public class BeamSkill2 : MonoBehaviour
        {
            GameObject Beam;
            public float Angle;
            float BeamScaleY = 1.1f;
            float BeamScaleYMax = 1.1f;
            float BeamScaleYMin = 0.8f;
            float ADDANGLE;
            float angleSpeed = 0f;
            float angleSpeedMinusFactor = 8f;
            float angleSpeedAddMax = 10f;
            float angleSpeedAddFactor = 1.3f;
            void Start()
            {
                BeamSummon();
            }
            void BeamSummon()
            {
                Beam = Instantiate(BEAM, BOSS.transform.position, Quaternion.Euler(0, 0, 90));
                Beam.transform.localScale = new Vector3(Beam.transform.localScale.x, Beam.transform.localScale.y * 0.8f, Beam.transform.localScale.z);
            }
            public void On(float scaleYmax, float scaleYmin)
            {
                BeamScaleYMax = scaleYmax;
                BeamScaleYMin = scaleYmin;
                BOSS.GetComponent<BossDash>().DashAuto();
                Invoke("Antic", 0.4f);
                Invoke("Fire", 1.4f);
                Invoke("End", 1.5f);
                Invoke("Antic", 1.6f);
                Invoke("Fire", 2.25f);
                Invoke("End", 2.35f);
                Invoke("Antic", 2.45f);
                Invoke("Fire", 3.1f);
                Invoke("End", 3.2f);
            }
            void Antic()
            {
                if (ShieldOn)
                {
                    Beam.LocateMyFSM("Control").SetState("Antic");
                    Beam.LocateMyFSM("Control").SendEvent("ANTIC");
                }
                else
                {
                    CancelInvoke("Antic");
                    CancelInvoke("FIRE");
                    CancelInvoke("END");
                }
                AnticAudio();
            }
            void Fire()
            {
                if(ShieldOn)
                {
                    BeamScaleY = BeamScaleYMax;
                    Beam.LocateMyFSM("Control").SendEvent("FIRE");
                    FireAudio();
                }
                else
                {
                    Beam.LocateMyFSM("Control").SendEvent("FIRE");
                    Beam.LocateMyFSM("Control").SendEvent("END");
                    CancelInvoke("Antic");
                    CancelInvoke("FIRE");
                    CancelInvoke("END");
                }
            }
            void End()
            {
                Beam.LocateMyFSM("Control").SendEvent("END");
            }
            void AnticAudio()
            {
                AudioClip Antic = BOSS.LocateMyFSM("Attack Commands").GetState("EB 1").GetAction<AudioPlaySimple>().oneShotClip.Value as AudioClip;
                BOSS.GetComponent<AudioSource>().PlayOneShot(Antic, 1f);
            }
            void FireAudio()
            {
                AudioClip Fire = BOSS.LocateMyFSM("Attack Commands").GetState("EB 1").GetAction<AudioPlayerOneShotSingle>().audioClip.Value as AudioClip;
                BOSS.GetComponent<AudioSource>().PlayOneShot(Fire, 1f);
            }
            void FixedUpdate()
            {
                BeamScaleY += (BeamScaleYMin - BeamScaleY) * Time.deltaTime * 8f;
                Beam.transform.localScale = new Vector3(40f, 1.6f * BeamScaleY, 1.5f);
                if (Math.Sign(Beam.transform.position.x - HeroController.instance.gameObject.transform.position.x) < 0)
                {
                    ADDANGLE = 180;
                }
                else
                {
                    ADDANGLE = 0;
                }
                //Vector3 forwardInWorld = Beam.transform.InverseTransformDirection(HeroController.instance.transform.forward);
                //Angle = Vector3.Angle(forwardInWorld, Vector3.forward);
                Angle = (float)RadiansToDegrees(Math.Atan((Beam.transform.position.y - HeroController.instance.gameObject.transform.position.y) / (Beam.transform.position.x - HeroController.instance.gameObject.transform.position.x))) + 90;
                if (Beam.transform.position.x - HeroController.instance.gameObject.transform.position.x < 0)
                {
                    Angle += -90f;
                }
                else
                {
                    Angle += 90f;
                }
                float angle = Angle - Beam.transform.eulerAngles.z;
                float angleSpeedAdd;
                if (HALO1 != null)
                {
                    Beam.transform.position = HALO1.transform.position;
                }
                //Beam.transform.eulerAngles = new Vector3(0, 0, Angle);
                if (angle > 180f)
                {
                    angleSpeedAdd = Math.Sign(angle - 360) * (float)Math.Min((float)Math.Abs((angle - 360) * Time.deltaTime * angleSpeedAddFactor), angleSpeedAddMax * Time.deltaTime * 4);
                    angleSpeed += angleSpeedAdd - angleSpeed * angleSpeedMinusFactor * Time.deltaTime;
                    Beam.transform.Rotate(0, 0, angleSpeed, Space.Self);
                    //Beam.transform.eulerAngles = new Vector3(0, 0, angle - 360) * Time.deltaTime * 3;
                }
                else if(angle < -180f)
                {
                    angleSpeedAdd = Math.Sign(angle + 360) * (float)Math.Min((float)Math.Abs((angle + 360) * Time.deltaTime * angleSpeedAddFactor), angleSpeedAddMax * Time.deltaTime * 4);
                    angleSpeed += angleSpeedAdd - angleSpeed * angleSpeedMinusFactor * Time.deltaTime;
                    Beam.transform.Rotate(0, 0, angleSpeed, Space.Self);
                    //Beam.transform.eulerAngles += new Vector3(0, 0, angle + 360) * Time.deltaTime * 3;
                }
                else
                {
                    angleSpeedAdd = Math.Sign(angle) * (float)Math.Min((float)Math.Abs((angle) * Time.deltaTime * angleSpeedAddFactor), angleSpeedAddMax * Time.deltaTime * 4);
                    angleSpeed += angleSpeedAdd - angleSpeed * angleSpeedMinusFactor * Time.deltaTime;
                    Beam.transform.Rotate(0, 0, angleSpeed, Space.Self);
                    //Beam.transform.eulerAngles += new Vector3(0, 0, angle) * Time.deltaTime * 3;
                }
            }
        }
        public class BeamSkill4 : MonoBehaviour
        {
            public List<GameObject> Beams = new List<GameObject>();
            int Count = 7;
            int MoreNum = 0;
            float R = 4f;
            double RX0 => random.NextDouble();
            public float RX => (float)RX0;
            public float SIGHX => random.Next(6, 13);
            void Start()
            {
                WAVE1 = Instantiate(WAVE_Prefab, gameObject.transform.position + new Vector3(0, 2f, 0), new Quaternion(), gameObject.transform);
                WAVE1.transform.Find("wave 2").gameObject.SetActive(false);
            }
            public void On(int moreBeamNum)
            {
                MoreNum = moreBeamNum;
                Invoke("SummonLoop", 1f);
                Invoke("SummonLoop", 1.7f);
                Invoke("SummonLoop", 2.4f);
                Invoke("SummonLoop", 3.1f);
            }
            void SummonLoop()
            {
                if(ShieldOn)
                {
                    Count = (int)SIGHX + MoreNum;
                    R = 5f + RX * 1f;
                    SummonBeam(Count, R);
                }
                else
                {
                    CancelInvoke("SummonLoop");
                    CancelInvoke("SummonLoop");
                    CancelInvoke("SummonLoop");
                }
            }
            void SummonBeam(int count, float r)
            {
                HALO1.GetComponent<HaloScaleChange_Small>().Flash();
                HALO1.GetComponent<HaloRotateChange_Small>().Flash();
                for (int i = 0;i < count;i++)
                {
                    var beam1 = Instantiate(BEAM, HALO1.transform.position, Quaternion.Euler(0, 0, i * 360 / count));
                    beam1.transform.localScale = new Vector3(beam1.transform.localScale.x, beam1.transform.localScale.y * 0.3f, beam1.transform.localScale.z);
                    beam1.AddComponent<BeamAuto_flower>();
                    beam1.GetComponent<BeamAuto_flower>().SetR(r);
                    var beam2 = Instantiate(BEAM, HALO1.transform.position, Quaternion.Euler(0, 0, i * 360 / count + 180));
                    beam2.transform.localScale = new Vector3(beam2.transform.localScale.x, beam2.transform.localScale.y * 0.3f, beam2.transform.localScale.z);
                    beam2.AddComponent<BeamAuto_flower_Reverse>();
                    beam2.GetComponent<BeamAuto_flower_Reverse>().SetR(r);
                }
                WaveActive();
            }
            void WaveActive()
            {
                BOSS.GetComponent<AudioSource>().PlayOneShot(ORBIMPACT, 1f);
                WAVE1.SetActive(true);
                Invoke("WaveClose", 0.3f);
            }
            void WaveClose()
            {
                WAVE1.SetActive(false);
            }
        }
        public class BeamSkill5 : MonoBehaviour
        {
            public List<GameObject> Beams = new List<GameObject>();
            int Count = 7;
            int MoreCount = 2;
            float R = 4f;
            double RX0 => random.NextDouble();
            public float RX => (float)RX0;
            public float SIGHX => random.Next(5, 11);
            void Start()
            {
                WAVE1 = Instantiate(WAVE_Prefab, gameObject.transform.position + new Vector3(0, 2f, 0), new Quaternion(), gameObject.transform);
                WAVE1.transform.Find("wave 2").gameObject.SetActive(false);
            }
            public void On(int moreCount)
            {
                MoreCount = moreCount;
                Invoke("SummonLoop", RX);
            }
            void SummonLoop()
            {
                if(ShieldOn)
                {
                    Count = (int)SIGHX + MoreCount;
                    R = 5f + RX * 2.5f;
                    SummonBeam(Count, R);
                }
                else
                {
                    CancelInvoke("SummonLoop");
                }
            }
            void SummonBeam(int count, float r)
            {
                HALO1.GetComponent<HaloScaleChange_Small>().Flash();
                HALO1.GetComponent<HaloRotateChange_Small>().Flash();
                for (int i = 0;i < count;i++)
                {
                    var beam1 = Instantiate(BEAM, HALO1.transform.position, Quaternion.Euler(0, 0, i * 360 / count));
                    beam1.transform.localScale = new Vector3(beam1.transform.localScale.x, beam1.transform.localScale.y * 0.3f, beam1.transform.localScale.z);
                    beam1.AddComponent<BeamAuto_flower>();
                    beam1.GetComponent<BeamAuto_flower>().SetR(r);
                    var beam2 = Instantiate(BEAM, HALO1.transform.position, Quaternion.Euler(0, 0, i * 360 / count + 180));
                    beam2.transform.localScale = new Vector3(beam2.transform.localScale.x, beam2.transform.localScale.y * 0.3f, beam2.transform.localScale.z);
                    beam2.AddComponent<BeamAuto_flower_Reverse>();
                    beam2.GetComponent<BeamAuto_flower_Reverse>().SetR(r);
                }
                WaveActive();
            }
            void WaveActive()
            {
                BOSS.GetComponent<AudioSource>().PlayOneShot(ORBIMPACT, 1f);
                WAVE1.SetActive(true);
                Invoke("WaveClose", 0.3f);
            }
            void WaveClose()
            {
                WAVE1.SetActive(false);
            }
        }
        //
        public class BeamSkill6 : MonoBehaviour
        {
            double RX0 => random.NextDouble();
            double RY0 => random.NextDouble();
            public float SIGHX => (random.Next(0, 2) * 2 - 1);
            public float SIGHY => (random.Next(0, 2) * 2 - 1);
            public float RX => (float)(RX0 * SIGHX);
            public float RY => (float)(RY0 * SIGHY);
            float Factor1 = 1;
            float Factor2 = 1;
            float LoopTime = 1;
            float X = 1;
            public bool p2 = false;
            public bool p4 = false;
            Vector3 pos1 = new Vector3(41f, 35.2f, 0f);
            Vector3 pos2 = new Vector3(43.5f, 35.2f, 0f);

            Vector3 pos3 = new Vector3(46f, 42.3f, 0f);
            Vector3 pos4 = new Vector3(48f, 42.3f, 0f);

            Vector3 pos5 = new Vector3(49.7f, 36.1f, 0f);
            Vector3 pos6 = new Vector3(53.5f, 36.1f, 0f);

            Vector3 pos7 = new Vector3(57.6f, 44.7f, 0f);
            Vector3 pos8 = new Vector3(61.7f, 44.7f, 0f);

            Vector3 pos9 = new Vector3(61.7f, 33.3f, 0f);
            Vector3 pos10 = new Vector3(62.3f, 33.3f, 0f);

            Vector3 pos11 = new Vector3(66.7f, 37.8f, 0f);
            Vector3 pos12 = new Vector3(69.0f, 37.8f, 0f);

            Vector3 pos13 = new Vector3(71.5f, 43.8f, 0f);
            Vector3 pos14 = new Vector3(74.1f, 43.8f, 0f);

            Vector3 pos15 = new Vector3(56.8f, 152.4f, 0f);
            Vector3 pos16 = new Vector3(59.3f, 152.4f, 0f);

            Vector3 pos17 = new Vector3(66.6f, 152.4f, 0f);
            Vector3 pos18 = new Vector3(69.1f, 152.4f, 0f);

            Vector3 pos19 = new Vector3(76.3f, 152.4f, 0f);
            Vector3 pos20 = new Vector3(78.8f, 152.4f, 0f);

            Vector3 pos21 = new Vector3(47.5f, 152.4f, 0f);
            Vector3 pos22 = new Vector3(50.1f, 152.4f, 0f);
            public void On(float loopTime, float endTime, float beamScaleFactor, bool One)
            {
                Factor1 = beamScaleFactor;
                LoopTime = loopTime;
                SummonLoop();
                Invoke("SummonEnd", endTime);
                if(One)
                {
                    SummonOne();
                }
            }
            void SummonOne()
            {
                float summonX = BOSS.transform.position.x;
                float summonY = 20;
                float scaleFactor = 3f * Factor1;
                if (p4)
                {
                    summonX = 4 * RX + HeroController.instance.transform.position.x;
                    if (summonX > pos15.x && summonX < pos16.x)
                    {
                        summonY = pos15.y;
                    }
                    else if (summonX > pos17.x && summonX < pos18.x)
                    {
                        summonY = pos17.y;
                    }
                    else if (summonX > pos19.x && summonX < pos20.x)
                    {
                        summonY = pos19.y;
                    }
                    else if (summonX > pos21.x && summonX < pos22.x)
                    {
                        summonY = pos21.y;
                    }
                    else
                    {
                        summonY = 148f;
                    }
                }
                else
                {
                    if (p2)
                    {
                        if (summonX > pos1.x && summonX < pos2.x)
                        {
                            summonY = pos1.y;
                        }
                        else if (summonX > pos3.x && summonX < pos4.x)
                        {
                            summonY = pos3.y;
                        }
                        else if (summonX > pos5.x && summonX < pos6.x)
                        {
                            summonY = pos5.y;
                        }
                        else if (summonX > pos7.x && summonX < pos8.x)
                        {
                            summonY = pos7.y;
                        }
                        else if (summonX > pos9.x && summonX < pos10.x)
                        {
                            summonY = pos9.y;
                        }
                        else if (summonX > pos11.x && summonX < pos12.x)
                        {
                            summonY = pos11.y;
                        }
                        else if (summonX > pos13.x && summonX < pos14.x)
                        {
                            summonY = pos13.y;
                        }
                    }
                }
                var beam = Instantiate(BEAM, new Vector3(summonX, summonY, 0), Quaternion.Euler(0, 0, 90));
                beam.transform.localScale *= Factor1 * 1.5f;
                beam.AddComponent<BeamAuto_Big>();
                StartCoroutine(DelayedExecution(1f, scaleFactor));
                IEnumerator DelayedExecution(float time, float impactScaleFactor)
                {
                    yield return new WaitForSeconds(time);
                    Impact(summonX, summonY, impactScaleFactor);
                }
            }
            void SummonLoop()
            {
                float summonX = 8 * RX + HeroController.instance.transform.position.x;
                float summonY = 20;
                float scaleFactor = 3f * Factor1;
                if(p4)
                {
                    summonX = 4 * RX + HeroController.instance.transform.position.x;
                    if (summonX > pos15.x && summonX < pos16.x)
                    {
                        summonY = pos15.y;
                    }
                    else if (summonX > pos17.x && summonX < pos18.x)
                    {
                        summonY = pos17.y;
                    }
                    else if (summonX > pos19.x && summonX < pos20.x)
                    {
                        summonY = pos19.y;
                    }
                    else if (summonX > pos21.x && summonX < pos22.x)
                    {
                        summonY = pos21.y;
                    }
                    else
                    {
                        summonY = 148f;
                    }
                }
                else
                {
                    if (p2)
                    {
                        if (summonX > pos1.x && summonX < pos2.x)
                        {
                            summonY = pos1.y;
                        }
                        else if (summonX > pos3.x && summonX < pos4.x)
                        {
                            summonY = pos3.y;
                        }
                        else if (summonX > pos5.x && summonX < pos6.x)
                        {
                            summonY = pos5.y;
                        }
                        else if (summonX > pos7.x && summonX < pos8.x)
                        {
                            summonY = pos7.y;
                        }
                        else if (summonX > pos9.x && summonX < pos10.x)
                        {
                            summonY = pos9.y;
                        }
                        else if (summonX > pos11.x && summonX < pos12.x)
                        {
                            summonY = pos11.y;
                        }
                        else if (summonX > pos13.x && summonX < pos14.x)
                        {
                            summonY = pos13.y;
                        }
                    }
                }
                var beam = Instantiate(BEAM, new Vector3(summonX, summonY, 0), Quaternion.Euler(0, 0, 90));
                beam.transform.localScale *= Factor1;
                beam.AddComponent<BeamAuto_Small>();
                StartCoroutine(DelayedExecution(0.75f, scaleFactor));
                IEnumerator DelayedExecution(float time, float impactScaleFactor)
                {
                    yield return new WaitForSeconds(time);
                    Impact(summonX, summonY, impactScaleFactor);
                }
                Invoke("SummonLoop", LoopTime);
            }
            void SummonEnd()
            {
                CancelInvoke("SummonLoop");
            }
            void Impact(float x, float y, float impactScaleFactor)
            {
                var orb = Instantiate(ORB, new Vector3(x, y, -0.001f), Quaternion.Euler(0, 0, RY * 180f));
                var pt = orb.transform.Find("Impact Particles").gameObject;
                pt.transform.SetParent(null);
                if(settings_Pt_.on)
                {
                    pt.GetComponent<ParticleSystem>().emissionRate = 400;
                }
                else
                {
                    pt.GetComponent<ParticleSystem>().emissionRate = 2000;
                }
                pt.GetComponent<ParticleSystem>().startSpeed = 150;
                pt.AddComponent<ObjRecycle>();
                var heroHurter = orb.transform.Find("Hero Hurter").gameObject;
                heroHurter.SetActive(false);
                heroHurter.transform.SetParent(pt.transform);
                var impact = orb.transform.Find("Impact").gameObject;
                impact.GetComponent<RandomScale>().enabled = false;
                impact.transform.SetParent(pt.transform);
                orb.transform.position -= new Vector3(0, 50, 0);
                orb.LocateMyFSM("Orb Control").SetState("Recycle");
                pt.transform.localScale *= 1.75f;
                pt.AddComponent<OrbBlast_Quick>();
                pt.transform.localScale *= Factor2;
                impact.transform.localScale *= Factor2;
                heroHurter.transform.localScale *= Factor2;
            }
        }
        public class BeamSkillP4 : MonoBehaviour
        {
            double RX0 => random.NextDouble();
            double RY0 => random.NextDouble();
            public float SIGHX => (random.Next(0, 2) * 2 - 1);
            public float SIGHY => (random.Next(0, 2) * 2 - 1);
            public float RX => (float)(RX0 * SIGHX);
            public float RY => (float)(RY0 * SIGHY);
            float Factor1 = 1;
            float Factor2 = 1;
            float LoopTime = 1;
            Vector3 pos15 = new Vector3(56.8f, 152.4f, 0f);
            Vector3 pos16 = new Vector3(59.3f, 152.4f, 0f);

            Vector3 pos17 = new Vector3(66.6f, 152.4f, 0f);
            Vector3 pos18 = new Vector3(69.1f, 152.4f, 0f);

            Vector3 pos19 = new Vector3(76.3f, 152.4f, 0f);
            Vector3 pos20 = new Vector3(78.8f, 152.4f, 0f);

            Vector3 pos21 = new Vector3(47.5f, 152.4f, 0f);
            Vector3 pos22 = new Vector3(50.1f, 152.4f, 0f);
            public void On(float loopTime, float endTime, float beamScaleFactor, bool One)
            {
                Factor1 = beamScaleFactor;
                LoopTime = loopTime;
                SummonLoop();
                Invoke("SummonEnd", endTime);
                if (One)
                {
                    SummonOne();
                }
            }
            void SummonOne()
            {
                float summonY = 20;
                float scaleFactor = 3f * Factor1;
                float summonX = 4 * RX + HeroController.instance.transform.position.x;
                if (summonX > pos15.x && summonX < pos16.x)
                {
                    summonY = pos15.y;
                }
                else if (summonX > pos17.x && summonX < pos18.x)
                {
                    summonY = pos17.y;
                }
                else if (summonX > pos19.x && summonX < pos20.x)
                {
                    summonY = pos19.y;
                }
                else if (summonX > pos21.x && summonX < pos22.x)
                {
                    summonY = pos21.y;
                }
                else
                {
                    summonY = 148f;
                }
                var beam = Instantiate(BEAM, new Vector3(summonX, summonY, 0), Quaternion.Euler(0, 0, 90));
                beam.transform.localScale *= Factor1 * 1.5f;
                beam.AddComponent<BeamAuto_Big>();
                StartCoroutine(DelayedExecution(1f, scaleFactor));
                IEnumerator DelayedExecution(float time, float impactScaleFactor)
                {
                    yield return new WaitForSeconds(time);
                    Impact(summonX, summonY, impactScaleFactor);
                }
            }
            void SummonLoop()
            {
                float summonY = 20;
                float scaleFactor = 3f * Factor1;
                float summonX = 4 * RX + HeroController.instance.transform.position.x;
                if (summonX > pos15.x && summonX < pos16.x)
                {
                    summonY = pos15.y;
                }
                else if (summonX > pos17.x && summonX < pos18.x)
                {
                    summonY = pos17.y;
                }
                else if (summonX > pos19.x && summonX < pos20.x)
                {
                    summonY = pos19.y;
                }
                else if (summonX > pos21.x && summonX < pos22.x)
                {
                    summonY = pos21.y;
                }
                else
                {
                    summonY = 148f;
                }
                var beam = Instantiate(BEAM, new Vector3(summonX, summonY, 0), Quaternion.Euler(0, 0, 90));
                beam.transform.localScale *= Factor1;
                beam.AddComponent<BeamAuto_Small>();
                StartCoroutine(DelayedExecution(0.75f, scaleFactor));
                IEnumerator DelayedExecution(float time, float impactScaleFactor)
                {
                    yield return new WaitForSeconds(time);
                    Impact(summonX, summonY, impactScaleFactor);
                }
                Invoke("SummonLoop", LoopTime);
            }
            void SummonEnd()
            {
                CancelInvoke("SummonLoop");
            }
            void Impact(float x, float y, float impactScaleFactor)
            {
                var orb = Instantiate(ORB, new Vector3(x, y, -0.001f), Quaternion.Euler(0, 0, RY * 180f));
                var pt = orb.transform.Find("Impact Particles").gameObject;
                pt.transform.SetParent(null);
                if (settings_Pt_.on)
                {
                    pt.GetComponent<ParticleSystem>().emissionRate = 400;
                }
                else
                {
                    pt.GetComponent<ParticleSystem>().emissionRate = 2000;
                }
                pt.GetComponent<ParticleSystem>().startSpeed = 150;
                pt.AddComponent<ObjRecycle>();
                var heroHurter = orb.transform.Find("Hero Hurter").gameObject;
                heroHurter.SetActive(false);
                heroHurter.transform.SetParent(pt.transform);
                var impact = orb.transform.Find("Impact").gameObject;
                impact.GetComponent<RandomScale>().enabled = false;
                impact.transform.SetParent(pt.transform);
                orb.transform.position -= new Vector3(0, 50, 0);
                orb.LocateMyFSM("Orb Control").SetState("Recycle");
                pt.transform.localScale *= 1.75f;
                pt.AddComponent<OrbBlast_Quick>();
                pt.transform.localScale *= Factor2;
                impact.transform.localScale *= Factor2;
                heroHurter.transform.localScale *= Factor2;
            }
        }
        public class BeamSkillRage : MonoBehaviour
        {
            double RX0 => random.NextDouble();
            double RY0 => random.NextDouble();
            public float SIGHX => (random.Next(0, 2) * 2 - 1);
            public float SIGHY => (random.Next(0, 2) * 2 - 1);
            public float RX => (float)(RX0 * SIGHX);
            public float RY => (float)(RY0 * SIGHY);
            float Factor1 = 1;
            float Factor2 = 1;
            float LoopTime = 1;
            float X = 1;
            public bool start = true;
            public bool started = false;
            Vector3 pos1 = new Vector3(41f, 35.2f, 0f);
            Vector3 pos2 = new Vector3(43.5f, 35.2f, 0f);

            Vector3 pos3 = new Vector3(46f, 42.3f, 0f);
            Vector3 pos4 = new Vector3(48f, 42.3f, 0f);

            Vector3 pos5 = new Vector3(49.7f, 36.1f, 0f);
            Vector3 pos6 = new Vector3(53.5f, 36.1f, 0f);

            Vector3 pos7 = new Vector3(57.6f, 44.7f, 0f);
            Vector3 pos8 = new Vector3(61.7f, 44.7f, 0f);

            Vector3 pos9 = new Vector3(61.7f, 33.3f, 0f);
            Vector3 pos10 = new Vector3(62.3f, 33.3f, 0f);

            Vector3 pos11 = new Vector3(66.7f, 37.8f, 0f);
            Vector3 pos12 = new Vector3(69.0f, 37.8f, 0f);

            Vector3 pos13 = new Vector3(71.5f, 43.8f, 0f);
            Vector3 pos14 = new Vector3(74.1f, 43.8f, 0f);

            Vector3 pos15 = new Vector3(56.8f, 152.4f, 0f);
            Vector3 pos16 = new Vector3(59.3f, 152.4f, 0f);

            Vector3 pos17 = new Vector3(66.6f, 152.4f, 0f);
            Vector3 pos18 = new Vector3(69.1f, 152.4f, 0f);

            Vector3 pos19 = new Vector3(76.3f, 152.4f, 0f);
            Vector3 pos20 = new Vector3(78.8f, 152.4f, 0f);

            Vector3 pos21 = new Vector3(47.5f, 152.4f, 0f);
            Vector3 pos22 = new Vector3(50.1f, 152.4f, 0f);

            private int[] myArray = { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 ,11, 12 };
            public void On(float loopTime, float beamScaleFactor)
            {
                if(!started)
                {
                    started = true;
                    Factor1 = beamScaleFactor;
                    LoopTime = loopTime;
                    start = true;
                    StartLoop();
                }
            }
            public void Off()
            {
                start = false;
                CancelInvoke("StartLoop");
            }
            void StartLoop()
            {
                int num = 0;
                StartCoroutine(DelayedExecution(LoopTime));
                IEnumerator DelayedExecution(float time)
                {
                    yield return new WaitForSeconds(time);
                    var shuffledArray = myArray.OrderBy(x => Guid.NewGuid()).ToArray();

                    float summonX = -12 + shuffledArray[num] * 2f + RX * 3f + HeroController.instance.transform.position.x;

                    num++;

                    float summonY = 20;
                    float scaleFactor = 3f * Factor1;
                    
                    var beam = Instantiate(BEAM, new Vector3(summonX, summonY, 0), Quaternion.Euler(0, 0, 90));
                    beam.transform.localScale *= Factor1;
                    beam.AddComponent<BeamAuto_Small>();
                    StartCoroutine(DelayedExecution1(0.75f, scaleFactor));
                    IEnumerator DelayedExecution1(float time1, float impactScaleFactor)
                    {
                        yield return new WaitForSeconds(time1);
                        Impact(summonX, summonY, impactScaleFactor);
                    }
                    if(start)
                    {
                        if(num < 11)
                        {
                            StartCoroutine(DelayedExecution(LoopTime));
                        }
                        else
                        {
                            Invoke("StartLoop", LoopTime);
                            num = 0;
                        }
                    }
                }
            }
            void Impact(float x, float y, float impactScaleFactor)
            {
                var orb = Instantiate(ORB, new Vector3(x, y, -0.001f), Quaternion.Euler(0, 0, RY * 180f));
                var pt = orb.transform.Find("Impact Particles").gameObject;
                pt.transform.SetParent(null);
                pt.GetComponent<ParticleSystem>().emissionRate = 2000;
                pt.GetComponent<ParticleSystem>().startSpeed = 150;
                pt.AddComponent<ObjRecycle>();
                var heroHurter = orb.transform.Find("Hero Hurter").gameObject;
                heroHurter.SetActive(false);
                heroHurter.transform.SetParent(pt.transform);
                var impact = orb.transform.Find("Impact").gameObject;
                impact.GetComponent<RandomScale>().enabled = false;
                impact.transform.SetParent(pt.transform);
                orb.transform.position -= new Vector3(0, 50, 0);
                orb.LocateMyFSM("Orb Control").SetState("Recycle");
                pt.transform.localScale *= 1.5f;
                pt.AddComponent<OrbBlast_Quick>();
                pt.transform.localScale *= Factor2;
                impact.transform.localScale *= Factor2;
                heroHurter.transform.localScale *= Factor2;
            }
        }
        public class SolarFlareSkill : MonoBehaviour
        {
            System.Random random = new System.Random();
            double RX0 => random.NextDouble();
            double RY0 => random.NextDouble();
            public float SIGHX => (random.Next(0, 2) * 2 - 1);
            public float SIGHY => (random.Next(0, 2) * 2 - 1);
            public float RX => (float)(RX0 * SIGHX);
            public float RY => (float)(RY0 * SIGHY);
            float NailR;
            float Side = 1f;
            float factor = 1f;
            float NailAngle = 0f;
            float RandomAngle = 0f;
            float StrikeAngle;
            float LoopTime = 0.04f;
            int NailCount = 0;
            public List<GameObject> Beams = new List<GameObject>();
            public void On(float loopTime, float endTime)
            {
                float distance = Vector2.Distance(HeroController.instance.gameObject.transform.position, BOSS.transform.position);
                if (distance > 9)
                {
                    NailR = 5 + RX * 2f;
                }
                else
                {
                    NailR = 2.5f + RX * 1.5f;
                }
                loopTime = loopTime;
                Side = SIGHX;
                factor = SIGHY;
                NailCount = 0;
                RandomAngle = RX * 25f;
                NailAngle = BOSS.GetComponent<HeroAngle>().Angle + 270f + RY * 60f;
                StrikeAngle = BOSS.GetComponent<HeroAngle>().Angle;
                Invoke("BeamFire", 0.5f);
                Invoke("FireEnd", endTime);
            }
            void Impact()
            {
                BOSS.GetComponent<OrbImpact>().Impact();
            }
            void FireEnd()
            {
                CancelInvoke("BeamFire");
            }
            void BeamFire()
            {
                if (ShieldOn)
                {
                    var beam = Instantiate(BEAM, BOSS.transform.position + new Vector3(0, 2.5f, 0), Quaternion.Euler(0, 0, RY * 180f));
                    beam.transform.localScale = new Vector3(beam.transform.localScale.x, beam.transform.localScale.y * (0.7f + 0.5f * RX), beam.transform.localScale.z);
                    beam.SetActive(true);
                    beam.AddComponent<BeamAuto_Small>();
                    beam.AddComponent<ObjRecycle>();
                    Beams.Add(beam);
                    AudioClip Antic = BOSS.LocateMyFSM("Attack Commands").GetState("EB 1").GetAction<AudioPlaySimple>().oneShotClip.Value as AudioClip;
                    BOSS.GetComponent<AudioSource>().PlayOneShot(Antic, 0.6f);
                    Invoke("BeamFire", LoopTime);
                }
            }
            void BeamSummonLoop()
            {
                while (NailCount <= 72)
                {
                    NailCount++;
                    NailSummon();
                    NailAngle += 5f;
                }
                Invoke("BeamFire", 0.3f);
            }
            void NailSummon()
            {
                float NailSummonX = (float)Math.Cos(DegreesToRadians(NailAngle)) * NailR;
                float NailSummonY = (float)Math.Sin(DegreesToRadians(NailAngle)) * NailR;
                var beam = Instantiate(BEAM,BOSS.transform.position + new Vector3(0, 2.5f, 0), Quaternion.Euler(0, 0, NailAngle));
                beam.transform.localScale = new Vector3(beam.transform.localScale.x, beam.transform.localScale.y * (0.7f + 0.5f * RX), beam.transform.localScale.z);
                beam.SetActive(true);
                Beams.Add(beam);
            }
        }
        public class OrbMove : MonoBehaviour
        {
            public float AnimX;
            public float AnimY;
            float Timer = 1.5f;
            bool Fired = false;
            void Update()
            {
                Timer -= Time.deltaTime;
                if(Timer > 0)
                {
                    var x = gameObject.transform.position.x;
                    var y = gameObject.transform.position.y;
                    gameObject.transform.position += new Vector3(AnimX - x, AnimY - y, 0f) / 30f;
                }
                else
                {
                    if(!Fired)
                    {
                        Fired = true;
                        gameObject.SetActive(true);
                        gameObject.LocateMyFSM("Orb Control").SetState("Chase Hero 2");
                        gameObject.LocateMyFSM("Orb Control").ChangeTransition("Chase Hero 2", "DISSIPATE", "Impact pause");
                        var hh = gameObject.transform.Find("Hero Hurter").gameObject;
                        hh.GetComponent<CircleCollider2D>().enabled = true;
                    }
                }
            }
        }
        public class OrbMove_2 : MonoBehaviour
        {
            public float AnimX;
            public float AnimY;
            float Timer = 0.2f;
            bool Fired = false;
            void Update()
            {
                Timer -= Time.deltaTime;
                if(Timer > 0)
                {
                    var x = gameObject.transform.position.x;
                    var y = gameObject.transform.position.y;
                    gameObject.transform.position += new Vector3(AnimX - x, AnimY - y, 0f) / 30f;
                }
                else
                {
                    if(!Fired)
                    {
                        Fired = true;
                        gameObject.LocateMyFSM("Orb Control").SetState("Chase Hero 2");
                        var hh = gameObject.transform.Find("Hero Hurter").gameObject;
                        hh.GetComponent<CircleCollider2D>().enabled = true;
                    }
                }
            }
        }
        public class OrbAuto : MonoBehaviour
        {
            public void On(float delayTime)
            {
                Invoke("Fire", delayTime);
            }
            void Fire()
            {
                gameObject.SetActive(true);
                gameObject.LocateMyFSM("Orb Control").SetState("Chase Hero 2");
                gameObject.LocateMyFSM("Orb Control").ChangeTransition("Chase Hero 2", "DISSIPATE", "Impact pause");
            }
        }
        //
        public class OrbSkillP4 : MonoBehaviour
        {
            System.Random random = new System.Random();
            double RX0 => random.NextDouble();
            double RY0 => random.NextDouble();
            public float SIGHX => (random.Next(0, 2) * 2 - 1);
            public float SIGHY => (random.Next(0, 2) * 2 - 1);
            public float RX => (float)(RX0 * SIGHX);
            public float RY => (float)(RY0 * SIGHY);
            public List<GameObject> Orbs = new List<GameObject>();
            float OrbR = 1f;
            float Side = 1f;
            float OrbAngle = 0f;
            float OrbAngleSpeed = 0f;
            float LoopTime = 0.15f;
            int OrbCount = 0;
            int OrbCountMax = 12;
            public void On(int countMax, float loopTime)
            {
                OrbCountMax = countMax;
                LoopTime = loopTime;
                OrbR = 5 + RX * 2f;
                Side = SIGHX;
                OrbCount = 0;
                OrbAngle = BOSS.GetComponent<HeroAngle>().Angle + 180f - 24f * OrbAngleSpeed * Side;
                OrbAngleSpeed = 3 - (float)RY0 * 2;
                OrbSummon();
            }
            void OrbSummon()
            {
                if (ShieldOn)
                {
                    OrbCount++;
                    if (OrbCount > 2)
                    {
                        float RandomAngle = RX * 180f;
                        float OrbSummonX = (float)Math.Cos(DegreesToRadians(OrbAngle)) * OrbR;
                        float OrbSummonY = (float)Math.Sin(DegreesToRadians(OrbAngle)) * OrbR;
                        var orb = Instantiate(ORB, BOSS.transform.position + new Vector3(OrbSummonX / 5, OrbSummonY / 5 + 2.5f, 0), Quaternion.Euler(0, 0, 0));
                        Orbs.Add(orb);
                        var hh = orb.transform.Find("Hero Hurter").gameObject;
                        hh.GetComponent<CircleCollider2D>().enabled = false;
                        orb.AddComponent<OrbMove>();
                        orb.GetComponent<OrbMove>().AnimX = BOSS.transform.position.x + OrbSummonX;
                        orb.GetComponent<OrbMove>().AnimY = BOSS.transform.position.y + OrbSummonY + 2.5f;
                    }
                    OrbAngle += 72f * OrbAngleSpeed + RX * 72;
                    if (OrbCount < OrbCountMax + 2)
                    {
                        Invoke("OrbSummon", LoopTime);
                    }
                    else
                    {
                        foreach (var Orb in Orbs)
                        {
                            Orb.LocateMyFSM("Orb Control").SetState("Chase Hero 2");
                            gameObject.LocateMyFSM("Orb Control").ChangeTransition("Chase Hero 2", "DISSIPATE", "Impact pause");
                        }
                        Orbs.Clear();
                    }
                }
            }
        }
        public class OrbSkill1 : MonoBehaviour
        {
            System.Random random = new System.Random();
            double RX0 => random.NextDouble();
            double RY0 => random.NextDouble();
            public float SIGHX => (random.Next(0, 2) * 2 - 1);
            public float SIGHY => (random.Next(0, 2) * 2 - 1);
            public float RX => (float)(RX0 * SIGHX);
            public float RY => (float)(RY0 * SIGHY);
            public List<GameObject> Orbs = new List<GameObject>();
            float OrbR = 1f;
            float Side = 1f;
            float OrbAngle = 0f;
            float OrbAngleSpeed = 0f;
            float LoopTime = 0.15f;
            int OrbCount = 0;
            int OrbCountMax = 12;
            public void On(int countMax, float loopTime)
            {
                OrbCountMax = countMax;
                LoopTime = loopTime;
                float distance = Vector2.Distance(HeroController.instance.gameObject.transform.position, BOSS.transform.position);
                if (distance > 8)
                {
                    OrbR = 5 + RX * 2f;
                }
                else
                {
                    OrbR = 14 + RX * 2f;
                }
                Side = SIGHX;
                OrbCount = 0;
                OrbAngle = BOSS.GetComponent<HeroAngle>().Angle + 180f - 24f * OrbAngleSpeed * Side;
                OrbAngleSpeed = 3 - (float)RY0 * 2;
                OrbSummon();
            }
            void OrbSummon()
            {
                if (ShieldOn)
                {
                    OrbCount++;
                    if (OrbCount > 2)
                    {
                        float RandomAngle = RX * 180f;
                        float OrbSummonX = (float)Math.Cos(DegreesToRadians(OrbAngle)) * OrbR;
                        float OrbSummonY = (float)Math.Sin(DegreesToRadians(OrbAngle)) * OrbR;
                        var orb = Instantiate(ORB, BOSS.transform.position + new Vector3(OrbSummonX / 5, OrbSummonY / 5 + 2.5f, 0), Quaternion.Euler(0, 0, 0));
                        orb.transform.localScale *= 0.8f + 0.3f * RY;
                        Orbs.Add(orb);
                        var hh = orb.transform.Find("Hero Hurter").gameObject;
                        hh.GetComponent<CircleCollider2D>().enabled = false;
                        orb.AddComponent<OrbMove>();
                        orb.GetComponent<OrbMove>().AnimX = BOSS.transform.position.x + OrbSummonX;
                        orb.GetComponent<OrbMove>().AnimY = BOSS.transform.position.y + OrbSummonY + 2.5f;
                    }
                    OrbAngle += 72f * OrbAngleSpeed + RX * 72;
                    if (OrbCount < OrbCountMax + 2)
                    {
                        Invoke("OrbSummon", LoopTime);
                    }
                    else
                    {
                        foreach (var Orb in Orbs)
                        {
                            Orb.LocateMyFSM("Orb Control").SetState("Chase Hero 2");
                            gameObject.LocateMyFSM("Orb Control").ChangeTransition("Chase Hero 2", "DISSIPATE", "Impact pause");
                        }
                        Orbs.Clear();
                    }
                }
            }
        }
        public class OrbSkill2 : MonoBehaviour
        {
            System.Random random = new System.Random();
            double RX0 => random.NextDouble();
            double RY0 => random.NextDouble();
            public float SIGHX => (random.Next(0, 2) * 2 - 1);
            public float SIGHY => (random.Next(0, 2) * 2 - 1);
            public float RX => (float)(RX0 * SIGHX);
            public float RY => (float)(RY0 * SIGHY);
            public void On()
            {
                BOSS.GetComponent<BossDash>().DashAuto();
                Invoke("Off", 3f);
                Invoke("BlastStart", 1f);
            }
            public void Off()
            {
            }
            void BlastStart()
            {
                if(ShieldOn)
                {
                    BOSS.GetComponent<OrbBlast>().On(12f, 2f);
                }
            }
        }
        public class OrbSkill3 : MonoBehaviour
        {
            System.Random random = new System.Random();
            double RX0 => random.NextDouble();
            double RY0 => random.NextDouble();
            public float SIGHX => (random.Next(0, 2) * 2 - 1);
            public float SIGHY => (random.Next(0, 2) * 2 - 1);
            public float RX => (float)(RX0 * SIGHX);
            public float RY => (float)(RY0 * SIGHY);
            public float SIGHZ => random.Next(0, 3);
            float R = 0f;
            float looptime = 0.15f;
            void BlastStart()
            {
            }
            public void On(float rFactor, float endTime, float loopTime)
            {
                if (ShieldOn)
                {
                    StartCoroutine(DelayedExecution(0.5f, endTime, loopTime));
                    IEnumerator DelayedExecution(float time, float EndTime, float LoopTime)
                    {
                        yield return new WaitForSeconds(time);
                        looptime = LoopTime;
                        SummonBlastLoop();
                        R = rFactor * 7f;
                        Invoke("StopLoop", EndTime);
                    }
                }
            }
            void SummonBlastLoop()
            {
                float angle = RX * (float)Math.PI;
                float random = (RY + 1) / 2;
                Vector3 SummonPosition = new Vector3((float)Math.Cos(angle) * R * random, (float)Math.Sin(angle) * R * random, 0);
                var orb = Instantiate(ORB, HALO1.transform.position + SummonPosition, Quaternion.Euler(0, 0, 0));
                orb.transform.localScale *= 0.8f + 0.3f * RY;
                orb.transform.localScale *= 0.8f + RX * 0.3f;
                orb.AddComponent<OrbAuto>();
                orb.GetComponent<OrbAuto>().On(0f);

                if (ShieldOn)
                {
                    Invoke("SummonBlastLoop", looptime);
                }
            }
            void StopLoop()
            {
                CancelInvoke("SummonBlastLoop");
            }
        }
        public class OrbSkill4 : MonoBehaviour
        {
            GameObject Beam;
            GameObject ChargePt1;
            public float Angle;
            float BeamScaleY = 5.4f;
            float angleSpeed = 0f;
            float angleSpeedMinusFactor = 8f;
            float angleSpeedAddMax = 10f;
            float angleSpeedAddFactor = 1.3f;
            float delayTime = 0.5f;
            bool move = true;
            public List<GameObject> Orbs = new List<GameObject>();
            double RX0 => random.NextDouble();
            double RY0 => random.NextDouble();
            double RZ0 => random.NextDouble();
            public float SIGHX => (random.Next(0, 2) * 2 - 1);
            public float SIGHY => (random.Next(0, 2) * 2 - 1);
            public float SIGHZ => (random.Next(0, 2) * 2 - 1);
            public float RX => (float)(RX0 * SIGHX);
            public float RY => (float)(RY0 * SIGHY);
            public float RZ => (float)(RZ0 * SIGHZ);
            float LoopTime = 0.15f;
            void Start()
            {
                Beam = Instantiate(BEAM, BOSS.transform.position, Quaternion.Euler(0, 0, 90), BOSS.transform);
                Beam.GetComponent<DamageHero>().damageDealt = 4;
                ChargePt1 = Instantiate(DREAMPTCHARG, HALO1.transform.position, new Quaternion(), gameObject.transform);

                ChargePt1.transform.GetComponent<ParticleSystem>().emissionRate = 400f;
                ChargePt1.transform.GetComponent<ParticleSystem>().maxParticles = 9999;
                ChargePt1.transform.GetComponent<ParticleSystem>().startSpeed = -24f;
                ChargePt1.transform.GetComponent<ParticleSystem>().startColor = new Color(1f, 0.9f, 0.73f, 1f);
                ChargePt1.transform.GetComponent<ParticleSystem>().startSize = 1f;
                ChargePt1.transform.GetComponent<ParticleSystem>().startLifetime = 2f;
                ChargePt1.transform.GetComponent<Behaviour>().enabled = true;
                ChargePt1.transform.localPosition = new Vector3(0, 0, 0);
                ChargePt1.transform.localScale = new Vector3(12f, 12f, ChargePt1.transform.localScale.z);
                ChargePt1.gameObject.SetActive(true);
            }
            public void On(float loopTime, float endTime)
            {
                LoopTime = loopTime;
                SummonLoop();
                Invoke("SummonLoopEnd", endTime - 2.6f);
                Invoke("Antic", endTime + delayTime - 1.9f);
                Invoke("Fire", endTime + delayTime - 0.4f);
                Invoke("End", endTime + delayTime - 0.3f);
                Invoke("Wave", endTime + delayTime - 0.3f);
                PtStart();
            }
            void SummonLoop()
            {
                if(ShieldOn)
                {
                    SummonOrb(0.6f + RY * 0.4f);
                    Invoke("SummonLoop", LoopTime);
                }
                else
                {
                    foreach(var orb in Orbs)
                    {
                        orb.LocateMyFSM("Orb Control").SetState("Recycle");
                    }
                }
            }
            void SummonLoopEnd()
            {
                CancelInvoke("SummonLoop");
                //BOSS.LocateMyFSM("Attack Commands").GetState("Orb Summon").GetAction<Wait>().time = 3f;
                PtEnd();
            }
            void SummonOrb(float orbScaleFactor)
            {
                float r = 30 + 4 * RY;
                double angle = RX * Math.PI;
                float x = (float)Math.Cos(angle) * r;
                float y = (float)Math.Sin(angle) * r;
                var orb = Instantiate(ORB, HALO1.transform.position + new Vector3(x, y, 0), Quaternion.Euler(0, 0, 0));
                orb.transform.localScale *= orbScaleFactor;
                orb.SetActive(true);
                orb.LocateMyFSM("Orb Control").GetState("Chase Hero").GetAction<ChaseObjectV2>().target = HALO1;
                orb.LocateMyFSM("Orb Control").GetState("Chase Hero").GetAction<ChaseObjectV2>().accelerationForce = 8f;
                orb.LocateMyFSM("Orb Control").GetState("Chase Hero").GetAction<ChaseObjectV2>().speedMax = 20f;
                orb.LocateMyFSM("Orb Control").GetState("Chase Hero").GetAction<Wait>(2).time = 10f;
                orb.LocateMyFSM("Orb Control").GetState("Chase Hero").GetAction<Wait>(4).time = 10f;
                orb.LocateMyFSM("Orb Control").GetState("Stop Particles").GetAction<Wait>().time = 0f;
                orb.LocateMyFSM("Orb Control").GetState("Stop Particles").AddMethod(()=>
                {
                    orb.transform.Find("Hero Hurter").gameObject.SetActive(false);
                });
                orb.LocateMyFSM("Orb Control").ChangeTransition("Chase Hero", "FINISHED", "Dissipate");
                orb.LocateMyFSM("Orb Control").ChangeTransition("Chase Hero", "ORBIT SHIELD", "Dissipate");
                orb.LocateMyFSM("Orb Control").SetState("Chase Hero");
                orb.AddComponent<OrbCheck>();
                Orbs.Add(orb);
            }
            void PtStart()
            {
                if(settings_Pt_.on)
                {
                    ChargePt1.gameObject.GetComponent<ParticleSystem>().enableEmission = true;
                }
            }
            void PtEnd()
            {
                ChargePt1.gameObject.GetComponent<ParticleSystem>().enableEmission = false;
            }
            void Wave()
            {
                BOSS.GetComponent<AudioSource>().PlayOneShot(ORBIMPACT, 1f);
                WAVE.SetActive(true);
                Invoke("WaveClose", 0.3f);
            }
            void WaveClose()
            {
                WAVE.SetActive(false);
            }
            void Antic()
            {
                if (ShieldOn)
                {
                    move = true;
                    Beam.transform.eulerAngles = new Vector3(0, 0, 90f);
                    Beam.LocateMyFSM("Control").SetState("Antic");
                    Beam.LocateMyFSM("Control").SendEvent("ANTIC");
                    AnticAudio();
                }
            }
            void Fire()
            {
                if (ShieldOn)
                {
                    move = false;
                    Beam.transform.localScale += new Vector3(0, 4, 0);
                    Beam.LocateMyFSM("Control").SendEvent("FIRE");
                    FireAudio();
                }
                else
                {
                    move = false;
                    Beam.LocateMyFSM("Control").SendEvent("FIRE");
                    Beam.LocateMyFSM("Control").SendEvent("END");
                }
            }
            void End()
            {
                Beam.LocateMyFSM("Control").SendEvent("END");
                Orbs.Clear();
            }
            void AnticAudio()
            {
                AudioClip Antic = BOSS.LocateMyFSM("Attack Commands").GetState("EB 1").GetAction<AudioPlaySimple>().oneShotClip.Value as AudioClip;
                BOSS.GetComponent<AudioSource>().PlayOneShot(Antic, 1f);
            }
            void FireAudio()
            {
                AudioClip Fire = BOSS.LocateMyFSM("Attack Commands").GetState("EB 1").GetAction<AudioPlayerOneShotSingle>().audioClip.Value as AudioClip;
                BOSS.GetComponent<AudioSource>().PlayOneShot(Fire, 1f);
            }
            void FixedUpdate()
            {
                if(move)
                {
                    BeamScaleY += (2f - BeamScaleY) * Time.deltaTime * 18f;
                    Beam.transform.localScale = new Vector3(40f, 1.6f * BeamScaleY, 1.5f);
                    Angle = (float)RadiansToDegrees(Math.Atan((Beam.transform.position.y - HeroController.instance.gameObject.transform.position.y) / (Beam.transform.position.x - HeroController.instance.gameObject.transform.position.x))) + 90;
                    if (Beam.transform.position.x - HeroController.instance.gameObject.transform.position.x < 0)
                    {
                        Angle += -90f;
                    }
                    else
                    {
                        Angle += 90f;
                    }
                    float angle = Angle - Beam.transform.eulerAngles.z;
                    float angleSpeedAdd;
                    if (HALO1 != null)
                    {
                        Beam.transform.position = HALO1.transform.position;
                    }
                    if (angle > 180f)
                    {
                        angleSpeedAdd = Math.Sign(angle - 360) * (float)Math.Min((float)Math.Abs((angle - 360) * Time.deltaTime * angleSpeedAddFactor), angleSpeedAddMax * Time.deltaTime * 4);
                        angleSpeed += angleSpeedAdd - angleSpeed * angleSpeedMinusFactor * Time.deltaTime;
                        Beam.transform.Rotate(0, 0, angleSpeed, Space.Self);
                    }
                    else if (angle < -180f)
                    {
                        angleSpeedAdd = Math.Sign(angle + 360) * (float)Math.Min((float)Math.Abs((angle + 360) * Time.deltaTime * angleSpeedAddFactor), angleSpeedAddMax * Time.deltaTime * 4);
                        angleSpeed += angleSpeedAdd - angleSpeed * angleSpeedMinusFactor * Time.deltaTime;
                        Beam.transform.Rotate(0, 0, angleSpeed, Space.Self);
                    }
                    else
                    {
                        angleSpeedAdd = Math.Sign(angle) * (float)Math.Min((float)Math.Abs((angle) * Time.deltaTime * angleSpeedAddFactor), angleSpeedAddMax * Time.deltaTime * 4);
                        angleSpeed += angleSpeedAdd - angleSpeed * angleSpeedMinusFactor * Time.deltaTime;
                        Beam.transform.Rotate(0, 0, angleSpeed, Space.Self);
                    }
                }
            }
        }
        public class OrbSkillP3 : MonoBehaviour
        {
            GameObject Beam;
            GameObject ChargePt1;
            public float Angle;
            float BeamScaleY = 5.4f;
            float angleSpeed = 0f;
            float angleSpeedMinusFactor = 8f;
            float angleSpeedAddMax = 10f;
            float angleSpeedAddFactor = 1.3f;
            float delayTime = 0.5f;
            bool move = true;
            public List<GameObject> Orbs = new List<GameObject>();
            double RX0 => random.NextDouble();
            double RY0 => random.NextDouble();
            double RZ0 => random.NextDouble();
            public float SIGHX => (random.Next(0, 2) * 2 - 1);
            public float SIGHY => (random.Next(0, 2) * 2 - 1);
            public float SIGHZ => (random.Next(0, 2) * 2 - 1);
            public float RX => (float)(RX0 * SIGHX);
            public float RY => (float)(RY0 * SIGHY);
            public float RZ => (float)(RZ0 * SIGHZ);
            float LoopTime = 0.15f;
            void Start()
            {
                Beam = Instantiate(BEAM, BOSS.transform.position, Quaternion.Euler(0, 0, 90), BOSS.transform);
                Beam.GetComponent<DamageHero>().damageDealt = 4;
                ChargePt1 = Instantiate(DREAMPTCHARG, HALO1.transform.position, new Quaternion(), gameObject.transform);

                ChargePt1.transform.GetComponent<ParticleSystem>().emissionRate = 400f;
                ChargePt1.transform.GetComponent<ParticleSystem>().maxParticles = 9999;
                ChargePt1.transform.GetComponent<ParticleSystem>().startSpeed = -24f;
                ChargePt1.transform.GetComponent<ParticleSystem>().startColor = new Color(1f, 0.9f, 0.73f, 1f);
                ChargePt1.transform.GetComponent<ParticleSystem>().startSize = 1f;
                ChargePt1.transform.GetComponent<ParticleSystem>().startLifetime = 8f;
                ChargePt1.transform.GetComponent<Behaviour>().enabled = true;
                ChargePt1.transform.localPosition = new Vector3(0, 0, 0);
                ChargePt1.transform.localScale = new Vector3(48f, 48f, ChargePt1.transform.localScale.z);
                ChargePt1.gameObject.SetActive(true);
            }
            public void On(float loopTime, float endTime)
            {
                HALO1.GetComponent<HaloColorChange_Small>().Glow();
                LoopTime = loopTime;
                SummonLoop();
                //Invoke("SummonLoopEnd", endTime - 2.6f);
                //Invoke("Antic", endTime + delayTime - 1.9f);
                //Invoke("Fire", endTime + delayTime - 0.4f);
                //Invoke("End", endTime + delayTime - 0.3f);
                //Invoke("Wave", endTime + delayTime - 0.3f);
                PtStart();
            }
            void SummonLoop()
            {
                float distance = Vector2.Distance(HeroController.instance.transform.position, BOSS.transform.position);
                SummonOrb(0.6f + RY * 0.4f, distance + 12f);
                Invoke("SummonLoop", LoopTime * (0.5f + 3f / distance));
            }
            public void SummonLoopEnd()
            {
                HALO1.GetComponent<HaloColorChange_Small>().GLowEnd();
                CancelInvoke("SummonLoop");
                PtEnd();
                GameObject[] allGameObjects = FindObjectsOfType<GameObject>();
                foreach (GameObject go in allGameObjects)
                {
                    if (go.name.Contains("Orb"))
                    {
                        go.LocateMyFSM("Orb Control").SetState("Recycle");
                    }
                    else if (go.name.Contains("Nail"))
                    {
                        go.LocateMyFSM("Control").SetState("Recycle");
                    }
                }

            }
            void SummonOrb(float orbScaleFactor, float R)
            {
                float r = R;
                double angle = RX * Math.PI;
                float x = (float)Math.Cos(angle) * r;
                float y = (float)Math.Sin(angle) * r;
                var orb = Instantiate(ORB, HALO1.transform.position + new Vector3(x, y, 0), Quaternion.Euler(0, 0, 0));
                orb.transform.localScale *= orbScaleFactor;
                orb.SetActive(true);
                orb.LocateMyFSM("Orb Control").GetState("Chase Hero").GetAction<ChaseObjectV2>().target = HALO1;
                orb.LocateMyFSM("Orb Control").GetState("Chase Hero").GetAction<ChaseObjectV2>().accelerationForce = 10f;
                orb.LocateMyFSM("Orb Control").GetState("Chase Hero").GetAction<ChaseObjectV2>().speedMax = 25f;
                orb.LocateMyFSM("Orb Control").GetState("Chase Hero").GetAction<Wait>(2).time = 10f;
                orb.LocateMyFSM("Orb Control").GetState("Chase Hero").GetAction<Wait>(4).time = 10f;
                orb.LocateMyFSM("Orb Control").GetState("Stop Particles").GetAction<Wait>().time = 0f;
                orb.LocateMyFSM("Orb Control").GetState("Stop Particles").AddMethod(()=>
                {
                    orb.transform.Find("Hero Hurter").gameObject.SetActive(false);
                });
                orb.LocateMyFSM("Orb Control").ChangeTransition("Chase Hero", "FINISHED", "Dissipate");
                orb.LocateMyFSM("Orb Control").ChangeTransition("Chase Hero", "ORBIT SHIELD", "Dissipate");
                orb.LocateMyFSM("Orb Control").SetState("Chase Hero");
                orb.AddComponent<OrbCheck>();
                Orbs.Add(orb);
            }
            void PtStart()
            {
                if(settings_Pt_.on)
                {
                    ChargePt1.gameObject.GetComponent<ParticleSystem>().enableEmission = true;
                }
            }
            void PtEnd()
            {
                ChargePt1.gameObject.GetComponent<ParticleSystem>().enableEmission = false;
            }
            void Wave()
            {
                BOSS.GetComponent<AudioSource>().PlayOneShot(ORBIMPACT, 1f);
                WAVE.SetActive(true);
                Invoke("WaveClose", 0.3f);
            }
            void WaveClose()
            {
                WAVE.SetActive(false);
            }
            void Antic()
            {
                if (ShieldOn)
                {
                    move = true;
                    Beam.transform.eulerAngles = new Vector3(0, 0, 90f);
                    Beam.LocateMyFSM("Control").SetState("Antic");
                    Beam.LocateMyFSM("Control").SendEvent("ANTIC");
                    AnticAudio();
                }
            }
            void Fire()
            {
                if (ShieldOn)
                {
                    move = false;
                    Beam.transform.localScale += new Vector3(0, 4, 0);
                    Beam.LocateMyFSM("Control").SendEvent("FIRE");
                    FireAudio();
                }
                else
                {
                    move = false;
                    Beam.LocateMyFSM("Control").SendEvent("FIRE");
                    Beam.LocateMyFSM("Control").SendEvent("END");
                }
            }
            void End()
            {
                Beam.LocateMyFSM("Control").SendEvent("END");
                Orbs.Clear();
            }
            void AnticAudio()
            {
                AudioClip Antic = BOSS.LocateMyFSM("Attack Commands").GetState("EB 1").GetAction<AudioPlaySimple>().oneShotClip.Value as AudioClip;
                BOSS.GetComponent<AudioSource>().PlayOneShot(Antic, 1f);
            }
            void FireAudio()
            {
                AudioClip Fire = BOSS.LocateMyFSM("Attack Commands").GetState("EB 1").GetAction<AudioPlayerOneShotSingle>().audioClip.Value as AudioClip;
                BOSS.GetComponent<AudioSource>().PlayOneShot(Fire, 1f);
            }
            void FixedUpdate()
            {
                if(move)
                {
                    BeamScaleY += (2f - BeamScaleY) * Time.deltaTime * 18f;
                    Beam.transform.localScale = new Vector3(40f, 1.6f * BeamScaleY, 1.5f);
                    Angle = (float)RadiansToDegrees(Math.Atan((Beam.transform.position.y - HeroController.instance.gameObject.transform.position.y) / (Beam.transform.position.x - HeroController.instance.gameObject.transform.position.x))) + 90;
                    if (Beam.transform.position.x - HeroController.instance.gameObject.transform.position.x < 0)
                    {
                        Angle += -90f;
                    }
                    else
                    {
                        Angle += 90f;
                    }
                    float angle = Angle - Beam.transform.eulerAngles.z;
                    float angleSpeedAdd;
                    if (HALO1 != null)
                    {
                        Beam.transform.position = HALO1.transform.position;
                    }
                    if (angle > 180f)
                    {
                        angleSpeedAdd = Math.Sign(angle - 360) * (float)Math.Min((float)Math.Abs((angle - 360) * Time.deltaTime * angleSpeedAddFactor), angleSpeedAddMax * Time.deltaTime * 4);
                        angleSpeed += angleSpeedAdd - angleSpeed * angleSpeedMinusFactor * Time.deltaTime;
                        Beam.transform.Rotate(0, 0, angleSpeed, Space.Self);
                    }
                    else if (angle < -180f)
                    {
                        angleSpeedAdd = Math.Sign(angle + 360) * (float)Math.Min((float)Math.Abs((angle + 360) * Time.deltaTime * angleSpeedAddFactor), angleSpeedAddMax * Time.deltaTime * 4);
                        angleSpeed += angleSpeedAdd - angleSpeed * angleSpeedMinusFactor * Time.deltaTime;
                        Beam.transform.Rotate(0, 0, angleSpeed, Space.Self);
                    }
                    else
                    {
                        angleSpeedAdd = Math.Sign(angle) * (float)Math.Min((float)Math.Abs((angle) * Time.deltaTime * angleSpeedAddFactor), angleSpeedAddMax * Time.deltaTime * 4);
                        angleSpeed += angleSpeedAdd - angleSpeed * angleSpeedMinusFactor * Time.deltaTime;
                        Beam.transform.Rotate(0, 0, angleSpeed, Space.Self);
                    }
                }
            }
        }
        public class OrbCheck: MonoBehaviour
        {
            void FixedUpdate()
            {
                float distance = Vector2.Distance(gameObject.transform.position, HALO1.transform.position);
                if(distance <= 1.5)
                {
                    gameObject.LocateMyFSM("Orb Control").SetState("Stop Particles");
                }
            }
        }
        public class NailSpeedUp : MonoBehaviour
        {
            private float NailSpeedAdd = 0f;
            public float NailSpeed = 0f;
            public float NailSpeedMax = 30f;
            public float delayTime = 0f;
            public float accerateFactor = 1f;
            private float NailLength;
            bool start = false;
            private void Start()
            {
                NailLength = gameObject.transform.localScale.y;
                Invoke("go", delayTime);
            }
            void go()
            {
                start = true;
            }
            private void FixedUpdate()
            {
                if(start)
                {
                    NailSpeedAdd = (NailSpeedMax - NailSpeed) * Time.deltaTime * 3f * accerateFactor;
                    NailSpeed += NailSpeedAdd;
                    gameObject.LocateMyFSM("Control").GetState("Fire CW").GetAction<SetVelocityAsAngle>().speed.Value += NailSpeedAdd;
                    gameObject.transform.localScale = new Vector3(gameObject.transform.localScale.x, NailLength + gameObject.LocateMyFSM("Control").GetState("Fire CW").GetAction<SetVelocityAsAngle>().speed.Value * 0.005f, gameObject.transform.localScale.z);
                }
            }
        }
        public class NailSpeedDown : MonoBehaviour
        {
            private float NailSpeedAdd;
            private float NailSpeedAdding;
            public float NailSpeed = 30f;
            public float NailSpeedTime;
            private float NailSpeedTimeLimit;
            private float NailLength;
            private void Start()
            {
                NailLength = gameObject.transform.localScale.y;
                NailSpeedTime = 0.2f;
                NailSpeedTimeLimit = 0.2f;
                NailSpeedAdd = 0f;
                NailSpeedAdding = NailSpeed / 1000f;
            }
            private void FixedUpdate()
            {
                NailSpeedTime -= Time.deltaTime;
                if (NailSpeedTime <= NailSpeedTimeLimit)
                {
                    NailSpeedTimeLimit -= 0.02f;
                    gameObject.LocateMyFSM("Control").GetState("Fire CW").GetAction<SetVelocityAsAngle>().speed.Value /= 1 + Time.deltaTime * 2;
                }
            }
        }
        public class NailFlash : MonoBehaviour
        {
            private List<GameObject> Beams = new List<GameObject>();
            public void Flash(float angle, Vector3 position, float delayTime)
            {
                var beam1 = GameObject.Instantiate(BEAM, gameObject.transform.position, Quaternion.Euler(0, 0, angle));
                //var beam2 = GameObject.Instantiate(BEAM, gameObject.transform.position, Quaternion.Euler(0, 0, angle + 180));
                beam1.transform.localScale = new Vector3(beam1.transform.localScale.x, 0.5f, beam1.transform.localScale.z);
                //beam2.transform.localScale = new Vector3(beam2.transform.localScale.x, 0.5f, beam2.transform.localScale.z);
                Beams.Add(beam1);
                //Beams.Add(beam2);
                foreach (var beam in Beams)
                {
                    beam.SetActive(true);
                    beam.SetActiveChildren(true);
                    beam.LocateMyFSM("Control").SetState("Antic");
                    beam.LocateMyFSM("Control").SendEvent("ANTIC");
                    beam.AddComponent<ObjRecycle>();
                }
                Invoke("FlashEnd", 0.04f);
                Invoke("NailFire", delayTime);
            }
            void FlashEnd()
            {
                foreach (var beam in Beams)
                {
                    beam.LocateMyFSM("Control").SendEvent("FIRE");
                    beam.LocateMyFSM("Control").SetState("End");
                    beam.LocateMyFSM("Control").SendEvent("END");
                    BOSS.GetComponent<AudioSource>().PlayOneShot(NAILCHARGE, 1f);
                }
                Beams.Clear();
            }
            void NailFire()
            {
                gameObject.LocateMyFSM("Control").FsmVariables.FindFsmFloat("Angle").Value = gameObject.transform.eulerAngles.z + 90f;
                gameObject.LocateMyFSM("Control").GetState("Fire CW").GetAction<SetVelocityAsAngle>().speed.Value = 150f;
                gameObject.transform.localScale = gameObject.transform.localScale + new Vector3(0, 0.85f, 0);
                gameObject.AddComponent<NailPtAuto>();

                BOSS.GetComponent<AudioSource>().PlayOneShot(NAILSHOT, 1f);
            }
        }
        public class NailAngle : MonoBehaviour
        {
            System.Random random = new System.Random();
            double RX0 => random.NextDouble();
            public float SIGHX => (random.Next(0, 2) * 2 - 1);
            public float RX => (float)(RX0 * SIGHX);
            bool enable = true;
            float Angle;
            float ADDANGLE;
            float RandomAngle = 0f;
            void Start()
            {
                RandomAngle = RX * 30f;
            }
            void FixedUpdate()
            {
                if(enable)
                {
                    if (Math.Sign(gameObject.transform.eulerAngles.x - HeroController.instance.gameObject.transform.position.x) < 0)
                    {
                        ADDANGLE = 180;
                    }
                    else
                    {
                        ADDANGLE = 0;
                    }
                    Angle = (float)RadiansToDegrees(Math.Atan((HeroController.instance.gameObject.transform.position.y - gameObject.transform.position.y) / (HeroController.instance.gameObject.transform.position.x - gameObject.transform.position.x))) + ADDANGLE + 90f;
                    float AngleAdding = 0f;
                    if (Angle - gameObject.transform.eulerAngles.z > 180f || Angle - gameObject.transform.eulerAngles.z < -180f)
                    {
                        if (Angle - gameObject.transform.eulerAngles.z > 180f)
                        {
                            AngleAdding = (Angle - gameObject.transform.eulerAngles.z - 360f) * Time.deltaTime * 5;
                        }
                        if (Angle - gameObject.transform.eulerAngles.z < -180f)
                        {
                            AngleAdding = (Angle - gameObject.transform.eulerAngles.z + 360f) * Time.deltaTime * 5;
                        }
                    }
                    else
                    {
                        AngleAdding = (Angle - gameObject.transform.eulerAngles.z) * Time.deltaTime * 5;
                    }
                    gameObject.transform.eulerAngles += new Vector3(0, 0, AngleAdding);
                }
            }
            public void End()
            {
                enable = false;
            }
        }
        public class NailMove : MonoBehaviour
        {
            float Timer = 2f;
            float AnimAngle;
            Vector3 AnimPosition;
            public void SetAnim(float angle, Vector3 position)
            {
                AnimAngle = angle + 90f;
                AnimPosition = position;
                Invoke("Flash", 1f);
                //Invoke("ChangeScale", 1.5f);
                Invoke("Fire", 1.5f);
            }
            public void Update()
            {
                Timer -= Time.deltaTime;
                if(Timer > 1f)
                {
                    gameObject.transform.position += (AnimPosition - gameObject.transform.position) * 3f * Time.deltaTime;
                }
            }
            void Flash()
            {
                gameObject.GetComponent<NailFlash>().Flash(gameObject.transform.eulerAngles.z + 90f, gameObject.transform.position, 0.5f);
            }
            void ChangeScale()
            {
                gameObject.transform.localScale = gameObject.transform.localScale + new Vector3(0, 0.7f, 0);
            }
            void Fire()
            {
                gameObject.AddComponent<NailSpeedUp>();
                //gameObject.LocateMyFSM("Control").GetState("Fire CW").GetAction<SetVelocityAsAngle>().speed.Value = 120f;
            }
        }
        public class NailMove_2 : MonoBehaviour
        {
            float Timer = 2f;
            float AnimAngle;
            Vector3 AnimPosition;
            public void SetAnim(float angle, Vector3 position)
            {
                AnimAngle = angle + 90f;
                AnimPosition = position;
                Invoke("Flash", 1f);
                Invoke("ChangeScale", 1.5f);
                Invoke("Fire", 1.5f);
            }
            public void Update()
            {
                Timer -= Time.deltaTime;
                if(Timer > 1f)
                {
                    gameObject.transform.position += (HeroController.instance.gameObject.transform.position + AnimPosition - gameObject.transform.position) * 3f * Time.deltaTime;
                }
            }
            void Flash()
            {
                gameObject.GetComponent<NailFlash>().Flash(gameObject.transform.eulerAngles.z + 90f, gameObject.transform.position, 0.5f);
            }
            void ChangeScale()
            {
                gameObject.transform.localScale = gameObject.transform.localScale + new Vector3(0, 0.7f, 0);
            }
            void Fire()
            {
                gameObject.LocateMyFSM("Control").GetState("Fire CW").GetAction<SetVelocityAsAngle>().speed.Value = 120f;
            }
        }
        public class NailDelayDamage : MonoBehaviour
        {
            public void TimeSetAndStart(float delayTime)
            {
                Invoke("DamageRecover", delayTime);
            }
            void DamageRecover()
            {
                gameObject.GetComponent<DamageHero>().damageDealt = 2;
            }
        }
        public class BeamSightPoint : MonoBehaviour
        {
            System.Random random = new System.Random();
            double RX0 => random.NextDouble();
            double RY0 => random.NextDouble();
            public float SIGHX => (random.Next(0, 2) * 2 - 1);
            public float SIGHY => (random.Next(0, 2) * 2 - 1);
            public float RX => (float)(RX0 * SIGHX);
            public float RY => (float)(RY0 * SIGHY);
            GameObject Beam1;
            GameObject Beam2;
            GameObject Beam3;
            public List<GameObject> Beams = new List<GameObject>();
            float Angle = 7f;
            bool fired = false;
            void Start()
            {
                float angle = RX * 180f;
                Beam1 = Instantiate(BEAM, gameObject.transform.position + new Vector3(0, 0, 0.03f), Quaternion.Euler(0, 0, angle), gameObject.transform);
                Beam2 = Instantiate(BEAM, gameObject.transform.position + new Vector3(0, 0, 0.03f), Quaternion.Euler(0, 0, angle + 120f), gameObject.transform);
                Beam3 = Instantiate(BEAM, gameObject.transform.position + new Vector3(0, 0, 0.03f), Quaternion.Euler(0, 0, angle + 240f), gameObject.transform);
                Beams.Add(Beam1);
                Beams.Add(Beam2);
                Beams.Add(Beam3);
                foreach (var beam in Beams)
                {
                    beam.transform.localScale = new Vector3(beam.transform.localScale.x, 0.5f, beam.transform.localScale.z);
                    beam.SetActive(true);
                    beam.LocateMyFSM("Control").SetState("Antic");
                    beam.LocateMyFSM("Control").SendEvent("ANTIC");
                    beam.AddComponent<ObjRecycle>();
                }
                Invoke("Fire", 1.8f);
                Invoke("End", 1.9f);
            }
            void Fire()
            {
                fired = true;
                foreach (var beam in Beams)
                {
                    beam.LocateMyFSM("Control").SendEvent("FIRE");
                }
                Impact();
            }
            void Impact()
            {
                gameObject.transform.Find("Hero Hurter").gameObject.SetActive(true);
                gameObject.transform.Find("Impact").gameObject.SetActive(true);
                gameObject.GetComponent<ParticleSystem>().Play();
                gameObject.GetComponent<AudioSource>().PlayOneShot(ORBIMPACT, 1f);
                Invoke("End", 0.1f);
            }
            void End()
            {
                gameObject.transform.Find("Hero Hurter").gameObject.Recycle();
                foreach (var beam in Beams)
                {
                    beam.LocateMyFSM("Control").SetState("End");
                    beam.LocateMyFSM("Control").SendEvent("END");
                }
            }
            void FixedUpdate()
            {
                if(!fired)
                {
                    float x = HeroController.instance.gameObject.transform.position.x - gameObject.transform.position.x;
                    float y = HeroController.instance.gameObject.transform.position.y - gameObject.transform.position.y;
                    gameObject.transform.position += new Vector3(x, y, 0) * Time.deltaTime * 4;
                    foreach (var beam in Beams)
                    {
                        beam.transform.Rotate(0, 0, Angle);
                    }
                    Angle /= (1 + Time.deltaTime * 2.4f);
                }
            }
        }
        public class BeamSight_1 : MonoBehaviour
        {
            System.Random random = new System.Random();
            double RX0 => random.NextDouble();
            double RY0 => random.NextDouble();
            double RZ0 => random.NextDouble();
            public float SIGHX => (random.Next(0, 2) * 2 - 1);
            public float SIGHY => (random.Next(0, 2) * 2 - 1);
            public float RX => (float)(RX0 * SIGHX);
            public float RY => (float)(RY0 * SIGHY);
            bool coolDown = true;
            public void On()
            {
                if(coolDown)
                {
                    coolDown = false;
                    Invoke("CoolDown", 2f);
                    Invoke("Summon", (float)RX0 * 0.5f);
                }
            }
            void Summon()
            {
                float angle = RX * (float)Math.PI;
                Vector3 SummonPosition = new Vector3((float)Math.Cos(angle) * 16, (float)Math.Sin(angle) * 8, 0);
                var orb = Instantiate(ORB, HeroController.instance.gameObject.transform.position + SummonPosition, new Quaternion(0, 0, 0, 0));
                var pt = orb.transform.Find("Impact Particles").gameObject;
                pt.transform.SetParent(null);
                pt.GetComponent<ParticleSystem>().emissionRate = 1000;
                pt.GetComponent<ParticleSystem>().startSpeed = 100;
                pt.AddComponent<BeamSightPoint>();
                pt.AddComponent<ObjRecycle>();
                var heroHurter = orb.transform.Find("Hero Hurter").gameObject;
                heroHurter.SetActive(false);
                heroHurter.transform.SetParent(pt.transform);
                var impact = orb.transform.Find("Impact").gameObject;
                impact.GetComponent<RandomScale>().enabled = false;
                impact.transform.SetParent(pt.transform);
                orb.transform.position -= new Vector3(0, 30, 0);
                orb.LocateMyFSM("Orb Control").SetState("Recycle");
            }
            void CoolDown()
            {
                coolDown = true;
            }
        }
        public class BeamSight_2 : MonoBehaviour
        {
            public void On()
            {
                var orb = Instantiate(ORB, BOSS.transform.position + new Vector3(-0.1f, 2f, 0f), new Quaternion(0, 0, 0, 0));
                ORBIMPACT = orb.LocateMyFSM("Orb Control").GetState("Impact").GetAction<AudioPlaySimple>().oneShotClip.Value as AudioClip;
                var heroHurter = orb.transform.Find("Hero Hurter").gameObject;
                heroHurter.SetActive(false);
                heroHurter.AddComponent<ObjRecycle>();
                heroHurter.AddComponent<BeamSightPoint>();
                var impact = orb.transform.Find("Impact").gameObject;
                impact.GetComponent<RandomScale>().enabled = false;
                impact.transform.SetParent(heroHurter.transform);
                var pt = orb.transform.Find("Impact Particles").gameObject;
                pt.GetComponent<ParticleSystem>().emissionRate = 5000;
                pt.GetComponent<ParticleSystem>().startSpeed = 100;
                pt.transform.SetParent(heroHurter.transform);
                orb.transform.position -= new Vector3(0, 30, 0);
                orb.LocateMyFSM("Orb Control").SetState("Recycle");
                heroHurter.AddComponent<BeamSightPoint>();
            }
        }
        public class NailPtAuto : MonoBehaviour
        {
            public void Start()
            {
                if(settings_Pt_.on)
                {
                    var Pt = GameObject.Instantiate(DREAMPT, gameObject.transform.position + new Vector3(0, 0, 0), Quaternion.Euler(gameObject.transform.eulerAngles + new Vector3(0, 0, 180f)));
                    Pt.gameObject.GetComponent<ParticleSystem>().emissionRate = 150f;
                    Pt.gameObject.GetComponent<ParticleSystem>().startSpeed = 250f;
                    Pt.gameObject.GetComponent<ParticleSystem>().Play();
                    Pt.AddComponent<ObjRecycle>();
                    SummonLoop();
                }
            }
            void SummonLoop()
            {
                Summon();
                Invoke("SummonLoop", 0.035f);
            }
            void Summon()
            {
                var Pt = GameObject.Instantiate(DREAMPT, gameObject.transform.position + new Vector3(0, 0, 0), Quaternion.Euler(gameObject.transform.eulerAngles + new Vector3(0, 0, 180f)));
                Pt.gameObject.GetComponent<Transform>().localScale = Pt.gameObject.GetComponent<Transform>().localScale + new Vector3(-0.95f, 0, 0);
                Pt.gameObject.GetComponent<ParticleSystem>().emissionRate = 10f;
                Pt.gameObject.GetComponent<ParticleSystem>().startSpeed = 150f;
                Pt.gameObject.GetComponent<ParticleSystem>().Play();
                Pt.AddComponent<ObjRecycle>();
            }
        }
        public class NailSkillRage : MonoBehaviour
        {
            System.Random random = new System.Random();
            double RX0 => random.NextDouble();
            public float SIGHX => (random.Next(0, 2) * 2 - 1);
            public float RX => (float)RX0;
            double RY0 => random.NextDouble();
            public float SIGHY => (random.Next(0, 2) * 2 - 1);
            public float RY => (float)(RY0 * SIGHY);
            float loopTime = 1f;
            float loopTime1 = 1f;
            float nailDistance = 30f;
            float accelerateFactor = 1f;
            float sigh = -1;
            int count = 0;
            bool turnOff = false;
            public void On(float looptime, float speedFactor)
            {
                count = 0;
                loopTime = looptime;
                loopTime1 = looptime * 8f;
                accelerateFactor = speedFactor;
                SummonStart();
            }
            void SummonStart()
            {
                SummonNailLoop();
            }
            public void SummonEnd()
            {
                CancelInvoke("SummonNailLoop");
                turnOff = true;
            }
            void SummonNailLoop()
            {
                if (!turnOff)
                {
                    count++;
                    sigh = -sigh;
                    float angle = -90 + RY * 20f;
                    float distance = Vector2.Distance(HeroController.instance.transform.position, HALO1.transform.position);
                    var nail1 = Instantiate(NAIL, HeroController.instance.transform.position + new Vector3(RX * 15 * sigh, nailDistance, 0f), Quaternion.Euler(0, 0, angle - 90));
                    if (count % 10 == 0)
                    {
                        if (nail1.transform.position.x - HeroController.instance.gameObject.transform.position.x < 0)
                        {
                            angle = (float)RadiansToDegrees(Math.Atan((nail1.transform.position.y - HeroController.instance.gameObject.transform.position.y) / (nail1.transform.position.x - HeroController.instance.gameObject.transform.position.x)))+ RY * 2f;
                        }
                        else
                        {
                            angle = (float)RadiansToDegrees(Math.Atan((nail1.transform.position.y - HeroController.instance.gameObject.transform.position.y) / (nail1.transform.position.x - HeroController.instance.gameObject.transform.position.x))) + 180 + RY * 2f;
                        }
                        nail1.transform.eulerAngles = new Vector3(0, 0, angle - 90);
                    }
                    nail1.SetActive(true);
                    nail1.AddComponent<NailFlash>();
                    nail1.GetComponent<NailFlash>().Flash(angle, nail1.transform.position, 0.4f);
                    nail1.LocateMyFSM("Control").GetState("Set Collider").AddMethod(() =>
                    {
                        nail1.LocateMyFSM("Control").FsmVariables.FindFsmFloat("Angle").Value = nail1.transform.eulerAngles.z + 90;
                        nail1.LocateMyFSM("Control").GetState("Fire CW").RemoveAction<FaceAngle>();
                        nail1.LocateMyFSM("Control").GetState("Fire CW").GetAction<FloatAdd>().add.Value = 0f;
                        nail1.LocateMyFSM("Control").GetState("Fire CW").GetAction<SetVelocityAsAngle>().speed.Value = 0f;
                        nail1.LocateMyFSM("Control").GetState("Fire CW").GetAction<Wait>().time.Value = 8f;
                        nail1.LocateMyFSM("Control").SetState("Fire CW");
                        nail1.AddComponent<NailBlock>();
                    });
                    BOSS.GetComponent<Audios>().Play(4);
                    Invoke("SummonNailLoop", loopTime1);
                }
            }
            void FixedUpdate()
            {
                loopTime1 += (loopTime - loopTime1) * Time.deltaTime * accelerateFactor;
            }
        }
        public class NailSkill1 : MonoBehaviour
        {
            System.Random random = new System.Random();
            double RX0 => random.NextDouble();
            double RY0 => random.NextDouble();
            public float SIGHX => (random.Next(0, 2) * 2 - 1);
            public float SIGHY => (random.Next(0, 2) * 2 - 1);
            public float RX => (float)(RX0 * SIGHX);
            public float RY => (float)(RY0 * SIGHY);
            float NailR;
            float Side = 1f;
            float factor = 1f;
            float fireTime = 0.06f;
            float summonLooptime = 0.04f;
            float NailAngle = 0f;
            float RandomAngle = 0f;
            float StrikeAngle;
            int NailCount = 0;
            public List<GameObject> Nails = new List<GameObject>();
            public void On(float firetime, float summonLoopTime)
            {
                fireTime = firetime;
                summonLooptime = summonLoopTime;
                float distance = Vector2.Distance(HeroController.instance.gameObject.transform.position, BOSS.transform.position);
                if (distance > 9)
                {
                    NailR = 5 + RX * 2f;
                }
                else
                {
                    NailR = 2.5f + RX * 1.5f;
                }
                Side = SIGHX;
                factor = SIGHY;
                NailCount = 0;
                RandomAngle = RX * 25f;
                NailAngle = BOSS.GetComponent<HeroAngle>().Angle + 270f + RY * 60f;
                StrikeAngle = BOSS.GetComponent<HeroAngle>().Angle;
                NailSummonLoop();
            }
            void Impact()
            {
                BOSS.GetComponent<OrbImpact>().Impact();
            }
            void NailFire()
            {
                if (Nails.Count != 0)
                {
                    int randomnub = random.Next(Nails.Count);
                    var randomnail = Nails[randomnub];
                    //randomnail.AddComponent<NailSpeedUp>();
                    randomnail.AddComponent<NailBlock>();
                    randomnail.AddComponent<NailFlash>();
                    randomnail.GetComponent<NailFlash>().Flash(randomnail.transform.eulerAngles.z + 90f, randomnail.transform.position, 0.5f);
                    Nails.Remove(randomnail);
                    Invoke("NailFire", fireTime);
                }
            }
            void NailColliderRecover()
            {
                foreach (var nail in Nails)
                {
                    nail.GetComponent<DamageHero>().damageDealt = 2;
                }
            }
            public void NailSummonLoopQuick()
            {
                NailR = 0.5f;
                for (int i = 0;i < 36;i++)
                {
                    NailAngle += 10;
                    NailSummon();
                }
                foreach (var nail in Nails)
                {
                    nail.AddComponent<NailFlash>();
                    nail.GetComponent<NailFlash>().Flash(nail.transform.eulerAngles.z + 90f, nail.transform.position, 0.5f);
                    Nails.Remove(nail);
                }
            }
            void NailSummonLoop()
            {
                if(ShieldOn)
                {
                    NailCount++;
                    if (NailCount <= 36)
                    {
                        NailSummon();
                        Invoke("NailSummonLoop", summonLooptime);
                    }
                    else
                    {
                        NailColliderRecover();
                        Invoke("NailFire", 0f);
                        Invoke("Impact", 0f);
                    }
                    NailAngle += (120f + 60f * factor) * Side;
                    if (factor < 0)
                    {
                        if (NailCount % 6 == 0)
                        {
                            NailAngle += 10f * Side;
                        }
                    }
                    if (factor == 0)
                    {
                        if (NailCount % 3 == 0)
                        {
                            NailAngle += 10f * Side;
                        }
                    }
                    if (factor > 0)
                    {
                        if (NailCount % 2 == 0)
                        {
                            NailAngle += 10f * Side;
                        }
                    }
                }
            }
            void NailSummon()
            {
                //NailAngle = BOSS.GetComponent<HeroAngle>().Angle + 270f + RY * 90f;
                float NailSummonX = (float)Math.Cos(DegreesToRadians(NailAngle)) * NailR;
                float NailSummonY = (float)Math.Sin(DegreesToRadians(NailAngle)) * NailR;
                var nail = Instantiate(NAIL, BOSS.transform.position + new Vector3(NailSummonX * 1.5f, NailSummonY * 1.5f + 2f, 0), Quaternion.Euler(0, 0, NailAngle - 90));
                nail.GetComponent<DamageHero>().damageDealt = 0;
                nail.SetActive(true);
                nail.LocateMyFSM("Control").GetState("Set Collider").AddMethod(() =>
                {
                    nail.LocateMyFSM("Control").FsmVariables.FindFsmFloat("Angle").Value = nail.transform.eulerAngles.z + 90;
                    nail.LocateMyFSM("Control").GetState("Fire CW").RemoveAction<FaceAngle>();
                    nail.LocateMyFSM("Control").GetState("Fire CW").GetAction<FloatAdd>().add.Value = 0f;
                    nail.LocateMyFSM("Control").GetState("Fire CW").GetAction<SetVelocityAsAngle>().speed.Value = 5f;
                    nail.LocateMyFSM("Control").GetState("Fire CW").GetAction<Wait>().time.Value = 8f;
                    nail.LocateMyFSM("Control").SetState("Fire CW");
                    nail.AddComponent<NailFlash>();
                    nail.AddComponent<NailSpeedDown>();
                    //nail.AddComponent<NailMove>();
                    //nail.GetComponent<NailMove>().SetAnim(NailAngle + 90, BOSS.transform.position + new Vector3(NailSummonX, NailSummonY + 2.5f, 0));
                });
                Nails.Add(nail);
                BOSS.GetComponent<Audios>().Play(4);
            }
        }
        public class NailSkill2 : MonoBehaviour
        {
            System.Random random = new System.Random();
            double RX0 => random.NextDouble();
            double RY0 => random.NextDouble();
            public float SIGHX => (random.Next(0, 2) * 2 - 1);
            public float SIGHY => (random.Next(0, 2) * 2 - 1);
            public float RX => (float)(RX0 * SIGHX);
            public float RY => (float)(RY0 * SIGHY);
            float NailR;
            float Side = 1f;
            float factor = 1f;
            float NailAngle = 0f;
            float RandomAngle = 0f;
            float StrikeAngle;
            int NailCount = 0;
            public List<GameObject> Nails = new List<GameObject>();
            public void On()
            {
                float distance = Vector2.Distance(HeroController.instance.gameObject.transform.position, BOSS.transform.position);
                if (distance > 9)
                {
                    NailR = 5 + RX * 2f;
                }
                else
                {
                    NailR = 2.5f + RX * 1.5f;
                }
                Side = SIGHX;
                factor = SIGHY;
                NailCount = 0;
                RandomAngle = RX * 25f;
                NailAngle = BOSS.GetComponent<HeroAngle>().Angle + 270f + RY * 60f;
                StrikeAngle = BOSS.GetComponent<HeroAngle>().Angle;
                NailSummonLoop();
            }
            void Impact()
            {
                BOSS.GetComponent<OrbImpact>().Impact();
            }
            void NailFire()
            {
                if (Nails.Count != 0)
                {
                    int randomnub = random.Next(Nails.Count);
                    var randomnail = Nails[randomnub];
                    randomnail.AddComponent<NailFlash>();
                    randomnail.GetComponent<NailFlash>().Flash(randomnail.transform.eulerAngles.z + 90f, randomnail.transform.position, 0.5f);
                    randomnail.GetComponent<NailAngle>().End();
                    Nails.Remove(randomnail);
                    Invoke("NailFire", 0.06f);
                    NailColliderRecover();
                }
            }
            void NailColliderRecover()
            {
                foreach (var nail in Nails)
                {
                    nail.GetComponent<DamageHero>().damageDealt = 2;
                }
            }
            void NailSummonLoop()
            {
                NailCount++;
                if (NailCount <= 9)
                {
                    NailSummon();
                    Invoke("NailSummonLoop", 0.16f);
                }
                else
                {
                    //Invoke("NailFire", 0.3f);
                }
                NailAngle += (120f + 60f * factor) * Side;
                if (factor < 0)
                {
                    if (NailCount % 6 == 0)
                    {
                        NailAngle += 20f * Side;
                    }
                }
                if (factor == 0)
                {
                    if (NailCount % 3 == 0)
                    {
                        NailAngle += 20f * Side;
                    }
                }
                if (factor > 0)
                {
                    if (NailCount % 2 == 0)
                    {
                        NailAngle += 20f * Side;
                    }
                }
            }
            void NailSummon()
            {
                if (ShieldOn)
                {
                    //NailAngle = BOSS.GetComponent<HeroAngle>().Angle + 270f + RY * 90f;
                    float NailSummonX = (float)Math.Cos(DegreesToRadians(NailAngle)) * NailR;
                    float NailSummonY = (float)Math.Sin(DegreesToRadians(NailAngle)) * NailR;
                    float ADDANGLE = 0f;
                    if (Math.Sign(NailSummonX - HeroController.instance.gameObject.transform.position.x) < 0)
                    {
                        ADDANGLE = 0;
                    }
                    else
                    {
                        ADDANGLE = 180;
                    }
                    float Angle = (float)RadiansToDegrees(Math.Atan((HeroController.instance.gameObject.transform.position.y - (BOSS.transform.position.y + NailSummonY)) / (HeroController.instance.gameObject.transform.position.x - (BOSS.transform.position.x + NailSummonX)))) + 90f;
                    var nail = Instantiate(NAIL, BOSS.transform.position + new Vector3(NailSummonX * 1.5f, NailSummonY * 1.5f + 2f, 0), Quaternion.Euler(0, 0, Angle));
                    nail.GetComponent<DamageHero>().damageDealt = 0;
                    nail.SetActive(true);
                    nail.LocateMyFSM("Control").GetState("Set Collider").AddMethod(() =>
                    {
                        nail.LocateMyFSM("Control").FsmVariables.FindFsmFloat("Angle").Value = nail.transform.eulerAngles.z + 90;
                        nail.LocateMyFSM("Control").GetState("Fire CW").RemoveAction<FaceAngle>();
                        nail.LocateMyFSM("Control").GetState("Fire CW").GetAction<FloatAdd>().add.Value = 0f;
                        nail.LocateMyFSM("Control").GetState("Fire CW").GetAction<SetVelocityAsAngle>().speed.Value = 0f;
                        nail.LocateMyFSM("Control").GetState("Fire CW").GetAction<Wait>().time.Value = 8f;
                        nail.LocateMyFSM("Control").SetState("Fire CW");
                        nail.AddComponent<NailFlash>();
                        nail.AddComponent<NailSpeedDown>();
                        //nail.AddComponent<NailAngle>();a
                        nail.GetComponent<NailFlash>().Flash(nail.transform.eulerAngles.z + 90f, nail.transform.position, 0.5f);
                        nail.AddComponent<NailBlock>();
                        //nail.AddComponent<NailMove>();
                        //nail.GetComponent<NailMove>().SetAnim(NailAngle + 90, BOSS.transform.position + new Vector3(NailSummonX, NailSummonY + 2.5f, 0));
                    });
                    Nails.Add(nail);
                }
            }
        }
        public class NailSkill3 : MonoBehaviour
        {
            float loopTime = 1f;
            public void On(float looptime, float endtime)
            {
                loopTime = looptime;
                SummonStart(endtime);
            }
            void SummonStart(float timeEnd)
            {
                Invoke("SummonEnd", timeEnd);
                SummonNailLoop();
            }
            void SummonEnd()
            {
                CancelInvoke("SummonNailLoop");
            }
            void SummonNailLoop()
            {
                if (ShieldOn)
                {
                    var nail1 = Instantiate(NAIL, HALO1.transform.position, Quaternion.Euler(0, 0, HALO1.transform.eulerAngles.z));
                    nail1.GetComponent<DamageHero>().damageDealt = 0;
                    nail1.AddComponent<NailDelayDamage>();
                    nail1.AddComponent<NailSpeedUp>();
                    nail1.GetComponent<NailSpeedUp>().delayTime = 0.8f;
                    nail1.GetComponent<NailDelayDamage>().TimeSetAndStart(1f);
                    nail1.SetActive(true);
                    nail1.LocateMyFSM("Control").GetState("Set Collider").AddMethod(() =>
                    {
                        nail1.LocateMyFSM("Control").FsmVariables.FindFsmFloat("Angle").Value = nail1.transform.eulerAngles.z + 90;
                        nail1.LocateMyFSM("Control").GetState("Fire CW").RemoveAction<FaceAngle>();
                        nail1.LocateMyFSM("Control").GetState("Fire CW").GetAction<FloatAdd>().add.Value = 0f;
                        nail1.LocateMyFSM("Control").GetState("Fire CW").GetAction<SetVelocityAsAngle>().speed.Value = 0f;
                        nail1.LocateMyFSM("Control").GetState("Fire CW").GetAction<Wait>().time.Value = 8f;
                        nail1.LocateMyFSM("Control").SetState("Fire CW");
                        nail1.AddComponent<NailBlock>();
                    });
                    var nail2 = Instantiate(NAIL, HALO1.transform.position, Quaternion.Euler(0, 0, HALO1.transform.eulerAngles.z + 180f));
                    nail2.GetComponent<DamageHero>().damageDealt = 0;
                    nail2.AddComponent<NailDelayDamage>();
                    nail2.AddComponent<NailSpeedUp>();
                    nail2.GetComponent<NailSpeedUp>().delayTime = 0.8f;
                    nail2.GetComponent<NailDelayDamage>().TimeSetAndStart(1f);
                    nail2.LocateMyFSM("Control").GetState("Set Collider").AddMethod(() =>
                    {
                        nail2.LocateMyFSM("Control").FsmVariables.FindFsmFloat("Angle").Value = nail2.transform.eulerAngles.z + 90;
                        nail2.LocateMyFSM("Control").GetState("Fire CW").RemoveAction<FaceAngle>();
                        nail2.LocateMyFSM("Control").GetState("Fire CW").GetAction<FloatAdd>().add.Value = 0f;
                        nail2.LocateMyFSM("Control").GetState("Fire CW").GetAction<SetVelocityAsAngle>().speed.Value = 0f;
                        nail2.LocateMyFSM("Control").GetState("Fire CW").GetAction<Wait>().time.Value = 8f;
                        nail2.LocateMyFSM("Control").SetState("Fire CW");
                        nail2.AddComponent<NailBlock>();
                    });
                    BOSS.GetComponent<Audios>().Play(4);
                    Invoke("SummonNailLoop", loopTime);
                }
            }
        }
        public class NailSkill4 : MonoBehaviour
        {
            System.Random random = new System.Random();
            double RX0 => random.NextDouble();
            public float SIGHX => (random.Next(0, 2) * 2 - 1);
            public float RX => (float)(RX0 * SIGHX);
            float loopTime = 1f;
            float loopTime1 = 1f;
            float nailDistance = 20f;
            public void On(float looptime, float endtime)
            {
                loopTime = looptime;
                loopTime1 = looptime * 5f;
                SummonStart(endtime);
                BOSS.GetComponent<BossDash>().DashAuto();
            }
            void SummonStart(float timeEnd)
            {
                Invoke("SummonEnd", timeEnd);
                SummonNailLoop();
            }
            void SummonEnd()
            {
                CancelInvoke("SummonNailLoop");
            }
            void SummonNailLoop()
            {
                if(ShieldOn)
                {
                    float y = HALO1.transform.position.y - HeroController.instance.transform.position.y;
                    float x = HALO1.transform.position.x - HeroController.instance.transform.position.x;
                    float angle = (float)RadiansToDegrees(Math.Atan(y / x)) + RX * 8f + 90f;
                    if (x < 0)
                    {
                        angle += -90f;
                    }
                    else
                    {
                        angle += 90f;
                    }
                    float distance = Vector2.Distance(HeroController.instance.transform.position, HALO1.transform.position);
                    var nail1 = Instantiate(NAIL, HALO1.transform.position + new Vector3(x / distance * nailDistance, y / distance * nailDistance, 0), Quaternion.Euler(0, 0, angle - 90));
                    nail1.SetActive(true);
                    nail1.LocateMyFSM("Control").GetState("Set Collider").AddMethod(() =>
                    {
                        nail1.LocateMyFSM("Control").FsmVariables.FindFsmFloat("Angle").Value = nail1.transform.eulerAngles.z + 90;
                        nail1.LocateMyFSM("Control").GetState("Fire CW").RemoveAction<FaceAngle>();
                        nail1.LocateMyFSM("Control").GetState("Fire CW").GetAction<FloatAdd>().add.Value = 0f;
                        nail1.LocateMyFSM("Control").GetState("Fire CW").GetAction<SetVelocityAsAngle>().speed.Value = 0f;
                        nail1.LocateMyFSM("Control").GetState("Fire CW").GetAction<Wait>().time.Value = 8f;
                        nail1.LocateMyFSM("Control").SetState("Fire CW");
                        nail1.AddComponent<NailFlash>();
                        nail1.GetComponent<NailFlash>().Flash(angle, nail1.transform.position, 0.25f);
                        nail1.AddComponent<NailBlock>();
                    });
                    BOSS.GetComponent<Audios>().Play(4);
                    Invoke("SummonNailLoop", loopTime1);
                }
            }
            void FixedUpdate()
            {
                loopTime1 += (loopTime - loopTime1) * Time.deltaTime * 1.4f;
            }
        }
        //
        public class NailSkill5 : MonoBehaviour
        {
            System.Random random = new System.Random();
            double RX0 => random.NextDouble();
            public float SIGHX => (random.Next(0, 2) * 2 - 1);
            public float RX => (float)RX0;
            double RY0 => random.NextDouble();
            public float SIGHY => (random.Next(0, 2) * 2 - 1);
            public float RY => (float)(RY0 * SIGHY);
            float loopTime = 1f;
            float loopTime1 = 1f;
            float nailDistance = 30f;
            float accelerateFactor = 1f;
            int count = 0;
            float sigh = -1;
            public void On(float looptime, float endtime)
            {
                count = 0;
                loopTime = looptime;
                loopTime1 = looptime * 5f;
                SummonStart(endtime);
            }
            void SummonStart(float timeEnd)
            {
                Invoke("SummonEnd", timeEnd);
                SummonNailLoop();
            }
            public void SummonEnd()
            {
                CancelInvoke("SummonNailLoop");
            }
            void SummonNailLoop()
            {
                if(ShieldOn)
                {
                    count++;
                    sigh = -sigh;
                    float angle = -90 + RY * 20f;
                    float distance = Vector2.Distance(HeroController.instance.transform.position, HALO1.transform.position);
                    var nail1 = Instantiate(NAIL, HeroController.instance.transform.position + new Vector3(RX * 15 * sigh, nailDistance, 0f), Quaternion.Euler(0, 0, angle - 90));
                    if (count % 10 == 0)
                    {
                        if (nail1.transform.position.x - HeroController.instance.gameObject.transform.position.x < 0)
                        {
                            angle = (float)RadiansToDegrees(Math.Atan((nail1.transform.position.y - HeroController.instance.gameObject.transform.position.y) / (nail1.transform.position.x - HeroController.instance.gameObject.transform.position.x))) + 180 + RY * 2f;
                        }
                        else
                        {
                            angle = (float)RadiansToDegrees(Math.Atan((nail1.transform.position.y - HeroController.instance.gameObject.transform.position.y) / (nail1.transform.position.x - HeroController.instance.gameObject.transform.position.x))) + RY * 2f;
                        }
                        nail1.transform.eulerAngles = new Vector3(0, 0, angle - 90);
                    }
                    nail1.SetActive(true);
                    nail1.AddComponent<NailFlash>();
                    nail1.GetComponent<NailFlash>().Flash(angle, nail1.transform.position, 0.4f);
                    nail1.LocateMyFSM("Control").GetState("Set Collider").AddMethod(() =>
                    {
                        nail1.LocateMyFSM("Control").FsmVariables.FindFsmFloat("Angle").Value = nail1.transform.eulerAngles.z + 90;
                        nail1.LocateMyFSM("Control").GetState("Fire CW").RemoveAction<FaceAngle>();
                        nail1.LocateMyFSM("Control").GetState("Fire CW").GetAction<FloatAdd>().add.Value = 0f;
                        nail1.LocateMyFSM("Control").GetState("Fire CW").GetAction<SetVelocityAsAngle>().speed.Value = 0f;
                        nail1.LocateMyFSM("Control").GetState("Fire CW").GetAction<Wait>().time.Value = 8f;
                        nail1.LocateMyFSM("Control").SetState("Fire CW");
                        nail1.AddComponent<NailBlock>();
                    });
                    BOSS.GetComponent<Audios>().Play(4);
                    Invoke("SummonNailLoop", loopTime1);
                }
            }
            void FixedUpdate()
            {
                loopTime1 += (loopTime - loopTime1) * Time.deltaTime * accelerateFactor;
            }
        }
        public class NailSkill6 : MonoBehaviour
        {
            GameObject Beam;
            GameObject Beam1;
            GameObject Beam2;
            GameObject Nail;
            public float Angle;
            float r = 0f;
            float angle1 = 45f;
            float angle2 = -45f;
            bool move = true;
            bool antic = false;
            void Start()
            {
                BeamSummon();
            }
            void BeamSummon()
            {
                Beam = Instantiate(BEAM, BOSS.transform.position, Quaternion.Euler(0, 0, 90), BOSS.transform);
                Beam.transform.localScale = new Vector3(Beam.transform.localScale.x, Beam.transform.localScale.y * 0.2f, Beam.transform.localScale.z);
                Beam.transform.localPosition += new Vector3(0, 2.5f, 0);
                Beam1 = Instantiate(BEAM, BOSS.transform.position, Quaternion.Euler(0, 0, 90), BOSS.transform);
                Beam1.transform.localScale = new Vector3(Beam.transform.localScale.x, Beam.transform.localScale.y * 1f, Beam.transform.localScale.z);
                Beam1.transform.localPosition += new Vector3(0, 2.5f, 0);
                Beam2 = Instantiate(BEAM, BOSS.transform.position, Quaternion.Euler(0, 0, 90), BOSS.transform);
                Beam2.transform.localScale = new Vector3(Beam.transform.localScale.x, Beam.transform.localScale.y * 1f, Beam.transform.localScale.z);
                Beam2.transform.localPosition += new Vector3(0, 2.5f, 0);
            }
            public void On()
            {
                Invoke("BeamSideAntic", 0.4f);
                Invoke("BeamSideStop", 1.45f);
                Invoke("Antic", 0.4f);
                Invoke("Stop", 1.45f);
                Invoke("Fire", 1.45f);
                Invoke("End", 1.45f);
                Invoke("Antic", 1.5f);
                Invoke("Stop", 2.2f);
                Invoke("Fire", 2.2f);
                Invoke("End", 2.2f);
                Invoke("Antic", 2.25f);
                Invoke("Stop", 2.85f);
                Invoke("Fire", 2.9f);
                Invoke("End", 2.9f);

                float angle;
                if (BOSS.transform.position.x - HeroController.instance.gameObject.transform.position.x < 0)
                {
                    angle = (float)RadiansToDegrees(Math.Atan((BOSS.transform.position.y - HeroController.instance.gameObject.transform.position.y + 5f) / (BOSS.transform.position.x - HeroController.instance.gameObject.transform.position.x - 14f))) + 180;
                }
                else
                {
                    angle = (float)RadiansToDegrees(Math.Atan((BOSS.transform.position.y - HeroController.instance.gameObject.transform.position.y + 5f) / (BOSS.transform.position.x - HeroController.instance.gameObject.transform.position.x + 14f)));
                }
                BOSS.GetComponent<BossDash>().Dash(angle, 16f);
                HALO1.GetComponent<HaloAround_Small>().LockHero(3.3f);
            }
            void BeamSideAntic()
            {
                antic = true;
                angle1 = 45f;
                angle2 = -45f;
                Beam1.LocateMyFSM("Control").SetState("Antic");
                Beam1.LocateMyFSM("Control").SendEvent("ANTIC");
                Beam2.LocateMyFSM("Control").SetState("Antic");
                Beam2.LocateMyFSM("Control").SendEvent("ANTIC");
            }
            void BeamSideStop()
            {
                Beam1.LocateMyFSM("Control").SendEvent("FIRE");
                Beam1.LocateMyFSM("Control").SendEvent("END");
                Beam2.LocateMyFSM("Control").SendEvent("FIRE");
                Beam2.LocateMyFSM("Control").SendEvent("END");
            }
            void Antic()
            {
                if(ShieldOn)
                {
                    r = 0f;
                    move = true;
                    Beam.LocateMyFSM("Control").SetState("Antic");
                    Beam.LocateMyFSM("Control").SendEvent("ANTIC");
                    AnticAudio();

                    Nail = Instantiate(NAIL, BOSS.transform.position + new Vector3(0, 2.5f, 0f), Quaternion.Euler(0, 0, Beam.transform.eulerAngles.z));
                    Nail.SetActive(true);
                    Nail.LocateMyFSM("Control").GetState("Set Collider").AddMethod(() =>
                    {
                        Nail.LocateMyFSM("Control").FsmVariables.FindFsmFloat("Angle").Value = Nail.transform.eulerAngles.z + 90;
                        Nail.LocateMyFSM("Control").GetState("Fire CW").RemoveAction<FaceAngle>();
                        Nail.LocateMyFSM("Control").GetState("Fire CW").GetAction<FloatAdd>().add.Value = 0f;
                        Nail.LocateMyFSM("Control").GetState("Fire CW").GetAction<SetVelocityAsAngle>().speed.Value = 0f;
                        Nail.LocateMyFSM("Control").GetState("Fire CW").GetAction<Wait>().time.Value = 8f;
                        Nail.LocateMyFSM("Control").SetState("Fire CW");
                        Nail.AddComponent<NailBlock>();
                    });

                }
            }
            void Stop()
            {
                antic = false;
                move = false;
                Beam.LocateMyFSM("Control").SendEvent("FIRE");
                Beam.LocateMyFSM("Control").SendEvent("END");
                Beam1.LocateMyFSM("Control").SendEvent("FIRE");
                Beam1.LocateMyFSM("Control").SendEvent("END");
            }
            void Fire()
            {
                if (ShieldOn)
                {
                    Nail.LocateMyFSM("Control").FsmVariables.FindFsmFloat("Angle").Value = Nail.transform.eulerAngles.z + 90;
                    Nail.LocateMyFSM("Control").GetState("Fire CW").GetAction<SetVelocityAsAngle>().speed.Value = 300f;
                    Nail.AddComponent<NailPtAuto>();
                    Nail.transform.localScale = gameObject.transform.localScale + new Vector3(0, 1.2f, 0);
                    BOSS.GetComponent<AudioSource>().PlayOneShot(NAILSHOT, 1f);
                    Beam.LocateMyFSM("Control").SendEvent("FIRE");
                    Beam1.LocateMyFSM("Control").SendEvent("FIRE");
                    HALO1.GetComponent<HaloAround_Small>().Flash();
                    HALO1.GetComponent<HaloRotateChange_Small>().Flash();
                }
                else
                {
                    Nail.Recycle();
                    Beam.LocateMyFSM("Control").SendEvent("FIRE");
                    Beam1.LocateMyFSM("Control").SendEvent("FIRE");
                    Beam.LocateMyFSM("Control").SendEvent("END");
                    Beam1.LocateMyFSM("Control").SendEvent("END");
                }
            }
            void End()
            {
                Beam.LocateMyFSM("Control").SendEvent("END");
                Beam1.LocateMyFSM("Control").SendEvent("END");
            }
            void AnticAudio()
            {
                AudioClip Antic = BOSS.LocateMyFSM("Attack Commands").GetState("EB 1").GetAction<AudioPlaySimple>().oneShotClip.Value as AudioClip;
                BOSS.GetComponent<AudioSource>().PlayOneShot(Antic, 1f);
            }
            void FireAudio()
            {
                AudioClip Fire = BOSS.LocateMyFSM("Attack Commands").GetState("EB 1").GetAction<AudioPlayerOneShotSingle>().audioClip.Value as AudioClip;
                BOSS.GetComponent<AudioSource>().PlayOneShot(Fire, 1f);
            }
            void FixedUpdate()
            {
                if (move)
                {
                    float angle;
                    angle = (float)RadiansToDegrees(Math.Atan((Beam.transform.position.y - HeroController.instance.gameObject.transform.position.y + 0.5f) / (Beam.transform.position.x - HeroController.instance.gameObject.transform.position.x))) + 90;
                    if (Beam.transform.position.x - HeroController.instance.gameObject.transform.position.x < 0)
                    {
                        angle += -90f;
                    }
                    else
                    {
                        angle += 90f;
                    }
                    Beam.transform.eulerAngles = new Vector3(0, 0, angle);
                    if (antic)
                    {
                        angle1 += -45 * Time.deltaTime;
                        angle2 += 45 * Time.deltaTime;
                        Beam1.transform.eulerAngles = new Vector3(0, 0, Beam.transform.eulerAngles.z + angle1);
                        Beam2.transform.eulerAngles = new Vector3(0, 0, Beam.transform.eulerAngles.z + angle2);
                    }

                    if (Nail != null)
                    {
                        r += (8f - r) * Time.deltaTime * 3f;
                        Nail.transform.eulerAngles = new Vector3(0, 0, angle - 90);
                        float x = (float)Math.Cos(DegreesToRadians(angle + 180)) * r;
                        float y = (float)Math.Sin(DegreesToRadians(angle + 180)) * r;
                        Nail.transform.position = BOSS.transform.position + new Vector3(x, y + 2.5f, -0.001f);
                    }
                }
                /*
                if (HALO1 != null)
                {
                    float x = (float)Math.Cos(DegreesToRadians(angle - 90f)) * 1f;
                    float y = (float)Math.Sin(DegreesToRadians(angle - 90f)) * 1f;
                    Beam.transform.position = HALO1.transform.position + new Vector3(x, y, 0);
                }
                */
            }
        }
        public class NailSkill6_H : MonoBehaviour
        {
            GameObject Beam;
            GameObject Beam1;
            GameObject Beam2;
            GameObject Nail;
            public float Angle;
            float angle1 = 45f;
            float angle2 = -45f;
            float r = 0f;
            bool move = true;
            bool antic = false;
            void Start()
            {
                BeamSummon();
            }
            void BeamSummon()
            {
                Beam = Instantiate(BEAM, BOSS.transform.position, Quaternion.Euler(0, 0, 90), BOSS.transform);
                Beam.transform.localScale = new Vector3(Beam.transform.localScale.x, Beam.transform.localScale.y * 0.2f, Beam.transform.localScale.z);
                Beam.transform.localPosition += new Vector3(0, 2.5f, 0);
                Beam1 = Instantiate(BEAM, BOSS.transform.position, Quaternion.Euler(0, 0, 90), BOSS.transform);
                Beam1.transform.localScale = new Vector3(Beam.transform.localScale.x, Beam.transform.localScale.y * 1f, Beam.transform.localScale.z);
                Beam1.transform.localPosition += new Vector3(0, 2.5f, 0);
                Beam2 = Instantiate(BEAM, BOSS.transform.position, Quaternion.Euler(0, 0, 90), BOSS.transform);
                Beam2.transform.localScale = new Vector3(Beam.transform.localScale.x, Beam.transform.localScale.y * 1f, Beam.transform.localScale.z);
                Beam2.transform.localPosition += new Vector3(0, 2.5f, 0);
            }
            public void On()
            {
                Invoke("BeamSideAntic", 0.4f);
                Invoke("BeamSideStop", 1.45f);
                Invoke("Antic", 0.4f);
                Invoke("Stop", 1.45f);
                Invoke("Fire", 1.45f);
                Invoke("End", 1.45f);
                Invoke("Antic", 1.5f);
                Invoke("Stop", 1.85f);
                Invoke("Fire", 1.85f);
                Invoke("End", 1.85f);
                Invoke("Antic", 1.9f);
                Invoke("Stop", 2.25f);
                Invoke("Fire", 2.25f);
                Invoke("End", 2.25f);
                Invoke("Antic", 2.3f);
                Invoke("Stop", 2.65f);
                Invoke("Fire", 2.65f);
                Invoke("End", 2.65f);
                Invoke("Antic", 2.7f);
                Invoke("Stop", 3.05f);
                Invoke("Fire", 3.05f);
                Invoke("End", 3.05f);
                Invoke("Antic", 3.1f);
                Invoke("Stop", 3.45f);
                Invoke("Fire", 3.45f);
                Invoke("End", 3.45f);

                float angle;
                if (BOSS.transform.position.x - HeroController.instance.gameObject.transform.position.x < 0)
                {
                    angle = (float)RadiansToDegrees(Math.Atan((BOSS.transform.position.y - HeroController.instance.gameObject.transform.position.y + 5f) / (BOSS.transform.position.x - HeroController.instance.gameObject.transform.position.x - 14f))) + 180;
                }
                else
                {
                    angle = (float)RadiansToDegrees(Math.Atan((BOSS.transform.position.y - HeroController.instance.gameObject.transform.position.y + 5f) / (BOSS.transform.position.x - HeroController.instance.gameObject.transform.position.x + 14f)));
                }
                BOSS.GetComponent<BossDash>().Dash(angle, 16f);
                HALO1.GetComponent<HaloAround_Small>().LockHero(3.3f);
            }
            void BeamSideAntic()
            {
                antic = true;
                angle1 = 45f;
                angle2 = -45f;
                Beam1.LocateMyFSM("Control").SetState("Antic");
                Beam1.LocateMyFSM("Control").SendEvent("ANTIC");
                Beam2.LocateMyFSM("Control").SetState("Antic");
                Beam2.LocateMyFSM("Control").SendEvent("ANTIC");
            }
            void Antic()
            {
                if(ShieldOn)
                {
                    r = 0f;
                    move = true;
                    Beam.LocateMyFSM("Control").SetState("Antic");
                    Beam.LocateMyFSM("Control").SendEvent("ANTIC");
                    AnticAudio();

                    Nail = Instantiate(NAIL, BOSS.transform.position + new Vector3(0, 2.5f, 0f), Quaternion.Euler(0, 0, Beam.transform.eulerAngles.z));
                    Nail.SetActive(true);
                    Nail.LocateMyFSM("Control").GetState("Set Collider").AddMethod(() =>
                    {
                        Nail.LocateMyFSM("Control").FsmVariables.FindFsmFloat("Angle").Value = Nail.transform.eulerAngles.z + 90;
                        Nail.LocateMyFSM("Control").GetState("Fire CW").RemoveAction<FaceAngle>();
                        Nail.LocateMyFSM("Control").GetState("Fire CW").GetAction<FloatAdd>().add.Value = 0f;
                        Nail.LocateMyFSM("Control").GetState("Fire CW").GetAction<SetVelocityAsAngle>().speed.Value = 0f;
                        Nail.LocateMyFSM("Control").GetState("Fire CW").GetAction<Wait>().time.Value = 8f;
                        Nail.LocateMyFSM("Control").SetState("Fire CW");
                        //Nail.AddComponent<NailBlock>();
                    });
                }
            }
            void BeamSideStop()
            {
                Beam1.LocateMyFSM("Control").SendEvent("FIRE");
                Beam1.LocateMyFSM("Control").SendEvent("END");
                Beam2.LocateMyFSM("Control").SendEvent("FIRE");
                Beam2.LocateMyFSM("Control").SendEvent("END");
            }
            void Stop()
            {
                move = false;
                antic = false;
                Beam.LocateMyFSM("Control").SendEvent("FIRE");
                Beam.LocateMyFSM("Control").SendEvent("END");
                Beam1.LocateMyFSM("Control").SendEvent("FIRE");
                Beam1.LocateMyFSM("Control").SendEvent("END");
            }
            void Fire()
            {
                if (ShieldOn)
                {
                    Nail.LocateMyFSM("Control").FsmVariables.FindFsmFloat("Angle").Value = Nail.transform.eulerAngles.z + 90;
                    Nail.LocateMyFSM("Control").GetState("Fire CW").GetAction<SetVelocityAsAngle>().speed.Value = 300f;
                    Nail.AddComponent<NailPtAuto>();
                    Nail.transform.localScale = gameObject.transform.localScale + new Vector3(0, 1.2f, 0);
                    BOSS.GetComponent<AudioSource>().PlayOneShot(NAILSHOT, 1f);
                    Beam.LocateMyFSM("Control").SendEvent("FIRE");
                    Beam1.LocateMyFSM("Control").SendEvent("FIRE");
                    HALO1.GetComponent<HaloAround_Small>().Flash();
                    HALO1.GetComponent<HaloRotateChange_Small>().Flash();
                }
                else
                {
                    Nail.Recycle();
                    Beam.LocateMyFSM("Control").SendEvent("FIRE");
                    Beam1.LocateMyFSM("Control").SendEvent("FIRE");
                    Beam.LocateMyFSM("Control").SendEvent("END");
                    Beam1.LocateMyFSM("Control").SendEvent("END");
                }
            }
            void End()
            {
                Beam.LocateMyFSM("Control").SendEvent("END");
                Beam1.LocateMyFSM("Control").SendEvent("END");
            }
            void AnticAudio()
            {
                AudioClip Antic = BOSS.LocateMyFSM("Attack Commands").GetState("EB 1").GetAction<AudioPlaySimple>().oneShotClip.Value as AudioClip;
                BOSS.GetComponent<AudioSource>().PlayOneShot(Antic, 1f);
            }
            void FireAudio()
            {
                AudioClip Fire = BOSS.LocateMyFSM("Attack Commands").GetState("EB 1").GetAction<AudioPlayerOneShotSingle>().audioClip.Value as AudioClip;
                BOSS.GetComponent<AudioSource>().PlayOneShot(Fire, 1f);
            }
            void FixedUpdate()
            {
                if (move)
                {
                    float angle;
                    angle = (float)RadiansToDegrees(Math.Atan((Beam.transform.position.y - HeroController.instance.gameObject.transform.position.y + 0.5f) / (Beam.transform.position.x - HeroController.instance.gameObject.transform.position.x))) + 90;
                    if (Beam.transform.position.x - HeroController.instance.gameObject.transform.position.x < 0)
                    {
                        angle += -90f;
                    }
                    else
                    {
                        angle += 90f;
                    }
                    Beam.transform.eulerAngles = new Vector3(0, 0, angle);
                    if (antic)
                    {
                        angle1 += -45 * Time.deltaTime;
                        angle2 += 45 * Time.deltaTime;
                        Beam1.transform.eulerAngles = new Vector3(0, 0, Beam.transform.eulerAngles.z + angle1);
                        Beam2.transform.eulerAngles = new Vector3(0, 0, Beam.transform.eulerAngles.z + angle2);
                    }

                    if (Nail != null)
                    {
                        r += (8f - r) * Time.deltaTime * 3f;
                        Nail.transform.eulerAngles = new Vector3(0, 0, angle - 90);
                        float x = (float)Math.Cos(DegreesToRadians(angle + 180)) * r;
                        float y = (float)Math.Sin(DegreesToRadians(angle + 180)) * r;
                        Nail.transform.position = BOSS.transform.position + new Vector3(x, y + 2.5f, -0.001f);
                    }
                }
                /*
                if (HALO1 != null)
                {
                    float x = (float)Math.Cos(DegreesToRadians(angle - 90f)) * 1f;
                    float y = (float)Math.Sin(DegreesToRadians(angle - 90f)) * 1f;
                    Beam.transform.position = HALO1.transform.position + new Vector3(x, y, 0);
                }
                */
            }
        }
    }
}
