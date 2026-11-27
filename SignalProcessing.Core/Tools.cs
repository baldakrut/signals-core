using System;
using System.Collections.Generic;
using System.Linq;

namespace SignalProcessing.Core
{
    public class Tools
    {

        public static SingleChannelSignal Line(float len, float frequency, float k, float b)
        {
            int count = (int)(len * frequency + 0.5);

            float[] x = new float[count];
            float[] y = new float[count];
            float diff = 1 / frequency;
            for (int i = 0; i < count; i++)
            {

                x[i] = i * diff;
                y[i] = k * x[i] + b;

            }

            return new SingleChannelSignal(y, frequency);
        }
        public static SingleChannelSignal Sin(float len, float frequency, float period, float phase, float scale)
        {
            int count = (int)(len * frequency + 0.5);
            float[] x = new float[count];
            float[] y = new float[count];
            float diff = 1 / frequency;
            for (int i = 0; i < count; i++)
            {

                x[i] = i * diff;
                y[i] = scale * (float)Math.Sin((x[i] - phase) * 2 * Math.PI / period);


            }

            return new SingleChannelSignal(y, frequency);
        }
        public static SingleChannelSignal Noise(float len, float frequency, float amplitude)
        {
            Random random = new Random();
            int count = (int)(len * frequency + 0.5);
            float[] x = new float[count];
            float[] y = new float[count];
            float diff = 1 / frequency;
            for (int i = 0; i < count; i++)
            {

                x[i] = i;
                y[i] = (float)random.NextDouble() * 2 * amplitude - amplitude;


            }

            return new SingleChannelSignal(y, frequency);
        }
    }
}