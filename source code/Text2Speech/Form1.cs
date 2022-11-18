using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using System.Speech.Synthesis;

namespace Text2Speech
{
    public partial class Form1 : Form
    {
        SpeechSynthesizer ss = new SpeechSynthesizer();

        public Form1()
        {
            InitializeComponent();

            var systemInstalledVolice = ss.GetInstalledVoices();

            DataTable dt = new DataTable();
            dt.Columns.Add("value");
            dt.Columns.Add("name");

            foreach (var voiceProfile in systemInstalledVolice)
            {
                dt.Rows.Add(voiceProfile.VoiceInfo.Name, voiceProfile.VoiceInfo.Description);
            }

            cbVoice.DataSource = dt;
            cbVoice.DisplayMember = "name";
            cbVoice.ValueMember = "value";

            cbVoice.SelectedIndex = 0;

            textBox1.KeyDown += TextBox1_KeyDown;
        }

        private void TextBox1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Control && (e.KeyCode == Keys.A))
            {
                if (sender != null)
                    ((TextBox)sender).SelectAll();
                e.Handled = true;
                e.SuppressKeyPress = true;
            }
        }

        private void btSave_Click(object sender, EventArgs e)
        {
            SaveFileDialog sf = new SaveFileDialog();
            sf.Filter = "WAV|*.wav";
            if (sf.ShowDialog() != System.Windows.Forms.DialogResult.OK)
                return;

            ss.SpeakAsyncCancelAll();

            ss.SelectVoice(cbVoice.SelectedValue + "");
            ss.Rate = Convert.ToInt32(nmSpeed.Value);
            ss.SetOutputToWaveFile(sf.FileName);
            ss.SpeakAsync(textBox1.Text);
        }

        private void btRead_Click(object sender, EventArgs e)
        {
            ss.SpeakAsyncCancelAll();

            while (ss.State == SynthesizerState.Speaking)
            {
                System.Threading.Thread.Sleep(250);
            }

            ss.SelectVoice(cbVoice.SelectedValue + "");
            ss.Rate = Convert.ToInt32(nmSpeed.Value);
            ss.SetOutputToDefaultAudioDevice();
            ss.SpeakAsync(textBox1.Text);
        }

        private void btStopReading_Click(object sender, EventArgs e)
        {
            ss.SpeakAsyncCancelAll();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            var aa = ss.GetInstalledVoices();

            foreach (var bb in aa)
            {
                MessageBox.Show(bb.VoiceInfo.Name + ", " + bb.VoiceInfo.Description + ", " + bb.VoiceInfo.Age + ", " + bb.VoiceInfo.Gender);
            }
        }

        private void btMoreVoice_Click(object sender, EventArgs e)
        {
            Form2 f = new Form2();
            f.ShowDialog();
        }
    }
}