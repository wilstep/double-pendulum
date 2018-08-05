using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace double_pendulum
{
    class Motion
    {
        double dt = 0.0002;
        double th1, th1d, th2, th2d;
        double l1, l2;
        double b1_pf, c1_pf_1, c1_pf_2;
        double a2_pf, c2_pf_1, c2_pf_2;

        public Motion(double m1_div_m2, double l1, double l2)
        {
            // Constructor takes 3 arguments, the ratio of the two masses, 
            // l1 the length of the first pendulum link and
            // l2 the length of the second pendulum link
            double m_ratio;

            m_ratio = 1.0 + m1_div_m2;
            m_ratio = 1.0 / m_ratio;
            b1_pf = m_ratio * l2 / l1;
            c1_pf_1 = -9.8 / l1;
            c1_pf_2 = -m_ratio * l2 / l1;
            a2_pf = l1 / l2;
            c2_pf_1 = -9.8 / l2;
            c2_pf_2 = l1 / l2;
            this.l1 = l1;
            this.l2 = l2;
        }

        public double GetTh1() { return th1; }
        public double GetTh2() { return th2; }

        public void Reset(double th1, double th2)
        {
            this.th1 = th1;
            this.th2 = th2;
            th1d = th2d = 0.0;
        }

        public void Run(int n)
        {
            for (int i = 0; i < n; ++i) Runge_Kutta();
        }

        void Runge_Kutta()
        {
            // 4th Order Runge-Kutta (RK4) applied to the 4 degrees of freedom,
            // (th1, th1d, th2, th2d). These 4 degrees of freedom are the angles
            // of the two pendulum links (th1, th2) and their angular velocities (th1d, th2d)
            // https://en.wikipedia.org/wiki/Runge-Kutta_methods

            double th1dd, th2dd;
            double k1_th1, k1_th1d, k1_th2, k1_th2d;
            double k2_th1, k2_th1d, k2_th2, k2_th2d;
            double k3_th1, k3_th1d, k3_th2, k3_th2d;
            double k4_th1, k4_th1d, k4_th2, k4_th2d;
            double b1, c1, a2, c2;
            double sin_1, sin_2, sin_12, cos_12;
            double th1_t, th2_t, th1d_t, th2d_t;

            //  1st RK step
            sin_1 = Math.Sin(th1);
            sin_2 = Math.Sin(th2);
            sin_12 = Math.Sin(th1 - th2);
            cos_12 = Math.Cos(th1 - th2);
            // b1, c1, a2, c2 are coefficients for the simultaneous equations
            // that must be solved
            b1 = b1_pf * cos_12; 
            c1 = c1_pf_2 * sin_12 * th2d * th2d;
            c1 += c1_pf_1 * sin_1;
            a2 = a2_pf * cos_12;
            c2 = c2_pf_2 * sin_12 * th1d * th1d;
            c2 += c2_pf_1 * sin_2;
            // th1dd and th2dd are the angular accelerations
            th1dd = c1 - c2 * b1;
            th1dd /= 1.0 - a2 * b1;
            th2dd = c2 - th1dd * a2;
            k1_th1 = dt * th1d;
            k1_th1d = dt * th1dd;
            k1_th2 = dt * th2d;
            k1_th2d = dt * th2dd;
            //  2nd RK step
            th1_t = th1 + 0.5 * k1_th1;
            th1d_t = th1d + 0.5 * k1_th1d;
            th2_t = th2 + 0.5 * k1_th2;
            th2d_t = th2d + 0.5 * k1_th2d;
            sin_1 = Math.Sin(th1_t);
            sin_2 = Math.Sin(th2_t);
            sin_12 = Math.Sin(th1_t - th2_t);
            cos_12 = Math.Cos(th1_t - th2_t);
            b1 = b1_pf * cos_12;
            c1 = c1_pf_2 * sin_12 * th2d_t * th2d_t;
            c1 += c1_pf_1 * sin_1;
            a2 = a2_pf * cos_12;
            c2 = c2_pf_2 * sin_12 * th1d_t * th1d_t;
            c2 += c2_pf_1 * sin_2;
            th1dd = c1 - c2 * b1;
            th1dd /= 1.0 - a2 * b1;
            th2dd = c2 - th1dd * a2;
            k2_th1 = dt * th1d_t;
            k2_th1d = dt * th1dd;
            k2_th2 = dt * th2d_t;
            k2_th2d = dt * th2dd;
            //  3rd RK step
            th1_t = th1 + 0.5 * k2_th1;
            th1d_t = th1d + 0.5 * k2_th1d;
            th2_t = th2 + 0.5 * k2_th2;
            th2d_t = th2d + 0.5 * k2_th2d;
            sin_1 = Math.Sin(th1_t);
            sin_2 = Math.Sin(th2_t);
            sin_12 = Math.Sin(th1_t - th2_t);
            cos_12 = Math.Cos(th1_t - th2_t);
            b1 = b1_pf * cos_12;
            c1 = c1_pf_2 * sin_12 * th2d_t * th2d_t;
            c1 += c1_pf_1 * sin_1;
            a2 = a2_pf * cos_12;
            c2 = c2_pf_2 * sin_12 * th1d_t * th1d_t;
            c2 += c2_pf_1 * sin_2;
            th1dd = c1 - c2 * b1;
            th1dd /= 1.0 - a2 * b1;
            th2dd = c2 - th1dd * a2;
            k3_th1 = dt * th1d_t;
            k3_th1d = dt * th1dd;
            k3_th2 = dt * th2d_t;
            k3_th2d = dt * th2dd;
            //  4th RK step
            th1_t = th1 + k3_th1;
            th1d_t = th1d + k3_th1d;
            th2_t = th2 + k3_th2;
            th2d_t = th2d + k3_th2d;
            sin_1 = Math.Sin(th1_t);
            sin_2 = Math.Sin(th2_t);
            sin_12 = Math.Sin(th1_t - th2_t);
            cos_12 = Math.Cos(th1_t - th2_t);
            b1 = b1_pf * cos_12;
            c1 = c1_pf_2 * sin_12 * th2d_t * th2d_t;
            c1 += c1_pf_1 * sin_1;
            a2 = a2_pf * cos_12;
            c2 = c2_pf_2 * sin_12 * th1d_t * th1d_t;
            c2 += c2_pf_1 * sin_2;
            th1dd = c1 - c2 * b1;
            th1dd /= 1.0 - a2 * b1;
            th2dd = c2 - th1dd * a2;
            k4_th1 = dt * th1d_t;
            k4_th1d = dt * th1dd;
            k4_th2 = dt * th2d_t;
            k4_th2d = dt * th2dd;
            //  now calculate the updated values
            th1 += (k1_th1 + 2.0 * k2_th1 + 2.0 * k3_th1 + k4_th1) / 6.0;
            th1d += (k1_th1d + 2.0 * k2_th1d + 2.0 * k3_th1d + k4_th1d) / 6.0;
            th2 += (k1_th2 + 2.0 * k2_th2 + 2.0 * k3_th2 + k4_th2) / 6.0;
            th2d += (k1_th2d + 2.0 * k2_th2d + 2.0 * k3_th2d + k4_th2d) / 6.0;
        }
    }
}
