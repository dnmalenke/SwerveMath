using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SwerveMath
{
    public class SwerveWheel
    {
        private double _angle;
        public double Angle
        {
            get { return _angle; }
            set
            {
                _angle = value;

                if (Math.Abs(PreviousAngle - _angle) is > 90 and < 270)
                {
                    if (_angle > 0)
                    {
                        _angle -= 180;
                    }
                    else
                    {
                        _angle += 180;
                    }

                    Speed *= -1;
                }

                if (PreviousAngle > 90 && _angle < -90)
                {
                    _revolutions++;
                }
                else if (PreviousAngle < -90 && _angle > 90)
                {
                    _revolutions--;
                }

                EncoderTarget = _angle * (EncoderCountsPerRevolution / 360.0) + EncoderCountsPerRevolution * _revolutions;

                PreviousAngle = _angle;
            }
        }
        public double PreviousAngle { get; set; }
        public double EncoderTarget { get; set; }
        public int EncoderCountsPerRevolution { get; init; }
        public double Speed { get; set; }

        private int _revolutions = 0;

        public SwerveWheel(int encoderCountsPerRevolution, double currentEncoderValue)
        {
            _revolutions = (int)currentEncoderValue / encoderCountsPerRevolution;
            EncoderCountsPerRevolution = encoderCountsPerRevolution;
        }
    }
}
