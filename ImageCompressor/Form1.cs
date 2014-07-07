using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;

namespace ImageCompressor
{
    public partial class Form1 : Form
    {
        int p = 15;
        private List<Pair<Point,Point>> mv;

        private Bitmap img2;
        private Bitmap timg;

        private MemoryStream stream = new MemoryStream();

        private int[][] cy;
        private int[][] cr;
        private int[][] cb;

        int[][] test;
        int[][] test2;

        private Color[][] rgb;

        //reverse zigzag constant array to tell where each element goes.
        private static int[][] rz =
        {
            new int[] { 0 , 1 , 5 , 6 , 14, 15, 27, 28 },
            new int[] { 2 , 4 , 7 , 13, 16, 26, 29, 42 },
            new int[] { 3 , 8 , 12, 17, 25, 30, 41, 43 },
            new int[] { 9 , 11, 18, 24, 31, 40, 44, 53 },
            new int[] { 10, 19, 23, 32, 39, 45, 52, 54 },
            new int[] { 20, 22, 33, 38, 46, 51, 55, 60 },
            new int[] { 21, 34, 37, 47, 50, 56, 59, 61 },
            new int[] { 35, 36, 48, 49, 57, 58, 62, 63 }
        };
        //zigzag constant array to tell where each element goes.
        private static int[] z = { 0, 1, 8, 16, 9, 2, 3, 10, 17, 24, 32, 25, 18, 11, 4, 5, 12, 19, 26, 33, 40, 48, 41, 34, 27, 20, 13, 6, 7, 14, 21, 28, 35, 42, 49, 56, 57, 50, 43, 36, 29, 22, 15, 23, 30, 37, 44, 51, 58, 59, 52, 45, 38, 31, 39, 46, 53, 60, 61, 54, 47, 55, 62, 63 };

        //Quantization tables...
        private static int[][] luma = 
        {
            new int[] { 16, 11, 10, 16, 24,  40,  51,  51 },
            new int[] { 12, 12, 14, 19, 26,  58,  60,  55 },
            new int[] { 14, 13, 16, 24, 40,  57,  69,  56 },
            new int[] { 14, 17, 22, 29, 51,  87,  80,  62 },
            new int[] { 18, 22, 37, 56, 68,  109, 103, 77 },
            new int[] { 24, 35, 55, 64, 81,  104, 113, 92 },
            new int[] { 49, 64, 78, 87, 103, 121, 120, 101},
            new int[] { 72, 92, 95, 98, 112, 100, 103, 99 }
        };

        private static float[][] chroma = 
        {
            new float[] { 17, 18, 24, 47, 99, 99, 99, 99 },
            new float[] { 18, 21, 26, 66, 99, 99, 99, 99 },
            new float[] { 24, 26, 56, 99, 99, 99, 99, 99 },
            new float[] { 47, 66, 99, 99, 99, 99, 99, 99 },
            new float[] { 99, 99, 99, 99, 99, 99, 99, 99 },
            new float[] { 99, 99, 99, 99, 99, 99, 99, 99 },
            new float[] { 99, 99, 99, 99, 99, 99, 99, 99 },
            new float[] { 99, 99, 99, 99, 99, 99, 99, 99 }
        };

        public Form1()
        {
            InitializeComponent();
            this.ResizeRedraw = true;
            this.DoubleBuffered = true;
            pb.Visible = false;
            stream = new MemoryStream();
        }
        //menu options...
        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void openToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenFileDialog open = new OpenFileDialog();
            if (open.ShowDialog() == DialogResult.OK)
            {
                timg = new Bitmap(open.FileName);
                this.Invalidate();
            }
            
            this.Invalidate();
        }

        private void openSecondFrameToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenFileDialog open = new OpenFileDialog();
            if (open.ShowDialog() == DialogResult.OK)
            {
                img2 = new Bitmap(open.FileName);
            }

            this.Invalidate();
        }

        private void openFileToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenFileDialog open = new OpenFileDialog();
            open.Filter = "Stuff|*.stuff";
            open.Title = "Open some stuff";
            if (open.ShowDialog() == DialogResult.OK)
            {
                FileStream fs = new FileStream(open.FileName, FileMode.Open);
                stream = new MemoryStream();
                fs.CopyTo(stream);
                stream.Position = 0;
                timg = uncompress(stream);
                this.Invalidate();
            }
        }

        private void saveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SaveFileDialog save = new SaveFileDialog();
            save.Filter = "Stuff|*.stuff";
            save.Title = "Save some stuff";
            save.ShowDialog();

            // If the file name is not an empty string open it for saving.
            if (save.FileName != "")
            {
                FileStream fs = (FileStream)save.OpenFile();
                stream.Position = 0;
                stream.WriteTo(fs);
                stream.Flush();
                fs.Close();
            }
        }

        private void openMotionStuffToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenFileDialog open = new OpenFileDialog();
            open.Filter = "MotionStuff|*.motionstuff";
            open.Title = "Open some motion stuff";
            if (open.ShowDialog() == DialogResult.OK)
            {
                FileStream fs = new FileStream(open.FileName, FileMode.Open);
                stream = new MemoryStream();
                fs.CopyTo(stream);
                stream.Position = 0;
                timg = uncompress(stream);
                img2 = uncompress_video(stream);
                this.Invalidate();
            }
        }

        private void saveMotionStuffToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SaveFileDialog save = new SaveFileDialog();
            save.Filter = "MotionStuff|*.motionstuff";
            save.Title = "Save some motion stuff";
            save.ShowDialog();

            // If the file name is not an empty string open it for saving.
            if (save.FileName != "")
            {
                FileStream fs = (FileStream)save.OpenFile();//MOTION STUFF HERE
                stream.Position = 0;
                stream.WriteTo(fs);
                stream.Flush();
                fs.Close();
            }
        }

        private void compressImgToolStripMenuItem_Click(object sender, EventArgs e)
        {
            timg = compress(timg);
        }

        private void compressvideoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            compress_video(timg, img2);
        }

        private void Form1_Paint(object sender, PaintEventArgs e)
        {
            e.Graphics.FillRectangle(new SolidBrush(Color.White), this.ClientRectangle);
            if (timg != null)
            {
                e.Graphics.DrawImage(timg, 25, 25);
            }
            if (img2 != null)
            {
                e.Graphics.DrawImage(img2, timg.Width+50, 25);
            }
            if (mv != null)
            {
                foreach(Pair<Point,Point> p in mv)
                {
                    e.Graphics.DrawLine(Pens.HotPink, p.First.X+25, p.First.Y + 25, p.Second.X+25, p.Second.Y+25);
                    e.Graphics.FillRectangle(new SolidBrush(Color.LightYellow), p.Second.X+25, p.Second.Y+25, 2, 2);
                }
            }
        }
        //should rewrite using YCrCb class, does what it says, converts an image to ycrcb
        private void convert_ycrcb(Bitmap b)
        {
            cy = new int[b.Height][];
            cb = new int[b.Height][];
            cr = new int[b.Height][];
            Color t;
            for (int y = 0; y < b.Height; y++)
            {
                cy[y] = new int[b.Width];
                cb[y] = new int[b.Width];
                cr[y] = new int[b.Width];
                for (int x = 0; x < b.Width; x++)
                {
                    t = b.GetPixel(x,y);
                    cy[y][x] = (int)(0.299f * (t.R) + 0.587f * (t.G) + 0.114f * (t.B));
                    cb[y][x] = (int)(128 - 0.168736f * (t.R) - 0.331264f * (t.G) + 0.5f * (t.B));
                    cr[y][x] = (int)(128 + 0.5f * (t.R) - 0.418688f * (t.G) - 0.081312f * (t.B));
                }
            }
        }
        //converts ycrcb to rgb
        private void convert_rgb(int h, int w)
        {
            rgb = new Color[h][];
            float r, b, g;
            for (int y = 0; y < h; y++)
            {
                rgb[y] = new Color[w];
                for (int x = 0; x < w; x++)
                {
                    r = cy[y][x] + 1.402f *   (cr[y][x] - 128.0f);
                    g = cy[y][x] - 0.34414f * (cb[y][x] - 128f) - 0.71414f * (cr[y][x]-128);
                    b = cy[y][x] + 1.772f *   (cb[y][x] - 128.0f);
                    r = r > 255 ? 255 : r < 0 ? 0 : r;
                    g = g > 255 ? 255 : g < 0 ? 0 : g;
                    b = b > 255 ? 255 : b < 0 ? 0 : b;
                    rgb[y][x] = Color.FromArgb((int)r, (int)g, (int)b);
                }
            }
        }
        //subsamples an array
        private int[][] subsample(int[][] a)
        {
            int[][] b = new int[(int)Math.Ceiling(a.Length/2.0f)][];
            for (int y = 0; y < b.Length; y++)
            {
                b[y] = new int[(int)Math.Ceiling(a[y].Length/2.0f)];
                for (int x = 0; x < b[y].Length; x++)
                {
                    b[y][x] = a[y*2][x*2];
                }
            }
            return b;
        }
        //doubles up an array
        private int[][] reverse_subsample(int[][] b)
        {
            int[][] a = new int[b.Length*2][];
            int x1 = 0, y1 = 0;
            for (int y = 0; y < b.Length; y++)
            {
                int[] row = new int[b[y].Length*2];
                x1 = 0;
                for (int x = 0; x < b[y].Length; x++)
                {
                    row[x1] = b[y][x];
                    x1++;
                    row[x1] = b[y][x];
                    x1++;
                }
                a[y1] = row;
                y1++;
                a[y1] = row;
                y1++;
            }
            return a;
        }
        //breaks an image into n x n chunks, a chunk is just a 2d int array
        private Chunk[][] chunkify(int[][] data, int n)
        {
            Chunk[][] t = new Chunk[(int)Math.Ceiling(data.Length/8.0f)][];
            Chunk[] q;
            int[][] sqr = new int[n][];
            int[] line;
            for (int y = 0, l = 0; y < data.Length; y+=n, l++)
            {
                q = new Chunk[(int)Math.Ceiling(data[y].Length / 8.0f)];
                for (int x = 0, k = 0; x < data[y].Length; x+=n, k++)
                {
                    sqr = new int[n][];
                    for (int i = 0; i < n; i++)
                    {
                        line = new int[n];
                        for (int j = 0; j < n; j++)
                        {
                            if (x + j >= data[i].Length || y + i >= data.Length)
                            {
                                break;
                            }
                            line[j] = data[y+i][x+j];
                        }
                        sqr[i] = line;
                    }
                    q[k] = new Chunk(sqr);
                }
                t[l] = q;
            }
            return t;
        }
        //zigzags a 2d array into a 1d array
        private int[] zigzag(int[][] a)
        {
            int[] t = new int[a.Length * a[0].Length];
            for (int i = 0; i < z.Length; i++)
            {
                t[i] = a[z[i]/8][z[i]%8];
            }
            return t;
        }
        //makes a 1d array into a 2d array zigzag style
        private int[][] reverse_zigzag(int[] a)
        {
            int[][] t = new int[8][];
            for (int i = 0; i < t.Length; i++)
            {
                t[i] = new int[8];
                for (int j = 0; j < t[i].Length; j++)
                {
                    t[i][j] = a[rz[i][j]];
                }
            }
            return t;
        }
        //actually a much simpler rle (technically far more inefficient, its possible to generate an array longer then original, although in our case that won't happen)
        private int[] rle(int[] a)
        {
            List<int> t = new List<int>();
            int cur = a[0], c = 0;
            for (int i = 0; i < a.Length; i++)
            {
                if (a[i] != cur)
                {
                    t.Add(c);
                    t.Add(cur);
                    cur = a[i];
                    c = 0;
                }
                c++;
            }
            t.Add(c);
            t.Add(cur);
            return t.ToArray();
        }
        //undoes rle 
        private int[] reverse_rle(int[] a)
        {
            int[] t = new int[64];//8 by 8
            int c, cur, loc = 0;
            for (int i = 0; i < a.Length; i++)
            {
                c = a[i++];
                cur = a[i];
                for (int j = loc; j < c+loc; j++)
                {
                    t[j] = cur;
                }
                loc += c;
            }
            return t;
        }
        //descrete cosine transform, orginize data into how much it 'changes'
        private float[][] DCT(int[][] a)
        {
            int N = a.Length;
            float[][] t = new float[N][];
            for (int u = 0; u < N; u++)
            {
                t[u] = new float[N];
                for (int v = 0; v < N; v++)
                {
                    t[u][v] = (float)((0.25f) * c(u) * c(v)* f(a, u, v));
                }
            }
            return t;
        }
        //for dct
        private double f(int[][] a, int u, int v)
        {
            double t = 0;
            int N  = a.Length;
            for (int i = 0; i < a.Length; i++)
            {
                for (int j = 0; j < a[i].Length; j++)
                {
                    t += a[i][j]*(Math.Cos((((2*j)+1)*u*Math.PI)/(2*N))*Math.Cos((((2*i)+1)*v*Math.PI)/(2*N)));
                }
            }
            return t;
        }
        //for dct
        private double c(int i)
        {
            return i == 0 ? 1 / Math.Sqrt(2) : 1;
        }
        //inverse dct
        private int[][] reverse_DCT(int[][] a)
        {
            int N = a.Length;
            int[][] t = new int[N][];
            for (int y = 0; y < N; y++)
            {
                t[y] = new int[N];
                for (int x = 0; x < N; x++)
                {
                    t[y][x] = (int)((0.25f) * reverse_f(a, y, x));
                }
            }
            return t;
        }
        //for idct
        private double reverse_f(int[][] a, int y, int x)
        {
            double t = 0;
            int N = a.Length;
            for (int u = 0; u < a.Length; u++)
            {
                for (int v = 0; v < a[u].Length; v++)
                {
                    t += c(u) * c(v) * a[u][v] * (Math.Cos((((2 * x) + 1) * u * Math.PI) / (2 * N)) * Math.Cos((((2 * y) + 1) * v * Math.PI) / (2 * N)));
                }
            }
            return t;
        }
        //reconstructs image from the 8x8 chunks
        private int[][] reverse_chunkify(Chunk[][] a)
        {
            int[][] t = new int[a.Length*8][];
            int x = 0, y = 0;
            foreach (Chunk[] line in a)
            {
                for (int i = 0; i < 8; i++)//make 8 new lines
                {
                    t[i + y] = new int[a[0].Length * 8];
                }
                foreach (Chunk chunk in line)
                {
                    for (int i = 0; i < chunk.chunk.Length; i++)
                    {
                        for (int j = 0; j < chunk.chunk[0].Length; j++)
                        {
                            t[y + i][x + j] = chunk.chunk[i][j];
                        }
                    }
                    x+=8;
                }
                x=0;
                y+=8;
            }
            return t;
        }
        //jpg compression(ish)
        private Bitmap compress(Bitmap b)
        {
            BinaryWriter o = new BinaryWriter(stream);//first we put everything into memory(user can hit save to write to a file if they want later)
            convert_ycrcb(b);

            Chunk[][] tchunkr = chunkify(subsample(cr), 8);//subsample cr/cb to save 50% for something hardly noticable(we notice luminance much more then colour) 
            Chunk[][] tchunkb = chunkify(subsample(cb), 8);
            Chunk[][] tchunky = chunkify(cy, 8);//4times cr or cb(2times width & 2times height)

            float[][] dctr;
            float[][] dctb;
            float[][] dcty;

            int[][] test;

            int[] finalr;
            int[] finalb;
            int[] finaly;

            byte[] result;

            pb.Visible = true;
            pb.Maximum = ((tchunky.Length * tchunky[0].Length) + (tchunkr.Length * tchunkr[0].Length) + (tchunkb.Length * tchunkb[0].Length));//retarded progress bar stuff
            pb.Value = 1;
            o.Write(bitconvert(timg.Width, false));//our 'header' is just the original img's height and width
            o.Write(bitconvert(timg.Height, false));

            for (int i = 0; i < tchunky.Length; i++)//for y...
            {
                for (int j = 0; j < tchunky[i].Length; j++)
                {
                    dcty = DCT(tchunky[i][j].chunk);

                    test = dcty.Divide(luma);
                    finaly = zigzag(test);
                    finaly = rle(finaly);
                    result = convert(finaly);//convert will add 128 to avoid negative values(using unsigned bytes)
                    o.Write(result);
                    o.Write(Convert.ToByte('\0'));//spacer between each 8x8 block
                    pb.PerformStep();
                }
            }
            
            for (int i = 0; i < tchunkr.Length; i++)//for cr
            {
                for (int j = 0; j < tchunkr[i].Length; j++)
                {
                    dctr = DCT(tchunkr[i][j].chunk);

                    test = dctr.Divide(chroma);
                    finalr = zigzag(test);
                    finalr = rle(finalr);
                    result = convert(finalr);
                    o.Write(result);
                    o.Write(Convert.ToByte('\0'));//spacer between each 8x8 block
                    pb.PerformStep();
                }
            }
            for (int i = 0; i < tchunkb.Length; i++)//for cb...
            {
                for (int j = 0; j < tchunkb[i].Length; j++)
                {
                    dctb = DCT(tchunkb[i][j].chunk);

                    test = dctb.Divide(chroma);
                    finalb = zigzag(test);
                    finalb = rle(finalb);
                    result = convert(finalb);
                    o.Write(result);
                    o.Write(Convert.ToByte('\0'));//spacer between each 8x8 block
                    pb.PerformStep();
                }
            }
            o.Flush();
            stream.Position = 0;
            pb.Visible = false;
            return uncompress(stream);
        }
        //opens the stuff file for veiwing
        private Bitmap uncompress(MemoryStream s)
        {
            Bitmap b;
            byte temp;
            List<int> rle = new List<int>();
            BinaryReader br = new BinaryReader(s);
            int w = br.ReadInt32();//read header first
            int h = br.ReadInt32();
            int blocks = (int)Math.Ceiling((w * h) / 8.0f);
            int[] t;
            int[][] block;
            Chunk[][] chunky = new Chunk[(int)Math.Ceiling(h/8.0f)][];
            Chunk[][] chunkr = new Chunk[(int)Math.Ceiling((h/8.0f)/2)][];
            Chunk[][] chunkb = new Chunk[(int)Math.Ceiling((h/8.0f)/2)][];
            pb.Visible = true;
            pb.Maximum = (int)((chunky.Length * (w / 8.0f)) + (chunkr.Length * (w / 8.0f)) + (chunkb.Length * (w / 8.0f)));
            pb.Value = 1;

            b = new Bitmap(w, h);//our image we will construct

            for (int i = 0; i < chunky.Length; i++)
            {
                chunky[i] = new Chunk[(int)Math.Ceiling(w / 8.0f)];
                for (int j = 0; j < chunky[i].Length; j++)
                {
                    while ( (temp = br.ReadByte()) != 0x00)//continue reading untill we hit the spacer
                    {
                        rle.Add(Convert.ToInt32(temp-128));//add to list(subtract 128 to reverse our conversion during compression)
                    }
                    t = rle.ToArray();//convert back to array to process
                    t = reverse_rle(t);//reverse order all steps
                    block = reverse_zigzag(t);
                    chunky[i][j] = new Chunk(reverse_DCT(block.Multiply(luma)));
                    rle.Clear();
                    pb.PerformStep();
                }
            }
            rle.Clear();

            for (int i = 0; i < chunkr.Length; i++)
            {
                chunkr[i] = new Chunk[(int)Math.Ceiling((w/8.0f)/2)];
                for (int j = 0; j < chunkr[i].Length; j++)
                {
                    while ((temp = br.ReadByte()) != 0x00)
                    {
                        rle.Add(Convert.ToInt32(temp - 128));
                    }
                    t = rle.ToArray();
                    t = reverse_rle(t);
                    block = reverse_zigzag(t);
                    chunkr[i][j] = new Chunk(reverse_DCT(block.Multiply(chroma)));
                    rle.Clear();
                    pb.PerformStep();
                }
            }
            rle.Clear();

            for (int i = 0; i < chunkb.Length; i++)
            {
                chunkb[i] = new Chunk[(int)Math.Ceiling((w/8.0f)/2)];
                for (int j = 0; j < chunkb[i].Length; j++)
                {
                    while ((temp = br.ReadByte()) != 0x00)
                    {
                        rle.Add(Convert.ToInt32(temp - 128));
                    }
                    t = rle.ToArray();
                    t = reverse_rle(t);
                    block = reverse_zigzag(t);
                    chunkb[i][j] = new Chunk(reverse_DCT(block.Multiply(chroma)));
                    rle.Clear();
                    pb.PerformStep();
                }
            }
            cy = reverse_chunkify(chunky);
            cr = reverse_chunkify(chunkr);
            cb = reverse_chunkify(chunkb);

            test = cr;//keep subsampled versions for video if needed
            test2 = cb;

            cr = reverse_subsample(cr);
            cb = reverse_subsample(cb);

            convert_rgb(h,w);
            for (int y = 0; y < h; y++)
            {
                for (int x = 0; x < w; x++)
                {
                    b.SetPixel(x, y, rgb[y][x]);
                }
            }
            pb.Visible = false;
            return b;
        }
        //converts an int array to a byte array and adds 128 as we're using unsigned bytes
        private byte[] convert(int[] a)
        {
            byte[] b = new byte[a.Length];
            for (int i = 0; i < a.Length; i++ )
            {
                b[i] = Convert.ToByte(a[i]+128);
            }
            return b;
        }
        //actually SAD algorithm but I don't believe in sadness. returns Sum Absolute Difference
        private double glad(int[][] C, int[][] R, int i, int j, int y, int x)
        {
            double t = 0;
            int N = 8;
            for (int k = 0; k < N; k++)
            {
                for (int l = 0; l < N; l++)
                {
                    if (y + i + k >= C.Length || x + j + l >= C[0].Length)//range check
                    {
                        continue;
                    }
                    t += Math.Abs(C[y + k][x + l] - R[y + i + k][x + j + l]);
                }
            }
            return t;
        }
        //calculates a motion vector
        private Point motion_vector(int[][] C, int[][] R, int y, int x, int p)
        {
            Point t = new Point(0, 0);
            double cur, min = glad(C, R, 0, 0, y, x);
            for (int i = (-p-4); i < (p-4); i++)
            {
                for (int j = (-p-4); j < (p-4); j++)
                {
                    if (i + y < 0 || y + 8 + p >= C.Length || j + x < 0 || x + 8 + p >= C[0].Length)//more range checking...
                    {
                        continue;
                    }
                    cur = glad(C, R, i, j, y, x);
                    if ((int)cur < (int)min)//motion vector is lowest glad
                    {
                        min = cur;
                        t = new Point(j, i);
                    }
                }
            }
            return t;
        }
        //mpeg compression(ish), one,two our current frames
        private void compress_video(Bitmap one, Bitmap two)
        {
            Point temp;//motion vector
            mv = new List<Pair<Point,Point>>();//mvs
            two = compress(two);//compress images(and uncompress)
            stream = new MemoryStream();
            one = compress(one);
            BinaryWriter o = new BinaryWriter(stream);//we're writting to the stream after the jpeg stuff
            Chunk[][] onechunk;
            
            convert_ycrcb(one);
            YCrCb onecolour = new YCrCb(cy,cr,cb);
            convert_ycrcb(two);
            YCrCb twocolour = new YCrCb(cy,cr,cb);
            int[][] block;
            float[][] dct;
            int[] final;
            byte[] result;
            onechunk = chunkify(onecolour.y, 8);
            for (int i = 0; i < (int)Math.Ceiling(one.Height / 8.0f); i++)
            {
                for (int j = 0; j < (int)Math.Ceiling(one.Width / 8.0f); j++)
                {
                    temp = motion_vector(onecolour.y, twocolour.y, (i*8), (j*8), p);//get the motion vector
                    block = extensions.Difference(onechunk[i][j], twocolour.y, new Point((j*8),(i*8)));//find the difference
                    dct = DCT(block);//normal steps...
                    block = dct.Divide(luma);
                    final = zigzag(block);
                    final = rle(final);
                    result = convert(final);
                    o.Write(result);
                    o.Write(Convert.ToByte('\0'));//spacer
                    o.Write(Convert.ToByte(temp.X+128));//write the mv to stream
                    o.Write(Convert.ToByte(temp.Y+128));
                    //mv.Add(new Pair<Point,Point>(new Point(j*8, i*8), new Point(temp.X + (j*8), temp.Y + (i*8))));  //add motion vectors to list to display
                }
            }

            onechunk = chunkify(onecolour.scr, 8);
            for (int i = 0; i < (int)Math.Ceiling(one.Height / 8.0f/2); i++)
            {
                for (int j = 0; j < (int)Math.Ceiling(one.Width / 8.0f/2); j++)
                {
                    temp = motion_vector(onecolour.scr, twocolour.scr, (i * 8), (j * 8), p);
                    block = extensions.Difference(onechunk[i][j], twocolour.scr, new Point( (j * 8),  (i * 8)));
                    dct = DCT(block);
                    block = dct.Divide(chroma);
                    final = zigzag(block);
                    final = rle(final);
                    result = convert(final);
                    o.Write(result);
                    o.Write(Convert.ToByte('\0'));
                    o.Write(Convert.ToByte(temp.X+128));
                    o.Write(Convert.ToByte(temp.Y+128));
                    //mv.Add(new Pair<Point,Point>(new Point(j*8, i*8), new Point(temp.X + (j*8), temp.Y + (i*8))));  //add motion vectors to list to display
                }
            }

            onechunk = chunkify(onecolour.scb, 8);
            for (int i = 0; i < (int)Math.Ceiling(one.Height / 8.0f/2); i++)
            {
                for (int j = 0; j < (int)Math.Ceiling(one.Width / 8.0f/2); j++)
                {
                    temp = motion_vector(onecolour.scb, twocolour.scb, (i * 8), (j * 8), p);
                    block = extensions.Difference(onechunk[i][j], twocolour.scb, new Point( (j * 8),  (i * 8)));
                    dct = DCT(block);
                    block = dct.Divide(chroma);
                    final = zigzag(block);
                    final = rle(final);
                    result = convert(final);
                    o.Write(result);
                    o.Write(Convert.ToByte('\0'));
                    //mv.Add(new Pair<Point,Point>(new Point(j*8, i*8), new Point(temp.X + (j*8), temp.Y + (i*8))));  //add motion vectors to list to display
                }
            }

            this.Invalidate();
        }

        //uncompress video
        private Bitmap uncompress_video(MemoryStream s)
        {
            int w = timg.Width;//first image's width
            int h = timg.Height;
            Bitmap b = new Bitmap(w,h);
            Point mv = new Point();
            Point[][] mvs = new Point[(int)Math.Ceiling((h / 8.0f) / 2)][];
            byte temp;
            List<int> rle = new List<int>();
            BinaryReader br = new BinaryReader(s);

            cr = test;
            cb = test2;

            int[] t;
            int[][] block;
            Chunk[][] chunky = new Chunk[(int)Math.Ceiling(h / 8.0f)][];
            Chunk[][] chunkr = new Chunk[(int)Math.Ceiling((h / 8.0f) / 2)][];
            Chunk[][] chunkb = new Chunk[(int)Math.Ceiling((h / 8.0f) / 2)][];

            for (int i = 0; i < chunky.Length; i++)// same steps as jpg uncompression...
            {
                chunky[i] = new Chunk[(int)Math.Ceiling(w / 8.0f)];
                for (int j = 0; j < chunky[i].Length; j++)
                {
                    while ((temp = br.ReadByte()) != 0x00)
                    {
                        rle.Add(Convert.ToInt32(temp - 128));
                    }
                    t = rle.ToArray();
                    t = reverse_rle(t);
                    block = reverse_zigzag(t);
                    chunky[i][j] = new Chunk(reverse_DCT(block.Multiply(luma)));
                    mv.X = br.ReadByte() - 128 + (j*8);//...except read back motion vectors
                    mv.Y = br.ReadByte() - 128 + (i*8);
                    chunky[i][j] = extensions.DAddition(chunky[i][j], cy, mv);//add the difference blocks to the original image.
                    rle.Clear();
                }
            }
            rle.Clear();

            for (int i = 0; i < chunkr.Length; i++)
            {
                chunkr[i] = new Chunk[(int)Math.Ceiling((w / 8.0f) / 2)];
                mvs[i] = new Point[(int)Math.Ceiling((w / 8.0f) / 2)];
                for (int j = 0; j < chunkr[i].Length; j++)
                {
                    while ((temp = br.ReadByte()) != 0x00)
                    {
                        rle.Add(Convert.ToInt32(temp - 128));
                    }
                    t = rle.ToArray();
                    t = reverse_rle(t);
                    block = reverse_zigzag(t);
                    chunkr[i][j] = new Chunk(reverse_DCT(block.Multiply(chroma)));
                    mv.X = br.ReadByte() - 128 + (j * 8);
                    mv.Y = br.ReadByte() - 128 + (i * 8);
                    chunkr[i][j] = extensions.DAddition(chunkr[i][j], cr, mv);
                    mvs[i][j] = mv;
                    rle.Clear();
                }
            }
            rle.Clear();

            for (int i = 0; i < chunkb.Length; i++)
            {
                chunkb[i] = new Chunk[(int)Math.Ceiling((w / 8.0f) / 2)];
                for (int j = 0; j < chunkb[i].Length; j++)
                {
                    while ((temp = br.ReadByte()) != 0x00)
                    {
                        rle.Add(Convert.ToInt32(temp - 128));
                    }
                    t = rle.ToArray();
                    t = reverse_rle(t);
                    block = reverse_zigzag(t);
                    chunkb[i][j] = new Chunk(reverse_DCT(block.Multiply(chroma)));
                    chunkb[i][j] = extensions.DAddition(chunkb[i][j], cb, mvs[i][j]);
                    rle.Clear();
                }
            }

            cy = reverse_chunkify(chunky);
            cr = reverse_chunkify(chunkr);
            cb = reverse_chunkify(chunkb);

            cr = reverse_subsample(cr);
            cb = reverse_subsample(cb);

            convert_rgb(h, w);
            for (int y = 0; y < h; y++)
            {
                for (int x = 0; x < w; x++)
                {
                    b.SetPixel(x, y, rgb[y][x]);
                }
            }
            return b;
        }

        private byte[] bitconvert(int source, bool bit)//true for 2bit, false for 4
        {
            int length = 4;
            if (bit)
            {
                length = 2;
            }
            var retVal = new byte[length];
            retVal[0] = (byte)(source & 0xFF);
            retVal[1] = (byte)((source >> 8) & 0xFF);
            if (length == 4)
            {
                retVal[2] = (byte)((source >> 0x10) & 0xFF);
                retVal[3] = (byte)((source >> 0x18) & 0xFF);
            }
            return retVal;
        }

    }
}
