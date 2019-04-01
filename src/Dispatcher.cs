namespace Dispatcher
{
    public static class CSV
    {
        public struct Record
        {
            public readonly string[] Row;
            public string this[int index] { get { return this.Row[index]; } }
            public Record(string[] row) { this.Row = row; }
        }

        public static global::System.Collections.Generic.List<global::Dispatcher.CSV.Record> ParseText(string text, char Token) { return global::Dispatcher.CSV.Parse(new global::System.IO.StringReader(text), Token); }
        public static global::System.Collections.Generic.List<global::Dispatcher.CSV.Record> ParseFile(string fn, char Token) { using (global::System.IO.StreamReader reader = global::System.IO.File.OpenText(fn)) { return global::Dispatcher.CSV.Parse(reader, Token); } }

        public static global::System.Collections.Generic.List<global::Dispatcher.CSV.Record> Parse(global::System.IO.TextReader reader, char Token)
        {
            global::System.Collections.Generic.List<global::Dispatcher.CSV.Record> data = new global::System.Collections.Generic.List<global::Dispatcher.CSV.Record>();
            global::System.Text.StringBuilder col = new global::System.Text.StringBuilder();
            global::System.Collections.Generic.List<string> row = new global::System.Collections.Generic.List<string>();
            string ln = reader.ReadLine();
            while (ln != null && ln != string.Empty)
            {
                if (global::Dispatcher.CSV.Tokenize(ln, Token, col, row))
                {
                    data.Add(new global::Dispatcher.CSV.Record(row.ToArray()));
                    row.Clear();
                }
                ln = reader.ReadLine();
            }
            return data;
        }

        public static bool Tokenize(string s, char Token, global::System.Text.StringBuilder col, global::System.Collections.Generic.List<string> row)
        {
            int i = 0;
            int parts = 0;
            int added = 0;
            if (col.Length > 0)
            {
                col.AppendLine();
                if (!global::Dispatcher.CSV.TokenizeQuote(s, ref i, col, row)) { return false; }
            }
            while (i < s.Length)
            {
                char ch = s[i];
                if (ch == Token)
                {
                    row.Add(col.ToString().Trim());
                    col.Length = 0;
                    i++;
                    added++;
                }
                else if (ch == '"')
                {
                    i++;
                    if (global::Dispatcher.CSV.TokenizeQuote(s, ref i, col, row)) { parts++; } else { return false; }
                }
                else
                {
                    col.Append(ch);
                    i++;
                }
            }
            if (col.Length > 0 || (parts > added))
            {
                row.Add(col.ToString().Trim());
                col.Length = 0;
            }
            return true;
        }

        private static bool TokenizeQuote(string s, ref int i, global::System.Text.StringBuilder col, global::System.Collections.Generic.List<string> row)
        {
            while (i < s.Length)
            {
                var ch = s[i];
                if (ch == '"')
                {
                    if (i + 1 < s.Length && s[i + 1] == '"')
                    {
                        col.Append('"');
                        i++;
                        i++;
                        continue;
                    }
                    i++;
                    return true;
                }
                else
                {
                    col.Append(ch);
                    i++;
                }
            }
            return false;
        }
    }

    internal class Icons
    {
        ///<summary>Value: 2.0</summary>
        protected const float Pen = 2.0f;
        ///<summary>Value: 24.0</summary>
        public const float IconSize = (global::Dispatcher.Icons.Pen * 12.0f);
        ///<summary>Value: 8.0</summary>
        protected const float MarginInternal = (global::Dispatcher.Icons.Pen * 4.0f);
        ///<summary>Value: 4.0</summary>
        protected const float MarginExternal = (global::Dispatcher.Icons.Pen * 2.0f);
        ///<summary>Value: 14</summary>
        protected const byte ColorDegration = 14;
        ///<summary>Value: (+)4</summary>
        protected const uint BarCount = (uint)((global::Dispatcher.Icons.IconSize - (global::Dispatcher.Icons.MarginExternal * 2.0f)) / global::Dispatcher.Icons.MarginExternal);

        protected static void Degrate(ref int R, ref int G, ref int B)
        {
            R += global::Dispatcher.Icons.ColorDegration;
            if (R < 0) { R = 0; } else if (R > 255) { R = 255; }
            G += global::Dispatcher.Icons.ColorDegration;
            if (G < 0) { G = 0; } else if (G > 255) { G = 255; }
            B += global::Dispatcher.Icons.ColorDegration;
            if (B < 0) { B = 0; } else if (B > 255) { B = 255; }
        }

        public static global::System.Drawing.Color GetDegrated(global::System.Drawing.Color Original, int Alpha = 255)
        {
            int R = Original.R;
            int G = Original.G;
            int B = Original.B;
            global::Dispatcher.Icons.Degrate(ref R, ref G, ref B);
            return global::System.Drawing.Color.FromArgb((Alpha < 0 ? 0 : (Alpha > 255 ? 255 : Alpha)), R, G, B);
        }

        protected static void DrawFrame(global::System.Drawing.Graphics gfx, global::System.Drawing.SolidBrush Darker, global::System.Drawing.SolidBrush Lighter)
        {
            gfx.FillPie(Lighter, 0, 0, global::Dispatcher.Icons.IconSize, global::Dispatcher.Icons.IconSize, 90, 180);
            gfx.FillPie(Darker, 0, 0, global::Dispatcher.Icons.IconSize, global::Dispatcher.Icons.IconSize, 270, 180);
        }

        protected static void DrawFrame(global::System.Drawing.Graphics gfx, global::System.Drawing.SolidBrush Darker) { using (global::System.Drawing.SolidBrush Lighter = new global::System.Drawing.SolidBrush(global::Dispatcher.Icons.GetDegrated(Darker.Color, Alpha: 255))) { global::Dispatcher.Icons.DrawFrame(gfx, Darker, Lighter); } }
        protected static void DrawFrame(global::System.Drawing.Graphics gfx, global::System.Drawing.Color Darker) { using (global::System.Drawing.SolidBrush DarkerBrush = new global::System.Drawing.SolidBrush(Darker)) { global::Dispatcher.Icons.DrawFrame(gfx, DarkerBrush); } }

        protected static global::System.Drawing.Graphics Build(out global::System.Drawing.Bitmap ico, global::System.Drawing.Color FrameColor = default(global::System.Drawing.Color))
        {
            ico = new global::System.Drawing.Bitmap((int)global::Dispatcher.Icons.IconSize, (int)global::Dispatcher.Icons.IconSize);
            global::System.Drawing.Graphics gfx = global::System.Drawing.Graphics.FromImage(ico);
            gfx.Clear(global::System.Drawing.Color.Transparent);
            if (FrameColor != default(global::System.Drawing.Color)) { global::Dispatcher.Icons.DrawFrame(gfx, FrameColor); }
            return gfx;
        }

        protected static global::System.Drawing.Bitmap DrawX(global::System.Drawing.Color PenColor, global::System.Drawing.Color FrameColor = default(global::System.Drawing.Color))
        {
            global::System.Drawing.Bitmap ico = null;
            using (global::System.Drawing.Graphics gfx = global::Dispatcher.Icons.Build(out ico, FrameColor: FrameColor))
            using (global::System.Drawing.Pen Pen = new global::System.Drawing.Pen(PenColor, global::Dispatcher.Icons.Pen))
            {
                float InfLimit = (global::Dispatcher.Icons.IconSize - global::Dispatcher.Icons.MarginInternal);
                gfx.DrawLine(Pen, global::Dispatcher.Icons.MarginInternal, global::Dispatcher.Icons.MarginInternal, InfLimit, InfLimit);
                gfx.DrawLine(Pen, global::Dispatcher.Icons.MarginInternal, InfLimit, InfLimit, global::Dispatcher.Icons.MarginInternal);
            }
            return ico;
        }

        protected static global::System.Drawing.Bitmap DrawV(global::System.Drawing.Color PenColor, global::System.Drawing.Color FrameColor = default(global::System.Drawing.Color))
        {
            global::System.Drawing.Bitmap ico = null;
            using (global::System.Drawing.Graphics gfx = global::Dispatcher.Icons.Build(out ico, FrameColor: FrameColor))
            using (global::System.Drawing.Pen Pen = new global::System.Drawing.Pen(PenColor, global::Dispatcher.Icons.Pen))
            {
                float OrigY = (global::Dispatcher.Icons.IconSize - global::Dispatcher.Icons.MarginInternal);
                float OrigX = (global::Dispatcher.Icons.IconSize / 2.0f);
                gfx.DrawLine(Pen, OrigX, OrigY, (OrigX + global::Dispatcher.Icons.MarginInternal), (OrigY - global::Dispatcher.Icons.MarginInternal));
                gfx.DrawLine(Pen, OrigX, OrigY, (OrigX - global::Dispatcher.Icons.MarginExternal), (OrigY - global::Dispatcher.Icons.MarginExternal));
            }
            return ico;
        }

        protected static global::System.Drawing.Bitmap DrawI(global::System.Drawing.Color PenColor, bool Invert, global::System.Drawing.Color FrameColor = default(global::System.Drawing.Color))
        {
            global::System.Drawing.Bitmap ico = null;
            using (global::System.Drawing.Graphics gfx = global::Dispatcher.Icons.Build(out ico, FrameColor: FrameColor))
            using (global::System.Drawing.SolidBrush Pen = new global::System.Drawing.SolidBrush(PenColor))
            {
                gfx.FillRectangle(Pen, global::Dispatcher.Icons.MarginInternal, global::Dispatcher.Icons.MarginExternal, global::Dispatcher.Icons.MarginInternal, global::Dispatcher.Icons.MarginExternal);
                float HalfParf = (global::Dispatcher.Icons.MarginInternal + (global::Dispatcher.Icons.MarginExternal / 2.0f));
                gfx.FillRectangle(Pen, global::Dispatcher.Icons.MarginInternal, HalfParf, global::Dispatcher.Icons.MarginInternal, HalfParf);
            }
            return ico;
        }

        protected static global::System.Drawing.Bitmap DrawBars(global::System.Drawing.Color PenColor, uint BarCount, uint Start)
        {
            global::System.Drawing.Bitmap ico = null;
            using (global::System.Drawing.Graphics gfx = global::Dispatcher.Icons.Build(out ico, FrameColor: default(global::System.Drawing.Color)))
            using (global::System.Drawing.Pen Pen = new global::System.Drawing.Pen(PenColor))
            {
                float OriY = global::Dispatcher.Icons.MarginExternal;
                float EndX = (global::Dispatcher.Icons.IconSize - global::Dispatcher.Icons.MarginExternal);
                for (uint i = 0; BarCount > 0; i++)
                {
                    if (i >= Start)
                    {
                        gfx.DrawLine(Pen, global::Dispatcher.Icons.MarginExternal, OriY, EndX, OriY);
                        BarCount--;
                    }
                    OriY += global::Dispatcher.Icons.MarginExternal;
                }
            }
            return ico;
        }

        protected static global::System.Drawing.Bitmap DrawSquare(global::System.Drawing.Color PenColor, global::System.Drawing.Color FrameColor = default(global::System.Drawing.Color))
        {
            global::System.Drawing.Bitmap ico = null;
            using (global::System.Drawing.Graphics gfx = global::Dispatcher.Icons.Build(out ico, FrameColor: FrameColor))
            using (global::System.Drawing.Pen Pen = new global::System.Drawing.Pen(PenColor))
            {
                float Size = (global::Dispatcher.Icons.IconSize - (2.0f * global::Dispatcher.Icons.MarginExternal));
                gfx.DrawRectangle(Pen, global::Dispatcher.Icons.MarginExternal, global::Dispatcher.Icons.MarginExternal, Size, Size);
            }
            return ico;
        }

        public static readonly global::System.Drawing.Color Red = global::System.Drawing.Color.FromArgb(200, 60, 70);
        public static readonly global::System.Drawing.Color Blue = global::System.Drawing.Color.FromArgb(90, 140, 230);
        public static readonly global::System.Drawing.Color Yellow = global::System.Drawing.Color.FromArgb(200, 200, 80);
        public static readonly global::System.Drawing.Color Green = global::System.Drawing.Color.FromArgb(80, 200, 130);
        public static readonly global::System.Drawing.Color Gray = global::System.Drawing.Color.FromArgb(140, 140, 140);
        ///<summary>Icon: White "X" on a red circle</summary>
        public readonly global::System.Drawing.Bitmap Error = global::Dispatcher.Icons.DrawX(global::System.Drawing.Color.White, FrameColor: global::Dispatcher.Icons.Red);
        ///<summary>Icon: White "X" on transparency</summary>
        public readonly global::System.Drawing.Bitmap Close = global::Dispatcher.Icons.DrawX(global::System.Drawing.Color.White, FrameColor: default(global::System.Drawing.Color));
        ///<summary>Icon: White "V" on a green circle</summary>
        public readonly global::System.Drawing.Bitmap OK = global::Dispatcher.Icons.DrawV(global::System.Drawing.Color.White, FrameColor: global::Dispatcher.Icons.Green);
        ///<summary>Icon: White "V" on transparency</summary>
        public readonly global::System.Drawing.Bitmap Check = global::Dispatcher.Icons.DrawV(global::System.Drawing.Color.White, FrameColor: default(global::System.Drawing.Color));
        ///<summary>Icon: White "V" on a blue circle</summary>
        public readonly global::System.Drawing.Bitmap Info = global::Dispatcher.Icons.DrawI(global::System.Drawing.Color.White, false, FrameColor: global::Dispatcher.Icons.Blue);
        ///<summary>Icon: White "V" on transparency</summary>
        public readonly global::System.Drawing.Bitmap Warning = global::Dispatcher.Icons.DrawI(global::System.Drawing.Color.White, true, FrameColor: global::Dispatcher.Icons.Yellow);
        ///<summary>Icon: White Horizontal Bars (multiple) on transparency</summary>
        public readonly global::System.Drawing.Bitmap Menu = global::Dispatcher.Icons.DrawBars(global::System.Drawing.Color.White, global::Dispatcher.Icons.BarCount, 0U);
        ///<summary>Icon: White Horizontal Bar (only one) on transparency</summary>
        public readonly global::System.Drawing.Bitmap Minimize = global::Dispatcher.Icons.DrawBars(global::System.Drawing.Color.White, 1U, (global::Dispatcher.Icons.BarCount / 2U));
        ///<summary>Icon: White square on a gray circle</summary>
        public readonly global::System.Drawing.Bitmap Query = global::Dispatcher.Icons.DrawSquare(global::System.Drawing.Color.White, FrameColor: global::Dispatcher.Icons.Gray);
        ///<summary>Icon: White square on transparency</summary>
        public readonly global::System.Drawing.Bitmap Maximize = global::Dispatcher.Icons.DrawSquare(global::System.Drawing.Color.White, FrameColor: default(global::System.Drawing.Color));
    }

    internal abstract class Form : global::System.Windows.Forms.Form
    {
        protected static readonly global::Dispatcher.Icons Icons = new global::Dispatcher.Icons();
        ///<summary>Value: "Verdana"</summary>
        public const string StandardFontFamily = "Verdana";
        ///<summary>Value: 2</summary>
        public const int ControlMargin = 2;
        ///<summary>Value: 80</summary>
        public const int StandardButtonWidth = 80;
        ///<summary>Value: 24</summary>
        public const int StandardButtonHeight = ((int)global::Dispatcher.Icons.IconSize);
        ///<summary>Value: 9.25</summary>
        public const float StandardFontSize = 9.25f;
        ///<summary>Value: 18</summary>
        public const int StandardLabelHeight = ((int)((global::Dispatcher.Form.StandardFontSize * 2.0f) - 0.45f));
        ///<summary>Value: 22</summary>
        public const int StandardControlHeight = ((int)((global::Dispatcher.Form.StandardFontSize * 2.0f) + (float)global::Dispatcher.Form.ControlMargin - 0.45f));

        protected class PosXY
        {
            public int X;
            public int Y;
            public global::System.Drawing.Point firstPoint;
            public bool mouseIsDown = false;

            public PosXY(int x, int y)
            {
                this.X = x;
                this.Y = y;
            }

            public PosXY(global::System.Drawing.Point Point) : this(Point.X, Point.Y) { /* NOTHING */ }
        }

        public enum Type : short
        {
            CLIENT = 0,
            INFO = 1,
            WARNING = 2,
            ERROR = 3,
            OK = 4,
            CONFIRM = 5,
            INPUT = 6
        }

        private bool Maximizable;
        public bool CanMaximize { get { return this.Maximizable; } }
        protected global::Dispatcher.Form.PosXY Pos;
        protected global::System.Drawing.Color Theme;
        protected global::System.Drawing.Color Hover;
        protected global::System.Windows.Forms.PictureBox buttonClose;
        protected global::System.Windows.Forms.PictureBox buttonMenu;
        protected global::System.Windows.Forms.PictureBox buttonMinimize;
        protected global::System.Windows.Forms.Label titleText;
        protected global::System.ComponentModel.IContainer components = null;
        public override string Text { get { return (this.titleText == null ? string.Empty : this.titleText.Text); } set { if (this.titleText != null) { this.titleText.Text = value; } } }
        protected virtual void OnMinimize(object sender, global::System.EventArgs e) { this.WindowState = global::System.Windows.Forms.FormWindowState.Minimized; }
        private void OnMouseMove(object sender, global::System.Windows.Forms.MouseEventArgs e) { if (this.Pos.mouseIsDown) { this.Location = new global::System.Drawing.Point((this.Location.X - (this.Pos.firstPoint.X - e.Location.X)), (this.Location.Y - (this.Pos.firstPoint.Y - e.Location.Y))); } }
        private void OnMouseUp(object sender, global::System.Windows.Forms.MouseEventArgs e) { this.Pos.mouseIsDown = false; }
        protected virtual global::System.Drawing.Bitmap IconMenu() { return (this.Maximizable ? global::Dispatcher.Form.Icons.Menu : null); }

        private void OnMouseDown(object sender, global::System.Windows.Forms.MouseEventArgs e)
        {
            this.Pos.firstPoint = e.Location;
            this.Pos.mouseIsDown = true;
        }

        public TControl InitControl<TControl>(string name, global::System.Drawing.Point Location, global::System.Drawing.Size Size, bool AddToForm, global::System.Drawing.Color ForeColor = default(global::System.Drawing.Color), global::System.Drawing.Color BackColor = default(global::System.Drawing.Color)) where TControl : global::System.Windows.Forms.Control, new()
        {
            TControl cnt = new TControl();
            cnt.Name = Name;
            cnt.Location = Location;
            cnt.Size = Size;
            cnt.Font = this.Font;
            cnt.ForeColor = ForeColor;
            cnt.BackColor = BackColor;
            if (AddToForm) { this.Controls.Add(cnt); }
            return cnt;
        }

        public TControl InitControl<TControl>(string name, int X, int Y, int Width, int Height, bool AddToForm, global::System.Drawing.Color ForeColor = default(global::System.Drawing.Color), global::System.Drawing.Color BackColor = default(global::System.Drawing.Color)) where TControl : global::System.Windows.Forms.Control, new() { return this.InitControl<TControl>(name, new global::System.Drawing.Point(X, Y), new global::System.Drawing.Size(Width, Height), AddToForm, ForeColor: ForeColor, BackColor: BackColor); }
        public TControl InitControl<TControl>(string name, int X, int Y, float Width, float Height, bool AddToForm, global::System.Drawing.Color ForeColor = default(global::System.Drawing.Color), global::System.Drawing.Color BackColor = default(global::System.Drawing.Color)) where TControl : global::System.Windows.Forms.Control, new() { return this.InitControl<TControl>(name, new global::System.Drawing.Point(X, Y), new global::System.Drawing.Size((int)Width, (int)Height), AddToForm, ForeColor: ForeColor, BackColor: BackColor); }

        public global::System.Windows.Forms.Label InitLabel(string name, int X, int Y, int Width, int Height, bool AddToForm, string Text, global::System.Drawing.Color ForeColor = default(global::System.Drawing.Color), global::System.Drawing.Color BackColor = default(global::System.Drawing.Color))
        {
            global::System.Windows.Forms.Label cnt = this.InitControl<global::System.Windows.Forms.Label>(name, X, Y, Width, Height, AddToForm, ForeColor: (ForeColor == default(global::System.Drawing.Color) ? global::System.Drawing.SystemColors.ControlText : ForeColor), BackColor: (BackColor == default(global::System.Drawing.Color) ? global::System.Drawing.Color.White : BackColor));
            cnt.BorderStyle = global::System.Windows.Forms.BorderStyle.FixedSingle;
            cnt.Text = Text;
            return cnt;
        }

        public global::System.Windows.Forms.Label InitLabel(string name, int X, int Y, int Width, bool AddToForm, string Text, global::System.Drawing.Color ForeColor = default(global::System.Drawing.Color), global::System.Drawing.Color BackColor = default(global::System.Drawing.Color)) { return this.InitLabel(name, X, Y, Width, 17, AddToForm, Text, ForeColor: (ForeColor == default(global::System.Drawing.Color) ? global::System.Drawing.SystemColors.ControlText : ForeColor), BackColor: (BackColor == default(global::System.Drawing.Color) ? global::System.Drawing.Color.White : BackColor)); }

        public global::System.Windows.Forms.TextBox InitTextBox(string name, int X, int Y, int Width, int Height, bool AddToForm, string Text = "", int MaxLength = 0, bool Multiline = false, global::System.Drawing.Color ForeColor = default(global::System.Drawing.Color), global::System.Drawing.Color BackColor = default(global::System.Drawing.Color))
        {
            global::System.Windows.Forms.TextBox cnt = this.InitControl<global::System.Windows.Forms.TextBox>(name, X, Y, Width, Height, AddToForm, ForeColor: (ForeColor == default(global::System.Drawing.Color) ? global::System.Drawing.SystemColors.ControlText : ForeColor), BackColor: (BackColor == default(global::System.Drawing.Color) ? global::System.Drawing.Color.White : BackColor));
            cnt.BorderStyle = global::System.Windows.Forms.BorderStyle.FixedSingle;
            cnt.MaxLength = MaxLength;
            cnt.Multiline = Multiline;
            if (Multiline) { cnt.Height *= 3; }
            cnt.Text = Text;
            return cnt;
        }

        public global::System.Windows.Forms.Button InitButton(string name, int X, int Y, string Text, bool AddToForm, int Width = global::Dispatcher.Form.StandardButtonWidth, int Height = global::Dispatcher.Form.StandardButtonHeight, global::System.EventHandler Handler = null)
        {
            global::System.Windows.Forms.Button btn = this.InitControl<global::System.Windows.Forms.Button>(name, X, Y, Width, Height, AddToForm, ForeColor: global::System.Drawing.Color.White, BackColor: this.Theme);
            btn.FlatStyle = global::System.Windows.Forms.FlatStyle.Flat;
            btn.Text = Text;
            if (Handler != null) { btn.Click += Handler; }
            return btn;
        }

        private global::System.Windows.Forms.PictureBox InitImageButton(string name, global::System.Drawing.Point Location, global::System.Drawing.Bitmap Image, global::System.EventHandler Handler = null)
        {
            global::System.Windows.Forms.PictureBox yielder = this.InitControl<global::System.Windows.Forms.PictureBox>(name, Location.X, Location.Y, global::Dispatcher.Icons.IconSize, global::Dispatcher.Icons.IconSize, true, ForeColor: global::System.Drawing.Color.White, BackColor: default(global::System.Drawing.Color));
            ((global::System.ComponentModel.ISupportInitialize)yielder).BeginInit();
            yielder.BackgroundImage = Image;
            yielder.BackgroundImageLayout = global::System.Windows.Forms.ImageLayout.Center;
            yielder.TabStop = false;
            ((global::System.ComponentModel.ISupportInitialize)yielder).EndInit();
            if (Handler != null) { yielder.Click += Handler; }
            return yielder;
        }

        protected virtual void OnClose(object sender, global::System.EventArgs e)
        {
            if (sender == this.AcceptButton) { this.DialogResult = (this.CancelButton == null ? global::System.Windows.Forms.DialogResult.OK : global::System.Windows.Forms.DialogResult.Yes); }
            else if (sender == this.CancelButton) { this.DialogResult = global::System.Windows.Forms.DialogResult.No; }
            else { this.DialogResult = global::System.Windows.Forms.DialogResult.OK; }
            this.Dispose();
            this.Close();
        }

        protected virtual void OnMaximize(object sender, global::System.EventArgs e)
        {
            if (this.Maximizable)
            {
                switch (this.WindowState)
                {
                    case global::System.Windows.Forms.FormWindowState.Normal: this.WindowState = global::System.Windows.Forms.FormWindowState.Maximized; break;
                    case global::System.Windows.Forms.FormWindowState.Maximized: this.WindowState = global::System.Windows.Forms.FormWindowState.Normal; break;
                }
            }
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing && (this.components != null)) { this.components.Dispose(); }
            base.Dispose(disposing);
        }

        protected virtual void SetColor(global::System.Drawing.Color Color)
        {
            if (this.buttonClose != null) this.buttonClose.BackColor = Color;
            if (this.buttonMenu != null) this.buttonMenu.BackColor = Color;
            if (this.buttonMinimize != null) this.buttonMinimize.BackColor = Color;
            if (this.titleText != null) this.titleText.BackColor = Color;
        }

        protected void SetEvent(global::System.Windows.Forms.Control Control)
        {
            if (Control != null)
            {
                Control.MouseHover += (s, e) => { this.SetColor(this.Hover); };
                Control.MouseLeave += (s, e) => { this.SetColor(this.Theme); };
            }
        }

        protected virtual void SetEvent(global::System.Drawing.Color Hover, global::System.Drawing.Color Leave)
        {
            this.Theme = Leave;
            this.Hover = Hover;
            this.SetColor(this.Theme);
            this.SetEvent(this.buttonClose);
            this.SetEvent(this.buttonMenu);
            this.SetEvent(this.buttonMinimize);
            this.SetEvent(this.titleText);
        }

        protected virtual void RelocateComponents(int Width, int Height)
        {
            Width -= ((int)global::Dispatcher.Icons.IconSize + global::Dispatcher.Form.ControlMargin);
            if (this.buttonClose != null) this.buttonClose.Location = new global::System.Drawing.Point(Width, global::Dispatcher.Form.ControlMargin);
            Width -= (int)global::Dispatcher.Icons.IconSize;
            if (this.buttonMenu != null) this.buttonMenu.Location = new global::System.Drawing.Point(Width, global::Dispatcher.Form.ControlMargin);
            if (this.buttonMinimize != null)
            {
                Width -= (int)global::Dispatcher.Icons.IconSize;
                this.buttonMinimize.Location = new global::System.Drawing.Point(Width, global::Dispatcher.Form.ControlMargin);
            }
            if (this.titleText != null) this.titleText.Size = new global::System.Drawing.Size(Width, 24);
        }

        protected virtual void CreateComponents(bool Minimizable, int Width, int Height)
        {
            this.titleText = this.InitControl<global::System.Windows.Forms.Label>("titleText", global::Dispatcher.Form.ControlMargin, global::Dispatcher.Form.ControlMargin, 0, 0, true, ForeColor: global::System.Drawing.Color.White, BackColor: default(global::System.Drawing.Color));
            this.titleText.TextAlign = global::System.Drawing.ContentAlignment.MiddleLeft;
            this.titleText.MouseDown += new global::System.Windows.Forms.MouseEventHandler(this.OnMouseDown);
            this.titleText.MouseMove += new global::System.Windows.Forms.MouseEventHandler(this.OnMouseMove);
            this.titleText.MouseUp += new global::System.Windows.Forms.MouseEventHandler(this.OnMouseUp);
            if (this.Maximizable)
            {
                this.titleText.DoubleClick += new global::System.EventHandler(this.OnMaximize);
                this.buttonMenu = this.InitImageButton("buttonMenu", default(global::System.Drawing.Point), this.IconMenu(), Handler: this.OnMaximize);
            } else { this.buttonMenu = this.InitImageButton("buttonMenu", default(global::System.Drawing.Point), this.IconMenu(), Handler: null); }
            this.buttonClose = this.InitImageButton("buttonClose", default(global::System.Drawing.Point), global::Dispatcher.Form.Icons.Close, Handler: this.OnClose);
            if (Minimizable) { this.buttonMinimize = this.InitImageButton("buttonMinimize", default(global::System.Drawing.Point), global::Dispatcher.Form.Icons.Minimize, Handler: this.OnMinimize); }
        }

        protected void SetEvent(global::System.Drawing.Color Leave) { this.SetEvent(global::Dispatcher.Icons.GetDegrated(Leave), Leave); }
        protected void RelocateComponents(global::System.Drawing.Size Size) { this.RelocateComponents(Size.Width, Size.Height); }
        protected void CreateComponents(bool Minimizable, global::System.Drawing.Size Size) { this.CreateComponents(Minimizable, Size.Width, Size.Height); }

        protected virtual void SetEvent(global::Dispatcher.Form.Type Type)
        {
            switch (Type)
            {
                case global::Dispatcher.Form.Type.ERROR: this.SetEvent(global::Dispatcher.Icons.Red); break;
                case global::Dispatcher.Form.Type.WARNING: this.SetEvent(global::Dispatcher.Icons.Yellow); break;
                case global::Dispatcher.Form.Type.OK: this.SetEvent(global::Dispatcher.Icons.Green); break;
                case global::Dispatcher.Form.Type.CONFIRM:
                case global::Dispatcher.Form.Type.INPUT: this.SetEvent(global::Dispatcher.Icons.Gray); break;
                default: this.SetEvent(global::Dispatcher.Icons.Blue); break;
            }
        }

        protected override void OnResize(global::System.EventArgs e)
        {
            base.OnResize(e);
            this.RelocateComponents(this.ClientSize);
        }

        protected void InitializeComponent(string Title, int Width, int Height, bool Maximizable, bool ShowInTaskbar, global::Dispatcher.Form.Type Type)
        {
            this.Maximizable = Maximizable;
            this.SuspendLayout();
            this.Font = new global::System.Drawing.Font(global::Dispatcher.Form.StandardFontFamily, global::Dispatcher.Form.StandardFontSize);
            this.AutoScaleMode = global::System.Windows.Forms.AutoScaleMode.None;
            this.TransparencyKey = global::System.Drawing.Color.FromArgb(128, 128, 128);
            this.FormBorderStyle = global::System.Windows.Forms.FormBorderStyle.None;
            this.MinimizeBox = false;
            this.MaximizeBox = false;
            this.ControlBox = false;
            base.Text = string.Empty;
            this.BackgroundImageLayout = global::System.Windows.Forms.ImageLayout.Stretch;
            this.ClientSize = new global::System.Drawing.Size(Width, Height);
            this.StartPosition = global::System.Windows.Forms.FormStartPosition.CenterScreen;
            this.BackColor = global::System.Drawing.Color.White;
            this.DoubleBuffered = true;
            this.ShowInTaskbar = ShowInTaskbar;
            this.components = new global::System.ComponentModel.Container();
            this.CreateComponents(ShowInTaskbar, this.ClientSize);
            this.SetEvent(Type);
            this.RelocateComponents(this.ClientSize);
            this.ResumeLayout(false);
            this.PerformLayout();
            this.Pos = new global::Dispatcher.Form.PosXY(this.Left, this.Top);
            this.Text = Title;
        }

        protected Form() : base() { /* NOTHING */ }
        protected Form(string Title, int Width, int Height, bool Maximizable, bool ShowInTaskbar, global::Dispatcher.Form.Type Type) : base() { this.InitializeComponent(Title, Width, Height, Maximizable, ShowInTaskbar, Type); }
        protected Form(string Title, int Width, int Height, bool Maximizable, bool ShowInTaskbar) : base() { this.InitializeComponent(Title, Width, Height, Maximizable, ShowInTaskbar, global::Dispatcher.Form.Type.CLIENT); }
    }

    internal class Container : global::Dispatcher.Form
    {
        private bool Resizing;
        public bool CanResize { get { return this.CanMaximize; } }
        public bool IsResizing { get { return this.Resizing; } }
        protected global::System.Drawing.ContentAlignment ResizeDirection;
        protected global::System.Drawing.Point ResizeOrigin;
        protected override global::System.Drawing.Bitmap IconMenu() { return (this.CanMaximize ? global::Dispatcher.Form.Icons.Maximize : null); }

        private void SetResize(bool On, global::System.Drawing.ContentAlignment Direction, global::System.Drawing.Point Origin)
        {
            this.Resizing = On;
            this.ResizeDirection = Direction;
            this.ResizeOrigin = Origin;
            if (Direction == global::System.Drawing.ContentAlignment.MiddleCenter) { this.Cursor = global::System.Windows.Forms.Cursors.Arrow; } else { this.Cursor = global::System.Windows.Forms.Cursors.SizeAll; }
        }

        private void OnMouseUp(object sender, global::System.Windows.Forms.MouseEventArgs e)
        {
            if (this.Resizing)
            {
                int difX = (e.X - this.ResizeOrigin.X);
                int difY = (e.Y - this.ResizeOrigin.Y);
                switch (this.ResizeDirection)
                {
                    case global::System.Drawing.ContentAlignment.BottomCenter: this.Height += difY; break;
                    case global::System.Drawing.ContentAlignment.MiddleRight: this.Width += difX; break;
                    case global::System.Drawing.ContentAlignment.BottomRight: this.Size = new global::System.Drawing.Size((this.Width + difX), (this.Height + difY)); break;
                    case global::System.Drawing.ContentAlignment.TopRight:
                        this.Top += difY;
                        this.Size = new global::System.Drawing.Size((this.Width + difX), (this.Height - difY));
                        break;
                    case global::System.Drawing.ContentAlignment.MiddleLeft:
                        this.Left += difX;
                        this.Width -= difX;
                        break;
                    case global::System.Drawing.ContentAlignment.TopCenter:
                        this.Top += difY;
                        this.Height -= difY;
                        break;
                    case global::System.Drawing.ContentAlignment.TopLeft:
                        this.Location = new global::System.Drawing.Point((this.Left + difX), (this.Top + difY));
                        this.Size = new global::System.Drawing.Size((this.Width - difX), (this.Height - difY));
                        break;
                    case global::System.Drawing.ContentAlignment.BottomLeft:
                        this.Left += difX;
                        this.Size = new global::System.Drawing.Size((this.Width - difX), (this.Height + difY));
                        break;
                }
                this.SetResize(false, global::System.Drawing.ContentAlignment.MiddleCenter, default(global::System.Drawing.Point));
            }
        }

        private void OnMouseDown(object sender, global::System.Windows.Forms.MouseEventArgs e)
        {
            if (e.X >= (this.Width - global::Dispatcher.Form.ControlMargin) && e.X <= this.Width) //Right Edge
            {
                if (e.Y >= (this.Height - global::Dispatcher.Form.ControlMargin) && e.Y <= this.Height) { this.SetResize(true, global::System.Drawing.ContentAlignment.BottomRight, e.Location); }
                else if (e.Y <= global::Dispatcher.Form.ControlMargin && e.Y >= 0) { this.SetResize(true, global::System.Drawing.ContentAlignment.TopRight, e.Location); }
                else { this.SetResize(true, global::System.Drawing.ContentAlignment.MiddleRight, e.Location); }
            }
            else if (e.X <= global::Dispatcher.Form.ControlMargin && e.X >= 0) //Left Edge
            {
                if (e.Y >= (this.Height - global::Dispatcher.Form.ControlMargin) && e.Y <= this.Height) { this.SetResize(true, global::System.Drawing.ContentAlignment.BottomLeft, e.Location); }
                else if (e.Y <= global::Dispatcher.Form.ControlMargin && e.Y >= 0) { this.SetResize(true, global::System.Drawing.ContentAlignment.TopLeft, e.Location); }
                else { this.SetResize(true, global::System.Drawing.ContentAlignment.MiddleLeft, e.Location); }
            }
            else if (e.Y >= (this.Height - global::Dispatcher.Form.ControlMargin) && e.Y <= this.Height) { this.SetResize(true, global::System.Drawing.ContentAlignment.BottomCenter, e.Location); }
            else if (e.Y <= global::Dispatcher.Form.ControlMargin && e.Y >= 0) { this.SetResize(true, global::System.Drawing.ContentAlignment.TopCenter, e.Location); }
        }

        protected void InitializeContainer(string Title, int Width, int Height, bool Maximizable, bool ShowInTaskbar, global::Dispatcher.Form.Type Type)
        {
            this.InitializeComponent(Title, Width, Height, Maximizable, ShowInTaskbar, Type);
            this.SetResize(false, global::System.Drawing.ContentAlignment.MiddleCenter, default(global::System.Drawing.Point));
            if (this.CanMaximize)
            {
                this.MouseDown += new global::System.Windows.Forms.MouseEventHandler(this.OnMouseDown);
                this.MouseUp += new global::System.Windows.Forms.MouseEventHandler(this.OnMouseUp);
            }
        }

        public void InitFieldLabel(string Name, string Text, ref int Y, global::System.Drawing.Color LabelBackColor = default(global::System.Drawing.Color))
        {
            this.InitLabel(("label" + Name), 6, Y, (this.Width - 12), true, Text, ForeColor: global::System.Drawing.Color.White, BackColor: (LabelBackColor == default(global::System.Drawing.Color) ? this.Theme : LabelBackColor));
            Y += 17;
        }

        public global::System.Windows.Forms.TextBox InitField(string Name, string Text, ref int Y, bool Multiline, global::System.Drawing.Color LabelBackColor = default(global::System.Drawing.Color))
        {
            this.InitFieldLabel(Name, Text, ref Y, LabelBackColor: LabelBackColor);
            global::System.Windows.Forms.TextBox field = this.InitTextBox(Name, 6, Y, (this.Width - 12), 22, true, Text: string.Empty, MaxLength: 0, Multiline: Multiline, ForeColor: default(global::System.Drawing.Color), BackColor: default(global::System.Drawing.Color));
            Y += (field.Height + 3);
            return field;
        }

        protected Container() : base() { /* NOTHING */ }
        public Container(string Title, int Width, int Height, bool Maximizable, bool ShowInTaskbar, global::Dispatcher.Form.Type Type) : base() { this.InitializeContainer(Title, Width, Height, Maximizable, ShowInTaskbar, Type); }
        public Container(string Title, int Width, int Height, bool Maximizable, bool ShowInTaskbar) : base() { this.InitializeContainer(Title, Width, Height, Maximizable, ShowInTaskbar, global::Dispatcher.Form.Type.CLIENT); }
    }

    internal sealed class Notifier : global::Dispatcher.Form
    {
        ///<summary>Value: 36</summary>
        public const int NotifierButtonHeight = ((int)((float)global::Dispatcher.Form.StandardButtonHeight * 1.5f));
        private global::System.Windows.Forms.PictureBox icon;
        private global::System.Windows.Forms.Label labelDesc;
        private global::System.Windows.Forms.ContextMenuStrip menu;
        private global::System.Windows.Forms.ToolStripMenuItem closeAllToolStripMenuItem;
        private global::System.Windows.Forms.TextBox inputbox;
        private static global::System.Collections.Generic.List<global::Dispatcher.Form.PosXY> posXY = new global::System.Collections.Generic.List<global::Dispatcher.Form.PosXY>(2);
        private static global::System.Collections.Generic.List<global::Dispatcher.Notifier> Notifiers = new global::System.Collections.Generic.List<global::Dispatcher.Notifier>(2);
        private static short ID = 0x00;
        private global::Dispatcher.Form.Type type = global::Dispatcher.Form.Type.INFO;
        private global::System.Windows.Forms.Form obscure;
        private bool isDiag;
        private string outinput;
        public bool IsDialog { get { return this.isDiag; } }
        public string Description { get { return (this.labelDesc == null ? string.Empty : this.labelDesc.Text); } set { if (this.labelDesc != null) { this.labelDesc.Text = value; } } }
        private bool posXYContains() { foreach (global::Dispatcher.Form.PosXY p in global::Dispatcher.Notifier.posXY) { if (p.X == this.Pos.X && p.Y == this.Pos.Y) { return true; } } return false; }
        private global::Dispatcher.Notifier getRemovedNotifier(string _title, string _desc, global::Dispatcher.Form.Type _type) { for (int i = global::Dispatcher.Notifier.Notifiers.Count - 1; i >= 0; i--) { if (global::Dispatcher.Notifier.Notifiers[i].Tag != null && global::Dispatcher.Notifier.Notifiers[i].Description == _desc && global::Dispatcher.Notifier.Notifiers[i].Text == _title && global::Dispatcher.Notifier.Notifiers[i].type == _type) { return global::Dispatcher.Notifier.Notifiers[i]; } } return null; }
        protected override global::System.Drawing.Bitmap IconMenu() { return global::Dispatcher.Form.Icons.Menu; }
        public string Input { get { return this.outinput; } }

        public static void CloseAll()
        {
            for (int i = 0; i < global::Dispatcher.Notifier.Notifiers.Count; i++) { if (global::Dispatcher.Notifier.Notifiers[i].Tag != null && global::Dispatcher.Notifier.Notifiers[i].Tag.ToString().Contains("__Notifier|")) { global::Dispatcher.Notifier.Notifiers[i].Dispose(); } }
            global::Dispatcher.Notifier.Notifiers.Clear();
            global::Dispatcher.Notifier.posXY.Clear();
            global::Dispatcher.Notifier.ID = 0;
        }

        protected override void OnClose(object sender, global::System.EventArgs e)
        {
            global::Dispatcher.Notifier NotifierMe = this.getRemovedNotifier(this.Text, this.Description, this.type);
            if (NotifierMe != null)
            {
                global::Dispatcher.Notifier.Notifiers.Remove(NotifierMe);
                global::Dispatcher.Notifier.posXY.Remove(this.Pos);
            }
            if (this.inputbox != null)
            {
                this.outinput = this.inputbox.Text;
                this.inputbox.Text = string.Empty;
            }
            base.OnClose(sender, e);
        }

        private static bool NotifierAlreadyPresent(ref string Description, global::Dispatcher.Form.Type type, ref string Title, out short updated_note_id, out short updated_note_occurency)
        {
            updated_note_id = 0;
            updated_note_occurency = 1;
            bool resParse = false;
            for (int i = 0; i < Notifiers.Count; i++)
            {
                short occurency = 1;
                string filteredTitle = Notifiers[i].Text;
                int indx = filteredTitle.IndexOf(']');
                if (indx > 0)
                {
                    string number_occ = filteredTitle.Substring(0, indx);
                    number_occ = number_occ.Trim(' ', ']', '[');
                    resParse = short.TryParse(number_occ, out occurency);
                    filteredTitle = filteredTitle.Substring(indx + 1).Trim();
                }
                if (global::Dispatcher.Notifier.Notifiers[i].Tag != null && global::Dispatcher.Notifier.Notifiers[i].Description == Description && filteredTitle == Title && global::Dispatcher.Notifier.Notifiers[i].type == type)
                {
                    string hex_id = global::Dispatcher.Notifier.Notifiers[i].Tag.ToString().Split('|')[1];
                    short id = global::System.Convert.ToInt16(hex_id, 16);
                    updated_note_id = id;
                    updated_note_occurency = ++occurency;
                    return true;
                }
            }
            return false;
        }

        public static void Update(short ID, string Description, global::Dispatcher.Form.Type noteType, string Title)
        {
            for (int i = 0; i < global::Dispatcher.Notifier.Notifiers.Count; i++)
            {
                if (global::Dispatcher.Notifier.Notifiers[i].Tag != null && global::Dispatcher.Notifier.Notifiers[i].Tag.Equals("__Notifier|" + ID.ToString("X4")))
                {
                    global::Dispatcher.Notifier myNote = (global::Dispatcher.Notifier)Notifiers[i];
                    myNote.Text = Title;
                    myNote.Description = Description;
                    myNote.setNotifier(noteType, true);
                }
            }
        }

        public static global::System.Windows.Forms.DialogResult ShowDialog(string Description, global::Dispatcher.Form.Type Type, string Title, out string Input, global::System.Windows.Forms.Form obscureMe = null)
        {
            global::System.Windows.Forms.Form back = new global::System.Windows.Forms.Form();
            back.FormBorderStyle = global::System.Windows.Forms.FormBorderStyle.None;
            back.BackColor = global::System.Drawing.Color.FromArgb(0, 0, 0);
            back.Opacity = 0.6;
            int border = 200;
            global::Dispatcher.Notifier not = new global::Dispatcher.Notifier(Title, Description, true, Type, Caller: obscureMe);
            bool orgTopMostSettings = false;
            if (obscureMe == null)
            {
                back.Location = new System.Drawing.Point(-border, -border);
                back.Size = new global::System.Drawing.Size(global::System.Windows.Forms.Screen.PrimaryScreen.WorkingArea.Width + border, global::System.Windows.Forms.Screen.PrimaryScreen.WorkingArea.Height + border);
                back.Show();
                back.TopMost = true;
            }
            else
            {
                orgTopMostSettings = obscureMe.TopMost;
                obscureMe.TopMost = true;
                global::System.Drawing.Point locationOnForm = obscureMe.PointToScreen(global::System.Drawing.Point.Empty);
                back.Size = obscureMe.Size;
                back.Location = obscureMe.Location;
                back.StartPosition = global::System.Windows.Forms.FormStartPosition.Manual;
                back.Show();
                back.TopMost = true;
            }
            global::Dispatcher.Notifier.Notifiers.Add(not);
            global::System.Windows.Forms.DialogResult Result = not.ShowDialog();
            back.Close();
            if (obscureMe != null) obscureMe.TopMost = orgTopMostSettings;
            if (Result == global::System.Windows.Forms.DialogResult.No || Result == global::System.Windows.Forms.DialogResult.Cancel) { Input = string.Empty; } else { Input = not.Input; }
            return Result;
        }

        public static short Show(global::Dispatcher.Notifier.Type Type, string Description, string Title)
        {
            short updated_note_id = 0;
            short updated_note_occurency = 0;
            if (global::Dispatcher.Notifier.NotifierAlreadyPresent(ref Description, Type, ref Title, out updated_note_id, out updated_note_occurency))
            {
                ID = updated_note_id;
                global::Dispatcher.Notifier.Update(ID, Description, Type, "[" + updated_note_occurency + "] " + Title);
            }
            else
            {
                global::Dispatcher.Notifier not = new global::Dispatcher.Notifier(Title, Description, false, Type, Caller: null);
                not.Show();
                global::Dispatcher.Notifier.Notifiers.Add(not);
            }
            return global::Dispatcher.Notifier.ID;
        }

        public static global::System.Windows.Forms.DialogResult ShowDialog(string Description, global::Dispatcher.Form.Type Type, string Title, global::System.Windows.Forms.Form obscureMe = null)
        {
            string outinput = string.Empty;
            return global::Dispatcher.Notifier.ShowDialog(Description, Type, Title, out outinput, obscureMe: obscureMe);
        }

        public static void Show(string Description, string Title, global::Dispatcher.Notifier.Type Type = global::Dispatcher.Notifier.Type.INFO) { global::Dispatcher.Notifier.Show(Type, Description, Title); }
        public static void Show(string Description) { global::Dispatcher.Notifier.Show(Description, "Notifier", Type: global::Dispatcher.Notifier.Type.INFO); }
        public static global::System.Windows.Forms.DialogResult ShowDialog(string Description, string Title, global::Dispatcher.Form.Type Type = global::Dispatcher.Form.Type.INFO) { return global::Dispatcher.Notifier.ShowDialog(Description, Type, Title, obscureMe: null); }
        public static global::System.Windows.Forms.DialogResult ShowDialog(string Description) { return global::Dispatcher.Notifier.ShowDialog(Description, "Notifier", Type: global::Dispatcher.Form.Type.INFO); }
        private void buttonMenu_Click(object sender, global::System.EventArgs e) { this.menu.Show(this.buttonMenu, new global::System.Drawing.Point(0, this.buttonMenu.Height)); }
        private void closeAllToolStripMenuItem_Click(object sender, global::System.EventArgs e) { global::Dispatcher.Notifier.CloseAll(); }

        private void SetEvent(global::Dispatcher.Form.Type Type, global::System.Drawing.Color Color, global::System.Drawing.Bitmap Image)
        {
            this.icon.Image = Image;
            this.SetEvent(Color);
            this.type = Type;
        }

        protected override void SetEvent(global::Dispatcher.Form.Type Type)
        {
            switch (Type)
            {
                case global::Dispatcher.Form.Type.ERROR: this.SetEvent(Type, global::Dispatcher.Icons.Red, global::Dispatcher.Form.Icons.Error); break;
                case global::Dispatcher.Form.Type.WARNING: this.SetEvent(Type, global::Dispatcher.Icons.Yellow, global::Dispatcher.Form.Icons.Warning); break;
                case global::Dispatcher.Form.Type.OK: this.SetEvent(Type, global::Dispatcher.Icons.Green, global::Dispatcher.Form.Icons.OK); break;
                case global::Dispatcher.Form.Type.INFO:
                case global::Dispatcher.Form.Type.CLIENT: this.SetEvent(global::Dispatcher.Form.Type.INFO, global::Dispatcher.Icons.Blue, global::Dispatcher.Form.Icons.Info); break;
                default: this.SetEvent(Type, global::Dispatcher.Icons.Gray, global::Dispatcher.Form.Icons.Query); break;
            }
            this.type = Type;
        }

        private void setNotifier(global::Dispatcher.Form.Type Type, bool isUpdate)
        {
            if (!this.isDiag && (Type == global::Dispatcher.Form.Type.INPUT || Type == global::Dispatcher.Form.Type.CONFIRM || Type == global::Dispatcher.Form.Type.CLIENT)) { Type = global::Dispatcher.Form.Type.INFO; }
            this.SetEvent(Type);
            if (!isUpdate)
            {
                global::System.Drawing.Rectangle rec = global::System.Windows.Forms.Screen.GetWorkingArea(global::System.Windows.Forms.Screen.PrimaryScreen.Bounds);
                if (this.isDiag)
                {
                    int btnP = (global::Dispatcher.Notifier.NotifierButtonHeight + global::Dispatcher.Form.ControlMargin);
                    this.Size = new global::System.Drawing.Size(this.Size.Width, (this.Size.Height + btnP));
                    int Tp = (this.Height - btnP);
                    if (Type == global::Dispatcher.Form.Type.CONFIRM || Type == global::Dispatcher.Form.Type.INPUT)
                    {
                        int Mid = 0;
                        if (Type == global::Dispatcher.Form.Type.INPUT)
                        {
                            Mid = (global::Dispatcher.Form.ControlMargin + (int)global::Dispatcher.Icons.IconSize + 12); //REUSE
                            this.inputbox = this.InitTextBox("inputbox", Mid, (Tp - (global::Dispatcher.Form.StandardControlHeight + global::Dispatcher.Form.ControlMargin)), (this.Width - (Mid + global::Dispatcher.Form.ControlMargin)), global::Dispatcher.Form.StandardControlHeight, true, Text: "", MaxLength: 0, Multiline: false, ForeColor: default(global::System.Drawing.Color), BackColor: default(global::System.Drawing.Color));
                        }
                        Mid = (this.Width / 2);
                        this.AcceptButton = this.InitButton("accept", (Mid - (global::Dispatcher.Form.StandardButtonWidth + global::Dispatcher.Form.ControlMargin)), Tp, "Yes", true, Width: global::Dispatcher.Form.StandardButtonWidth, Height: global::Dispatcher.Notifier.NotifierButtonHeight, Handler: this.OnClose);
                        this.CancelButton = this.InitButton("cancel", (Mid + global::Dispatcher.Form.ControlMargin), Tp, "No", true, Width: global::Dispatcher.Form.StandardButtonWidth, Height: global::Dispatcher.Notifier.NotifierButtonHeight, Handler: this.OnClose);
                        this.Controls.Add(this.CancelButton as global::System.Windows.Forms.Button);
                    } else { this.AcceptButton = this.InitButton("accept", ((this.Width - global::Dispatcher.Form.StandardButtonWidth) / 2), Tp, "OK", true, Width: global::Dispatcher.Form.StandardButtonWidth, Height: global::Dispatcher.Notifier.NotifierButtonHeight, Handler: this.OnClose); }
                    if (this.obscure == null) { this.Pos = new global::Dispatcher.Notifier.PosXY(((rec.Width - this.Width) / 2), ((rec.Height - this.Height) / 2)); }
                    else { this.Pos = new global::Dispatcher.Notifier.PosXY(((this.obscure.Location.X + this.obscure.Size.Width / 2) - this.Width / 2), ((this.obscure.Location.Y + this.obscure.Size.Height / 2) - this.Height / 2)); }
                }
                else
                {
                    int maxNot = rec.Height / this.Height;
                    int x_Pos = rec.Width - this.Width;
                    int x_Shift = 25;
                    int n_columns = 0;
                    int n_max_columns = rec.Width / x_Shift;
                    bool add = false;
                    this.Pos = new global::Dispatcher.Notifier.PosXY(x_Pos, rec.Height - (this.Height * 1));
                    while (!add)
                    {
                        for (int n_not = 1; n_not <= maxNot; n_not++)
                        {
                            this.Pos = new global::Dispatcher.Notifier.PosXY(x_Pos, rec.Height - (this.Height * n_not));
                            if (!this.posXYContains()) { add = true; break; }
                            if (n_not == maxNot)
                            {
                                n_columns++;
                                n_not = 0;
                                x_Pos = rec.Width - this.Width - x_Shift * n_columns;
                            }
                            if (n_columns >= n_max_columns) { add = true; break; }
                        }
                    }
                }
                this.Location = new global::System.Drawing.Point(this.Pos.X, this.Pos.Y);
                global::Dispatcher.Notifier.posXY.Add(this.Pos);
            }
        }

        protected override void CreateComponents(bool Minimizable, int Width, int Height)
        {
            base.CreateComponents(Minimizable, Width, Height);
            this.labelDesc = this.InitControl<global::System.Windows.Forms.Label>("labelDesc", 42, 30, (Width - 54), (Height - 55), true, ForeColor: default(global::System.Drawing.Color), BackColor: default(global::System.Drawing.Color));
            this.labelDesc.TextAlign = global::System.Drawing.ContentAlignment.MiddleLeft;
            this.icon = this.InitControl<global::System.Windows.Forms.PictureBox>("icon", 12, 54, global::Dispatcher.Icons.IconSize, global::Dispatcher.Icons.IconSize, true, ForeColor: default(global::System.Drawing.Color), BackColor: default(global::System.Drawing.Color));
            this.icon.BackgroundImageLayout = global::System.Windows.Forms.ImageLayout.Stretch;
            this.icon.Image = global::Dispatcher.Notifier.Icons.Check;
            this.closeAllToolStripMenuItem = new global::System.Windows.Forms.ToolStripMenuItem();
            this.closeAllToolStripMenuItem.Font = this.Font;
            this.closeAllToolStripMenuItem.Name = "closeAllToolStripMenuItem";
            this.closeAllToolStripMenuItem.Size = new global::System.Drawing.Size(120, 22);
            this.closeAllToolStripMenuItem.Text = "Close All";
            this.closeAllToolStripMenuItem.Click += new global::System.EventHandler(this.closeAllToolStripMenuItem_Click);
            this.menu = new global::System.Windows.Forms.ContextMenuStrip(this.components);
            this.menu.SuspendLayout();
            this.menu.Items.AddRange(new global::System.Windows.Forms.ToolStripItem[] { this.closeAllToolStripMenuItem });
            this.menu.Name = "menu";
            this.menu.Size = new global::System.Drawing.Size(120, 26);
            this.menu.ResumeLayout(false);
            this.buttonMenu.Click += new global::System.EventHandler(this.buttonMenu_Click);
            this.buttonMenu.ContextMenuStrip = this.menu;
            this.Name = "Toast";
            this.TopMost = true;
        }

        private Notifier(string Title, string Description, bool IsDialog, global::Dispatcher.Form.Type Type, global::System.Windows.Forms.Form Caller = null) : base(Title, 324, (128 - (global::Dispatcher.Form.StandardButtonHeight + (global::Dispatcher.Form.ControlMargin * 2))), false, false, Type)
        {
            this.isDiag = IsDialog;
            this.Description = Description;
            global::Dispatcher.Notifier.ID++;
            this.Tag = "__Notifier|" + global::Dispatcher.Notifier.ID.ToString("X4");
            this.obscure = Caller;
            this.setNotifier(this.type, false);
        }
    }

    public sealed class Mailer
    {
        public string SenderUser { get; set; }
        public string SenderPass { get; set; }
        public string SenderHost { get; set; }
        public int SenderPort { get; set; }
        public int SendInterval { get; set; }

        public static string ConfigPath()
        {
            string Path = global::System.Environment.GetFolderPath(global::System.Environment.SpecialFolder.ApplicationData);
            if (string.IsNullOrEmpty(Path)) { throw new global::System.Exception("Error - Path to AppData not Found"); }
            else
            {
                if (!Path.EndsWith("\\")) { Path += "\\"; }
                return Path;
            }
        }

        private string ConfigFile()
        {
            string Path = global::Dispatcher.Mailer.ConfigPath();
            if (string.IsNullOrEmpty(Path)) { return string.Empty; }
            else
            {
                Path += "mailer.ini";
                return Path;
            }
        }

        public static global::System.Net.Mail.SmtpClient NewClient(string Host, string User, string Pass, int Port)
        {
            global::System.Net.Mail.SmtpClient cli = new global::System.Net.Mail.SmtpClient(Host, Port);
            cli.UseDefaultCredentials = false;
            cli.Credentials = new global::System.Net.NetworkCredential(User, Pass);
            cli.EnableSsl = (Port == 465 || Port == 587);
            return cli;
        }

        public global::System.Net.Mail.SmtpClient NewClient() { return global::Dispatcher.Mailer.NewClient(this.SenderHost, this.SenderUser, this.SenderPass, this.SenderPort); }

        public void Initialize(string Host, string User, string Pass, int Port, int Interval)
        {
            this.SenderHost = Host;
            this.SenderUser = User;
            this.SenderPass = Pass;
            this.SenderPort = Port;
            this.SendInterval = Interval;
        }

        public void Save()
        {
            string Tx = this.SenderHost + global::System.Environment.NewLine;
            Tx += this.SenderUser + global::System.Environment.NewLine;
            Tx += this.SenderPass + global::System.Environment.NewLine;
            Tx += this.SenderPort.ToString() + global::System.Environment.NewLine;
            Tx += this.SendInterval.ToString();
            global::System.IO.File.WriteAllText(this.ConfigFile(), Tx);
        }

        public void QueryUser()
        {
            global::Dispatcher.Container Frm = new global::Dispatcher.Container("Configuração SMTP", 480, 320, false, true, global::Dispatcher.Form.Type.CLIENT);
            int Y = 32;
            global::System.Windows.Forms.TextBox smtpHost = Frm.InitField("smtpHost", "Host (SMTP)", ref Y, false, LabelBackColor: default(global::System.Drawing.Color));
            smtpHost.Text = this.SenderHost;
            Frm.InitFieldLabel("smtpPort", "Porta (SMTP)", ref Y, LabelBackColor: default(global::System.Drawing.Color));
            global::System.Windows.Forms.NumericUpDown smtpPort = Frm.InitControl<global::System.Windows.Forms.NumericUpDown>("smtpPort", 6, Y, (Frm.Width - 12), 22, true, ForeColor: default(global::System.Drawing.Color), BackColor: default(global::System.Drawing.Color));
            smtpPort.Maximum = 65536;
            smtpPort.Value = (this.SenderPort < 1 ? 465 : this.SenderPort);
            Y += (22 + 3);
            global::System.Windows.Forms.TextBox smtpUser = Frm.InitField("smtpUser", "Usuário (SMTP)", ref Y, false, LabelBackColor: default(global::System.Drawing.Color));
            smtpUser.Text = this.SenderUser;
            global::System.Windows.Forms.TextBox smtpPass = Frm.InitField("smtpPass", "Senha (SMTP)", ref Y, false, LabelBackColor: default(global::System.Drawing.Color));
            smtpPass.Text = this.SenderPass;
            Frm.InitFieldLabel("intvPick", "Intervalo de Envio entre Mensagens", ref Y, LabelBackColor: default(global::System.Drawing.Color));
            global::System.Windows.Forms.DateTimePicker intvPick = Frm.InitControl<global::System.Windows.Forms.DateTimePicker>("intvPick", 6, Y, (Frm.Width - 12), 22, true, ForeColor: default(global::System.Drawing.Color), BackColor: default(global::System.Drawing.Color));
            intvPick.Format = global::System.Windows.Forms.DateTimePickerFormat.Time;
            global::System.TimeSpan min = new global::System.TimeSpan(0, 1, 0);
            intvPick.MinDate = global::System.DateTime.MinValue;
            intvPick.Value = intvPick.MinDate.AddTicks(min.Ticks);
            intvPick.MaxDate = intvPick.MinDate.Add(new global::System.TimeSpan(23, 59, 59));
            intvPick.ShowUpDown = true;
            global::System.EventHandler acceptMethod = new global::System.EventHandler((sender, e) => 
            {
                if (string.IsNullOrEmpty(smtpHost.Text)) { global::Dispatcher.Notifier.ShowDialog("Campo \"Host\" Inválido!", "Valor Inválido", global::Dispatcher.Form.Type.ERROR); }
                else if (smtpPort.Value < 1) { global::Dispatcher.Notifier.ShowDialog("Campo \"Porta\" Inválido!", "Valor Inválido", global::Dispatcher.Form.Type.ERROR); }
                else if (string.IsNullOrEmpty(smtpUser.Text)) { global::Dispatcher.Notifier.ShowDialog("Campo \"Usuário\" Inválido!", "Valor Inválido", global::Dispatcher.Form.Type.ERROR); }
                else if (string.IsNullOrEmpty(smtpPass.Text)) { global::Dispatcher.Notifier.ShowDialog("Campo \"Senha\" Inválido!", "Valor Inválido", global::Dispatcher.Form.Type.ERROR); }
                else
                {
                    this.SenderHost = smtpHost.Text;
                    this.SenderPort = (int)smtpPort.Value;
                    this.SenderUser = smtpUser.Text;
                    this.SenderPass = smtpPass.Text;
                    if (intvPick.Value.TimeOfDay < min) { this.SendInterval = (int)((new global::System.TimeSpan(0, 1, 0)).TotalMilliseconds); } else { this.SendInterval = (int)((intvPick.Value.TimeOfDay).TotalMilliseconds); }
                    this.Save();
                    Frm.Close();
                }
            });
            Frm.InitButton("acceptCfg", (Frm.Width - (global::Dispatcher.Form.StandardButtonWidth + 6)), (Frm.Height - (global::Dispatcher.Form.StandardButtonHeight + 6)), "Aceitar", true, Width: global::Dispatcher.Form.StandardButtonWidth, Height: global::Dispatcher.Form.StandardButtonHeight, Handler: acceptMethod);
            Frm.ShowDialog();
        }

        public void Initialize(bool QueryTheUser)
        {
            string AUX = this.ConfigFile();
            if (!string.IsNullOrEmpty(AUX) && global::System.IO.File.Exists(AUX))
            {
                string[] L = global::System.IO.File.ReadAllLines(AUX);
                AUX = string.Empty;
                if (L != null && L.Length == 5) { this.Initialize(L[0], L[1], L[2], int.Parse(L[3]), int.Parse(L[4])); } else if (QueryTheUser) { this.QueryUser(); }
            } else if (QueryTheUser) { this.QueryUser(); }
        }

        public Mailer(string Host, string User, string Pass, int Port, int Interval) { this.Initialize(Host, User, Pass, Port, Interval); }
        public Mailer() { this.Initialize(true); }

        public Mailer(bool QueryTheUser)
        {
            this.SendInterval = (int)((new global::System.TimeSpan(0, 1, 0)).TotalMilliseconds);
            this.Initialize(false);
            if (QueryTheUser) { this.QueryUser(); }
        }
    }

    public sealed class Worker : global::System.IDisposable
    {
        public delegate void WorkerHandler(global::Dispatcher.Worker worker);

        public interface IOwner
        {
            void Notify(string Message);
            void Ended();
            event global::Dispatcher.Worker.WorkerHandler AbortWorker;
        }

        public global::Dispatcher.Mailer Mailer { get; private set; }
        private bool AbortCalled;
        private global::Dispatcher.Worker.IOwner Owner;
        private global::System.Net.Mail.SmtpClient Client;
        private global::System.IO.StreamReader File;
        private global::System.Threading.Thread Thread;
        private char sep;
        public bool IsOpen { get { return (this.File != null); } }
        public bool IsWorking { get { return (this.Thread != null); } }
        public int CurrentLine { get; private set; }
        public int ValidLines { get; private set; }
        public int SourceLines { get; private set; }
        public char Separator { get { return this.sep; } set { if (!this.IsOpen) { this.sep = value; } } }
        private void NoForm() { global::Dispatcher.Notifier.ShowDialog("Form Não Definido!", "Erro", global::Dispatcher.Form.Type.ERROR); }
        private void AbortListener(global::Dispatcher.Worker worker) { this.Abort = true; }

        private void ThreadAbort()
        {
            if (this.Thread != null)
            {
                try { this.Thread.Abort(); } catch { /* NOTHING */ }
                this.Thread = null;
            }
        }

        public void Close()
        {
            this.ThreadAbort();
            this.CurrentLine = 0;
            this.ValidLines = 0;
            this.SourceLines = 0;
            if (this.File != null)
            {
                this.File.Close();
                this.File = null;
            }
            this.AbortCalled = false;
            if (this.Client != null) { this.Client = null; }
        }

        void global::System.IDisposable.Dispose()
        {
            this.Owner = null;
            this.Mailer = null;
            this.Close();
        }

        public bool Abort
        {
            get { return this.AbortCalled; }
            set
            {
                this.AbortCalled = true;
                this.Close();
                if (this.Owner != null) { this.Owner.Ended(); }
            }
        }

        public void Open(global::Dispatcher.Worker.IOwner Caller, string FilePath)
        {
            this.Close();
            if (Caller == null) { this.NoForm(); }
            else if (string.IsNullOrEmpty(FilePath)) { global::Dispatcher.Notifier.ShowDialog("Caminho Inválido!", "Erro", global::Dispatcher.Form.Type.ERROR); }
            else
            {
                Caller.Notify("Lendo Arquivo...");
                global::System.Windows.Forms.Application.DoEvents();
                FilePath = FilePath.ToLower();
                if (!FilePath.EndsWith(".csv")) { global::Dispatcher.Notifier.ShowDialog("Arquivo deve ter a extensão \"csv\"!", "Erro", global::Dispatcher.Form.Type.ERROR); }
                else if (!global::System.IO.File.Exists(FilePath)) { global::Dispatcher.Notifier.ShowDialog("Arquivo Não Existe!", "Erro", global::Dispatcher.Form.Type.ERROR); }
                else
                {
                    string JobPath = (global::Dispatcher.Mailer.ConfigPath() + "work.csv");
                    using (global::System.IO.TextWriter writer = new global::System.IO.StreamWriter(JobPath, false))
                    using (global::System.IO.TextReader reader = new global::System.IO.StreamReader(FilePath))
                    {
                        FilePath = reader.ReadLine(); //REUSE
                        string[] aux = null;
                        global::System.Text.StringBuilder b = new global::System.Text.StringBuilder(); //AUX
                        global::System.Collections.Generic.List<string> row = new global::System.Collections.Generic.List<string>(7);
                        while (FilePath != null && FilePath != string.Empty)
                        {
                            if (global::Dispatcher.CSV.Tokenize(FilePath, this.Separator, b, row) && row.Count == 7)
                            {
                                try //"To" argument
                                {
                                    row[0] = row[0].ToLower();
                                    aux = row[0].Split(new char[] { ';' }, global::System.StringSplitOptions.RemoveEmptyEntries); //Mail split - is always ";"
                                    foreach (string arg in aux) { if (!arg.Contains("@") && arg.StartsWith("@") && arg.EndsWith("@")) { continue; } }
                                } catch { continue; }
                                try //"Cc" argument
                                {
                                    row[1] = row[1].ToLower();
                                    aux = row[1].Split(new char[] { ';' }, global::System.StringSplitOptions.RemoveEmptyEntries); //Mail split - is always ";"
                                    foreach (string arg in aux) { if (!arg.Contains("@") && arg.StartsWith("@") && arg.EndsWith("@")) { continue; } }
                                } catch { continue; }
                                row[2] = row[2].Trim(); //"Summary" argument
                                row[3] = row[3].Trim(); //"Body" argument
                                if (string.IsNullOrEmpty(row[4]) || !global::System.IO.File.Exists(row[4])) { row[4] = "NULL"; } else { row[4] = row[4].ToLower(); } //"Attachment 1" argument
                                if (string.IsNullOrEmpty(row[5]) || !global::System.IO.File.Exists(row[5])) { row[4] = "NULL"; } else { row[4] = row[5].ToLower(); } //"Attachment 2" argument
                                if (string.IsNullOrEmpty(row[6]) || !global::System.IO.File.Exists(row[6])) { row[4] = "NULL"; } else { row[6] = row[6].ToLower(); } //"Attachment 3" argument
                                FilePath = "\"" + row[0] + "\"" + this.Separator + "\"" + row[1] + "\"" + this.Separator + "\"" + row[2] + "\"" + this.Separator + "\"" + row[3] + "\"" + this.Separator + "\"" + row[4] + "\"" + this.Separator + "\"" + row[5] + "\"" + this.Separator + "\"" + row[6] + "\"" + this.Separator;
                                writer.WriteLine(FilePath);
                                this.ValidLines++;
                            }
                            this.SourceLines++;
                            FilePath = reader.ReadLine(); //REUSE
                        }
                    }
                    this.Owner = Caller;
                    this.Owner.AbortWorker += this.AbortListener;
                    this.File = new global::System.IO.StreamReader(JobPath);
                    string Msg = "Lida(s) " + this.SourceLines.ToString() + " e " + ((this.SourceLines == this.ValidLines) ? "Todas" : ("Apenas " + this.ValidLines.ToString())) + " eram Válidas." + global::System.Environment.NewLine;
                    Msg += "";
                    this.Owner.Notify(Msg);
                }
            }
        }

        private bool Send()
        {
            try
            {
                global::System.Collections.Generic.List<string> row = new global::System.Collections.Generic.List<string>(7);
                global::Dispatcher.CSV.Tokenize(this.File.ReadLine(), this.Separator, new global::System.Text.StringBuilder(), row);
                this.CurrentLine++;
                global::System.Net.Mail.MailMessage MailToSend = new global::System.Net.Mail.MailMessage();
                foreach (string arg in row[0].Split(new char[] { ';' }, global::System.StringSplitOptions.RemoveEmptyEntries)) { MailToSend.To.Add(arg); }
                foreach (string arg in row[1].Split(new char[] { ';' }, global::System.StringSplitOptions.RemoveEmptyEntries)) { MailToSend.CC.Add(arg); }
                MailToSend.Subject = row[2];
                MailToSend.Body = row[3];
                MailToSend.IsBodyHtml = true;
                MailToSend.From = new global::System.Net.Mail.MailAddress(this.Mailer.SenderUser);
                if (!string.IsNullOrEmpty(row[4]) && row[4] != "NULL") { MailToSend.Attachments.Add(new global::System.Net.Mail.Attachment(row[4])); }
                if (!string.IsNullOrEmpty(row[5]) && row[5] != "NULL") { MailToSend.Attachments.Add(new global::System.Net.Mail.Attachment(row[5])); }
                if (!string.IsNullOrEmpty(row[6]) && row[6] != "NULL") { MailToSend.Attachments.Add(new global::System.Net.Mail.Attachment(row[6])); }
                this.Client.Send(MailToSend);
                try { if (this.Owner != null) { this.Owner.Notify("Processadas " + this.CurrentLine.ToString() + " de " + this.ValidLines.ToString()); } } catch { /* NOTHING */ }
                return true;
            } catch { return false; }
        }

        private void ExecuteJob()
        {
            while (this.CurrentLine < this.ValidLines)
            {
                if (this.AbortCalled) { break; }
                else if (!this.Send())
                {
                    if (this.Owner == null) { global::Dispatcher.Notifier.ShowDialog("Erro ao Enviar Mensagem " + this.CurrentLine.ToString() + " de " + this.ValidLines.ToString(), "Erro", global::Dispatcher.Form.Type.ERROR); } else { this.Owner.Notify("Erro ao Enviar Mensagem " + this.CurrentLine.ToString() + " de " + this.ValidLines.ToString()); }
                    break;
                }
                else
                {
                    global::System.Windows.Forms.Application.DoEvents();
                    if (this.AbortCalled) { break; }
                    global::System.Threading.Thread.Sleep(this.Mailer.SendInterval);
                }
            }
            if (this.Owner != null) { this.Owner.Ended(); }
            this.ThreadAbort();
        }

        public void Start()
        {
            if (this.Owner == null) { this.NoForm(); }
            else if (!this.IsOpen) { this.Owner.Notify("Abra um arquivo para o Trabalho."); }
            else
            {
                this.ThreadAbort();
                this.Client = this.Mailer.NewClient();
                this.Thread = new global::System.Threading.Thread(this.ExecuteJob);
                this.Thread.IsBackground = true;
                this.Thread.Start();
            }
        }

        public Worker(char Separator, global::Dispatcher.Mailer Mailer)
        {
            this.Separator = Separator;
            this.Mailer = Mailer;
        }

        public Worker(char Separator) : this(Separator, new global::Dispatcher.Mailer()) { /* NOTHING */ }
        public Worker() : this(';') { /* NOTHING */ }
    }

    internal sealed class Program : global::Dispatcher.Container, global::Dispatcher.Worker.IOwner
    {
        private static global::Dispatcher.Mailer mailer = null;
        private static global::Dispatcher.Worker worker = null;
        private global::System.Windows.Forms.Button openConfig;
        private global::System.Windows.Forms.TextBox csvPath;
        private global::System.Windows.Forms.TextBox csvSep;
        private global::System.Windows.Forms.TextBox messages;
        private global::System.Windows.Forms.Button abortTask;
        private global::System.Windows.Forms.Button startTask;
        public event global::Dispatcher.Worker.WorkerHandler AbortWorker;
        private void OnAbort() { if (this.AbortWorker != null) { this.AbortWorker(global::Dispatcher.Program.worker); } }
        private void NotifyOut(string Message) { global::Dispatcher.Notifier.ShowDialog(Message, "Erro", global::Dispatcher.Form.Type.ERROR); }
        void global::Dispatcher.Worker.IOwner.Notify(string Message) { if (global::Dispatcher.Program.worker == null || (global::Dispatcher.Program.worker.IsOpen && !global::Dispatcher.Program.worker.IsWorking)) { global::Dispatcher.Notifier.ShowDialog(Message, "Aviso", global::Dispatcher.Form.Type.INFO); } else { this.messages.Text = Message; } }

        private void EndWorker()
        {
            (global::Dispatcher.Program.worker as global::System.IDisposable).Dispose();
            global::Dispatcher.Program.worker = null;
        }

        void global::Dispatcher.Worker.IOwner.Ended()
        {
            if (global::Dispatcher.Program.worker != null)
            {
                this.messages.Text = "Feitos " + global::Dispatcher.Program.worker.CurrentLine.ToString() + " de " + global::Dispatcher.Program.worker.ValidLines.ToString() + " ao fim da Tarefa";
                this.EndWorker();
            }
            this.abortTask.Enabled = false;
            this.startTask.Enabled = true;
            this.csvPath.ReadOnly = false;
        }

        private void OnStart()
        {
            if (string.IsNullOrEmpty(this.csvPath.Text)) { this.NotifyOut("Campo com Caminho para o Arquivo CSV vazio!"); }
            else if (string.IsNullOrEmpty(this.csvSep.Text)) { this.NotifyOut("Campo informando o Separador usado pelo Arquivo CSV vazio!"); }
            else
            {
                if (global::Dispatcher.Program.mailer == null) { global::Dispatcher.Program.mailer = new global::Dispatcher.Mailer(); }
                if (global::Dispatcher.Program.worker == null) { global::Dispatcher.Program.worker = new global::Dispatcher.Worker(this.csvSep.Text[0], global::Dispatcher.Program.mailer); }
                global::Dispatcher.Program.worker.Open(this, this.csvPath.Text);
                if (global::Dispatcher.Program.worker.IsOpen)
                {
                    if (global::System.Windows.Forms.DialogResult.Yes == global::Dispatcher.Notifier.ShowDialog("Confirma o início do Processo de envio?", "Confirmar", Type: global::Dispatcher.Form.Type.CONFIRM))
                    {
                        this.abortTask.Enabled = true;
                        this.startTask.Enabled = false;
                        this.csvPath.ReadOnly = true;
                        global::Dispatcher.Program.worker.Start();
                    } else { this.EndWorker(); }
                }
                else
                {
                    this.EndWorker();
                    this.NotifyOut("Falha na Abertura do arquivo fonte!"); 
                }
            }
        }

        private void PickFile()
        {
            if (!this.csvPath.ReadOnly)
            {
                global::System.Windows.Forms.OpenFileDialog dlg = new global::System.Windows.Forms.OpenFileDialog();
                if (dlg.ShowDialog() == global::System.Windows.Forms.DialogResult.OK)
                {
                    string sFileName = dlg.FileName.ToLower();
                    if (sFileName != null && sFileName.EndsWith(".csv")) { this.csvPath.Text = sFileName; }
                }
            }
        }

        private void Form_DragEnter(object sender, global::System.Windows.Forms.DragEventArgs e) { if (e.Data.GetDataPresent(global::System.Windows.Forms.DataFormats.FileDrop)) e.Effect = global::System.Windows.Forms.DragDropEffects.Copy; else e.Effect = global::System.Windows.Forms.DragDropEffects.None; }

        private void Form_DragDrop(object sender, global::System.Windows.Forms.DragEventArgs e)
        {
            if (!this.csvPath.ReadOnly)
            {
                string[] dropped = ((string[])e.Data.GetData(global::System.Windows.Forms.DataFormats.FileDrop));
                if (dropped != null && dropped.Length == 1)
                {
                    dropped[0] = dropped[0].ToLower();
                    if (dropped[0].EndsWith(".csv")) { this.csvPath.Text = dropped[0]; }
                }
            }
        }

        [global::System.STAThread] internal static int Main(string[] Args)
        {
            global::System.Windows.Forms.Application.Run(new global::Dispatcher.Program(Args));
            return 0;
        }

        public Program(string[] Args) : base()
        {
            this.InitializeContainer("Dispacher", 600, 340, false, true, global::Dispatcher.Form.Type.CLIENT);
            this.AllowDrop = true;
            this.DragEnter += this.Form_DragEnter;
            this.DragDrop += this.Form_DragDrop;
            int Y = 32;
            this.openConfig = this.InitButton("openConfig", 6, Y, "Configurações", true, Width: (global::Dispatcher.Form.StandardButtonWidth * 2), Height: global::Dispatcher.Form.StandardButtonHeight, Handler: new global::System.EventHandler((sender, e) => { if (global::Dispatcher.Program.mailer == null) { global::Dispatcher.Program.mailer = new global::Dispatcher.Mailer(true); } else { global::Dispatcher.Program.mailer.QueryUser(); } }));
            Y += (global::Dispatcher.Form.StandardButtonHeight + 6);
            this.csvPath = this.InitField("csvPath", "Caminho para o Arquivo CSV", ref Y, false, LabelBackColor: default(global::System.Drawing.Color));
            this.csvPath.DoubleClick += (sender, e) => { this.PickFile(); };
            this.csvSep = this.InitField("csvSep", "Separador das Colunas do Arquivo (CSV)", ref Y, false, LabelBackColor: default(global::System.Drawing.Color));
            this.csvSep.MaxLength = 1;
            this.csvSep.Text = ",";
            Y += (this.InitLabel("infoLabel", 6, Y, (this.Width - 12), 66, true, "O Arquivo deve ser no Formato CSV, representando as colunas na ordem: e-mail(s) do campo \"Para\" (se mais de 1, separado por \";\"), e-mail(s) do campo \"Cópia\" (se mais de 1, separado por \";\"), assunto do e-mail, corpo do e-mail, endereço do anexo (caminho completo para o arquivo) - se houver, e até 3 anexos (1 por coluna)", ForeColor: global::Dispatcher.Icons.Red, BackColor: this.BackColor).Height + 6);
            this.messages = this.InitField("messages", "Mensagens de Status", ref Y, true, LabelBackColor: default(global::System.Drawing.Color));
            this.messages.ReadOnly = true;
            Y = (this.Height - (global::Dispatcher.Form.StandardButtonHeight + 6));
            this.abortTask = this.InitButton("abortTask", 6, Y, "Abortar!", true, Width: (global::Dispatcher.Form.StandardButtonWidth * 2), Height: global::Dispatcher.Form.StandardButtonHeight, Handler: new global::System.EventHandler((sender, e) => { this.OnAbort(); }));
            this.abortTask.Enabled = false;
            this.startTask = this.InitButton("startTask", (this.Width - (6 + (global::Dispatcher.Form.StandardButtonWidth * 2))), Y, "Iniciar", true, Width: (global::Dispatcher.Form.StandardButtonWidth * 2), Height: global::Dispatcher.Form.StandardButtonHeight, Handler: new global::System.EventHandler((sender, e) => { this.OnStart(); }));
            if (Args != null && Args.Length > 0) { this.csvPath.Text = Args[0]; }
        }
    }
}
