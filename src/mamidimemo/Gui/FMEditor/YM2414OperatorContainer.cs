﻿// copyright-holders:K.Ito
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using zanac.MAmidiMEmo.Instruments.Chips;
using zanac.MAmidiMEmo.Instruments;

namespace zanac.MAmidiMEmo.Gui.FMEditor
{
    public partial class YM2414OperatorContainer : RegisterContainerBase
    {
        private YM2414.YM2414Operator op;

        /// <summary>
        /// 
        /// </summary>
        public override string SerializeData
        {
            get
            {
                return SerializeProps(this,
                        nameof(op.EN),
                        nameof(op.AR),
                        nameof(op.D1R),
                        nameof(op.D2R),
                        nameof(op.RR),
                        nameof(op.SL),
                        nameof(op.TL),
                        nameof(op.RS),
                        nameof(op.MUL),
                        nameof(op.DT1),
                        nameof(op.AM),
                        nameof(op.DT2),
                        nameof(op.FINE),
                        nameof(op.FIX),
                        nameof(op.FIXR),
                        nameof(op.FIXF),
                        nameof(op.OSCW),
                        nameof(op.EGSF),
                        nameof(op.REV),
                        nameof(op.LS),
                        nameof(op.KVS)
                        );
            }
            set
            {
                DeserializeProps(this, value,
                    nameof(op.EN),
                    nameof(op.AR),
                    nameof(op.D1R),
                    nameof(op.D2R),
                    nameof(op.RR),
                    nameof(op.SL),
                    nameof(op.TL),
                    nameof(op.RS),
                    nameof(op.MUL),
                    nameof(op.DT1),
                    nameof(op.AM),
                    nameof(op.DT2),
                    nameof(op.FINE),
                    nameof(op.FIX),
                    nameof(op.FIXR),
                    nameof(op.FIXF),
                    nameof(op.OSCW),
                    nameof(op.EGSF),
                    nameof(op.REV),
                    nameof(op.LS),
                    nameof(op.KVS)
                    );
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public YM2414OperatorContainer(YM2414.YM2414Operator op, string name) : base(op, name)
        {
            InitializeComponent();

            this.op = op;

            AddControl(new RegisterFlag("EN", op.EN != 0 ? true : false));
            AddControl(new RegisterValue("AR", op.AR, 0, 31));
            AddControl(new RegisterValue("D1R", op.D1R, 0, 31));
            AddControl(new RegisterValue("D2R", op.D2R, 0, 31));
            AddControl(new RegisterValue("RR", op.RR, 0, 15));
            AddControl(new RegisterValue("SL", op.SL, 0, 15));
            AddControl(new RegisterValue("TL", op.TL, 0, 127));
            AddControl(new RegisterValue("RS", op.RS, 0, 3));
            AddControl(new RegisterValue("MUL", op.MUL, 0, 15));
            AddControl(new RegisterValue("DT1", op.DT1, 0, 7));
            AddControl(new RegisterValue("AM", op.AM, 0, 1));
            AddControl(new RegisterValue("DT2", op.DT2, 0, 3));
            AddControl(new RegisterValue("FINE", op.FINE, 0, 15));
            AddControl(new RegisterValue("FIX", op.FIX, 0, 1));
            AddControl(new RegisterValue("FIXR", op.FIXR, 0, 7));
            AddControl(new RegisterValue("FIXF", op.FIXR, 0, 15));
            AddControl(new RegisterValue("OSCW", op.OSCW, 0, 7));
            AddControl(new RegisterValue("EGSF", op.EGSF, 0, 3));
            AddControl(new RegisterValue("REV", op.REV, 0, 7));
            AddControl(new RegisterValue("LS", op.LS, 0, 99));
            AddControl(new RegisterValue("KVS", op.KVS, -1, 7));

            AddControl(new RegisterSpace("spc") { Dock = DockStyle.Right });
            AddControl(new RegisterOpzWaveForm((RegisterValue)GetControl("OSCW")));
            AddControl(new RegisterEnvForm(
                (RegisterValue)GetControl("AR"),
                (RegisterValue)GetControl("TL"),
                (RegisterValue)GetControl("D1R"), false,
                (RegisterValue)GetControl("SL"), false,
                (RegisterValue)GetControl("D2R"),
                (RegisterValue)GetControl("RR")
                ));
        }

    }
}
