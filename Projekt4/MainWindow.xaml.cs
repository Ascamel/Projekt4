using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Projekt4
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void LoadImageButtonClicked(object sender, RoutedEventArgs e)
        {
            OpenFileDialog op = new OpenFileDialog();
            op.Title = "Select a picture";
            op.Filter = "All supported graphics|*.jpg;*.jpeg;*.png|" +
              "JPEG (*.jpg;*.jpeg)|*.jpg;*.jpeg|" +
              "Portable Network Graphic (*.png)|*.png";
            if (op.ShowDialog() == true)
            {
                MyImage.Source = new BitmapImage(new Uri(op.FileName));
            }
        }

        private void TranformButtonClick(object sender, RoutedEventArgs e)
        {
            BitmapSource bitmap = (BitmapSource)MyImage.Source;
            var topLeftColor = GetPixelColor(bitmap, 0, 0);
            var topRightColor = GetPixelColor(bitmap, bitmap.PixelWidth - 1, 0);
            var bottomLeftColor = GetPixelColor(bitmap, 0, bitmap.PixelHeight - 1);
            var bottomRightColor = GetPixelColor(bitmap, bitmap.PixelWidth - 1, bitmap.PixelHeight - 1);

            WriteableBitmap writeableBitmap = new(bitmap);
            Int32Rect rect = new Int32Rect(0, 0, (int)bitmap.Width, (int)bitmap.Height);
            int alpha = 0;
            int red = Convert.ToInt32(RValueTextBox.Text);
            int green = Convert.ToInt32(GValueTextBox.Text);
            int blue = Convert.ToInt32(BValueTextBox.Text);

            byte[] pixels = new byte[(int)bitmap.Width * (int)bitmap.Height * writeableBitmap.Format.BitsPerPixel / 8];
            for (int i = 0; i<writeableBitmap.PixelHeight; i++)
            {
                for (int j = 0; j<writeableBitmap.PixelWidth; j++)
                {
                    int pixelOffset = (j + i * writeableBitmap.PixelWidth) * writeableBitmap.Format.BitsPerPixel/8;
                    Color color = GetPixelColor(bitmap, j, i);

                    if (AddButton.IsChecked.GetValueOrDefault())
                    {
                        if (color.B + blue > 255)
                            pixels[pixelOffset] = 255;
                        else
                            pixels[pixelOffset] = (byte)(color.B + blue);

                        if (color.G +green > 255)
                            pixels[pixelOffset + 1] = 255;
                        else
                            pixels[pixelOffset + 1] = (byte)(color.G +green);

                        if (color.R +red > 255)
                            pixels[pixelOffset + 2] = 255;
                        else
                            pixels[pixelOffset + 2] = (byte)(color.R +red);


                        pixels[pixelOffset + 3] = (byte)(color.A +alpha);
                    }
                    else if (RemoveButton.IsChecked.GetValueOrDefault())
                    {
                        if (color.B - blue < 0)
                            pixels[pixelOffset] = 0;
                        else
                            pixels[pixelOffset] = (byte)(color.B - blue);

                        if (color.G -green < 0)
                            pixels[pixelOffset + 1] = 0;
                        else
                            pixels[pixelOffset + 1] = (byte)(color.G - green);

                        if (color.R -red < 0)
                            pixels[pixelOffset + 2] = 0;
                        else
                            pixels[pixelOffset + 2] = (byte)(color.R - red);

                        pixels[pixelOffset + 3] = (byte)(color.A - alpha);
                    }
                    else if (MultiplicationButton.IsChecked.GetValueOrDefault())
                    {
                        if (color.B * blue > 255)
                            pixels[pixelOffset] = 255;
                        else
                            pixels[pixelOffset] = (byte)(color.B * blue);

                        if (color.G *green > 255)
                            pixels[pixelOffset + 1] = 255;
                        else
                            pixels[pixelOffset + 1] = (byte)(color.G * green);

                        if (color.R *red > 255)
                            pixels[pixelOffset + 2] = 255;
                        else
                            pixels[pixelOffset + 2] = (byte)(color.R * red);

                        pixels[pixelOffset + 3] = (byte)(color.A * alpha);
                    }
                    else if (DivideButton.IsChecked.GetValueOrDefault())
                    {
                        if (blue == 0)
                            blue = 1;

                        if (green == 0)
                            green = 1;

                        if (red == 0)
                            red = 1;

                        if (alpha == 0)
                            alpha = 1;


                        pixels[pixelOffset] = (byte)(color.B / blue);
                        pixels[pixelOffset + 1] = (byte)(color.G / green);
                        pixels[pixelOffset + 2] = (byte)(color.R / red);
                        pixels[pixelOffset + 3] = (byte)(color.A / alpha);
                    }
                    else if (BrightnessButton.IsChecked.GetValueOrDefault())
                    {
                        int brightness = Convert.ToInt32(BrightnessValue.Text);

                        if (brightness + color.B > 255)
                            pixels[pixelOffset] = 255;
                        else
                            pixels[pixelOffset] = (byte)(brightness + color.B);

                        if (brightness + color.G > 255)
                            pixels[pixelOffset + 1] = 255;
                        else
                            pixels[pixelOffset + 1] = (byte)(brightness + color.G);

                        if (brightness + color.R > 255)
                            pixels[pixelOffset + 2] = 255;
                        else
                            pixels[pixelOffset + 2] = (byte)(brightness + color.R);



                        pixels[pixelOffset + 3] = (byte)(color.A);
                    }
                    else if(GrayScaleButton1.IsChecked.GetValueOrDefault())
                    {
                        int grayScale = (int)((color.R * 0.3) + (color.G * 0.59) + (color.B * 0.11));
                        Color nc = Color.FromArgb(color.A, (byte)grayScale, (byte)grayScale, (byte)grayScale);

                        pixels[pixelOffset] = (byte)(nc.B );
                        pixels[pixelOffset + 1] = (byte)(nc.G );
                        pixels[pixelOffset + 2] = (byte)(nc.R );
                        pixels[pixelOffset + 3] = (byte)(nc.A );
                    }
                    else if (GrayScaleButton2.IsChecked.GetValueOrDefault())
                    {
                        int grayScale = (int)((color.R + color.G + color.B)/3);
                        Color nc = Color.FromArgb(color.A, (byte)grayScale, (byte)grayScale, (byte)grayScale);

                        pixels[pixelOffset] = (byte)(nc.B);
                        pixels[pixelOffset + 1] = (byte)(nc.G);
                        pixels[pixelOffset + 2] = (byte)(nc.R);
                        pixels[pixelOffset + 3] = (byte)(nc.A);
                    }


                }

                int stride = (writeableBitmap.PixelWidth * writeableBitmap.Format.BitsPerPixel) / 8;
                writeableBitmap.WritePixels(rect, pixels, stride, 0);
            }

            MyImage.Source = writeableBitmap;

        }

        public static Color GetPixelColor(BitmapSource bitmap, int x, int y)
        {
            Color color;
            var bytesPerPixel = (bitmap.Format.BitsPerPixel + 7) / 8;
            var bytes = new byte[bytesPerPixel];
            var rect = new Int32Rect(x, y, 1, 1);

            bitmap.CopyPixels(rect, bytes, bytesPerPixel, 0);

            if (bitmap.Format == PixelFormats.Bgra32)
            {
                color = Color.FromArgb(bytes[3], bytes[2], bytes[1], bytes[0]);
            }
            else if (bitmap.Format == PixelFormats.Bgr32)
            {
                color = Color.FromRgb(bytes[2], bytes[1], bytes[0]);
            }
            // handle other required formats
            else
            {
                color = Colors.Black;
            }

            return color;
        }
    }
}
