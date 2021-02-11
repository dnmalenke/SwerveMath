using System;
using System.Drawing;
using System.Windows.Forms;
using Vortice.XInput;

namespace SwerveMath
{
    public partial class Form1 : Form
    {       
        int _prevPacketNum = 0;
        private SwerveDrive _swerveDrive;
        private readonly Pen _arrowPen = new Pen(Brushes.Black, 5);

        public Form1()
        {
            InitializeComponent();

            _arrowPen.EndCap = System.Drawing.Drawing2D.LineCap.ArrowAnchor;

            _swerveDrive = new SwerveDrive((double)numericUpDown5.Value, (double)numericUpDown4.Value, 1024);

            CalculateValues();
        }     
       
        private void CalculateValues()
        {
            _swerveDrive.UpdateWheels((double)numericUpDown1.Value, (double)numericUpDown2.Value, (double)numericUpDown3.Value);

            label4.Text = $"Back Left Speed: {Math.Round(_swerveDrive.RearLeftWheel.Speed,2)}";
            label11.Text = $"Front Left Speed: {Math.Round(_swerveDrive.FrontLeftWheel.Speed,2)}";
            label7.Text = $"Back Right Speed: {Math.Round(_swerveDrive.RearRightWheel.Speed, 2)}";
            label9.Text = $"Front Right Speed: {Math.Round(_swerveDrive.FrontRightWheel.Speed, 2)}";

            label5.Text = $"Back Left Angle: {Math.Round(_swerveDrive.RearLeftWheel.Angle,2)}";
            label10.Text = $"Front Left Angle: {Math.Round(_swerveDrive.FrontLeftWheel.Angle, 2)}";
            label6.Text = $"Back Right Angle: {Math.Round(_swerveDrive.RearRightWheel.Angle, 2)}";
            label8.Text = $"Front Right Angle: {Math.Round(_swerveDrive.FrontRightWheel.Angle, 2)}";

            label15.Text = $"Target Encoder Reading: {Math.Round(_swerveDrive.RearLeftWheel.EncoderTarget, 2)}";
            label16.Text = $"Target Encoder Reading: {Math.Round(_swerveDrive.RearRightWheel.EncoderTarget, 2)}";
            label17.Text = $"Target Encoder Reading: {Math.Round(_swerveDrive.FrontRightWheel.EncoderTarget, 2)}";
            label18.Text = $"Target Encoder Reading: {Math.Round(_swerveDrive.FrontLeftWheel.EncoderTarget, 2)}";

            DrawTrajectory(pictureBox1, _swerveDrive.RearLeftWheel);
            DrawTrajectory(pictureBox2, _swerveDrive.FrontLeftWheel);
            DrawTrajectory(pictureBox3, _swerveDrive.FrontRightWheel);
            DrawTrajectory(pictureBox4, _swerveDrive.RearRightWheel);
        }

        private void DrawTrajectory(PictureBox pictureBox, SwerveWheel swerveWheel)
        {
            Bitmap bmp = new Bitmap(pictureBox.Width, pictureBox.Height);
            using (Graphics g = Graphics.FromImage(bmp))
            {
                PointF center = new PointF(pictureBox.Width / 2, pictureBox.Height / 2);
                PointF end = new PointF((float)(center.X + swerveWheel.Speed * center.X * Math.Sin(Math.PI / 180 * swerveWheel.Angle)), 
                    (float)(center.X + swerveWheel.Speed * center.X * Math.Cos(Math.PI / 180 * swerveWheel.Angle)));

                g.DrawLine(_arrowPen, center, end);
            }
            pictureBox.Image = bmp;
        }        

        private void Form1_Load(object sender, EventArgs e)
        {
            XInput.GetCapabilities(0, DeviceQueryType.Gamepad, out Capabilities c);
            if (c.Type == DeviceType.Gamepad)
            {
                label12.Text = "Controller Connected";
                timer1.Enabled = true;

                numericUpDown1.Enabled = false;
                numericUpDown2.Enabled = false;
                numericUpDown3.Enabled = false;
            }
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            XInput.GetState(0, out State s);
            if (s.PacketNumber != _prevPacketNum)
            {
                _prevPacketNum = s.PacketNumber;

                numericUpDown1.Value = Math.Clamp((decimal)s.Gamepad.LeftThumbY / 32767, -1, 1);
                numericUpDown2.Value = Math.Clamp((decimal)s.Gamepad.LeftThumbX / 32767, -1, 1);
                numericUpDown3.Value = Math.Clamp((decimal)s.Gamepad.RightThumbX / 32767, -1, 1);
            }
        }

        private void NumericUpDown1_ValueChanged(object sender, EventArgs e)
        {
            CalculateValues();
        }

        private void numericUpDown2_ValueChanged(object sender, EventArgs e)
        {
            CalculateValues();
        }

        private void numericUpDown3_ValueChanged(object sender, EventArgs e)
        {
            CalculateValues();
        }

        private void numericUpDown5_ValueChanged(object sender, EventArgs e)
        {
            _swerveDrive.Length = (double)numericUpDown5.Value;
            CalculateValues();
        }

        private void numericUpDown4_ValueChanged(object sender, EventArgs e)
        {
            _swerveDrive.Width = (double)numericUpDown4.Value;
            CalculateValues();
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            _swerveDrive.SquareSpeeds = checkBox1.Checked;
            CalculateValues();
        }
    }
}
