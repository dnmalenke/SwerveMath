using System;
using System.Collections.Generic;
using System.Linq;

namespace SwerveMath
{
    public class SwerveDrive
    {
        public bool SquareSpeeds { get; set; } = false;
        public int EncoderCountsPerRevolution { get; init; }
        public double Deadband { get; set; } = 0.06;
        public double Length { get; set; }
        public double Width { get; set; }

        public SwerveWheel FrontLeftWheel { get; init; } 
        public SwerveWheel FrontRightWheel { get; init; }
        public SwerveWheel RearLeftWheel { get; init; }
        public SwerveWheel RearRightWheel { get; init; }
        public List<SwerveWheel> Wheels { get; init; } = new List<SwerveWheel>();

        public SwerveDrive(double length, double width, int encoderCountsPerRevolution, double frontLeftEncoderValue = 0, double frontRightEncoderValue = 0, double rearLeftEncoderValue = 0, double rearRightEncoderValue = 0)
        {
            Length = length;
            Width = width;
            EncoderCountsPerRevolution = encoderCountsPerRevolution;

            FrontLeftWheel = new SwerveWheel(encoderCountsPerRevolution, frontLeftEncoderValue);
            FrontRightWheel = new SwerveWheel(encoderCountsPerRevolution, frontRightEncoderValue);
            RearLeftWheel = new SwerveWheel(encoderCountsPerRevolution, rearLeftEncoderValue);
            RearRightWheel = new SwerveWheel(encoderCountsPerRevolution, rearRightEncoderValue);

            Wheels.AddRange(new List<SwerveWheel> { FrontLeftWheel, FrontRightWheel, RearLeftWheel, RearRightWheel });
        }

        public void UpdateWheels(double y1, double x1, double x2)
        {
            if (Math.Abs(y1) < Deadband)
            {
                y1 = 0;
            }

            if (Math.Abs(x1) < Deadband)
            {
                x1 = 0;
            }

            if (Math.Abs(x2) < Deadband)
            {
                x2 = 0;
            }

            y1 *= -1;

            double r = Hypotenuse(Length, Width);
            double a = x1 - (x2 * (Length / r));
            double b = x1 + (x2 * (Length / r));
            double c = y1 - (x2 * (Width / r));
            double d = y1 + (x2 * (Width / r));

            FrontLeftWheel.Speed = Hypotenuse(b, c);
            FrontRightWheel.Speed = Hypotenuse(b, d);
            RearLeftWheel.Speed = Hypotenuse(a, c);
            RearRightWheel.Speed = Hypotenuse(a, d);

            double maxSpeed = Wheels.Max(w => w.Speed);

            if (maxSpeed > 1)
            {
                Wheels.ForEach(w => w.Speed /= maxSpeed);
            }

            if (SquareSpeeds)
            {
                Wheels.ForEach(w => w.Speed *= w.Speed);
            }

            FrontLeftWheel.Angle = 180 * Math.Atan2(b, c) / Math.PI;
            FrontRightWheel.Angle = 180 * Math.Atan2(b, d) / Math.PI;
            RearLeftWheel.Angle = 180 * Math.Atan2(a, c) / Math.PI;
            RearRightWheel.Angle = 180 * Math.Atan2(a, d) / Math.PI;
        }

        private static double Hypotenuse(double x, double y)
        {
            return Math.Sqrt(Math.Pow(x, 2) + Math.Pow(y, 2));
        }
    }
}
