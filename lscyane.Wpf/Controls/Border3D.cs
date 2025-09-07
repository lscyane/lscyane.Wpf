using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace lscyane.Wpf.Controls
{
    /// <summary>
    /// 要素の周囲に立体表現の罫線を描きます。
    /// </summary>
    public class Border3D : Border
    {
        /// <summary>
        /// 罫線の種類
        /// </summary>
        public enum EMode
        {
            /// <summary> 凹面 </summary>
            Concave,
            /// <summary> 凸面 </summary>
            Convex,
        }


        /// <summary>
        /// 罫線の種類
        /// </summary>
        public EMode Mode
        {
            get; set;
        }


        /// <summary>
        /// コンストラクタ
        /// </summary>
        public Border3D()
        {
            this.Padding = BorderThickness; // コントロールの内部に描画するので罫線の太さの分のPaddingを設定する
            this.Background = SystemColors.WindowBrush;
        }


        /// <summary>
        /// 描画イベント
        /// </summary>
        /// <param name="dc"></param>
        protected override void OnRender(DrawingContext dc)
        {
            base.OnRender(dc);

            double wid = this.ActualWidth;
            double hei = this.ActualHeight;
            Thickness tc = BorderThickness;
            PointCollection points;

            // 上、左辺の描画
            points = new PointCollection();
            points.Add(new Point(0, 0));
            points.Add(new Point(wid, 0));
            points.Add(new Point(wid - tc.Right, tc.Top));
            points.Add(new Point(tc.Left, tc.Top));
            points.Add(new Point(tc.Left, hei - tc.Bottom));
            points.Add(new Point(0, hei));
            if (Mode == EMode.Concave)
            {
                this.drawPolygon(dc, points, Brushes.Gray);
            }
            else
            {
                this.drawPolygon(dc, points, Brushes.WhiteSmoke);
            }

            // 右、下辺の描画
            points = new PointCollection();
            points.Add(new Point(wid, hei));
            points.Add(new Point(0, hei));
            points.Add(new Point(tc.Left, hei - tc.Bottom));
            points.Add(new Point(wid - tc.Right, hei - tc.Bottom));
            points.Add(new Point(wid - tc.Right, tc.Top));
            points.Add(new Point(wid, 0));
            if (Mode == EMode.Concave)
            {
                this.drawPolygon(dc, points, Brushes.WhiteSmoke);
            }
            else
            {
                this.drawPolygon(dc, points, Brushes.Gray);
            }
        }


        /// <summary>
        /// 枠線の描画
        /// </summary>
        /// <param name="dc"></param>
        /// <param name="pc"></param>
        /// <param name="color"></param>
        private void drawPolygon(DrawingContext dc, PointCollection pc, Brush color)
        {
            StreamGeometry sg = new StreamGeometry();
            using (StreamGeometryContext geometryContext = sg.Open())
            {
                geometryContext.BeginFigure(pc[0], true, true);
                geometryContext.PolyLineTo(pc, true, true);
            }

            dc.DrawGeometry(color, null, sg);
        }

    }
}
