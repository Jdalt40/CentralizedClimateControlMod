﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using RimWorld;
using Verse;
using Verse.Sound;

namespace EnhancedTemperature
{
    public class CompAirFlowProducer : CompAirFlow
    {
        public const string AirFlowOutputKey = "EnhancedTemperature.AirFlowOutput";
        public const string IntakeTempKey = "EnhancedTemperature.Producer.IntakeTemperature";

        [Unsaved]
        public bool IsOperatingAtHighPower;
        public float CurrentAirFlow = 0.0f;
        public float IntakeTemperature = 0.0f;
        protected CompFlickable FlickableComp;

        public float AirFlowOutput
        {
            get
            {
                if (IsOperating())
                {
                    return this.CurrentAirFlow;
                }

                return 0.0f;
            }
        }

        public string DebugString
        {
            get
            {
                StringBuilder stringBuilder = new StringBuilder();
                stringBuilder.AppendLine(this.parent.LabelCap + " CompAirFlow:");
                stringBuilder.AppendLine("   AirFlow IsOperating: " + IsOperating());
                stringBuilder.AppendLine("   AirFlow Output: " + this.AirFlowOutput);
                return stringBuilder.ToString();
            }
        }

        public override void PostSpawnSetup(bool respawningAfterLoad)
        {
            EnhancedTemperatureUtility.GetNetManager(this.parent.Map).RegisterProducer(this);
            this.FlickableComp = this.parent.GetComp<CompFlickable>();

            base.PostSpawnSetup(respawningAfterLoad);
        }

        public override void PostDeSpawn(Map map)
        {
            EnhancedTemperatureUtility.GetNetManager(map).DeregisterProducer(this);
            ResetFlowVariables();
            base.PostDeSpawn(map);
        }

        public override string CompInspectStringExtra()
        {
            string str = "";

            if (IsOperating())
            {
                str += AirFlowOutputKey.Translate(new object[] { this.AirFlowOutput.ToString("#####0") });
                str += "\n";
                str += IntakeTempKey.Translate(new object[] { IntakeTemperature });
            }
            else
            {
                str = "PowerNeeded".Translate();
            }

            return str + "\n" + base.CompInspectStringExtra();
        }

        public override void ResetFlowVariables()
        {
            CurrentAirFlow = 0.0f;
            IntakeTemperature = 0.0f;
            IsOperatingAtHighPower = false;
            base.ResetFlowVariables();
        }
    }
}