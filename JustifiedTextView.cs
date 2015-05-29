
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Util;
using Android.Views;
using Android.Widget;
using Android.Text;
using Android.Graphics;

namespace JustifiedXamarin
{
	public class JustifiedTextView : TextView
	{

		private int mLineY;
		private int mViewWidth;


		public JustifiedTextView (Context context) :
			base (context)
		{
			Initialize ();
		}

		public JustifiedTextView (Context context, IAttributeSet attrs) :
			base (context, attrs)
		{
			Initialize ();
		}

		public JustifiedTextView (Context context, IAttributeSet attrs, int defStyle) :
			base (context, attrs, defStyle)
		{
			Initialize ();
		}

		void Initialize ()
		{
		}

		protected override void OnLayout (bool changed, int left, int top, int right, int bottom)
		{
			base.OnLayout (changed, left, top, right, bottom);
		}

		protected override void OnDraw (Android.Graphics.Canvas canvas)
		{
			TextPaint paint = Paint;
			paint.Color = new Color(CurrentTextColor);
			paint.DrawableState = GetDrawableState();
			mViewWidth = MeasuredWidth;
			String text = (String) this.Text;
			mLineY = 0;
			mLineY += (int)TextSize;
			Layout layout = Layout;
			for (int i = 0; i < layout.LineCount; i++) {
				int lineStart = layout.GetLineStart(i);
				int lineEnd = layout.GetLineEnd(i);
				String line = text.Substring(lineStart, lineEnd-lineStart);

				float width = StaticLayout.GetDesiredWidth(text, lineStart, lineEnd, Paint);
				if (NeedScale(line)) {
					DrawScaledText(canvas, lineStart, line, width);
				} else {
					canvas.DrawText(line, 0, mLineY, paint);
				}

				mLineY += LineHeight;
			}
		}

		private void DrawScaledText(Canvas canvas, int lineStart, String line, float lineWidth) {
			float x = 0;
			if (IsFirstLineOfParagraph(lineStart, line)) {
				String blanks = "  ";
				canvas.DrawText(blanks, x, mLineY, Paint);
				float bw = StaticLayout.GetDesiredWidth(blanks, Paint);
				x += bw;
	
				line = line.Substring(3);
			}
	
			float d = (mViewWidth - lineWidth) / line.Length - 1;
			for (int i = 0; i < line.Length; i++) {
				String c = line[i].ToString();
				float cw = StaticLayout.GetDesiredWidth(c, Paint);
				canvas.DrawText(c, x, mLineY, Paint);
				x += cw + d;
			}
		}
	
		private bool IsFirstLineOfParagraph(int lineStart, String line) {
			return line.Length > 3 && line[0] == ' ' && line[1] == ' ';
		}
	
		private bool NeedScale(String line) {
			if (line.Length == 0) {
				return false;
			} else {
				return line[line.Length - 1] != '\n';
			}
		}
	}
}