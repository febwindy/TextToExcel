﻿#pragma checksum "..\..\..\View\ConfWindow.xaml" "{406ea660-64cf-4c82-b6f0-42d48172a799}" "C19612A59EE1EEF665497C35EB1B2A2F"
//------------------------------------------------------------------------------
// <auto-generated>
//     此代码由工具生成。
//     运行时版本:4.0.30319.42000
//
//     对此文件的更改可能会导致不正确的行为，并且如果
//     重新生成代码，这些更改将会丢失。
// </auto-generated>
//------------------------------------------------------------------------------

using System;
using System.Diagnostics;
using System.Windows;
using System.Windows.Automation;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Effects;
using System.Windows.Media.Imaging;
using System.Windows.Media.Media3D;
using System.Windows.Media.TextFormatting;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Shell;


namespace TextToExcel.View {
    
    
    /// <summary>
    /// ConfWindow
    /// </summary>
    public partial class ConfWindow : System.Windows.Window, System.Windows.Markup.IComponentConnector {
        
        
        #line 29 "..\..\..\View\ConfWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBox Address;
        
        #line default
        #line hidden
        
        
        #line 30 "..\..\..\View\ConfWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBox Port;
        
        #line default
        #line hidden
        
        
        #line 31 "..\..\..\View\ConfWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.CheckBox Anonymous;
        
        #line default
        #line hidden
        
        
        #line 32 "..\..\..\View\ConfWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBox Username;
        
        #line default
        #line hidden
        
        
        #line 33 "..\..\..\View\ConfWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.PasswordBox Password;
        
        #line default
        #line hidden
        
        
        #line 34 "..\..\..\View\ConfWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBox Path;
        
        #line default
        #line hidden
        
        
        #line 37 "..\..\..\View\ConfWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button ConnectBtn;
        
        #line default
        #line hidden
        
        
        #line 38 "..\..\..\View\ConfWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button CloseBtn;
        
        #line default
        #line hidden
        
        private bool _contentLoaded;
        
        /// <summary>
        /// InitializeComponent
        /// </summary>
        [System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [System.CodeDom.Compiler.GeneratedCodeAttribute("PresentationBuildTasks", "4.0.0.0")]
        public void InitializeComponent() {
            if (_contentLoaded) {
                return;
            }
            _contentLoaded = true;
            System.Uri resourceLocater = new System.Uri("/TextToExcel;component/view/confwindow.xaml", System.UriKind.Relative);
            
            #line 1 "..\..\..\View\ConfWindow.xaml"
            System.Windows.Application.LoadComponent(this, resourceLocater);
            
            #line default
            #line hidden
        }
        
        [System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [System.CodeDom.Compiler.GeneratedCodeAttribute("PresentationBuildTasks", "4.0.0.0")]
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Never)]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Design", "CA1033:InterfaceMethodsShouldBeCallableByChildTypes")]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Maintainability", "CA1502:AvoidExcessiveComplexity")]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1800:DoNotCastUnnecessarily")]
        void System.Windows.Markup.IComponentConnector.Connect(int connectionId, object target) {
            switch (connectionId)
            {
            case 1:
            this.Address = ((System.Windows.Controls.TextBox)(target));
            return;
            case 2:
            this.Port = ((System.Windows.Controls.TextBox)(target));
            return;
            case 3:
            this.Anonymous = ((System.Windows.Controls.CheckBox)(target));
            
            #line 31 "..\..\..\View\ConfWindow.xaml"
            this.Anonymous.Click += new System.Windows.RoutedEventHandler(this.AnonymousConfFTPWindowClick);
            
            #line default
            #line hidden
            return;
            case 4:
            this.Username = ((System.Windows.Controls.TextBox)(target));
            return;
            case 5:
            this.Password = ((System.Windows.Controls.PasswordBox)(target));
            return;
            case 6:
            this.Path = ((System.Windows.Controls.TextBox)(target));
            return;
            case 7:
            this.ConnectBtn = ((System.Windows.Controls.Button)(target));
            
            #line 37 "..\..\..\View\ConfWindow.xaml"
            this.ConnectBtn.Click += new System.Windows.RoutedEventHandler(this.ConnectConfFTPWindowClick);
            
            #line default
            #line hidden
            return;
            case 8:
            this.CloseBtn = ((System.Windows.Controls.Button)(target));
            
            #line 38 "..\..\..\View\ConfWindow.xaml"
            this.CloseBtn.Click += new System.Windows.RoutedEventHandler(this.CloseConfFTPWindowClick);
            
            #line default
            #line hidden
            return;
            }
            this._contentLoaded = true;
        }
    }
}

