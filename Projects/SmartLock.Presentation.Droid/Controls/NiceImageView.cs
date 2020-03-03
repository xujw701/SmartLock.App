using System;
using Android.Content;
using Android.Graphics;
using Android.OS;
using Android.Runtime;
using Android.Support.V7.Widget;
using Android.Util;
using SmartLock.Presentation.Droid.Support;

namespace SmartLock.Presentation.Droid.Controls
{
    public class NiceImageView : AppCompatImageView
    {
        private Context _context;

        private bool _isCircle; // 是否显示为圆形，如果为圆形则设置的corner无效
        private bool _isCoverSrc; // border、inner_border是否覆盖图片
        private int _borderWidth; // 边框宽度
        private Color _borderColor = Color.White; // 边框颜色
        private int _innerBorderWidth; // 内层边框宽度
        private Color _innerBorderColor = Color.White; // 内层边框充色

        private int _cornerRadius; // 统一设置圆角半径，优先级高于单独设置每个角的半径
        private int _cornerTopLeftRadius; // 左上角圆角半径
        private int _cornerTopRightRadius; // 右上角圆角半径
        private int _cornerBottomLeftRadius; // 左下角圆角半径
        private int _cornerBottomRightRadius; // 右下角圆角半径

        private Xfermode _xfermode;

        private int _width;
        private int _height;
        private float _radius;

        private float[] _borderRadii;
        private float[] _srcRadii;

        private RectF _srcRectF; // 图片占的矩形区域
        private RectF _borderRectF; // 边框的矩形区域

        private Paint _paint;
        private Path _path; // 用来裁剪图片的ptah
        private Path _srcPath; // 图片区域大小的path

        public NiceImageView(Context context) : base(context)
        {
            _context = context;

            Init();
        }

        public NiceImageView(Context context, IAttributeSet attrs) : base(context, attrs)
        {
            _context = context;

            Init();
        }

        public NiceImageView(Context context, IAttributeSet attrs, int defStyleAttr) : base(context, attrs, defStyleAttr)
        {
            _context = context;

            Init();
        }

        protected NiceImageView(IntPtr javaReference, JniHandleOwnership transfer) : base(javaReference, transfer)
        {
            Init();
        }

        private void Init()
        {
            _isCoverSrc = true;
            _borderWidth = 3;
            _borderColor = new Color(188, 195, 207);
            _cornerRadius = 24;

            _borderRadii = new float[8];
            _srcRadii = new float[8];

            _borderRectF = new RectF();
            _srcRectF = new RectF();

            _paint = new Paint();
            _path = new Path();

            if (Build.VERSION.SdkInt <= BuildVersionCodes.OMr1)
            {
                _xfermode = new PorterDuffXfermode(PorterDuff.Mode.DstIn);
            }
            else
            {
                _xfermode = new PorterDuffXfermode(PorterDuff.Mode.DstOut);
                _srcPath = new Path();
            }

            CalculateRadii();
            ClearInnerBorderWidth();
        }

        public void IsCoverSrc(bool isCoverSrc)
        {
            _isCoverSrc = isCoverSrc;
            InitSrcRectF();
            Invalidate();
        }

        public void IsCircle(bool isCircle)
        {
            _isCircle = isCircle;
            ClearInnerBorderWidth();
            InitSrcRectF();
            Invalidate();
        }

        public void SetBorderWidth(int borderWidth)
        {
            _borderWidth = DisplayMetricsHelper.Dp2Px(_context, borderWidth);
            CalculateRadiiAndRectF(false);
        }

        public void SetBorderColor(Color borderColor)
        {
            _borderColor = borderColor;
            Invalidate();
        }

        public void setInnerBorderWidth(int innerBorderWidth)
        {
            _innerBorderWidth = DisplayMetricsHelper.Dp2Px(_context, innerBorderWidth);
            ClearInnerBorderWidth();
            Invalidate();
        }

        public void SetInnerBorderColor(Color innerBorderColor)
        {
            _innerBorderColor = innerBorderColor;
            Invalidate();
        }

        public void SetCornerRadius(int cornerRadius)
        {
            _cornerRadius = DisplayMetricsHelper.Dp2Px(_context, cornerRadius);
            CalculateRadiiAndRectF(false);
        }

        protected override void OnSizeChanged(int w, int h, int oldw, int oldh)
        {
            base.OnSizeChanged(w, h, oldw, oldh);

            _width = w;
            _height = h;

            InitBorderRectF();
            InitSrcRectF();
        }

        protected override void OnDraw(Canvas canvas)
        {
            // 使用图形混合模式来显示指定区域的图片
            canvas.SaveLayer(_srcRectF, null, SaveFlags.All);
            if (!_isCoverSrc)
            {
                float sx = 1.0f * (_width - 2 * _borderWidth - 2 * _innerBorderWidth) / _width;
                float sy = 1.0f * (_height - 2 * _borderWidth - 2 * _innerBorderWidth) / _height;
                // 缩小画布，使图片内容不被borders覆盖
                canvas.Scale(sx, sy, _width / 2.0f, _height / 2.0f);
            }

            base.OnDraw(canvas);

            _paint.Reset();
            _path.Reset();
            if (_isCircle)
            {
                _path.AddCircle(_width / 2.0f, _height / 2.0f, _radius, Path.Direction.Ccw);
            }
            else
            {
                _path.AddRoundRect(_srcRectF, _srcRadii, Path.Direction.Ccw);
            }

            _paint.AntiAlias = true;
            _paint.SetStyle(Paint.Style.Fill);
            _paint.SetXfermode(_xfermode);
            if (Build.VERSION.SdkInt <= BuildVersionCodes.OMr1)
            {
                canvas.DrawPath(_path, _paint);
            }
            else
            {
                _srcPath.AddRect(_srcRectF, Path.Direction.Ccw);
                // 计算tempPath和path的差集
                _srcPath.InvokeOp(_path, Path.Op.Difference);
                canvas.DrawPath(_srcPath, _paint);
            }
            _paint.SetXfermode(null);

            // 恢复画布
            canvas.Restore();
            // 绘制边框
            DrawBorders(canvas);
        }

        private void DrawBorders(Canvas canvas)
        {
            if (_isCircle)
            {
                if (_borderWidth > 0)
                {
                    DrawCircleBorder(canvas, _borderWidth, _borderColor, _radius - _borderWidth / 2.0f);
                }
                if (_innerBorderWidth > 0)
                {
                    DrawCircleBorder(canvas, _innerBorderWidth, _innerBorderColor, _radius - _borderWidth - _innerBorderWidth / 2.0f);
                }
            }
            else
            {
                if (_borderWidth > 0)
                {
                    DrawRectFBorder(canvas, _borderWidth, _borderColor, _borderRectF, _borderRadii);
                }
            }
        }

        private void DrawCircleBorder(Canvas canvas, int borderWidth, Color borderColor, float radius)
        {
            InitBorderPaint(borderWidth, borderColor);
            _path.AddCircle(_width / 2.0f, _height / 2.0f, radius, Path.Direction.Ccw);
            canvas.DrawPath(_path, _paint);
        }

        private void DrawRectFBorder(Canvas canvas, int borderWidth, Color borderColor, RectF rectF, float[] radii)
        {
            InitBorderPaint(borderWidth, borderColor);
            _path.AddRoundRect(rectF, radii, Path.Direction.Ccw);
            canvas.DrawPath(_path, _paint);
        }

        private void InitBorderPaint(int borderWidth, Color borderColor)
        {
            _path.Reset();
            _paint.StrokeWidth = borderWidth;
            _paint.Color = borderColor;
            _paint.SetStyle(Paint.Style.Stroke);
        }

        private void InitBorderRectF()
        {
            if (!_isCircle)
            {
                _borderRectF.Set(_borderWidth / 2.0f, _borderWidth / 2.0f, _width - _borderWidth / 2.0f, _height - _borderWidth / 2.0f);
            }
        }

        /**
         * 计算图片原始区域的RectF
         */
        private void InitSrcRectF()
        {
            if (_isCircle)
            {
                _radius = Math.Min(_width, _height) / 2.0f;
                _srcRectF.Set(_width / 2.0f - _radius, _height / 2.0f - _radius, _width / 2.0f + _radius, _height / 2.0f + _radius);
            }
            else
            {
                _srcRectF.Set(0, 0, _width, _height);
                if (_isCoverSrc)
                {
                    _srcRectF = _borderRectF;
                }
            }
        }

        /**
         * 计算RectF的圆角半径
         */
        private void CalculateRadii()
        {
            if (_isCircle)
            {
                return;
            }
            if (_cornerRadius > 0)
            {
                for (int i = 0; i < _borderRadii.Length; i++)
                {
                    _borderRadii[i] = _cornerRadius;
                    _srcRadii[i] = _cornerRadius - _borderWidth / 2.0f;
                }
            }
            else
            {
                _borderRadii[0] = _borderRadii[1] = _cornerTopLeftRadius;
                _borderRadii[2] = _borderRadii[3] = _cornerTopRightRadius;
                _borderRadii[4] = _borderRadii[5] = _cornerBottomRightRadius;
                _borderRadii[6] = _borderRadii[7] = _cornerBottomLeftRadius;

                _srcRadii[0] = _srcRadii[1] = _cornerTopLeftRadius - _borderWidth / 2.0f;
                _srcRadii[2] = _srcRadii[3] = _cornerTopRightRadius - _borderWidth / 2.0f;
                _srcRadii[4] = _srcRadii[5] = _cornerBottomRightRadius - _borderWidth / 2.0f;
                _srcRadii[6] = _srcRadii[7] = _cornerBottomLeftRadius - _borderWidth / 2.0f;
            }
        }
        private void CalculateRadiiAndRectF(bool reset)
        {
            if (reset)
            {
                _cornerRadius = 0;
            }
            CalculateRadii();
            InitBorderRectF();
            Invalidate();
        }

        /**
         * 目前圆角矩形情况下不支持inner_border，需要将其置0
         */
        private void ClearInnerBorderWidth()
        {
            if (!_isCircle)
            {
                _innerBorderWidth = 0;
            }
        }
    }
}