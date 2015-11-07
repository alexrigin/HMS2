using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Media;
using System.Windows;

namespace HMS_Converters {
  public class IsObsoleteToTextDecorationsConverter: IValueConverter {

    public object Convert(object value, Type targetType, object parameter, CultureInfo culture) {
      if (value is bool) {
        if ((bool)value) {
          TextDecorationCollection redStrikthroughTextDecoration = TextDecorations.Strikethrough.CloneCurrentValue();
          redStrikthroughTextDecoration[0].Pen = new Pen {Brush=Brushes.Red, Thickness = 3 };
          return redStrikthroughTextDecoration; 
        }
      }
      return new TextDecorationCollection(); 
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) {
      throw new NotImplementedException();
    }
  }
}