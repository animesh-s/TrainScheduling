﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Signals
{
    class Calculate
    {
        List<double> distance = new List<double>();
        List<int> time = new List<int>();
        List<int> signal = new List<int>();
        List<double> velocity = new List<double>();
        List<int> flag = new List<int>();
        List<int> count = new List<int>();
        List<int> hasReached = new List<int>();
        List<int> sg = new List<int>();
        List<int> fs = new List<int>();
        List<int> sp = new List<int>();
        List<int> timer = new List<int>();
        List<int> special = new List<int>();
        List<int> diff = new List<int>();
        List<double> s = new List<double>();
        List<double> d = new List<double>();

        private int temp = 1, tc = 0, start = 1, var = 0, t = 0;
        private double v = 0;

        /* Constructor */
        public Calculate(int m, int n, double vM)
        {
            for (int i = 0; i < m; i++)
            {
                distance.Add(0);
                time.Add(0);
                velocity.Add(vM);
                flag.Add(0);
                count.Add(0);
                hasReached.Add(0);
                s.Add(0);
                d.Add(0);
                sg.Add(0);
                fs.Add(0);
                sp.Add(0);
                timer.Add(0);
                special.Add(0);
            }

            for (int i = 0; i < n; i++)
            {
                signal.Add(3);
                diff.Add(0);
            }
        }

        public void Simulate(int m, int n, int counter, int gap, string currentTime, int speedTimer, Form1 form, double vM, double vR, double sD)
        {
            double p = 0;
            Form1 form1 = form;

            /* If the train at the front finishes */
            try
            {
                if (distance[tc] >= (sD * (n + 1)))
                {
                    tc = tc + 1;
                    start = start + 1;
                    var = 0;
                }
            }
            catch (Exception e)
            {
                Console.Beep();
                UpdateInfoConsole(form1, "\r\n\r\nError ! : " + e.Message);
            }

            /* All the signals ahead of the train at the front are given Proceed */
            try
            {
                int u = (int)(distance[tc] / sD);
                for (int i = u; i < n; i++)
                {
                    signal[i] = 3;
                }
            }
            catch (Exception e)
            {
                Console.Beep();
                UpdateInfoConsole(form1, "\r\n\r\nError ! : " + e.Message);
            }

            /* Speed, Time and Distance Calculation for the train at the front and not yet finished */
            if (distance[tc] <= (sD * (n + 1)))
            {
                /* If the train at the train has just become the train at the front and its velocity is not 60 km/hr */
                if (velocity[tc] != vM)
                {
                    if (var == 0)
                    {
                        v = velocity[tc];
                        t = (int)((3600 * 2 * sD) / (v + vM));
                        count[tc] = 0;
                        var++;
                    }

                    if (count[tc] != t)
                    {
                        velocity[tc] = velocity[tc] + (((vM - v) * (vM + v)) / (double)(7200 * sD));
                        s[tc] = (velocity[tc] / 3600) + (((vM - v) * (vM + v)) / (double)(sD * 7200 * 3600));
                        distance[tc] = distance[tc] + s[tc];

                        time[tc] = time[tc] + 1;
                        count[tc]++;
                    }

                    if (count[tc] == t)
                    {
                        velocity[tc] = vM;
                    }
                }

                /* If the velocity is or has become vM km/hr */
                if (velocity[tc] == vM)
                {
                    distance[tc] = distance[tc] + (velocity[tc] / 3600);
                    time[tc] = time[tc] + 1;
                }
            }

            /* For making trains enter the stretch after every 'user input' number of seconds later */
            if ((counter % (gap + 1)) == 0)
            {
                if (temp < m)
                {
                    temp++;
                }
            }

            /* Speed, Time and Distance calculation for all trains leaving the train at the front */
            for (int i = start; i < temp; i++)
            {
                hasReached[i] = 0;

                for (int j = 1; j <= n; j++)
                {
                    /* Checking if the train reaches any signal */
                    if (Math.Abs((sD * j) - distance[i]) < 0.007)
                    {
                        sg[i] = j;
                        hasReached[i] = 1;

                        /* Calculating distance of the train from the train ahead of it */
                        p = distance[i - 1] - distance[i];

                        /* Cases depending on the gap between the two trains */
                        if (p > 0 && p < sD)
                        {
                            try
                            {
                                signal[j - 1] = 0;
                            }
                            catch (Exception e)
                            {
                                UpdateInfoConsole(form1, "\nError! : " + e.Message);
                            }
                        }

                        if (p > sD && p < (sD * 2))
                        {
                            if (j == n)
                            {
                                signal[j - 1] = 3;
                            }
                            else
                            {
                                try
                                {
                                    signal[j - 1] = 1;
                                    signal[j] = 0;
                                }
                                catch (Exception e)
                                {
                                    UpdateInfoConsole(form1, "\nError! : " + e.Message);
                                }
                            }
                        }

                        if (p > (sD * 2) && p < (sD * 3))
                        {
                            if (j == (n - 1))
                            {
                                signal[n - 2] = 3;
                                signal[n - 1] = 3;
                            }
                            else
                            {
                                try
                                {
                                    signal[j - 1] = 2;
                                    signal[j] = 1;
                                    signal[j + 1] = 0;
                                }
                                catch (Exception e)
                                {
                                    UpdateInfoConsole(form1, "\nError! : " + e.Message);
                                }
                            }
                        }

                        if (p > (sD * 3))
                        {
                            int k = (int)(distance[i - 1] / sD);

                            try
                            {
                                if (k == (n + 1))
                                {
                                    for (int l = j - 1; l < k - 1; l++)
                                    {
                                        signal[l] = 3;
                                    }
                                }

                                else
                                {
                                    signal[k - 1] = 0;
                                    signal[k - 2] = 1;
                                    signal[k - 3] = 2;

                                    for (int l = k - 4; l >= j - 1; l--)
                                    {
                                        signal[l] = 3;
                                    }
                                }
                            }
                            catch (Exception e)
                            {
                                UpdateInfoConsole(form1, "\nError! : " + e.Message);
                            }
                        }

                        /* Attention and velocity = vM km/hr */
                        if (signal[j - 1] == 2 && (vM - velocity[i]) < 0.21)
                        {
                            count[i] = 0;
                            flag[i] = 1;
                            special[i] = 0;
                            time[i] = time[i] + 1;
                            count[i]++;

                            velocity[i] = velocity[i] + ((double)(vR - vM) * (vR + vM) / (7200 * sD));
                            s[i] = (velocity[i] / 3600) + ((double)(vR - vM) * (vR + vM) / (sD * 7200 * 7200));
                            distance[i] = distance[i] + s[i];
                        }

                        /* Attention and velocity = vR km/hr */
                        if (signal[j - 1] == 2 && velocity[i] < (vR + 1) && (vR - velocity[i]) < 0.11)
                        {
                            count[i] = 0;
                            flag[i] = 2;
                            special[i] = 0;
                            time[i] = time[i] + 1;
                            count[i]++;

                            s[i] = (velocity[i] / 3600);
                            distance[i] = distance[i] + s[i];
                        }

                        /* Attention and velocity = 0 km/hr */
                        if (signal[j - 1] == 2 && (velocity[i] - 0) < 0.11)
                        {
                            count[i] = 0;
                            special[i] = 1;
                            flag[i] = 5;
                            time[i] = time[i] + 1;
                            count[i]++;

                            velocity[i] = velocity[i] + ((double)(vR * vR) / (7200 * sD));
                            s[i] = (velocity[i] / 3600) + ((double)(vR * vR) / (sD * 7200 * 7200));
                            distance[i] = distance[i] + s[i];
                        }

                        /* Caution and velocity = vM km/hr */
                        if (signal[j - 1] == 1 && (vM - velocity[i]) < 0.021)
                        {
                            count[i] = 0;
                            flag[i] = 3;
                            special[i] = 0;
                            time[i] = time[i] + 1;
                            count[i]++;

                            velocity[i] = velocity[i] - ((double)(vM * vM) / (7200 * sD));
                            s[i] = (velocity[i] / 3600) - ((double)(vM * vM) / (sD * 7200 * 7200));
                            distance[i] = distance[i] + s[i];
                        }

                        /* Caution and velocity = vR km/hr */
                        if (signal[j - 1] == 1 && velocity[i] < (vR + 1) && (vR - velocity[i]) < 0.11)
                        {
                            count[i] = 0;
                            flag[i] = 4;
                            special[i] = 0;
                            time[i] = time[i] + 1;
                            count[i]++;

                            velocity[i] = velocity[i] - ((double)(vR * vR) / (7200 * sD));
                            s[i] = (velocity[i] / 3600) - ((double)(vR * vR) / (sD * 7200 * 7200));
                            distance[i] = distance[i] + s[i];
                        }

                        /* Caution and velocity = 0 km/hr */
                        if (signal[j - 1] == 1 && (velocity[i] - 0) < 0.11)
                        {
                            count[i] = 0;
                            special[i] = 1;
                            flag[i] = 5;
                            time[i] = time[i] + 1;
                            count[i]++;

                            velocity[i] = velocity[i] + ((double)(vR * vR) / (7200 * sD));
                            s[i] = (velocity[i] / 3600) + ((double)(vR * vR) / (sD * 7200 * 7200));
                            distance[i] = distance[i] + s[i];
                        }

                        /* Proceed and velocity = vM km/hr */
                        if (signal[j - 1] == 3 && (vM - velocity[i]) < 0.021)
                        {
                            count[i] = 0;
                            flag[i] = 6;
                            special[i] = 0;
                            time[i] = time[i] + 1;
                            count[i]++;

                            s[i] = (velocity[i] / 3600);
                            distance[i] = distance[i] + s[i];
                        }

                        /* Proceed and velocity = vR km/hr */
                        if (signal[j - 1] == 3 && velocity[i] < (vR + 1) && (vR - velocity[i]) < 0.11)
                        {
                            count[i] = 0;
                            flag[i] = 7;
                            special[i] = 0;
                            time[i] = time[i] + 1;
                            count[i]++;

                            velocity[i] = velocity[i] + ((double)(vM - vR) * (vR + vM) / (7200 * sD));
                            s[i] = (velocity[i] / 3600) + ((double)(vM - vR) * (vR + vM) / (sD * 7200 * 7200));
                            distance[i] = distance[i] + s[i];
                        }

                        /* Proceed and velocity = 0 km/hr */
                        if (signal[j - 1] == 3 && (velocity[i] - 0) < 0.11)
                        {
                            count[i] = 0;
                            special[i] = 1;
                            flag[i] = 5;
                            time[i] = time[i] + 1;
                            count[i]++;

                            velocity[i] = velocity[i] + ((double)(vR * vR) / (7200 * sD));
                            s[i] = (velocity[i] / 3600) + ((double)(vR * vR) / (sD * 7200 * 7200));
                            distance[i] = distance[i] + s[i];
                        }

                        /* Stop and velocity = vM km/hr */
                        if (signal[j - 1] == 0 && (vM - velocity[i]) < 0.021)
                        {
                            count[i] = 0;
                            flag[i] = 8;
                            special[i] = 0;
                            d[i] = p;
                            fs[i] = 0;
                            time[i] = time[i] + 1;
                            count[i]++;

                            velocity[i] = velocity[i] - ((double)(vM * vM) / (7200 * p));
                            s[i] = (velocity[i] / 3600) - ((double)(vM * vM) / (p * 7200 * 7200));
                            distance[i] = distance[i] + s[i];
                        }

                        /* Stop and velocity = vR km/hr */
                        if (signal[j - 1] == 0 && velocity[i] < (vR + 1) && (vR - velocity[i]) < 1)
                        {
                            count[i] = 0;
                            flag[i] = 9;
                            special[i] = 0;
                            d[i] = p;
                            fs[i] = 0;
                            time[i] = time[i] + 1;
                            count[i]++;

                            velocity[i] = velocity[i] - ((double)(vR * vR) / (7200 * p));
                            s[i] = (velocity[i] / 3600) - ((double)(vR * vR) / (p * 7200 * 7200));
                            distance[i] = distance[i] + s[i];
                        }

                        /* Stop and velocity = 0 km/hr */
                        if (signal[j - 1] == 0 && (velocity[i] - 0) < 0.11)
                        {
                            count[i] = 0;
                            special[i] = 1;
                            flag[i] = 5;
                            time[i] = time[i] + 1;
                            count[i]++;

                            velocity[i] = velocity[i] + ((double)(vR * vR) / (7200 * sD));
                            s[i] = (velocity[i] / 3600) + ((double)(vR * vR) / (sD * 7200 * 7200));
                            distance[i] = distance[i] + s[i];
                        }


                        if (special[i] == 1)
                        {
                            Increment(i, n, vM, vR, sD);
                        }
                    }
                }

                if (hasReached[i] == 0)
                {
                    Increment(i, n, vM, vR, sD);
                }
            }
        }

        /* Speed, Time and Distance calculation when the trains are between signals */
        public void Increment(int i, int n, double vM, double vR, double sD)
        {
            switch (flag[i])
            {
                /* Going with vM km/hr */
                case 0:
                    if (count[i] != ((int)((3600 * sD) / vM) + 1))
                    {
                        s[i] = (velocity[i] / 3600);
                        distance[i] = distance[i] + s[i];
                        count[i]++;
                        time[i] = time[i] + 1;
                    }
                    else
                    {
                        count[i] = 0;
                        Rectify(i, n, sD);
                    }
                    break;

                /* Going from vM km/hr to vR km/hr */
                case 1:
                    if (count[i] != (int)((7200 * sD) / (vM + vR)))
                    {
                        velocity[i] = velocity[i] + ((double)(vR - vM) * (vR + vM) / (7200 * sD));
                        s[i] = (velocity[i] / 3600) + ((double)(vR - vM) * (vR + vM) / (sD * 7200 * 7200));
                        distance[i] = distance[i] + s[i];
                        count[i]++;
                        time[i] = time[i] + 1;
                    }
                    else
                    {
                        count[i] = 0;
                        Rectify(i, n, sD);
                    }
                    break;

                /* Going with vR km/hr */
                case 2:
                    if (count[i] != (int)((3600 * sD) / vR))
                    {
                        s[i] = (velocity[i] / 3600);
                        distance[i] = distance[i] + s[i];
                        count[i]++;
                        time[i] = time[i] + 1;
                    }
                    else
                    {
                        count[i] = 0;
                        Rectify(i, n, sD);
                    }
                    break;

                /* Going from vM km/hr to 0 km/hr */
                case 3:
                    if (count[i] != (int)((7200 * sD) / vM))
                    {
                        velocity[i] = velocity[i] - ((double)(vM * vM) / (7200 * sD));
                        s[i] = (velocity[i] / 3600) - ((double)(vM * vM) / (sD * 7200 * 7200));
                        distance[i] = distance[i] + s[i];
                        count[i]++;
                        time[i] = time[i] + 1;

                        if (count[i] == ((int)((7200 * sD) / vM) - 1))
                        {
                            velocity[i] = 0;
                            distance[i] = sD * (sg[i] + 1);
                        }
                    }
                    else
                    {
                        count[i] = 0;
                        Rectify(i, n, sD);
                    }
                    break;

                /* Going from vR km/hr to 0 km/hr */
                case 4:
                    if (count[i] != (int)((7200 * sD) / vR))
                    {
                        velocity[i] = velocity[i] - ((double)(vR * vR) / (7200 * sD));
                        s[i] = (velocity[i] / 3600) - ((double)(vR * vR) / (sD * 7200 * 7200));
                        distance[i] = distance[i] + s[i];
                        count[i]++;
                        time[i] = time[i] + 1;

                        if (count[i] == ((int)((7200 * sD) / vR) - 1))
                        {
                            velocity[i] = 0;
                            distance[i] = sD * (sg[i] + 1);
                        }
                    }
                    else
                    {
                        count[i] = 0;
                        Rectify(i, n, sD);
                    }
                    break;

                /* Going from 0 km/hr to vR km/hr */
                case 5:
                    if (count[i] != (int)((7200 * sD) / vR))
                    {
                        velocity[i] = velocity[i] + ((double)(vR * vR) / (7200 * sD));
                        s[i] = (velocity[i] / 3600) + ((double)(vR * vR) / (sD * 7200 * 7200));
                        distance[i] = distance[i] + s[i];
                        count[i]++;
                        time[i] = time[i] + 1;
                    }
                    else
                    {
                        count[i] = 0;
                        Rectify(i, n, sD);
                    }
                    break;

                /* Going with vM km/hr */
                case 6:
                    if (count[i] != (int)((3600 * sD) / vM))
                    {
                        s[i] = (velocity[i] / 3600);
                        distance[i] = distance[i] + s[i];
                        count[i]++;
                        time[i] = time[i] + 1;
                    }
                    else
                    {
                        count[i] = 0;
                        Rectify(i, n, sD);
                    }
                    break;

                /* Going from vR km/hr to vM km/hr */
                case 7:
                    if (count[i] != (int)((7200 * sD) / (vM + vR)))
                    {
                        velocity[i] = velocity[i] + ((double)(vM - vR) * (vR + vM) / (7200 * sD));
                        s[i] = (velocity[i] / 3600) + ((double)(vM - vR) * (vR + vM) / (sD * 7200 * 7200));
                        distance[i] = distance[i] + s[i];
                        count[i]++;
                        time[i] = time[i] + 1;

                        if (count[i] == ((int)((7200 * sD) / (vM + vR)) - 1))
                        {
                            velocity[i] = vM;
                            distance[i] = sD * (sg[i] + 1);
                        }
                    }
                    else
                    {
                        count[i] = 0;
                        Rectify(i, n, sD);
                    }
                    break;

                /* Going from vM km/hr to 0 km/hr , waiting for 1 minute and the going with vR km/hr */
                case 8:
                    if ((count[i] != (int)((7200 * d[i]) / vM)) || (fs[i] == 1))
                    {
                        /* Speed decreasing from vM km/hr to 0 km/hr in the distance 'p' */
                        if (fs[i] == 0)
                        {
                            velocity[i] = velocity[i] - ((double)(vM * vM) / (7200 * d[i]));
                            s[i] = (velocity[i] / 3600) - ((double)(vM * vM) / (d[i] * 7200 * 7200));
                            distance[i] = distance[i] + s[i];
                            count[i]++;
                            time[i] = time[i] + 1;
                        }

                        if (count[i] == ((int)((7200 * d[i]) / vM) - 1))
                        {
                            fs[i] = 1;
                            sp[i] = 0;
                            s[i] = distance[i];
                            count[i]++;
                            time[i] = time[i] - 1;
                        }

                        if ((fs[i] == 1) && (count[i] >= (int)((7200 * d[i]) / vM)))
                        {
                            /* Waiting for 1 minute */
                            if (sp[i] != 60)
                            {
                                sp[i]++;
                                velocity[i] = 0;
                                count[i] = count[i] + 1;
                                time[i] = time[i] + 1;

                                if (sp[i] == 60)
                                {
                                    time[i] = time[i] - 1;
                                }
                            }

                            /* Going with vR km/hr till the next signal */
                            if (sp[i] == 60)
                            {
                                if (timer[i] != (int)((3600 / vR) * ((sD * (sg[i] + 1)) - s[i])))
                                {
                                    velocity[i] = vR;
                                    distance[i] = distance[i] + (velocity[i] / (double)3600);
                                    time[i] = time[i] + 1;
                                    timer[i]++;
                                }
                            }
                        }
                    }
                    else
                    {
                        count[i] = 0;
                        Rectify(i, n, sD);
                    }
                    break;

                /* Going from vR km/hr to 0 km/hr , waiting for 1 minute and the going with vR km/hr */
                case 9:
                    if ((count[i] != (int)((7200 * d[i]) / vR)) || (fs[i] == 1))
                    {
                        /* Speed decreasing from vR km/hr to 0 km/hr in the distance 'p' */
                        if (fs[i] == 0)
                        {
                            velocity[i] = velocity[i] - ((double)(vR * vR) / (7200 * d[i]));
                            s[i] = (velocity[i] / 3600) - ((double)(vR * vR) / (d[i] * 7200 * 7200));
                            distance[i] = distance[i] + s[i];
                            count[i]++;
                            time[i] = time[i] + 1;
                        }

                        if (count[i] == ((int)((7200 * d[i]) / vR) - 1))
                        {
                            fs[i] = 1;
                            sp[i] = 0;
                            s[i] = distance[i];
                            count[i]++;
                            time[i] = time[i] - 1;
                        }

                        if ((fs[i] == 1) && (count[i] >= (int)((7200 * d[i]) / vR)))
                        {
                            /* Waiting for 1 minute */
                            if (sp[i] != 60)
                            {
                                sp[i]++;
                                velocity[i] = 0;
                                count[i] = count[i] + 1;
                                time[i] = time[i] + 1;

                                if (sp[i] == 60)
                                {
                                    time[i] = time[i] - 1;
                                }
                            }

                            /* Going with vR km/hr till the next signal */
                            if (sp[i] == 60)
                            {
                                if (timer[i] != (int)((3600 / vR) * ((sD * (sg[i] + 1)) - s[i])))
                                {
                                    velocity[i] = vR;
                                    distance[i] = distance[i] + (velocity[i] / (double)3600);
                                    time[i] = time[i] + 1;
                                    timer[i]++;
                                }
                            }
                        }
                    }
                    else
                    {
                        count[i] = 0;
                        Rectify(i, n, sD);
                    }
                    break;
            }
        }

        /* If the train stops unexpectedly */
        public void Rectify(int i, int n, double sD)
        {
            for (int j = 1; j <= n; j++)
            {
                diff[j - 1] = (int)(Math.Floor(distance[i] - (sD * j)));
            }

            for (int j = 0; j < n; j++)
            {
                if (diff[j] == 0 || diff[j] == (-1))
                {
                    distance[i] = (sD * (j + 1));
                }
            }
        }

        /* Checks whether the simulation finishes or all the trains cover the stretch */
        public bool Finished(int m, int n, double sD)
        {
            for (int i = 0; i < m; i++)
            {
                if (distance[i] < (sD * (n + 1)))
                {
                    return false;
                }
            }

            return true;
        }

        /* Checks whether any train crashes with the train at the front */
        public bool Crashed(int m, int n, Form1 form1, double sD)
        {
            for (int i = 1; i < m; i++)
            {
                if ((distance[i] >= distance[i - 1]) && (distance[i] > 0) && (distance[i] < (sD * (n + 1))))
                {
                    UpdateSignalConsole(form1, "\r\n\r\nThere will be a crash between trains due to the combination of these inputs or due to the gap between trains being very small.\r\nPlease try another input.\r\n");
                    return true;
                }
            }

            return false;
        }

        /* To use the Form TextBox Control */
        private void UpdateSignalConsole(Form1 form1, string data)
        {
            form1.SConsole = data;
        }

        /* To use the Form TextBox Control */
        private void UpdateInfoConsole(Form1 form1, string data)
        {
            form1.IConsole = data;
        }
    }
}
