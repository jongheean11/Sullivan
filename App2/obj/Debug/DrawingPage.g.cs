﻿

#pragma checksum "C:\Users\James\Desktop\졸프\졸프\App2\App2\DrawingPage.xaml" "{406ea660-64cf-4c82-b6f0-42d48172a799}" "BE3B2C0524E1F8904D72BBA9F310AE17"
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace App2
{
    partial class DrawingPage : global::Windows.UI.Xaml.Controls.Page, global::Windows.UI.Xaml.Markup.IComponentConnector
    {
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Windows.UI.Xaml.Build.Tasks"," 4.0.0.0")]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
 
        public void Connect(int connectionId, object target)
        {
            switch(connectionId)
            {
            case 1:
                #line 22 "..\..\DrawingPage.xaml"
                ((global::Windows.UI.Xaml.Controls.Primitives.ButtonBase)(target)).Click += this.PreviewBtn_Click;
                 #line default
                 #line hidden
                break;
            case 2:
                #line 23 "..\..\DrawingPage.xaml"
                ((global::Windows.UI.Xaml.Controls.Primitives.ButtonBase)(target)).Click += this.SaveBtn_Click;
                 #line default
                 #line hidden
                break;
            case 3:
                #line 139 "..\..\DrawingPage.xaml"
                ((global::Windows.UI.Xaml.Controls.Primitives.ButtonBase)(target)).Click += this.SavefileBtn_Click;
                 #line default
                 #line hidden
                break;
            case 4:
                #line 46 "..\..\DrawingPage.xaml"
                ((global::PerpetuumSoft.Controls.RadialItem)(target)).Click += this.TableItem_Click;
                 #line default
                 #line hidden
                break;
            case 5:
                #line 62 "..\..\DrawingPage.xaml"
                ((global::PerpetuumSoft.Controls.RadialItem)(target)).Click += this.TextItem_Click;
                 #line default
                 #line hidden
                break;
            case 6:
                #line 65 "..\..\DrawingPage.xaml"
                ((global::PerpetuumSoft.Controls.RadialItem)(target)).Click += this.ImageItem_Click;
                 #line default
                 #line hidden
                break;
            case 7:
                #line 118 "..\..\DrawingPage.xaml"
                ((global::PerpetuumSoft.Controls.RadialItem)(target)).Click += this.ClearAllItem_Click;
                 #line default
                 #line hidden
                break;
            case 8:
                #line 77 "..\..\DrawingPage.xaml"
                ((global::PerpetuumSoft.Controls.RadialItem)(target)).Click += this.PolygonItem_Click;
                 #line default
                 #line hidden
                break;
            case 9:
                #line 78 "..\..\DrawingPage.xaml"
                ((global::PerpetuumSoft.Controls.RadialItem)(target)).Click += this.TriangleItem_Click;
                 #line default
                 #line hidden
                break;
            case 10:
                #line 79 "..\..\DrawingPage.xaml"
                ((global::PerpetuumSoft.Controls.RadialItem)(target)).Click += this.RectangleItem_Click;
                 #line default
                 #line hidden
                break;
            case 11:
                #line 80 "..\..\DrawingPage.xaml"
                ((global::PerpetuumSoft.Controls.RadialItem)(target)).Click += this.LineItem_Click;
                 #line default
                 #line hidden
                break;
            case 12:
                #line 73 "..\..\DrawingPage.xaml"
                ((global::Windows.UI.Xaml.UIElement)(target)).PointerPressed += this.PenItem_Click;
                 #line default
                 #line hidden
                break;
            }
            this._contentLoaded = true;
        }
    }
}


