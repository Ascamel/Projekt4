using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
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
using Color = System.Windows.Media.Color;

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


        #region Zadanie 1
        private void TranformButtonClick(object sender, RoutedEventArgs e)
        {
            BitmapSource bitmap = (BitmapSource)MyImage.Source;

            WriteableBitmap writeableBitmap = new(bitmap);
            Int32Rect rect = new Int32Rect(0, 0, (int)bitmap.Width, (int)bitmap.Height);
            int alpha = 0;
            int red = Convert.ToInt32(RValueTextBox.Text);
            int green = Convert.ToInt32(GValueTextBox.Text);
            int blue = Convert.ToInt32(BValueTextBox.Text);

            byte[] pixels = new byte[(int)bitmap.Width * (int)bitmap.Height * writeableBitmap.Format.BitsPerPixel / 8];
            int stride = (writeableBitmap.PixelWidth * writeableBitmap.Format.BitsPerPixel) / 8;

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
                    else if (GrayScaleButton1.IsChecked.GetValueOrDefault())
                    {
                        int grayScale = (int)((color.R * 0.21) + (color.G * 0.72) + (color.B * 0.07));
                        Color nc = Color.FromArgb(color.A, (byte)grayScale, (byte)grayScale, (byte)grayScale);

                        pixels[pixelOffset] = (byte)(nc.B);
                        pixels[pixelOffset + 1] = (byte)(nc.G);
                        pixels[pixelOffset + 2] = (byte)(nc.R);
                        pixels[pixelOffset + 3] = (byte)(nc.A);
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

        #endregion

        private void TranformButtonClick2(object sender, RoutedEventArgs e)
        {
            if(SmoothingButton.IsChecked.GetValueOrDefault())
            {
                SmoothImage();
            }
            else if(MedianButton.IsChecked.GetValueOrDefault())
            {
                MedianImage();
            }
            else if(SobelButton.IsChecked.GetValueOrDefault())
            {

            }
            else if (HighPassButton.IsChecked.GetValueOrDefault())
            {

            }
            else if (GaussButton.IsChecked.GetValueOrDefault())
            {

            }
            else if (Mask.IsChecked.GetValueOrDefault())
            {

            }
        }
        public static int Median(byte[] data)
        {
            Array.Sort(data);

            if (data.Length % 2 == 0)
                return (data[data.Length / 2 - 1] + data[data.Length / 2]) / 2;
            else
                return data[data.Length / 2];
        }

        private void MedianImage()
        {
            BitmapSource bitmap = (BitmapSource)MyImage2.Source;

            WriteableBitmap writeableBitmap = new(bitmap);
            Int32Rect rect = new Int32Rect(0, 0, (int)bitmap.Width, (int)bitmap.Height);

            byte[] pixels = new byte[(int)bitmap.Width * (int)bitmap.Height * writeableBitmap.Format.BitsPerPixel / 8];
            int stride = (writeableBitmap.PixelWidth * writeableBitmap.Format.BitsPerPixel) / 8;

            for (int i = 0; i<writeableBitmap.PixelHeight; i++)
            {
                for (int j = 0; j<writeableBitmap.PixelWidth; j++)
                {
                    int pixelOffset = (j + i * writeableBitmap.PixelWidth) * writeableBitmap.Format.BitsPerPixel/8;
                    List<Color> colors = new();
                    for (int k = -1; k < 2; k++)
                    {
                        for (int l = -1; l < 2; l++)
                        {
                            if (j+l < 0 || i+k < 0 || j+l >= bitmap.PixelWidth || i+k >= bitmap.PixelHeight)
                                continue;

                            colors.Add(GetPixelColor(bitmap, j+l, i+k));
                        }
                    }

                    int r = 0, g = 0, b = 0;

                    IEnumerable<byte> rArray =
                        from value in colors
                        select value.R;

                    IEnumerable<byte> gArray =
                        from value in colors
                        select value.G;

                    IEnumerable<byte> bArray =
                        from value in colors
                        select value.B;


                    pixels[pixelOffset] = (byte)(Median(bArray.ToArray()));
                    pixels[pixelOffset + 1] = (byte)(Median(gArray.ToArray()));
                    pixels[pixelOffset + 2] = (byte)(Median(rArray.ToArray()));
                    pixels[pixelOffset + 3] = (byte)(255);

                }
                writeableBitmap.WritePixels(rect, pixels, stride, 0);
            }

            MyImage2.Source = writeableBitmap;
        }

        private void SmoothImage()
        {
            BitmapSource bitmap = (BitmapSource)MyImage2.Source;

            WriteableBitmap writeableBitmap = new(bitmap);
            Int32Rect rect = new Int32Rect(0, 0, (int)bitmap.Width, (int)bitmap.Height);

            byte[] pixels = new byte[(int)bitmap.Width * (int)bitmap.Height * writeableBitmap.Format.BitsPerPixel / 8];
            int stride = (writeableBitmap.PixelWidth * writeableBitmap.Format.BitsPerPixel) / 8;

            for (int i = 0; i<writeableBitmap.PixelHeight; i++)
            {
                for (int j = 0; j<writeableBitmap.PixelWidth; j++)
                {
                    int pixelOffset = (j + i * writeableBitmap.PixelWidth) * writeableBitmap.Format.BitsPerPixel/8;
                    List<Color> colors = new();
                    for (int k = -1; k < 2; k++)
                    {
                        for (int l = -1; l < 2; l++)
                        {
                            if (j+l < 0 || i+k < 0 || j+l >= bitmap.PixelWidth || i+k >= bitmap.PixelHeight)
                                continue;

                            colors.Add(GetPixelColor(bitmap, j+l, i+k));
                        }
                    }

                    int r = 0, g = 0, b = 0;
                    foreach (Color c in colors)
                    {
                        r += c.R;
                        g += c.G;
                        b += c.B;
                    }

                    if (colors.Count > 0)
                    {
                        r /= colors.Count;
                        g /= colors.Count;
                        b /= colors.Count;
                    }


                    pixels[pixelOffset] = (byte)(b);
                    pixels[pixelOffset + 1] = (byte)(g);
                    pixels[pixelOffset + 2] = (byte)(r);
                    pixels[pixelOffset + 3] = (byte)(255);

                }
                writeableBitmap.WritePixels(rect, pixels, stride, 0);
            }

            MyImage2.Source = writeableBitmap;
        }

        private void LoadImageButtonClicked2(object sender, RoutedEventArgs e)
        {
            OpenFileDialog op = new OpenFileDialog();
            op.Title = "Select a picture";
            op.Filter = "All supported graphics|*.jpg;*.jpeg;*.png|" +
              "JPEG (*.jpg;*.jpeg)|*.jpg;*.jpeg|" +
              "Portable Network Graphic (*.png)|*.png";
            if (op.ShowDialog() == true)
            {
                MyImage2.Source = new BitmapImage(new Uri(op.FileName));
            }
        }
    }
}
