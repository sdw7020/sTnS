using System;
using System.Drawing;
using System.Windows.Forms;
using System.Diagnostics;
using System.IO;

namespace sTnS
{
    public partial class sTnS : Form
    {
        public sTnS()
        {
            InitializeComponent();
        }

        protected override CreateParams CreateParams
        {
            get
            {
                const int CS_DROPSHADOW = 0x20000;
                CreateParams cp = base.CreateParams;
                cp.ClassStyle |= CS_DROPSHADOW;
                return cp;
            }
        }

        Stopwatch sw = new Stopwatch();
        private void btn_startpause_Click(object sender, EventArgs e)
        {
            if (sw.IsRunning == false)
            {
                sw.Start();
                btn_startpause.Text = "Stop";
                btn_splitreset.Text = "Split";
                btn_startpause.FlatAppearance.MouseOverBackColor = Color.FromArgb(200, 0, 0);
                btn_startpause.FlatAppearance.MouseDownBackColor = Color.FromArgb(200, 100, 100);
            }
            else
            {
                sw.Stop();
                btn_startpause.Text = "Start";
                btn_splitreset.Text = "Reset";
                btn_startpause.FlatAppearance.MouseOverBackColor = Color.FromArgb(0, 200, 0);
                btn_startpause.FlatAppearance.MouseDownBackColor = Color.FromArgb(100, 200, 100);
            }

        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            lbl_timer.Text = sw.Elapsed.ToString();
            lbl_time.Text = DateTime.Now.ToString("HH:mm");
        }

        private void btn_splitreset_Click(object sender, EventArgs e)
        {
            if (sw.IsRunning == false)
            {
                sw.Reset();
                lbo_laps.Items.Clear();
            }
            else
            {
                lbo_laps.Items.Add(sw.Elapsed.ToString());
                lbo_laps.SelectedIndex = lbo_laps.Items.Count - 1;
                lbo_laps.SelectedIndex = -1;
            }
        }

        private void btn_close_Click(object sender, EventArgs e)
        {
            DialogResult dr = MessageBox.Show("Are you sure you want to close sTnS?", "sTnS 1.0", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (dr == DialogResult.Yes)
            {
                this.Close();
            }
        }

        private void btn_fullscreen_Click(object sender, EventArgs e)
        {
            if (this.WindowState != FormWindowState.Maximized)
            {
                this.WindowState = FormWindowState.Maximized;
            }
            else
            {
                this.WindowState = FormWindowState.Normal;
                lbo_laps.SelectedIndex = lbo_laps.Items.Count - 1;
                lbo_laps.SelectedIndex = -1;
            }
        }

        private void lbl_time_Click(object sender, EventArgs e)
        {
            if (lbl_time.ForeColor == Color.FromArgb(222, 222, 222))
            {
                lbl_time.ForeColor = Color.FromArgb(18, 18, 18);
            }
            else
            {
                lbl_time.ForeColor = Color.FromArgb(222, 222, 222);
            }
        }

        private void lbl_timer_Click(object sender, EventArgs e)
        {

        }

        private void btn_openstopwatch_Click(object sender, EventArgs e)
        {
            grp_menu.Visible = false;
            grp_timer.Visible = false;
            grp_stopwatch.Visible = true;
        }

        private void btn_menu_Click(object sender, EventArgs e)
        {
            if (grp_stopwatch.Visible == true)
            {
                grp_stopwatch.Visible = false;
                grp_menu.Visible = true;
            }
            else if (grp_timer.Visible == true)
            {
                grp_timer.Visible = false;
                grp_menu.Visible = true;
            }
        }

        private void btn_opentimer_Click(object sender, EventArgs e)
        {
            grp_menu.Visible = false;
            grp_stopwatch.Visible = false;
            grp_timer.Visible = true;
            txt_hh.SelectAll();
            txt_hh.Focus();
        }

        private void label1_Click(object sender, EventArgs e)
        {
            grp_timer.Visible = false;
            grp_menu.Visible = true;
        }

        private void btn_save_Click(object sender, EventArgs e)
        {
            if (grp_stopwatch.Visible == true && grp_menu.Visible == false)
            {

                if (sw.ElapsedMilliseconds > 0)
                {
                    if (sw.IsRunning == true)
                    {
                        sw.Stop();
                        btn_startpause.Text = "Start";
                        btn_splitreset.Text = "Reset";
                        btn_startpause.FlatAppearance.MouseOverBackColor = Color.FromArgb(0, 200, 0);
                        btn_startpause.FlatAppearance.MouseDownBackColor = Color.FromArgb(100, 200, 100);
                    }
                    if (Directory.Exists(@"C:\Users\" + Environment.UserName + @"\Documents\sTnS 1.0\") == false)
                    {
                        Directory.CreateDirectory(@"C:\Users\" + Environment.UserName + @"\Documents\sTnS 1.0\");
                    }
                    if (Directory.Exists(@"C:\Users\" + Environment.UserName + @"\Documents\sTnS 1.0\Stopwatch results") == false)
                    {
                        Directory.CreateDirectory(@"C:\Users\" + Environment.UserName + @"\Documents\sTnS 1.0\Stopwatch results");
                    }
                    using (StreamWriter results = new StreamWriter(@"C:\Users\" + Environment.UserName + @"\Documents\sTnS 1.0\Stopwatch results\StopwatchResults_" + DateTime.Now.ToString(@"MM-dd-yy_HHmmss") + ".txt"))
                    {
                        results.WriteLine("sTnS 1.0");
                        results.WriteLine("Stopwatch Results");
                        results.WriteLine(DateTime.Now.ToString(@"MM-dd-yyyy, HH:mm:ss"));
                        results.WriteLine("---------------------");
                        results.WriteLine("");
                        int laps = 0;
                        foreach (var item in lbo_laps.Items)
                        {
                            laps++;
                            results.WriteLine("Lap " + laps + ": " + item);
                        }
                        results.WriteLine("");
                        results.WriteLine("Total time: " + sw.Elapsed);
                        string str_avg = sw.Elapsed.TotalMilliseconds.ToString();
                        double dbl_avg = (Double.Parse(str_avg) / laps) / 1000;
                        results.WriteLine("");
                        results.WriteLine("Average time: " + dbl_avg + " seconds");
                        results.WriteLine("");
                        results.WriteLine("---------------------");
                        results.WriteLine("");
                        results.WriteLine("\n© sdw7020 - 2016");
                        if (MessageBox.Show("The stopwatch results are saved.", "sTnS 1.0", MessageBoxButtons.OK, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1) == DialogResult.OK)
                        {
                            Process.Start(@"C:\Users\" + Environment.UserName + @"\Documents\sTnS 1.0\Stopwatch results\");
                        }
                    }
                }
                else
                {
                    MessageBox.Show("There are no results to save.", "sTnS 1.0", MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1);
                }
            }
            else
            {
                MessageBox.Show("Saving is only available when using stopwatch.", "sTnS 1.0", MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1);
            }
        }

        private void btn_min_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;
        }

        private void txt_hh_TextChanged(object sender, EventArgs e)
        {
            int timer_hh;
            bool isTimer_hh = Int32.TryParse(txt_hh.Text, out timer_hh);
            if (!isTimer_hh)
            {
                txt_hh.Clear();
                txt_hh.SelectAll();
                txt_hh.Focus();
            }
            else
            {
                if (txt_hh.TextLength >= 2)
                {
                    txt_mm.SelectAll();
                    txt_mm.Focus();
                }
            }
        }

        private void txt_mm_TextChanged(object sender, EventArgs e)
        {
            int timer_mm;
            bool isTimer_mm = Int32.TryParse(txt_mm.Text, out timer_mm);
            if (!isTimer_mm)
            {
                txt_mm.Clear();
                txt_mm.SelectAll();
                txt_mm.Focus();
            }
            else
            {
                if (Int32.Parse(txt_mm.Text) >= 60)
                {
                    txt_mm.Clear();
                    txt_mm.SelectAll();
                    txt_mm.Focus();
                }
                else
                {
                    if (txt_mm.TextLength >= 2)
                    {
                        txt_ss.SelectAll();
                        txt_ss.Focus();
                    }
                }
            }
        }

        private void txt_ss_TextChanged(object sender, EventArgs e)
        {

            int timer_ss;
            bool isTimer_ss = Int32.TryParse(txt_ss.Text, out timer_ss);
            if (!isTimer_ss)
            {
                txt_ss.Clear();
                txt_ss.SelectAll();
                txt_ss.Focus();
            }
            else
            {
                if (Int32.Parse(txt_ss.Text) >= 60)
                {
                    txt_ss.Clear();
                    txt_ss.SelectAll();
                    txt_ss.Focus();
                }
                else
                {
                    if (txt_ss.TextLength >= 2)
                    {
                        txt_ms.SelectAll();
                        txt_ms.Focus();
                    }
                }
            }
        }

        private void txt_ms_TextChanged(object sender, EventArgs e)
        {
            int timer_ms;
            bool isTimer_ms = Int32.TryParse(txt_ms.Text, out timer_ms);
            if (!isTimer_ms)
            {
                txt_ms.Clear();
                txt_ms.SelectAll();
                txt_ms.Focus();
            }
            else
            {
                if (Int32.Parse(txt_ms.Text) >= 60)
                {
                    txt_ms.Clear();
                    txt_ms.SelectAll();
                    txt_ms.Focus();
                }
                else
                {
                    if (txt_ms.TextLength >= 2)
                    {
                        // select button start
                    }
                }
            }
        }

        Stopwatch cd = new Stopwatch();
        private void btn_timerstart_Click(object sender, EventArgs e)
        {
            if (cd.IsRunning == false)
            {
                cd.Start();
                btn_timerstart.Text = "Stop";
                btn_timerstart.FlatAppearance.MouseOverBackColor = Color.FromArgb(200, 0, 0);
                btn_timerstart.FlatAppearance.MouseDownBackColor = Color.FromArgb(200, 100, 100);
                grp_cd.Visible = true;
            }
            else
            {
                cd.Stop();
                btn_timerstart.Text = "Start";
                btn_timerstart.FlatAppearance.MouseOverBackColor = Color.FromArgb(0, 200, 0);
                btn_timerstart.FlatAppearance.MouseDownBackColor = Color.FromArgb(100, 200, 100);
                grp_cd.Visible = false;
            }
        }

        private void btn_timerreset_Click(object sender, EventArgs e)
        {
            txt_hh.Clear();
            txt_mm.Clear();
            txt_ss.Clear();
            txt_ms.Clear();
            txt_hh.SelectAll();
            txt_hh.Focus();
        }
    }
}
