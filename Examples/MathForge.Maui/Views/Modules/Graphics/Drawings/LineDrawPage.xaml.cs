using MathForge.Maui.Views.Graphics;
using SkiaSharp;
using MathForge.Graphics.Drawings.Lines;
using MathForge.LinearAlgebra.Vectors;

namespace MathForge.Maui.Views.Modules.Graphics.Drawings;

public partial class LineDrawPage : DrawPageBase
{
    public LineDrawPage()
    {
        InitializeComponent();
    }

    protected override GridCanvas? GetGridCanvas() => GridCanvas;

    private bool _waitingForStartPoint = true;

    private SKPoint? _startPoint;
    private SKPoint? _endPoint;
    
    private void OnCanvasTapped(object sender, TappedEventArgs e)
    {
        var pos = e.GetPosition(GridCanvas);

        if (pos == null) return;

        var gridPointI = GridCanvas.ScreenToGrid(new SKPoint((float)((Point)pos).X, (float)((Point)pos).Y));
        var gridPoint = new SKPoint(gridPointI.X, gridPointI.Y);

        if (_waitingForStartPoint)
        {
            _startPoint = gridPoint;
            _waitingForStartPoint = false;
            GridCanvas.SetStartPoint(gridPoint);
        }
        else
        {
            _endPoint = gridPoint;
            _waitingForStartPoint = true;
            GridCanvas.SetEndPoint(gridPoint);

            if (_startPoint == null || _endPoint == null)
                return;

            GridCanvas.SetReferenceLine((SKPoint)_startPoint, (SKPoint)_endPoint);

            DrawLine();
        }
    }
    
    private void OnDrawLineClicked(object sender, EventArgs e) => DrawLine();

    private void DrawLine()
    {
        if (_startPoint == null || _endPoint == null) return;

        var x0 = (int)((SKPoint)_startPoint).X;
        var y0 = (int)((SKPoint)_startPoint).Y;
        var x1 = (int)((SKPoint)_endPoint).X;
        var y1 = (int)((SKPoint)_endPoint).Y;

        var algorithm = AlgorithmPicker.SelectedItem?.ToString() ?? "DDA";
        var start = new Float2(x0, y0);
        var end = new Float2(x1, y1);

        var linePoints = algorithm switch
        {
            "Простейший пошаговый" => SimpleLineRasterizer.Default.Rasterize(start, end),
            "DDA" => DdaLineRasterizer.Default.Rasterize(start, end),
            "Брезенхем" => BresenhamLineRasterizer.Default.Rasterize(start, end),
            "Ву" => WuLineRasterizer.Default.Rasterize(start, end),
            _ => []
        };

        GridCanvas.PrepareNextGeneration();

        foreach (var p in linePoints)
        {
            GridCanvas.AddPoint(new SKPoint(p.X, p.Y), p.Z);
        }

        GridCanvas.InvalidateSurface();
    }
}
