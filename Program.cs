using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

using _3dSharp;

ApplicationConfiguration.Initialize();

var form = new Form();

form.Size = new Size(1280, 720);

// form.WindowState = FormWindowState.Maximized;

Graphics g = null;
Bitmap bmp = null;

PictureBox pb = new PictureBox();
pb.Dock = DockStyle.Fill;
form.Controls.Add(pb);

var tm = new Timer();
tm.Interval = 5;

Pen pen = new Pen(Color.Black);

Point3d cameraPos = new Point3d(0, 0, 0);
double FOV = 1000;
Ratio ratio = new Ratio(16*100, 9*100, 720);
Angle angle = new Angle(0, 0, 0);
double ratioScale = 1;

Camera camera = new Camera(cameraPos, FOV, angle, ratio, ratioScale);

// _3dSharp.Panel panel = new _3dSharp.Panel(new Point3d(9000,-1000,-1000), new Point3d(11000,1000,-1000));
Cube cube = new Cube(new Point3d(9000, -1000, -1000), new Point3d(11000, 1000, 1000));
Cube cube2 = new Cube(new Point3d(12000, -1000, 2000), new Point3d(14000, 1000, 4000));

// camera.YawAdd(30);

tm.Tick += delegate
{
    List<Triangle2d> triangles = Scene.BruteRender(camera);

    var drawFont = new Font("Arial", 16);

    PointF drawPoint = new PointF(150.0F, 150.0F);
    PointF drawPoint2 = new PointF(150.0F, 200.0F);
    PointF drawPoint3 = new PointF(150.0F, 250.0F);
    PointF drawPoint4 = new PointF(150.0F, 300.0F);

    g.Clear(Color.White);

    g.DrawString($"Camera FOV: {camera.cameraView.FOVpoint}", drawFont, Brushes.Black, drawPoint);
    g.DrawString($"{triangles.Count} Triangles Rendered", drawFont, Brushes.Black, drawPoint2);
    g.DrawString($"Camera angles: {camera.cameraView.angle.yaw}, {camera.cameraView.angle.pitch}", drawFont, Brushes.Black, drawPoint3);
    g.DrawString($"Camera position: {camera.position}", drawFont, Brushes.Black, drawPoint4);

    // g.DrawLine(new Pen(Color.Red), new Point(0, Screen.PrimaryScreen.Bounds.Height/2), new Point(Screen.PrimaryScreen.Bounds.Width, Screen.PrimaryScreen.Bounds.Height/2));
    // g.DrawLine(new Pen(Color.Red), new Point(Screen.PrimaryScreen.Bounds.Width/2, 0), new Point(Screen.PrimaryScreen.Bounds.Width/2, Screen.PrimaryScreen.Bounds.Height));

    foreach (var triangle in triangles)
    {
        g.DrawPolygon(new Pen(Color.Black), triangle.points);
    }


    pb.Refresh();
};

form.Load += delegate
{
    bmp = new Bitmap(pb.Width, pb.Height);
    g = Graphics.FromImage(bmp);
    g.Clear(Color.White);
    pb.Image = bmp;
    tm.Start();
};

form.KeyPreview = true;
form.KeyDown += (s, e) =>
{
    if (e.KeyCode == Keys.Escape)
        Application.Exit();
    if (e.KeyCode == Keys.Left)
        camera.YawAdd(1);
    if (e.KeyCode == Keys.Right)
        camera.YawAdd(-1);
    if (e.KeyCode == Keys.Up)
        camera.PitchAdd(1);
    if (e.KeyCode == Keys.Down)
        camera.PitchAdd(-1);
    if (e.KeyCode == Keys.W)
        camera.Translate(100, 0, 0);
    if (e.KeyCode == Keys.S)
        camera.Translate(-100, 0, 0);
    if (e.KeyCode == Keys.A)
        camera.Translate(0, 0, -100);
    if (e.KeyCode == Keys.D)
        camera.Translate(0, 0, 100);
    if (e.KeyCode == Keys.Z)
        camera.Translate(0, -100, 0);
    if (e.KeyCode == Keys.Space)
        camera.Translate(0, 100, 0);
};

Application.Run(form);