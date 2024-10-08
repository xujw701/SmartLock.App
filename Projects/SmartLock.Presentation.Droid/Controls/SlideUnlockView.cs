﻿using System;
using Android.Animation;
using Android.Content;
using Android.Graphics;
using Android.Graphics.Drawables;
using Android.OS;
using Android.Util;
using Android.Views;
using Android.Views.Animations;
using SmartLock.Presentation.Droid.Support;

namespace SmartLock.Presentation.Droid.Controls
{
    public class SlideUnlockView : View
    {
        private const int DRAW_INTERVAL = 50;
        private const int STEP_LENGTH = 10;//速度

        private Handler mHandler = new Handler();

        private Paint mPaint;//文字的画笔
        private Paint mSliPaint;//滑块画笔
        private Paint mBgPaint;//背景画笔
        private Paint mUnlockedBgPaint;//背景画笔

        private VelocityTracker mVelocityTracker;//滑动速度
        private int mMaxVelocity;
        private LinearGradient mGradient;//渐变色
        private LinearGradient bgGradient;//背景渐变色
        private LinearGradient unlockedGgGradient;//背景渐变色

        private LinearGradient sliGradient;//滑块渐变色
                                           //    LinearGradient有两个构造函数;
                                           //
                                           //    public LinearGradient(float x0, float y0, float x1, float y1, int[] colors, float[] positions,Shader.TileMode tile)
                                           //
                                           //    参数:
                                           //
                                           //    float x0: 渐变起始点x坐标
                                           //
                                           //    float y0:渐变起始点y坐标
                                           //
                                           //    float x1:渐变结束点x坐标
                                           //
                                           //    float y1:渐变结束点y坐标
                                           //
                                           //    int[] colors:颜色 的int 数组
                                           //
                                           //    float[] positions: 相对位置的颜色数组,可为null,  若为null,可为null,颜色沿渐变线均匀分布
                                           //
                                           //    Shader.TileMode tile: 渲染器平铺模式
                                           //
                                           //
                                           //
                                           //    public LinearGradient(float x0, float y0, float x1, float y1, int color0, int color1,Shader.TileMode tile)
                                           //
                                           //    float x0: 渐变起始点x坐标
                                           //
                                           //    float y0:渐变起始点y坐标
                                           //
                                           //    float x1:渐变结束点x坐标
                                           //
                                           //    float y1:渐变结束点y坐标
                                           //
                                           //    int color0: 起始渐变色
                                           //    int color1: 结束渐变色
                                           //
                                           //    Shader.TileMode tile: 渲染器平铺模式

        private int[] mGradientColors;
        private static int mGradientIndex;
        private IInterpolator mInterpolator;
        private float mDensity;
        private Matrix mMatrix;
        private ValueAnimator mValueAnimator;

        private int width;
        private int height;

        private int mTextSize;//文字大小
        private int mTextLeft;//文字距离左边
        private int mR;//滑块的半径
        private int margin;
        private int marginTop;

        private Rect mSliderRect;
        private int mSlidableLength;    // SlidableLength = BackgroundWidth - LeftMagins - RightMagins - SliderWidth
        private int mEffectiveLength;   // Suggested length is 20pixels shorter than SlidableLength
        private float mEffectiveVelocity = 1000;//滑块自动回滚的速度

        private float mStartX;
        private float mStartY;
        private float mLastX;
        private float mMoveX;

        private bool _init = false;

        private Context _context;

        public Action Unlocked;

        private Bitmap bitmap;
        private bool _unlocked;

        public SlideUnlockView(Context context) : base(context)
        {
            _context = context;
        }

        public SlideUnlockView(Context context, IAttributeSet attrs) : base(context, attrs)
        {
            _context = context;
        }

        public SlideUnlockView(Context context, IAttributeSet attrs, int defStyleAttr) : base(context, attrs, defStyleAttr)
        {
            _context = context;
        }

        public SlideUnlockView(Context context, IAttributeSet attrs, int defStyleAttr, int defStyleRes) : base(context, attrs, defStyleAttr, defStyleRes)
        {
            _context = context;
        }

        private void Init()
        {
            mDensity = Resources.DisplayMetrics.Density;
            ViewConfiguration configuration = ViewConfiguration.Get(_context);
            mMaxVelocity = configuration.ScaledMaximumFlingVelocity;
            mInterpolator = new AccelerateDecelerateInterpolator();

            Clickable = true;
            Focusable = true;
            FocusableInTouchMode = true;

            mSlidableLength = 200;
            mTextSize = (int)DisplayMetricsHelper.Dp2Px(_context, 21);//文字大小
            mTextLeft = (int)DisplayMetricsHelper.Dp2Px(_context, 84);//文字距离左边
            mMoveX = 0;
            mGradientIndex = 0;
            mSliPaint = new Paint();
            mSliPaint.AntiAlias = true;

            mBgPaint = new Paint();
            mBgPaint.AntiAlias = true;

            mUnlockedBgPaint = new Paint();
            mUnlockedBgPaint.AntiAlias = true;

            mPaint = new Paint();
            mPaint.TextSize = mTextSize;
            //该方法即为设置基线上那个点究竟是left,center,还是right
            mPaint.TextAlign = Paint.Align.Left;

            bitmap = BitmapFactory.DecodeResource(_context.Resources, Resource.Drawable.icon_unlock_key);

            // Hard code it
            var currentHeight = DisplayMetricsHelper.Dp2Px(_context, 75);
            bitmap = BitmapHelper.ResizeBitmap(bitmap, (int)currentHeight, true);

            if (mHandler != null)
            {
                mHandler.RemoveCallbacks(Redraw);
                mHandler = new Handler();
                mHandler.PostDelayed(Redraw, DRAW_INTERVAL);
            }
        }

        protected override void OnMeasure(int widthMeasureSpec, int heightMeasureSpec)
        {
            base.OnMeasure(widthMeasureSpec, heightMeasureSpec);

            int specWidthSize = MeasureSpec.GetSize(widthMeasureSpec);//宽
            int specHeightSize = MeasureSpec.GetSize(heightMeasureSpec);//高

            mMatrix = new Matrix();

            width = specWidthSize;
            height = specHeightSize;

            if (!_init)
            {
                Init();

                _init = true;
            }

            margin = (int)DisplayMetricsHelper.Dp2Px(_context, 15);
            marginTop = (int)DisplayMetricsHelper.Dp2Px(_context, 10);
            mR = (int)(bitmap.Height / 2);

            mSlidableLength = (int)(specWidthSize - mR * 2/* - margin * 2*/);
            mEffectiveLength = mSlidableLength - 20;

            mSliderRect = new Rect((int)margin, (int)margin, (int)(mR * 2 + margin * 2),
                (int)(mR * 2 + margin * 2));

            mGradientColors = new int[]{Color.Argb(255, 170, 170, 170),
                    Color.Argb(255, 170, 170, 170), Color.Argb(255, 255, 255, 255)};

            mGradient = new LinearGradient(0, 0, width / 2, 0, mGradientColors,
                    new float[] { 0, 0.7f, 1 }, Shader.TileMode.Mirror);

            bgGradient = new LinearGradient(
                    0, 0, width, height,
                    Color.Argb(0Xff, 0X3e, 0X53, 0Xc3), Color.Argb(0Xff, 0X52, 0X9c, 0Xfa),
                    Shader.TileMode.Clamp
            );

            unlockedGgGradient = new LinearGradient(
                    0, 0, width, height,
                    Color.Argb(0Xff, 0X3c, 0Xff, 0Xe0), Color.Argb(0Xff, 0X2e, 0Xcf, 0X62),
                    Shader.TileMode.Clamp
            );

            sliGradient = new LinearGradient(
                    0, 0, 0, (float)((height) / 2.0),
                    Color.Argb(80, 0Xbb, 0Xbb, 0Xbb), Color.Argb(200, 0X77, 0X77, 0X77),
                    Shader.TileMode.Clamp
            );

            mBgPaint.SetShader(bgGradient);
            mUnlockedBgPaint.SetShader(unlockedGgGradient);

            mSliPaint.SetShader(sliGradient);

            if (mHandler != null)
            {
                mHandler.RemoveCallbacks(Redraw);
                mHandler.PostDelayed(Redraw, DRAW_INTERVAL);
            }
        }

        protected override void OnDraw(Canvas canvas)
        {
            base.OnDraw(canvas);

            RectF oval = new RectF(margin, marginTop, width - margin, height - marginTop);// 设置个新的长方形

            mPaint.SetShader(mGradient);
            var fontMetrics = mPaint.GetFontMetrics();
            int baseLineY = (int)(height / 2 - fontMetrics.Top / 2 - fontMetrics.Bottom / 2);//基线中间点的y轴计算公式
            int baseLineX = (int)(width / 2);

            if (!_unlocked)
            {
                canvas.DrawRoundRect(oval, 30, 30, mBgPaint);
                canvas.DrawText("Slide to unlock", mTextLeft, baseLineY, mPaint);
                canvas.DrawBitmap(bitmap, mMoveX, -10, new Paint());
            }
            else
            {
                var paint = new Paint();
                paint.Color = Color.White;
                paint.TextSize = mTextSize;
                paint.TextAlign = Paint.Align.Center;

                canvas.DrawRoundRect(oval, 30, 30, mUnlockedBgPaint);
                canvas.DrawText("Unlocked", baseLineX, baseLineY, paint);
            }
        }

        public void Reset()
        {
            if (mValueAnimator != null)
            {
                mValueAnimator.Cancel();
            }

            _unlocked = false;

            mMoveX = 0;

            if (mPaint != null)
            {
                mPaint.Alpha = 255;
            }

            if (mHandler != null)
            {
                mHandler.RemoveCallbacks(Redraw);
                mHandler = new Handler();
                mHandler.PostDelayed(Redraw, 200);
            }
        }

        public override bool OnTouchEvent(MotionEvent motionEvent)
        {
            // 点击是否在滑块上
            if (motionEvent.Action != MotionEventActions.Down
                    && !mSliderRect.Contains((int)mStartX, (int)mStartY))
            {
                if (motionEvent.Action == MotionEventActions.Up
                        || motionEvent.Action == MotionEventActions.Cancel)
                {
                    //mHandler.PostDelayed(Redraw, DRAW_INTERVAL);
                }
                return base.OnTouchEvent(motionEvent);
            }
            acquireVelocityTrackerAndAddMovement(motionEvent);

            switch (motionEvent.Action)
            {
                case MotionEventActions.Down:
                    mStartX = motionEvent.GetX();
                    mStartY = motionEvent.GetY();
                    mLastX = mStartX;

                    mHandler.RemoveCallbacks(Redraw);
                    break;

                case MotionEventActions.Move:
                    mLastX = motionEvent.GetX();
                    if (mLastX > mStartX) {
                        int alpha = (int)(255 - (mLastX - mStartX) * 3 / mDensity);
                        if (alpha > 1) {
                            mPaint.Alpha = alpha;
                        } else {
                            mPaint.Alpha = 0;
                        }

                        if (mLastX - mStartX > mSlidableLength) {
                            mLastX = mStartX + mSlidableLength;
                            mMoveX = mSlidableLength;
                        } else {
                            mMoveX = (int)(mLastX - mStartX);
                        }
                    } else {
                        mLastX = mStartX;
                        mMoveX = 0;
                    }
                    Invalidate();
                    break;

                case MotionEventActions.Up:
                case MotionEventActions.Cancel:
                    mVelocityTracker.ComputeCurrentVelocity(1000, mMaxVelocity);
                    float velocityX = mVelocityTracker.XVelocity;

                    if (mLastX - mStartX > mEffectiveLength || velocityX / 2 > mEffectiveVelocity) {
                        startAnimator(mLastX - mStartX, mSlidableLength, velocityX, true);
                    } else {
                        startAnimator(mLastX - mStartX, 0, velocityX, false);

                        //mHandler.PostDelayed(Redraw, DRAW_INTERVAL);
                    }
                    releaseVelocityTracker();
                    break;
            }
            return base.OnTouchEvent(motionEvent);
        }

        private void startAnimator(float start, float end, float velocity, bool isRightMoving) {
            if (velocity < mEffectiveVelocity) {
                velocity = mEffectiveVelocity;
            }
            int duration = (int)(Math.Abs(end - start) * 1000 / velocity);
            mValueAnimator = ValueAnimator.OfFloat(start, end);
            mValueAnimator.Update += (s, e) =>
            {
                mMoveX = (float)e.Animation.AnimatedValue;
                int alpha = (int)(255 - (mMoveX) * 3 / mDensity);
                if (alpha > 1)
                {
                    mPaint.Alpha = alpha;
                }
                else
                {
                    mPaint.Alpha = 0;
                }
                Invalidate();
            };
            mValueAnimator.SetDuration(duration);
            mValueAnimator.SetInterpolator(mInterpolator);
            if (isRightMoving) {
                mValueAnimator.AnimationEnd += (s, e) =>
                {
                    Unlocked?.Invoke();
                    _unlocked = true;
                };
            }
            mValueAnimator.Start();
        }

        private void acquireVelocityTrackerAndAddMovement(MotionEvent ev) {
            if (mVelocityTracker == null) {
                mVelocityTracker = VelocityTracker.Obtain();
            }
            mVelocityTracker.AddMovement(ev);
        }

        private void releaseVelocityTracker() {
            if (mVelocityTracker != null) {
                mVelocityTracker.Recycle();
                mVelocityTracker = null;
            }
        }

        private void Redraw()
        {
            try
            {
                if (mMatrix == null)
                {
                    mMatrix = new Matrix();

                }

                mMatrix.SetTranslate(mGradientIndex, 0);

                if (mGradient == null)
                {
                    mGradientColors = new int[]{Color.Argb(255, 120, 120, 120),
                                    Color.Argb(255, 120, 120, 120), Color.Argb(255, 255, 255, 255)};

                    mGradient = new LinearGradient(0, 0, width / 2, 0, mGradientColors,
                            new float[] { 0, 0.7f, 1 }, Shader.TileMode.Mirror);
                }

                mGradient.SetLocalMatrix(mMatrix);
                Invalidate();
                mGradientIndex += STEP_LENGTH;
                if (mGradientIndex >= 100000)
                {
                    mGradientIndex = 0;
                }

                mHandler.PostDelayed(Redraw, DRAW_INTERVAL);
            }
            catch (Exception e)
            {
            }
        }
    }
}