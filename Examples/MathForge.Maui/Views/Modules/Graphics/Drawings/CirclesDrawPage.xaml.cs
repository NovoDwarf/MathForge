using MathForge.Maui.Views.Graphics;
using SkiaSharp;
using MathForge.Geometry.Primitives;
using MathForge.Graphics.Drawings.Circles;
using MathForge.Graphics.Drawings.Fillers;
using MathForge.LinearAlgebra.Vectors;

namespace MathForge.Maui.Views.Modules.Graphics.Drawings;

public partial class CirclesDrawPage : DrawPageBase
{
	public CirclesDrawPage()
	{
		InitializeComponent();
    }

	protected override GridCanvas? GetGridCanvas() => GridCanvas;
	
	private bool _waitingForStartPoint = true;

    private SKPoint? _centerPoint;
    private SKPoint? _radiusPoint;
    
    private void OnCanvasTapped(object? sender, TappedEventArgs e)
    {
        var pos = e.GetPosition(GridCanvas);

        if (pos == null) return;

        var gridPointI = GridCanvas.ScreenToGrid(new SKPoint((float)((Point)pos).X, (float)((Point)pos).Y));
        var gridPoint = new SKPoint(gridPointI.X, gridPointI.Y);

        if (_waitingForStartPoint)
        {
            _centerPoint = gridPoint;
            _waitingForStartPoint = false;
            GridCanvas.SetStartPoint(gridPoint);
        }
        else
        {
            _radiusPoint = gridPoint;
            _waitingForStartPoint = true;
            GridCanvas.SetEndPoint(gridPoint);

            if (_centerPoint == null || _radiusPoint == null)
                return;

            GridCanvas.SetReferenceLine((SKPoint)_centerPoint, (SKPoint)_radiusPoint);

            DrawCircle();
        }
    }
    
    private void OnDrawLineClicked(object sender, EventArgs e) => DrawCircle();

    private void DrawCircle()
    {
        if (_centerPoint == null || _radiusPoint == null) return;

        int x0 = (int)((SKPoint)_centerPoint).X;
        int y0 = (int)((SKPoint)_centerPoint).Y;
        int x1 = (int)((SKPoint)_radiusPoint).X;
        int y1 = (int)((SKPoint)_radiusPoint).Y;

        string algorithm = AlgorithmPicker.SelectedItem?.ToString() ?? "DDA";
        var radius = (int)Math.Round(Math.Sqrt((double)(x1 - x0) * (x1 - x0) + (double)(y1 - y0) * (y1 - y0)));
        var centerPoints = algorithm switch
        {
            "DDA" => DdaCircleRasterizer.Default.Rasterize(x0, y0, radius),
            "Брезенхем" => BresenhamCircleRasterizer.Default.Rasterize(x0, y0, radius),
            "Ву" => WuCircleRasterizer.Default.Rasterize(x0, y0, radius),
            _ => []
        };

        GridCanvas.PrepareNextGeneration();

        var enumerable = centerPoints.ToList();
        
        foreach (var p in enumerable)
            GridCanvas.AddPoint(new SKPoint(p.X, p.Y), p.Z);

        var fillAlgorithm = FillPicker.SelectedItem?.ToString();
        if (!string.IsNullOrEmpty(fillAlgorithm))
        {
            var seed = new Float2(x0, y0);
            var size = new Size2(
                Math.Max(1, (int)Math.Ceiling(GridCanvas.Width / GridCanvas.PixelScale)),
                Math.Max(1, (int)Math.Ceiling(GridCanvas.Height / GridCanvas.PixelScale)));
            var fillPoints = fillAlgorithm switch
            {
                "Flood Fill" => FloodFiller.Default.Fill(
                    seed, size, GridCanvas.IsEmpty
                ),
                "Seed Fill" => SeedFiller.Default.Fill(
                    seed, size, GridCanvas.IsEmpty
                ),
                "Scanline Fill" =>
                    ScanlineFiller.Default.Fill(
                        enumerable
                            .OrderBy(p => Math.Atan2(p.Y - y0, p.X - x0))
                            .Select(p => new Float2(p.X, p.Y))
                            .ToList(),
                        size
                    ),
                _ => null
            };

            if (fillPoints != null)
            {
                foreach (var fp in fillPoints)
                    GridCanvas.AddPoint(new SKPoint(fp.X, fp.Y), fp.Z);
            }
        }

        GridCanvas.InvalidateSurface();
    }
}
