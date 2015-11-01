using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

using System.Windows;
using Windows.UI.Input;
using Windows.UI.Xaml.Shapes;
using Windows.UI;
using Windows.UI.Input.Inking;
using Windows.Devices.Input;
using Windows.Storage.Pickers;
using Windows.Storage;
using Windows.UI.ViewManagement;
using Windows.Storage.Streams;
using Windows.UI.Xaml.Media.Imaging;
using Windows.System;
using PerpetuumSoft.Controls;
using ClipperLib;
using Windows.UI.Popups;
using System.Runtime.Serialization;
using Windows.Data.Xml.Dom;
using System.Threading.Tasks;
using Windows.Security.Credentials.UI;
using Windows.Storage.AccessCache;


// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace App2
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    
    public sealed partial class DrawingPage : Page
    {

        bool havetobeclosed = false;

        bool visible = false, itemselected = false,
            penselected = false, pen_pointdown = false,
            polygonselected = false, lineselected = false,
            triangleselected = false, rectangleselected = false,
            textboxselected = false, imageselected = false,
            selectselected = false, tableselected = false,
            tablelayoutinserted = false;

        //펜그리기
        InkManager _inkKhaled = new InkManager();
        private uint _penID;
        private uint _touchID;
        private Point _previousContactPt;
        private Point currentContactPt;
        private double x1;
        private double y1;
        private double x2;
        private double y2;
        private Color pencolor;

        private PathFigure _pathFigure = new PathFigure();
        PathFigureCollection _pathCollection = new PathFigureCollection();
        PathSegmentCollection _segments = new PathSegmentCollection();
        private PathGeometry _pathGeometry = new PathGeometry();

        //Line그리기
        private int line_start = -1, line_prevcount = -1;

        //Polygon그리기
        private List<Point> polygonPoints = new List<Point>();
        private Point poly_startPoint = new Point();
        private int poly_start = -1, poly_previtem = -1, poly_prevcount = -1, poly_edgecount = 0;
        bool poly_startget = false;

        //Rectangle 그리기
        private int rec_prevcount = -1;

        //Rectangle 그리기
        private int table_prevcount = -1;

        //Text쓰기
        private List<TextBox> textbox_list = new List<TextBox>();

        //Image삽입
        private StorageFile loadedImage;
        private int img_prevcount = -1;
        Image img = new Image();
        bool img_left = false, img_right = false, img_top = false, img_bottom = false,
            img_move = false;
        double img_prev_left = 0, img_prev_top = 0;

        //Table 그리기
        double table_row, table_column;
        

        bool previewselected = false;



        //class to develop tts
        class customitems
        {
            public int index;
            public string tts_message;
            public customitems(int a, string b)
            {
                index = a;
                tts_message = b;
            }
            public void set_tts(string b)
            {
                tts_message = b;
            }
            public int get_index()
            {
                return index;
            }
        }
        private List<customitems> List_item = new List<customitems>();


        public DrawingPage()
        {
            PerpetuumSoft.WinRT.Framework.Licensing.LicenseManager.SetKey(@"CIAADGAADDAAC2AADJAAC1AADIAABZAACIAAC3AADGAADEAAC3AADIAADJAADJAA
                DBAAA6AACNAACBAAA6AAB5AADDAADCAADIAADGAADDAADAAADHAAA6AAC4AADDAA
                DGAAA6AACPAAC7AADCAAC2AADDAADLAADHAAA6AABUAAANAAAKAACGAACZAADBAA
                C3AABZAAC8AADDAADCAAC5AAC6AAC3AACZAADCAABNAABOAAANAAAKAAB7AADMAA
                DEAAC7AADGAACZAADIAAC7AADDAADCAAB6AACZAADIAAC3AABZAABMAABOAABLAA
                BOAABSAABLAABOAABMAABNAABQAAA6AABMAABMAABWAABMAABMAABWAABMAABMAA
                ANAAAKAACEAAC7AAC1AAC3AADCAADHAAC3AACMAADNAADEAAC3AABZAACKAAC3AA
                C5AAC7AADHAADIAAC3AADGAAC3AAC2AAANAAAKAACEAAC7AAC1AAC3AADCAADHAA
                C3AADHAACBAADCAAB9AADGAADDAADJAADEAABZAABNAAANAAAKAACPAAC7AADCAA
                BUAACNAACBAAB5AADDAADCAADIAADGAADDAADAAADHAABZAABNAAANAAAKAACPAA
                C7AADCAABUAACEAAC7AAC1AAC3AADCAADHAAC7AADCAAC5AABZAABNAAANAAAKAA
                CKAADJAADCAACMAAC7AADBAAC3AACLAAC7AAC5AADCAACZAADIAADJAADGAAC3AA
                BZAAB5AACDAAB7AAB7AAB5AACKAAB5AACGAAB8AABUAAB4AACAAAB3AABQAAB8AA
                BQAAB3AACOAAB9AACIAAB4AABUAAB9AACRAAB6AABQAAB5AACPAAB5AACMAAB6AA
                B9AAB9AABRAAB8AACLAAB9AAB8AAB3AACNAAB7AAB8AAB5AACBAAB4AAB7AAB5AA
                B7AAB6AABTAAB9AABPAAB8AABSAAB6AACJAAB4AAB6AAB3AACEAAB8AACAAAB8AA
                CQAAB8AACIAAB4AACRAAB5AACFAAB6AAB5AAB3AAB3AAB4AACLAAB4AAB7AAB6AA
                CCAAB5AACRAAB5AAB7AAB9AABQAAB9AABUAAB7AACJAAB7AACBAAB9AACOAAB3AA
                CIAAB5AACSAAB3AABNAAB5AACJAAB9AACIAAB6AACQAAB7AABVAAB3AACJAAB9AA
                BOAAB4AACCAAB9AAB8AAB4AABQAAB4AACRAAB8AAB5AAB8AACSAAB5AACFAAB3AA
                CEAAB8AACIAAB8AACLAAB5AACJAAB4AABPAAB7AACHAAB3AAB8AAB5AACMAAB5AA
                CFAAB7AABTAAB7AACFAAB3AACMAAB5AAB5AAB6AAB8AAB4AACNAAB9AAB6AAB8AA
                B4AAB9AACSAAB9AAB3AAB6AABTAAB8AAB5AAB4AACBAAB4AABSAAB8AAB9AAB9AA
                CDAAB7AACQAAB3AACJAAB6AACAAAB6AACPAAB5AAB9AAB5AACRAAB6AACEAAB4AA
                CAAAB6AACEAAB5AAB7AAB5AAB7AAB9AAB9AAB9AAB7AAB4AACFAAB6AAB4AAB3AA
                CAAAB4AACPAAB9AACCAAB6AABRAAB5AACIAAB4AACEAAB6AABVAAB6AACEAAB3AA
                CMAAB3AAB7AAB9AACPAAB7AACBAAB5AACPAAB8AABNAAB5AAB9AAB5AACBAAB8AA
                B7AAB5AACFAAB7AACIAAB3AAB8AAB5AACMAAB7AACFAAB5AACSAAB3AABNAAB3AA
                B4AAANAAAKAAB6AAC3AADHAAC7AAC5AADCAACMAAC7AADBAAC3AACLAAC7AAC5AA
                DCAACZAADIAADJAADGAAC3AABZAAB7AACGAAB4AACLAAB3AABTAAB6AACPAAB9AA
                BNAAB5AACJAAB4AACKAAB8AACQAAB3AACDAAB7AACOAAB4AAB7AAB5AAB4AAB6AA
                CFAAB4AABUAAB9AACCAAB3AACOAAB8AACJAAB7AACMAAB3AACPAAB9AACPAAB9AA
                CSAAB5AACIAAB5AACNAAB9AABVAAB9AABQAAB8AABVAAB6AACCAAB5AAB7AAB5AA
                CKAAB5AACEAACAAAB5AAB9AABPAAB7AACAAAB4AACBAAB6AACEAAB4AAB9AAB9AA
                B3AAB8AACAAAB3AACJAAB6AACHAAB9AABTAAB3AACHAAB8AABUAAB9AABVAAB3AA
                CBAAB8AABVAAB7AABSAAB7AABUAAB4AABNAAB8AAB6AAB7AACGAAB3AACIAAB9AA
                BOAAB3AABVAAB7AACDAAB6AACQAAB5AABSAAB5AAB8AACAAAB5AAB3AABVAAB6AA
                CNAAB3AACHAAB5AACMAAB4AACCAAB9AACJAAB6AACNAAB4AACFAAB5AABNAAB8AA
                BRAAB6AACLAAB7AACKAAB8AACNAACAAAB6AAB4AAB4AAB8AAB7AAB6AACPAAB4AA
                CLAAB3AACSAAB9AABVAAB8AACKAAB7AACHAAB5AACPAAB6AACCAAB9AACFAAB5AA
                CFAAB8AACAAAB3AABQAAB6AABPAAB4AABMAAB5AAB3AAB5AACCAAB4AAB5AAB7AA
                B6AAB9AABNAAB7AACKAAB9AACKAAB9AABQAAB7AABNAAB9AACEAAB6AACOAAB6AA
                B9AAB8AABOAAB6AACKAAB4AABPAAB6AACKAAB6AABPAAB7AABQAAB9AACFAAB7AA
                BTAAB4AACRAAB8AAB3AAB4AACKAAB3AABSAAB7AACFAAB4AACLAAB9AACOAAB7AA
                CPAAB7AABPAAB5AABVAAB6AACFAAB4AAB5AAB8AABRAAB5AAB8AAB9AABTAAB9AA
                B7AAB3AACSAAB3AACAAAB3AABUAAANAAAKAA");
            this.InitializeComponent();
            //PerpetuumSoft.Controls.RadialMenuService.HideMenu(MainRadialMenu);
            MainRadialMenu.Visibility = Visibility.Collapsed;
            
            MyCanvas.PointerPressed += new PointerEventHandler(MyCanvas_PointerPressed);
            MyCanvas.PointerMoved += new PointerEventHandler(MyCanvas_PointerMoved);
            MyCanvas.PointerReleased += new PointerEventHandler(MyCanvas_PointerReleased);
            MyCanvas.PointerExited += new PointerEventHandler(MyCanvas_PointerReleased);
            MyCanvas.RightTapped += new RightTappedEventHandler(MyCanvas_RightTapped);
            
            MainGrid.KeyDown += new KeyEventHandler(MainGrid_KeyDown);
            MyCanvas.KeyDown += new KeyEventHandler(MyCanvas_KeyDown);
            MainRadialMenu.KeyDown += new KeyEventHandler(MainRadialMenu_KeyDown);
        }

        private void Drawing_Initialize(object sender, PointerRoutedEventArgs e)
        {
            PointerPoint current = e.GetCurrentPoint(this);
            if (e.Pointer.PointerId == _penID)
            {
                //Pass the pointer information to the InkManager. 
                _inkKhaled.ProcessPointerUp(current);
            }

            else if (e.Pointer.PointerId == _touchID)
            {
            // Process touch input
            }

            _touchID = 0;
            _penID = 0;

            // Call an application-defined function to render the ink strokes.

            e.Handled = true;
        }

        private void MyCanvas_RightTapped(object sender, RightTappedRoutedEventArgs e)
        {
            if (polygonselected)
            {
                Polygon newshape = new Polygon()
                {
                    StrokeThickness = 2,
                    Stroke = new SolidColorBrush(Colors.Green)
                };
                int count = MyCanvas.Children.Count;
                int i = MyCanvas.Children.Count - poly_edgecount,
                    j = 0;
                polygonPoints.RemoveAt(polygonPoints.Count - 1);
                while (i != count)
                {
                    newshape.Points.Add(polygonPoints.ElementAt(j));
                    MyCanvas.Children.RemoveAt(MyCanvas.Children.Count - 1);
                    i++;
                    j++;
                }
                MyCanvas.Children.Add(newshape);
                List_item.Add(new customitems(MyCanvas.Children.IndexOf(newshape), null));
                newshape.PointerPressed += newshape_PointerPressed;
                newshape.PointerEntered += newshape_PointerEntered;

                TextBox newbox = new TextBox()
                {
                    Width = 200,
                    Height = 16,
                    Background = new SolidColorBrush(Colors.LightGray),
                    BorderBrush = new SolidColorBrush(Colors.Black)
                };
                Canvas.SetLeft(newbox, currentContactPt.X + 10);
                Canvas.SetTop(newbox, currentContactPt.Y - 10);

                Button Okbtn = new Button()
                {
                    Width = 60,
                    Height = 30,
                    FontSize = 10,
                    FontStyle = Windows.UI.Text.FontStyle.Normal,
                    Content = "OK",
                    Background = new SolidColorBrush(Colors.LightGray),
                    BorderBrush = new SolidColorBrush(Colors.Black)
                };
                Canvas.SetLeft(Okbtn, currentContactPt.X + 220);
                Canvas.SetTop(Okbtn, currentContactPt.Y - 10);
                MyCanvas.Children.Add(newbox);
                MyCanvas.Children.Add(Okbtn);
                Okbtn.Click += Okbtn_Click;

                polygonPoints.Clear();

                poly_start = -1;
                poly_previtem = -1;
                poly_edgecount = 0;
                //Drawing_Initialize(sender, e);
                poly_startget = false;
            }
        }

        private void MyCanvas_PointerReleased(object sender, PointerRoutedEventArgs e)
        {
            TranslateTransform posTransform = new TranslateTransform();
            if (havetobeclosed)
            {
                posTransform.X = Window.Current.Bounds.Width - 300;
                posTransform.Y = -30;
                MainRadialMenu.RenderTransform = posTransform;
                return;
            }
            if (!previewselected) { 
                if (penselected || triangleselected || rectangleselected || imageselected || tablelayoutinserted)
                {
                    if (e.Pointer.PointerId == _penID)
                    {
                        Windows.UI.Input.PointerPoint pt = e.GetCurrentPoint(MyCanvas);

                        // Pass the pointer information to the InkManager. 
                        _inkKhaled.ProcessPointerUp(pt);
                    }

                    else if (e.Pointer.PointerId == _touchID)
                    {
                        // Process touch input
                    }

                    _touchID = 0;
                    _penID = 0;

                    // Call an application-defined function to render the ink strokes.

                    e.Handled = true;
                    rec_prevcount = -1;
                }
            
                if (imageselected)//&&(img_prevcount!=-1))
                {
                    //MyCanvas.Children.RemoveAt(img_prevcount);
                    //MyCanvas.Children.Add(img);
                    //imageselected = false;
                    //img_prevcount = -1;
                    //itemselected = false;
                    img_left = img_right = img_top = img_bottom = img_move = false;
                    img_prev_left = img_prev_top = 0;
                }
                    
                ///////////////////////////////////////////////////////////////

                if (tableselected)
                {
                    tableselected = false;
                    itemselected = false;
                    tablelayoutinserted = false;
                }


                if (itemselected||MyCanvas.Children.Count>0)
                {
                    posTransform.X = Window.Current.Bounds.Width - 300;
                    posTransform.Y = -30;
                    pen_pointdown = false;
                }

                else
                {
                    //278 & 318
                    if (visible == false)
                    {
                        MainRadialMenu.Visibility = Visibility.Visible;
                        visible = true;
                    }
                    PointerPoint TappedPoint = e.GetCurrentPoint(null);
                    GeneralTransform transform = MainRadialMenu.TransformToVisual(MainGrid);
                    Point absolutePosition = transform.TransformPoint(new Point(0, 0));

                    //아직 정확도에 대한 코딩이 미완성

                    if (MainRadialMenu.IsOpen)
                    {
                        if (((TappedPoint.Position.X - absolutePosition.X) < 258) && ((TappedPoint.Position.X - absolutePosition.X) > 18))
                        {
                            if (((TappedPoint.Position.Y - absolutePosition.Y) < 299) && ((TappedPoint.Position.Y - absolutePosition.Y) > 59))
                            {
                                if (((TappedPoint.Position.X - absolutePosition.X) < 160) && ((TappedPoint.Position.X - absolutePosition.X) > 120))
                                {
                                    if (((TappedPoint.Position.Y - absolutePosition.Y) < 200) && ((TappedPoint.Position.Y - absolutePosition.Y) > 160))
                                        return;
                                }
                                else
                                    return;
                            }
                        }
                        else
                            MainRadialMenu.Close();
                    }


                    //Menu Docking Issue
                    posTransform.X = TappedPoint.Position.X - 133;
                    posTransform.Y = TappedPoint.Position.Y;
                    if (Window.Current.Bounds.Width - posTransform.X < 300)
                        posTransform.X = Window.Current.Bounds.Width - 300;
                    else if (posTransform.X < 75)
                        posTransform.X = 75;
                    if (Window.Current.Bounds.Height - posTransform.Y < 80)
                        posTransform.Y = Window.Current.Bounds.Height - 330;
                    else if (posTransform.Y < 80)
                        posTransform.Y = -30;
                    else posTransform.Y = TappedPoint.Position.Y - 173;
                }
                MainRadialMenu.RenderTransform = posTransform;
            }
        }

        private void MyCanvas_PointerMoved(object sender, PointerRoutedEventArgs e)
        {
            if (!previewselected)
            {
                if (penselected || triangleselected || rectangleselected || polygonselected || lineselected || imageselected || tablelayoutinserted)
                {
                    if (e.Pointer.PointerId == _penID)
                    {
                        TranslateTransform posTransform = new TranslateTransform();
                        posTransform.X = -400;
                        posTransform.Y = -400;
                        MainRadialMenu.RenderTransform = posTransform;

                        PointerPoint pt = e.GetCurrentPoint(MyCanvas);

                        // Render a red line on the canvas as the pointer moves. 
                        // Distance() is an application-defined function that tests
                        // whether the pointer has moved far enough to justify 
                        // drawing a new line.

                        #region 펜 그리기
                        if (penselected)
                        {
                            currentContactPt = pt.Position;
                            x1 = _previousContactPt.X;
                            y1 = _previousContactPt.Y;
                            x2 = currentContactPt.X;
                            y2 = currentContactPt.Y;
                            //if (Distance(x1, y1, x2, y2) > 2.0) // We need to developp this method now 
                            //{
                            Line line = new Line()
                            {
                                X1 = x1,
                                Y1 = y1,
                                X2 = x2,
                                Y2 = y2,
                                StrokeThickness = 4.0,
                                Stroke = new SolidColorBrush(pencolor)
                            };

                            _previousContactPt = currentContactPt;

                            // Draw the line on the canvas by adding the Line object as
                            // a child of the Canvas object.
                            MyCanvas.Children.Add(line);

                            // Pass the pointer information to the InkManager.
                            _inkKhaled.ProcessPointerUpdate(pt);
                            //}
                        }
                        #endregion

                        #region 삼각형 그리기
                        else if (triangleselected)
                        {
                            if (Distance(x1, y1, x2, y2) > 2.0) // We need to developp this method now 
                            {
                                Line line = new Line()
                                {
                                    X1 = x1,
                                    Y1 = y1,
                                    X2 = x2,
                                    Y2 = y2,
                                    StrokeThickness = 4.0,
                                    Stroke = new SolidColorBrush(Colors.Green)
                                };
                                _previousContactPt = currentContactPt;

                                // Draw the line on the canvas by adding the Line object as
                                // a child of the Canvas object.
                                MyCanvas.Children.Add(line);

                                // Pass the pointer information to the InkManager.
                                _inkKhaled.ProcessPointerUpdate(pt);
                            }
                        }
                        #endregion

                        #region 사각형 그리기
                        else if (rectangleselected)
                        {
                            int area1 = 0, area2 = 0, area3 = 0, area4 = 0;
                            currentContactPt = pt.Position;
                            x1 = _previousContactPt.X;
                            y1 = _previousContactPt.Y;
                            x2 = currentContactPt.X;
                            y2 = currentContactPt.Y;
                            Rectangle rectangle = new Rectangle();
                            if (x1 < x2) { rectangle.Width = x2 - x1; area1 = 1; area4 = 1; }
                            else { rectangle.Width = x1 - x2; area2 = 1; area3 = 1; }
                            if (y1 < y2)
                            {
                                rectangle.Height = y2 - y1;
                                area3 = 0;
                                area4 = 0;
                            }
                            else
                            {
                                rectangle.Height = y1 - y2;
                                area1 = 0;
                                area2 = 0;
                            }
                            rectangle.StrokeThickness = 4.0;
                            rectangle.Stroke = new SolidColorBrush(Colors.Green);
                            if (area1 == 1) { Canvas.SetLeft(rectangle, x1); Canvas.SetTop(rectangle, y1); }
                            else if (area2 == 1) { Canvas.SetLeft(rectangle, x2); Canvas.SetTop(rectangle, y1); }
                            else if (area3 == 1) { Canvas.SetLeft(rectangle, x2); Canvas.SetTop(rectangle, y2); }
                            else { Canvas.SetLeft(rectangle, x1); Canvas.SetTop(rectangle, y2); }

                            // Draw the line on the canvas by adding the Line object as
                            // a child of the Canvas object.

                            if (rec_prevcount != -1)
                                MyCanvas.Children.RemoveAt(rec_prevcount);
                            MyCanvas.Children.Add(rectangle);
                            rec_prevcount = MyCanvas.Children.IndexOf(rectangle);
                            // Pass the pointer information to the InkManager.
                            _inkKhaled.ProcessPointerUpdate(pt);
                        }
                        #endregion

                        #region 다각형 그리기
                        else if (polygonselected && (poly_previtem == 0))
                        {

                            if ((Distance(pt.Position.X, pt.Position.Y, poly_startPoint.X, poly_startPoint.Y) < 7.0) && (poly_edgecount > 1))
                            {
                                currentContactPt = poly_startPoint;
                                poly_startget = true;
                            }
                            else
                            {
                                currentContactPt = pt.Position;
                                poly_startget = false;
                            }
                            x1 = _previousContactPt.X;
                            y1 = _previousContactPt.Y;
                            x2 = currentContactPt.X;
                            y2 = currentContactPt.Y;

                            Line line = new Line()
                            {
                                X1 = x1,
                                Y1 = y1,
                                X2 = x2,
                                Y2 = y2,
                                StrokeThickness = 4.0,
                                Stroke = new SolidColorBrush(Colors.Black)
                            };

                            // Draw the line on the canvas by adding the Line object as
                            // a child of the Canvas object.

                            if (poly_prevcount != -1)
                                MyCanvas.Children.RemoveAt(poly_prevcount);

                            MyCanvas.Children.Add(line);
                            poly_prevcount = MyCanvas.Children.IndexOf(line);

                            // Pass the pointer information to the InkManager.
                            _inkKhaled.ProcessPointerUpdate(pt);
                        }
                        #endregion

                        #region 선 그리기
                        else if (lineselected)
                        {
                            currentContactPt = pt.Position;
                            x1 = _previousContactPt.X;
                            y1 = _previousContactPt.Y;
                            x2 = currentContactPt.X;
                            y2 = currentContactPt.Y;
                            Line line = new Line()
                            {
                                X1 = x1,
                                Y1 = y1,
                                X2 = x2,
                                Y2 = y2,
                                StrokeThickness = 4.0,
                                Stroke = new SolidColorBrush(Colors.Black)
                            };


                            // Draw the line on the canvas by adding the Line object as
                            // a child of the Canvas object.

                            if (line_prevcount != -1)
                                MyCanvas.Children.RemoveAt(line_prevcount);

                            MyCanvas.Children.Add(line);
                            line_prevcount = MyCanvas.Children.IndexOf(line);

                            // Pass the pointer information to the InkManager.
                            _inkKhaled.ProcessPointerUpdate(pt);
                        }
                        #endregion

                        #region 이미지 그리기
                        else if (imageselected)
                        {
                            currentContactPt = pt.Position;

                            Image temp_img = new Image();
                            temp_img = (Image)MyCanvas.Children.Last();
                            var location = temp_img.TransformToVisual(Window.Current.Content);
                            Point location_img = location.TransformPoint(new Point(0, 0));
                            double change_x = currentContactPt.X - _previousContactPt.X;
                            double change_y = currentContactPt.Y - _previousContactPt.Y;
                            temp_img.Stretch = Stretch.Fill;
                            if (img_move)
                            {
                                temp_img.Width = temp_img.ActualWidth;
                                temp_img.Height = temp_img.ActualHeight;
                                Canvas.SetLeft(temp_img, location_img.X + change_x);
                                Canvas.SetTop(temp_img, location_img.Y + change_y);

                                MyCanvas.Children.RemoveAt(MyCanvas.Children.Count - 1);
                                MyCanvas.Children.Add(temp_img);

                                _previousContactPt = currentContactPt;

                                _inkKhaled.ProcessPointerUpdate(pt);
                            }
                            else if (img_left || img_right || img_top || img_bottom)
                            {

                                if (img_left)
                                {
                                    if (img_prev_left == 0)
                                        img_prev_left = temp_img.ActualWidth + location_img.X;
                                    temp_img.Width = img_prev_left - currentContactPt.X;
                                    Canvas.SetLeft(temp_img, currentContactPt.X);
                                }
                                else if (img_right)
                                {
                                    temp_img.Width = currentContactPt.X - location_img.X;
                                    Canvas.SetLeft(temp_img, location_img.X);
                                }
                                else
                                {
                                    temp_img.Width = temp_img.ActualWidth;
                                    Canvas.SetLeft(temp_img, location_img.X);
                                }
                                if (img_top)
                                {
                                    if (img_prev_top == 0)
                                        img_prev_top = temp_img.ActualHeight + location_img.Y;
                                    temp_img.Height = img_prev_top - currentContactPt.Y;
                                    Canvas.SetTop(temp_img, currentContactPt.Y);
                                }
                                else if (img_bottom)
                                {
                                    temp_img.Height = currentContactPt.Y - location_img.Y;
                                    Canvas.SetTop(temp_img, location_img.Y);
                                }
                                else
                                {
                                    temp_img.Height = temp_img.ActualHeight;
                                    Canvas.SetTop(temp_img, location_img.Y);
                                }
                                MyCanvas.Children.RemoveAt(MyCanvas.Children.Count - 1);
                                MyCanvas.Children.Add(temp_img);
                                //_previousContactPt = currentContactPt;
                                _inkKhaled.ProcessPointerUpdate(pt);
                            }
                        }
                        #endregion

                        #region 표 그리기
                        else if (tablelayoutinserted)
                        {
                            int area1 = 0, area2 = 0, area3 = 0, area4 = 0;
                            currentContactPt = pt.Position;
                            x1 = _previousContactPt.X;
                            y1 = _previousContactPt.Y;
                            x2 = currentContactPt.X;
                            y2 = currentContactPt.Y;
        
                            Grid newGrid = new Grid()
                            {
                                Background = new SolidColorBrush(Colors.Wheat)
                            };

                            /////////////////////
                            int row_count = 0, column_count = 0;
                            
                            while (row_count < table_row)
                            {
                                RowDefinition row_new = new RowDefinition();
                                newGrid.RowDefinitions.Add(row_new);
                                row_count++;
                            }

                            while (column_count < table_column)
                            {
                                ColumnDefinition column_new = new ColumnDefinition();
                                newGrid.ColumnDefinitions.Add(column_new);
                                column_count++;
                            }
                            Style style_temp1 = new Style(typeof(Border));
                            Setter BorderBrushProp = new Setter()
                            {
                                Property = BorderBrushProperty,
                                Value = new SolidColorBrush(Colors.Black)
                            };
                            Setter BorderThicknessProp = new Setter()
                            {
                                Property = BorderThicknessProperty,
                                Value = 2
                            };
                            Setter BackgroundProp = new Setter()
                            {
                                Property = BackgroundProperty,
                                Value = new SolidColorBrush(Colors.White)
                            };
                            Setter PaddingProp = new Setter()
                            {
                                Property = PaddingProperty,
                                Value = 5
                            };
                            style_temp1.Setters.Add(BorderBrushProp);
                            style_temp1.Setters.Add(BorderThicknessProp);
                            style_temp1.Setters.Add(BackgroundProp);
                            style_temp1.Setters.Add(PaddingProp);

                            Style style_temp2 = new Style(typeof(TextBlock));
                            Setter ForegroundProp = new Setter()
                            {
                                Property = ForegroundProperty,
                                Value = new SolidColorBrush(Colors.Black)
                            };
                            style_temp2.Setters.Add(ForegroundProp);

                            double current_width, current_height;
                            if(currentContactPt.X > _previousContactPt.X)
                                current_width = currentContactPt.X - _previousContactPt.X;
                            else
                                current_width = _previousContactPt.X - currentContactPt.X;
                            if (currentContactPt.Y > _previousContactPt.Y)
                                current_height = currentContactPt.Y - _previousContactPt.Y;
                            else
                                current_height = _previousContactPt.Y - currentContactPt.Y;
                            
                            int table_entry_x = 0, table_entry_y=0;
                            while (table_entry_x*table_entry_y != table_row * table_column)
                            {
                                TextBox textbox_new = new TextBox()
                                {
                                    HorizontalAlignment = HorizontalAlignment.Stretch,
                                    VerticalAlignment = VerticalAlignment.Stretch,
                                    TextAlignment = TextAlignment.Center,
                                    BorderBrush = new SolidColorBrush(Colors.Black),
                                    Background = new SolidColorBrush(Colors.LightGray),
                                    TextWrapping = TextWrapping.Wrap,
                                    FontStyle = Windows.UI.Text.FontStyle.Normal,
                                    Width = current_width / table_column,
                                    Height = current_height / table_row
                                };
                               /* double width1_result = textbox_new.Width / 7;
                                if (textbox_new.Height / width1_result < 2.8)
                                    textbox_new.FontSize = width1_result * 2.4 / (textbox_new.Height / width1_result);
                                else
                                    textbox_new.FontSize = width1_result * 1.2;*/
                                Grid.SetRow(textbox_new, table_entry_x);
                                Grid.SetColumn(textbox_new, table_entry_y);
                                if (table_entry_y == table_column - 1)
                                {
                                    table_entry_y = 0;
                                    table_entry_x++;
                                }
                                else
                                    table_entry_y++;
                                newGrid.Children.Add(textbox_new);
                            }
                            
                            ////////////////


                            if (x1 < x2) { newGrid.Width = x2 - x1; area1 = 1; area4 = 1; }
                            else { newGrid.Width = x1 - x2; area2 = 1; area3 = 1; }
                            if (y1 < y2)
                            {
                                newGrid.Height = y2 - y1;
                                area3 = 0;
                                area4 = 0;
                            }
                            else
                            {
                                newGrid.Height = y1 - y2;
                                area1 = 0;
                                area2 = 0;
                            }
                            if (area1 == 1) { Canvas.SetLeft(newGrid, x1); Canvas.SetTop(newGrid, y1); }
                            else if (area2 == 1) { Canvas.SetLeft(newGrid, x2); Canvas.SetTop(newGrid, y1); }
                            else if (area3 == 1) { Canvas.SetLeft(newGrid, x2); Canvas.SetTop(newGrid, y2); }
                            else { Canvas.SetLeft(newGrid, x1); Canvas.SetTop(newGrid, y2); }

                            // Draw the line on the canvas by adding the Line object as
                            // a child of the Canvas object.

                            if (table_prevcount != -1)
                                MyCanvas.Children.RemoveAt(table_prevcount);
                            MyCanvas.Children.Add(newGrid);
                            table_prevcount = MyCanvas.Children.IndexOf(newGrid);
                            // Pass the pointer information to the InkManager.
                            _inkKhaled.ProcessPointerUpdate(pt);
                            tableselected = true;
                        }
                        #endregion
                    }
                    
                    if (imageselected)
                    {
                        currentContactPt = e.GetCurrentPoint(MyCanvas).Position;
                        Image temp_img = (Image)MyCanvas.Children.Last();
                        var location = temp_img.TransformToVisual(Window.Current.Content);
                        Point location_img = location.TransformPoint(new Point(0, 0));
                        double left = location_img.X,
                            right = location_img.X + temp_img.ActualWidth,
                            top = location_img.Y,
                            bottom = location_img.Y + temp_img.ActualHeight;

                        if (Math.Abs(currentContactPt.X - left) < 3)
                        {
                            if (Math.Abs(currentContactPt.Y - top) < 3)
                                Window.Current.CoreWindow.PointerCursor = new Windows.UI.Core.CoreCursor(Windows.UI.Core.CoreCursorType.SizeNorthwestSoutheast, 1);
                            else if (Math.Abs(currentContactPt.Y - bottom) < 3)
                                Window.Current.CoreWindow.PointerCursor = new Windows.UI.Core.CoreCursor(Windows.UI.Core.CoreCursorType.SizeNortheastSouthwest, 1);
                            else
                                Window.Current.CoreWindow.PointerCursor = new Windows.UI.Core.CoreCursor(Windows.UI.Core.CoreCursorType.SizeWestEast, 1);
                        }

                        else if (Math.Abs(currentContactPt.X - right) < 3)
                        {
                            if (Math.Abs(currentContactPt.Y - top) < 3)
                                Window.Current.CoreWindow.PointerCursor = new Windows.UI.Core.CoreCursor(Windows.UI.Core.CoreCursorType.SizeNortheastSouthwest, 1);
                            else if (Math.Abs(currentContactPt.Y - bottom) < 3)
                                Window.Current.CoreWindow.PointerCursor = new Windows.UI.Core.CoreCursor(Windows.UI.Core.CoreCursorType.SizeNorthwestSoutheast, 1);
                            else Window.Current.CoreWindow.PointerCursor = new Windows.UI.Core.CoreCursor(Windows.UI.Core.CoreCursorType.SizeWestEast, 1);
                        }

                        else if ((Math.Abs(currentContactPt.Y - top) < 3) || (Math.Abs(currentContactPt.Y - bottom) < 3))
                            Window.Current.CoreWindow.PointerCursor = new Windows.UI.Core.CoreCursor(Windows.UI.Core.CoreCursorType.SizeNorthSouth, 1);

                        else if (((currentContactPt.X - left >= 3) && (right - currentContactPt.X >= 3))
                            && ((currentContactPt.Y - top >= 3) && (bottom - currentContactPt.Y >= 3)))
                            Window.Current.CoreWindow.PointerCursor = new Windows.UI.Core.CoreCursor(Windows.UI.Core.CoreCursorType.SizeAll, 1);

                        else Window.Current.CoreWindow.PointerCursor = new Windows.UI.Core.CoreCursor(Windows.UI.Core.CoreCursorType.Arrow, 1);
                    }
                    else Window.Current.CoreWindow.PointerCursor = new Windows.UI.Core.CoreCursor(Windows.UI.Core.CoreCursorType.Arrow, 1);
                }
                int i = textbox_list.Count;
                /*while (i != 0)
                {
                    if (textbox_list.ElementAt(i - 1).Focus(Windows.UI.Xaml.FocusState.Pointer))
                        textbox_list.ElementAt(i - 1).Background = new SolidColorBrush(Colors.Green);
                    else textbox_list.ElementAt(i - 1).Background = new SolidColorBrush(Colors.LightGray);
                    i--;
                }*/
                if ((textbox_list.Count != 0) && textboxselected && (textbox_list.Last().Text != null))
                    textbox_list.Last().Focus(Windows.UI.Xaml.FocusState.Pointer);
            }
        }

        private void MyCanvas_PointerPressed(object sender, PointerRoutedEventArgs e)
        {
            PointerPoint pt = e.GetCurrentPoint(MyCanvas);
            currentContactPt = pt.Position;
            if (!previewselected) { 
            if (penselected || rectangleselected || triangleselected || imageselected || tablelayoutinserted)
            {
                // Get information about the pointer location.
                
                _previousContactPt = pt.Position;

                // Accept input only from a pen or mouse with the left button pressed. 
                PointerDeviceType pointerDevType = e.Pointer.PointerDeviceType;
//                if (pointerDevType == PointerDeviceType.Pen ||
  //                      pointerDevType == PointerDeviceType.Mouse &&
    //                    pt.Properties.IsLeftButtonPressed)
      //          {
                    // Pass the pointer information to the InkManager.
                    _inkKhaled.ProcessPointerDown(pt);
                    _penID = pt.PointerId;

                    e.Handled = true;
        //        }

          //      else if (pointerDevType == PointerDeviceType.Touch)
            //    {
                    // Process touch input
              //  }
                    if (tablelayoutinserted)
                        table_prevcount = -1;
            }

            ///////////////////////////////

          
            
            //////////////////////////////////////////

            if (polygonselected)
            {
                polygonPoints.Add(pt.Position);
                if (poly_previtem == -1 && poly_start == -1)
                {
                    if (poly_start == -1)
                        poly_startPoint = pt.Position;
                    _previousContactPt = pt.Position;
                    poly_start = 0;
                    poly_previtem = 0;

                    // Accept input only from a pen or mouse with the left button pressed. 
                    PointerDeviceType pointerDevType = e.Pointer.PointerDeviceType;
//                    if (pointerDevType == PointerDeviceType.Pen ||
  //                          pointerDevType == PointerDeviceType.Mouse &&
    //                        current.Properties.IsLeftButtonPressed)
      //              {
                        // Pass the pointer information to the InkManager.
                        _inkKhaled.ProcessPointerDown(pt);
                        _penID = pt.PointerId;

                        e.Handled = true;
        //            }

          //          else if (pointerDevType == PointerDeviceType.Touch)
            //        {
                        // Process touch input
              //      }
                }
                else if (poly_previtem == 0)
                {

                    if (e.Pointer.PointerId == _penID)
                    {

                        // Pass the pointer information to the InkManager. 
                        _inkKhaled.ProcessPointerUp(pt);
                    }

                    else if (e.Pointer.PointerId == _touchID)
                    {
                        // Process touch input
                    }

                    _touchID = 0;
                    _penID = 0;

                    // Call an application-defined function to render the ink strokes.


                    e.Handled = true;
                    rec_prevcount = -1;

                    ///////////////////////////////////////////////////////////

                    // Accept input only from a pen or mouse with the left button pressed. 
                    PointerDeviceType pointerDevType = e.Pointer.PointerDeviceType;
//                    if (pointerDevType == PointerDeviceType.Pen ||
  //                          pointerDevType == PointerDeviceType.Mouse &&
    //                        current.Properties.IsLeftButtonPressed)
      //              {
                        // Pass the pointer information to the InkManager.
                        _inkKhaled.ProcessPointerDown(pt);
                        _penID = pt.PointerId;

                        e.Handled = true;
        //            }

          //          else if (pointerDevType == PointerDeviceType.Touch)
            //        {
                        // Process touch input
              //      }
                    /////////////////////////////////////////////////////////////////
                    if (poly_startget)
                        currentContactPt = poly_startPoint;
                    else currentContactPt = pt.Position;
                    x1 = _previousContactPt.X;
                    y1 = _previousContactPt.Y;
                    x2 = currentContactPt.X;
                    y2 = currentContactPt.Y;
                    Line line = new Line()
                    {
                        X1 = x1,
                        Y1 = y1,
                        X2 = x2,
                        Y2 = y2,
                        StrokeThickness = 4.0,
                        Stroke = new SolidColorBrush(Colors.Black)
                    };

                    // Draw the line on the canvas by adding the Line object as
                    // a child of the Canvas object.
                    MyCanvas.Children.RemoveAt(MyCanvas.Children.Count - 1);
                    MyCanvas.Children.Add(line);

                    // Pass the pointer information to the InkManager.
                    _inkKhaled.ProcessPointerUpdate(pt);

                    poly_edgecount++;
                    if (poly_startPoint.X == currentContactPt.X && poly_startPoint.Y == currentContactPt.Y)
                    {
                        
                        Polygon newshape = new Polygon()
                            {
                                StrokeThickness = 2,
                                Stroke = new SolidColorBrush(Colors.Green)
                            };
                        int count = MyCanvas.Children.Count;
                        int i = MyCanvas.Children.Count - poly_edgecount,
                            j=0;
                        polygonPoints.RemoveAt(polygonPoints.Count - 1);
                        while (i != count)
                        {
                            newshape.Points.Add(polygonPoints.ElementAt(j));
                            MyCanvas.Children.RemoveAt(MyCanvas.Children.Count-1);
                            i++;
                            j++;
                        }
                        MyCanvas.Children.Add(newshape);
                        List_item.Add(new customitems(MyCanvas.Children.IndexOf(newshape), null));
                        newshape.PointerPressed += newshape_PointerPressed;
                        newshape.PointerEntered += newshape_PointerEntered;

                        TextBox newbox = new TextBox()
                        {
                            Width = 200,
                            Height = 16,
                            Background = new SolidColorBrush(Colors.LightGray),
                            BorderBrush = new SolidColorBrush(Colors.Black)
                        };
                        Canvas.SetLeft(newbox, currentContactPt.X + 10);
                        Canvas.SetTop(newbox, currentContactPt.Y - 10);

                        Button Okbtn = new Button()
                        {
                            Width = 60,
                            Height = 30,
                            FontSize = 10,
                            FontStyle = Windows.UI.Text.FontStyle.Normal,
                            Content="OK",
                            Background = new SolidColorBrush(Colors.LightGray),
                            BorderBrush = new SolidColorBrush(Colors.Black)
                        };
                        Canvas.SetLeft(Okbtn, currentContactPt.X + 220);
                        Canvas.SetTop(Okbtn, currentContactPt.Y - 10);
                        MyCanvas.Children.Add(newbox);
                        MyCanvas.Children.Add(Okbtn);
                        Okbtn.Click += Okbtn_Click;

                        polygonPoints.Clear();
                       
                        poly_start = -1;
                        poly_previtem = -1;
                        poly_edgecount = 0;
                        Drawing_Initialize(sender, e);
                        poly_startget = false;
                    }
                    else {
                        _previousContactPt = pt.Position;
                    }
                    poly_prevcount = -1;
                }
            }

            if (lineselected)
            {
                if (line_start == -1)
                {
                    _previousContactPt = pt.Position;
                    line_start = 0;

                    // Accept input only from a pen or mouse with the left button pressed. 
                    PointerDeviceType pointerDevType = e.Pointer.PointerDeviceType;
//                    if (pointerDevType == PointerDeviceType.Pen ||
  //                          pointerDevType == PointerDeviceType.Mouse &&
    //                        current.Properties.IsLeftButtonPressed)
      //              {
                        // Pass the pointer information to the InkManager.
                        _inkKhaled.ProcessPointerDown(pt);
                        _penID = pt.PointerId;

                        e.Handled = true;
        //            }

          //          else if (pointerDevType == PointerDeviceType.Touch)
            //        {
                        // Process touch input
              //      }
                }
                else 
                {
                    if (e.Pointer.PointerId == _penID)
                    {

                        // Pass the pointer information to the InkManager. 
                        _inkKhaled.ProcessPointerUp(pt);
                    }

                    else if (e.Pointer.PointerId == _touchID)
                    {
                        // Process touch input
                    }

                    _touchID = 0;
                    _penID = 0;

                    // Call an application-defined function to render the ink strokes.

                    e.Handled = true;
                    rec_prevcount = -1;

                    ///////////////////////////////////////////////////////////

                    // Accept input only from a pen or mouse with the left button pressed. 
                    PointerDeviceType pointerDevType = e.Pointer.PointerDeviceType;
//                    if (pointerDevType == PointerDeviceType.Pen ||
  //                          pointerDevType == PointerDeviceType.Mouse &&
    //                        current.Properties.IsLeftButtonPressed)
      //              {
                        // Pass the pointer information to the InkManager.
                        _inkKhaled.ProcessPointerDown(pt);
                        _penID = pt.PointerId;

                        e.Handled = true;
        //            }

          //          else if (pointerDevType == PointerDeviceType.Touch)
            //        {
                        // Process touch input
              //      }
                    /////////////////////////////////////////////////////////////////
                    
                    x1 = _previousContactPt.X;
                    y1 = _previousContactPt.Y;
                    x2 = currentContactPt.X;
                    y2 = currentContactPt.Y;
                    Line line = new Line()
                    {
                        X1 = x1,
                        Y1 = y1,
                        X2 = x2,
                        Y2 = y2,
                        StrokeThickness = 4.0,
                        Stroke = new SolidColorBrush(Colors.Green)
                    };

                    // Draw the line on the canvas by adding the Line object as
                    // a child of the Canvas object.
                    MyCanvas.Children.RemoveAt(MyCanvas.Children.Count - 1);
                    MyCanvas.Children.Add(line);

                    // Pass the pointer information to the InkManager.
                    _inkKhaled.ProcessPointerUpdate(pt);

                    line_start = -1;
                    line_prevcount = -1;
                    Drawing_Initialize(sender, e);
                }
            }

            if (textboxselected)
            {
                TextBox text_box = new TextBox()
                {
                    Width = 200,
                    Height = 30,
                };
                Canvas.SetLeft(text_box, e.GetCurrentPoint(MyCanvas).Position.X);
                Canvas.SetTop(text_box, e.GetCurrentPoint(MyCanvas).Position.Y);
                text_box.Background = new SolidColorBrush(Colors.LightGray);
                
                Button Text_Okbtn = new Button()
                {
                    Width = 60,
                    Height = 30,
                    FontSize = 10,
                    FontStyle = Windows.UI.Text.FontStyle.Normal,
                    Content = "OK",
                    Background = new SolidColorBrush(Colors.LightGray),
                    BorderBrush = new SolidColorBrush(Colors.Black)
                };
                Canvas.SetLeft(Text_Okbtn, e.GetCurrentPoint(MyCanvas).Position.X + 210);
                Canvas.SetTop(Text_Okbtn, e.GetCurrentPoint(MyCanvas).Position.Y);
                
                Text_Okbtn.Click += Text_Okbtn_Click;

                Button Text_Cancelbtn = new Button()
                {
                    Width = 60,
                    Height = 30,
                    FontSize = 10,
                    FontStyle = Windows.UI.Text.FontStyle.Normal,
                    Content = "Cancel",
                    Background = new SolidColorBrush(Colors.LightGray),
                    BorderBrush = new SolidColorBrush(Colors.Black)
                };

                Canvas.SetLeft(Text_Cancelbtn, e.GetCurrentPoint(MyCanvas).Position.X + 275);
                Canvas.SetTop(Text_Cancelbtn, e.GetCurrentPoint(MyCanvas).Position.Y);

                Text_Cancelbtn.Click += Text_Cancelbtn_Click;

                MyCanvas.Children.Add(text_box);
                MyCanvas.Children.Add(Text_Okbtn);
                MyCanvas.Children.Add(Text_Cancelbtn);
                textbox_list.Add(text_box);
                //text_box.Focus(Windows.UI.Xaml.FocusState.Keyboard);
                textbox_list.Last().Focus(Windows.UI.Xaml.FocusState.Pointer);   
            }

            if (imageselected)
            {
                int count = MyCanvas.Children.Count;
                Image temp_img = (Image)MyCanvas.Children.Last();
                /*double top = ((Window.Current.Bounds.Height - temp_img.Height) / 2);
                double bottom = temp_img.Height + top;
                double left = ((Window.Current.Bounds.Width - temp_img.Width) / 2);
                double right = temp_img.Width + left;*/
                var location = temp_img.TransformToVisual(Window.Current.Content);
                Point location_img = location.TransformPoint(new Point(0, 0));
                double left = location_img.X,
                    right = location_img.X + temp_img.ActualWidth,
                    top = location_img.Y,
                    bottom = location_img.Y + temp_img.ActualHeight;

                if (Math.Abs(pt.Position.X - left) < 3) img_left = true;

                if (Math.Abs(pt.Position.X - right) < 3) img_right = true;

                if (Math.Abs(pt.Position.Y - top) < 3) img_top = true;

                if (Math.Abs(pt.Position.Y - bottom) < 3) img_bottom = true;

                if (((pt.Position.X - left >= 3) && (right - pt.Position.X >= 3))
                    && ((pt.Position.Y - top >= 3) && (bottom - pt.Position.Y >= 3)))
                    img_move = true;
                }   
            }
        }


        void newshape_PointerEntered(object sender, PointerRoutedEventArgs e)
        {
            if (previewselected) { 
                testsound.Play();
            }
        }

        void Okbtn_Click(object sender, RoutedEventArgs e)
        {
            TextBox temp =  (TextBox)MyCanvas.Children.ElementAt(MyCanvas.Children.Count - 2);
            string tts_input = temp.Text;
            MyCanvas.Children.RemoveAt(MyCanvas.Children.Count - 1);
            MyCanvas.Children.RemoveAt(MyCanvas.Children.Count - 1);
            Polygon ab = (Polygon)MyCanvas.Children.ElementAt(MyCanvas.Children.Count - 1);
            ab.Stroke = new SolidColorBrush(Colors.Blue);
        }

        void Text_Okbtn_Click(object sender, RoutedEventArgs e)
        {
            TextBox temp = (TextBox)MyCanvas.Children.ElementAt(MyCanvas.Children.Count - 3);
            string tts_input = temp.Text;
            MyCanvas.Children.RemoveAt(MyCanvas.Children.Count - 1);
            textbox_list.Last().Focus(Windows.UI.Xaml.FocusState.Pointer);
            //MyCanvas.Children.RemoveAt(MyCanvas.Children.Count - 1);
            //Polygon ab = (Polygon)MyCanvas.Children.ElementAt(MyCanvas.Children.Count - 1);
            //ab.Stroke = new SolidColorBrush(Colors.Blue);
        }

        void Text_Cancelbtn_Click(object sender, RoutedEventArgs e)
        {
            MyCanvas.Children.RemoveAt(MyCanvas.Children.Count - 1);
            MyCanvas.Children.RemoveAt(MyCanvas.Children.Count - 1);
            MyCanvas.Children.RemoveAt(MyCanvas.Children.Count - 1);
        }

        void newshape_PointerPressed(object sender, PointerRoutedEventArgs e)
        {
            if (!previewselected)
            {
                int i = 0;
                int target;
                Polygon temp = sender as Polygon;
                while (i < List_item.Count)
                {
                    if (MyCanvas.Children.IndexOf(temp) == List_item.ElementAt(i).get_index())
                    {
                        target = List_item.ElementAt(i).get_index();
                        break;
                    }
                }
            }

            /* TTS 읽어주기.. */
        }

        async void OpenImage()
        {
            if (true)//EnsureUnsnapped())
            {
                FileOpenPicker openPicker = new FileOpenPicker();
                openPicker.ViewMode = PickerViewMode.Thumbnail;
                openPicker.SuggestedStartLocation = PickerLocationId.PicturesLibrary;

                openPicker.FileTypeFilter.Clear();
                openPicker.FileTypeFilter.Add(".jpg");
                openPicker.FileTypeFilter.Add(".jpeg");
                openPicker.FileTypeFilter.Add(".png");
                openPicker.FileTypeFilter.Add(".bmp");

                StorageFile file = await openPicker.PickSingleFileAsync();
                if (file != null)
                {
                    // Application now has read/write access to the picked file
                    loadedImage = file;
                    var stream = await file.OpenAsync(Windows.Storage.FileAccessMode.Read);
                    using (IRandomAccessStream fileStream = await file.OpenAsync(Windows.Storage.FileAccessMode.Read))
                    {
                        // Set the image source to the selected bitmap
                        BitmapImage bitmapImage = new BitmapImage();

                        await bitmapImage.SetSourceAsync(fileStream);// bitmapImage.UriSource.ToString();
                        
                        Image temp_img = new Image();
                        temp_img.Source = bitmapImage;
                        double height = bitmapImage.PixelHeight;
                        double width = bitmapImage.PixelWidth;

                        if (Window.Current.Bounds.Height - 100 < height)
                        {
                            temp_img.Height = Window.Current.Bounds.Height - 100;
                            height = Window.Current.Bounds.Height - 100;
                        }
                        else temp_img.Height = bitmapImage.PixelHeight;

                        if (Window.Current.Bounds.Width - 100 < width)
                        {
                            temp_img.Width = Window.Current.Bounds.Width - 100;
                            width = Window.Current.Bounds.Width - 100;
                        }
                        else temp_img.Width = bitmapImage.PixelWidth;

                        temp_img.Stretch = Stretch.UniformToFill;
                        Canvas.SetTop(temp_img,(Window.Current.Bounds.Height-height)/2);
                        Canvas.SetLeft(temp_img,(Window.Current.Bounds.Width-width)/2);
                        MyCanvas.Children.Add(temp_img);
                        imageselected = true;
                        itemselected = true;
                    }
                }
                else
                    loadedImage = null;
            }
        }

        private double Distance(double x1, double y1, double x2, double y2)
        {
            double d = 0;
            d = Math.Sqrt(Math.Pow((x2 - x1), 2) + Math.Pow((y2 - y1), 2));
            return d;
        }


        private void ClearAllItem_Click(object sender, RoutedEventArgs e)
        {
            int i = MyCanvas.Children.Count;
            while (MyCanvas.Children.Count != 0) { 
                MyCanvas.Children.RemoveAt(i - 1);
                i--;
            }
            AllPropertiesToFalse();
            itemselected = false;
        }

        private void PenItem_Click(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {
            // TODO: Add event handler implementation here.
            AllPropertiesToFalse();
            penselected = true;
            itemselected = true;
            RadialColorPicker temp = sender as RadialColorPicker;
            /*DependencyProperty SpeciesProperty = DependencyProperty.Register(
                "Species",
                typeof(String),
                typeof(Fish), null
            );*/
            pencolor = temp.Color;
            MainRadialMenu.Close();
            BitmapImage bm = new BitmapImage(new Uri("ms-appx:///Assets/Radial/penM.png", UriKind.RelativeOrAbsolute));
            SelectedMenu.Source = bm;
        }

        private void PolygonItem_Click(object sender, RoutedEventArgs e)
        {
            AllPropertiesToFalse();
            polygonselected = true;
            itemselected = true;
            MainRadialMenu.Close();
            
            //MainRadialMenu.SetBinding(RadialObject.BodyBrushProperty,new Binding{Source=, Path="/Assets/Radial/remove.png"})
        }

        private void TriangleItem_Click(object sender, RoutedEventArgs e)
        {
            AllPropertiesToFalse();
            triangleselected = true;
            itemselected = true;
            MainRadialMenu.Close();
        }

        private void RectangleItem_Click(object sender, RoutedEventArgs e)
        {
            AllPropertiesToFalse();
            rectangleselected = true;
            itemselected = true;
            MainRadialMenu.Close();
        }
        private void LineItem_Click(object sender, RoutedEventArgs e)
        {
            AllPropertiesToFalse();
            lineselected = true;
            itemselected = true;
            MainRadialMenu.Close();
        }

        private void TextItem_Click(object sender, RoutedEventArgs e)
        {
            AllPropertiesToFalse();
            textboxselected = true;
            itemselected = true;
            MainRadialMenu.Close();
        }

        private void ImageItem_Click(object sender, RoutedEventArgs e)
        {
            AllPropertiesToFalse();
            OpenImage();
            MainRadialMenu.Close();
        }
        private void TableItem_Click(object sender, RoutedEventArgs e)
        {
            // TODO: Add event handler implementation here.
            AllPropertiesToFalse();
            MainRadialMenu.Close();
            Table_MessageBox(currentContactPt.X, currentContactPt.Y);
            itemselected = true;
        }

        void AllPropertiesToFalse()
        {
            itemselected = penselected = pen_pointdown = polygonselected = triangleselected = rectangleselected = lineselected = textboxselected = imageselected = selectselected = false;
        }

        private void MainGrid_KeyDown(object sender, KeyRoutedEventArgs e)
        {
            if (e.Key == VirtualKey.Escape && MainRadialMenu.IsOpen)
                MainRadialMenu.Close();
        }

        void MainRadialMenu_KeyDown(object sender, KeyRoutedEventArgs e)
        {
            if (e.Key == VirtualKey.Escape && MainRadialMenu.IsOpen)
                MainRadialMenu.Close();
        }

        void MyCanvas_KeyDown(object sender, KeyRoutedEventArgs e)
        {
            if (e.Key == VirtualKey.Escape && MainRadialMenu.IsOpen)
                MainRadialMenu.Close();
        }

        private void PreviewBtn_Click(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {
        	// TODO: Add event handler implementation here.
            MainRadialMenu.Visibility = Visibility.Collapsed;
            previewselected = true;
            MyCanvas.Background = new SolidColorBrush(Colors.Black);
            Polygon temp = (Polygon)MyCanvas.Children.Last();
            temp.Stroke = new SolidColorBrush(Colors.Transparent);
        }

        private void Table_MessageBox(double X, double Y)
        {
            Style style = new Style(typeof(Border));
            style.BasedOn = new Style(typeof(Border));
            Setter BorderBrushProp = new Setter()
            {
                Property = BorderBrushProperty,
                Value = new SolidColorBrush(Colors.Black)
            };
            Setter BorderThicknessProp = new Setter()
            {
                Property = BorderThicknessProperty,
                Value = 2
            };
            Setter BackgroundProp = new Setter()
            {
                Property = BackgroundProperty,
                Value = new SolidColorBrush(Colors.White)
            };
            Setter PaddingProp = new Setter()
            {
                Property = PaddingProperty,
                Value = 5
            };
            style.Setters.Add(BorderBrushProp);
            style.Setters.Add(BorderThicknessProp);
            style.Setters.Add(BackgroundProp);
            style.Setters.Add(PaddingProp);

            Grid table_input = new Grid()
            {
                Width = 150,
                Height = 140
            };

            GridLength length_40 = new GridLength(40);
            GridLength length_10 = new GridLength(10);
            GridLength length_70 = new GridLength(70);

            RowDefinition row1 = new RowDefinition();
            row1.Height = length_40;
            RowDefinition row2 = new RowDefinition();
            row2.Height = length_10;
            RowDefinition row3 = new RowDefinition();
            row3.Height = length_40;
            RowDefinition row4 = new RowDefinition();
            row4.Height = length_10;
            RowDefinition row5 = new RowDefinition();
            row5.Height = length_40;
            
            ColumnDefinition col1 = new ColumnDefinition();
            col1.Width = length_70;
            ColumnDefinition col2 = new ColumnDefinition();
            col2.Width = length_10;
            ColumnDefinition col3 = new ColumnDefinition();
            col3.Width = length_70;
            ColumnDefinition temp1 = new ColumnDefinition();
            ColumnDefinition temp2 = new ColumnDefinition();
            table_input.RowDefinitions.Add(row1);
            table_input.RowDefinitions.Add(row2);
            table_input.RowDefinitions.Add(row3);
            table_input.RowDefinitions.Add(row4);
            table_input.RowDefinitions.Add(row5);
            table_input.ColumnDefinitions.Add(temp1);
            table_input.ColumnDefinitions.Add(col1);
            table_input.ColumnDefinitions.Add(col2);
            table_input.ColumnDefinitions.Add(col3);
            table_input.ColumnDefinitions.Add(temp2);
            
            TextBox row = new TextBox()
            {
                VerticalAlignment = Windows.UI.Xaml.VerticalAlignment.Center,
                HorizontalAlignment = Windows.UI.Xaml.HorizontalAlignment.Center,
                Width = 70,
                Height = 38,
                Text = "행 : ",
                FontSize = 20,
                TextAlignment = TextAlignment.Center
            };
            Grid.SetRow(row, 0);
            Grid.SetColumn(row, 1);
            TextBox row_input = new TextBox()
            {
                VerticalAlignment = Windows.UI.Xaml.VerticalAlignment.Center,
                HorizontalAlignment = Windows.UI.Xaml.HorizontalAlignment.Center,
                Width = 70,
                Height = 38,
                Background = new SolidColorBrush(Colors.LightGray),
                FontSize = 20,
                TextAlignment = TextAlignment.Center
            };
            Grid.SetRow(row_input, 0);
            Grid.SetColumn(row_input, 3);
            TextBox col = new TextBox()
            {
                VerticalAlignment = Windows.UI.Xaml.VerticalAlignment.Center,
                HorizontalAlignment = Windows.UI.Xaml.HorizontalAlignment.Center,
                Width = 70,
                Height = 38,
                Text = "열 : ",
                FontSize = 20,
                TextAlignment = TextAlignment.Center
            };
            Grid.SetRow(col, 2);
            Grid.SetColumn(col, 1);
            TextBox column_input = new TextBox()
            {
                VerticalAlignment = Windows.UI.Xaml.VerticalAlignment.Center,
                HorizontalAlignment = Windows.UI.Xaml.HorizontalAlignment.Center,
                Width = 70,
                Height = 38,
                Background = new SolidColorBrush(Colors.LightGray),
                FontSize = 20,
                TextAlignment = TextAlignment.Center
            };
            Grid.SetRow(column_input, 2);
            Grid.SetColumn(column_input, 3);
            Button Ok = new Button()
            {
                VerticalAlignment = Windows.UI.Xaml.VerticalAlignment.Center,
                HorizontalAlignment = Windows.UI.Xaml.HorizontalAlignment.Center,
                Width = 70,
                Height = 38,
                BorderBrush = new SolidColorBrush(Colors.Black),
                Background = new SolidColorBrush(Colors.LightGray),
                Content = "확인",
                FontSize = 15
            };
            Grid.SetRow(Ok, 4);
            Grid.SetColumn(Ok, 1);
            Button Cancel = new Button()
            {
                VerticalAlignment = Windows.UI.Xaml.VerticalAlignment.Center,
                HorizontalAlignment = Windows.UI.Xaml.HorizontalAlignment.Center,
                Width = 70,
                Height = 38,
                BorderBrush = new SolidColorBrush(Colors.Black),
                Background = new SolidColorBrush(Colors.LightGray),
                Content = "취소",
                FontSize = 15,
            };
            Grid.SetRow(Cancel, 4);
            Grid.SetColumn(Cancel, 3);

            table_input.Children.Add(row);
            table_input.Children.Add(row_input);
            table_input.Children.Add(col);
            table_input.Children.Add(column_input);
            table_input.Children.Add(Ok);
            table_input.Children.Add(Cancel);

            Ok.Click += Ok_Click;
            Cancel.Click += Cancel_Click;
            Canvas.SetLeft(table_input, X - 100);
            Canvas.SetTop(table_input, Y - 50);
            MyCanvas.Children.Add(table_input);

            //table_input.Style = style;

            havetobeclosed = true;
        }

        async void Ok_Click(object sender, RoutedEventArgs e)
        {
            Grid temp_grid = (Grid)MyCanvas.Children.Last();
            TextBox row_input = (TextBox)temp_grid.Children.ElementAt(1);
            TextBox column_input = (TextBox)temp_grid.Children.ElementAt(3);
            if ((row_input.Text == "") || (column_input.Text == ""))
            {
                MessageDialog showerror = new MessageDialog("행과 열의 갯수를 모두 입력하지 않으셨습니다.");
                showerror.Commands.Add(new UICommand("OK"));
                showerror.DefaultCommandIndex = 0;
                showerror.CancelCommandIndex = 1;
                await showerror.ShowAsync();
                //코딩미완성   20140511
            }
            else
            {
                table_row = double.Parse(row_input.Text);
                table_column = double.Parse(column_input.Text);
                if (MyCanvas.Children.Count() != 0)
                    MyCanvas.Children.RemoveAt(MyCanvas.Children.Count() - 1);
                havetobeclosed = false;
                tablelayoutinserted = true;
            }
        }

        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            itemselected = false;
            if (MyCanvas.Children.Count() != 0)
                MyCanvas.Children.RemoveAt(MyCanvas.Children.Count() - 1);
            havetobeclosed = false;
        }

        async void Ok_PointerPressed(object sender, PointerRoutedEventArgs e)
        {
            Grid temp_grid = (Grid)MyCanvas.Children.Last();
            TextBox row_input = (TextBox)temp_grid.Children.ElementAt(1);
            TextBox column_input = (TextBox)temp_grid.Children.ElementAt(3);
            if((row_input.Text=="")||(column_input.Text==""))
            {
                MessageDialog showerror = new MessageDialog("행과 열의 갯수를 모두 입력하지 않으셨습니다.");
                showerror.Commands.Add(new UICommand("OK"));
                showerror.DefaultCommandIndex = 0;
                showerror.CancelCommandIndex = 1;
                await showerror.ShowAsync();
                //코딩미완성   20140511
            }
            else
            {
                table_row = double.Parse(row_input.Text);
                table_column = double.Parse(column_input.Text);
                if (MyCanvas.Children.Count() != 0)
                    MyCanvas.Children.RemoveAt(MyCanvas.Children.Count()-1);
                havetobeclosed = false;
                tablelayoutinserted = true;
            }
        }

        void Cancel_PointerPressed(object sender, PointerRoutedEventArgs e)
        {
            itemselected = false;
            if (MyCanvas.Children.Count()!=0)
                MyCanvas.Children.RemoveAt(MyCanvas.Children.Count()-1);
            havetobeclosed = false;
        }
        private void SaveBtn_Click(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {
            // TODO: 여기에 구현된 이벤트 처리기를 추가하십시오.
            //string new_doc = Serialize(MyCanvas);
            
            /*MessageDialog savedialog = new MessageDialog("파일 이름 : ");
            savedialog.Title = "파일 저장";
            savedialog.Commands.Add(new UICommand("OK"));
            UICommand a = new UICommand();
            savedialog.DefaultCommandIndex = 0;
            savedialog.CancelCommandIndex = 1;
            await savedialog.ShowAsync();*/
            
            

            if (SaveBtn.Content.Equals("Save"))
            {
                SaveBtn.Content = "Cancel";
                SaveGrid.Visibility = Windows.UI.Xaml.Visibility.Visible;
            }
            else
            {
                SaveBtn.Content = "Save";
                SaveGrid.Visibility = Windows.UI.Xaml.Visibility.Collapsed;
                FileNameBox.Text = "";
            }
        }

        private async void SavefileBtn_Click(object sender, RoutedEventArgs e)
        {
            if (FileNameBox.Text == "")
            {
                MessageDialog showerror = new MessageDialog("파일 이름을 입력하지 않으셨습니다.");
                showerror.Commands.Add(new UICommand("OK"));
                showerror.DefaultCommandIndex = 0;
                showerror.CancelCommandIndex = 1;
                await showerror.ShowAsync();
            }
            else
            {
                string filename = FileNameBox.Text;
                
                SaveAsync(filename, MyCanvas);
            }

        }
       
        public static string Serialize(object obj)
        {
            using (MemoryStream memoryStream = new MemoryStream())
            using (StreamReader reader = new StreamReader(memoryStream))
            {
                DataContractSerializer serializer = new DataContractSerializer(obj.GetType());
                serializer.WriteObject(memoryStream, obj);
                memoryStream.Position = 0;
                return reader.ReadToEnd();
            }
            
        }
        
        public static object Deserialize(string xml, Type toType)
        {
            using (Stream stream = new MemoryStream())
            {
                byte[] data = System.Text.Encoding.UTF8.GetBytes(xml);
                stream.Write(data, 0, data.Length);
                stream.Position = 0;
                DataContractSerializer deserializer = new DataContractSerializer(toType);
                return deserializer.ReadObject(stream);
            }
        }


        public static async void SaveAsync(string filename, object obj)
        {
            /*
            //this use the Local folder but can be other
            
            var applicationData = ApplicationData.Current;
            var storageFolder = applicationData.LocalFolder;

            var file = await storageFolder.CreateFileAsync("User.xml", CreationCollisionOption.ReplaceExisting);

            var dataContractSerializer = new DataContractSerializer(obj.GetType());
            var memoryStream = new MemoryStream();
            dataContractSerializer.WriteObject(memoryStream, obj);
            memoryStream.Seek(0, SeekOrigin.Begin);

            string serialized = new StreamReader(memoryStream).ReadToEnd();
            memoryStream.Dispose();


            var doc = new XmlDocument();
            doc.LoadXml(serialized);

            await doc.SaveToFileAsync(file);
            */


            MessageDialog dialog;
            // Declare Variables
            StorageFolder storageFolder = Windows.Storage.ApplicationData.Current.LocalFolder;
            StorageFile xmlFile = null;

            try
            {
                xmlFile = await storageFolder.GetFileAsync("filePathList.xml");
            }
            catch (FileNotFoundException ex)
            {
                string message = ex.Message;
            }

            // If xml file doesn't exist, make it with initial setting
            if (xmlFile == null)
            {
                String xml = "<?xml version=\"1.0\" encoding=\"utf-8\" ?><files></files>";
                var doc = new XmlDocument();
                doc.LoadXml(xml);

                // save xml to a file
                xmlFile = await storageFolder.CreateFileAsync("filePathList.xml"
                    , Windows.Storage.CreationCollisionOption.GenerateUniqueName);
                await doc.SaveToFileAsync(xmlFile);

                // Add permission to the files
                StorageApplicationPermissions.FutureAccessList.Add(xmlFile);
            }

            // Make XmlDom and init variable with the file
            XmlDocument xmlDoc = await XmlDocument.LoadFromFileAsync(xmlFile);
            XmlElement root = xmlDoc.DocumentElement;

            // Check duplicated name
            XmlNodeList fileNodeList = root.GetElementsByTagName("file");
            if (!(fileNodeList.Count() == 0))
            {
                foreach (XmlElement fileNode in fileNodeList)
                {
                    string munoramaName = fileNode.GetAttribute("id");
                    if (filename.Equals(munoramaName))
                    {
                        // Create the message dialog and set its content
                        dialog = new Windows.UI.Popups.MessageDialog("이미 같은 이름의 파일이 있습니다. 다른 이름으로 만들어 주세요.");
                        dialog.Commands.Add(new Windows.UI.Popups.UICommand("OK"));
                        dialog.DefaultCommandIndex = 0;
                        dialog.CancelCommandIndex = 1;
                        await dialog.ShowAsync();
                        return;
                    }
                }
            }

            // Get file element
            XmlElement file = xmlDoc.CreateElement("file");
            file.SetAttribute("id", filename);

            // Get MyCanvas Element
            XmlElement newcanvas = xmlDoc.CreateElement("canvas");
           
            Canvas canvas_save = obj as Canvas;
            var memoryStream = new MemoryStream();
            UserDetail Stats = new UserDetail();
            Type each_type;
            while (canvas_save.Children.Count!=0) {
                each_type = canvas_save.Children.First().GetType();
                Stats.Type_of = each_type.ToString();
                
                if (each_type == typeof(Line))
                {
                    Line thisline = canvas_save.Children.First() as Line;
                    Stats.Points_X1 = thisline.X1;
                    Stats.Points_Y1 = thisline.Y1;
                    Stats.Points_X2 = thisline.X2;
                    Stats.Points_Y2 = thisline.Y2;
                    
                    Brush brush = thisline.Stroke;
                    Stats.Color_of_A = ((Color)brush.GetValue(SolidColorBrush.ColorProperty)).A;
                    Stats.Color_of_G = ((Color)brush.GetValue(SolidColorBrush.ColorProperty)).G;
                    Stats.Color_of_R = ((Color)brush.GetValue(SolidColorBrush.ColorProperty)).R;
                    Stats.Color_of_B = ((Color)brush.GetValue(SolidColorBrush.ColorProperty)).B;
                    
                    //Stats.Brush_of = thisline.Stroke;
                    //Stats.Stretch_of = thisline.Stretch;
                }

                else if (each_type == typeof(TextBox))
                {
                    TextBox thistextbox = canvas_save.Children.First() as TextBox;
                    Stats.Points_X1 = Canvas.GetLeft(thistextbox);
                    Stats.Points_Y1 = Canvas.GetTop(thistextbox);
                    Stats.Points_X2 = Canvas.GetLeft(thistextbox) + thistextbox.Width;
                    Stats.Points_Y2 = Canvas.GetTop(thistextbox) + thistextbox.Height;
                    //Stats.Brush_of = thistextbox.Background;
                    Stats.Name = thistextbox.Text;
                    Stats.Id = int.Parse(thistextbox.Text);
                }
                //140517새벽

                var dataContractSerializer = new DataContractSerializer(typeof(UserDetail));
                dataContractSerializer.WriteObject(memoryStream, Stats);
                canvas_save.Children.RemoveAt(0);
            }
            memoryStream.Seek(0, SeekOrigin.Begin);

            string serialized = new StreamReader(memoryStream).ReadToEnd();
            memoryStream.Dispose();
            newcanvas.InnerText = serialized;
            
            // Add elements to file node
            file.AppendChild(newcanvas);

            // Add file node to Root
            root.AppendChild(file);

            // Flush xml elements to xml file
            await xmlDoc.SaveToFileAsync(xmlFile);

            // Create the message dialog and set its content
            dialog = new Windows.UI.Popups.MessageDialog("성공적으로 새 파일을 만들었습니다. 목록에서 확인하세요.");
            dialog.Commands.Add(new Windows.UI.Popups.UICommand("OK"));
            dialog.DefaultCommandIndex = 0;
            dialog.CancelCommandIndex = 1;
            await dialog.ShowAsync();
        }
        /*
        public static async Task<object> ReadAsync(string fileName, Type type)
        {
            var applicationData = ApplicationData.Current;
            var storageFolder = applicationData.LocalFolder;
            
            var file = await storageFolder.GetFileAsync(fileName);
            var stream = await file.OpenStreamForReadAsync();
            var serializer = new DataContractSerializer(type);
            return serializer.ReadObject(stream);
        }*/

        public async void ReadAsync()
        {
            StorageFile file = await ApplicationData.Current.LocalFolder.GetFileAsync("UserDetails");
            if (file == null) return;
            IRandomAccessStream inStream = await file.OpenReadAsync();
            // Deserialize the Session State.
            DataContractSerializer serializer = new DataContractSerializer(typeof(UserDetail));
            var StatsDetails = (UserDetail)serializer.ReadObject(inStream.AsStreamForRead());
            inStream.Dispose();

            //구현필요한부분
            if (StatsDetails.Type_of == "Line")
            {
                Line newline = new Line()
                {
                    X1 = StatsDetails.Points_X1,
                    X2 = StatsDetails.Points_X2,
                    Y1 = StatsDetails.Points_Y1,
                    Y2 = StatsDetails.Points_Y2,
                    Stroke = new SolidColorBrush(Color.FromArgb(StatsDetails.Color_of_A,StatsDetails.Color_of_R,StatsDetails.Color_of_G,StatsDetails.Color_of_B))
                };
            }
            else if (StatsDetails.Type_of == "Textbox")
            {
                TextBox newbox = new TextBox()
                {

                };
            }
        }
        public class UserDetail
        {
            public int Id { get; set; }
            public string Name { get; set; }
            public double Points_X1 { get; set; }
            public double Points_Y1 { get; set; }
            public double Points_X2 { get; set; }
            public double Points_Y2 { get; set; }
            public string Type_of { get; set; }
            public byte Color_of_A { get; set; }
            public byte Color_of_G { get; set; }
            public byte Color_of_R { get; set; }
            public byte Color_of_B { get; set; }
            /*public Brush Brush_of { get; set; }
            public Stretch Stretch_of { get; set; }*/
        }
        /*
        public async void SaveAS()
        {
            StorageFile userdetailsfile = await ApplicationData.Current.LocalFolder.CreateFileAsync("UserDetails", CreationCollisionOption.ReplaceExisting);
            IRandomAccessStream raStream = await userdetailsfile.OpenAsync(FileAccessMode.ReadWrite);
            using (IOutputStream outStream = raStream.GetOutputStreamAt(0))
            {
                // Serialize the Session State.
                DataContractSerializer serializer = new DataContractSerializer(typeof(UserDetail));
                serializer.WriteObject(outStream.AsStreamForWrite(), Stats);
                await outStream.FlushAsync();
            }
        }*/
    }

}

/*
namespace CanvasSerializer
{
    public class PointerDetail
    {

        public PointerDetail()
        {

        }
        public double X { get; set; }
        public double Y { get; set; }

    }

    public class SingleMovePointerCollection
    {
        public SingleMovePointerCollection()
        {
        }

        public List<PointerDetail> SingleMove
        {
            get;
            set;
        }
        public DateTime TimeStamp
        {
            get;
            set;
        }
        public double StrokeThickness { get; set; }
        public Color SelectedColor { get; set; }





    }

    public class AllMovePointerCollection
    {
        public List<SingleMovePointerCollection> AllMove
        {
            get;
            set;
        }

        public void Sort()
        {
            this.AllMove.OrderBy(x => x.TimeStamp).ToList();
        }
        public AllMovePointerCollection()
        {

        }


    }

}*/