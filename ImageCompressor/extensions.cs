using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ImageCompressor
{
    //method extensions.
    public static class extensions
    {
        //generic round which will round .5 up to 1 unlike Math.Round which will round .5 down to 0. o.O
        public static int Round<T>(this T a) where T: struct
        {
            return (int)(Convert.ToDouble(a) + 0.5);
        }
        //generic direct array multiplication
        public static int[][] Multiply<T,U>(this T[][] a, U[][] b) where T: struct
        {
            int[][] t = new int[a.Length][];
            for (int i = 0; i < a.Length; i++)
            {
                t[i] = new int[a[i].Length];
                for (int j = 0; j < a[i].Length; j++)
                {
                    t[i][j] = (int)(Convert.ToDouble(a[i][j]) * Convert.ToDouble(b[i][j]));
                }
            }
            return t;
        }

        public static int[][] Divide<T,U>(this T[][] a, U[][] b) where T: struct
                                                                 where U: struct
        {
            int[][] t = new int[a.Length][];
            for (int i = 0; i < a.Length; i++)
            {
                t[i] = new int[a[i].Length];
                for (int j = 0; j < a[i].Length; j++)
                {
                    t[i][j] = (int)(Convert.ToDouble(a[i][j]) / Convert.ToDouble(b[i][j]));
                }
            }
            return t;
        }
        //converts float array to int array
        public static int[][] FloatToInt(this float[][] a)
        {
            int[][] t = new int[a.Length][];
            for (int i = 0; i < a.Length; i++)
            {
                t[i] = new int[a[i].Length];
                for (int j = 0; j < a[i].Length; j++)
                {
                    t[i][j] = Convert.ToInt32(a[i][j]);
                }
            }
            return t;
        }
        //a = chunk, b = whole image, calculates difference between two arrays with an offset
        public static int[][] Difference(int[][] a, int[][] b, System.Drawing.Point c)
        {
            int[][] t = new int[8][];
            for (int i = 0; i < 8; i++)
            {
                t[i] = new int[8];
                for (int j = 0; j < 8; j++)
                {
                    if (c.X + j >= b[0].Length || c.Y + i >= b.Length)
                    {
                        continue;
                    }
                    t[i][j] = b[i + c.Y][j + c.X] - a[i][j];
                }
            }
            return t;
        }
        //a = chunk, b = whole image.
        public static int[][] DAddition(int[][] a, int[][] b, System.Drawing.Point c)
        {
            int[][] t = new int[8][];
            for (int i = 0; i < 8; i++)
            {
                t[i] = new int[8];
                for (int j = 0; j < 8; j++)
                {
                    if (c.X + j >= b[0].Length || c.Y + i >= b.Length)
                    {
                        continue;
                    }
                    t[i][j] = a[i][j] + b[i + c.Y][j + c.X];
                }
            }
            return t;
        }

        public static float[][] IntToFloat(this int[][] a)
        {
            float[][] t = new float[a.Length][];
            for (int i = 0; i < a.Length; i++)
            {
                t[i] = new float[a[i].Length];
                for (int j = 0; j < a[i].Length; j++)
                {
                    t[i][j] = a[i][j];
                }
            }
            return t;
        }
        //generic print 2d array
        public static void Print<T>(this T[][] a)
        {
            foreach (T[] b in a)
            {
                foreach (T c in b)
                {
                    Console.Write("{0} ", c);
                }
                Console.WriteLine();
            }
        }
        //generic print 1d array
        public static void Print<T>(this T[] a)
        {
            foreach (T b in a)
            {
                Console.Write("{0} ", b);
            }
            Console.WriteLine();
        }
        //point multiplication
        public static System.Drawing.Point M(this System.Drawing.Point a, int b)
        {
            return new System.Drawing.Point(a.X * b, a.Y * b);
        }

    }
}
