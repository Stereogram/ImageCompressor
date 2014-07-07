using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ImageCompressor
{
    //colour class with y/cr/cb arrays
    public class YCrCb
    {
        public int[][] y { get; set; }
        public int[][] cr { get; set; }
        public int[][] cb { get; set; }
        public int[][] scr { get; set; }//subsampled version
        public int[][] scb { get; set; }

        public YCrCb(int[][] y,int[][] cr,int[][] cb)
        {
            this.y = y;
            this.cr = cr;
            this.cb = cb;
            this.scr = subsample(cr);
            this.scb = subsample(cb);
            
        }
        public YCrCb() { }

        private int[][] subsample(int[][] a)
        {
            int[][] b = new int[(int)Math.Ceiling(a.Length / 2.0f)][];
            for (int y = 0; y < b.Length; y++)
            {
                b[y] = new int[(int)Math.Ceiling(a[y].Length / 2.0f)];
                for (int x = 0; x < b[y].Length; x++)
                {
                    b[y][x] = a[y * 2][x * 2];
                }
            }
            return b;
        }

    }
}
