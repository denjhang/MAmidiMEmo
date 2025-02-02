﻿using Melanchall.DryWetMidi.Common;
using Melanchall.DryWetMidi.Core;
using MetroFramework.Controls;
using MetroFramework.Forms;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using zanac.MAmidiMEmo.Instruments.Chips;
using zanac.MAmidiMEmo.Midi;
using zanac.MAmidiMEmo.Properties;
using static zanac.MAmidiMEmo.Instruments.Chips.SP0256;

namespace zanac.MAmidiMEmo.Gui
{
    public partial class FormAllophonesEditor : FormBase
    {
        private SP0256Timbre timbre;
        private int timbreNo;
        private SP0256 inst;

        /// <summary>
        /// 
        /// </summary>
        public event EventHandler ValueChanged;

        /// <summary>
        /// 
        /// </summary>
        public string Allophones
        {
            get;
            set;
        }

        /// <summary>
        /// 
        /// </summary>
        public FormAllophonesEditor()
        {

        }

        /// <summary>
        /// 
        /// </summary>
        public FormAllophonesEditor(SP0256 inst, SP0256Timbre timbre, bool singleSelect)
        {
            this.inst = inst;
            this.timbre = timbre;
            for (int i = 0; i < inst.BaseTimbres.Length; i++)
            {
                if (inst.BaseTimbres[i] == timbre)
                {
                    timbreNo = i;
                    break;
                }
            }

            InitializeComponent();

            metroTextBoxAllophones.Text = timbre.Allophones;

            Size = Settings.Default.AllophonesEdSize;
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="e"></param>
        protected override void OnClosing(CancelEventArgs e)
        {
            base.OnClosing(e);

            Settings.Default.AllophonesEdSize = Size;
        }

        private TaggedNoteOnEvent noteOnEvent;



        private void metroButtonPlay_MouseDown(object sender, MouseEventArgs e)
        {
            Allophones = metroTextBoxAllophones.Text;
            ValueChanged?.Invoke(this, EventArgs.Empty);

            noteOnEvent = new TaggedNoteOnEvent((SevenBitNumber)69, (SevenBitNumber)127);
            noteOnEvent.Tag = new NoteOnTimbreInfo(timbre, timbreNo);
            inst.NotifyMidiEvent(noteOnEvent);
        }

        private void metroButtonPlay_MouseUp(object sender, MouseEventArgs e)
        {
            if (noteOnEvent != null)
            {
                NoteOffEvent noe = new NoteOffEvent(noteOnEvent.NoteNumber, noteOnEvent.Velocity);
                inst.NotifyMidiEvent(noe);

                noteOnEvent = null;
            }
        }

        private void metroButton2_Click(object sender, EventArgs e)
        {
            var mb = (MetroButton)sender;

            var insertText = mb.Text.Split(new string[] { "\r\n" }, StringSplitOptions.RemoveEmptyEntries)[0];

            var field = typeof(MetroTextBox).GetField("baseTextBox", BindingFlags.Instance | BindingFlags.Instance | BindingFlags.NonPublic);
            TextBox tb = (TextBox)field.GetValue(metroTextBoxAllophones);

            var selectionIndex = tb.SelectionStart;
            metroTextBoxAllophones.Text = metroTextBoxAllophones.Text.Insert(selectionIndex, insertText).Trim();
            tb.SelectionStart = selectionIndex + insertText.Length;
        }

        private void metroButton2_MouseDown(object sender, MouseEventArgs e)
        {
            var mb = (MetroButton)sender;

            string allophone = mb.Text.Split(new string[] { "\r\n" }, StringSplitOptions.RemoveEmptyEntries)[0] + " ";

            noteOnEvent = new TaggedNoteOnEvent((SevenBitNumber)69, (SevenBitNumber)127);
            noteOnEvent.Tag = new NoteOnTimbreInfo(timbre, timbreNo) { Tag = allophone };
            inst.NotifyMidiEvent(noteOnEvent);
        }

        private void metroButton2_MouseUp(object sender, MouseEventArgs e)
        {
            if (noteOnEvent != null)
            {
                NoteOffEvent noe = new NoteOffEvent(noteOnEvent.NoteNumber, noteOnEvent.Velocity);
                inst.NotifyMidiEvent(noe);

                noteOnEvent = null;
            }
        }

        private void metroButton1_Click(object sender, EventArgs e)
        {
            Process.Start("https://github.com/110-kenichi/mame/blob/master/docs/MAmidiMEmo/Chips/SP0256-AL2.pdf");
        }
    }
}
