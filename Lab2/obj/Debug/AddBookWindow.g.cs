﻿#pragma checksum "..\..\AddBookWindow.xaml" "{406ea660-64cf-4c82-b6f0-42d48172a799}" "72AB627C154C299FE089C5E16EE89645"
//------------------------------------------------------------------------------
// <auto-generated>
//     Этот код создан программой.
//     Исполняемая версия:4.0.30319.42000
//
//     Изменения в этом файле могут привести к неправильной работе и будут потеряны в случае
//     повторной генерации кода.
// </auto-generated>
//------------------------------------------------------------------------------

using Lab2;
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


namespace Lab2 {
    
    
    /// <summary>
    /// AddBookWindow
    /// </summary>
    public partial class AddBookWindow : System.Windows.Window, System.Windows.Markup.IComponentConnector, System.Windows.Markup.IStyleConnector {
        
        
        #line 10 "..\..\AddBookWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Grid border;
        
        #line default
        #line hidden
        
        
        #line 50 "..\..\AddBookWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.ComboBox authorsComboBox;
        
        #line default
        #line hidden
        
        
        #line 51 "..\..\AddBookWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.ComboBox tagsComboBox;
        
        #line default
        #line hidden
        
        
        #line 52 "..\..\AddBookWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.ListView authorsPanel;
        
        #line default
        #line hidden
        
        
        #line 65 "..\..\AddBookWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.ListView tagsPanel;
        
        #line default
        #line hidden
        
        
        #line 102 "..\..\AddBookWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBox bookISBN;
        
        #line default
        #line hidden
        
        
        #line 103 "..\..\AddBookWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBox bookName;
        
        #line default
        #line hidden
        
        
        #line 104 "..\..\AddBookWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBox bookYear;
        
        #line default
        #line hidden
        
        
        #line 105 "..\..\AddBookWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBox bookPage;
        
        #line default
        #line hidden
        
        
        #line 106 "..\..\AddBookWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.ComboBox housesComboBox;
        
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
            System.Uri resourceLocater = new System.Uri("/Lab2;component/addbookwindow.xaml", System.UriKind.Relative);
            
            #line 1 "..\..\AddBookWindow.xaml"
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
            this.border = ((System.Windows.Controls.Grid)(target));
            return;
            case 2:
            this.authorsComboBox = ((System.Windows.Controls.ComboBox)(target));
            
            #line 50 "..\..\AddBookWindow.xaml"
            this.authorsComboBox.SelectionChanged += new System.Windows.Controls.SelectionChangedEventHandler(this.authorsComboBox_SelectionChanged);
            
            #line default
            #line hidden
            return;
            case 3:
            this.tagsComboBox = ((System.Windows.Controls.ComboBox)(target));
            
            #line 51 "..\..\AddBookWindow.xaml"
            this.tagsComboBox.SelectionChanged += new System.Windows.Controls.SelectionChangedEventHandler(this.tagsComboBox_SelectionChanged);
            
            #line default
            #line hidden
            return;
            case 4:
            this.authorsPanel = ((System.Windows.Controls.ListView)(target));
            return;
            case 6:
            this.tagsPanel = ((System.Windows.Controls.ListView)(target));
            return;
            case 8:
            this.bookISBN = ((System.Windows.Controls.TextBox)(target));
            return;
            case 9:
            this.bookName = ((System.Windows.Controls.TextBox)(target));
            return;
            case 10:
            this.bookYear = ((System.Windows.Controls.TextBox)(target));
            return;
            case 11:
            this.bookPage = ((System.Windows.Controls.TextBox)(target));
            return;
            case 12:
            this.housesComboBox = ((System.Windows.Controls.ComboBox)(target));
            
            #line 106 "..\..\AddBookWindow.xaml"
            this.housesComboBox.SelectionChanged += new System.Windows.Controls.SelectionChangedEventHandler(this.housesComboBox_SelectionChanged);
            
            #line default
            #line hidden
            return;
            case 13:
            
            #line 108 "..\..\AddBookWindow.xaml"
            ((System.Windows.Controls.Button)(target)).Click += new System.Windows.RoutedEventHandler(this.Button_Click);
            
            #line default
            #line hidden
            return;
            case 14:
            
            #line 109 "..\..\AddBookWindow.xaml"
            ((System.Windows.Controls.Button)(target)).Click += new System.Windows.RoutedEventHandler(this.Button_Click_1);
            
            #line default
            #line hidden
            return;
            }
            this._contentLoaded = true;
        }
        
        [System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [System.CodeDom.Compiler.GeneratedCodeAttribute("PresentationBuildTasks", "4.0.0.0")]
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Never)]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Design", "CA1033:InterfaceMethodsShouldBeCallableByChildTypes")]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1800:DoNotCastUnnecessarily")]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Maintainability", "CA1502:AvoidExcessiveComplexity")]
        void System.Windows.Markup.IStyleConnector.Connect(int connectionId, object target) {
            switch (connectionId)
            {
            case 5:
            
            #line 60 "..\..\AddBookWindow.xaml"
            ((System.Windows.Controls.Label)(target)).MouseLeftButtonDown += new System.Windows.Input.MouseButtonEventHandler(this.Label_MouseLeftButtonDown);
            
            #line default
            #line hidden
            break;
            case 7:
            
            #line 73 "..\..\AddBookWindow.xaml"
            ((System.Windows.Controls.Label)(target)).MouseLeftButtonDown += new System.Windows.Input.MouseButtonEventHandler(this.Label_MouseLeftButtonDown_1);
            
            #line default
            #line hidden
            break;
            }
        }
    }
}

