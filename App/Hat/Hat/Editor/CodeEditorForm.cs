using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.Integration;
using ICSharpCode.AvalonEdit;
using ICSharpCode.AvalonEdit.Search;
using System.Windows.Media;
using System.Windows.Input;
using ICSharpCode.AvalonEdit.CodeCompletion;
using Hat.Editor;
using ICSharpCode.AvalonEdit.Editing;

namespace Hat
{
    public partial class CodeEditorForm : Form
    {
        public CodeEditorForm()
        {
            InitializeComponent();
        }

        const string STATUS_SAVED = "status_saved";
        const string STATUS_NOT_SAVE = "status_not_saved";

        CompletionWindow completionWindow;
        public BrowserForm parent;
        List<object[]> files; // [ 0 - имя файла | 1 - путь файла | 2 - статус | 3 - индекс | 4 - TabPage (вкладка) | 5 - TextEditorControl (редактор)]

        private string[] handbook = new string[] {
@"{\rtf1\ansi\ansicpg1251\deff0\nouicompat\deflang1049{\fonttbl{\f0\fnil\fcharset0 Calibri;}{\f1\fnil\fcharset204 Calibri;}}
{\colortbl ;\red0\green77\blue187;\red155\green0\blue211;\red0\green0\blue0;}
{\*\generator Riched20 10.0.22000}\viewkind4\uc1 
\pard\sl276\slmult1\cf1\f0\fs20\lang9 Tester\cf0\par
\cf2\f1\lang1049\'ce\'ef\'e8\'f1\'e0\'ed\'e8\'e5\cf0 : \'ea\'ee\'ed\'f1\'f2\'f0\'f3\'ea\'f2\'ee\'f0 \'ea\'eb\'e0\'f1\'f1\'e0\par
\cf2\'d1\'e8\'ed\'f2\'e0\'ea\'f1\'e8\'f1\cf0 : Tester(Form browserForm)\par
\cf3\par
\cf2\'cf\'f0\'e8\'ec\'e5\'f0\cf3 :\par

\pard\sl240\slmult1\cf0 Tester tester;\par
public async void Main(Form browserWindow)\par
\{\par
\tab tester = new Tester(browserWindow);\par
\}\par

\pard\sa200\sl276\slmult1\f0\fs22\lang9\par
}",

@"{\rtf1\ansi\ansicpg1251\deff0\nouicompat\deflang1049{\fonttbl{\f0\fnil\fcharset0 Calibri;}{\f1\fnil\fcharset204 Calibri;}}
{\colortbl ;\red0\green77\blue187;\red155\green0\blue211;\red0\green0\blue0;}
{\*\generator Riched20 10.0.22000}\viewkind4\uc1 
\pard\sl276\slmult1\cf1\f0\fs20\lang9 IMAGE_STATUS_PROCESS\cf0\par
\cf2\f1\lang1049\'ce\'ef\'e8\'f1\'e0\'ed\'e8\'e5\cf0 : \'e8\'ed\'e4\'e5\'ea\'f1 \'ea\'e0\'f0\'f2\'e8\'ed\'ea\'e8 \'ea\'ee\'f2\'ee\'f0\'e0\'ff \'ee\'e1\'ee\'e7\'ed\'e0\'f7\'e0\'e5\'f2 \'f1\'f2\'e0\'f2\'f3\'f1 \'e2 \'ef\'f0\'ee\'f6\'e5\'f1\'f1\'e5\par
\cf2\'d1\'e8\'ed\'f2\'e0\'ea\'f1\'e8\'f1\cf0 : IMAGE_STATUS_PROCESS =  0\par
\cf3\par
\cf2\'cf\'f0\'e8\'ec\'e5\'f0\cf3 :\par

\pard\sa200\sl276\slmult1\cf0 Tester.IMAGE_STATUS_PROCESS\f0\fs22\lang9\par
}",

@"{\rtf1\ansi\ansicpg1251\deff0\nouicompat\deflang1049{\fonttbl{\f0\fnil\fcharset0 Calibri;}{\f1\fnil\fcharset204 Calibri;}}
{\colortbl ;\red0\green77\blue187;\red155\green0\blue211;\red0\green0\blue0;}
{\*\generator Riched20 10.0.22000}\viewkind4\uc1 
\pard\sl276\slmult1\cf1\f0\fs20\lang9 IMAGE_STATUS_PASSED\cf0\par
\cf2\f1\lang1049\'ce\'ef\'e8\'f1\'e0\'ed\'e8\'e5\cf0 : \'e8\'ed\'e4\'e5\'ea\'f1 \'ea\'e0\'f0\'f2\'e8\'ed\'ea\'e8 \'ea\'ee\'f2\'ee\'f0\'e0\'ff \'ee\'e1\'ee\'e7\'ed\'e0\'f7\'e0\'e5\'f2 \'f1\'f2\'e0\'f2\'f3\'f1 \'f3\'f1\'ef\'e5\'f8\'ed\'ee\par
\cf2\'d1\'e8\'ed\'f2\'e0\'ea\'f1\'e8\'f1\cf0 : IMAGE_STATUS_PASSED =  1\par
\cf3\par
\cf2\'cf\'f0\'e8\'ec\'e5\'f0\cf3 :\par

\pard\sa200\sl276\slmult1\cf0 Tester.IMAGE_STATUS_PASSED\f0\fs22\lang9\par
}",

@"{\rtf1\ansi\ansicpg1251\deff0\nouicompat\deflang1049{\fonttbl{\f0\fnil\fcharset0 Calibri;}{\f1\fnil\fcharset204 Calibri;}}
{\colortbl ;\red0\green77\blue187;\red155\green0\blue211;\red0\green0\blue0;}
{\*\generator Riched20 10.0.22000}\viewkind4\uc1 
\pard\sl276\slmult1\cf1\f0\fs20\lang9 IMAGE_STATUS_FAILED\cf0\par
\cf2\f1\lang1049\'ce\'ef\'e8\'f1\'e0\'ed\'e8\'e5\cf0 : \'e8\'ed\'e4\'e5\'ea\'f1 \'ea\'e0\'f0\'f2\'e8\'ed\'ea\'e8 \'ea\'ee\'f2\'ee\'f0\'e0\'ff \'ee\'e1\'ee\'e7\'ed\'e0\'f7\'e0\'e5\'f2 \'f1\'f2\'e0\'f2\'f3\'f1 \'ef\'f0\'ee\'e2\'e0\'eb\'fc\'ed\'ee\par
\cf2\'d1\'e8\'ed\'f2\'e0\'ea\'f1\'e8\'f1\cf0 : IMAGE_STATUS_FAILED =  2\par
\cf3\par
\cf2\'cf\'f0\'e8\'ec\'e5\'f0\cf3 :\par

\pard\sa200\sl276\slmult1\cf0 Tester.IMAGE_STATUS_FAILED\f0\fs22\lang9\par
}",

@"{\rtf1\ansi\ansicpg1251\deff0\nouicompat\deflang1049{\fonttbl{\f0\fnil\fcharset0 Calibri;}{\f1\fnil\fcharset204 Calibri;}}
{\colortbl ;\red0\green77\blue187;\red155\green0\blue211;\red0\green0\blue0;}
{\*\generator Riched20 10.0.22000}\viewkind4\uc1 
\pard\sl276\slmult1\cf1\f0\fs20\lang9 IMAGE_STATUS_MESSAGE\cf0\par
\cf2\f1\lang1049\'ce\'ef\'e8\'f1\'e0\'ed\'e8\'e5\cf0 : \'e8\'ed\'e4\'e5\'ea\'f1 \'ea\'e0\'f0\'f2\'e8\'ed\'ea\'e8 \'ea\'ee\'f2\'ee\'f0\'e0\'ff \'ee\'e1\'ee\'e7\'ed\'e0\'f7\'e0\'e5\'f2 \'f1\'f2\'e0\'f2\'f3\'f1 \'f1\'ee\'ee\'e1\'f9\'e5\'ed\'e8\'e5\par
\cf2\'d1\'e8\'ed\'f2\'e0\'ea\'f1\'e8\'f1\cf0 : IMAGE_STATUS_MESSAGE =  3\par
\cf3\par
\cf2\'cf\'f0\'e8\'ec\'e5\'f0\cf3 :\par

\pard\sa200\sl276\slmult1\cf0 Tester.IMAGE_STATUS_MESSAGE\f0\fs22\lang9\par
}",

@"{\rtf1\ansi\ansicpg1251\deff0\nouicompat\deflang1049{\fonttbl{\f0\fnil\fcharset0 Calibri;}{\f1\fnil\fcharset204 Calibri;}}
{\colortbl ;\red0\green77\blue187;\red155\green0\blue211;\red0\green0\blue0;}
{\*\generator Riched20 10.0.22000}\viewkind4\uc1 
\pard\sl276\slmult1\cf1\f0\fs20\lang9 IMAGE_STATUS_WARNING\cf0\par
\cf2\f1\lang1049\'ce\'ef\'e8\'f1\'e0\'ed\'e8\'e5\cf0 : \'e8\'ed\'e4\'e5\'ea\'f1 \'ea\'e0\'f0\'f2\'e8\'ed\'ea\'e8 \'ea\'ee\'f2\'ee\'f0\'e0\'ff \'ee\'e1\'ee\'e7\'ed\'e0\'f7\'e0\'e5\'f2 \'f1\'f2\'e0\'f2\'f3\'f1 \'ef\'f0\'e5\'e4\'f3\'ef\'f0\'e5\'e6\'e4\'e5\'ed\'e8\'e5\par
\cf2\'d1\'e8\'ed\'f2\'e0\'ea\'f1\'e8\'f1\cf0 : IMAGE_STATUS_WARNING =  4\par
\cf3\par
\cf2\'cf\'f0\'e8\'ec\'e5\'f0\cf3 :\par

\pard\sa200\sl276\slmult1\cf0 Tester.IMAGE_STATUS_WARNING\f0\fs22\lang9\par
}",

@"{\rtf1\ansi\ansicpg1251\deff0\nouicompat\deflang1049{\fonttbl{\f0\fnil\fcharset0 Calibri;}{\f1\fnil\fcharset204 Calibri;}}
{\colortbl ;\red0\green77\blue187;\red155\green0\blue211;\red0\green0\blue0;}
{\*\generator Riched20 10.0.22000}\viewkind4\uc1 
\pard\sl276\slmult1\cf1\f0\fs20\lang9 PASSED\cf0\par
\cf2\f1\lang1049\'ce\'ef\'e8\'f1\'e0\'ed\'e8\'e5\cf0 : \'ea\'ee\'ed\'f1\'f2\'e0\'ed\'f2\'e0 \'ee\'e1\'ee\'e7\'ed\'e0\'f7\'e0\'e5\'f2 \'f1\'f2\'e0\'f2\'f3\'f1 \'f3\'f1\'ef\'e5\'f8\'ed\'ee\par
\cf2\'d1\'e8\'ed\'f2\'e0\'ea\'f1\'e8\'f1\cf0 : PASSED = ""\'d3\'f1\'ef\'e5\'f8\'ed\'ee""\par
\cf3\par
\cf2\'cf\'f0\'e8\'ec\'e5\'f0\cf3 :\par

\pard\sa200\sl276\slmult1\cf0 Tester.PASSED\f0\fs22\lang9\par
}",

@"{\rtf1\ansi\ansicpg1251\deff0\nouicompat\deflang1049{\fonttbl{\f0\fnil\fcharset0 Calibri;}{\f1\fnil\fcharset204 Calibri;}}
{\colortbl ;\red0\green77\blue187;\red155\green0\blue211;\red0\green0\blue0;}
{\*\generator Riched20 10.0.22000}\viewkind4\uc1 
\pard\sl276\slmult1\cf1\f0\fs20\lang9 FAILED\cf0\par
\cf2\f1\lang1049\'ce\'ef\'e8\'f1\'e0\'ed\'e8\'e5\cf0 : \'ea\'ee\'ed\'f1\'f2\'e0\'ed\'f2\'e0 \'ee\'e1\'ee\'e7\'ed\'e0\'f7\'e0\'e5\'f2 \'f1\'f2\'e0\'f2\'f3\'f1 \'ef\'f0\'ee\'e2\'e0\'eb\'fc\'ed\'ee\par
\cf2\'d1\'e8\'ed\'f2\'e0\'ea\'f1\'e8\'f1\cf0 : FAILED = ""\'cf\'f0\'ee\'e2\'e0\'eb\'fc\'ed\'ee""\par
\cf3\par
\cf2\'cf\'f0\'e8\'ec\'e5\'f0\cf3 :\par

\pard\sa200\sl276\slmult1\cf0 Tester.FAILED\f0\fs22\lang9\par
}",

@"{\rtf1\ansi\ansicpg1251\deff0\nouicompat\deflang1049{\fonttbl{\f0\fnil\fcharset0 Calibri;}{\f1\fnil\fcharset204 Calibri;}}
{\colortbl ;\red0\green77\blue187;\red155\green0\blue211;\red0\green0\blue0;}
{\*\generator Riched20 10.0.22000}\viewkind4\uc1 
\pard\sl276\slmult1\cf1\f0\fs20\lang9 STOPPED\cf0\par
\cf2\f1\lang1049\'ce\'ef\'e8\'f1\'e0\'ed\'e8\'e5\cf0 : \'ea\'ee\'ed\'f1\'f2\'e0\'ed\'f2\'e0 \'ee\'e1\'ee\'e7\'ed\'e0\'f7\'e0\'e5\'f2 \'f1\'f2\'e0\'f2\'f3\'f1 \'ee\'f1\'f2\'e0\'ed\'ee\'e2\'eb\'e5\'ed\'ee\par
\cf2\'d1\'e8\'ed\'f2\'e0\'ea\'f1\'e8\'f1\cf0 : STOPPED = ""\'ce\'f1\'f2\'e0\'ed\'ee\'e2\'eb\'e5\'ed\'ee""\par
\cf3\par
\cf2\'cf\'f0\'e8\'ec\'e5\'f0\cf3 :\par

\pard\sa200\sl276\slmult1\cf0 Tester.STOPPED\f0\fs22\lang9\par
}",

@"{\rtf1\ansi\ansicpg1251\deff0\nouicompat\deflang1049{\fonttbl{\f0\fnil\fcharset0 Calibri;}{\f1\fnil\fcharset204 Calibri;}}
{\colortbl ;\red0\green77\blue187;\red155\green0\blue211;\red0\green0\blue0;}
{\*\generator Riched20 10.0.22000}\viewkind4\uc1 
\pard\sl276\slmult1\cf1\f0\fs20\lang9 PROCESS\cf0\par
\cf2\f1\lang1049\'ce\'ef\'e8\'f1\'e0\'ed\'e8\'e5\cf0 : \'ea\'ee\'ed\'f1\'f2\'e0\'ed\'f2\'e0 \'ee\'e1\'ee\'e7\'ed\'e0\'f7\'e0\'e5\'f2 \'f1\'f2\'e0\'f2\'f3\'f1 \'e2\'fb\'ef\'ee\'eb\'ed\'ff\'e5\'f2\'f1\'ff\par
\cf2\'d1\'e8\'ed\'f2\'e0\'ea\'f1\'e8\'f1\cf0 : PROCESS = ""\'c2\'fb\'ef\'ee\'eb\'ed\'ff\'e5\'f2\'f1\'ff""\par
\cf3\par
\cf2\'cf\'f0\'e8\'ec\'e5\'f0\cf3 :\par

\pard\sa200\sl276\slmult1\cf0 Tester.PROCESS\f0\fs22\lang9\par
}",

@"{\rtf1\ansi\ansicpg1251\deff0\nouicompat\deflang1049{\fonttbl{\f0\fnil\fcharset0 Calibri;}{\f1\fnil\fcharset204 Calibri;}}
{\colortbl ;\red0\green77\blue187;\red155\green0\blue211;\red0\green0\blue0;}
{\*\generator Riched20 10.0.22000}\viewkind4\uc1 
\pard\sl276\slmult1\cf1\f0\fs20\lang9 COMPLETED\cf0\par
\cf2\f1\lang1049\'ce\'ef\'e8\'f1\'e0\'ed\'e8\'e5\cf0 : \'ea\'ee\'ed\'f1\'f2\'e0\'ed\'f2\'e0 \'ee\'e1\'ee\'e7\'ed\'e0\'f7\'e0\'e5\'f2 \'f1\'f2\'e0\'f2\'f3\'f1 \'e2\'fb\'ef\'ee\'eb\'ed\'e5\'ed\'ee\par
\cf2\'d1\'e8\'ed\'f2\'e0\'ea\'f1\'e8\'f1\cf0 : COMPLETED\f0\lang1033  \f1\lang1049 = ""\'c2\'fb\'ef\'ee\'eb\'ed\'e5\'ed\'ee""\par
\cf3\par
\cf2\'cf\'f0\'e8\'ec\'e5\'f0\cf3 :\par

\pard\sa200\sl276\slmult1\cf0 Tester.COMPLETED\f0\fs22\lang9\par
}",

@"{\rtf1\ansi\ansicpg1251\deff0\nouicompat\deflang1049{\fonttbl{\f0\fnil\fcharset0 Calibri;}{\f1\fnil\fcharset204 Calibri;}}
{\colortbl ;\red0\green77\blue187;\red155\green0\blue211;\red0\green0\blue0;}
{\*\generator Riched20 10.0.22000}\viewkind4\uc1 
\pard\sl276\slmult1\cf1\f0\fs20\lang9 WARNING\cf0\par
\cf2\f1\lang1049\'ce\'ef\'e8\'f1\'e0\'ed\'e8\'e5\cf0 : \'ea\'ee\'ed\'f1\'f2\'e0\'ed\'f2\'e0 \'ee\'e1\'ee\'e7\'ed\'e0\'f7\'e0\'e5\'f2 \'f1\'f2\'e0\'f2\'f3\'f1 \'ef\'f0\'e5\'e4\'f3\'ef\'f0\'e5\'e6\'e4\'e5\'ed\'e8\'e5\par
\cf2\'d1\'e8\'ed\'f2\'e0\'ea\'f1\'e8\'f1\cf0 : WARNING\f0\lang1033  \f1\lang1049 = ""\'cf\'f0\'e5\'e4\'f3\'ef\'f0\'e5\'e6\'e4\'e5\'ed\'e8\'e5""\par
\cf3\par
\cf2\'cf\'f0\'e8\'ec\'e5\'f0\cf3 :\par

\pard\sa200\sl276\slmult1\cf0 Tester.WARNING\f0\fs22\lang9\par
}",

@"{\rtf1\ansi\ansicpg1251\deff0\nouicompat\deflang1049{\fonttbl{\f0\fnil\fcharset0 Calibri;}{\f1\fnil\fcharset204 Calibri;}}
{\colortbl ;\red0\green77\blue187;\red155\green0\blue211;\red0\green0\blue0;}
{\*\generator Riched20 10.0.22000}\viewkind4\uc1 
\pard\sl276\slmult1\cf1\f0\fs20\lang9 BrowserView\cf0\par
\cf2\f1\lang1049\'ce\'ef\'e8\'f1\'e0\'ed\'e8\'e5\cf0 : \'ef\'e5\'f0\'e5\'ec\'e5\'ed\'ed\'e0\'ff \'f1\'f1\'fb\'eb\'e0\'e5\'f2\'f1\'ff \'ed\'e0 \'ee\'e1\'fa\'e5\'ea\'f2 \'ef\'f0\'e5\'e4\'f1\'f2\'e0\'e2\'eb\'ff\'fe\'f9\'e8\'e9 \'ee\'e1\'eb\'e0\'f1\'f2\'fc \'e1\'f0\'e0\'f3\'e7\'e5\'f0\'e0\par
\cf2\'d1\'e8\'ed\'f2\'e0\'ea\'f1\'e8\'f1\cf0 : WebView2 BrowserView\par
\cf3\par
\cf2\'cf\'f0\'e8\'ec\'e5\'f0\cf3 :\par
\cf0 tester.BrowserView.Reload();\par
tester.BrowserView.Refresh();\par
tester.BrowserView.Source = new Uri(url);\par
tester.BrowserView.Update();\par
}",

@"{\rtf1\ansi\ansicpg1251\deff0\nouicompat\deflang1049{\fonttbl{\f0\fnil\fcharset0 Calibri;}{\f1\fnil\fcharset204 Calibri;}}
{\colortbl ;\red0\green77\blue187;\red155\green0\blue211;\red0\green0\blue0;}
{\*\generator Riched20 10.0.22000}\viewkind4\uc1 
\pard\sl276\slmult1\cf1\f0\fs20\lang9 BrowserWindow\cf0\par
\cf2\f1\lang1049\'ce\'ef\'e8\'f1\'e0\'ed\'e8\'e5\cf0 : \'ef\'e5\'f0\'e5\'ec\'e5\'ed\'ed\'e0\'ff \'f1\'f1\'fb\'eb\'e0\'e5\'f2\'f1\'ff \'ed\'e0 \'ee\'ea\'ed\'ee \'ef\'f0\'e8\'eb\'ee\'e6\'e5\'ed\'e8\'ff\par
\cf2\'d1\'e8\'ed\'f2\'e0\'ea\'f1\'e8\'f1\cf0 : Form BrowserWindow\par
\cf3\par
\cf2\'cf\'f0\'e8\'ec\'e5\'f0\cf3 :\par
\cf0 Tester tester;\par
public async void Main(Form browserWindow)\par
\{\par
\tab tester = new Tester(browserWindow);\par
\tab tester.BrowserWindow.Text = ""Hat"";\par
\tab tester.BrowserWindow.Width = 800;\par
\tab tester.BrowserWindow.Height = 600;\par
\tab tester.BrowserWindow.Close();\par
\}\f0\fs22\lang9\par
}",

@"{\rtf1\ansi\ansicpg1251\deff0\nouicompat\deflang1049{\fonttbl{\f0\fnil\fcharset0 Calibri;}{\f1\fnil\fcharset204 Calibri;}}
{\colortbl ;\red0\green77\blue187;\red155\green0\blue211;\red0\green0\blue0;}
{\*\generator Riched20 10.0.22000}\viewkind4\uc1 
\pard\sl276\slmult1\cf1\f0\fs20\lang9 BrowserCloseAsync\cf0\par
\cf2\f1\lang1049\'ce\'ef\'e8\'f1\'e0\'ed\'e8\'e5\cf0 : \'ec\'e5\'f2\'ee\'e4 \'e7\'e0\'ea\'f0\'fb\'e2\'e0\'e5\'f2 \'ee\'ea\'ed\'ee \'e1\'f0\'e0\'f3\'e7\'e5\'f0\'e0\par
\cf2\'d1\'e8\'ed\'f2\'e0\'ea\'f1\'e8\'f1\cf0 : BrowserCloseAsync()\par
\cf3\par
\cf2\'cf\'f0\'e8\'ec\'e5\'f0\cf3 :\par
\cf0 await tester.BrowserCloseAsync();\par
}",

@"{\rtf1\ansi\ansicpg1251\deff0\nouicompat\deflang1049{\fonttbl{\f0\fnil\fcharset0 Calibri;}{\f1\fnil\fcharset204 Calibri;}}
{\colortbl ;\red0\green77\blue187;\red155\green0\blue211;\red0\green0\blue0;}
{\*\generator Riched20 10.0.22000}\viewkind4\uc1 
\pard\sl276\slmult1\cf1\f0\fs20\lang9 BrowserSizeAsync\cf0\par
\cf2\f1\lang1049\'ce\'ef\'e8\'f1\'e0\'ed\'e8\'e5\cf0 : \'ec\'e5\'f2\'ee\'e4 \'f3\'f1\'f2\'e0\'ed\'e0\'e2\'eb\'e8\'e2\'e0\'e5\'f2 \'f0\'e0\'e7\'ec\'e5\'f0 \'e1\'f0\'e0\'f3\'e7\'e5\'f0\'e0\par
\cf2\'d1\'e8\'ed\'f2\'e0\'ea\'f1\'e8\'f1\cf0 : BrowserSizeAsync(int width, int height)\par
\cf3\par
\cf2\'cf\'f0\'e8\'ec\'e5\'f0\cf3 :\par
\cf0 await tester.BrowserSizeAsync(\f0\lang1033 800, 600\f1\lang1049 );\par
}",

@"{\rtf1\ansi\ansicpg1251\deff0\nouicompat\deflang1049{\fonttbl{\f0\fnil\fcharset0 Calibri;}{\f1\fnil\fcharset204 Calibri;}}
{\colortbl ;\red0\green77\blue187;\red155\green0\blue211;\red0\green0\blue0;}
{\*\generator Riched20 10.0.22000}\viewkind4\uc1 
\pard\sl276\slmult1\cf1\f0\fs20\lang9 BrowserFullScreenAsync\cf0\par
\cf2\f1\lang1049\'ce\'ef\'e8\'f1\'e0\'ed\'e8\'e5\cf0 : \'ec\'e5\'f2\'ee\'e4 \'f3\'f1\'f2\'e0\'ed\'e0\'e2\'eb\'e8\'e2\'e0\'e5\'f2 \'ef\'ee\'eb\'ed\'ee\'fd\'ea\'f0\'e0\'ed\'ed\'fb\'e9 \'f0\'e0\'e7\'ec\'e5\'f0 \'e1\'f0\'e0\'f3\'e7\'e5\'f0\'e0\par
\cf2\'d1\'e8\'ed\'f2\'e0\'ea\'f1\'e8\'f1\cf0 : BrowserFullScreenAsync()\par
\cf3\par
\cf2\'cf\'f0\'e8\'ec\'e5\'f0\cf3 :\par
\cf0 await tester.BrowserFullScreenAsync();\par
}",

@"{\rtf1\ansi\ansicpg1251\deff0\nouicompat\deflang1049{\fonttbl{\f0\fnil\fcharset0 Calibri;}{\f1\fnil\fcharset204 Calibri;}}
{\colortbl ;\red0\green77\blue187;\red155\green0\blue211;\red0\green0\blue0;}
{\*\generator Riched20 10.0.22000}\viewkind4\uc1 
\pard\sl276\slmult1\cf1\f0\fs20\lang9 BrowserSetUserAgentAsync\cf0\par
\cf2\f1\lang1049\'ce\'ef\'e8\'f1\'e0\'ed\'e8\'e5\cf0 : \'ec\'e5\'f2\'ee\'e4 \'f3\'f1\'f2\'e0\'ed\'e0\'e2\'eb\'e8\'e2\'e0\'e5\'f2 \'e7\'ed\'e0\'f7\'e5\'ed\'e8\'e5 \f0\lang1033 User-Agent\f1\lang1049  \'e4\'eb\'ff \'e1\'f0\'e0\'f3\'e7\'e5\'f0\'e0\par
\cf2\'d1\'e8\'ed\'f2\'e0\'ea\'f1\'e8\'f1\cf0 : BrowserSetUserAgentAsync(string value)\par
\cf3\par
\cf2\'cf\'f0\'e8\'ec\'e5\'f0\cf3 :\par
\cf0 await tester.BrowserSetUserAgentAsync(\f0\lang1033 ""my user-agent""\f1\lang1049 );\par
}",

@"{\rtf1\ansi\ansicpg1251\deff0\nouicompat\deflang1049{\fonttbl{\f0\fnil\fcharset0 Calibri;}{\f1\fnil\fcharset204 Calibri;}}
{\colortbl ;\red0\green77\blue187;\red155\green0\blue211;\red0\green0\blue0;}
{\*\generator Riched20 10.0.22000}\viewkind4\uc1 
\pard\sl276\slmult1\cf1\f0\fs20\lang9 BrowserGetUserAgentAsync\cf0\par
\cf2\f1\lang1049\'ce\'ef\'e8\'f1\'e0\'ed\'e8\'e5\cf0 : \'ec\'e5\'f2\'ee\'e4 \'e2\'ee\'e7\'e2\'f0\'e0\'f9\'e0\'e5\'f2 \'f1\'f2\'f0\'ee\'f7\'ed\'ee\'e5 \'e7\'ed\'e0\'f7\'e5\'ed\'e8\'e5 \f0\lang1033 User-Agent\f1\lang1049  \'e1\'f0\'e0\'f3\'e7\'e5\'f0\'e0\par
\cf2\'d1\'e8\'ed\'f2\'e0\'ea\'f1\'e8\'f1\cf0 : BrowserGetUserAgentAsync()\par
\cf3\par
\cf2\'cf\'f0\'e8\'ec\'e5\'f0\cf3 :\par
\cf0\f0\lang1033 string ua = \f1\lang1049 await tester.BrowserGetUserAgentAsync();\par
}",

@"{\rtf1\ansi\ansicpg1251\deff0\nouicompat\deflang1049{\fonttbl{\f0\fnil\fcharset0 Calibri;}{\f1\fnil\fcharset204 Calibri;}}
{\colortbl ;\red0\green77\blue187;\red155\green0\blue211;\red0\green0\blue0;}
{\*\generator Riched20 10.0.22000}\viewkind4\uc1 
\pard\sl276\slmult1\cf1\f0\fs20\lang9 ConsoleMsg\cf0\par
\cf2\f1\lang1049\'ce\'ef\'e8\'f1\'e0\'ed\'e8\'e5\cf0 : \'ec\'e5\'f2\'ee\'e4 \'e2\'fb\'e2\'ee\'e4\'e8\'f2 \'f1\'ee\'ee\'e1\'f9\'e5\'ed\'e8\'e5 \'e2 \'ea\'ee\'ed\'f1\'ee\'eb\'e8 \'e1\'f0\'e0\'f3\'e7\'e5\'f0\'e0\par
\cf2\'d1\'e8\'ed\'f2\'e0\'ea\'f1\'e8\'f1\cf0 : ConsoleMsg(string message)\par
\cf3\par
\cf2\'cf\'f0\'e8\'ec\'e5\'f0\cf3 :\par
\cf0 tester.ConsoleMsg(\f0\lang1033 ""\f1\lang1049\'f2\'e5\'ea\'f1\'f2 \'f1\'ee\'ee\'e1\'f9\'e5\'ed\'e8\'ff\f0\lang1033 ""\f1\lang1049 );\par
}",

@"{\rtf1\ansi\ansicpg1251\deff0\nouicompat\deflang1049{\fonttbl{\f0\fnil\fcharset0 Calibri;}{\f1\fnil\fcharset204 Calibri;}}
{\colortbl ;\red0\green77\blue187;\red155\green0\blue211;\red0\green0\blue0;}
{\*\generator Riched20 10.0.22000}\viewkind4\uc1 
\pard\sl276\slmult1\cf1\f0\fs20\lang9 ConsoleMsgError\cf0\par
\cf2\f1\lang1049\'ce\'ef\'e8\'f1\'e0\'ed\'e8\'e5\cf0 : \'ec\'e5\'f2\'ee\'e4 \'e2\'fb\'e2\'ee\'e4\'e8\'f2 \'f1\'ee\'ee\'e1\'f9\'e5\'ed\'e8\'e5 \'ee\'e1 \'ee\'f8\'e8\'e1\'ea\'e5 \'e2 \'f1\'e8\'f1\'f2\'e5\'ec\'ed\'f3\'fe \'ea\'ee\'ed\'f1\'ee\'eb\'fc \'e8 \'ea\'ee\'ed\'f1\'ee\'eb\'fc \'e1\'f0\'e0\'f3\'e7\'e5\'f0\'e0\par
\cf2\'d1\'e8\'ed\'f2\'e0\'ea\'f1\'e8\'f1\cf0 : ConsoleMsgError(string message)\par
\cf3\par
\cf2\'cf\'f0\'e8\'ec\'e5\'f0\cf3 :\par
\cf0 try\par
\{\par
\tab\par
\}\par
catch (Exception ex)\par
\{\par
\tab tester.ConsoleMsgError(ex.ToString());\par
\}\par
}",

@"{\rtf1\ansi\ansicpg1251\deff0\nouicompat\deflang1049{\fonttbl{\f0\fnil\fcharset0 Calibri;}{\f1\fnil\fcharset204 Calibri;}}
{\colortbl ;\red0\green77\blue187;\red155\green0\blue211;\red0\green0\blue0;}
{\*\generator Riched20 10.0.22000}\viewkind4\uc1 
\pard\sl276\slmult1\cf1\f0\fs20\lang9 ClearMessage\cf0\par
\cf2\f1\lang1049\'ce\'ef\'e8\'f1\'e0\'ed\'e8\'e5\cf0 : \'ec\'e5\'f2\'ee\'e4 \'ee\'f7\'e8\'f9\'e0\'e5\'f2 \'f1\'ee\'ee\'e1\'f9\'e5\'ed\'e8\'ff \'e2 \'f2\'e0\'e1\'eb\'e8\'f6\'e5 \'e2\'fb\'e2\'ee\'e4\'e0 \'ef\'f0\'ee\'f6\'e5\'f1\'f1\'e0 \'e2\'fb\'ef\'ee\'eb\'ed\'e5\'ed\'e8\'ff \'f2\'e5\'f1\'f2\'e5\par
\cf2\'d1\'e8\'ed\'f2\'e0\'ea\'f1\'e8\'f1\cf0 : ClearMessage()\par
\cf3\par
\cf2\'cf\'f0\'e8\'ec\'e5\'f0\cf3 :\par
\cf0 tester.ClearMessage();\par
}",

@"{\rtf1\ansi\ansicpg1251\deff0\nouicompat\deflang1049{\fonttbl{\f0\fnil\fcharset0 Calibri;}{\f1\fnil\fcharset204 Calibri;}}
{\colortbl ;\red0\green77\blue187;\red155\green0\blue211;\red0\green0\blue0;}
{\*\generator Riched20 10.0.22000}\viewkind4\uc1 
\pard\sl276\slmult1\cf1\f0\fs20\lang9 SendMessage\cf0\par
\cf2\f1\lang1049\'ce\'ef\'e8\'f1\'e0\'ed\'e8\'e5\cf0 : \'ec\'e5\'f2\'ee\'e4 \'e2\'fb\'e2\'ee\'e4\'e8\'f2 \'f1\'ee\'ee\'e1\'f9\'e5\'ed\'e8\'ff \'e2 \'f2\'e0\'e1\'eb\'e8\'f6\'e5 \'ef\'f0\'ee\'f6\'e5\'f1\'f1\'e0 \'e2\'fb\'ef\'ee\'eb\'ed\'e5\'ed\'e8\'ff \'f2\'e5\'f1\'f2\'e0\par
\cf2\'d1\'e8\'ed\'f2\'e0\'ea\'f1\'e8\'f1\cf0 : SendMessage(string action, string status, string comment)\par
\cf3\par
\cf2\'cf\'f0\'e8\'ec\'e5\'f0\cf3 :\par
\cf0 SendMessage(""\'f2\'e5\'ea\'f1\'f2 \'e4\'e5\'e9\'f1\'f2\'e2\'e8\'ff"", \f0\lang1033 Tester.\f1\lang1049 PROCESS, ""\'f2\'e5\'ea\'f1\'f2 \'ea\'ee\'ec\'ec\'e5\'ed\'f2\'e0\'f0\'e8\'ff"");\par
}",

@"{\rtf1\ansi\ansicpg1251\deff0\nouicompat\deflang1049{\fonttbl{\f0\fnil\fcharset0 Calibri;}{\f1\fnil\fcharset204 Calibri;}}
{\colortbl ;\red0\green77\blue187;\red155\green0\blue211;\red0\green0\blue0;}
{\*\generator Riched20 10.0.22000}\viewkind4\uc1 
\pard\sl276\slmult1\cf1\f0\fs20\lang9 EditMessage\cf0\par
\cf2\f1\lang1049\'ce\'ef\'e8\'f1\'e0\'ed\'e8\'e5\cf0 : \'ec\'e5\'f2\'ee\'e4 \'e8\'e7\'ec\'e5\'ed\'ff\'e5\'f2 \'f0\'e0\'ed\'e5\'e5 \'e2\'fb\'e2\'e5\'e4\'e5\'ed\'ed\'ee\'e5 \'f1\'ee\'ee\'e1\'f9\'e5\'ed\'e8\'ff \'e2 \'f2\'e0\'e1\'eb\'e8\'f6\'e5 \'ef\'f0\'ee\'f6\'e5\'f1\'f1\'e0 \'e2\'fb\'ef\'ee\'eb\'ed\'e5\'ed\'e8\'ff \'f2\'e5\'f1\'f2\'e0\par
\cf2\'d1\'e8\'ed\'f2\'e0\'ea\'f1\'e8\'f1\cf0 : EditMessage(int index, string action, string status, string comment, int image)\par
\cf3\par
\cf2\'cf\'f0\'e8\'ec\'e5\'f0\cf3 :\par
\cf0\f0\lang1033 tester.\f1\lang1049 EditMessage(step, ""\'f2\'e5\'ea\'f1\'f2 \'e4\'e5\'e9\'f1\'f2\'e2\'e8\'ff"", \f0\lang1033 Tester.\f1\lang1049 PASSED, ""\'f2\'e5\'ea\'f1\'f2 \'ea\'ee\'ec\'ec\'e5\'ed\'f2\'e0\'f0\'e8\'ff"", \f0\lang1033 Tester.\f1\lang1049 IMAGE_STATUS_PASSED);\par
\par
\f0\lang1033 tester.\f1\lang1049 EditMessage(step, \f0\lang1033 null\f1\lang1049 , \f0\lang1033 Tester.\f1\lang1049 FAILED, ""\'f2\'e5\'ea\'f1\'f2 \'ea\'ee\'ec\'ec\'e5\'ed\'f2\'e0\'f0\'e8\'ff"", \f0\lang1033 Tester.\f1\lang1049 IMAGE_STATUS_FAILED);\par
\par
    }",

@"{\rtf1\ansi\ansicpg1251\deff0\nouicompat\deflang1049{\fonttbl{\f0\fnil\fcharset0 Calibri;}{\f1\fnil\fcharset204 Calibri;}}
{\colortbl ;\red0\green77\blue187;\red155\green0\blue211;\red0\green0\blue0;}
{\*\generator Riched20 10.0.22000}\viewkind4\uc1 
\pard\sl276\slmult1\cf1\f0\fs20\lang9 TestBeginAsync\cf0\par
\cf2\f1\lang1049\'ce\'ef\'e8\'f1\'e0\'ed\'e8\'e5\cf0 : \'ec\'e5\'f2\'ee\'e4 \'ef\'ee\'e4\'e3\'ee\'f2\'ee\'e2\'ea\'e8 \'ea \'f2\'e5\'f1\'f2\'f3 (\'ea\'e0\'e6\'e4\'fb\'e9 \'f2\'e5\'f1\'f2 \'ee\'e1\'ff\'e7\'e0\'f2\'e5\'eb\'fc\'ed\'ee \'e4\'ee\'eb\'e6\'e5\'ed \'ed\'e0\'f7\'e8\'ed\'e0\'f2\'fc\'f1\'ff \'fd\'f2\'e8\'ec \'ec\'e5\'f2\'ee\'e4\'ee\'ec)\par
\cf2\'d1\'e8\'ed\'f2\'e0\'ea\'f1\'e8\'f1\cf0 : TestBeginAsync()\par
\cf3\par
\cf2\'cf\'f0\'e8\'ec\'e5\'f0\cf3 :\par
\cf0\f0\lang1033 await tester.\f1\lang1049 TestBeginAsync();\par
}",

@"{\rtf1\ansi\ansicpg1251\deff0\nouicompat\deflang1049{\fonttbl{\f0\fnil\fcharset0 Calibri;}{\f1\fnil\fcharset204 Calibri;}}
{\colortbl ;\red0\green77\blue187;\red155\green0\blue211;\red0\green0\blue0;}
{\*\generator Riched20 10.0.22000}\viewkind4\uc1 
\pard\sl276\slmult1\cf1\f0\fs20\lang9 TestEndAsync\cf0\par
\cf2\f1\lang1049\'ce\'ef\'e8\'f1\'e0\'ed\'e8\'e5\cf0 : \'ec\'e5\'f2\'ee\'e4 \'e7\'e0\'e2\'e5\'f0\'f8\'e5\'ed\'e8\'ff \'f2\'e5\'f1\'f2\'e0 (\'ea\'e0\'e6\'e4\'fb\'e9 \'f2\'e5\'f1\'f2 \'ee\'e1\'ff\'e7\'e0\'f2\'e5\'eb\'fc\'ed\'ee \'e4\'ee\'eb\'e6\'e5\'ed \'e7\'e0\'ea\'e0\'ed\'f7\'e8\'e2\'e0\'f2\'fc\'f1\'ff \'fd\'f2\'e8\'ec \'ec\'e5\'f2\'ee\'e4\'ee\'ec)\par
\cf2\'d1\'e8\'ed\'f2\'e0\'ea\'f1\'e8\'f1\cf0 : TestEndAsync()\par
\cf3\par
\cf2\'cf\'f0\'e8\'ec\'e5\'f0\cf3 :\par
\cf0\f0\lang1033 await tester.\f1\lang1049 TestEndAsync();\par
}",

@"{\rtf1\ansi\ansicpg1251\deff0\nouicompat\deflang1049{\fonttbl{\f0\fnil\fcharset0 Calibri;}{\f1\fnil\fcharset204 Calibri;}}
{\colortbl ;\red0\green77\blue187;\red155\green0\blue211;\red0\green0\blue0;}
{\*\generator Riched20 10.0.22000}\viewkind4\uc1 
\pard\sl276\slmult1\cf1\f0\fs20\lang9 TestStopAsync\cf0\par
\cf2\f1\lang1049\'ce\'ef\'e8\'f1\'e0\'ed\'e8\'e5\cf0 : \'ec\'e5\'f2\'ee\'e4 \'ef\'f0\'e8\'ed\'f3\'e4\'e8\'f2\'e5\'eb\'fc\'ed\'ee \'ee\'f1\'f2\'e0\'ed\'e0\'e2\'eb\'e8\'e2\'e0\'e5\'f2 \'ef\'f0\'ee\'f6\'e5\'f1\'f1 \'f2\'e5\'f1\'f2\'e8\'f0\'ee\'e2\'e0\'ed\'e8\'ff\par
\cf2\'d1\'e8\'ed\'f2\'e0\'ea\'f1\'e8\'f1\cf0 : TestStopAsync()\par
\cf3\par
\cf2\'cf\'f0\'e8\'ec\'e5\'f0\cf3 :\par
\cf0\f0\lang1033 await tester.\f1\lang1049 TestStopAsync();\par
}",

@"{\rtf1\ansi\ansicpg1251\deff0\nouicompat\deflang1049{\fonttbl{\f0\fnil\fcharset0 Calibri;}{\f1\fnil\fcharset204 Calibri;}}
{\colortbl ;\red0\green77\blue187;\red155\green0\blue211;\red0\green0\blue0;}
{\*\generator Riched20 10.0.22000}\viewkind4\uc1 
\pard\sl276\slmult1\cf1\f0\fs20\lang9 DefineTestStop\cf0\par
\cf2\f1\lang1049\'ce\'ef\'e8\'f1\'e0\'ed\'e8\'e5\cf0 : \'ec\'e5\'f2\'ee\'e4 \'ef\'f0\'ee\'e2\'e5\'f0\'ff\'e5\'f2 \'f1\'f2\'e0\'f2\'f3\'f1 \'ef\'f0\'ee\'f6\'e5\'f1\'f1\'e0 (\'ee\'f1\'f2\'e0\'ed\'ee\'e2\'eb\'e5\'ed \'e8\'eb\'e8 \'ed\'e5\'f2)\par
\cf2\'d1\'e8\'ed\'f2\'e0\'ea\'f1\'e8\'f1\cf0 : DefineTestStop()\par
\cf3\par
\cf2\'cf\'f0\'e8\'ec\'e5\'f0\cf3 :\par
\cf0\f0\lang1033 if (tester.DefineTestStop() == true) return;\f1\lang1049\par
}",

@"{\rtf1\ansi\ansicpg1251\deff0\nouicompat\deflang1049{\fonttbl{\f0\fnil\fcharset0 Calibri;}{\f1\fnil\fcharset204 Calibri;}}
{\colortbl ;\red0\green77\blue187;\red155\green0\blue211;\red0\green0\blue0;}
{\*\generator Riched20 10.0.22000}\viewkind4\uc1 
\pard\sl276\slmult1\cf1\f0\fs20\lang9 ClickElementByClassAsync\cf0\par
\cf2\f1\lang1049\'ce\'ef\'e8\'f1\'e0\'ed\'e8\'e5\cf0 : \'ec\'e5\'f2\'ee\'e4 \'e2\'fb\'ef\'ee\'eb\'ed\'ff\'e5\'f2 \'ed\'e0\'e6\'e0\'f2\'e8\'e5 \'ed\'e0 \'fd\'eb\'e5\'ec\'e5\'ed\'f2\par
\cf2\'d1\'e8\'ed\'f2\'e0\'ea\'f1\'e8\'f1\cf0 : ClickElementByClassAsync(string _class, int index);\par
\cf3\par
\cf2\'cf\'f0\'e8\'ec\'e5\'f0\cf3 :\par
\cf0\f0\lang1033 await tester.ClickElementByClassAsync(""my-element"", 0)\f1\lang1049\par
}",

@"{\rtf1\ansi\ansicpg1251\deff0\nouicompat\deflang1049{\fonttbl{\f0\fnil\fcharset0 Calibri;}{\f1\fnil\fcharset204 Calibri;}}
{\colortbl ;\red0\green77\blue187;\red155\green0\blue211;\red0\green0\blue0;}
{\*\generator Riched20 10.0.22000}\viewkind4\uc1 
\pard\sl276\slmult1\cf1\f0\fs20\lang9 ClickElementAsync\cf0\par
\cf2\f1\lang1049\'ce\'ef\'e8\'f1\'e0\'ed\'e8\'e5\cf0 : \'ec\'e5\'f2\'ee\'e4 \'e2\'fb\'ef\'ee\'eb\'ed\'ff\'e5\'f2 \'ed\'e0\'e6\'e0\'f2\'e8\'e5 \'ed\'e0 \'fd\'eb\'e5\'ec\'e5\'ed\'f2\par
\cf2\'d1\'e8\'ed\'f2\'e0\'ea\'f1\'e8\'f1\cf0 : \f0\lang1033 ClickElementAsync(string by, string locator)\f1\lang1049\par
\cf3\par
\cf2\'cf\'f0\'e8\'ec\'e5\'f0\cf3 :\par
\cf0\f0\lang1033 await tester.ClickElementAsync(Tester.BY_CSS, ""#auth #buttonLogin"");\par
\par
await tester.ClickElementAsync(Tester.BY_XPATH, ""//div[@id='auth']//input[@id='buttonLogin']"");\par
    }",

@"{\rtf1\ansi\ansicpg1251\deff0\nouicompat\deflang1049{\fonttbl{\f0\fnil\fcharset0 Calibri;}{\f1\fnil\fcharset204 Calibri;}}
{\colortbl ;\red0\green77\blue187;\red155\green0\blue211;\red0\green0\blue0;}
{\*\generator Riched20 10.0.22000}\viewkind4\uc1 
\pard\sl276\slmult1\cf1\f0\fs20\lang9 ClickElementByIdAsync\cf0\par
\cf2\f1\lang1049\'ce\'ef\'e8\'f1\'e0\'ed\'e8\'e5\cf0 : \'ec\'e5\'f2\'ee\'e4 \'e2\'fb\'ef\'ee\'eb\'ed\'ff\'e5\'f2 \'ed\'e0\'e6\'e0\'f2\'e8\'e5 \'ed\'e0 \'fd\'eb\'e5\'ec\'e5\'ed\'f2\par
\cf2\'d1\'e8\'ed\'f2\'e0\'ea\'f1\'e8\'f1\cf0 : ClickElementByIdAsync(string id)\par
\cf3\par
\cf2\'cf\'f0\'e8\'ec\'e5\'f0\cf3 :\par
\cf0\f0\lang1033 await tester.ClickElementByIdAsync(""MyElement"");\f1\lang1049\par
}",

@"{\rtf1\ansi\ansicpg1251\deff0\nouicompat\deflang1049{\fonttbl{\f0\fnil\fcharset0 Calibri;}{\f1\fnil\fcharset204 Calibri;}}
{\colortbl ;\red0\green77\blue187;\red155\green0\blue211;\red0\green0\blue0;}
{\*\generator Riched20 10.0.22000}\viewkind4\uc1 
\pard\sl276\slmult1\cf1\f0\fs20\lang9 ClickElementByNameAsync\cf0\par
\cf2\f1\lang1049\'ce\'ef\'e8\'f1\'e0\'ed\'e8\'e5\cf0 : \'ec\'e5\'f2\'ee\'e4 \'e2\'fb\'ef\'ee\'eb\'ed\'ff\'e5\'f2 \'ed\'e0\'e6\'e0\'f2\'e8\'e5 \'ed\'e0 \'fd\'eb\'e5\'ec\'e5\'ed\'f2\par
\cf2\'d1\'e8\'ed\'f2\'e0\'ea\'f1\'e8\'f1\cf0 : ClickElementByNameAsync(string name, int index)\par
\cf3\par
\cf2\'cf\'f0\'e8\'ec\'e5\'f0\cf3 :\par
\cf0\f0\lang1033 await tester.ClickElementByNameAsync(""MyElement"", 0);\f1\lang1049\par
}",

@"{\rtf1\ansi\ansicpg1251\deff0\nouicompat\deflang1049{\fonttbl{\f0\fnil\fcharset0 Calibri;}{\f1\fnil\fcharset204 Calibri;}}
{\colortbl ;\red0\green77\blue187;\red155\green0\blue211;\red0\green0\blue0;}
{\*\generator Riched20 10.0.22000}\viewkind4\uc1 
\pard\sl276\slmult1\cf1\f0\fs20\lang9 ClickElementByTagAsync\cf0\par
\cf2\f1\lang1049\'ce\'ef\'e8\'f1\'e0\'ed\'e8\'e5\cf0 : \'ec\'e5\'f2\'ee\'e4 \'e2\'fb\'ef\'ee\'eb\'ed\'ff\'e5\'f2 \'ed\'e0\'e6\'e0\'f2\'e8\'e5 \'ed\'e0 \'fd\'eb\'e5\'ec\'e5\'ed\'f2\par
\cf2\'d1\'e8\'ed\'f2\'e0\'ea\'f1\'e8\'f1\cf0 : ClickElementByTagAsync(string tag, int index)\par
\cf3\par
\cf2\'cf\'f0\'e8\'ec\'e5\'f0\cf3 :\par
\cf0\f0\lang1033 await tester.ClickElementByTagAsync(""a"", 0);\f1\lang1049\par
}",

@"{\rtf1\ansi\ansicpg1251\deff0\nouicompat\deflang1049{\fonttbl{\f0\fnil\fcharset0 Calibri;}{\f1\fnil\fcharset204 Calibri;}}
{\colortbl ;\red0\green77\blue187;\red155\green0\blue211;\red0\green0\blue0;}
{\*\generator Riched20 10.0.22000}\viewkind4\uc1 
\pard\sl276\slmult1\cf1\f0\fs20\lang9 FindElementByClassAsync\cf0\par
\cf2\f1\lang1049\'ce\'ef\'e8\'f1\'e0\'ed\'e8\'e5\cf0 : \'ec\'e5\'f2\'ee\'e4 \'e2\'fb\'ef\'ee\'eb\'ed\'ff\'e5\'f2 \'ef\'ee\'e8\'f1\'ea \'fd\'eb\'e5\'ec\'e5\'ed\'f2 \'e2 DOM \'f1 \'ee\'e6\'e8\'e4\'e0\'ed\'e8\'e5\'ec \'e2 \'f1\'e5\'ea\'f3\'ed\'e4\'e0\'f5 \'e8 \'e2\'ee\'e7\'e2\'f0\'e0\'f9\'e0\'e5\'f2 \'eb\'ee\'e3\'e8\'f7\'e5\'f1\'ea\'e8\'e9 \'f0\'e5\'e7\'f3\'eb\'fc\'f2\'e0\'f2 \f0\lang1033 true \f1\lang1049\'e8\'eb\'e8 \f0\lang1033 false\f1\lang1049\par
\cf2\'d1\'e8\'ed\'f2\'e0\'ea\'f1\'e8\'f1\cf0 : FindElementByClassAsync(string _class, int index, int sec)\par
\cf3\par
\cf2\'cf\'f0\'e8\'ec\'e5\'f0\cf3 :\par
\cf0\f0\lang1033 bool result = await tester.FindElementByClassAsync(""my-element"", 0, 5);\f1\lang1049\par
}",

@"{\rtf1\ansi\ansicpg1251\deff0\nouicompat\deflang1049{\fonttbl{\f0\fnil\fcharset0 Calibri;}{\f1\fnil\fcharset204 Calibri;}}
{\colortbl ;\red0\green77\blue187;\red155\green0\blue211;\red0\green0\blue0;}
{\*\generator Riched20 10.0.22000}\viewkind4\uc1 
\pard\sl276\slmult1\cf1\f0\fs20\lang9 FindElementAsync\cf0\par
\cf2\f1\lang1049\'ce\'ef\'e8\'f1\'e0\'ed\'e8\'e5\cf0 : \'ec\'e5\'f2\'ee\'e4 \'e2\'fb\'ef\'ee\'eb\'ed\'ff\'e5\'f2 \'ef\'ee\'e8\'f1\'ea \'fd\'eb\'e5\'ec\'e5\'ed\'f2 \'e2 DOM \'f1 \'ee\'e6\'e8\'e4\'e0\'ed\'e8\'e5\'ec \'e2 \'f1\'e5\'ea\'f3\'ed\'e4\'e0\'f5 \'e8 \'e2\'ee\'e7\'e2\'f0\'e0\'f9\'e0\'e5\'f2 \'eb\'ee\'e3\'e8\'f7\'e5\'f1\'ea\'e8\'e9 \'f0\'e5\'e7\'f3\'eb\'fc\'f2\'e0\'f2 true \'e8\'eb\'e8 false\par
\cf2\'d1\'e8\'ed\'f2\'e0\'ea\'f1\'e8\'f1\cf0 : \f0\lang1033 FindElementAsync(string by, string locator, int sec)\f1\lang1049\par
\cf3\par
\cf2\'cf\'f0\'e8\'ec\'e5\'f0\cf3 :\par
\cf0\f0\lang1033 bool result = await tester.FindElementAsync(Tester.BY_CSS, ""div[id='result']"", 2);\par
\par
bool result = await tester.FindElementAsync(Tester.BY_XPATH, ""//div[@id='result']"", 2);\par
    }",

@"{\rtf1\ansi\ansicpg1251\deff0\nouicompat\deflang1049{\fonttbl{\f0\fnil\fcharset0 Calibri;}{\f1\fnil\fcharset204 Calibri;}}
{\colortbl ;\red0\green77\blue187;\red155\green0\blue211;\red0\green0\blue0;}
{\*\generator Riched20 10.0.22000}\viewkind4\uc1 
\pard\sl276\slmult1\cf1\f0\fs20\lang9 FindElementByIdAsync\cf0\par
\cf2\f1\lang1049\'ce\'ef\'e8\'f1\'e0\'ed\'e8\'e5\cf0 : \'ec\'e5\'f2\'ee\'e4 \'e2\'fb\'ef\'ee\'eb\'ed\'ff\'e5\'f2 \'ef\'ee\'e8\'f1\'ea \'fd\'eb\'e5\'ec\'e5\'ed\'f2 \'e2 DOM \'f1 \'ee\'e6\'e8\'e4\'e0\'ed\'e8\'e5\'ec \'e2 \'f1\'e5\'ea\'f3\'ed\'e4\'e0\'f5 \'e8 \'e2\'ee\'e7\'e2\'f0\'e0\'f9\'e0\'e5\'f2 \'eb\'ee\'e3\'e8\'f7\'e5\'f1\'ea\'e8\'e9 \'f0\'e5\'e7\'f3\'eb\'fc\'f2\'e0\'f2 \f0\lang1033 true \f1\lang1049\'e8\'eb\'e8 \f0\lang1033 false\f1\lang1049\par
\cf2\'d1\'e8\'ed\'f2\'e0\'ea\'f1\'e8\'f1\cf0 : FindElementByIdAsync(string id, int sec)\par
\cf3\par
\cf2\'cf\'f0\'e8\'ec\'e5\'f0\cf3 :\par
\cf0\f0\lang1033 bool result = await tester.FindElementByIdAsync(""MyElement"", 5);\f1\lang1049\par
}",

@"{\rtf1\ansi\ansicpg1251\deff0\nouicompat\deflang1049{\fonttbl{\f0\fnil\fcharset0 Calibri;}{\f1\fnil\fcharset204 Calibri;}}
{\colortbl ;\red0\green77\blue187;\red155\green0\blue211;\red0\green0\blue0;}
{\*\generator Riched20 10.0.22000}\viewkind4\uc1 
\pard\sl276\slmult1\cf1\f0\fs20\lang9 FindElementByNameAsync\cf0\par
\cf2\f1\lang1049\'ce\'ef\'e8\'f1\'e0\'ed\'e8\'e5\cf0 : \'ec\'e5\'f2\'ee\'e4 \'e2\'fb\'ef\'ee\'eb\'ed\'ff\'e5\'f2 \'ef\'ee\'e8\'f1\'ea \'fd\'eb\'e5\'ec\'e5\'ed\'f2 \'e2 DOM \'f1 \'ee\'e6\'e8\'e4\'e0\'ed\'e8\'e5\'ec \'e2 \'f1\'e5\'ea\'f3\'ed\'e4\'e0\'f5 \'e8 \'e2\'ee\'e7\'e2\'f0\'e0\'f9\'e0\'e5\'f2 \'eb\'ee\'e3\'e8\'f7\'e5\'f1\'ea\'e8\'e9 \'f0\'e5\'e7\'f3\'eb\'fc\'f2\'e0\'f2 \f0\lang1033 true \f1\lang1049\'e8\'eb\'e8 \f0\lang1033 false\f1\lang1049\par
\cf2\'d1\'e8\'ed\'f2\'e0\'ea\'f1\'e8\'f1\cf0 : FindElementByNameAsync(string name, int index, int sec)\par
\cf3\par
\cf2\'cf\'f0\'e8\'ec\'e5\'f0\cf3 :\par
\cf0\f0\lang1033 bool result = await tester.FindElementByNameAsync(""MyElement"", 0, 5);\f1\lang1049\par
}",

@"{\rtf1\ansi\ansicpg1251\deff0\nouicompat\deflang1049{\fonttbl{\f0\fnil\fcharset0 Calibri;}{\f1\fnil\fcharset204 Calibri;}}
{\colortbl ;\red0\green77\blue187;\red155\green0\blue211;\red0\green0\blue0;}
{\*\generator Riched20 10.0.22000}\viewkind4\uc1 
\pard\sl276\slmult1\cf1\f0\fs20\lang9 FindElementByTagAsync\cf0\par
\cf2\f1\lang1049\'ce\'ef\'e8\'f1\'e0\'ed\'e8\'e5\cf0 : \'ec\'e5\'f2\'ee\'e4 \'e2\'fb\'ef\'ee\'eb\'ed\'ff\'e5\'f2 \'ef\'ee\'e8\'f1\'ea \'fd\'eb\'e5\'ec\'e5\'ed\'f2 \'e2 DOM \'f1 \'ee\'e6\'e8\'e4\'e0\'ed\'e8\'e5\'ec \'e2 \'f1\'e5\'ea\'f3\'ed\'e4\'e0\'f5 \'e8 \'e2\'ee\'e7\'e2\'f0\'e0\'f9\'e0\'e5\'f2 \'eb\'ee\'e3\'e8\'f7\'e5\'f1\'ea\'e8\'e9 \'f0\'e5\'e7\'f3\'eb\'fc\'f2\'e0\'f2 \f0\lang1033 true \f1\lang1049\'e8\'eb\'e8 \f0\lang1033 false\f1\lang1049\par
\cf2\'d1\'e8\'ed\'f2\'e0\'ea\'f1\'e8\'f1\cf0 : FindElementByTagAsync(string tag, int index, int sec)\par
\cf3\par
\cf2\'cf\'f0\'e8\'ec\'e5\'f0\cf3 :\par
\cf0\f0\lang1033 bool result = await tester.FindElementByTagAsync(""h1"", 0, 5);\f1\lang1049\par
}",

@"{\rtf1\ansi\ansicpg1251\deff0\nouicompat\deflang1049{\fonttbl{\f0\fnil\fcharset0 Calibri;}{\f1\fnil\fcharset204 Calibri;}}
{\colortbl ;\red0\green77\blue187;\red155\green0\blue211;\red0\green0\blue0;}
{\*\generator Riched20 10.0.22000}\viewkind4\uc1 
\pard\sl276\slmult1\cf1\f0\fs20\lang9 FindVisibleElementByClassAsync\cf0\par
\cf2\f1\lang1049\'ce\'ef\'e8\'f1\'e0\'ed\'e8\'e5\cf0 : \'ec\'e5\'f2\'ee\'e4 \'e2\'fb\'ef\'ee\'eb\'ed\'ff\'e5\'f2 \'ef\'ee\'e8\'f1\'ea\f0\lang1033  \f1\lang1049\'e2\'e8\'e7\'f3\'e0\'eb\'fc\'ed\'ee \'ee\'f2\'ee\'e1\'f0\'e0\'e6\'e0\'e5\'ec\'ee\'e3\'ee \'fd\'eb\'e5\'ec\'e5\'ed\'f2\'e0 \'f1 \'ee\'e6\'e8\'e4\'e0\'ed\'e8\'e5\'ec \'e2 \'f1\'e5\'ea\'f3\'ed\'e4\'e0\'f5 \'e8 \'e2\'ee\'e7\'e2\'f0\'e0\'f9\'e0\'e5\'f2 \'eb\'ee\'e3\'e8\'f7\'e5\'f1\'ea\'e8\'e9 \'f0\'e5\'e7\'f3\'eb\'fc\'f2\'e0\'f2 \f0\lang1033 true \f1\lang1049\'e8\'eb\'e8 \f0\lang1033 false\f1\lang1049\par
\cf2\'d1\'e8\'ed\'f2\'e0\'ea\'f1\'e8\'f1\cf0 : FindVisibleElementByClassAsync(string _class, int index, int sec)\par
\cf3\par
\cf2\'cf\'f0\'e8\'ec\'e5\'f0\cf3 :\par
\cf0\f0\lang1033 bool result = await tester.FindVisibleElementByClassAsync(""my-element"", 0, 5);\f1\lang1049\par
}",

@"{\rtf1\ansi\ansicpg1251\deff0\nouicompat\deflang1049{\fonttbl{\f0\fnil\fcharset0 Calibri;}{\f1\fnil\fcharset204 Calibri;}}
{\colortbl ;\red0\green77\blue187;\red155\green0\blue211;\red0\green0\blue0;}
{\*\generator Riched20 10.0.22000}\viewkind4\uc1 
\pard\sl276\slmult1\cf1\f0\fs20\lang9 FindVisibleElementAsync\cf0\par
\cf2\f1\lang1049\'ce\'ef\'e8\'f1\'e0\'ed\'e8\'e5\cf0 : \'ec\'e5\'f2\'ee\'e4 \'e2\'fb\'ef\'ee\'eb\'ed\'ff\'e5\'f2 \'ef\'ee\'e8\'f1\'ea \'e2\'e8\'e7\'f3\'e0\'eb\'fc\'ed\'ee \'ee\'f2\'ee\'e1\'f0\'e0\'e6\'e0\'e5\'ec\'ee\'e3\'ee \'fd\'eb\'e5\'ec\'e5\'ed\'f2\'e0 \'f1 \'ee\'e6\'e8\'e4\'e0\'ed\'e8\'e5\'ec \'e2 \'f1\'e5\'ea\'f3\'ed\'e4\'e0\'f5 \'e8 \'e2\'ee\'e7\'e2\'f0\'e0\'f9\'e0\'e5\'f2 \'eb\'ee\'e3\'e8\'f7\'e5\'f1\'ea\'e8\'e9 \'f0\'e5\'e7\'f3\'eb\'fc\'f2\'e0\'f2 true \'e8\'eb\'e8 false\par
\cf2\'d1\'e8\'ed\'f2\'e0\'ea\'f1\'e8\'f1\cf0 : \f0\lang1033 FindVisibleElementAsync(string by, string locator, int sec)\f1\lang1049\par
\cf3\par
\cf2\'cf\'f0\'e8\'ec\'e5\'f0\cf3 :\par
\cf0\f0\lang1033 bool result = await tester.FindVisibleElementAsync(Tester.BY_CSS, ""#auth #buttonLogin"", 2);\par
\par
bool result = await tester.FindVisibleElementAsync(Tester.BY_XPATH, ""//div[@id='auth']//input[@id='buttonLogin']"", 2);\par
    }",

@"{\rtf1\ansi\ansicpg1251\deff0\nouicompat\deflang1049{\fonttbl{\f0\fnil\fcharset0 Calibri;}{\f1\fnil\fcharset204 Calibri;}}
{\colortbl ;\red0\green77\blue187;\red155\green0\blue211;\red0\green0\blue0;}
{\*\generator Riched20 10.0.22000}\viewkind4\uc1 
\pard\sl276\slmult1\cf1\f0\fs20\lang9 FindVisibleElementByIdAsync\cf0\par
\cf2\f1\lang1049\'ce\'ef\'e8\'f1\'e0\'ed\'e8\'e5\cf0 : \'ec\'e5\'f2\'ee\'e4 \'e2\'fb\'ef\'ee\'eb\'ed\'ff\'e5\'f2 \'ef\'ee\'e8\'f1\'ea\f0\lang1033  \f1\lang1049\'e2\'e8\'e7\'f3\'e0\'eb\'fc\'ed\'ee \'ee\'f2\'ee\'e1\'f0\'e0\'e6\'e0\'e5\'ec\'ee\'e3\'ee \'fd\'eb\'e5\'ec\'e5\'ed\'f2\'e0 \'f1 \'ee\'e6\'e8\'e4\'e0\'ed\'e8\'e5\'ec \'e2 \'f1\'e5\'ea\'f3\'ed\'e4\'e0\'f5 \'e8 \'e2\'ee\'e7\'e2\'f0\'e0\'f9\'e0\'e5\'f2 \'eb\'ee\'e3\'e8\'f7\'e5\'f1\'ea\'e8\'e9 \'f0\'e5\'e7\'f3\'eb\'fc\'f2\'e0\'f2 \f0\lang1033 true \f1\lang1049\'e8\'eb\'e8 \f0\lang1033 false\f1\lang1049\par
\cf2\'d1\'e8\'ed\'f2\'e0\'ea\'f1\'e8\'f1\cf0 : FindVisibleElementByIdAsync(string id, int sec)\par
\cf3\par
\cf2\'cf\'f0\'e8\'ec\'e5\'f0\cf3 :\par
\cf0\f0\lang1033 bool result = await tester.FindVisibleElementByIdAsync(""MyElement"", 5);\f1\lang1049\par
}",

@"{\rtf1\ansi\ansicpg1251\deff0\nouicompat\deflang1049{\fonttbl{\f0\fnil\fcharset0 Calibri;}{\f1\fnil\fcharset204 Calibri;}}
{\colortbl ;\red0\green77\blue187;\red155\green0\blue211;\red0\green0\blue0;}
{\*\generator Riched20 10.0.22000}\viewkind4\uc1 
\pard\sl276\slmult1\cf1\f0\fs20\lang9 FindVisibleElementByNameAsync\cf0\par
\cf2\f1\lang1049\'ce\'ef\'e8\'f1\'e0\'ed\'e8\'e5\cf0 : \'ec\'e5\'f2\'ee\'e4 \'e2\'fb\'ef\'ee\'eb\'ed\'ff\'e5\'f2 \'ef\'ee\'e8\'f1\'ea\f0\lang1033  \f1\lang1049\'e2\'e8\'e7\'f3\'e0\'eb\'fc\'ed\'ee \'ee\'f2\'ee\'e1\'f0\'e0\'e6\'e0\'e5\'ec\'ee\'e3\'ee \'fd\'eb\'e5\'ec\'e5\'ed\'f2\'e0 \'f1 \'ee\'e6\'e8\'e4\'e0\'ed\'e8\'e5\'ec \'e2 \'f1\'e5\'ea\'f3\'ed\'e4\'e0\'f5 \'e8 \'e2\'ee\'e7\'e2\'f0\'e0\'f9\'e0\'e5\'f2 \'eb\'ee\'e3\'e8\'f7\'e5\'f1\'ea\'e8\'e9 \'f0\'e5\'e7\'f3\'eb\'fc\'f2\'e0\'f2 \f0\lang1033 true \f1\lang1049\'e8\'eb\'e8 \f0\lang1033 false\f1\lang1049\par
\cf2\'d1\'e8\'ed\'f2\'e0\'ea\'f1\'e8\'f1\cf0 : FindVisibleElementByNameAsync(string name, int index, int sec)\par
\cf3\par
\cf2\'cf\'f0\'e8\'ec\'e5\'f0\cf3 :\par
\cf0\f0\lang1033 bool result = await tester.FindVisibleElementByNameAsync(""MyElement"", 0, 5);\f1\lang1049\par
}",

@"{\rtf1\ansi\ansicpg1251\deff0\nouicompat\deflang1049{\fonttbl{\f0\fnil\fcharset0 Calibri;}{\f1\fnil\fcharset204 Calibri;}}
{\colortbl ;\red0\green77\blue187;\red155\green0\blue211;\red0\green0\blue0;}
{\*\generator Riched20 10.0.22000}\viewkind4\uc1 
\pard\sl276\slmult1\cf1\f0\fs20\lang9 FindVisibleElementByTagAsync\cf0\par
\cf2\f1\lang1049\'ce\'ef\'e8\'f1\'e0\'ed\'e8\'e5\cf0 : \'ec\'e5\'f2\'ee\'e4 \'e2\'fb\'ef\'ee\'eb\'ed\'ff\'e5\'f2 \'ef\'ee\'e8\'f1\'ea\f0\lang1033  \f1\lang1049\'e2\'e8\'e7\'f3\'e0\'eb\'fc\'ed\'ee \'ee\'f2\'ee\'e1\'f0\'e0\'e6\'e0\'e5\'ec\'ee\'e3\'ee \'fd\'eb\'e5\'ec\'e5\'ed\'f2\'e0 \'f1 \'ee\'e6\'e8\'e4\'e0\'ed\'e8\'e5\'ec \'e2 \'f1\'e5\'ea\'f3\'ed\'e4\'e0\'f5 \'e8 \'e2\'ee\'e7\'e2\'f0\'e0\'f9\'e0\'e5\'f2 \'eb\'ee\'e3\'e8\'f7\'e5\'f1\'ea\'e8\'e9 \'f0\'e5\'e7\'f3\'eb\'fc\'f2\'e0\'f2 \f0\lang1033 true \f1\lang1049\'e8\'eb\'e8 \f0\lang1033 false\f1\lang1049\par
\cf2\'d1\'e8\'ed\'f2\'e0\'ea\'f1\'e8\'f1\cf0 : FindVisibleElementByTagAsync(string tag, int index, int sec)\par
\cf3\par
\cf2\'cf\'f0\'e8\'ec\'e5\'f0\cf3 :\par
\cf0\f0\lang1033 bool result = await tester.FindVisibleElementByTagAsync(""h1"", 0, 5);\f1\lang1049\par
}",

@"{\rtf1\ansi\ansicpg1251\deff0\nouicompat\deflang1049{\fonttbl{\f0\fnil\fcharset0 Calibri;}{\f1\fnil\fcharset204 Calibri;}}
{\colortbl ;\red0\green77\blue187;\red155\green0\blue211;\red0\green0\blue0;}
{\*\generator Riched20 10.0.22000}\viewkind4\uc1 
\pard\sl276\slmult1\cf1\f0\fs20\lang9 GetAttributeFromElementByClassAsync\cf0\par
\cf2\f1\lang1049\'ce\'ef\'e8\'f1\'e0\'ed\'e8\'e5\cf0 : \'ec\'e5\'f2\'ee\'e4 \'e2\'ee\'e7\'e2\'f0\'e0\'f9\'e0\'e5\'f2 \'f1\'f2\'f0\'ee\'f7\'ed\'ee\'e5 \'e7\'ed\'e0\'f7\'e5\'ed\'e8\'e5 \'e8\'e7 \'f3\'ea\'e0\'e7\'e0\'ed\'ed\'ee\'e3\'ee \'e0\'f2\'f0\'e8\'e1\'f3\'f2\'e0 \'e2 \'e2\'fb\'e1\'f0\'e0\'ed\'ed\'ee\'ec \'fd\'eb\'e5\'ec\'e5\'ed\'f2\'e5\par
\cf2\'d1\'e8\'ed\'f2\'e0\'ea\'f1\'e8\'f1\cf0 : GetAttributeFromElementByClassAsync(string _class, int index, string attribute)\par
\cf3\par
\cf2\'cf\'f0\'e8\'ec\'e5\'f0\cf3 :\par
\cf0\f0\lang1033 string value = await tester.GetAttributeFromElementByClassAsync(""my-element"", 0, ""href"");\f1\lang1049\par
}",

@"{\rtf1\ansi\ansicpg1251\deff0\nouicompat\deflang1049{\fonttbl{\f0\fnil\fcharset0 Calibri;}{\f1\fnil\fcharset204 Calibri;}}
{\colortbl ;\red0\green77\blue187;\red155\green0\blue211;\red0\green0\blue0;}
{\*\generator Riched20 10.0.22000}\viewkind4\uc1 
\pard\sl276\slmult1\cf1\f0\fs20\lang9 GetAttributeFromElementAsync\cf0\par
\cf2\f1\lang1049\'ce\'ef\'e8\'f1\'e0\'ed\'e8\'e5\cf0 : \'ec\'e5\'f2\'ee\'e4 \'e2\'ee\'e7\'e2\'f0\'e0\'f9\'e0\'e5\'f2 \'f1\'f2\'f0\'ee\'f7\'ed\'ee\'e5 \'e7\'ed\'e0\'f7\'e5\'ed\'e8\'e5 \'e8\'e7 \'f3\'ea\'e0\'e7\'e0\'ed\'ed\'ee\'e3\'ee \'e0\'f2\'f0\'e8\'e1\'f3\'f2\'e0 \'e2 \'e2\'fb\'e1\'f0\'e0\'ed\'ed\'ee\'ec \'fd\'eb\'e5\'ec\'e5\'ed\'f2\'e5\par
\cf2\'d1\'e8\'ed\'f2\'e0\'ea\'f1\'e8\'f1\cf0 : \f0\lang1033 GetAttributeFromElementAsync(string by, string locator, string attribute)\f1\lang1049\par
\cf3\par
\cf2\'cf\'f0\'e8\'ec\'e5\'f0\cf3 :\par
\cf0\f0\lang1033 string value = await tester.GetAttributeFromElementAsync(Tester.BY_CSS, ""input"", ""name"");\par
\par
string value = await tester.GetAttributeFromElementAsync(Tester.BY_XPATH, ""//input"", ""name"");\par
    }",

@"{\rtf1\ansi\ansicpg1251\deff0\nouicompat\deflang1049{\fonttbl{\f0\fnil\fcharset0 Calibri;}{\f1\fnil\fcharset204 Calibri;}}
{\colortbl ;\red0\green77\blue187;\red155\green0\blue211;\red0\green0\blue0;}
{\*\generator Riched20 10.0.22000}\viewkind4\uc1 
\pard\sl276\slmult1\cf1\f0\fs20\lang9 GetAttributeFromElementByIdAsync\cf0\par
\cf2\f1\lang1049\'ce\'ef\'e8\'f1\'e0\'ed\'e8\'e5\cf0 : \'ec\'e5\'f2\'ee\'e4 \'e2\'ee\'e7\'e2\'f0\'e0\'f9\'e0\'e5\'f2 \'f1\'f2\'f0\'ee\'f7\'ed\'ee\'e5 \'e7\'ed\'e0\'f7\'e5\'ed\'e8\'e5 \'e8\'e7 \'f3\'ea\'e0\'e7\'e0\'ed\'ed\'ee\'e3\'ee \'e0\'f2\'f0\'e8\'e1\'f3\'f2\'e0 \'e2 \'e2\'fb\'e1\'f0\'e0\'ed\'ed\'ee\'ec \'fd\'eb\'e5\'ec\'e5\'ed\'f2\'e5\par
\cf2\'d1\'e8\'ed\'f2\'e0\'ea\'f1\'e8\'f1\cf0 : GetAttributeFromElementByIdAsync(string id, string attribute)\par
\cf3\par
\cf2\'cf\'f0\'e8\'ec\'e5\'f0\cf3 :\par
\cf0\f0\lang1033 string value = await tester.GetAttributeFromElementByIdAsync(""MyElement"", ""href"");\f1\lang1049\par
}",

@"{\rtf1\ansi\ansicpg1251\deff0\nouicompat\deflang1049{\fonttbl{\f0\fnil\fcharset0 Calibri;}{\f1\fnil\fcharset204 Calibri;}}
{\colortbl ;\red0\green77\blue187;\red155\green0\blue211;\red0\green0\blue0;}
{\*\generator Riched20 10.0.22000}\viewkind4\uc1 
\pard\sl276\slmult1\cf1\f0\fs20\lang9 GetAttributeFromElementByNameAsync\cf0\par
\cf2\f1\lang1049\'ce\'ef\'e8\'f1\'e0\'ed\'e8\'e5\cf0 : \'ec\'e5\'f2\'ee\'e4 \'e2\'ee\'e7\'e2\'f0\'e0\'f9\'e0\'e5\'f2 \'f1\'f2\'f0\'ee\'f7\'ed\'ee\'e5 \'e7\'ed\'e0\'f7\'e5\'ed\'e8\'e5 \'e8\'e7 \'f3\'ea\'e0\'e7\'e0\'ed\'ed\'ee\'e3\'ee \'e0\'f2\'f0\'e8\'e1\'f3\'f2\'e0 \'e2 \'e2\'fb\'e1\'f0\'e0\'ed\'ed\'ee\'ec \'fd\'eb\'e5\'ec\'e5\'ed\'f2\'e5\par
\cf2\'d1\'e8\'ed\'f2\'e0\'ea\'f1\'e8\'f1\cf0 : GetAttributeFromElementByNameAsync(string name, int index, string attribute)\par
\cf3\par
\cf2\'cf\'f0\'e8\'ec\'e5\'f0\cf3 :\par
\cf0\f0\lang1033 string value = await tester.GetAttributeFromElementByNameAsync(""MyElement"", 0, ""href"");\f1\lang1049\par
}",

@"{\rtf1\ansi\ansicpg1251\deff0\nouicompat\deflang1049{\fonttbl{\f0\fnil\fcharset0 Calibri;}{\f1\fnil\fcharset204 Calibri;}}
{\colortbl ;\red0\green77\blue187;\red155\green0\blue211;\red0\green0\blue0;}
{\*\generator Riched20 10.0.22000}\viewkind4\uc1 
\pard\sl276\slmult1\cf1\f0\fs20\lang9 GetAttributeFromElementByTagAsync\cf0\par
\cf2\f1\lang1049\'ce\'ef\'e8\'f1\'e0\'ed\'e8\'e5\cf0 : \'ec\'e5\'f2\'ee\'e4 \'e2\'ee\'e7\'e2\'f0\'e0\'f9\'e0\'e5\'f2 \'f1\'f2\'f0\'ee\'f7\'ed\'ee\'e5 \'e7\'ed\'e0\'f7\'e5\'ed\'e8\'e5 \'e8\'e7 \'f3\'ea\'e0\'e7\'e0\'ed\'ed\'ee\'e3\'ee \'e0\'f2\'f0\'e8\'e1\'f3\'f2\'e0 \'e2 \'e2\'fb\'e1\'f0\'e0\'ed\'ed\'ee\'ec \'fd\'eb\'e5\'ec\'e5\'ed\'f2\'e5\par
\cf2\'d1\'e8\'ed\'f2\'e0\'ea\'f1\'e8\'f1\cf0 : GetAttributeFromElementByTagAsync(string tag, int index, string attribute)\par
\cf3\par
\cf2\'cf\'f0\'e8\'ec\'e5\'f0\cf3 :\par
\cf0\f0\lang1033 string value = await tester.GetAttributeFromElementByTagAsync(""a"", 0, ""href"");\f1\lang1049\par
}",

@"{\rtf1\ansi\ansicpg1251\deff0\nouicompat\deflang1049{\fonttbl{\f0\fnil\fcharset0 Calibri;}{\f1\fnil\fcharset204 Calibri;}}
{\colortbl ;\red0\green77\blue187;\red155\green0\blue211;\red0\green0\blue0;}
{\*\generator Riched20 10.0.22000}\viewkind4\uc1 
\pard\sl276\slmult1\cf1\f0\fs20\lang9 GetAttributeFromElementsAsync\cf0\par
\cf2\f1\lang1049\'ce\'ef\'e8\'f1\'e0\'ed\'e8\'e5\cf0 : \'ec\'e5\'f2\'ee\'e4 \'e2\'ee\'e7\'e2\'f0\'e0\'f9\'e0\'e5\'f2 \'f1\'ef\'e8\'f1\'ee\'ea \'e7\'ed\'e0\'f7\'e5\'ed\'e8\'e9 \'f3\'ea\'e0\'e7\'e0\'ed\'ed\'ee\'e3\'ee \'e0\'f2\'f0\'e8\'e1\'f3\'f2\'e0 \'e8\'e7 \'ec\'ed\'ee\'e6\'e5\'f1\'f2\'e2\'e0 \'fd\'eb\'e5\'ec\'e5\'ed\'f2\'ee\'e2\par
\cf2\'d1\'e8\'ed\'f2\'e0\'ea\'f1\'e8\'f1\cf0 : \f0\lang1033 GetAttributeFromElementsAsync(string by, string locator, string attribute)\f1\lang1049\par
\cf3\par
\cf2\'cf\'f0\'e8\'ec\'e5\'f0\cf3 :\par
\cf0\f0\lang1033 List<string> values = await tester.GetAttributeFromElementsAsync(Tester.BY_CSS, ""input"", ""name"");\par
if(values != null) \par
\{\par
\tab foreach (string attr in values)\par
\tab\tab tester.ConsoleMsg(attr);\par
\}\par
\par
List<string> values = await tester.GetAttributeFromElementsAsync(Tester.BY_XPATH, ""//input"", ""name"");\par
if(values != null)\par
\{\par
\tab foreach (string attr in values)\par
\tab\tab tester.ConsoleMsg(attr);\par
\}\par
}",

@"{\rtf1\ansi\ansicpg1251\deff0\nouicompat\deflang1049{\fonttbl{\f0\fnil\fcharset0 Calibri;}{\f1\fnil\fcharset204 Calibri;}}
{\colortbl ;\red0\green77\blue187;\red155\green0\blue211;\red0\green0\blue0;}
{\*\generator Riched20 10.0.22000}\viewkind4\uc1 
\pard\sl276\slmult1\cf1\f0\fs20\lang9 GetCountElementsByClassAsync\cf0\par
\cf2\f1\lang1049\'ce\'ef\'e8\'f1\'e0\'ed\'e8\'e5\cf0 : \'ec\'e5\'f2\'ee\'e4 \'e2\'ee\'e7\'e2\'f0\'e0\'f9\'e0\'e5\'f2\f0\lang1033  \f1\lang1049\'ea\'ee\'eb\'e8\'f7\'e5\'f1\'f2\'e2\'ee \'ed\'e0\'e9\'e4\'e5\'ed\'ed\'fb\'f5 \'fd\'eb\'e5\'ec\'e5\'ed\'f2\'ee\'e2\par
\cf2\'d1\'e8\'ed\'f2\'e0\'ea\'f1\'e8\'f1\cf0 : GetCountElementsByClassAsync(string _class)\par
\cf3\par
\cf2\'cf\'f0\'e8\'ec\'e5\'f0\cf3 :\par
\cf0\f0\lang1033 int count = await tester.GetCountElementsByClassAsync(""my-element"");\f1\lang1049\par
}",

@"{\rtf1\ansi\ansicpg1251\deff0\nouicompat\deflang1049{\fonttbl{\f0\fnil\fcharset0 Calibri;}{\f1\fnil\fcharset204 Calibri;}}
{\colortbl ;\red0\green77\blue187;\red155\green0\blue211;\red0\green0\blue0;}
{\*\generator Riched20 10.0.22000}\viewkind4\uc1 
\pard\sl276\slmult1\cf1\f0\fs20\lang9 GetCountElementsAsync\cf0\par
\cf2\f1\lang1049\'ce\'ef\'e8\'f1\'e0\'ed\'e8\'e5\cf0 : \'ec\'e5\'f2\'ee\'e4 \'e2\'ee\'e7\'e2\'f0\'e0\'f9\'e0\'e5\'f2 \'ea\'ee\'eb\'e8\'f7\'e5\'f1\'f2\'e2\'ee \'ed\'e0\'e9\'e4\'e5\'ed\'ed\'fb\'f5 \'fd\'eb\'e5\'ec\'e5\'ed\'f2\'ee\'e2\par
\cf2\'d1\'e8\'ed\'f2\'e0\'ea\'f1\'e8\'f1\cf0 : \f0\lang1033 GetCountElementsAsync(string by, string locator)\f1\lang1049\par
\cf3\par
\cf2\'cf\'f0\'e8\'ec\'e5\'f0\cf3 :\par
\cf0\f0\lang1033 int count = await tester.GetCountElementsAsync(Tester.BY_CSS, ""input"");\par
\par
int count = await tester.GetCountElementsAsync(Tester.BY_XPATH, ""//input"");\par
    }",

@"{\rtf1\ansi\ansicpg1251\deff0\nouicompat\deflang1049{\fonttbl{\f0\fnil\fcharset0 Calibri;}{\f1\fnil\fcharset204 Calibri;}}
{\colortbl ;\red0\green77\blue187;\red155\green0\blue211;\red0\green0\blue0;}
{\*\generator Riched20 10.0.22000}\viewkind4\uc1 
\pard\sl276\slmult1\cf1\f0\fs20\lang9 GetCountElementsByNameAsync\cf0\par
\cf2\f1\lang1049\'ce\'ef\'e8\'f1\'e0\'ed\'e8\'e5\cf0 : \'ec\'e5\'f2\'ee\'e4 \'e2\'ee\'e7\'e2\'f0\'e0\'f9\'e0\'e5\'f2\f0\lang1033  \f1\lang1049\'ea\'ee\'eb\'e8\'f7\'e5\'f1\'f2\'e2\'ee \'ed\'e0\'e9\'e4\'e5\'ed\'ed\'fb\'f5 \'fd\'eb\'e5\'ec\'e5\'ed\'f2\'ee\'e2\par
\cf2\'d1\'e8\'ed\'f2\'e0\'ea\'f1\'e8\'f1\cf0 : GetCountElementsByNameAsync(string name)\par
\cf3\par
\cf2\'cf\'f0\'e8\'ec\'e5\'f0\cf3 :\par
\cf0\f0\lang1033 int count = await tester.GetCountElementsByNameAsync(""MyElement"");\f1\lang1049\par
}",

@"{\rtf1\ansi\ansicpg1251\deff0\nouicompat\deflang1049{\fonttbl{\f0\fnil\fcharset0 Calibri;}{\f1\fnil\fcharset204 Calibri;}}
{\colortbl ;\red0\green77\blue187;\red155\green0\blue211;\red0\green0\blue0;}
{\*\generator Riched20 10.0.22000}\viewkind4\uc1 
\pard\sl276\slmult1\cf1\f0\fs20\lang9 GetCountElementsByTagAsync\cf0\par
\cf2\f1\lang1049\'ce\'ef\'e8\'f1\'e0\'ed\'e8\'e5\cf0 : \'ec\'e5\'f2\'ee\'e4 \'e2\'ee\'e7\'e2\'f0\'e0\'f9\'e0\'e5\'f2\f0\lang1033  \f1\lang1049\'ea\'ee\'eb\'e8\'f7\'e5\'f1\'f2\'e2\'ee \'ed\'e0\'e9\'e4\'e5\'ed\'ed\'fb\'f5 \'fd\'eb\'e5\'ec\'e5\'ed\'f2\'ee\'e2\par
\cf2\'d1\'e8\'ed\'f2\'e0\'ea\'f1\'e8\'f1\cf0 : GetCountElementsByTagAsync(string tag)\par
\cf3\par
\cf2\'cf\'f0\'e8\'ec\'e5\'f0\cf3 :\par
\cf0\f0\lang1033 int count = await tester.GetCountElementsByTagAsync(""h2"");\f1\lang1049\par
}",

@"{\rtf1\ansi\ansicpg1251\deff0\nouicompat\deflang1049{\fonttbl{\f0\fnil\fcharset0 Calibri;}{\f1\fnil\fcharset204 Calibri;}}
{\colortbl ;\red0\green77\blue187;\red155\green0\blue211;\red0\green0\blue0;}
{\*\generator Riched20 10.0.22000}\viewkind4\uc1 
\pard\sl276\slmult1\cf1\f0\fs20\lang9 GetElementAsync\cf0\par
\cf2\f1\lang1049\'ce\'ef\'e8\'f1\'e0\'ed\'e8\'e5\cf0 : \'ec\'e5\'f2\'ee\'e4 \'e2\'ee\'e7\'e2\'f0\'e0\'f9\'e0\'e5\'f2 \'fd\'eb\'e5\'ec\'e5\'ed\'f2 \'e2 \'e2\'e8\'e4\'e5 \'ee\'e1\'fa\'e5\'ea\'f2 \'ea\'eb\'e0\'f1\'f1 \'ea\'ee\'f2\'ee\'f0\'ee\'e3\'ee HTMLElement\par
\cf2\'d1\'e8\'ed\'f2\'e0\'ea\'f1\'e8\'f1\cf0 : \f0\lang1033 GetElementAsync(string by, string locator)\f1\lang1049\par
\cf3\par
\cf2\'cf\'f0\'e8\'ec\'e5\'f0\cf3 :\par
\cf0\f0\lang1033 HTMLElement element = await tester.GetElementAsync(Tester.BY_CSS, ""#auth #buttonLogin"");\par
await element.ClickAsync();\par
\par
HTMLElement element = await tester.GetElementAsync(Tester.BY_XPATH, ""//div[@id='auth']//input[@id='buttonLogin']"");\par
await element.ClickAsync();\par
\par
HTMLElement element = await tester.GetElementAsync(Tester.BY_XPATH, ""//*[@id='MyFile']"");\par
tester.ConsoleMsg(""ID: "" + element.Id);\par
\par
HTMLElement element = await tester.GetElementAsync(Tester.BY_XPATH, ""//h1"");\par
string text = await element.GetTextAsync();\par
tester.ConsoleMsg(text);\par
await element.SetTextAsync(""TEST"");\par
\par
HTMLElement element = await tester.GetElementAsync(Tester.BY_XPATH, ""//*[@id='MyInput']"");\par
await element.SetValueAsync(""\f1\lang1049\'d2\'e5\'f1\'f2\'e8\'f0\'ee\'e2\'e0\'ed\'e8\'e5"");\par
string value = await element.GetValueAsync();\par
tester.ConsoleMsg(value);\par
\par
HTMLElement element = await tester.GetElementAsync(Tester.BY_XPATH, ""//h1"");\par
await element.SetAttributeAsync(""class"", ""my-class"");\par
string attrClass = await element.GetAttributeAsync(""class"");\par
tester.ConsoleMsg(attrClass);\par
\par
HTMLElement element = await tester.GetElementAsync(Tester.BY_XPATH, ""//h1"");\par
string html = await element.GetHtmlAsync();\par
tester.ConsoleMsg(html);\par
\par
HTMLElement element = await tester.GetElementAsync(Tester.BY_XPATH, ""//h1"");\par
await element.SetHtmlAsync(""<div>\'dd\'f2\'ee \'f2\'e5\'f1\'f2</div>"");\par
\par
HTMLElement element = await tester.GetElementAsync(Tester.BY_XPATH, ""/html/body/footer"");\par
await element.ScrollToAsync();\par
\par
HTMLElement element = await tester.GetElementAsync(Tester.BY_XPATH, ""//h1"");\par
await element.WaitVisibleAsync(2);\par
await element.WaitNotVisibleAsync(2);\f0\lang1033\par
    }
 ",

@"{\rtf1\ansi\ansicpg1251\deff0\nouicompat\deflang1049{\fonttbl{\f0\fnil\fcharset0 Calibri;}{\f1\fnil\fcharset204 Calibri;}}
{\colortbl ;\red0\green77\blue187;\red155\green0\blue211;\red0\green0\blue0;}
{\*\generator Riched20 10.0.22000}\viewkind4\uc1 
\pard\sl276\slmult1\cf1\f0\fs20\lang9 GetTextFromElementByClassAsync\cf0\par
\cf2\f1\lang1049\'ce\'ef\'e8\'f1\'e0\'ed\'e8\'e5\cf0 : \'ec\'e5\'f2\'ee\'e4 \'e2\'ee\'e7\'e2\'f0\'e0\'f9\'e0\'e5\'f2\f0\lang1033  \f1\lang1049\'f2\'e5\'ea\'f1\'f2 \'e8\'e7 \'f3\'ea\'e0\'e7\'e0\'ed\'ed\'ee\'e3\'ee \'fd\'eb\'e5\'ec\'e5\'ed\'f2\'e0\par
\cf2\'d1\'e8\'ed\'f2\'e0\'ea\'f1\'e8\'f1\cf0 : GetTextFromElementByClassAsync(string _class, int index)\par
\cf3\par
\cf2\'cf\'f0\'e8\'ec\'e5\'f0\cf3 :\par
\cf0\f0\lang1033 string text = await tester.GetTextFromElementByClassAsync(""my-element"", 0);\f1\lang1049\par
}",

@"{\rtf1\ansi\ansicpg1251\deff0\nouicompat\deflang1049{\fonttbl{\f0\fnil\fcharset0 Calibri;}{\f1\fnil\fcharset204 Calibri;}}
{\colortbl ;\red0\green77\blue187;\red155\green0\blue211;\red0\green0\blue0;}
{\*\generator Riched20 10.0.22000}\viewkind4\uc1 
\pard\sl276\slmult1\cf1\f0\fs20\lang9 GetTextFromElementAsync\cf0\par
\cf2\f1\lang1049\'ce\'ef\'e8\'f1\'e0\'ed\'e8\'e5\cf0 : \'ec\'e5\'f2\'ee\'e4 \'e2\'ee\'e7\'e2\'f0\'e0\'f9\'e0\'e5\'f2 \'f2\'e5\'ea\'f1\'f2 \'e8\'e7 \'f3\'ea\'e0\'e7\'e0\'ed\'ed\'ee\'e3\'ee \'fd\'eb\'e5\'ec\'e5\'ed\'f2\'e0\par
\cf2\'d1\'e8\'ed\'f2\'e0\'ea\'f1\'e8\'f1\cf0 : \f0\lang1033 GetTextFromElementAsync(string by, string locator)\f1\lang1049\par
\cf3\par
\cf2\'cf\'f0\'e8\'ec\'e5\'f0\cf3 :\par
\cf0\f0\lang1033 string text = await tester.GetTextFromElementAsync(Tester.BY_CSS, ""#auth > h2"");\par
\par
string text = await tester.GetTextFromElementAsync(Tester.BY_XPATH, ""//div[@id='auth']//h2"");\par
    }",

@"{\rtf1\ansi\ansicpg1251\deff0\nouicompat\deflang1049{\fonttbl{\f0\fnil\fcharset0 Calibri;}{\f1\fnil\fcharset204 Calibri;}}
{\colortbl ;\red0\green77\blue187;\red155\green0\blue211;\red0\green0\blue0;}
{\*\generator Riched20 10.0.22000}\viewkind4\uc1 
\pard\sl276\slmult1\cf1\f0\fs20\lang9 GetTextFromElementByIdAsync\cf0\par
\cf2\f1\lang1049\'ce\'ef\'e8\'f1\'e0\'ed\'e8\'e5\cf0 : \'ec\'e5\'f2\'ee\'e4 \'e2\'ee\'e7\'e2\'f0\'e0\'f9\'e0\'e5\'f2\f0\lang1033  \f1\lang1049\'f2\'e5\'ea\'f1\'f2 \'e8\'e7 \'f3\'ea\'e0\'e7\'e0\'ed\'ed\'ee\'e3\'ee \'fd\'eb\'e5\'ec\'e5\'ed\'f2\'e0\par
\cf2\'d1\'e8\'ed\'f2\'e0\'ea\'f1\'e8\'f1\cf0 : GetTextFromElementByIdAsync(string id)\par
\cf3\par
\cf2\'cf\'f0\'e8\'ec\'e5\'f0\cf3 :\par
\cf0\f0\lang1033 string text = await tester.GetTextFromElementByIdAsync(""MyElement"");\f1\lang1049\par
}",

@"{\rtf1\ansi\ansicpg1251\deff0\nouicompat\deflang1049{\fonttbl{\f0\fnil\fcharset0 Calibri;}{\f1\fnil\fcharset204 Calibri;}}
{\colortbl ;\red0\green77\blue187;\red155\green0\blue211;\red0\green0\blue0;}
{\*\generator Riched20 10.0.22000}\viewkind4\uc1 
\pard\sl276\slmult1\cf1\f0\fs20\lang9 GetTextFromElementByNameAsync\cf0\par
\cf2\f1\lang1049\'ce\'ef\'e8\'f1\'e0\'ed\'e8\'e5\cf0 : \'ec\'e5\'f2\'ee\'e4 \'e2\'ee\'e7\'e2\'f0\'e0\'f9\'e0\'e5\'f2\f0\lang1033  \f1\lang1049\'f2\'e5\'ea\'f1\'f2 \'e8\'e7 \'f3\'ea\'e0\'e7\'e0\'ed\'ed\'ee\'e3\'ee \'fd\'eb\'e5\'ec\'e5\'ed\'f2\'e0\par
\cf2\'d1\'e8\'ed\'f2\'e0\'ea\'f1\'e8\'f1\cf0 : GetTextFromElementByNameAsync(string name, int index)\par
\cf3\par
\cf2\'cf\'f0\'e8\'ec\'e5\'f0\cf3 :\par
\cf0\f0\lang1033 string text = await tester.GetTextFromElementByNameAsync(""MyElement"", 0);\f1\lang1049\par
}",

@"{\rtf1\ansi\ansicpg1251\deff0\nouicompat\deflang1049{\fonttbl{\f0\fnil\fcharset0 Calibri;}{\f1\fnil\fcharset204 Calibri;}}
{\colortbl ;\red0\green77\blue187;\red155\green0\blue211;\red0\green0\blue0;}
{\*\generator Riched20 10.0.22000}\viewkind4\uc1 
\pard\sl276\slmult1\cf1\f0\fs20\lang9 GetTextFromElementByTagAsync\cf0\par
\cf2\f1\lang1049\'ce\'ef\'e8\'f1\'e0\'ed\'e8\'e5\cf0 : \'ec\'e5\'f2\'ee\'e4 \'e2\'ee\'e7\'e2\'f0\'e0\'f9\'e0\'e5\'f2\f0\lang1033  \f1\lang1049\'f2\'e5\'ea\'f1\'f2 \'e8\'e7 \'f3\'ea\'e0\'e7\'e0\'ed\'ed\'ee\'e3\'ee \'fd\'eb\'e5\'ec\'e5\'ed\'f2\'e0\par
\cf2\'d1\'e8\'ed\'f2\'e0\'ea\'f1\'e8\'f1\cf0 : GetTextFromElementByTagAsync(string tag, int index)\par
\cf3\par
\cf2\'cf\'f0\'e8\'ec\'e5\'f0\cf3 :\par
\cf0\f0\lang1033 string text = await tester.GetTextFromElementByTagAsync(""h1"", 0);\f1\lang1049\par
}",

@"{\rtf1\ansi\ansicpg1251\deff0\nouicompat\deflang1049{\fonttbl{\f0\fnil\fcharset0 Calibri;}{\f1\fnil\fcharset204 Calibri;}}
{\colortbl ;\red0\green77\blue187;\red155\green0\blue211;\red0\green0\blue0;}
{\*\generator Riched20 10.0.22000}\viewkind4\uc1 
\pard\sl276\slmult1\cf1\f0\fs20\lang9 GetTitleAsync\cf0\par
\cf2\f1\lang1049\'ce\'ef\'e8\'f1\'e0\'ed\'e8\'e5\cf0 : \'ec\'e5\'f2\'ee\'e4 \'e2\'ee\'e7\'e2\'f0\'e0\'f9\'e0\'e5\'f2\f0\lang1033  \f1\lang1049\'e7\'e0\'e3\'ee\'eb\'ee\'e2\'ee\'ea \'f1\'f2\'f0\'e0\'ed\'e8\'f6\'fb\par
\cf2\'d1\'e8\'ed\'f2\'e0\'ea\'f1\'e8\'f1\cf0 : GetTitleAsync()\par
\cf3\par
\cf2\'cf\'f0\'e8\'ec\'e5\'f0\cf3 :\par
\cf0\f0\lang1033 string text = await tester.GetTitleAsync();\f1\lang1049\par
}",

@"{\rtf1\ansi\ansicpg1251\deff0\nouicompat\deflang1049{\fonttbl{\f0\fnil\fcharset0 Calibri;}{\f1\fnil\fcharset204 Calibri;}}
{\colortbl ;\red0\green77\blue187;\red155\green0\blue211;\red0\green0\blue0;}
{\*\generator Riched20 10.0.22000}\viewkind4\uc1 
\pard\sl276\slmult1\cf1\f0\fs20\lang9 GetUrlAsync\cf0\par
\cf2\f1\lang1049\'ce\'ef\'e8\'f1\'e0\'ed\'e8\'e5\cf0 : \'ec\'e5\'f2\'ee\'e4 \'e2\'ee\'e7\'e2\'f0\'e0\'f9\'e0\'e5\'f2\f0\lang1033  \f1\lang1049\'f2\'e5\'ea\'f3\'f9\'e8\'e9 \f0\lang1033 URL\f1\lang1049\par
\cf2\'d1\'e8\'ed\'f2\'e0\'ea\'f1\'e8\'f1\cf0 : GetUrlAsync()\par
\cf3\par
\cf2\'cf\'f0\'e8\'ec\'e5\'f0\cf3 :\par
\cf0\f0\lang1033 string text = await tester.GetUrlAsync();\f1\lang1049\par
}",

@"{\rtf1\ansi\ansicpg1251\deff0\nouicompat\deflang1049{\fonttbl{\f0\fnil\fcharset0 Calibri;}{\f1\fnil\fcharset204 Calibri;}}
{\colortbl ;\red0\green77\blue187;\red155\green0\blue211;\red0\green0\blue0;}
{\*\generator Riched20 10.0.22000}\viewkind4\uc1 
\pard\sl276\slmult1\cf1\f0\fs20\lang9 GetValueFromElementByClassAsync\cf0\par
\cf2\f1\lang1049\'ce\'ef\'e8\'f1\'e0\'ed\'e8\'e5\cf0 : \'ec\'e5\'f2\'ee\'e4 \'e2\'ee\'e7\'e2\'f0\'e0\'f9\'e0\'e5\'f2\f0\lang1033  \f1\lang1049\'e7\'ed\'e0\'f7\'e5\'ed\'e8\'e5 \'e8\'e7 \'f3\'ea\'e0\'e7\'e0\'ed\'ed\'ee\'e3\'ee \'fd\'eb\'e5\'ec\'e5\'ed\'f2\'e0\par
\cf2\'d1\'e8\'ed\'f2\'e0\'ea\'f1\'e8\'f1\cf0 : GetValueFromElementByClassAsync(string _class, int index)\par
\cf3\par
\cf2\'cf\'f0\'e8\'ec\'e5\'f0\cf3 :\par
\cf0\f0\lang1033 string value = await tester.GetValueFromElementByClassAsync(""my-element"", 0);\f1\lang1049\par
}",

@"{\rtf1\ansi\ansicpg1251\deff0\nouicompat\deflang1049{\fonttbl{\f0\fnil\fcharset0 Calibri;}{\f1\fnil\fcharset204 Calibri;}}
{\colortbl ;\red0\green77\blue187;\red155\green0\blue211;\red0\green0\blue0;}
{\*\generator Riched20 10.0.22000}\viewkind4\uc1 
\pard\sl276\slmult1\cf1\f0\fs20\lang9 GetValueFromElementAsync\cf0\par
\cf2\f1\lang1049\'ce\'ef\'e8\'f1\'e0\'ed\'e8\'e5\cf0 : \'ec\'e5\'f2\'ee\'e4 \'e2\'ee\'e7\'e2\'f0\'e0\'f9\'e0\'e5\'f2 \'e7\'ed\'e0\'f7\'e5\'ed\'e8\'e5 \'e8\'e7 \'f3\'ea\'e0\'e7\'e0\'ed\'ed\'ee\'e3\'ee \'fd\'eb\'e5\'ec\'e5\'ed\'f2\'e0\par
\cf2\'d1\'e8\'ed\'f2\'e0\'ea\'f1\'e8\'f1\cf0 : \f0\lang1033 GetValueFromElementAsync(string by, string locator)\f1\lang1049\par
\cf3\par
\cf2\'cf\'f0\'e8\'ec\'e5\'f0\cf3 :\par
\cf0\f0\lang1033 string value = await tester.GetValueFromElementAsync(Tester.BY_CSS, ""input[id='login']"");\par
\par
string value = await tester.GetValueFromElementAsync(Tester.BY_XPATH, ""//input[@id='login']"");\par
    }",

@"{\rtf1\ansi\ansicpg1251\deff0\nouicompat\deflang1049{\fonttbl{\f0\fnil\fcharset0 Calibri;}{\f1\fnil\fcharset204 Calibri;}}
{\colortbl ;\red0\green77\blue187;\red155\green0\blue211;\red0\green0\blue0;}
{\*\generator Riched20 10.0.22000}\viewkind4\uc1 
\pard\sl276\slmult1\cf1\f0\fs20\lang9 GetValueFromElementByIdAsync\cf0\par
\cf2\f1\lang1049\'ce\'ef\'e8\'f1\'e0\'ed\'e8\'e5\cf0 : \'ec\'e5\'f2\'ee\'e4 \'e2\'ee\'e7\'e2\'f0\'e0\'f9\'e0\'e5\'f2\f0\lang1033  \f1\lang1049\'e7\'ed\'e0\'f7\'e5\'ed\'e8\'e5 \'e8\'e7 \'f3\'ea\'e0\'e7\'e0\'ed\'ed\'ee\'e3\'ee \'fd\'eb\'e5\'ec\'e5\'ed\'f2\'e0\par
\cf2\'d1\'e8\'ed\'f2\'e0\'ea\'f1\'e8\'f1\cf0 : GetValueFromElementByIdAsync(string id)\par
\cf3\par
\cf2\'cf\'f0\'e8\'ec\'e5\'f0\cf3 :\par
\cf0\f0\lang1033 string value = await tester.GetValueFromElementByIdAsync(""MyElement"");\f1\lang1049\par
}",

@"{\rtf1\ansi\ansicpg1251\deff0\nouicompat\deflang1049{\fonttbl{\f0\fnil\fcharset0 Calibri;}{\f1\fnil\fcharset204 Calibri;}}
{\colortbl ;\red0\green77\blue187;\red155\green0\blue211;\red0\green0\blue0;}
{\*\generator Riched20 10.0.22000}\viewkind4\uc1 
\pard\sl276\slmult1\cf1\f0\fs20\lang9 GetValueFromElementByNameAsync\cf0\par
\cf2\f1\lang1049\'ce\'ef\'e8\'f1\'e0\'ed\'e8\'e5\cf0 : \'ec\'e5\'f2\'ee\'e4 \'e2\'ee\'e7\'e2\'f0\'e0\'f9\'e0\'e5\'f2\f0\lang1033  \f1\lang1049\'e7\'ed\'e0\'f7\'e5\'ed\'e8\'e5 \'e8\'e7 \'f3\'ea\'e0\'e7\'e0\'ed\'ed\'ee\'e3\'ee \'fd\'eb\'e5\'ec\'e5\'ed\'f2\'e0\par
\cf2\'d1\'e8\'ed\'f2\'e0\'ea\'f1\'e8\'f1\cf0 : GetValueFromElementByNameAsync(string name, int index)\par
\cf3\par
\cf2\'cf\'f0\'e8\'ec\'e5\'f0\cf3 :\par
\cf0\f0\lang1033 string value = await tester.GetValueFromElementByNameAsync(""MyElement"", 0);\f1\lang1049\par
}",

@"{\rtf1\ansi\ansicpg1251\deff0\nouicompat\deflang1049{\fonttbl{\f0\fnil\fcharset0 Calibri;}{\f1\fnil\fcharset204 Calibri;}}
{\colortbl ;\red0\green77\blue187;\red155\green0\blue211;\red0\green0\blue0;}
{\*\generator Riched20 10.0.22000}\viewkind4\uc1 
\pard\sl276\slmult1\cf1\f0\fs20\lang9 GetValueFromElementByTagAsync\cf0\par
\cf2\f1\lang1049\'ce\'ef\'e8\'f1\'e0\'ed\'e8\'e5\cf0 : \'ec\'e5\'f2\'ee\'e4 \'e2\'ee\'e7\'e2\'f0\'e0\'f9\'e0\'e5\'f2\f0\lang1033  \f1\lang1049\'e7\'ed\'e0\'f7\'e5\'ed\'e8\'e5 \'e8\'e7 \'f3\'ea\'e0\'e7\'e0\'ed\'ed\'ee\'e3\'ee \'fd\'eb\'e5\'ec\'e5\'ed\'f2\'e0\par
\cf2\'d1\'e8\'ed\'f2\'e0\'ea\'f1\'e8\'f1\cf0 : GetValueFromElementByTagAsync(string tag, int index)\par
\cf3\par
\cf2\'cf\'f0\'e8\'ec\'e5\'f0\cf3 :\par
\cf0\f0\lang1033 string value = await tester.GetValueFromElementByTagAsync(""input"", 0);\f1\lang1049\par
}",

@"{\rtf1\ansi\ansicpg1251\deff0\nouicompat\deflang1049{\fonttbl{\f0\fnil\fcharset0 Calibri;}{\f1\fnil\fcharset204 Calibri;}}
{\colortbl ;\red0\green77\blue187;\red155\green0\blue211;\red0\green0\blue0;}
{\*\generator Riched20 10.0.22000}\viewkind4\uc1 
\pard\sl276\slmult1\cf1\f0\fs20\lang9 GoToUrlAsync\cf0\par
\cf2\f1\lang1049\'ce\'ef\'e8\'f1\'e0\'ed\'e8\'e5\cf0 : \'ec\'e5\'f2\'ee\'e4 \'e2\'fb\'ef\'ee\'eb\'ed\'ff\'e5\'f2 \'e7\'e0\'e3\'f0\'f3\'e7\'ea\'f3 \'e2\'e5\'e1 \'f1\'e0\'e9\'f2\'e0 \'ef\'ee \'f3\'ea\'e0\'e7\'e0\'ed\'ee\'ec\'f3 \f0\lang1033 URL \f1\lang1049\'f1 \'f3\'ea\'e0\'e7\'e0\'ed\'ed\'fb\'ec \'ee\'e6\'e8\'e4\'e0\'ed\'e8\'e5\'ec \'e2 \'f1\'e5\'ea\'f3\'ed\'e4\'e0\'f5\par
\cf2\'d1\'e8\'ed\'f2\'e0\'ea\'f1\'e8\'f1\cf0 : GoToUrlAsync(string url, int sec, bool abortLoadAfterTime = false)\par
\cf3\par
\cf2\'cf\'f0\'e8\'ec\'e5\'f0\cf3 :\par
\cf0\f0\lang1033 await tester.GoToUrlAsync(@""https://www.google.com/"", 25);\f1\lang1049\par
}",

@"{\rtf1\ansi\ansicpg1251\deff0\nouicompat\deflang1049{\fonttbl{\f0\fnil\fcharset0 Calibri;}{\f1\fnil\fcharset204 Calibri;}}
{\colortbl ;\red0\green77\blue187;\red155\green0\blue211;\red0\green0\blue0;}
{\*\generator Riched20 10.0.22000}\viewkind4\uc1 
\pard\sl276\slmult1\cf1\f0\fs20\lang9 ScrollToElementAsync\cf0\par
\cf2\f1\lang1049\'ce\'ef\'e8\'f1\'e0\'ed\'e8\'e5\cf0 : \'ec\'e5\'f2\'ee\'e4 \'e2\'fb\'ef\'ee\'eb\'ed\'ff\'e5\'f2 \'ef\'f0\'ee\'ea\'f0\'f3\'f2\'ea\'f3 \'ea \'f3\'ea\'e0\'e7\'e0\'ed\'ed\'ee\'ec\'f3 \'fd\'eb\'e5\'ec\'e5\'ed\'f2\'f3 (\'ef\'e0\'f0\'e0\'ec\'e5\'f2\'f0 behaviorSmooth \'ee\'ef\'f0\'e5\'e4\'e5\'eb\'ff\'e5\'f2 \'ef\'eb\'e0\'e2\'ed\'ee\'f1\'f2\'fc \'ef\'f0\'ee\'ea\'f0\'f3\'f2\'ea\'e8)\par
\cf2\'d1\'e8\'ed\'f2\'e0\'ea\'f1\'e8\'f1\cf0 : \f0\lang1033 ScrollToElementAsync(string by, string locator, bool behaviorSmooth = false)\f1\lang1049\par
\cf3\par
\cf2\'cf\'f0\'e8\'ec\'e5\'f0\cf3 :\par
\cf0\f0\lang1033 await tester.ScrollToElementAsync(Tester.BY_CSS, ""body > footer"", true);\par
\par
await tester.ScrollToElementAsync(Tester.BY_XPATH, ""/html/body/footer"", true);\par
    }",

@"{\rtf1\ansi\ansicpg1251\deff0\nouicompat\deflang1049{\fonttbl{\f0\fnil\fcharset0 Calibri;}{\f1\fnil\fcharset204 Calibri;}}
{\colortbl ;\red0\green77\blue187;\red155\green0\blue211;\red0\green0\blue0;}
{\*\generator Riched20 10.0.22000}\viewkind4\uc1 
\pard\sl276\slmult1\cf1\f0\fs20\lang9 SetAttributeInElementByClassAsync\cf0\par
\cf2\f1\lang1049\'ce\'ef\'e8\'f1\'e0\'ed\'e8\'e5\cf0 : \'ec\'e5\'f2\'ee\'e4 \'e2\'f1\'f2\'e0\'e2\'eb\'ff\'e5\'f2 \'e0\'f2\'f0\'e8\'e1\'f3\'f2 \'f1\'ee \'e7\'ed\'e0\'f7\'e5\'ed\'e8\'e5\'ec \'e2 \'f3\'ea\'e0\'e7\'e0\'ed\'ed\'fb\'e9 \'fd\'eb\'e5\'ec\'e5\'ed\'f2 \par
\cf2\'d1\'e8\'ed\'f2\'e0\'ea\'f1\'e8\'f1\cf0 : SetAttributeInElementByClassAsync(string _class, int index, string attribute, string value)\par
\cf3\par
\cf2\'cf\'f0\'e8\'ec\'e5\'f0\cf3 :\par
\cf0\f0\lang1033 await tester.SetAttributeInElementByClassAsync(""my-element"", 0, ""value"", ""\f1\lang1049\'e7\'ed\'e0\'f7\'e5\'ed\'e8\'e5\f0\lang1033 "");\f1\lang1049\par
}",

@"{\rtf1\ansi\ansicpg1251\deff0\nouicompat\deflang1049{\fonttbl{\f0\fnil\fcharset0 Calibri;}{\f1\fnil\fcharset204 Calibri;}}
{\colortbl ;\red0\green77\blue187;\red155\green0\blue211;\red0\green0\blue0;}
{\*\generator Riched20 10.0.22000}\viewkind4\uc1 
\pard\sl276\slmult1\cf1\f0\fs20\lang9 SetAttributeInElementAsync\cf0\par
\cf2\f1\lang1049\'ce\'ef\'e8\'f1\'e0\'ed\'e8\'e5\cf0 : \'ec\'e5\'f2\'ee\'e4 \'e2\'f1\'f2\'e0\'e2\'eb\'ff\'e5\'f2 \'e0\'f2\'f0\'e8\'e1\'f3\'f2 \'f1\'ee \'e7\'ed\'e0\'f7\'e5\'ed\'e8\'e5\'ec \'e2 \'f3\'ea\'e0\'e7\'e0\'ed\'ed\'fb\'e9 \'fd\'eb\'e5\'ec\'e5\'ed\'f2\par
\cf2\'d1\'e8\'ed\'f2\'e0\'ea\'f1\'e8\'f1\cf0 : \f0\lang1033 SetAttributeInElementAsync(string by, string locator, string attribute, string value)\f1\lang1049\par
\cf3\par
\cf2\'cf\'f0\'e8\'ec\'e5\'f0\cf3 :\par
\cf0\f0\lang1033 await tester.SetAttributeInElementAsync(Tester.BY_CSS, ""#auth > h2"", ""name"", ""test"");\par
\par
await tester.SetAttributeInElementAsync(Tester.BY_XPATH, ""//div[@id='auth']//h2"", ""name"", ""test"");\par
    }",

@"{\rtf1\ansi\ansicpg1251\deff0\nouicompat\deflang1049{\fonttbl{\f0\fnil\fcharset0 Calibri;}{\f1\fnil\fcharset204 Calibri;}}
{\colortbl ;\red0\green77\blue187;\red155\green0\blue211;\red0\green0\blue0;}
{\*\generator Riched20 10.0.22000}\viewkind4\uc1 
\pard\sl276\slmult1\cf1\f0\fs20\lang9 SetAttributeInElementByIdAsync\cf0\par
\cf2\f1\lang1049\'ce\'ef\'e8\'f1\'e0\'ed\'e8\'e5\cf0 : \'ec\'e5\'f2\'ee\'e4 \'e2\'f1\'f2\'e0\'e2\'eb\'ff\'e5\'f2 \'e0\'f2\'f0\'e8\'e1\'f3\'f2 \'f1\'ee \'e7\'ed\'e0\'f7\'e5\'ed\'e8\'e5\'ec \'e2 \'f3\'ea\'e0\'e7\'e0\'ed\'ed\'fb\'e9 \'fd\'eb\'e5\'ec\'e5\'ed\'f2 \par
\cf2\'d1\'e8\'ed\'f2\'e0\'ea\'f1\'e8\'f1\cf0 : SetAttributeInElementByIdAsync(string id, string attribute, string value)\par
\cf3\par
\cf2\'cf\'f0\'e8\'ec\'e5\'f0\cf3 :\par
\cf0\f0\lang1033 await tester.SetAttributeInElementByIdAsync(""MyElement"", ""value"",  ""\f1\lang1049\'e7\'ed\'e0\'f7\'e5\'ed\'e8\'e5\f0\lang1033 "");\f1\lang1049\par
}",

@"{\rtf1\ansi\ansicpg1251\deff0\nouicompat\deflang1049{\fonttbl{\f0\fnil\fcharset0 Calibri;}{\f1\fnil\fcharset204 Calibri;}}
{\colortbl ;\red0\green77\blue187;\red155\green0\blue211;\red0\green0\blue0;}
{\*\generator Riched20 10.0.22000}\viewkind4\uc1 
\pard\sl276\slmult1\cf1\f0\fs20\lang9 SetAttributeInElementByNameAsync\cf0\par
\cf2\f1\lang1049\'ce\'ef\'e8\'f1\'e0\'ed\'e8\'e5\cf0 : \'ec\'e5\'f2\'ee\'e4 \'e2\'f1\'f2\'e0\'e2\'eb\'ff\'e5\'f2 \'e0\'f2\'f0\'e8\'e1\'f3\'f2 \'f1\'ee \'e7\'ed\'e0\'f7\'e5\'ed\'e8\'e5\'ec \'e2 \'f3\'ea\'e0\'e7\'e0\'ed\'ed\'fb\'e9 \'fd\'eb\'e5\'ec\'e5\'ed\'f2 \par
\cf2\'d1\'e8\'ed\'f2\'e0\'ea\'f1\'e8\'f1\cf0 : SetAttributeInElementByNameAsync(string name, int index, string attribute, string value)\par
\cf3\par
\cf2\'cf\'f0\'e8\'ec\'e5\'f0\cf3 :\par
\cf0\f0\lang1033 await tester.SetAttributeInElementByNameAsync(""MyElement"", 0, ""value"",  ""\f1\lang1049\'e7\'ed\'e0\'f7\'e5\'ed\'e8\'e5\f0\lang1033 "");\f1\lang1049\par
}",

@"{\rtf1\ansi\ansicpg1251\deff0\nouicompat\deflang1049{\fonttbl{\f0\fnil\fcharset0 Calibri;}{\f1\fnil\fcharset204 Calibri;}}
{\colortbl ;\red0\green77\blue187;\red155\green0\blue211;\red0\green0\blue0;}
{\*\generator Riched20 10.0.22000}\viewkind4\uc1 
\pard\sl276\slmult1\cf1\f0\fs20\lang9 SetAttributeInElementByTagAsync\cf0\par
\cf2\f1\lang1049\'ce\'ef\'e8\'f1\'e0\'ed\'e8\'e5\cf0 : \'ec\'e5\'f2\'ee\'e4 \'e2\'f1\'f2\'e0\'e2\'eb\'ff\'e5\'f2 \'e0\'f2\'f0\'e8\'e1\'f3\'f2 \'f1\'ee \'e7\'ed\'e0\'f7\'e5\'ed\'e8\'e5\'ec \'e2 \'f3\'ea\'e0\'e7\'e0\'ed\'ed\'fb\'e9 \'fd\'eb\'e5\'ec\'e5\'ed\'f2 \par
\cf2\'d1\'e8\'ed\'f2\'e0\'ea\'f1\'e8\'f1\cf0 : SetAttributeInElementByTagAsync(string tag, int index, string attribute, string value)\par
\cf3\par
\cf2\'cf\'f0\'e8\'ec\'e5\'f0\cf3 :\par
\cf0\f0\lang1033 await tester.SetAttributeInElementByTagAsync(""input"", 0, ""value"",  ""\f1\lang1049\'e7\'ed\'e0\'f7\'e5\'ed\'e8\'e5\f0\lang1033 "");\f1\lang1049\par
}",

@"{\rtf1\ansi\ansicpg1251\deff0\nouicompat\deflang1049{\fonttbl{\f0\fnil\fcharset0 Calibri;}{\f1\fnil\fcharset204 Calibri;}}
{\colortbl ;\red0\green77\blue187;\red155\green0\blue211;\red0\green0\blue0;}
{\*\generator Riched20 10.0.22000}\viewkind4\uc1 
\pard\sl276\slmult1\cf1\f0\fs20\lang9 SetTextInElementByClassAsync\cf0\par
\cf2\f1\lang1049\'ce\'ef\'e8\'f1\'e0\'ed\'e8\'e5\cf0 : \'ec\'e5\'f2\'ee\'e4 \'e2\'f1\'f2\'e0\'e2\'eb\'ff\'e5\'f2 \'f2\'e5\'ea\'f1\'f2 \'e2 \'f3\'ea\'e0\'e7\'e0\'ed\'ed\'fb\'e9 \'fd\'eb\'e5\'ec\'e5\'ed\'f2 \par
\cf2\'d1\'e8\'ed\'f2\'e0\'ea\'f1\'e8\'f1\cf0 : SetTextInElementByClassAsync(string _class, int index, string text)\par
\cf3\par
\cf2\'cf\'f0\'e8\'ec\'e5\'f0\cf3 :\par
\cf0\f0\lang1033 await tester.SetTextInElementByClassAsync(""my-element"", 0, ""\f1\lang1049\'f2\'e5\'ea\'f1\'f2\f0\lang1033 "");\f1\lang1049\par
}",

@"{\rtf1\ansi\ansicpg1251\deff0\nouicompat\deflang1049{\fonttbl{\f0\fnil\fcharset0 Calibri;}{\f1\fnil\fcharset204 Calibri;}}
{\colortbl ;\red0\green77\blue187;\red155\green0\blue211;\red0\green0\blue0;}
{\*\generator Riched20 10.0.22000}\viewkind4\uc1 
\pard\sl276\slmult1\cf1\f0\fs20\lang9 SetTextInElementAsync\cf0\par
\cf2\f1\lang1049\'ce\'ef\'e8\'f1\'e0\'ed\'e8\'e5\cf0 : \'ec\'e5\'f2\'ee\'e4 \'e2\'f1\'f2\'e0\'e2\'eb\'ff\'e5\'f2 \'f2\'e5\'ea\'f1\'f2 \'e2 \'f3\'ea\'e0\'e7\'e0\'ed\'ed\'fb\'e9 \'fd\'eb\'e5\'ec\'e5\'ed\'f2\par
\cf2\'d1\'e8\'ed\'f2\'e0\'ea\'f1\'e8\'f1\cf0 : \f0\lang1033 SetTextInElementAsync(string by, string locator, string text)\f1\lang1049\par
\cf3\par
\cf2\'cf\'f0\'e8\'ec\'e5\'f0\cf3 :\par
\cf0\f0\lang1033 await tester.SetTextInElementAsync(Tester.BY_CSS, ""#auth > h2"", ""\f1\lang1049\'d2\'e5\'f1\'f2\'ee\'e2\'fb\'e9 \'e7\'e0\'e3\'ee\'eb\'ee\'e2\'ee\'ea"");\par
\par
await tester.SetTextInElementAsync(Tester.BY_XPATH, ""//div[@id='auth']//h2"", ""\'d2\'e5\'f1\'f2\'ee\'e2\'fb\'e9 \'e7\'e0\'e3\'ee\'eb\'ee\'e2\'ee\'ea"");\f0\lang1033\par
    }",

@"{\rtf1\ansi\ansicpg1251\deff0\nouicompat\deflang1049{\fonttbl{\f0\fnil\fcharset0 Calibri;}{\f1\fnil\fcharset204 Calibri;}}
{\colortbl ;\red0\green77\blue187;\red155\green0\blue211;\red0\green0\blue0;}
{\*\generator Riched20 10.0.22000}\viewkind4\uc1 
\pard\sl276\slmult1\cf1\f0\fs20\lang9 SetTextInElementByIdAsync\cf0\par
\cf2\f1\lang1049\'ce\'ef\'e8\'f1\'e0\'ed\'e8\'e5\cf0 : \'ec\'e5\'f2\'ee\'e4 \'e2\'f1\'f2\'e0\'e2\'eb\'ff\'e5\'f2 \'f2\'e5\'ea\'f1\'f2 \'e2 \'f3\'ea\'e0\'e7\'e0\'ed\'ed\'fb\'e9 \'fd\'eb\'e5\'ec\'e5\'ed\'f2 \par
\cf2\'d1\'e8\'ed\'f2\'e0\'ea\'f1\'e8\'f1\cf0 : SetTextInElementByIdAsync(string id, string text)\par
\cf3\par
\cf2\'cf\'f0\'e8\'ec\'e5\'f0\cf3 :\par
\cf0\f0\lang1033 await tester.SetTextInElementByIdAsync(""MyElement"", ""\f1\lang1049\'f2\'e5\'ea\'f1\'f2\f0\lang1033 "");\f1\lang1049\par
}",

@"{\rtf1\ansi\ansicpg1251\deff0\nouicompat\deflang1049{\fonttbl{\f0\fnil\fcharset0 Calibri;}{\f1\fnil\fcharset204 Calibri;}}
{\colortbl ;\red0\green77\blue187;\red155\green0\blue211;\red0\green0\blue0;}
{\*\generator Riched20 10.0.22000}\viewkind4\uc1 
\pard\sl276\slmult1\cf1\f0\fs20\lang9 SetTextInElementByNameAsync\cf0\par
\cf2\f1\lang1049\'ce\'ef\'e8\'f1\'e0\'ed\'e8\'e5\cf0 : \'ec\'e5\'f2\'ee\'e4 \'e2\'f1\'f2\'e0\'e2\'eb\'ff\'e5\'f2 \'f2\'e5\'ea\'f1\'f2 \'e2 \'f3\'ea\'e0\'e7\'e0\'ed\'ed\'fb\'e9 \'fd\'eb\'e5\'ec\'e5\'ed\'f2 \par
\cf2\'d1\'e8\'ed\'f2\'e0\'ea\'f1\'e8\'f1\cf0 : SetTextInElementByNameAsync(string name, int index, string text)\par
\cf3\par
\cf2\'cf\'f0\'e8\'ec\'e5\'f0\cf3 :\par
\cf0\f0\lang1033 await tester.SetTextInElementByNameAsync(""MyElement"", 0, ""\f1\lang1049\'f2\'e5\'ea\'f1\'f2\f0\lang1033 "");\f1\lang1049\par
}",

@"{\rtf1\ansi\ansicpg1251\deff0\nouicompat\deflang1049{\fonttbl{\f0\fnil\fcharset0 Calibri;}{\f1\fnil\fcharset204 Calibri;}}
{\colortbl ;\red0\green77\blue187;\red155\green0\blue211;\red0\green0\blue0;}
{\*\generator Riched20 10.0.22000}\viewkind4\uc1 
\pard\sl276\slmult1\cf1\f0\fs20\lang9 SetTextInElementByTagAsync\cf0\par
\cf2\f1\lang1049\'ce\'ef\'e8\'f1\'e0\'ed\'e8\'e5\cf0 : \'ec\'e5\'f2\'ee\'e4 \'e2\'f1\'f2\'e0\'e2\'eb\'ff\'e5\'f2 \'f2\'e5\'ea\'f1\'f2 \'e2 \'f3\'ea\'e0\'e7\'e0\'ed\'ed\'fb\'e9 \'fd\'eb\'e5\'ec\'e5\'ed\'f2 \par
\cf2\'d1\'e8\'ed\'f2\'e0\'ea\'f1\'e8\'f1\cf0 : SetTextInElementByTagAsync(string tag, int index, string text)\par
\cf3\par
\cf2\'cf\'f0\'e8\'ec\'e5\'f0\cf3 :\par
\cf0\f0\lang1033 await tester.SetTextInElementByTagAsync(""h1"", 0, ""\f1\lang1049\'f2\'e5\'ea\'f1\'f2\f0\lang1033 "");\f1\lang1049\par
}",

@"{\rtf1\ansi\ansicpg1251\deff0\nouicompat\deflang1049{\fonttbl{\f0\fnil\fcharset0 Calibri;}{\f1\fnil\fcharset204 Calibri;}}
{\colortbl ;\red0\green77\blue187;\red155\green0\blue211;\red0\green0\blue0;}
{\*\generator Riched20 10.0.22000}\viewkind4\uc1 
\pard\sl276\slmult1\cf1\f0\fs20\lang9 SetValueInElementByClassAsync\cf0\par
\cf2\f1\lang1049\'ce\'ef\'e8\'f1\'e0\'ed\'e8\'e5\cf0 : \'ec\'e5\'f2\'ee\'e4 \'e2\'f1\'f2\'e0\'e2\'eb\'ff\'e5\'f2 \'f2\'e5\'ea\'f1\'f2 \'e2 \'f3\'ea\'e0\'e7\'e0\'ed\'ed\'fb\'e9 \'fd\'eb\'e5\'ec\'e5\'ed\'f2 \par
\cf2\'d1\'e8\'ed\'f2\'e0\'ea\'f1\'e8\'f1\cf0 : SetValueInElementByClassAsync(string _class, int index, string value)\par
\cf3\par
\cf2\'cf\'f0\'e8\'ec\'e5\'f0\cf3 :\par
\cf0\f0\lang1033 await tester.SetValueInElementByClassAsync(""my-element"", 0, ""\f1\lang1049\'e7\'ed\'e0\'f7\'e5\'ed\'e8\'e5\f0\lang1033 "");\f1\lang1049\par
}",

@"{\rtf1\ansi\ansicpg1251\deff0\nouicompat\deflang1049{\fonttbl{\f0\fnil\fcharset0 Calibri;}{\f1\fnil\fcharset204 Calibri;}}
{\colortbl ;\red0\green77\blue187;\red155\green0\blue211;\red0\green0\blue0;}
{\*\generator Riched20 10.0.22000}\viewkind4\uc1 
\pard\sl276\slmult1\cf1\f0\fs20\lang9 SetValueInElementAsync\cf0\par
\cf2\f1\lang1049\'ce\'ef\'e8\'f1\'e0\'ed\'e8\'e5\cf0 : \'ec\'e5\'f2\'ee\'e4 \'e2\'f1\'f2\'e0\'e2\'eb\'ff\'e5\'f2 \'e7\'ed\'e0\'f7\'e5\'ed\'e8\'e5 \'e2 \'f3\'ea\'e0\'e7\'e0\'ed\'ed\'fb\'e9 \'fd\'eb\'e5\'ec\'e5\'ed\'f2\par
\cf2\'d1\'e8\'ed\'f2\'e0\'ea\'f1\'e8\'f1\cf0 : \f0\lang1033 SetValueInElementAsync(string by, string locator, string value)\f1\lang1049\par
\cf3\par
\cf2\'cf\'f0\'e8\'ec\'e5\'f0\cf3 :\par
\cf0\f0\lang1033 await tester.SetValueInElementAsync(Tester.BY_CSS, ""input[id='login']"", ""admin"");\par
await tester.SetValueInElementAsync(Tester.BY_CSS, ""input[id='pass']"", ""0000"");\par
\par
await tester.SetValueInElementAsync(Tester.BY_XPATH, ""//input[@id='login']"", ""admin"");\par
await tester.SetValueInElementAsync(Tester.BY_XPATH, ""//input[@id='pass']"", ""0000"");\par
    }",

@"{\rtf1\ansi\ansicpg1251\deff0\nouicompat\deflang1049{\fonttbl{\f0\fnil\fcharset0 Calibri;}{\f1\fnil\fcharset204 Calibri;}}
{\colortbl ;\red0\green77\blue187;\red155\green0\blue211;\red0\green0\blue0;}
{\*\generator Riched20 10.0.22000}\viewkind4\uc1 
\pard\sl276\slmult1\cf1\f0\fs20\lang9 SetValueInElementByIdAsync\cf0\par
\cf2\f1\lang1049\'ce\'ef\'e8\'f1\'e0\'ed\'e8\'e5\cf0 : \'ec\'e5\'f2\'ee\'e4 \'e2\'f1\'f2\'e0\'e2\'eb\'ff\'e5\'f2 \'f2\'e5\'ea\'f1\'f2 \'e2 \'f3\'ea\'e0\'e7\'e0\'ed\'ed\'fb\'e9 \'fd\'eb\'e5\'ec\'e5\'ed\'f2 \par
\cf2\'d1\'e8\'ed\'f2\'e0\'ea\'f1\'e8\'f1\cf0 : SetValueInElementByIdAsync(string id, string value)\par
\cf3\par
\cf2\'cf\'f0\'e8\'ec\'e5\'f0\cf3 :\par
\cf0\f0\lang1033 await tester.SetValueInElementByIdAsync(""MyElement"", ""\f1\lang1049\'e7\'ed\'e0\'f7\'e5\'ed\'e8\'e5\f0\lang1033 "");\f1\lang1049\par
}",

@"{\rtf1\ansi\ansicpg1251\deff0\nouicompat\deflang1049{\fonttbl{\f0\fnil\fcharset0 Calibri;}{\f1\fnil\fcharset204 Calibri;}}
{\colortbl ;\red0\green77\blue187;\red155\green0\blue211;\red0\green0\blue0;}
{\*\generator Riched20 10.0.22000}\viewkind4\uc1 
\pard\sl276\slmult1\cf1\f0\fs20\lang9 SetValueInElementByNameAsync\cf0\par
\cf2\f1\lang1049\'ce\'ef\'e8\'f1\'e0\'ed\'e8\'e5\cf0 : \'ec\'e5\'f2\'ee\'e4 \'e2\'f1\'f2\'e0\'e2\'eb\'ff\'e5\'f2 \'f2\'e5\'ea\'f1\'f2 \'e2 \'f3\'ea\'e0\'e7\'e0\'ed\'ed\'fb\'e9 \'fd\'eb\'e5\'ec\'e5\'ed\'f2 \par
\cf2\'d1\'e8\'ed\'f2\'e0\'ea\'f1\'e8\'f1\cf0 : SetValueInElementByNameAsync(string name, int index, string value)\par
\cf3\par
\cf2\'cf\'f0\'e8\'ec\'e5\'f0\cf3 :\par
\cf0\f0\lang1033 await tester.SetValueInElementByNameAsync(""MyElement"", 0, ""\f1\lang1049\'e7\'ed\'e0\'f7\'e5\'ed\'e8\'e5\f0\lang1033 "");\f1\lang1049\par
}",

@"{\rtf1\ansi\ansicpg1251\deff0\nouicompat\deflang1049{\fonttbl{\f0\fnil\fcharset0 Calibri;}{\f1\fnil\fcharset204 Calibri;}}
{\colortbl ;\red0\green77\blue187;\red155\green0\blue211;\red0\green0\blue0;}
{\*\generator Riched20 10.0.22000}\viewkind4\uc1 
\pard\sl276\slmult1\cf1\f0\fs20\lang9 SetValueInElementByTagAsync\cf0\par
\cf2\f1\lang1049\'ce\'ef\'e8\'f1\'e0\'ed\'e8\'e5\cf0 : \'ec\'e5\'f2\'ee\'e4 \'e2\'f1\'f2\'e0\'e2\'eb\'ff\'e5\'f2 \'f2\'e5\'ea\'f1\'f2 \'e2 \'f3\'ea\'e0\'e7\'e0\'ed\'ed\'fb\'e9 \'fd\'eb\'e5\'ec\'e5\'ed\'f2 \par
\cf2\'d1\'e8\'ed\'f2\'e0\'ea\'f1\'e8\'f1\cf0 : SetValueInElementByTagAsync(string tag, int index, string value)\par
\cf3\par
\cf2\'cf\'f0\'e8\'ec\'e5\'f0\cf3 :\par
\cf0\f0\lang1033 await tester.SetValueInElementByTagAsync(""input"", 0, ""\f1\lang1049\'e7\'ed\'e0\'f7\'e5\'ed\'e8\'e5\f0\lang1033 "");\f1\lang1049\par
}",

@"{\rtf1\ansi\ansicpg1251\deff0\nouicompat\deflang1049{\fonttbl{\f0\fnil\fcharset0 Calibri;}{\f1\fnil\fcharset204 Calibri;}}
{\colortbl ;\red0\green77\blue187;\red155\green0\blue211;\red0\green0\blue0;}
{\*\generator Riched20 10.0.22000}\viewkind4\uc1 
\pard\sl276\slmult1\cf1\f0\fs20\lang9 WaitAsync\cf0\par
\cf2\f1\lang1049\'ce\'ef\'e8\'f1\'e0\'ed\'e8\'e5\cf0 : \'ec\'e5\'f2\'ee\'e4 \'e2\'fb\'ef\'ee\'eb\'ed\'ff\'e5\'f2 \'e2\'f0\'e5\'ec\'e5\'ed\'ed\'f3\'fe \'ee\'f1\'f2\'e0\'ed\'ee\'e2\'ea\'f3 \'e2\'fb\'ef\'ee\'eb\'ed\'e5\'ed\'e8\'ff \'f2\'e5\'f1\'f2\'e0 \'ed\'e0 \'f3\'ea\'e0\'e7\'e0\'ed\'ed\'ee\'e5 \'ea\'ee\'eb\'e8\'f7\'e5\'f1\'f2\'e2\'ee \'f1\'e5\'ea\'f3\'ed\'e4\par
\cf2\'d1\'e8\'ed\'f2\'e0\'ea\'f1\'e8\'f1\cf0 : WaitAsync(int sec)\par
\cf3\par
\cf2\'cf\'f0\'e8\'ec\'e5\'f0\cf3 :\par
\cf0\f0\lang1033 await tester.WaitAsync(\f1\lang1049 5\f0\lang1033 );\f1\lang1049\par
}",

@"{\rtf1\ansi\ansicpg1251\deff0\nouicompat\deflang1049{\fonttbl{\f0\fnil\fcharset0 Calibri;}{\f1\fnil\fcharset204 Calibri;}}
{\colortbl ;\red0\green77\blue187;\red155\green0\blue211;\red0\green0\blue0;}
{\*\generator Riched20 10.0.22000}\viewkind4\uc1 
\pard\sl276\slmult1\cf1\f0\fs20\lang9 WaitNotVisibleElementByClassAsync\cf0\par
\cf2\f1\lang1049\'ce\'ef\'e8\'f1\'e0\'ed\'e8\'e5\cf0 : \'ec\'e5\'f2\'ee\'e4 \'e2\'fb\'ef\'ee\'eb\'ed\'ff\'e5\'f2 \'e2\'f0\'e5\'ec\'e5\'ed\'ed\'f3\'fe \'ee\'f1\'f2\'e0\'ed\'ee\'e2\'ea\'f3 \'e2\'fb\'ef\'ee\'eb\'ed\'e5\'ed\'e8\'ff \'f2\'e5\'f1\'f2\'e0 \'ed\'e0 \'f3\'ea\'e0\'e7\'e0\'ed\'ed\'ee\'e5 \'ea\'ee\'eb\'e8\'f7\'e5\'f1\'f2\'e2\'ee \'f1\'e5\'ea\'f3\'ed\'e4\f0\lang1033  \f1\lang1049\'e8 \'e6\'e4\'e5\'f2 \'ea\'ee\'e3\'e4\'e0  \'e7\'e0\'ef\'f0\'e0\'f8\'e8\'e2\'e0\'e5\'ec\'fb\'e9 \'fd\'eb\'e5\'ec\'e5\'ed\'f2 \'ef\'e5\'f0\'e5\'f1\'f2\'e0\'ed\'e5\'f2 \'ee\'f2\'ee\'e1\'f0\'e0\'e6\'e0\'e5\'f2\'fc\'f1\'ff \par
\cf2\'d1\'e8\'ed\'f2\'e0\'ea\'f1\'e8\'f1\cf0 : WaitNotVisibleElementByClassAsync(string _class, int index, int sec)\par
\cf3\par
\cf2\'cf\'f0\'e8\'ec\'e5\'f0\cf3 :\par
\cf0\f0\lang1033 await tester.WaitNotVisibleElementByClassAsync(""my-element"", 0, 25);\f1\lang1049\par
}",

@"{\rtf1\ansi\ansicpg1251\deff0\nouicompat\deflang1049{\fonttbl{\f0\fnil\fcharset0 Calibri;}{\f1\fnil\fcharset204 Calibri;}}
{\colortbl ;\red0\green77\blue187;\red155\green0\blue211;\red0\green0\blue0;}
{\*\generator Riched20 10.0.22000}\viewkind4\uc1 
\pard\sl276\slmult1\cf1\f0\fs20\lang9 WaitNotVisibleElementAsync\cf0\par
\cf2\f1\lang1049\'ce\'ef\'e8\'f1\'e0\'ed\'e8\'e5\cf0 : \'ec\'e5\'f2\'ee\'e4 \'e2\'fb\'ef\'ee\'eb\'ed\'ff\'e5\'f2 \'e2\'f0\'e5\'ec\'e5\'ed\'ed\'f3\'fe \'ee\'f1\'f2\'e0\'ed\'ee\'e2\'ea\'f3 \'e2\'fb\'ef\'ee\'eb\'ed\'e5\'ed\'e8\'ff \'f2\'e5\'f1\'f2\'e0 \'ed\'e0 \'f3\'ea\'e0\'e7\'e0\'ed\'ed\'ee\'e5 \'ea\'ee\'eb\'e8\'f7\'e5\'f1\'f2\'e2\'ee \'f1\'e5\'ea\'f3\'ed\'e4 \'e8 \'e6\'e4\'e5\'f2 \'ea\'ee\'e3\'e4\'e0  \'e7\'e0\'ef\'f0\'e0\'f8\'e8\'e2\'e0\'e5\'ec\'fb\'e9 \'fd\'eb\'e5\'ec\'e5\'ed\'f2 \'ef\'e5\'f0\'e5\'f1\'f2\'e0\'ed\'e5\'f2 \'ee\'f2\'ee\'e1\'f0\'e0\'e6\'e0\'e5\'f2\'fc\'f1\'ff\par
\cf2\'d1\'e8\'ed\'f2\'e0\'ea\'f1\'e8\'f1\cf0 : \f0\lang1033 WaitNotVisibleElementAsync(string by, string locator, int sec)\f1\lang1049\par
\cf3\par
\cf2\'cf\'f0\'e8\'ec\'e5\'f0\cf3 :\par
\cf0\f0\lang1033 await tester.WaitNotVisibleElementAsync(Tester.BY_CSS, ""div[id='result']"", 2);\par
\par
await tester.WaitNotVisibleElementAsync(Tester.BY_XPATH, ""//div[@id='result']"", 2);\par
    }",

@"{\rtf1\ansi\ansicpg1251\deff0\nouicompat\deflang1049{\fonttbl{\f0\fnil\fcharset0 Calibri;}{\f1\fnil\fcharset204 Calibri;}}
{\colortbl ;\red0\green77\blue187;\red155\green0\blue211;\red0\green0\blue0;}
{\*\generator Riched20 10.0.22000}\viewkind4\uc1 
\pard\sl276\slmult1\cf1\f0\fs20\lang9 WaitNotVisibleElementByIdAsync\cf0\par
\cf2\f1\lang1049\'ce\'ef\'e8\'f1\'e0\'ed\'e8\'e5\cf0 : \'ec\'e5\'f2\'ee\'e4 \'e2\'fb\'ef\'ee\'eb\'ed\'ff\'e5\'f2 \'e2\'f0\'e5\'ec\'e5\'ed\'ed\'f3\'fe \'ee\'f1\'f2\'e0\'ed\'ee\'e2\'ea\'f3 \'e2\'fb\'ef\'ee\'eb\'ed\'e5\'ed\'e8\'ff \'f2\'e5\'f1\'f2\'e0 \'ed\'e0 \'f3\'ea\'e0\'e7\'e0\'ed\'ed\'ee\'e5 \'ea\'ee\'eb\'e8\'f7\'e5\'f1\'f2\'e2\'ee \'f1\'e5\'ea\'f3\'ed\'e4\f0\lang1033  \f1\lang1049\'e8 \'e6\'e4\'e5\'f2 \'ea\'ee\'e3\'e4\'e0  \'e7\'e0\'ef\'f0\'e0\'f8\'e8\'e2\'e0\'e5\'ec\'fb\'e9 \'fd\'eb\'e5\'ec\'e5\'ed\'f2 \'ef\'e5\'f0\'e5\'f1\'f2\'e0\'ed\'e5\'f2 \'ee\'f2\'ee\'e1\'f0\'e0\'e6\'e0\'e5\'f2\'fc\'f1\'ff \par
\cf2\'d1\'e8\'ed\'f2\'e0\'ea\'f1\'e8\'f1\cf0 : WaitNotVisibleElementByIdAsync(string id, int sec)\par
\cf3\par
\cf2\'cf\'f0\'e8\'ec\'e5\'f0\cf3 :\par
\cf0\f0\lang1033 await tester.WaitNotVisibleElementByIdAsync(""MyElement"", 25);\f1\lang1049\par
}",

@"{\rtf1\ansi\ansicpg1251\deff0\nouicompat\deflang1049{\fonttbl{\f0\fnil\fcharset0 Calibri;}{\f1\fnil\fcharset204 Calibri;}}
{\colortbl ;\red0\green77\blue187;\red155\green0\blue211;\red0\green0\blue0;}
{\*\generator Riched20 10.0.22000}\viewkind4\uc1 
\pard\sl276\slmult1\cf1\f0\fs20\lang9 WaitNotVisibleElementByNameAsync\cf0\par
\cf2\f1\lang1049\'ce\'ef\'e8\'f1\'e0\'ed\'e8\'e5\cf0 : \'ec\'e5\'f2\'ee\'e4 \'e2\'fb\'ef\'ee\'eb\'ed\'ff\'e5\'f2 \'e2\'f0\'e5\'ec\'e5\'ed\'ed\'f3\'fe \'ee\'f1\'f2\'e0\'ed\'ee\'e2\'ea\'f3 \'e2\'fb\'ef\'ee\'eb\'ed\'e5\'ed\'e8\'ff \'f2\'e5\'f1\'f2\'e0 \'ed\'e0 \'f3\'ea\'e0\'e7\'e0\'ed\'ed\'ee\'e5 \'ea\'ee\'eb\'e8\'f7\'e5\'f1\'f2\'e2\'ee \'f1\'e5\'ea\'f3\'ed\'e4\f0\lang1033  \f1\lang1049\'e8 \'e6\'e4\'e5\'f2 \'ea\'ee\'e3\'e4\'e0  \'e7\'e0\'ef\'f0\'e0\'f8\'e8\'e2\'e0\'e5\'ec\'fb\'e9 \'fd\'eb\'e5\'ec\'e5\'ed\'f2 \'ef\'e5\'f0\'e5\'f1\'f2\'e0\'ed\'e5\'f2 \'ee\'f2\'ee\'e1\'f0\'e0\'e6\'e0\'e5\'f2\'fc\'f1\'ff \par
\cf2\'d1\'e8\'ed\'f2\'e0\'ea\'f1\'e8\'f1\cf0 : WaitNotVisibleElementByNameAsync(string name, int index, int sec)\par
\cf3\par
\cf2\'cf\'f0\'e8\'ec\'e5\'f0\cf3 :\par
\cf0\f0\lang1033 await tester.WaitNotVisibleElementByNameAsync(""MyElement"", 0, 25);\f1\lang1049\par
}",

@"{\rtf1\ansi\ansicpg1251\deff0\nouicompat\deflang1049{\fonttbl{\f0\fnil\fcharset0 Calibri;}{\f1\fnil\fcharset204 Calibri;}}
{\colortbl ;\red0\green77\blue187;\red155\green0\blue211;\red0\green0\blue0;}
{\*\generator Riched20 10.0.22000}\viewkind4\uc1 
\pard\sl276\slmult1\cf1\f0\fs20\lang9 WaitNotVisibleElementByTagAsync\cf0\par
\cf2\f1\lang1049\'ce\'ef\'e8\'f1\'e0\'ed\'e8\'e5\cf0 : \'ec\'e5\'f2\'ee\'e4 \'e2\'fb\'ef\'ee\'eb\'ed\'ff\'e5\'f2 \'e2\'f0\'e5\'ec\'e5\'ed\'ed\'f3\'fe \'ee\'f1\'f2\'e0\'ed\'ee\'e2\'ea\'f3 \'e2\'fb\'ef\'ee\'eb\'ed\'e5\'ed\'e8\'ff \'f2\'e5\'f1\'f2\'e0 \'ed\'e0 \'f3\'ea\'e0\'e7\'e0\'ed\'ed\'ee\'e5 \'ea\'ee\'eb\'e8\'f7\'e5\'f1\'f2\'e2\'ee \'f1\'e5\'ea\'f3\'ed\'e4\f0\lang1033  \f1\lang1049\'e8 \'e6\'e4\'e5\'f2 \'ea\'ee\'e3\'e4\'e0  \'e7\'e0\'ef\'f0\'e0\'f8\'e8\'e2\'e0\'e5\'ec\'fb\'e9 \'fd\'eb\'e5\'ec\'e5\'ed\'f2 \'ef\'e5\'f0\'e5\'f1\'f2\'e0\'ed\'e5\'f2 \'ee\'f2\'ee\'e1\'f0\'e0\'e6\'e0\'e5\'f2\'fc\'f1\'ff \par
\cf2\'d1\'e8\'ed\'f2\'e0\'ea\'f1\'e8\'f1\cf0 : WaitNotVisibleElementByTagAsync(string tag, int index, int sec)\par
\cf3\par
\cf2\'cf\'f0\'e8\'ec\'e5\'f0\cf3 :\par
\cf0\f0\lang1033 await tester.WaitNotVisibleElementByTagAsync(""h1"", 0, 25);\f1\lang1049\par
}",

@"{\rtf1\ansi\ansicpg1251\deff0\nouicompat\deflang1049{\fonttbl{\f0\fnil\fcharset0 Calibri;}{\f1\fnil\fcharset204 Calibri;}}
{\colortbl ;\red0\green77\blue187;\red155\green0\blue211;\red0\green0\blue0;}
{\*\generator Riched20 10.0.22000}\viewkind4\uc1 
\pard\sl276\slmult1\cf1\f0\fs20\lang9 WaitVisibleElementByClassAsync\cf0\par
\cf2\f1\lang1049\'ce\'ef\'e8\'f1\'e0\'ed\'e8\'e5\cf0 : \'ec\'e5\'f2\'ee\'e4 \'e2\'fb\'ef\'ee\'eb\'ed\'ff\'e5\'f2 \'e2\'f0\'e5\'ec\'e5\'ed\'ed\'f3\'fe \'ee\'f1\'f2\'e0\'ed\'ee\'e2\'ea\'f3 \'e2\'fb\'ef\'ee\'eb\'ed\'e5\'ed\'e8\'ff \'f2\'e5\'f1\'f2\'e0 \'ed\'e0 \'f3\'ea\'e0\'e7\'e0\'ed\'ed\'ee\'e5 \'ea\'ee\'eb\'e8\'f7\'e5\'f1\'f2\'e2\'ee \'f1\'e5\'ea\'f3\'ed\'e4\f0\lang1033  \f1\lang1049\'e8 \'e6\'e4\'e5\'f2 \'ea\'ee\'e3\'e4\'e0  \'e7\'e0\'ef\'f0\'e0\'f8\'e8\'e2\'e0\'e5\'ec\'fb\'e9 \'fd\'eb\'e5\'ec\'e5\'ed\'f2 \'ee\'f2\'ee\'e1\'f0\'e0\'e7\'e8\'f2\'f1\'ff\par
\cf2\'d1\'e8\'ed\'f2\'e0\'ea\'f1\'e8\'f1\cf0 : WaitVisibleElementByClassAsync(string _class, int index, int sec)\par
\cf3\par
\cf2\'cf\'f0\'e8\'ec\'e5\'f0\cf3 :\par
\cf0\f0\lang1033 await tester.WaitVisibleElementByClassAsync(""my-element"", 0, 25);\f1\lang1049\par
}",

@"{\rtf1\ansi\ansicpg1251\deff0\nouicompat\deflang1049{\fonttbl{\f0\fnil\fcharset0 Calibri;}{\f1\fnil\fcharset204 Calibri;}}
{\colortbl ;\red0\green77\blue187;\red155\green0\blue211;\red0\green0\blue0;}
{\*\generator Riched20 10.0.22000}\viewkind4\uc1 
\pard\sl276\slmult1\cf1\f0\fs20\lang9 WaitVisibleElementAsync\cf0\par
\cf2\f1\lang1049\'ce\'ef\'e8\'f1\'e0\'ed\'e8\'e5\cf0 : \'ec\'e5\'f2\'ee\'e4 \'e2\'fb\'ef\'ee\'eb\'ed\'ff\'e5\'f2 \'e2\'f0\'e5\'ec\'e5\'ed\'ed\'f3\'fe \'ee\'f1\'f2\'e0\'ed\'ee\'e2\'ea\'f3 \'e2\'fb\'ef\'ee\'eb\'ed\'e5\'ed\'e8\'ff \'f2\'e5\'f1\'f2\'e0 \'ed\'e0 \'f3\'ea\'e0\'e7\'e0\'ed\'ed\'ee\'e5 \'ea\'ee\'eb\'e8\'f7\'e5\'f1\'f2\'e2\'ee \'f1\'e5\'ea\'f3\'ed\'e4 \'e8 \'e6\'e4\'e5\'f2 \'ea\'ee\'e3\'e4\'e0  \'e7\'e0\'ef\'f0\'e0\'f8\'e8\'e2\'e0\'e5\'ec\'fb\'e9 \'fd\'eb\'e5\'ec\'e5\'ed\'f2 \'ee\'f2\'ee\'e1\'f0\'e0\'e7\'e8\'f2\'f1\'ff\par
\cf2\'d1\'e8\'ed\'f2\'e0\'ea\'f1\'e8\'f1\cf0 : \f0\lang1033 WaitVisibleElementAsync(string by, string locator, int sec)\f1\lang1049\par
\cf3\par
\cf2\'cf\'f0\'e8\'ec\'e5\'f0\cf3 :\par
\cf0\f0\lang1033 await tester.WaitVisibleElementAsync(Tester.BY_CSS, ""div[id='result']"", 2);\par
\par
await tester.WaitVisibleElementAsync(Tester.BY_XPATH, ""//div[@id='result']"", 2);\par
    }",

@"{\rtf1\ansi\ansicpg1251\deff0\nouicompat\deflang1049{\fonttbl{\f0\fnil\fcharset0 Calibri;}{\f1\fnil\fcharset204 Calibri;}}
{\colortbl ;\red0\green77\blue187;\red155\green0\blue211;\red0\green0\blue0;}
{\*\generator Riched20 10.0.22000}\viewkind4\uc1 
\pard\sl276\slmult1\cf1\f0\fs20\lang9 WaitVisibleElementByIdAsync\cf0\par
\cf2\f1\lang1049\'ce\'ef\'e8\'f1\'e0\'ed\'e8\'e5\cf0 : \'ec\'e5\'f2\'ee\'e4 \'e2\'fb\'ef\'ee\'eb\'ed\'ff\'e5\'f2 \'e2\'f0\'e5\'ec\'e5\'ed\'ed\'f3\'fe \'ee\'f1\'f2\'e0\'ed\'ee\'e2\'ea\'f3 \'e2\'fb\'ef\'ee\'eb\'ed\'e5\'ed\'e8\'ff \'f2\'e5\'f1\'f2\'e0 \'ed\'e0 \'f3\'ea\'e0\'e7\'e0\'ed\'ed\'ee\'e5 \'ea\'ee\'eb\'e8\'f7\'e5\'f1\'f2\'e2\'ee \'f1\'e5\'ea\'f3\'ed\'e4\f0\lang1033  \f1\lang1049\'e8 \'e6\'e4\'e5\'f2 \'ea\'ee\'e3\'e4\'e0  \'e7\'e0\'ef\'f0\'e0\'f8\'e8\'e2\'e0\'e5\'ec\'fb\'e9 \'fd\'eb\'e5\'ec\'e5\'ed\'f2 \'ee\'f2\'ee\'e1\'f0\'e0\'e7\'e8\'f2\'f1\'ff\par
\cf2\'d1\'e8\'ed\'f2\'e0\'ea\'f1\'e8\'f1\cf0 : WaitVisibleElementByIdAsync(string id, int sec)\par
\cf3\par
\cf2\'cf\'f0\'e8\'ec\'e5\'f0\cf3 :\par
\cf0\f0\lang1033 await tester.WaitVisibleElementByIdAsync(""MyElement"", 25);\f1\lang1049\par
}",

@"{\rtf1\ansi\ansicpg1251\deff0\nouicompat\deflang1049{\fonttbl{\f0\fnil\fcharset0 Calibri;}{\f1\fnil\fcharset204 Calibri;}}
{\colortbl ;\red0\green77\blue187;\red155\green0\blue211;\red0\green0\blue0;}
{\*\generator Riched20 10.0.22000}\viewkind4\uc1 
\pard\sl276\slmult1\cf1\f0\fs20\lang9 WaitVisibleElementByNameAsync\cf0\par
\cf2\f1\lang1049\'ce\'ef\'e8\'f1\'e0\'ed\'e8\'e5\cf0 : \'ec\'e5\'f2\'ee\'e4 \'e2\'fb\'ef\'ee\'eb\'ed\'ff\'e5\'f2 \'e2\'f0\'e5\'ec\'e5\'ed\'ed\'f3\'fe \'ee\'f1\'f2\'e0\'ed\'ee\'e2\'ea\'f3 \'e2\'fb\'ef\'ee\'eb\'ed\'e5\'ed\'e8\'ff \'f2\'e5\'f1\'f2\'e0 \'ed\'e0 \'f3\'ea\'e0\'e7\'e0\'ed\'ed\'ee\'e5 \'ea\'ee\'eb\'e8\'f7\'e5\'f1\'f2\'e2\'ee \'f1\'e5\'ea\'f3\'ed\'e4\f0\lang1033  \f1\lang1049\'e8 \'e6\'e4\'e5\'f2 \'ea\'ee\'e3\'e4\'e0  \'e7\'e0\'ef\'f0\'e0\'f8\'e8\'e2\'e0\'e5\'ec\'fb\'e9 \'fd\'eb\'e5\'ec\'e5\'ed\'f2 \'ee\'f2\'ee\'e1\'f0\'e0\'e7\'e8\'f2\'f1\'ff\par
\cf2\'d1\'e8\'ed\'f2\'e0\'ea\'f1\'e8\'f1\cf0 : WaitVisibleElementByNameAsync(string name, int index, int sec)\par
\cf3\par
\cf2\'cf\'f0\'e8\'ec\'e5\'f0\cf3 :\par
\cf0\f0\lang1033 await tester.WaitVisibleElementByNameAsync(""MyElement"", 25);\f1\lang1049\par
}",

@"{\rtf1\ansi\ansicpg1251\deff0\nouicompat\deflang1049{\fonttbl{\f0\fnil\fcharset0 Calibri;}{\f1\fnil\fcharset204 Calibri;}}
{\colortbl ;\red0\green77\blue187;\red155\green0\blue211;\red0\green0\blue0;}
{\*\generator Riched20 10.0.22000}\viewkind4\uc1 
\pard\sl276\slmult1\cf1\f0\fs20\lang9 WaitVisibleElementByTagAsync\cf0\par
\cf2\f1\lang1049\'ce\'ef\'e8\'f1\'e0\'ed\'e8\'e5\cf0 : \'ec\'e5\'f2\'ee\'e4 \'e2\'fb\'ef\'ee\'eb\'ed\'ff\'e5\'f2 \'e2\'f0\'e5\'ec\'e5\'ed\'ed\'f3\'fe \'ee\'f1\'f2\'e0\'ed\'ee\'e2\'ea\'f3 \'e2\'fb\'ef\'ee\'eb\'ed\'e5\'ed\'e8\'ff \'f2\'e5\'f1\'f2\'e0 \'ed\'e0 \'f3\'ea\'e0\'e7\'e0\'ed\'ed\'ee\'e5 \'ea\'ee\'eb\'e8\'f7\'e5\'f1\'f2\'e2\'ee \'f1\'e5\'ea\'f3\'ed\'e4\f0\lang1033  \f1\lang1049\'e8 \'e6\'e4\'e5\'f2 \'ea\'ee\'e3\'e4\'e0  \'e7\'e0\'ef\'f0\'e0\'f8\'e8\'e2\'e0\'e5\'ec\'fb\'e9 \'fd\'eb\'e5\'ec\'e5\'ed\'f2 \'ee\'f2\'ee\'e1\'f0\'e0\'e7\'e8\'f2\'f1\'ff\par
\cf2\'d1\'e8\'ed\'f2\'e0\'ea\'f1\'e8\'f1\cf0 : WaitVisibleElementByTagAsync(string tag, int index, int sec)\par
\cf3\par
\cf2\'cf\'f0\'e8\'ec\'e5\'f0\cf3 :\par
\cf0\f0\lang1033 await tester.WaitVisibleElementByTagAsync(""h1"", 25);\f1\lang1049\par
}",

@"{\rtf1\ansi\ansicpg1251\deff0\nouicompat\deflang1049{\fonttbl{\f0\fnil\fcharset0 Calibri;}{\f1\fnil\fcharset204 Calibri;}}
{\colortbl ;\red0\green77\blue187;\red155\green0\blue211;\red0\green0\blue0;}
{\*\generator Riched20 10.0.22000}\viewkind4\uc1 
\pard\sl276\slmult1\cf1\f0\fs20\lang9 ExecuteJavaScriptAsync\cf0\par
\cf2\f1\lang1049\'ce\'ef\'e8\'f1\'e0\'ed\'e8\'e5\cf0 : \'ec\'e5\'f2\'ee\'e4 \'e2\'fb\'ef\'ee\'eb\'ed\'ff\'e5\'f2 \f0\lang1033 JavaScript \f1\lang1049\'ea\'ee\'e4 \'e8 \'e2\'ee\'e7\'e2\'f0\'e0\'f9\'e0\'e5\'f2 \'f1\'f2\'f0\'ee\'ea\'f3 \'f1 \'f0\'e5\'e7\'f3\'eb\'fc\'f2\'e0\'f2\'ee\'ec \'e2\'fb\'ef\'ee\'eb\'ed\'e5\'ed\'e8\'ff\par
\cf2\'d1\'e8\'ed\'f2\'e0\'ea\'f1\'e8\'f1\cf0 : ExecuteJavaScriptAsync(string script)\par
\cf3\par
\cf2\'cf\'f0\'e8\'ec\'e5\'f0\cf3 :\par
\cf0\f0\lang1033 string script = ""(function()\{ var element = document.getElementById('MyElement'); return element.innerText; \}());"";\par
string result = await tester.ExecuteJavaScriptAsync(script);\f1\lang1049\par
}
",

@"{\rtf1\ansi\ansicpg1251\deff0\nouicompat\deflang1049{\fonttbl{\f0\fnil\fcharset0 Calibri;}{\f1\fnil\fcharset204 Calibri;}}
{\colortbl ;\red0\green77\blue187;\red155\green0\blue211;\red0\green0\blue0;}
{\*\generator Riched20 10.0.22000}\viewkind4\uc1 
\pard\sl276\slmult1\cf1\f0\fs20\lang9 AssertEqualsAsync\cf0\par
\cf2\f1\lang1049\'ce\'ef\'e8\'f1\'e0\'ed\'e8\'e5\cf0 : \'ec\'e5\'f2\'ee\'e4 \'e2\'fb\'ef\'ee\'eb\'ed\'ff\'e5\'f2 \'ef\'f0\'ee\'e2\'e5\'f0\'ea\'f3 \'ec\'e5\'e6\'e4\'f3 \'f4\'e0\'ea\'f2\'e8\'f7\'e5\'f1\'ea\'e8\'ec \'e8 \'ee\'e6\'e8\'e4\'e0\'e5\'ec\'fb\'ec \'e7\'ed\'e0\'f7\'e5\'ed\'e8\'ff\'ec\'e8, \'e2 \'f1\'eb\'f3\'f7\'e0\'e5 \'ed\'e5\'f1\'ee\'e2\'ef\'e0\'e4\'e5\'ed\'e8\'ff \'ef\'f0\'ee\'e2\'e5\'f0\'ea\'e0 \'e1\'f3\'e4\'e5\'f2 \'f1\'f7\'e8\'f2\'e0\'f2\'fc\'f1\'ff \'ef\'f0\'ee\'e2\'e0\'eb\'fc\'ed\'ee\'e9\par
\cf2\'d1\'e8\'ed\'f2\'e0\'ea\'f1\'e8\'f1\cf0 : AssertEqualsAsync(dynamic expected, dynamic actual)\par
\cf3\par
\cf2\'cf\'f0\'e8\'ec\'e5\'f0\cf3 :\par
\cf0\f0\lang1033 string expected = ""xyz"";\par
string actual = ""xyz"";\par
bool result = await tester.AssertEqualsAsync(expected, actual);\f1\lang1049\par
    }",

@"{\rtf1\ansi\ansicpg1251\deff0\nouicompat\deflang1049{\fonttbl{\f0\fnil\fcharset0 Calibri;}{\f1\fnil\fcharset204 Calibri;}}
{\colortbl ;\red0\green77\blue187;\red155\green0\blue211;\red0\green0\blue0;}
{\*\generator Riched20 10.0.22000}\viewkind4\uc1 
\pard\sl276\slmult1\cf1\f0\fs20\lang9 AssertNotEqualsAsync\cf0\par
\cf2\f1\lang1049\'ce\'ef\'e8\'f1\'e0\'ed\'e8\'e5\cf0 : \'ec\'e5\'f2\'ee\'e4 \'e2\'fb\'ef\'ee\'eb\'ed\'ff\'e5\'f2 \'ef\'f0\'ee\'e2\'e5\'f0\'ea\'f3 \'ec\'e5\'e6\'e4\'f3 \'f4\'e0\'ea\'f2\'e8\'f7\'e5\'f1\'ea\'e8\'ec \'e8 \'ee\'e6\'e8\'e4\'e0\'e5\'ec\'fb\'ec \'e7\'ed\'e0\'f7\'e5\'ed\'e8\'ff\'ec\'e8, \'e2 \'f1\'eb\'f3\'f7\'e0\'e5 \'f1\'ee\'e2\'ef\'e0\'e4\'e5\'ed\'e8\'ff \'ef\'f0\'ee\'e2\'e5\'f0\'ea\'e0 \'e1\'f3\'e4\'e5\'f2 \'f1\'f7\'e8\'f2\'e0\'f2\'fc\'f1\'ff \'ef\'f0\'ee\'e2\'e0\'eb\'fc\'ed\'ee\'e9\par
\cf2\'d1\'e8\'ed\'f2\'e0\'ea\'f1\'e8\'f1\cf0 : AssertNotEqualsAsync(dynamic expected, dynamic actual)\par
\cf3\par
\cf2\'cf\'f0\'e8\'ec\'e5\'f0\cf3 :\par
\cf0\f0\lang1033 string expected = ""abc"";\par
string actual = ""xyz"";\par
bool result = await tester.AssertNotEqualsAsync(expected, actual);\f1\lang1049\par
    }",

@"{\rtf1\ansi\ansicpg1251\deff0\nouicompat\deflang1049{\fonttbl{\f0\fnil\fcharset0 Calibri;}{\f1\fnil\fcharset204 Calibri;}}
{\colortbl ;\red0\green77\blue187;\red155\green0\blue211;\red0\green0\blue0;}
{\*\generator Riched20 10.0.22000}\viewkind4\uc1 
\pard\sl276\slmult1\cf1\f0\fs20\lang9 AssertTrueAsync\cf0\par
\cf2\f1\lang1049\'ce\'ef\'e8\'f1\'e0\'ed\'e8\'e5\cf0 : \'ec\'e5\'f2\'ee\'e4 \'e2\'fb\'ef\'ee\'eb\'ed\'ff\'e5\'f2 \'ef\'f0\'ee\'e2\'e5\'f0\'ea\'f3 \'e7\'ed\'e0\'f7\'e5\'ed\'e8\'ff \f0\lang1033 true \f1\lang1049\'e8\'eb\'e8 \f0\lang1033 false\f1\lang1049 , \'e8 \'e2 \'f1\'eb\'f3\'f7\'e0\'e5 \'e5\'f1\'eb\'e8 \'e7\'ed\'e0\'f7\'e5\'ed\'e8\'e5\f0\lang1033  \f1\lang1049\'f0\'e0\'e2\'ed\'ee \f0\lang1033 false\f1\lang1049  \'ef\'f0\'ee\'e2\'e5\'f0\'ea\'e0 \'e1\'f3\'e4\'e5\'f2 \'f1\'f7\'e8\'f2\'e0\'f2\'fc\'f1\'ff \'ef\'f0\'ee\'e2\'e0\'eb\'fc\'ed\'ee\'e9\par
\cf2\'d1\'e8\'ed\'f2\'e0\'ea\'f1\'e8\'f1\cf0 : AssertTrueAsync(bool condition)\par
\cf3\par
\cf2\'cf\'f0\'e8\'ec\'e5\'f0\cf3 :\par
\cf0\f0\lang1033 bool flag = true;\par
bool result = await tester.AssertTrueAsync(flag);\f1\lang1049\par
}",

@"{\rtf1\ansi\ansicpg1251\deff0\nouicompat\deflang1049{\fonttbl{\f0\fnil\fcharset0 Calibri;}{\f1\fnil\fcharset204 Calibri;}}
{\colortbl ;\red0\green77\blue187;\red155\green0\blue211;\red0\green0\blue0;}
{\*\generator Riched20 10.0.22000}\viewkind4\uc1 
\pard\sl276\slmult1\cf1\f0\fs20\lang9 AssertFalseAsync\cf0\par
\cf2\f1\lang1049\'ce\'ef\'e8\'f1\'e0\'ed\'e8\'e5\cf0 : \'ec\'e5\'f2\'ee\'e4 \'e2\'fb\'ef\'ee\'eb\'ed\'ff\'e5\'f2 \'ef\'f0\'ee\'e2\'e5\'f0\'ea\'f3 \'e7\'ed\'e0\'f7\'e5\'ed\'e8\'ff \f0\lang1033 true \f1\lang1049\'e8\'eb\'e8 \f0\lang1033 false\f1\lang1049 , \'e8 \'e2 \'f1\'eb\'f3\'f7\'e0\'e5 \'e5\'f1\'eb\'e8 \'e7\'ed\'e0\'f7\'e5\'ed\'e8\'e5\f0\lang1033  \f1\lang1049\'f0\'e0\'e2\'ed\'ee \f0\lang1033 true\f1\lang1049  \'ef\'f0\'ee\'e2\'e5\'f0\'ea\'e0 \'e1\'f3\'e4\'e5\'f2 \'f1\'f7\'e8\'f2\'e0\'f2\'fc\'f1\'ff \'ef\'f0\'ee\'e2\'e0\'eb\'fc\'ed\'ee\'e9\par
\cf2\'d1\'e8\'ed\'f2\'e0\'ea\'f1\'e8\'f1\cf0 : AssertFalseAsync(bool condition)\par
\cf3\par
\cf2\'cf\'f0\'e8\'ec\'e5\'f0\cf3 :\par
\cf0\f0\lang1033 bool flag = false;\par
bool result = await tester.AssertFalseAsync(flag);\f1\lang1049\par
}",

@"{\rtf1\ansi\ansicpg1251\deff0\nouicompat\deflang1049{\fonttbl{\f0\fnil\fcharset0 Calibri;}{\f1\fnil\fcharset204 Calibri;}}
{\colortbl ;\red0\green77\blue187;\red155\green0\blue211;\red0\green0\blue0;}
{\*\generator Riched20 10.0.22000}\viewkind4\uc1 
\pard\sl276\slmult1\cf1\f0\fs20\lang9 BrowserGetErrorsAsync\cf0\par
\cf2\f1\lang1049\'ce\'ef\'e8\'f1\'e0\'ed\'e8\'e5\cf0 : \'ec\'e5\'f2\'ee\'e4 \'e2\'ee\'e7\'e2\'f0\'e0\'f9\'e0\'e5\'f2 \'f1\'ef\'e8\'f1\'ee\'ea \'ee\'f8\'e8\'e1\'ee\'ea \'e8 \'ef\'f0\'e5\'e4\'f3\'ef\'f0\'e5\'e6\'e4\'e5\'ed\'e8\'e9 \'e1\'f0\'e0\'f3\'e7\'e5\'f0\'e0\par
\cf2\'d1\'e8\'ed\'f2\'e0\'ea\'f1\'e8\'f1\cf0 : BrowserGetErrorsAsync()\par
\cf3\par
\cf2\'cf\'f0\'e8\'ec\'e5\'f0\cf3 :\par
\cf0\f0\lang1033 List<string> errors = await tester.BrowserGetErrorsAsync();\par
foreach (string error in errors)\par
\{\par
\tab tester.ConsoleMsg(error);\par
\}\par
}",

@"{\rtf1\ansi\ansicpg1251\deff0\nouicompat\deflang1049{\fonttbl{\f0\fnil\fcharset0 Calibri;}{\f1\fnil\fcharset204 Calibri;}}
{\colortbl ;\red0\green77\blue187;\red155\green0\blue211;\red0\green0\blue0;}
{\*\generator Riched20 10.0.22000}\viewkind4\uc1 
\pard\sl276\slmult1\cf1\f0\fs20\lang9 BrowserGetNetworkAsync\cf0\par
\cf2\f1\lang1049\'ce\'ef\'e8\'f1\'e0\'ed\'e8\'e5\cf0 : \'ec\'e5\'f2\'ee\'e4 \'e2\'ee\'e7\'e2\'f0\'e0\'f9\'e0\'e5\'f2 \'e2 \'f4\'ee\'f0\'ec\'e0\'f2\'e5 json \'e2\'f1\'e5 \'f2\'e5\'ea\'f3\'f9\'e8\'e5 \'f1\'ee\'ee\'e1\'f9\'e5\'ed\'e8\'ff \'e8\'e7 network\par
\cf2\'d1\'e8\'ed\'f2\'e0\'ea\'f1\'e8\'f1\cf0 : BrowserGetNetworkAsync()\par
\cf3\par
\cf2\'cf\'f0\'e8\'ec\'e5\'f0\cf3 :\par
\cf0\f0\lang1033 string events = await tester.BrowserGetNetworkAsync();\par
tester.ConsoleMsg(events);\par
}",

@"{\rtf1\ansi\ansicpg1251\deff0\nouicompat\deflang1049{\fonttbl{\f0\fnil\fcharset0 Calibri;}{\f1\fnil\fcharset204 Calibri;}}
{\colortbl ;\red0\green77\blue187;\red155\green0\blue211;\red0\green0\blue0;}
{\*\generator Riched20 10.0.22000}\viewkind4\uc1 
\pard\sl276\slmult1\cf1\f0\fs20\lang9 GetAttributeFromElementsByClassAsync\cf0\par
\cf2\f1\lang1049\'ce\'ef\'e8\'f1\'e0\'ed\'e8\'e5\cf0 : \'ec\'e5\'f2\'ee\'e4 \'e2\'ee\'e7\'e2\'f0\'e0\'f9\'e0\'e5\'f2\f0\lang1033  \f1\lang1049\'f1\'ef\'e8\'f1\'ee\'ea \'e7\'ed\'e0\'f7\'e5\'ed\'e8\'e9 \'f3\'ea\'e0\'e7\'e0\'ed\'ed\'ee\'e3\'ee \'e0\'f2\'f0\'e8\'e1\'f3\'f2\'e0 \'e8\'e7 \'ec\'ed\'ee\'e6\'e5\'f1\'f2\'e2\'e0 \'fd\'eb\'e5\'ec\'e5\'ed\'f2\'ee\'e2\par
\cf2\'d1\'e8\'ed\'f2\'e0\'ea\'f1\'e8\'f1\cf0 : GetAttributeFromElementsByClassAsync(string _class, string attribute)\par
\cf3\par
\cf2\'cf\'f0\'e8\'ec\'e5\'f0\cf3 :\par
\cf0\f0\lang1033 List<string> values = await tester.GetAttributeFromElementsByClassAsync(""text-field"", ""name"");\par
foreach (string value in values)\par
\{\par
\tab tester.ConsoleMsg(value);\par
\}\fs22\par
}
",

@"{\rtf1\ansi\ansicpg1251\deff0\nouicompat\deflang1049{\fonttbl{\f0\fnil\fcharset0 Calibri;}{\f1\fnil\fcharset204 Calibri;}}
{\colortbl ;\red0\green77\blue187;\red155\green0\blue211;\red0\green0\blue0;}
{\*\generator Riched20 10.0.22000}\viewkind4\uc1 
\pard\sl276\slmult1\cf1\f0\fs20\lang9 GetAttributeFromElementsByNameAsync\cf0\par
\cf2\f1\lang1049\'ce\'ef\'e8\'f1\'e0\'ed\'e8\'e5\cf0 : \'ec\'e5\'f2\'ee\'e4 \'e2\'ee\'e7\'e2\'f0\'e0\'f9\'e0\'e5\'f2\f0\lang1033  \f1\lang1049\'f1\'ef\'e8\'f1\'ee\'ea \'e7\'ed\'e0\'f7\'e5\'ed\'e8\'e9 \'f3\'ea\'e0\'e7\'e0\'ed\'ed\'ee\'e3\'ee \'e0\'f2\'f0\'e8\'e1\'f3\'f2\'e0 \'e8\'e7 \'ec\'ed\'ee\'e6\'e5\'f1\'f2\'e2\'e0 \'fd\'eb\'e5\'ec\'e5\'ed\'f2\'ee\'e2\par
\cf2\'d1\'e8\'ed\'f2\'e0\'ea\'f1\'e8\'f1\cf0 : GetAttributeFromElementsByNameAsync(string \f0\lang1033 name\f1\lang1049 , string attribute)\par
\cf3\par
\cf2\'cf\'f0\'e8\'ec\'e5\'f0\cf3 :\par
\cf0\f0\lang1033 List<string> values = await tester.GetAttributeFromElementsByNameAsync(""link"", ""href"");\par
foreach (string value in values)\par
\{\par
\tab tester.ConsoleMsg(value);\par
\}\fs22\par
}
",

@"{\rtf1\ansi\ansicpg1251\deff0\nouicompat\deflang1049{\fonttbl{\f0\fnil\fcharset0 Calibri;}{\f1\fnil\fcharset204 Calibri;}}
{\colortbl ;\red0\green77\blue187;\red155\green0\blue211;\red0\green0\blue0;}
{\*\generator Riched20 10.0.22000}\viewkind4\uc1 
\pard\sl276\slmult1\cf1\f0\fs20\lang9 GetAttributeFromElementsByTagAsync\cf0\par
\cf2\f1\lang1049\'ce\'ef\'e8\'f1\'e0\'ed\'e8\'e5\cf0 : \'ec\'e5\'f2\'ee\'e4 \'e2\'ee\'e7\'e2\'f0\'e0\'f9\'e0\'e5\'f2\f0\lang1033  \f1\lang1049\'f1\'ef\'e8\'f1\'ee\'ea \'e7\'ed\'e0\'f7\'e5\'ed\'e8\'e9 \'f3\'ea\'e0\'e7\'e0\'ed\'ed\'ee\'e3\'ee \'e0\'f2\'f0\'e8\'e1\'f3\'f2\'e0 \'e8\'e7 \'ec\'ed\'ee\'e6\'e5\'f1\'f2\'e2\'e0 \'fd\'eb\'e5\'ec\'e5\'ed\'f2\'ee\'e2\par
\cf2\'d1\'e8\'ed\'f2\'e0\'ea\'f1\'e8\'f1\cf0 : GetAttributeFromElementsByTagAsync(string \f0\lang1033 tag\f1\lang1049 , string attribute)\par
\cf3\par
\cf2\'cf\'f0\'e8\'ec\'e5\'f0\cf3 :\par
\cf0\f0\lang1033 List<string> values = await tester.GetAttributeFromElementsByTagAsync(""a"", ""href"");\par
foreach (string value in values)\par
\{\par
\tab tester.ConsoleMsg(value);\par
\}\fs22\par
}
",

@"{\rtf1\ansi\ansicpg1251\deff0\nouicompat\deflang1049{\fonttbl{\f0\fnil\fcharset0 Calibri;}{\f1\fnil\fcharset204 Calibri;}}
{\colortbl ;\red0\green77\blue187;\red155\green0\blue211;\red0\green0\blue0;}
{\*\generator Riched20 10.0.22000}\viewkind4\uc1 
\pard\sl276\slmult1\cf1\f0\fs20\lang9 SetAttributeInElementsByClassAsync\cf0\par
\cf2\f1\lang1049\'ce\'ef\'e8\'f1\'e0\'ed\'e8\'e5\cf0 : \'ec\'e5\'f2\'ee\'e4 \'e2\'f1\'f2\'e0\'e2\'e8\'f2\'fc \'e0\'f2\'f0\'e8\'e1\'f3\'f2 \'f1 \'f3\'ea\'e0\'e7\'e0\'ed\'ed\'fb\'ec \'e7\'ed\'e0\'f7\'e5\'ed\'e8\'e5\'ec \'e2 \'ec\'ed\'ee\'e6\'e5\'f1\'f2\'e2\'ee \'fd\'eb\'e5\'ec\'e5\'ed\'f2\'ee\'e2 \'e8 \'e2 \'f0\'e5\'e7\'f3\'eb\'fc\'f2\'e0\'f2\'e5 \'e2\'ee\'e7\'e2\'f0\'e0\'f9\'e0\'e5\'f2\f0\lang1033  \f1\lang1049\'f1\'ef\'e8\'f1\'ee\'ea\par
\cf2\'d1\'e8\'ed\'f2\'e0\'ea\'f1\'e8\'f1\cf0 : \f0\lang1033 SetAttributeInElementsByClassAsync\f1\lang1049 (string \f0\lang1033 _class\f1\lang1049 , string attribute\f0\lang1033 , string value\f1\lang1049 )\par
\cf3\par
\cf2\'cf\'f0\'e8\'ec\'e5\'f0\cf3 :\par
\cf0\f0\lang1033 List<string> values = await tester.SetAttributeInElementsByClassAsync(""text-field"", ""value"", ""test"");\par
foreach (string value in values)\par
\{\par
\tab tester.ConsoleMsg(value);\par
\}\par
}
",

@"{\rtf1\ansi\ansicpg1251\deff0\nouicompat\deflang1049{\fonttbl{\f0\fnil\fcharset0 Calibri;}{\f1\fnil\fcharset204 Calibri;}}
{\colortbl ;\red0\green77\blue187;\red155\green0\blue211;\red0\green0\blue0;}
{\*\generator Riched20 10.0.22000}\viewkind4\uc1 
\pard\sl276\slmult1\cf1\f0\fs20\lang9 SetAttributeInElementsAsync\cf0\par
\cf2\f1\lang1049\'ce\'ef\'e8\'f1\'e0\'ed\'e8\'e5\cf0 : \'ec\'e5\'f2\'ee\'e4 \'e2\'f1\'f2\'e0\'e2\'e8\'f2\'fc \'e0\'f2\'f0\'e8\'e1\'f3\'f2 \'f1 \'f3\'ea\'e0\'e7\'e0\'ed\'ed\'fb\'ec \'e7\'ed\'e0\'f7\'e5\'ed\'e8\'e5\'ec \'e2 \'ec\'ed\'ee\'e6\'e5\'f1\'f2\'e2\'ee \'fd\'eb\'e5\'ec\'e5\'ed\'f2\'ee\'e2 \'e8 \'e2 \'f0\'e5\'e7\'f3\'eb\'fc\'f2\'e0\'f2\'e5 \'e2\'ee\'e7\'e2\'f0\'e0\'f9\'e0\'e5\'f2 \'f1\'ef\'e8\'f1\'ee\'ea\par
\cf2\'d1\'e8\'ed\'f2\'e0\'ea\'f1\'e8\'f1\cf0 : \f0\lang1033 SetAttributeInElementsAsync(string by, string locator, string attribute, string value)\f1\lang1049\par
\cf3\par
\cf2\'cf\'f0\'e8\'ec\'e5\'f0\cf3 :\par
\cf0\f0\lang1033 List<string> values = await tester.SetAttributeInElementsAsync(Tester.BY_CSS, ""input"", ""class"", ""test-class"");\par
foreach (string value in values)\par
\{\par
\tab tester.ConsoleMsg(value);\par
\}\par
\par
List<string> values = await tester.SetAttributeInElementsAsync(Tester.BY_XPATH, ""//input"", ""class"", ""test-class"");\par
foreach (string value in values)\par
\{\par
\tab tester.ConsoleMsg(value);\par
\}\par
}",

@"{\rtf1\ansi\ansicpg1251\deff0\nouicompat\deflang1049{\fonttbl{\f0\fnil\fcharset0 Calibri;}{\f1\fnil\fcharset204 Calibri;}}
{\colortbl ;\red0\green77\blue187;\red155\green0\blue211;\red0\green0\blue0;\red0\green0\blue255;}
{\*\generator Riched20 10.0.22000}\viewkind4\uc1 
\pard\sl276\slmult1\cf1\f0\fs20\lang9 SetAttributeInElementsByNameAsync\cf0\par
\cf2\f1\lang1049\'ce\'ef\'e8\'f1\'e0\'ed\'e8\'e5\cf0 : \'ec\'e5\'f2\'ee\'e4 \'e2\'f1\'f2\'e0\'e2\'e8\'f2\'fc \'e0\'f2\'f0\'e8\'e1\'f3\'f2 \'f1 \'f3\'ea\'e0\'e7\'e0\'ed\'ed\'fb\'ec \'e7\'ed\'e0\'f7\'e5\'ed\'e8\'e5\'ec \'e2 \'ec\'ed\'ee\'e6\'e5\'f1\'f2\'e2\'ee \'fd\'eb\'e5\'ec\'e5\'ed\'f2\'ee\'e2 \'e8 \'e2 \'f0\'e5\'e7\'f3\'eb\'fc\'f2\'e0\'f2\'e5 \'e2\'ee\'e7\'e2\'f0\'e0\'f9\'e0\'e5\'f2\f0\lang1033  \f1\lang1049\'f1\'ef\'e8\'f1\'ee\'ea\par
\cf2\'d1\'e8\'ed\'f2\'e0\'ea\'f1\'e8\'f1\cf0 : \f0\lang1033 SetAttributeInElementsByNameAsync\f1\lang1049 (string \f0\lang1033 name\f1\lang1049 , string attribute\f0\lang1033 , string value\f1\lang1049 )\par
\cf3\par
\cf2\'cf\'f0\'e8\'ec\'e5\'f0\cf3 :\par
\cf0\f0\lang1033 List<string> values = await tester.SetAttributeInElementsByNameAsync(""link"", ""href"", ""www.test.ru"");\par
foreach (string value in values)\par
\{\par
\tab tester.ConsoleMsg(value);\par
\}\par
}
",

@"{\rtf1\ansi\ansicpg1251\deff0\nouicompat\deflang1049{\fonttbl{\f0\fnil\fcharset0 Calibri;}{\f1\fnil\fcharset204 Calibri;}}
{\colortbl ;\red0\green77\blue187;\red155\green0\blue211;\red0\green0\blue0;\red0\green0\blue255;}
{\*\generator Riched20 10.0.22000}\viewkind4\uc1 
\pard\sl276\slmult1\cf1\f0\fs20\lang9 SetAttributeInElementsByTagAsync\cf0\par
\cf2\f1\lang1049\'ce\'ef\'e8\'f1\'e0\'ed\'e8\'e5\cf0 : \'ec\'e5\'f2\'ee\'e4 \'e2\'f1\'f2\'e0\'e2\'e8\'f2\'fc \'e0\'f2\'f0\'e8\'e1\'f3\'f2 \'f1 \'f3\'ea\'e0\'e7\'e0\'ed\'ed\'fb\'ec \'e7\'ed\'e0\'f7\'e5\'ed\'e8\'e5\'ec \'e2 \'ec\'ed\'ee\'e6\'e5\'f1\'f2\'e2\'ee \'fd\'eb\'e5\'ec\'e5\'ed\'f2\'ee\'e2 \'e8 \'e2 \'f0\'e5\'e7\'f3\'eb\'fc\'f2\'e0\'f2\'e5 \'e2\'ee\'e7\'e2\'f0\'e0\'f9\'e0\'e5\'f2\f0\lang1033  \f1\lang1049\'f1\'ef\'e8\'f1\'ee\'ea\par
\cf2\'d1\'e8\'ed\'f2\'e0\'ea\'f1\'e8\'f1\cf0 : \f0\lang1033 SetAttributeInElementsByTagAsync\f1\lang1049 (string \f0\lang1033 tag\f1\lang1049 , string attribute\f0\lang1033 , string value\f1\lang1049 )\par
\cf3\par
\cf2\'cf\'f0\'e8\'ec\'e5\'f0\cf3 :\par
\cf0\f0\lang1033 List<string> values = await tester.SetAttributeInElementsByTagAsync(""a"", ""href"", ""www.test.ru"");\par
foreach (string value in values)\par
\{\par
\tab tester.ConsoleMsg(value);\par
\}\par
}
",

@"{\rtf1\ansi\ansicpg1251\deff0\nouicompat\deflang1049{\fonttbl{\f0\fnil\fcharset0 Calibri;}{\f1\fnil\fcharset204 Calibri;}}
{\colortbl ;\red0\green77\blue187;\red155\green0\blue211;\red0\green0\blue0;}
{\*\generator Riched20 10.0.22000}\viewkind4\uc1 
\pard\sl276\slmult1\cf1\f0\fs20\lang9 GetHtmlFromElementByClassAsync\cf0\par
\cf2\f1\lang1049\'ce\'ef\'e8\'f1\'e0\'ed\'e8\'e5\cf0 : \'ec\'e5\'f2\'ee\'e4 \'e2\'ee\'e7\'e2\'f0\'e0\'f9\'e0\'e5\'f2 \f0\lang1033 html \f1\lang1049\'ef\'f0\'e5\'e4\'f1\'f2\'e0\'e2\'eb\'e5\'ed\'e8\'e5 \'ee\'e1\'fa\'e5\'ea\'f2\'e0 \'e2 \'f1\'f2\'f0\'ee\'f7\'ed\'ee\'ec \'e2\'fb\'f0\'e0\'e6\'e5\'ed\'e8\'e8\par
\cf2\'d1\'e8\'ed\'f2\'e0\'ea\'f1\'e8\'f1\cf0 : \f0\lang1033 GetHtmlFromElementByClassAsync\f1\lang1049 (string _class, int index)\par
\cf3\par
\cf2\'cf\'f0\'e8\'ec\'e5\'f0\cf3 :\par
\cf0\f0\lang1033 string html = await tester.GetHtmlFromElementByClassAsync(""text-field"", 0);\par
}",

@"{\rtf1\ansi\ansicpg1251\deff0\nouicompat\deflang1049{\fonttbl{\f0\fnil\fcharset0 Calibri;}{\f1\fnil\fcharset204 Calibri;}}
{\colortbl ;\red0\green77\blue187;\red155\green0\blue211;\red0\green0\blue0;}
{\*\generator Riched20 10.0.22000}\viewkind4\uc1 
\pard\sl276\slmult1\cf1\f0\fs20\lang9 GetHtmlFromElementAsync\cf0\par
\cf2\f1\lang1049\'ce\'ef\'e8\'f1\'e0\'ed\'e8\'e5\cf0 : \'ec\'e5\'f2\'ee\'e4 \'e2\'ee\'e7\'e2\'f0\'e0\'f9\'e0\'e5\'f2 html \'ef\'f0\'e5\'e4\'f1\'f2\'e0\'e2\'eb\'e5\'ed\'e8\'e5 \'ee\'e1\'fa\'e5\'ea\'f2\'e0 \'e2 \'f1\'f2\'f0\'ee\'f7\'ed\'ee\'ec \'e2\'fb\'f0\'e0\'e6\'e5\'ed\'e8\'e8\par
\cf2\'d1\'e8\'ed\'f2\'e0\'ea\'f1\'e8\'f1\cf0 : \f0\lang1033 GetHtmlFromElementAsync(string by, string locator)\f1\lang1049\par
\cf3\par
\cf2\'cf\'f0\'e8\'ec\'e5\'f0\cf3 :\par
\cf0\f0\lang1033 string html = await tester.GetHtmlFromElementAsync(Tester.BY_CSS, ""#auth > h2"");\par
\par
string html = await tester.GetHtmlFromElementAsync(Tester.BY_XPATH, ""//div[@id='auth']//h2"");\par
    }",

@"{\rtf1\ansi\ansicpg1251\deff0\nouicompat\deflang1049{\fonttbl{\f0\fnil\fcharset0 Calibri;}{\f1\fnil\fcharset204 Calibri;}}
{\colortbl ;\red0\green77\blue187;\red155\green0\blue211;\red0\green0\blue0;}
{\*\generator Riched20 10.0.22000}\viewkind4\uc1 
\pard\sl276\slmult1\cf1\f0\fs20\lang9 GetHtmlFromElementByIdAsync\cf0\par
\cf2\f1\lang1049\'ce\'ef\'e8\'f1\'e0\'ed\'e8\'e5\cf0 : \'ec\'e5\'f2\'ee\'e4 \'e2\'ee\'e7\'e2\'f0\'e0\'f9\'e0\'e5\'f2 \f0\lang1033 html \f1\lang1049\'ef\'f0\'e5\'e4\'f1\'f2\'e0\'e2\'eb\'e5\'ed\'e8\'e5 \'ee\'e1\'fa\'e5\'ea\'f2\'e0 \'e2 \'f1\'f2\'f0\'ee\'f7\'ed\'ee\'ec \'e2\'fb\'f0\'e0\'e6\'e5\'ed\'e8\'e8\par
\cf2\'d1\'e8\'ed\'f2\'e0\'ea\'f1\'e8\'f1\cf0 : \f0\lang1033 GetHtmlFromElementByIdAsync(string id)\f1\lang1049\par
\cf3\par
\cf2\'cf\'f0\'e8\'ec\'e5\'f0\cf3 :\par
\cf0\f0\lang1033 string html = await tester.GetHtmlFromElementByIdAsync(""login"");\par
}",

@"{\rtf1\ansi\ansicpg1251\deff0\nouicompat\deflang1049{\fonttbl{\f0\fnil\fcharset0 Calibri;}{\f1\fnil\fcharset204 Calibri;}}
{\colortbl ;\red0\green77\blue187;\red155\green0\blue211;\red0\green0\blue0;}
{\*\generator Riched20 10.0.22000}\viewkind4\uc1 
\pard\sl276\slmult1\cf1\f0\fs20\lang9 GetHtmlFromElementByNameAsync\cf0\par
\cf2\f1\lang1049\'ce\'ef\'e8\'f1\'e0\'ed\'e8\'e5\cf0 : \'ec\'e5\'f2\'ee\'e4 \'e2\'ee\'e7\'e2\'f0\'e0\'f9\'e0\'e5\'f2 \f0\lang1033 html \f1\lang1049\'ef\'f0\'e5\'e4\'f1\'f2\'e0\'e2\'eb\'e5\'ed\'e8\'e5 \'ee\'e1\'fa\'e5\'ea\'f2\'e0 \'e2 \'f1\'f2\'f0\'ee\'f7\'ed\'ee\'ec \'e2\'fb\'f0\'e0\'e6\'e5\'ed\'e8\'e8\par
\cf2\'d1\'e8\'ed\'f2\'e0\'ea\'f1\'e8\'f1\cf0 : \f0\lang1033 GetHtmlFromElementByNameAsync(string name, int index)\f1\lang1049\par
\cf3\par
\cf2\'cf\'f0\'e8\'ec\'e5\'f0\cf3 :\par
\cf0\f0\lang1033 string html = await tester.GetHtmlFromElementByNameAsync(""field"", 0);\par
}",

@"{\rtf1\ansi\ansicpg1251\deff0\nouicompat\deflang1049{\fonttbl{\f0\fnil\fcharset0 Calibri;}{\f1\fnil\fcharset204 Calibri;}}
{\colortbl ;\red0\green77\blue187;\red155\green0\blue211;\red0\green0\blue0;}
{\*\generator Riched20 10.0.22000}\viewkind4\uc1 
\pard\sl276\slmult1\cf1\f0\fs20\lang9 GetHtmlFromElementByTagAsync\cf0\par
\cf2\f1\lang1049\'ce\'ef\'e8\'f1\'e0\'ed\'e8\'e5\cf0 : \'ec\'e5\'f2\'ee\'e4 \'e2\'ee\'e7\'e2\'f0\'e0\'f9\'e0\'e5\'f2 \f0\lang1033 html \f1\lang1049\'ef\'f0\'e5\'e4\'f1\'f2\'e0\'e2\'eb\'e5\'ed\'e8\'e5 \'ee\'e1\'fa\'e5\'ea\'f2\'e0 \'e2 \'f1\'f2\'f0\'ee\'f7\'ed\'ee\'ec \'e2\'fb\'f0\'e0\'e6\'e5\'ed\'e8\'e8\par
\cf2\'d1\'e8\'ed\'f2\'e0\'ea\'f1\'e8\'f1\cf0 : \f0\lang1033 GetHtmlFromElementByTagAsync(string tag, int index)\f1\lang1049\par
\cf3\par
\cf2\'cf\'f0\'e8\'ec\'e5\'f0\cf3 :\par
\cf0\f0\lang1033 string html = await tester.GetHtmlFromElementByTagAsync(""h1"", 0);\par
}",

@"{\rtf1\ansi\ansicpg1251\deff0\nouicompat\deflang1049{\fonttbl{\f0\fnil\fcharset0 Calibri;}{\f1\fnil\fcharset204 Calibri;}}
{\colortbl ;\red0\green77\blue187;\red155\green0\blue211;\red0\green0\blue0;}
{\*\generator Riched20 10.0.22000}\viewkind4\uc1 
\pard\sl276\slmult1\cf1\f0\fs20\lang9 SetHtmlInElementByClassAsync\cf0\par
\cf2\f1\lang1049\'ce\'ef\'e8\'f1\'e0\'ed\'e8\'e5\cf0 : \'ec\'e5\'f2\'ee\'e4 \'e2\'f1\'f2\'e0\'e2\'eb\'ff\'e5\'f2 \f0\lang1033 html \f1\lang1049\'ef\'f0\'e5\'e4\'f1\'f2\'e0\'e2\'eb\'e5\'ed\'e8\'e5 \'ee\'e1\'fa\'e5\'ea\'f2\'e0 \'e2 \'f3\'ea\'e0\'e7\'e0\'ed\'ed\'fb\'e9 \'fd\'eb\'e5\'ec\'e5\'ed\'f2\par
\cf2\'d1\'e8\'ed\'f2\'e0\'ea\'f1\'e8\'f1\cf0 : \f0\lang1033 SetHtmlInElementByClassAsync(string _class, int index, string html)\f1\lang1049\par
\cf3\par
\cf2\'cf\'f0\'e8\'ec\'e5\'f0\cf3 :\par
\cf0\f0\lang1033 await tester.SetHtmlInElementByClassAsync(""text-field"", 0, ""<h1>\f1\lang1049\'fd\'f2\'ee \'f2\'e5\'f1\'f2</h1>"");\f0\lang1033\par
}",

@"{\rtf1\ansi\ansicpg1251\deff0\nouicompat\deflang1049{\fonttbl{\f0\fnil\fcharset0 Calibri;}{\f1\fnil\fcharset204 Calibri;}}
{\colortbl ;\red0\green77\blue187;\red155\green0\blue211;\red0\green0\blue0;}
{\*\generator Riched20 10.0.22000}\viewkind4\uc1 
\pard\sl276\slmult1\cf1\f0\fs20\lang9 SetHtmlInElementAsync\cf0\par
\cf2\f1\lang1049\'ce\'ef\'e8\'f1\'e0\'ed\'e8\'e5\cf0 : \'ec\'e5\'f2\'ee\'e4 \'e2\'f1\'f2\'e0\'e2\'eb\'ff\'e5\'f2 html \'ef\'f0\'e5\'e4\'f1\'f2\'e0\'e2\'eb\'e5\'ed\'e8\'e5 \'ee\'e1\'fa\'e5\'ea\'f2\'e0 \'e2 \'f3\'ea\'e0\'e7\'e0\'ed\'ed\'fb\'e9 \'fd\'eb\'e5\'ec\'e5\'ed\'f2\par
\cf2\'d1\'e8\'ed\'f2\'e0\'ea\'f1\'e8\'f1\cf0 : \f0\lang1033 SetHtmlInElementAsync(string by, string locator, string html)\f1\lang1049\par
\cf3\par
\cf2\'cf\'f0\'e8\'ec\'e5\'f0\cf3 :\par
\cf0\f0\lang1033 await tester.SetHtmlInElementAsync(Tester.BY_CSS, ""#auth > h2"", ""<div>\f1\lang1049\'d2\'e5\'f1\'f2\'ee\'e2\'fb\'e9 \'e1\'eb\'ee\'ea</div>"");\par
\par
await tester.SetHtmlInElementAsync(Tester.BY_XPATH, ""//div[@id='auth']//h2"", ""<div>\'d2\'e5\'f1\'f2\'ee\'e2\'fb\'e9 \'e1\'eb\'ee\'ea</div>"");\f0\lang1033\par
    }",

@"{\rtf1\ansi\ansicpg1251\deff0\nouicompat\deflang1049{\fonttbl{\f0\fnil\fcharset0 Calibri;}{\f1\fnil\fcharset204 Calibri;}}
{\colortbl ;\red0\green77\blue187;\red155\green0\blue211;\red0\green0\blue0;}
{\*\generator Riched20 10.0.22000}\viewkind4\uc1 
\pard\sl276\slmult1\cf1\f0\fs20\lang9 SetHtmlInElementByIdAsync\cf0\par
\cf2\f1\lang1049\'ce\'ef\'e8\'f1\'e0\'ed\'e8\'e5\cf0 : \'ec\'e5\'f2\'ee\'e4 \'e2\'f1\'f2\'e0\'e2\'eb\'ff\'e5\'f2 \f0\lang1033 html \f1\lang1049\'ef\'f0\'e5\'e4\'f1\'f2\'e0\'e2\'eb\'e5\'ed\'e8\'e5 \'ee\'e1\'fa\'e5\'ea\'f2\'e0 \'e2 \'f3\'ea\'e0\'e7\'e0\'ed\'ed\'fb\'e9 \'fd\'eb\'e5\'ec\'e5\'ed\'f2\par
\cf2\'d1\'e8\'ed\'f2\'e0\'ea\'f1\'e8\'f1\cf0 : \f0\lang1033 SetHtmlInElementByIdAsync(string id, string html)\f1\lang1049\par
\cf3\par
\cf2\'cf\'f0\'e8\'ec\'e5\'f0\cf3 :\par
\cf0\f0\lang1033 await tester.SetHtmlInElementByIdAsync(""auth"", ""<h1>\f1\lang1049\'fd\'f2\'ee \'f2\'e5\'f1\'f2</h1>"");\f0\lang1033\par
}",

@"{\rtf1\ansi\ansicpg1251\deff0\nouicompat\deflang1049{\fonttbl{\f0\fnil\fcharset0 Calibri;}{\f1\fnil\fcharset204 Calibri;}}
{\colortbl ;\red0\green77\blue187;\red155\green0\blue211;\red0\green0\blue0;}
{\*\generator Riched20 10.0.22000}\viewkind4\uc1 
\pard\sl276\slmult1\cf1\f0\fs20\lang9 SetHtmlInElementByNameAsync\cf0\par
\cf2\f1\lang1049\'ce\'ef\'e8\'f1\'e0\'ed\'e8\'e5\cf0 : \'ec\'e5\'f2\'ee\'e4 \'e2\'f1\'f2\'e0\'e2\'eb\'ff\'e5\'f2 \f0\lang1033 html \f1\lang1049\'ef\'f0\'e5\'e4\'f1\'f2\'e0\'e2\'eb\'e5\'ed\'e8\'e5 \'ee\'e1\'fa\'e5\'ea\'f2\'e0 \'e2 \'f3\'ea\'e0\'e7\'e0\'ed\'ed\'fb\'e9 \'fd\'eb\'e5\'ec\'e5\'ed\'f2\par
\cf2\'d1\'e8\'ed\'f2\'e0\'ea\'f1\'e8\'f1\cf0 : \f0\lang1033 SetHtmlInElementByNameAsync(string name, int index, string html)\f1\lang1049\par
\cf3\par
\cf2\'cf\'f0\'e8\'ec\'e5\'f0\cf3 :\par
\cf0\f0\lang1033 await tester.SetHtmlInElementByNameAsync(""block"", 0,  ""<h1>\f1\lang1049\'fd\'f2\'ee \'f2\'e5\'f1\'f2</h1>"");\f0\lang1033\par
}",

@"{\rtf1\ansi\ansicpg1251\deff0\nouicompat\deflang1049{\fonttbl{\f0\fnil\fcharset0 Calibri;}{\f1\fnil\fcharset204 Calibri;}}
{\colortbl ;\red0\green77\blue187;\red155\green0\blue211;\red0\green0\blue0;}
{\*\generator Riched20 10.0.22000}\viewkind4\uc1 
\pard\sl276\slmult1\cf1\f0\fs20\lang9 SetHtmlInElementByTagAsync\cf0\par
\cf2\f1\lang1049\'ce\'ef\'e8\'f1\'e0\'ed\'e8\'e5\cf0 : \'ec\'e5\'f2\'ee\'e4 \'e2\'f1\'f2\'e0\'e2\'eb\'ff\'e5\'f2 \f0\lang1033 html \f1\lang1049\'ef\'f0\'e5\'e4\'f1\'f2\'e0\'e2\'eb\'e5\'ed\'e8\'e5 \'ee\'e1\'fa\'e5\'ea\'f2\'e0 \'e2 \'f3\'ea\'e0\'e7\'e0\'ed\'ed\'fb\'e9 \'fd\'eb\'e5\'ec\'e5\'ed\'f2\par
\cf2\'d1\'e8\'ed\'f2\'e0\'ea\'f1\'e8\'f1\cf0 : \f0\lang1033 SetHtmlInElementByTagAsync(string tag, int index, string html)\f1\lang1049\par
\cf3\par
\cf2\'cf\'f0\'e8\'ec\'e5\'f0\cf3 :\par
\cf0\f0\lang1033 await tester.SetHtmlInElementByTagAsync(""div"", 0,  ""<h1>\f1\lang1049\'fd\'f2\'ee \'f2\'e5\'f1\'f2</h1>"");\f0\lang1033\par
}",

@"{\rtf1\ansi\ansicpg1251\deff0\nouicompat\deflang1049{\fonttbl{\f0\fnil\fcharset0 Calibri;}{\f1\fnil\fcharset204 Calibri;}}
{\colortbl ;\red0\green77\blue187;\red155\green0\blue211;\red0\green0\blue0;}
{\*\generator Riched20 10.0.22000}\viewkind4\uc1 
\pard\sl276\slmult1\cf1\f0\fs20\lang9 HTMLElement\cf0\par
\cf2\f1\lang1049\'ce\'ef\'e8\'f1\'e0\'ed\'e8\'e5\cf0 : \'e2\'f1\'ef\'ee\'ec\'ee\'e3\'e0\'f2\'e5\'eb\'fc\'ed\'fb\'e9 \'ea\'eb\'e0\'f1\'f1, \'ee\'e1\'fa\'e5\'ea\'f2 html \'fd\'eb\'e5\'ec\'e5\'ed\'f2\'e0\par
\cf2\'d1\'e8\'ed\'f2\'e0\'ea\'f1\'e8\'f1\cf0 : \f0\lang1033 HTMLElement(Tester tester, string by, string locator)\f1\lang1049\par
\cf3\par
\cf2\'cf\'f0\'e8\'ec\'e5\'f0\cf3 :\par
\cf0\f0\lang1033 HTMLElement element = await tester.GetElementAsync(Tester.BY_CSS, ""#auth #buttonLogin"");\par
await element.ClickAsync();\par
\par
HTMLElement element = await tester.GetElementAsync(Tester.BY_XPATH, ""//div[@id='auth']//input[@id='buttonLogin']"");\par
await element.ClickAsync();\par
    }
 ",

@"{\rtf1\ansi\ansicpg1251\deff0\nouicompat\deflang1049{\fonttbl{\f0\fnil\fcharset0 Calibri;}{\f1\fnil\fcharset204 Calibri;}}
{\colortbl ;\red0\green77\blue187;\red155\green0\blue211;\red0\green0\blue0;}
{\*\generator Riched20 10.0.22000}\viewkind4\uc1 
\pard\sl276\slmult1\cf1\f0\fs20\lang9 Id\cf0\par
\cf2\f1\lang1049\'ce\'ef\'e8\'f1\'e0\'ed\'e8\'e5\cf0 : \'ef\'e5\'f0\'e5\'ec\'e5\'ed\'ed\'e0\'ff \'e2\'ee\'e7\'e2\'f0\'e0\'f9\'e0\'e5\'f2 \'e8\'eb\'e8 \'ef\'ee\'eb\'f3\'f7\'e0\'e5\'f2 \'e4\'e0\'ed\'ed\'fb\'e5 ID\par
\cf2\'d1\'e8\'ed\'f2\'e0\'ea\'f1\'e8\'f1\cf0 : \f0\lang1033 string Id \{ get; set; \}\f1\lang1049\par
\cf3\par
\cf2\'cf\'f0\'e8\'ec\'e5\'f0\cf3 :\par
\cf0\f0\lang1033 HTMLElement element = await tester.GetElementAsync(Tester.BY_XPATH, ""//*[@id='MyInput']"");\par
tester.ConsoleMsg(""ID: "" + element.Id);\par
    }",

@"{\rtf1\ansi\ansicpg1251\deff0\nouicompat\deflang1049{\fonttbl{\f0\fnil\fcharset0 Calibri;}{\f1\fnil\fcharset204 Calibri;}}
{\colortbl ;\red0\green77\blue187;\red155\green0\blue211;\red0\green0\blue0;}
{\*\generator Riched20 10.0.22000}\viewkind4\uc1 
\pard\sl276\slmult1\cf1\f0\fs20\lang9 Name\cf0\par
\cf2\f1\lang1049\'ce\'ef\'e8\'f1\'e0\'ed\'e8\'e5\cf0 : \'ef\'e5\'f0\'e5\'ec\'e5\'ed\'ed\'e0\'ff \'e2\'ee\'e7\'e2\'f0\'e0\'f9\'e0\'e5\'f2 \'e8\'eb\'e8 \'ef\'ee\'eb\'f3\'f7\'e0\'e5\'f2 \'e4\'e0\'ed\'ed\'fb\'e5 Name\par
\cf2\'d1\'e8\'ed\'f2\'e0\'ea\'f1\'e8\'f1\cf0 : \f0\lang1033 string Name \{ get; set; \}\f1\lang1049\par
\cf3\par
\cf2\'cf\'f0\'e8\'ec\'e5\'f0\cf3 :\par
\cf0\f0\lang1033 HTMLElement element = await tester.GetElementAsync(Tester.BY_XPATH, ""//*[@id='MyInput']"");\par
tester.ConsoleMsg(""NAME: "" + element.Name);\par
    }",

@"{\rtf1\ansi\ansicpg1251\deff0\nouicompat\deflang1049{\fonttbl{\f0\fnil\fcharset0 Calibri;}{\f1\fnil\fcharset204 Calibri;}}
{\colortbl ;\red0\green77\blue187;\red155\green0\blue211;\red0\green0\blue0;}
{\*\generator Riched20 10.0.22000}\viewkind4\uc1 
\pard\sl276\slmult1\cf1\f0\fs20\lang9 Class\cf0\par
\cf2\f1\lang1049\'ce\'ef\'e8\'f1\'e0\'ed\'e8\'e5\cf0 : \'ef\'e5\'f0\'e5\'ec\'e5\'ed\'ed\'e0\'ff \'e2\'ee\'e7\'e2\'f0\'e0\'f9\'e0\'e5\'f2 \'e8\'eb\'e8 \'ef\'ee\'eb\'f3\'f7\'e0\'e5\'f2 \'e4\'e0\'ed\'ed\'fb\'e5 Class\par
\cf2\'d1\'e8\'ed\'f2\'e0\'ea\'f1\'e8\'f1\cf0 : \f0\lang1033 string Class \{ get; set; \}\f1\lang1049\par
\cf3\par
\cf2\'cf\'f0\'e8\'ec\'e5\'f0\cf3 :\par
\cf0\f0\lang1033 HTMLElement element = await tester.GetElementAsync(Tester.BY_XPATH, ""//*[@id='MyInput']"");\par
tester.ConsoleMsg(""CLASS: "" + element.Class);\par
    }",

@"{\rtf1\ansi\ansicpg1251\deff0\nouicompat\deflang1049{\fonttbl{\f0\fnil\fcharset0 Calibri;}{\f1\fnil\fcharset204 Calibri;}}
{\colortbl ;\red0\green77\blue187;\red155\green0\blue211;\red0\green0\blue0;}
{\*\generator Riched20 10.0.22000}\viewkind4\uc1 
\pard\sl276\slmult1\cf1\f0\fs20\lang9 Type\cf0\par
\cf2\f1\lang1049\'ce\'ef\'e8\'f1\'e0\'ed\'e8\'e5\cf0 : \'ef\'e5\'f0\'e5\'ec\'e5\'ed\'ed\'e0\'ff \'e2\'ee\'e7\'e2\'f0\'e0\'f9\'e0\'e5\'f2 \'e8\'eb\'e8 \'ef\'ee\'eb\'f3\'f7\'e0\'e5\'f2 \'e4\'e0\'ed\'ed\'fb\'e5 Type\par
\cf2\'d1\'e8\'ed\'f2\'e0\'ea\'f1\'e8\'f1\cf0 : \f0\lang1033 string Type \{ get; set; \}\f1\lang1049\par
\cf3\par
\cf2\'cf\'f0\'e8\'ec\'e5\'f0\cf3 :\par
\cf0\f0\lang1033 HTMLElement element = await tester.GetElementAsync(Tester.BY_XPATH, ""//*[@id='MyInput']"");\par
tester.ConsoleMsg(""TYPE: "" + element.Type);\par
    }",

@"{\rtf1\ansi\ansicpg1251\deff0\nouicompat\deflang1049{\fonttbl{\f0\fnil\fcharset0 Calibri;}{\f1\fnil\fcharset204 Calibri;}}
{\colortbl ;\red0\green77\blue187;\red155\green0\blue211;\red0\green0\blue0;}
{\*\generator Riched20 10.0.22000}\viewkind4\uc1 
\pard\sl276\slmult1\cf1\f0\fs20\lang9 ClickAsync\cf0\par
\cf2\f1\lang1049\'ce\'ef\'e8\'f1\'e0\'ed\'e8\'e5\cf0 : \'ec\'e5\'f2\'ee\'e4 \'e2\'fb\'ef\'ee\'eb\'ed\'ff\'e5\'f2 \'ed\'e0\'e6\'e0\'f2\'e8\'e5 \'ed\'e0 \'fd\'eb\'e5\'ec\'e5\'ed\'f2\par
\cf2\'d1\'e8\'ed\'f2\'e0\'ea\'f1\'e8\'f1\cf0 : \f0\lang1033 ClickAsync()\f1\lang1049\par
\cf3\par
\cf2\'cf\'f0\'e8\'ec\'e5\'f0\cf3 :\par
\cf0\f0\lang1033 await element.ClickAsync();\par
}",

@"{\rtf1\ansi\ansicpg1251\deff0\nouicompat\deflang1049{\fonttbl{\f0\fnil\fcharset0 Calibri;}{\f1\fnil\fcharset204 Calibri;}}
{\colortbl ;\red0\green77\blue187;\red155\green0\blue211;\red0\green0\blue0;}
{\*\generator Riched20 10.0.22000}\viewkind4\uc1 
\pard\sl276\slmult1\cf1\f0\fs20\lang9 GetAttributeAsync\cf0\par
\cf2\f1\lang1049\'ce\'ef\'e8\'f1\'e0\'ed\'e8\'e5\cf0 : \'ec\'e5\'f2\'ee\'e4 \'e2\'ee\'e7\'e2\'f0\'e0\'f9\'e0\'e5\'f2 \'e7\'ed\'e0\'f7\'e5\'ed\'e8\'e5 \'f3\'ea\'e0\'e7\'e0\'ed\'ed\'ee\'e3\'ee \'e0\'f2\'f0\'e8\'e1\'f3\'f2\'e0 \'e8\'e7 \'fd\'eb\'e5\'ec\'e5\'ed\'f2\'e5\par
\cf2\'d1\'e8\'ed\'f2\'e0\'ea\'f1\'e8\'f1\cf0 : \f0\lang1033 GetAttributeAsync(string name)\f1\lang1049\par
\cf3\par
\cf2\'cf\'f0\'e8\'ec\'e5\'f0\cf3 :\par
\cf0\f0\lang1033 string attrClass = await element.GetAttributeAsync(""class"");\par
}",

@"{\rtf1\ansi\ansicpg1251\deff0\nouicompat\deflang1049{\fonttbl{\f0\fnil\fcharset0 Calibri;}{\f1\fnil\fcharset204 Calibri;}}
{\colortbl ;\red0\green77\blue187;\red155\green0\blue211;\red0\green0\blue0;}
{\*\generator Riched20 10.0.22000}\viewkind4\uc1 
\pard\sl276\slmult1\cf1\f0\fs20\lang9 GetHtmlAsync\cf0\par
\cf2\f1\lang1049\'ce\'ef\'e8\'f1\'e0\'ed\'e8\'e5\cf0 : \'ec\'e5\'f2\'ee\'e4 \'e2\'ee\'e7\'e2\'f0\'e0\'f9\'e0\'e5\'f2 html \'ef\'f0\'e5\'e4\'f1\'f2\'e0\'e2\'eb\'e5\'ed\'e8\'e5 \'fd\'eb\'e5\'ec\'e5\'ed\'f2\'e0 \'e2 \'f1\'f2\'f0\'ee\'f7\'ed\'ee\'ec \'e2\'fb\'f0\'e0\'e6\'e5\'ed\'e8\'e8\par
\cf2\'d1\'e8\'ed\'f2\'e0\'ea\'f1\'e8\'f1\cf0 : \f0\lang1033 GetHtmlAsync()\f1\lang1049\par
\cf3\par
\cf2\'cf\'f0\'e8\'ec\'e5\'f0\cf3 :\par
\cf0\f0\lang1033 string html = await element.GetHtmlAsync();\par
}",

@"{\rtf1\ansi\ansicpg1251\deff0\nouicompat\deflang1049{\fonttbl{\f0\fnil\fcharset0 Calibri;}{\f1\fnil\fcharset204 Calibri;}}
{\colortbl ;\red0\green77\blue187;\red155\green0\blue211;\red0\green0\blue0;}
{\*\generator Riched20 10.0.22000}\viewkind4\uc1 
\pard\sl276\slmult1\cf1\f0\fs20\lang9 GetTextAsync\cf0\par
\cf2\f1\lang1049\'ce\'ef\'e8\'f1\'e0\'ed\'e8\'e5\cf0 : \'ec\'e5\'f2\'ee\'e4 \'e2\'ee\'e7\'e2\'f0\'e0\'f9\'e0\'e5\'f2 \'f2\'e5\'ea\'f1\'f2 \'e8\'e7 \'fd\'eb\'e5\'ec\'e5\'ed\'f2\'e0\par
\cf2\'d1\'e8\'ed\'f2\'e0\'ea\'f1\'e8\'f1\cf0 : \f0\lang1033 GetTextAsync()\f1\lang1049\par
\cf3\par
\cf2\'cf\'f0\'e8\'ec\'e5\'f0\cf3 :\par
\cf0\f0\lang1033 string text = await element.GetTextAsync();\par
}",

@"{\rtf1\ansi\ansicpg1251\deff0\nouicompat\deflang1049{\fonttbl{\f0\fnil\fcharset0 Calibri;}{\f1\fnil\fcharset204 Calibri;}}
{\colortbl ;\red0\green77\blue187;\red155\green0\blue211;\red0\green0\blue0;}
{\*\generator Riched20 10.0.22000}\viewkind4\uc1 
\pard\sl276\slmult1\cf1\f0\fs20\lang9 GetValueAsync\cf0\par
\cf2\f1\lang1049\'ce\'ef\'e8\'f1\'e0\'ed\'e8\'e5\cf0 : \'ec\'e5\'f2\'ee\'e4 \'e2\'ee\'e7\'e2\'f0\'e0\'f9\'e0\'e5\'f2 \'e7\'ed\'e0\'f7\'e5\'ed\'e8\'e5 \'e8\'e7 \'fd\'eb\'e5\'ec\'e5\'ed\'f2\'e0\par
\cf2\'d1\'e8\'ed\'f2\'e0\'ea\'f1\'e8\'f1\cf0 : \f0\lang1033 GetValueAsync()\f1\lang1049\par
\cf3\par
\cf2\'cf\'f0\'e8\'ec\'e5\'f0\cf3 :\par
\cf0\f0\lang1033 string value = await element.GetValueAsync();\par
}",

@"{\rtf1\ansi\ansicpg1251\deff0\nouicompat\deflang1049{\fonttbl{\f0\fnil\fcharset0 Calibri;}{\f1\fnil\fcharset204 Calibri;}}
{\colortbl ;\red0\green77\blue187;\red155\green0\blue211;\red0\green0\blue0;}
{\*\generator Riched20 10.0.22000}\viewkind4\uc1 
\pard\sl276\slmult1\cf1\f0\fs20\lang9 ScrollToAsync\cf0\par
\cf2\f1\lang1049\'ce\'ef\'e8\'f1\'e0\'ed\'e8\'e5\cf0 : \'ec\'e5\'f2\'ee\'e4 \'e2\'fb\'ef\'ee\'eb\'ed\'ff\'e5\'f2 \'ef\'f0\'ee\'ea\'f0\'f3\'f2\'ea\'f3 \'ea \'fd\'eb\'e5\'ec\'e5\'ed\'f2\'f3 (\'ef\'e0\'f0\'e0\'ec\'e5\'f2\'f0 behaviorSmooth \'ee\'ef\'f0\'e5\'e4\'e5\'eb\'ff\'e5\'f2 \'ef\'eb\'e0\'e2\'ed\'ee\'f1\'f2\'fc \'ef\'f0\'ee\'ea\'f0\'f3\'f2\'ea\'e8)\par
\cf2\'d1\'e8\'ed\'f2\'e0\'ea\'f1\'e8\'f1\cf0 : \f0\lang1033 ScrollToAsync(bool behaviorSmooth = false)\f1\lang1049\par
\cf3\par
\cf2\'cf\'f0\'e8\'ec\'e5\'f0\cf3 :\par
\cf0\f0\lang1033 await element.ScrollToAsync();\par
await element.ScrollToAsync(true);\par
await element.ScrollToAsync(false);\par
}",

@"{\rtf1\ansi\ansicpg1251\deff0\nouicompat\deflang1049{\fonttbl{\f0\fnil\fcharset0 Calibri;}{\f1\fnil\fcharset204 Calibri;}}
{\colortbl ;\red0\green77\blue187;\red155\green0\blue211;\red0\green0\blue0;}
{\*\generator Riched20 10.0.22000}\viewkind4\uc1 
\pard\sl276\slmult1\cf1\f0\fs20\lang9 SetAttributeAsync\cf0\par
\cf2\f1\lang1049\'ce\'ef\'e8\'f1\'e0\'ed\'e8\'e5\cf0 : \'ec\'e5\'f2\'ee\'e4 \'e2\'f1\'f2\'e0\'e2\'eb\'ff\'e5\'f2 \'e7\'ed\'e0\'f7\'e5\'ed\'e8\'e5 \'f3\'ea\'e0\'e7\'e0\'ed\'ed\'ee\'e3\'ee \'e0\'f2\'f0\'e8\'e1\'f3\'f2\'e0 \'e2 \'fd\'eb\'e5\'ec\'e5\'ed\'f2\par
\cf2\'d1\'e8\'ed\'f2\'e0\'ea\'f1\'e8\'f1\cf0 : \f0\lang1033 SetAttributeAsync(string name, string value)\f1\lang1049\par
\cf3\par
\cf2\'cf\'f0\'e8\'ec\'e5\'f0\cf3 :\par
\cf0\f0\lang1033 await element.SetAttributeAsync(""class"", ""my-class"");\par
}",

@"{\rtf1\ansi\ansicpg1251\deff0\nouicompat\deflang1049{\fonttbl{\f0\fnil\fcharset0 Calibri;}{\f1\fnil\fcharset204 Calibri;}}
{\colortbl ;\red0\green77\blue187;\red155\green0\blue211;\red0\green0\blue0;}
{\*\generator Riched20 10.0.22000}\viewkind4\uc1 
\pard\sl276\slmult1\cf1\f0\fs20\lang9 SetHtmlAsync\cf0\par
\cf2\f1\lang1049\'ce\'ef\'e8\'f1\'e0\'ed\'e8\'e5\cf0 : \'ec\'e5\'f2\'ee\'e4 \'e2\'f1\'f2\'e0\'e2\'eb\'ff\'e5\'f2 html \'e2 \'fd\'eb\'e5\'ec\'e5\'ed\'f2\par
\cf2\'d1\'e8\'ed\'f2\'e0\'ea\'f1\'e8\'f1\cf0 : \f0\lang1033 SetHtmlAsync(string html)\f1\lang1049\par
\cf3\par
\cf2\'cf\'f0\'e8\'ec\'e5\'f0\cf3 :\par
\cf0\f0\lang1033 await element.SetHtmlAsync(""<div>\f1\lang1049\'dd\'f2\'ee \'f2\'e5\'f1\'f2</div>"");\f0\lang1033\par
}",

@"{\rtf1\ansi\ansicpg1251\deff0\nouicompat\deflang1049{\fonttbl{\f0\fnil\fcharset0 Calibri;}{\f1\fnil\fcharset204 Calibri;}}
{\colortbl ;\red0\green77\blue187;\red155\green0\blue211;\red0\green0\blue0;}
{\*\generator Riched20 10.0.22000}\viewkind4\uc1 
\pard\sl276\slmult1\cf1\f0\fs20\lang9 SetTextAsync\cf0\par
\cf2\f1\lang1049\'ce\'ef\'e8\'f1\'e0\'ed\'e8\'e5\cf0 : \'ec\'e5\'f2\'ee\'e4 \'e2\'f1\'f2\'e0\'e2\'eb\'ff\'e5\'f2 \'f2\'e5\'ea\'f1\'f2 \'e2 \'fd\'eb\'e5\'ec\'e5\'ed\'f2\par
\cf2\'d1\'e8\'ed\'f2\'e0\'ea\'f1\'e8\'f1\cf0 : \f0\lang1033 SetTextAsync(string text)\f1\lang1049\par
\cf3\par
\cf2\'cf\'f0\'e8\'ec\'e5\'f0\cf3 :\par
\cf0\f0\lang1033 await element.SetTextAsync(""\f1\lang1049\'fd\'f2\'ee \'f2\'e5\'f1\'f2\'ee\'e2\'fb\'e9 \'f2\'e5\'ea\'f1\'f2"");\f0\lang1033\par
}",

@"{\rtf1\ansi\ansicpg1251\deff0\nouicompat\deflang1049{\fonttbl{\f0\fnil\fcharset0 Calibri;}{\f1\fnil\fcharset204 Calibri;}}
{\colortbl ;\red0\green77\blue187;\red155\green0\blue211;\red0\green0\blue0;}
{\*\generator Riched20 10.0.22000}\viewkind4\uc1 
\pard\sl276\slmult1\cf1\f0\fs20\lang9 SetValueAsync\cf0\par
\cf2\f1\lang1049\'ce\'ef\'e8\'f1\'e0\'ed\'e8\'e5\cf0 : \'ec\'e5\'f2\'ee\'e4 \'e2\'f1\'f2\'e0\'e2\'eb\'ff\'e5\'f2 \'e7\'ed\'e0\'f7\'e5\'ed\'e8\'e5 \'e2 \'fd\'eb\'e5\'ec\'e5\'ed\'f2\par
\cf2\'d1\'e8\'ed\'f2\'e0\'ea\'f1\'e8\'f1\cf0 : \f0\lang1033 SetValueAsync(string value)\f1\lang1049\par
\cf3\par
\cf2\'cf\'f0\'e8\'ec\'e5\'f0\cf3 :\par
\cf0\f0\lang1033 await element.SetValueAsync(""\f1\lang1049\'fd\'f2\'ee \'f2\'e5\'f1\'f2\'ee\'e2\'ee\'e5 \'e7\'ed\'e0\'f7\'e5\'ed\'e8\'e5"");\f0\lang1033\par
}",

@"{\rtf1\ansi\ansicpg1251\deff0\nouicompat\deflang1049{\fonttbl{\f0\fnil\fcharset0 Calibri;}{\f1\fnil\fcharset204 Calibri;}}
{\colortbl ;\red0\green77\blue187;\red155\green0\blue211;\red0\green0\blue0;}
{\*\generator Riched20 10.0.22000}\viewkind4\uc1 
\pard\sl276\slmult1\cf1\f0\fs20\lang9 WaitNotVisible\cf0\par
\cf2\f1\lang1049\'ce\'ef\'e8\'f1\'e0\'ed\'e8\'e5\cf0 : \'ec\'e5\'f2\'ee\'e4 \'e6\'e4\'e5\'f2 \'f3\'ea\'e0\'e7\'e0\'ed\'ed\'ee\'e5 \'ea\'ee\'eb\'e8\'f7\'e5\'f1\'f2\'e2\'ee \'f1\'e5\'ea\'f3\'ed\'e4 \'ef\'ee\'ea\'e0 \'fd\'eb\'e5\'ec\'e5\'ed\'f2 \'ef\'e5\'f0\'e5\'f1\'f2\'e0\'ed\'e5\'f2 \'ee\'f2\'ee\'e1\'f0\'e0\'e6\'e0\'f2\'fc\'f1\'ff\par
\cf2\'d1\'e8\'ed\'f2\'e0\'ea\'f1\'e8\'f1\cf0 : \f0\lang1033 WaitNotVisibleAsync(int sec)\f1\lang1049\par
\cf3\par
\cf2\'cf\'f0\'e8\'ec\'e5\'f0\cf3 :\par
\cf0\f0\lang1033 await element.WaitNotVisibleAsync(2);\par
}",

@"{\rtf1\ansi\ansicpg1251\deff0\nouicompat\deflang1049{\fonttbl{\f0\fnil\fcharset0 Calibri;}{\f1\fnil\fcharset204 Calibri;}}
{\colortbl ;\red0\green77\blue187;\red155\green0\blue211;\red0\green0\blue0;}
{\*\generator Riched20 10.0.22000}\viewkind4\uc1 
\pard\sl276\slmult1\cf1\f0\fs20\lang9 WaitVisibleAsync\cf0\par
\cf2\f1\lang1049\'ce\'ef\'e8\'f1\'e0\'ed\'e8\'e5\cf0 : \'ec\'e5\'f2\'ee\'e4 \'e6\'e4\'e5\'f2 \'ee\'f2\'ee\'e1\'f0\'e0\'e6\'e5\'ed\'e8\'e5 \'fd\'eb\'e5\'ec\'e5\'ed\'f2\'e0 \'f3\'ea\'e0\'e7\'e0\'ed\'ed\'ee\'e5 \'ea\'ee\'eb\'e8\'f7\'e5\'f1\'f2\'e2\'ee \'f1\'e5\'ea\'f3\'ed\'e4\par
\cf2\'d1\'e8\'ed\'f2\'e0\'ea\'f1\'e8\'f1\cf0 : \f0\lang1033 WaitVisibleAsync(int sec)\f1\lang1049\par
\cf3\par
\cf2\'cf\'f0\'e8\'ec\'e5\'f0\cf3 :\par
\cf0\f0\lang1033 await element.WaitVisibleAsync(2);\par
}",

@"{\rtf1\ansi\ansicpg1251\deff0\nouicompat\deflang1049{\fonttbl{\f0\fnil\fcharset0 Calibri;}{\f1\fnil\fcharset204 Calibri;}}
{\colortbl ;\red0\green77\blue187;\red155\green0\blue211;\red0\green0\blue0;}
{\*\generator Riched20 10.0.22000}\viewkind4\uc1 
\pard\sl276\slmult1\cf1\f0\fs20\lang9 BY_CSS\cf0\par
\cf2\f1\lang1049\'ce\'ef\'e8\'f1\'e0\'ed\'e8\'e5\cf0 : \'ea\'ee\'ed\'f1\'f2\'e0\'ed\'f2\'e0 \'ee\'e1\'ee\'e7\'ed\'e0\'f7\'e0\'e5\'f2 \'f2\'e8\'ef \'e2\'e2\'ee\'e4\'e8\'ec\'ee\'e3\'ee \'eb\'ee\'ea\'e0\'f2\'ee\'f0\'e0\par
\cf2\'d1\'e8\'ed\'f2\'e0\'ea\'f1\'e8\'f1\cf0 : \f0\lang1033 BY_CSS = ""BY_CSS""\f1\lang1049\par
\cf3\par
\cf2\'cf\'f0\'e8\'ec\'e5\'f0\cf3 :\par
\cf0\f0\lang1033 Tester.BY_CSS\par
}",

@"{\rtf1\ansi\ansicpg1251\deff0\nouicompat\deflang1049{\fonttbl{\f0\fnil\fcharset0 Calibri;}{\f1\fnil\fcharset204 Calibri;}}
{\colortbl ;\red0\green77\blue187;\red155\green0\blue211;\red0\green0\blue0;}
{\*\generator Riched20 10.0.22000}\viewkind4\uc1 
\pard\sl276\slmult1\cf1\f0\fs20\lang9 BY_XPATH\cf0\par
\cf2\f1\lang1049\'ce\'ef\'e8\'f1\'e0\'ed\'e8\'e5\cf0 : \'ea\'ee\'ed\'f1\'f2\'e0\'ed\'f2\'e0 \'ee\'e1\'ee\'e7\'ed\'e0\'f7\'e0\'e5\'f2 \'f2\'e8\'ef \'e2\'e2\'ee\'e4\'e8\'ec\'ee\'e3\'ee \'eb\'ee\'ea\'e0\'f2\'ee\'f0\'e0\par
\cf2\'d1\'e8\'ed\'f2\'e0\'ea\'f1\'e8\'f1\cf0 : \f0\lang1033 BY_XPATH = ""BY_XPATH""\f1\lang1049\par
\cf3\par
\cf2\'cf\'f0\'e8\'ec\'e5\'f0\cf3 :\par
\cf0\f0\lang1033 Tester.BY_XPATH\par
}",

@"{\rtf1\ansi\ansicpg1251\deff0\nouicompat\deflang1049{\fonttbl{\f0\fnil\fcharset0 Calibri;}{\f1\fnil\fcharset204 Calibri;}}
{\colortbl ;\red0\green77\blue187;\red155\green0\blue211;\red0\green0\blue0;}
{\*\generator Riched20 10.0.19041}\viewkind4\uc1 
\pard\sl276\slmult1\cf1\f0\fs20\lang9 RestGetAsync\cf0\par
\cf2\f1\lang1049\'ce\'ef\'e8\'f1\'e0\'ed\'e8\'e5\cf0 : \'ec\'e5\'f2\'ee\'e4 \'e2\'fb\'ef\'ee\'eb\'ed\'ff\'e5\'f2 Get Rest \'e7\'e0\'ef\'f0\'ee\'f1 \'e8 \'ef\'ee\'eb\'f3\'f7\'e0\'e5\'f2 \'f0\'e5\'e7\'f3\'eb\'fc\'f2\'e0\'f2 \'e2 \'f4\'ee\'f0\'ec\'e0\'f2\'e5 json\par
\cf2\'d1\'e8\'ed\'f2\'e0\'ea\'f1\'e8\'f1\cf0 : \f0\lang1033 RestGetAsync(string url, TimeSpan timeout, string charset = ""UTF-8"")\f1\lang1049\par
\cf3\par
\cf2\'cf\'f0\'e8\'ec\'e5\'f0\cf3 :\par
\cf0\f0\lang1033 string result = await tester.RestGetAsync(""https://jsonplaceholder.typicode.com/posts/1/"", TimeSpan.FromDays(1), ""UTF-8"");\par
tester.ConsoleMsg(result);\par
\par
\cf2\f1\lang1049\'c4\'e0\'ed\'ed\'fb\'e9 \'ec\'e5\'f2\'ee\'e4 \'e8\'f1\'ef\'ee\'eb\'fc\'e7\'f3\'e5\'f2 \'f1\'f2\'e0\'ed\'e4\'e0\'f0\'f2\'ed\'fb\'e9 \'ef\'ee\'e4\'f5\'ee\'e4\cf3 :\par
\cf0\f0\lang1033 using System.Net;\par
using System.Net.Http;\par
using System.Net.Http.Headers;\par
\par
Uri uri = new Uri(url);\par
HttpClient client = new HttpClient();\par
client.Timeout = TimeSpan.FromDays(1);\par
client.BaseAddress = uri;\par
client.DefaultRequestHeaders.Clear();\par
client.DefaultRequestHeaders.Accept.Clear();\par
client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue(""application/json""));\par
client.DefaultRequestHeaders.Add(""charset"", ""UTF-8"");\par
client.DefaultRequestHeaders.Add(""User-Agent"", userAgent);\par
HttpResponseMessage response = await client.GetAsync(url);\par
if (response.IsSuccessStatusCode)\par
\{\par
\tab return await response.Content.ReadAsStringAsync();\par
\}\par
}",

@"{\rtf1\ansi\ansicpg1251\deff0\nouicompat\deflang1049{\fonttbl{\f0\fnil\fcharset0 Calibri;}{\f1\fnil\fcharset204 Calibri;}}
{\colortbl ;\red0\green77\blue187;\red155\green0\blue211;\red0\green0\blue0;\red0\green0\blue255;}
{\*\generator Riched20 10.0.22000}\viewkind4\uc1 
\pard\sl276\slmult1\cf1\f0\fs20\lang9 RestGetBasicAuthAsync\cf0\par
\cf2\f1\lang1049\'ce\'ef\'e8\'f1\'e0\'ed\'e8\'e5\cf0 : \'ec\'e5\'f2\'ee\'e4 \'e2\'fb\'ef\'ee\'eb\'ed\'ff\'e5\'f2 \f0\lang1033 Get Rest \f1\lang1049\'e7\'e0\'ef\'f0\'ee\'f1 \'e8 \'ef\'ee\'eb\'f3\'f7\'e0\'e5\'f2 \'f0\'e5\'e7\'f3\'eb\'fc\'f2\'e0\'f2 \'e2 \'f4\'ee\'f0\'ec\'e0\'f2\'e5 \f0\lang1033 json\f1\lang1049\par
\cf2\'d1\'e8\'ed\'f2\'e0\'ea\'f1\'e8\'f1\cf0 : \f0\lang1033 RestGetBasicAuthAsync(string login, string pass, string url, TimeSpan timeout, string charset = ""UTF-8"")\f1\lang1049\par
\cf3\par
\cf2\'cf\'f0\'e8\'ec\'e5\'f0\cf3 :\par
\cf0\f0\lang1033 string result = await tester.RestGetBasicAuthAsync(""admin"", ""0000"", ""https://jsonplaceholder.typicode.com/posts/1/"", TimeSpan.FromDays(1), ""UTF-8"");\par
tester.ConsoleMsg(result);\par
\f1\lang1049\par
\cf2\'c4\'e0\'ed\'ed\'fb\'e9 \'ec\'e5\'f2\'ee\'e4 \'e8\'f1\'ef\'ee\'eb\'fc\'e7\'f3\'e5\'f2 \'f1\'f2\'e0\'ed\'e4\'e0\'f0\'f2\'ed\'fb\'e9 \'ef\'ee\'e4\'f5\'ee\'e4\cf3 :\par
\cf0\f0\lang1033 using System.Net;\par
using System.Net.Http;\par
using System.Net.Http.Headers;\par
\par
byte[] authToken = Encoding.ASCII.GetBytes($""\{login\}:\{pass\}"");\par
Uri uri = new Uri(url);\par
HttpClient client = new HttpClient();\par
client.Timeout = TimeSpan.FromDays(1);\par
client.BaseAddress = uri;\par
client.DefaultRequestHeaders.Clear();\par
client.DefaultRequestHeaders.Accept.Clear();\par
client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue(""application/json""));\par
client.DefaultRequestHeaders.Add(""charset"", ""UTF-8"");\par
client.DefaultRequestHeaders.Add(""User-Agent"", userAgent);\par
client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(""Basic"", Convert.ToBase64String(authToken));\par
HttpResponseMessage response = await client.GetAsync(url);\par
if (response.IsSuccessStatusCode)\par
\{\par
\tab return await response.Content.ReadAsStringAsync();\par
\}\f1\lang1049\par
}",

@"{\rtf1\ansi\ansicpg1251\deff0\nouicompat\deflang1049{\fonttbl{\f0\fnil\fcharset0 Calibri;}{\f1\fnil\fcharset204 Calibri;}}
{\colortbl ;\red0\green77\blue187;\red155\green0\blue211;\red0\green0\blue0;}
{\*\generator Riched20 10.0.22000}\viewkind4\uc1 
\pard\sl276\slmult1\cf1\f0\fs20\lang9 BrowserGoBackAsync\cf0\par
\cf2\f1\lang1049\'ce\'ef\'e8\'f1\'e0\'ed\'e8\'e5\cf0 : \'ec\'e5\'f2\'ee\'e4 \'ed\'e0\'e2\'e8\'e3\'e0\'f6\'e8\'e8 \'e1\'f0\'e0\'f3\'e7\'e5\'f0\'e0 \'e2\'fb\'ef\'ee\'eb\'ed\'ff\'e5\'f2 \'e4\'e5\'e9\'f1\'f2\'e2\'e8\'e5 \'ed\'e0\'e7\'e0\'e4 \'f1 \'f3\'ea\'e0\'e7\'e0\'ed\'ed\'fb\'ec \'ee\'e6\'e8\'e4\'e0\'ed\'e8\'e5\'ec \'e2 \'f1\'e5\'ea\'f3\'ed\'e4\'e0\'f5\par
\cf2\'d1\'e8\'ed\'f2\'e0\'ea\'f1\'e8\'f1\cf0 : \f0\lang1033 BrowserGoBackAsync(int sec, bool abortLoadAfterTime = false)\f1\lang1049\par
\cf3\par
\cf2\'cf\'f0\'e8\'ec\'e5\'f0\cf3 :\par
\cf0\f0\lang1033 Tester tester = new Tester(browserForm);\par
await tester.TestBeginAsync();\par
await tester.GoToUrlAsync(""https://www.yahoo.com/"", 5);\par
string currentUrl = await tester.GetUrlAsync();\par
await tester.AssertEqualsAsync(""https://www.yahoo.com/"", currentUrl);\par
\par
await tester.GoToUrlAsync(""https://yandex.ru/"", 5);\par
currentUrl = await tester.GetUrlAsync();\par
await tester.AssertEqualsAsync(""https://yandex.ru/"", currentUrl);\par
\tab\tab\par
await tester.BrowserGoBackAsync(10);\par
currentUrl = await tester.GetUrlAsync();\par
await tester.AssertEqualsAsync(""https://www.yahoo.com/"", currentUrl);\par
\par
await tester.BrowserGoForwardAsync(10);\par
currentUrl = await tester.GetUrlAsync();\par
await tester.AssertEqualsAsync(""https://yandex.ru/"", currentUrl);\par
await tester.TestEndAsync();\par
    }",

@"{\rtf1\ansi\ansicpg1251\deff0\nouicompat\deflang1049{\fonttbl{\f0\fnil\fcharset0 Calibri;}{\f1\fnil\fcharset204 Calibri;}}
{\colortbl ;\red0\green77\blue187;\red155\green0\blue211;\red0\green0\blue0;}
{\*\generator Riched20 10.0.22000}\viewkind4\uc1 
\pard\sl276\slmult1\cf1\f0\fs20\lang9 BrowserGoForwardAsync\cf0\par
\cf2\f1\lang1049\'ce\'ef\'e8\'f1\'e0\'ed\'e8\'e5\cf0 : \'ec\'e5\'f2\'ee\'e4 \'ed\'e0\'e2\'e8\'e3\'e0\'f6\'e8\'e8 \'e1\'f0\'e0\'f3\'e7\'e5\'f0\'e0 \'e2\'fb\'ef\'ee\'eb\'ed\'ff\'e5\'f2 \'e4\'e5\'e9\'f1\'f2\'e2\'e8\'e5 \'e2\'ef\'e5\'f0\'e5\'e4 \'f1 \'f3\'ea\'e0\'e7\'e0\'ed\'ed\'fb\'ec \'ee\'e6\'e8\'e4\'e0\'ed\'e8\'e5\'ec \'e2 \'f1\'e5\'ea\'f3\'ed\'e4\'e0\'f5\par
\cf2\'d1\'e8\'ed\'f2\'e0\'ea\'f1\'e8\'f1\cf0 : \f0\lang1033 BrowserGoForwardAsync(int sec, bool abortLoadAfterTime = false)\f1\lang1049\par
\cf3\par
\cf2\'cf\'f0\'e8\'ec\'e5\'f0\cf3 :\par
\cf0\f0\lang1033 Tester tester = new Tester(browserForm);\par
await tester.TestBeginAsync();\par
await tester.GoToUrlAsync(""https://www.yahoo.com/"", 5);\par
string currentUrl = await tester.GetUrlAsync();\par
await tester.AssertEqualsAsync(""https://www.yahoo.com/"", currentUrl);\par
\par
await tester.GoToUrlAsync(""https://yandex.ru/"", 5);\par
currentUrl = await tester.GetUrlAsync();\par
await tester.AssertEqualsAsync(""https://yandex.ru/"", currentUrl);\par
\tab\tab\par
await tester.BrowserGoBackAsync(10);\par
currentUrl = await tester.GetUrlAsync();\par
await tester.AssertEqualsAsync(""https://www.yahoo.com/"", currentUrl);\par
\par
await tester.BrowserGoForwardAsync(10);\par
currentUrl = await tester.GetUrlAsync();\par
await tester.AssertEqualsAsync(""https://yandex.ru/"", currentUrl);\par
await tester.TestEndAsync();\par
    }",

@"{\rtf1\ansi\ansicpg1251\deff0\nouicompat\deflang1049{\fonttbl{\f0\fnil\fcharset0 Calibri;}{\f1\fnil\fcharset204 Calibri;}}
{\colortbl ;\red0\green77\blue187;\red155\green0\blue211;\red0\green0\blue0;}
{\*\generator Riched20 10.0.22000}\viewkind4\uc1 
\pard\sl276\slmult1\cf1\f0\fs20\lang9 BrowserBasicAuthenticationAsync\cf0\par
\cf2\f1\lang1049\'ce\'ef\'e8\'f1\'e0\'ed\'e8\'e5\cf0 : \'ec\'e5\'f2\'ee\'e4 \'ed\'e0\'e2\'e8\'e3\'e0\'f6\'e8\'e8 \'e1\'f0\'e0\'f3\'e7\'e5\'f0\'e0 \'e2\'fb\'ef\'ee\'eb\'ed\'ff\'e5\'f2 \'e4\'e5\'e9\'f1\'f2\'e2\'e8\'e5 \'e2\'ef\'e5\'f0\'e5\'e4 \'f1 \'f3\'ea\'e0\'e7\'e0\'ed\'ed\'fb\'ec \'ee\'e6\'e8\'e4\'e0\'ed\'e8\'e5\'ec \'e2 \'f1\'e5\'ea\'f3\'ed\'e4\'e0\'f5\par
\cf2\'d1\'e8\'ed\'f2\'e0\'ea\'f1\'e8\'f1\cf0 : \f0\lang1033 BrowserBasicAuthenticationAsync(string user, string pass)\f1\lang1049\par
\cf3\par
\cf2\'cf\'f0\'e8\'ec\'e5\'f0\cf3 :\par
\cf0\f0\lang1033 Tester tester = new Tester(browserForm);\par
await tester.BrowserBasicAuthenticationAsync(""user"", ""pass"");\par
await tester.TestBeginAsync();\par
await tester.GoToUrlAsync(""http://test.ru/basic_auth.html"", 5);\par
await tester.TestEndAsync();\par
    }",

@"{\rtf1\ansi\ansicpg1251\deff0\nouicompat\deflang1049{\fonttbl{\f0\fnil\fcharset0 Calibri;}{\f1\fnil\fcharset204 Calibri;}}
{\colortbl ;\red0\green77\blue187;\red155\green0\blue211;\red0\green0\blue0;}
{\*\generator Riched20 10.0.22000}\viewkind4\uc1 
\pard\sl276\slmult1\cf1\f0\fs20\lang9 BrowserEnableSendMailAsync\cf0\par
\cf2\f1\lang1049\'ce\'ef\'e8\'f1\'e0\'ed\'e8\'e5\cf0 : \'ec\'e5\'f2\'ee\'e4 \'e2\'ea\'eb\'fe\'f7\'e0\'e5\'f2 \'ee\'ef\'f6\'e8\'fe \'ee\'f2\'ef\'f0\'e0\'e2\'ea\'e8 \'ee\'f2\'f7\'e5\'f2\'e0 \'ed\'e0 \'ef\'ee\'f7\'f2\'f3 \'e2 \'f1\'eb\'f3\'f7\'e0\'e5 \'ef\'f0\'ee\'e2\'e0\'eb\'e0 \'e2 \'f0\'e0\'e1\'ee\'f2\'e5 \'e0\'e2\'f2\'ee\'f2\'e5\'f1\'f2\'e0\par
\cf2\'d1\'e8\'ed\'f2\'e0\'ea\'f1\'e8\'f1\cf0 : \f0\lang1033 BrowserEnableSendMailAsync(bool byFailure = true, bool bySuccess = true)\f1\lang1049\par
\cf3\par
\cf2\'cf\'f0\'e8\'ec\'e5\'f0\cf3 :\par
\cf0\f0\lang1033 Tester tester = new Tester(browserForm);\par
await tester.BrowserEnableSendMailAsync(true, false); \f1\lang1049 // \'f2\'ee\'eb\'fc\'ea\'ee \'e2 \'f1\'eb\'f3\'f7\'e0\'e5 \'ef\'f0\'ee\'e2\'e0\'eb\'e0\par
\f0\lang1033 await tester.BrowserEnableSendMailAsync(false, true); \f1\lang1049 // \'f2\'ee\'eb\'fc\'ea\'ee \'e2 \'f1\'eb\'f3\'f7\'e0\'e5 \'f3\'f1\'ef\'e5\'f5\'e0\par
\f0\lang1033 await tester.BrowserEnableSendMailAsync();\f1\lang1049  // \'e2 \'ee\'e1\'ee\'e8\'f5 \'f1\'eb\'f3\'f7\'e0\'ff\'f5\par
\f0\lang1033 await tester.TestBeginAsync();\par
\f1\lang1049 ...\f0\lang1033\par
await tester.TestEndAsync();\par
}",

@"{\rtf1\ansi\ansicpg1251\deff0\nouicompat\deflang1049{\fonttbl{\f0\fnil\fcharset0 Calibri;}{\f1\fnil\fcharset204 Calibri;}}
{\colortbl ;\red0\green77\blue187;\red155\green0\blue211;\red0\green0\blue0;}
{\*\generator Riched20 10.0.22000}\viewkind4\uc1 
\pard\sl276\slmult1\cf1\f0\fs20\lang9 SelectOptionAsync\cf0\par
\cf2\f1\lang1049\'ce\'ef\'e8\'f1\'e0\'ed\'e8\'e5\cf0 : \'ec\'e5\'f2\'ee\'e4 \'e2\'fb\'e1\'e8\'f0\'e0\'e5\'f2 \f0\lang1033 option \f1\lang1049\'e8\'e7 \'fd\'eb\'e5\'ec\'e5\'ed\'f2\'e0 \f0\lang1033 select \f1\lang1049\'ef\'ee \'f3\'ea\'e0\'e7\'e0\'ed\'ed\'ee\'ec\'f3 \'e8\'ed\'e4\'e5\'ea\'f1\'f3, \'f2\'e5\'ea\'f1\'f2\'f3 \'e8\'eb\'e8 \'e7\'ed\'e0\'f7\'e5\'ed\'e8\'fe. \par
\cf2\'d1\'e8\'ed\'f2\'e0\'ea\'f1\'e8\'f1\cf0 : \f0\lang1033 SelectOptionAsync(string by, string value)\f1\lang1049\par
\cf3\par
\cf2\'cf\'f0\'e8\'ec\'e5\'f0\cf3 :\par
\cf0\f0\lang1033 Tester tester = new Tester(browserForm);\par
await tester.TestBeginAsync();\par
await tester.GoToUrlAsync(""https://somovstudio.github.io/test2.html"", 5);\par
HTMLElement element = await tester.GetElementAsync(Tester.BY_XPATH, ""//*[@id='MySelect']"");\par
await element.SelectOptionAsync(HTMLElement.BY_INDEX, ""2"");\par
await element.SelectOptionAsync(HTMLElement.BY_VALUE, ""Mobile"");\par
await element.SelectOptionAsync(HTMLElement.BY_TEXT, ""Other"");\par
string index = await element.GetOptionAsync(HTMLElement.BY_INDEX);\par
string text = await element.GetOptionAsync(HTMLElement.BY_TEXT);\par
string value = await element.GetOptionAsync(HTMLElement.BY_VALUE);\par
...\par
await tester.TestEndAsync();\par
    }",

@"{\rtf1\ansi\ansicpg1251\deff0\nouicompat\deflang1049{\fonttbl{\f0\fnil\fcharset0 Calibri;}{\f1\fnil\fcharset204 Calibri;}}
{\colortbl ;\red0\green77\blue187;\red155\green0\blue211;\red0\green0\blue0;}
{\*\generator Riched20 10.0.22000}\viewkind4\uc1 
\pard\sl276\slmult1\cf1\f0\fs20\lang9 GetOptionAsync\cf0\par
\cf2\f1\lang1049\'ce\'ef\'e8\'f1\'e0\'ed\'e8\'e5\cf0 : \'ec\'e5\'f2\'ee\'e4 \'e2\'ee\'e7\'e2\'f0\'e0\'f9\'e0\'e5\'f2 \'e8\'ed\'e4\'e5\'ea\'f1, \'f2\'e5\'ea\'f1\'f2 \'e8\'eb\'e8 \'e7\'ed\'e0\'f7\'e5\'ed\'e8\'e5 \'e2\'fb\'e1\'ee\'e0\'ed\'ed\'ee\'e9 \'ee\'ef\'f6\'e8\'e8\f0\lang1033  \f1\lang1049\'e2\'fb\'e1\'f0\'e0\'ed\'ed\'ee\'e9 \f0\lang1033 option \f1\lang1049\'e8\'e7 \'fd\'eb\'e5\'ec\'e5\'ed\'f2\'e0 \f0\lang1033 select\f1\lang1049\par
\cf2\'d1\'e8\'ed\'f2\'e0\'ea\'f1\'e8\'f1\cf0 : \f0\lang1033 GetOptionAsync(string by)\f1\lang1049\par
\cf3\par
\cf2\'cf\'f0\'e8\'ec\'e5\'f0\cf3 :\par
\cf0\f0\lang1033 Tester tester = new Tester(browserForm);\par
await tester.TestBeginAsync();\par
await tester.GoToUrlAsync(""https://somovstudio.github.io/test2.html"", 5);\par
HTMLElement element = await tester.GetElementAsync(Tester.BY_XPATH, ""//*[@id='MySelect']"");\par
await element.SelectOptionAsync(HTMLElement.BY_INDEX, ""2"");\par
await element.SelectOptionAsync(HTMLElement.BY_VALUE, ""Mobile"");\par
await element.SelectOptionAsync(HTMLElement.BY_TEXT, ""Other"");\par
string index = await element.GetOptionAsync(HTMLElement.BY_INDEX);\par
string text = await element.GetOptionAsync(HTMLElement.BY_TEXT);\par
string value = await element.GetOptionAsync(HTMLElement.BY_VALUE);\par
...\par
await tester.TestEndAsync();\par
    }",

@"{\rtf1\ansi\ansicpg1251\deff0\nouicompat\deflang1049{\fonttbl{\f0\fnil\fcharset0 Calibri;}{\f1\fnil\fcharset204 Calibri;}}
{\colortbl ;\red0\green77\blue187;\red155\green0\blue211;\red0\green0\blue0;}
{\*\generator Riched20 10.0.22000}\viewkind4\uc1 
\pard\sl276\slmult1\cf1\f0\fs20\lang9 BY_INDEX\cf0\par
\cf2\f1\lang1049\'ce\'ef\'e8\'f1\'e0\'ed\'e8\'e5\cf0 : \'ea\'ee\'ed\'f1\'f2\'e0\'ed\'f2\'e0 \'ee\'e1\'ee\'e7\'ed\'e0\'f7\'e0\'e5\'f2 \'f2\'e8\'ef \'ee\'e1\'f0\'e0\'e1\'e0\'f2\'fb\'e2\'e0\'e5\'ec\'ee\'e3\'ee \'e7\'ed\'e0\'f7\'e5\'ed\'e8\'ff\par
\cf2\'d1\'e8\'ed\'f2\'e0\'ea\'f1\'e8\'f1\cf0 : \f0\lang1033 BY_INDEX = ""BY_INDEX""\f1\lang1049\par
\cf3\par
\cf2\'cf\'f0\'e8\'ec\'e5\'f0\cf3 :\par
\cf0\f0\lang1033 HTMLElement.BY_INDEX\par
}",

@"{\rtf1\ansi\ansicpg1251\deff0\nouicompat\deflang1049{\fonttbl{\f0\fnil\fcharset0 Calibri;}{\f1\fnil\fcharset204 Calibri;}}
{\colortbl ;\red0\green77\blue187;\red155\green0\blue211;\red0\green0\blue0;}
{\*\generator Riched20 10.0.22000}\viewkind4\uc1 
\pard\sl276\slmult1\cf1\f0\fs20\lang9 BY_TEXT\cf0\par
\cf2\f1\lang1049\'ce\'ef\'e8\'f1\'e0\'ed\'e8\'e5\cf0 : \'ea\'ee\'ed\'f1\'f2\'e0\'ed\'f2\'e0 \'ee\'e1\'ee\'e7\'ed\'e0\'f7\'e0\'e5\'f2 \'f2\'e8\'ef \'ee\'e1\'f0\'e0\'e1\'e0\'f2\'fb\'e2\'e0\'e5\'ec\'ee\'e3\'ee \'e7\'ed\'e0\'f7\'e5\'ed\'e8\'ff\par
\cf2\'d1\'e8\'ed\'f2\'e0\'ea\'f1\'e8\'f1\cf0 : \f0\lang1033 BY_TEXT = ""BY_TEXT""\f1\lang1049\par
\cf3\par
\cf2\'cf\'f0\'e8\'ec\'e5\'f0\cf3 :\par
\cf0\f0\lang1033 HTMLElement.BY_TEXT\par
}
 ",

@"{\rtf1\ansi\ansicpg1251\deff0\nouicompat\deflang1049{\fonttbl{\f0\fnil\fcharset0 Calibri;}{\f1\fnil\fcharset204 Calibri;}}
{\colortbl ;\red0\green77\blue187;\red155\green0\blue211;\red0\green0\blue0;}
{\*\generator Riched20 10.0.22000}\viewkind4\uc1 
\pard\sl276\slmult1\cf1\f0\fs20\lang9 BY_VALUE\cf0\par
\cf2\f1\lang1049\'ce\'ef\'e8\'f1\'e0\'ed\'e8\'e5\cf0 : \'ea\'ee\'ed\'f1\'f2\'e0\'ed\'f2\'e0 \'ee\'e1\'ee\'e7\'ed\'e0\'f7\'e0\'e5\'f2 \'f2\'e8\'ef \'ee\'e1\'f0\'e0\'e1\'e0\'f2\'fb\'e2\'e0\'e5\'ec\'ee\'e3\'ee \'e7\'ed\'e0\'f7\'e5\'ed\'e8\'ff\par
\cf2\'d1\'e8\'ed\'f2\'e0\'ea\'f1\'e8\'f1\cf0 : \f0\lang1033 BY_VALUE = ""BY_VALUE""\f1\lang1049\par
\cf3\par
\cf2\'cf\'f0\'e8\'ec\'e5\'f0\cf3 :\par
\cf0\f0\lang1033 HTMLElement.BY_VALUE\par
}",

@"{\rtf1\ansi\ansicpg1251\deff0\nouicompat\deflang1049{\fonttbl{\f0\fnil\fcharset0 Calibri;}{\f1\fnil\fcharset204 Calibri;}}
{\colortbl ;\red0\green77\blue187;\red155\green0\blue211;\red0\green0\blue0;}
{\*\generator Riched20 10.0.22000}\viewkind4\uc1 
\pard\sl276\slmult1\cf1\f0\fs20\lang9 IsClickableAsync\cf0\par
\cf2\f1\lang1049\'ce\'ef\'e8\'f1\'e0\'ed\'e8\'e5\cf0 : \'ec\'e5\'f2\'ee\'e4 \'ee\'ef\'f0\'e5\'e4\'e5\'eb\'ff\'e5\'f2 \'ea\'eb\'e8\'ea\'e0\'e1\'e5\'eb\'fc\'ed\'ee\'f1\'f2\'fc \'fd\'eb\'e5\'ec\'e5\'ed\'f2\'e0 \'e8 \'e2\'ee\'e7\'e2\'f0\'e0\'f9\'e0\'e5\'f2 true \'e8\'eb\'e8 false\par
\cf2\'d1\'e8\'ed\'f2\'e0\'ea\'f1\'e8\'f1\cf0 : \f0\lang1033 IsClickableAsync()\f1\lang1049\par
\cf3\par
\cf2\'cf\'f0\'e8\'ec\'e5\'f0\cf3 :\par
\cf0\f0\lang1033 Tester tester = new Tester(browserForm);\par
await tester.TestBeginAsync();\par
await tester.GoToUrlAsync(""https://somovstudio.github.io/test.html"", 5);\par
HTMLElement element = await tester.GetElementAsync(Tester.BY_XPATH, ""//*[@id='buttonLogin']"");\par
bool clickable = await element.IsClickableAsync();\par
await tester.AssertTrueAsync(clickable);\par
await tester.TestEndAsync();\par
    }",

@"{\rtf1\ansi\ansicpg1251\deff0\nouicompat\deflang1049{\fonttbl{\f0\fnil\fcharset0 Calibri;}{\f1\fnil\fcharset204 Calibri;}}
{\colortbl ;\red0\green77\blue187;\red155\green0\blue211;\red0\green0\blue0;}
{\*\generator Riched20 10.0.22000}\viewkind4\uc1 
\pard\sl276\slmult1\cf1\f0\fs20\lang9 IsClickableElementAsync\cf0\par
\cf2\f1\lang1049\'ce\'ef\'e8\'f1\'e0\'ed\'e8\'e5\cf0 : \'ec\'e5\'f2\'ee\'e4 \'ee\'ef\'f0\'e5\'e4\'e5\'eb\'ff\'e5\'f2 \'ea\'eb\'e8\'ea\'e0\'e1\'e5\'eb\'fc\'ed\'ee\'f1\'f2\'fc \'fd\'eb\'e5\'ec\'e5\'ed\'f2\'e0 \'e8 \'e2\'ee\'e7\'e2\'f0\'e0\'f9\'e0\'e5\'f2 true \'e8\'eb\'e8 false\par
\cf2\'d1\'e8\'ed\'f2\'e0\'ea\'f1\'e8\'f1\cf0 : \f0\lang1033 IsClickableElementAsync(string by, string locator)\f1\lang1049\par
\cf3\par
\cf2\'cf\'f0\'e8\'ec\'e5\'f0\cf3 :\par
\cf0\f0\lang1033 Tester tester = new Tester(browserForm);\par
await tester.TestBeginAsync();\par
await tester.GoToUrlAsync(""https://somovstudio.github.io/test.html"", 5);\par
bool clickable = await tester.IsClickableElementAsync(Tester.BY_XPATH, ""//*[@id='buttonLogin']"");\par
await tester.AssertTrueAsync(clickable);\par
await tester.TestEndAsync();\par
    }",

@"{\rtf1\ansi\ansicpg1251\deff0\nouicompat\deflang1049{\fonttbl{\f0\fnil\fcharset0 Calibri;}{\f1\fnil\fcharset204 Calibri;}}
{\colortbl ;\red0\green77\blue187;\red155\green0\blue211;\red0\green0\blue0;}
{\*\generator Riched20 10.0.22000}\viewkind4\uc1 
\pard\sl276\slmult1\cf1\f0\fs20\lang9 GetFrameAsync\cf0\par
\cf2\f1\lang1049\'ce\'ef\'e8\'f1\'e0\'ed\'e8\'e5\cf0 : \'ec\'e5\'f2\'ee\'e4 \'e2\'ee\'e7\'e2\'f0\'e0\'f9\'e0\'e5\'f2 \'fd\'eb\'e5\'ec\'e5\'ed\'f2 \'e2 \'e2\'e8\'e4\'e5 \'ee\'e1\'fa\'e5\'ea\'f2 \'ea\'eb\'e0\'f1\'f1 \'ea\'ee\'f2\'ee\'f0\'ee\'e3\'ee FRAMEElement\par
\cf2\'d1\'e8\'ed\'f2\'e0\'ea\'f1\'e8\'f1\cf0 : \f0\lang1033 GetFrameAsync(int index)\f1\lang1049\par
\cf3\par
\cf2\'cf\'f0\'e8\'ec\'e5\'f0\cf3 :\par
\cf0\f0\lang1033 Tester tester = new Tester(browserForm);\par
await tester.TestBeginAsync();\par
await tester.GoToUrlAsync(""https://somovstudio.github.io/test2.html"", 5);\par
\par
FRAMEElement frame = await tester.GetFrameAsync(0);\par
tester.ConsoleMsg(""Index: "" + frame.Index);\par
tester.ConsoleMsg(""Name: "" + frame.Name);\par
\par
string name = await frame.GetAttributeFromElementAsync(Tester.BY_XPATH, ""//input[@id='login']"", ""name"");\par
\par
List<string> values = await frame.GetAttributeFromElementsAsync(Tester.BY_XPATH, ""//input"", ""name"");\par
if (values != null)\par
\{\par
\tab foreach (string attr in values)\par
\tab\tab tester.ConsoleMsg(attr);\par
\}\par
\par
await frame.SetAttributeInElementAsync(Tester.BY_XPATH, ""//input[@id='buttonLogin']"", ""name"", ""NameButtonLogin"");\par
\par
await frame.SetAttributeInElementsAsync(Tester.BY_XPATH, ""//input"", ""name"", ""test"");\par
\par
await frame.SetValueInElementAsync(Tester.BY_XPATH, ""//input[@id='login']"", ""\f1\lang1049\'d2\'e5\'f1\'f2\'e8\'f0\'ee\'e2\'f9\'e8\'ea"");\par
string value = await frame.GetValueFromElementAsync(Tester.BY_XPATH, ""//input[@id='login']"");\par
\par
await frame.ClickElementAsync(Tester.BY_XPATH, ""//*[@id='buttonLogin']"");\par
\par
bool result = await frame.IsClickableElementAsync(Tester.BY_XPATH, ""//*[@id='buttonLogin']"");\par
\par
await frame.ScrollToElementAsync(Tester.BY_XPATH, ""//*[@id='buttonLogin']"", true);\par
\par
int result = await frame.GetCountElementsAsync(Tester.BY_XPATH, ""//input"");\par
\par
string html = await frame.GetHtmlFromElementAsync(Tester.BY_XPATH, ""//*[@id='buttonLogin']"");\par
\par
await frame.SetHtmlInElementAsync(Tester.BY_XPATH, ""//*[@id='auth']/h2"", ""<h2>\'d2\'e5\'f1\'f2\'ee\'e2\'fb\'e9 \'e7\'e0\'e3\'ee\'eb\'ee\'e2\'ee\'ea</h2>"");\par
\par
await frame.WaitNotVisibleElementAsync(Tester.BY_XPATH, ""//*[@id='result']"", 5);\par
await frame.ClickElementAsync(Tester.BY_XPATH, ""//*[@id='buttonLogin']"");\par
await frame.WaitVisibleElementAsync(Tester.BY_XPATH, ""//*[@id='result']"", 5);\par
\par
bool result = await frame.FindElementAsync(Tester.BY_XPATH, ""//*[@id='result']"", 5);\par
\par
bool result = await frame.FindVisibleElementAsync(Tester.BY_XPATH, ""//*[@id='result']"", 5);\par
\par
string title = await frame.GetTitleAsync();\par
\par
string url = await frame.GetUrlAsync();\par
\par
await frame.SetTextInElementAsync(Tester.BY_XPATH, ""//*[@id='auth']/h2"", ""\'dd\'f2\'ee \'f2\'e5\'f1\'f2"");\par
string text = await frame.GetTextFromElementAsync(Tester.BY_XPATH, ""//*[@id='auth']/h2"");\par
\par
await frame.SelectOptionAsync(Tester.BY_XPATH, ""//*[@id='MySelect']"", FRAMEElement.BY_INDEX, ""2"");\par
await frame.SelectOptionAsync(Tester.BY_XPATH, ""//*[@id='MySelect']"", FRAMEElement.BY_VALUE, ""Mobile"");\par
await frame.SelectOptionAsync(Tester.BY_XPATH, ""//*[@id='MySelect']"", FRAMEElement.BY_TEXT, ""Other"");\par
\par
string index = await frame.GetOptionAsync(Tester.BY_XPATH, ""//*[@id='MySelect']"", FRAMEElement.BY_INDEX);\par
string value = await frame.GetOptionAsync(Tester.BY_XPATH, ""//*[@id='MySelect']"", FRAMEElement.BY_VALUE);\par
string text = await frame.GetOptionAsync(Tester.BY_XPATH, ""//*[@id='MySelect']"", FRAMEElement.BY_TEXT);\f0\lang1033\par
}
",

@"{\rtf1\ansi\ansicpg1251\deff0\nouicompat\deflang1049{\fonttbl{\f0\fnil\fcharset0 Calibri;}{\f1\fnil\fcharset204 Calibri;}}
{\colortbl ;\red0\green77\blue187;\red155\green0\blue211;\red0\green0\blue0;}
{\*\generator Riched20 10.0.22000}\viewkind4\uc1 
\pard\sl276\slmult1\cf1\f0\fs20\lang9 FRAMEElement\cf0\par
\cf2\f1\lang1049\'ce\'ef\'e8\'f1\'e0\'ed\'e8\'e5\cf0 : \'e2\'f1\'ef\'ee\'ec\'ee\'e3\'e0\'f2\'e5\'eb\'fc\'ed\'fb\'e9 \'ea\'eb\'e0\'f1\'f1 \'e4\'eb\'ff \'f0\'e0\'e1\'ee\'f2\'fb \'f1 \'f4\'f0\'e5\'e9\'ec\'ee\'ec \'f3\'ea\'e0\'e7\'e0\'ed\'ed\'ee\'e3\'ee \'e8\'ed\'e4\'e5\'ea\'f1\'e0\par
\cf2\'d1\'e8\'ed\'f2\'e0\'ea\'f1\'e8\'f1\cf0 : \f0\lang1033 FRAMEElement(Tester tester, int index)\f1\lang1049\par
\cf3\par
\cf2\'cf\'f0\'e8\'ec\'e5\'f0 \'ef\'ee\'eb\'f3\'f7\'e5\'ed\'e8\'ff \'ee\'e1\'fa\'e5\'ea\'f2\'e0\cf3 :\par
\cf0\f0\lang1033 FRAMEElement frame = await tester.GetFrameAsync(0);\par
tester.ConsoleMsg(""Index: "" + frame.Index);\par
tester.ConsoleMsg(""Name: "" + frame.Name);\par
\par
    }",

@"{\rtf1\ansi\ansicpg1251\deff0\nouicompat\deflang1049{\fonttbl{\f0\fnil\fcharset0 Calibri;}{\f1\fnil\fcharset204 Calibri;}}
{\colortbl ;\red0\green77\blue187;\red155\green0\blue211;\red0\green0\blue0;}
{\*\generator Riched20 10.0.22000}\viewkind4\uc1 
\pard\sl276\slmult1\cf1\f0\fs20\lang9 BY_INDEX\cf0\par
\cf2\f1\lang1049\'ce\'ef\'e8\'f1\'e0\'ed\'e8\'e5\cf0 : \'ea\'ee\'ed\'f1\'f2\'e0\'ed\'f2\'e0 \'ee\'e1\'ee\'e7\'ed\'e0\'f7\'e0\'e5\'f2 \'f2\'e8\'ef \'ee\'e1\'f0\'e0\'e1\'e0\'f2\'fb\'e2\'e0\'e5\'ec\'ee\'e3\'ee \'e7\'ed\'e0\'f7\'e5\'ed\'e8\'ff\par
\cf2\'d1\'e8\'ed\'f2\'e0\'ea\'f1\'e8\'f1\cf0 : \f0\lang1033 BY_INDEX = ""BY_INDEX""\f1\lang1049\par
\cf3\par
\cf2\'cf\'f0\'e8\'ec\'e5\'f0\cf3 :\par
\cf0\f0\lang1033 FRAMEElement.BY_INDEX\par
}",

@"{\rtf1\ansi\ansicpg1251\deff0\nouicompat\deflang1049{\fonttbl{\f0\fnil\fcharset0 Calibri;}{\f1\fnil\fcharset204 Calibri;}}
{\colortbl ;\red0\green77\blue187;\red155\green0\blue211;\red0\green0\blue0;}
{\*\generator Riched20 10.0.22000}\viewkind4\uc1 
\pard\sl276\slmult1\cf1\f0\fs20\lang9 BY_TEXT\cf0\par
\cf2\f1\lang1049\'ce\'ef\'e8\'f1\'e0\'ed\'e8\'e5\cf0 : \'ea\'ee\'ed\'f1\'f2\'e0\'ed\'f2\'e0 \'ee\'e1\'ee\'e7\'ed\'e0\'f7\'e0\'e5\'f2 \'f2\'e8\'ef \'ee\'e1\'f0\'e0\'e1\'e0\'f2\'fb\'e2\'e0\'e5\'ec\'ee\'e3\'ee \'e7\'ed\'e0\'f7\'e5\'ed\'e8\'ff\par
\cf2\'d1\'e8\'ed\'f2\'e0\'ea\'f1\'e8\'f1\cf0 : \f0\lang1033 BY_TEXT = ""BY_TEXT""\f1\lang1049\par
\cf3\par
\cf2\'cf\'f0\'e8\'ec\'e5\'f0\cf3 :\par
\cf0\f0\lang1033 FRAMEElement.BY_TEXT\par
}",

@"{\rtf1\ansi\ansicpg1251\deff0\nouicompat\deflang1049{\fonttbl{\f0\fnil\fcharset0 Calibri;}{\f1\fnil\fcharset204 Calibri;}}
{\colortbl ;\red0\green77\blue187;\red155\green0\blue211;\red0\green0\blue0;}
{\*\generator Riched20 10.0.22000}\viewkind4\uc1 
\pard\sl276\slmult1\cf1\f0\fs20\lang9 BY_VALUE\cf0\par
\cf2\f1\lang1049\'ce\'ef\'e8\'f1\'e0\'ed\'e8\'e5\cf0 : \'ea\'ee\'ed\'f1\'f2\'e0\'ed\'f2\'e0 \'ee\'e1\'ee\'e7\'ed\'e0\'f7\'e0\'e5\'f2 \'f2\'e8\'ef \'ee\'e1\'f0\'e0\'e1\'e0\'f2\'fb\'e2\'e0\'e5\'ec\'ee\'e3\'ee \'e7\'ed\'e0\'f7\'e5\'ed\'e8\'ff\par
\cf2\'d1\'e8\'ed\'f2\'e0\'ea\'f1\'e8\'f1\cf0 : \f0\lang1033 BY_VALUE = ""BY_VALUE""\f1\lang1049\par
\cf3\par
\cf2\'cf\'f0\'e8\'ec\'e5\'f0\cf3 :\par
\cf0\f0\lang1033 FRAMEElement.BY_VALUE\par
}",

@"{\rtf1\ansi\ansicpg1251\deff0\nouicompat\deflang1049{\fonttbl{\f0\fnil\fcharset0 Calibri;}{\f1\fnil\fcharset204 Calibri;}}
{\colortbl ;\red0\green77\blue187;\red155\green0\blue211;\red0\green0\blue0;}
{\*\generator Riched20 10.0.22000}\viewkind4\uc1 
\pard\sl276\slmult1\cf1\f0\fs20\lang9 Name\cf0\par
\cf2\f1\lang1049\'ce\'ef\'e8\'f1\'e0\'ed\'e8\'e5\cf0 : \'ef\'e5\'f0\'e5\'ec\'e5\'ed\'ed\'e0\'ff \'e2\'ee\'e7\'e2\'f0\'e0\'f9\'e0\'e5\'f2 \'e8\'eb\'e8 \'ef\'ee\'eb\'f3\'f7\'e0\'e5\'f2 \'e8\'ec\'ff (Name) \'f4\'f0\'e5\'e9\'ec\'e0\par
\cf2\'d1\'e8\'ed\'f2\'e0\'ea\'f1\'e8\'f1\cf0 : \f0\lang1033 string Name \{ get; set; \}\f1\lang1049\par
\cf3\par
\cf2\'cf\'f0\'e8\'ec\'e5\'f0\cf3 :\par
\cf0\f0\lang1033 FRAMEElement frame = await tester.GetFrameAsync(0);\par
tester.ConsoleMsg(""Name: "" + frame.Name);\par
}",

@"{\rtf1\ansi\ansicpg1251\deff0\nouicompat\deflang1049{\fonttbl{\f0\fnil\fcharset0 Calibri;}{\f1\fnil\fcharset204 Calibri;}}
{\colortbl ;\red0\green77\blue187;\red155\green0\blue211;\red0\green0\blue0;}
{\*\generator Riched20 10.0.22000}\viewkind4\uc1 
\pard\sl276\slmult1\cf1\f0\fs20\lang9 Index\cf0\par
\cf2\f1\lang1049\'ce\'ef\'e8\'f1\'e0\'ed\'e8\'e5\cf0 : \'ef\'e5\'f0\'e5\'ec\'e5\'ed\'ed\'e0\'ff \'e2\'ee\'e7\'e2\'f0\'e0\'f9\'e0\'e5\'f2 \'e8\'eb\'e8 \'ef\'ee\'eb\'f3\'f7\'e0\'e5\'f2 \'e8\'ed\'e4\'e5\'ea\'f1 (Index) \'f4\'f0\'e5\'e9\'ec\'e0\par
\cf2\'d1\'e8\'ed\'f2\'e0\'ea\'f1\'e8\'f1\cf0 : \f0\lang1033 int Index \{ get; set; \}\f1\lang1049\par
\cf3\par
\cf2\'cf\'f0\'e8\'ec\'e5\'f0\cf3 :\par
\cf0\f0\lang1033 FRAMEElement frame = await tester.GetFrameAsync(0);\par
tester.ConsoleMsg(""Index: "" + frame.Index);\par
}",

@"{\rtf1\ansi\ansicpg1251\deff0\nouicompat\deflang1049{\fonttbl{\f0\fnil\fcharset0 Calibri;}{\f1\fnil\fcharset204 Calibri;}}
{\colortbl ;\red0\green77\blue187;\red155\green0\blue211;\red0\green0\blue0;}
{\*\generator Riched20 10.0.22000}\viewkind4\uc1 
\pard\sl276\slmult1\cf1\f0\fs20\lang9 ClickElementAsync\cf0\par
\cf2\f1\lang1049\'ce\'ef\'e8\'f1\'e0\'ed\'e8\'e5\cf0 : \'ec\'e5\'f2\'ee\'e4 \'e2\'fb\'ef\'ee\'eb\'ed\'ff\'e5\'f2 \'ed\'e0\'e6\'e0\'f2\'e8\'e5 \'ed\'e0 \'fd\'eb\'e5\'ec\'e5\'ed\'f2\par
\cf2\'d1\'e8\'ed\'f2\'e0\'ea\'f1\'e8\'f1\cf0 : \f0\lang1033 ClickElementAsync(string by, string locator)\f1\lang1049\par
\cf3\par
\cf2\'cf\'f0\'e8\'ec\'e5\'f0\cf3 :\par
\cf0\f0\lang1033 FRAMEElement frame = await tester.GetFrameAsync(0);\par
await frame.ClickElementAsync(Tester.BY_CSS, ""#auth #buttonLogin"");\par
\par
await frame.ClickElementAsync(Tester.BY_XPATH, ""//div[@id='auth']//input[@id='buttonLogin']"");\par
    }",

@"{\rtf1\ansi\ansicpg1251\deff0\nouicompat\deflang1049{\fonttbl{\f0\fnil\fcharset0 Calibri;}{\f1\fnil\fcharset204 Calibri;}}
{\colortbl ;\red0\green77\blue187;\red155\green0\blue211;\red0\green0\blue0;}
{\*\generator Riched20 10.0.22000}\viewkind4\uc1 
\pard\sl276\slmult1\cf1\f0\fs20\lang9 FindElementAsync\cf0\par
\cf2\f1\lang1049\'ce\'ef\'e8\'f1\'e0\'ed\'e8\'e5\cf0 : \'ec\'e5\'f2\'ee\'e4 \'e2\'fb\'ef\'ee\'eb\'ed\'ff\'e5\'f2 \'ef\'ee\'e8\'f1\'ea \'fd\'eb\'e5\'ec\'e5\'ed\'f2 \'e2 DOM \'f1 \'ee\'e6\'e8\'e4\'e0\'ed\'e8\'e5\'ec \'e2 \'f1\'e5\'ea\'f3\'ed\'e4\'e0\'f5 \'e8 \'e2\'ee\'e7\'e2\'f0\'e0\'f9\'e0\'e5\'f2 \'eb\'ee\'e3\'e8\'f7\'e5\'f1\'ea\'e8\'e9 \'f0\'e5\'e7\'f3\'eb\'fc\'f2\'e0\'f2 true \'e8\'eb\'e8 false\par
\cf2\'d1\'e8\'ed\'f2\'e0\'ea\'f1\'e8\'f1\cf0 : \f0\lang1033 FindElementAsync(string by, string locator, int sec)\f1\lang1049\par
\cf3\par
\cf2\'cf\'f0\'e8\'ec\'e5\'f0\cf3 :\par
\cf0\f0\lang1033 FRAMEElement frame = await tester.GetFrameAsync(0);\par
bool result = await frame.FindElementAsync(Tester.BY_CSS, ""div[id='result']"", 2);\par
\par
bool result = await frame.FindElementAsync(Tester.BY_XPATH, ""//div[@id='result']"", 2);\par
    }",

@"{\rtf1\ansi\ansicpg1251\deff0\nouicompat\deflang1049{\fonttbl{\f0\fnil\fcharset0 Calibri;}{\f1\fnil\fcharset204 Calibri;}}
{\colortbl ;\red0\green77\blue187;\red155\green0\blue211;\red0\green0\blue0;}
{\*\generator Riched20 10.0.22000}\viewkind4\uc1 
\pard\sl276\slmult1\cf1\f0\fs20\lang9 FindVisibleElementAsync\cf0\par
\cf2\f1\lang1049\'ce\'ef\'e8\'f1\'e0\'ed\'e8\'e5\cf0 : \'ec\'e5\'f2\'ee\'e4 \'e2\'fb\'ef\'ee\'eb\'ed\'ff\'e5\'f2 \'ef\'ee\'e8\'f1\'ea \'e2\'e8\'e7\'f3\'e0\'eb\'fc\'ed\'ee \'ee\'f2\'ee\'e1\'f0\'e0\'e6\'e0\'e5\'ec\'ee\'e3\'ee \'fd\'eb\'e5\'ec\'e5\'ed\'f2\'e0 \'f1 \'ee\'e6\'e8\'e4\'e0\'ed\'e8\'e5\'ec \'e2 \'f1\'e5\'ea\'f3\'ed\'e4\'e0\'f5 \'e8 \'e2\'ee\'e7\'e2\'f0\'e0\'f9\'e0\'e5\'f2 \'eb\'ee\'e3\'e8\'f7\'e5\'f1\'ea\'e8\'e9 \'f0\'e5\'e7\'f3\'eb\'fc\'f2\'e0\'f2 true \'e8\'eb\'e8 false\par
\cf2\'d1\'e8\'ed\'f2\'e0\'ea\'f1\'e8\'f1\cf0 : \f0\lang1033 FindVisibleElementAsync(string by, string locator, int sec)\f1\lang1049\par
\cf3\par
\cf2\'cf\'f0\'e8\'ec\'e5\'f0\cf3 :\par
\cf0\f0\lang1033 FRAMEElement frame = await tester.GetFrameAsync(0);\par
bool result = await frame.FindVisibleElementAsync(Tester.BY_CSS, ""#auth #buttonLogin"", 2);\par
\par
bool result = await frame.FindVisibleElementAsync(Tester.BY_XPATH, ""//div[@id='auth']//input[@id='buttonLogin']"", 2);\par
    }",

@"{\rtf1\ansi\ansicpg1251\deff0\nouicompat\deflang1049{\fonttbl{\f0\fnil\fcharset0 Calibri;}{\f1\fnil\fcharset204 Calibri;}}
{\colortbl ;\red0\green77\blue187;\red155\green0\blue211;\red0\green0\blue0;}
{\*\generator Riched20 10.0.22000}\viewkind4\uc1 
\pard\sl276\slmult1\cf1\f0\fs20\lang9 GetAttributeFromElementAsync\cf0\par
\cf2\f1\lang1049\'ce\'ef\'e8\'f1\'e0\'ed\'e8\'e5\cf0 : \'ec\'e5\'f2\'ee\'e4 \'e2\'ee\'e7\'e2\'f0\'e0\'f9\'e0\'e5\'f2 \'f1\'f2\'f0\'ee\'f7\'ed\'ee\'e5 \'e7\'ed\'e0\'f7\'e5\'ed\'e8\'e5 \'e8\'e7 \'f3\'ea\'e0\'e7\'e0\'ed\'ed\'ee\'e3\'ee \'e0\'f2\'f0\'e8\'e1\'f3\'f2\'e0 \'e2 \'e2\'fb\'e1\'f0\'e0\'ed\'ed\'ee\'ec \'fd\'eb\'e5\'ec\'e5\'ed\'f2\'e5\par
\cf2\'d1\'e8\'ed\'f2\'e0\'ea\'f1\'e8\'f1\cf0 : \f0\lang1033 GetAttributeFromElementAsync(string by, string locator, string attribute)\f1\lang1049\par
\cf3\par
\cf2\'cf\'f0\'e8\'ec\'e5\'f0\cf3 :\par
\cf0\f0\lang1033 FRAMEElement frame = await tester.GetFrameAsync(0);\par
string value = await frame.GetAttributeFromElementAsync(Tester.BY_CSS, ""input"", ""name"");\par
\par
string value = await frame.GetAttributeFromElementAsync(Tester.BY_XPATH, ""//input"", ""name"");\par
    }",

@"{\rtf1\ansi\ansicpg1251\deff0\nouicompat\deflang1049{\fonttbl{\f0\fnil\fcharset0 Calibri;}{\f1\fnil\fcharset204 Calibri;}}
{\colortbl ;\red0\green77\blue187;\red155\green0\blue211;\red0\green0\blue0;}
{\*\generator Riched20 10.0.22000}\viewkind4\uc1 
\pard\sl276\slmult1\cf1\f0\fs20\lang9 GetAttributeFromElementsAsync\cf0\par
\cf2\f1\lang1049\'ce\'ef\'e8\'f1\'e0\'ed\'e8\'e5\cf0 : \'ec\'e5\'f2\'ee\'e4 \'e2\'ee\'e7\'e2\'f0\'e0\'f9\'e0\'e5\'f2 \'f1\'ef\'e8\'f1\'ee\'ea \'e7\'ed\'e0\'f7\'e5\'ed\'e8\'e9 \'f3\'ea\'e0\'e7\'e0\'ed\'ed\'ee\'e3\'ee \'e0\'f2\'f0\'e8\'e1\'f3\'f2\'e0 \'e8\'e7 \'ec\'ed\'ee\'e6\'e5\'f1\'f2\'e2\'e0 \'fd\'eb\'e5\'ec\'e5\'ed\'f2\'ee\'e2\par
\cf2\'d1\'e8\'ed\'f2\'e0\'ea\'f1\'e8\'f1\cf0 : \f0\lang1033 GetAttributeFromElementsAsync(string by, string locator, string attribute)\f1\lang1049\par
\cf3\par
\cf2\'cf\'f0\'e8\'ec\'e5\'f0\cf3 :\par
\cf0\f0\lang1033 FRAMEElement frame = await tester.GetFrameAsync(0);\par
List<string> values = await frame.GetAttributeFromElementsAsync(Tester.BY_CSS, ""input"", ""name"");\par
if(values != null) \par
\{\par
\tab foreach (string attr in values)\par
\tab\tab tester.ConsoleMsg(attr);\par
\}\par
\par
\par
List<string> values = await frame.GetAttributeFromElementsAsync(Tester.BY_XPATH, ""//input"", ""name"");\par
if(values != null)\par
\{\par
\tab foreach (string attr in values)\par
\tab\tab tester.ConsoleMsg(attr);\par
\}\par
}",

@"{\rtf1\ansi\ansicpg1251\deff0\nouicompat\deflang1049{\fonttbl{\f0\fnil\fcharset0 Calibri;}{\f1\fnil\fcharset204 Calibri;}}
{\colortbl ;\red0\green77\blue187;\red155\green0\blue211;\red0\green0\blue0;}
{\*\generator Riched20 10.0.22000}\viewkind4\uc1 
\pard\sl276\slmult1\cf1\f0\fs20\lang9 GetCountElementsAsync\cf0\par
\cf2\f1\lang1049\'ce\'ef\'e8\'f1\'e0\'ed\'e8\'e5\cf0 : \'ec\'e5\'f2\'ee\'e4 \'e2\'ee\'e7\'e2\'f0\'e0\'f9\'e0\'e5\'f2 \'ea\'ee\'eb\'e8\'f7\'e5\'f1\'f2\'e2\'ee \'ed\'e0\'e9\'e4\'e5\'ed\'ed\'fb\'f5 \'fd\'eb\'e5\'ec\'e5\'ed\'f2\'ee\'e2\par
\cf2\'d1\'e8\'ed\'f2\'e0\'ea\'f1\'e8\'f1\cf0 : \f0\lang1033 GetCountElementsAsync(string by, string locator)\f1\lang1049\par
\cf3\par
\cf2\'cf\'f0\'e8\'ec\'e5\'f0\cf3 :\par
\cf0\f0\lang1033 FRAMEElement frame = await tester.GetFrameAsync(0);\par
int count = await frame.GetCountElementsAsync(Tester.BY_CSS, ""input"");\par
\par
int count = await frame.GetCountElementsAsync(Tester.BY_XPATH, ""//input"");\par
    }",

@"{\rtf1\ansi\ansicpg1251\deff0\nouicompat\deflang1049{\fonttbl{\f0\fnil\fcharset0 Calibri;}{\f1\fnil\fcharset204 Calibri;}}
{\colortbl ;\red0\green77\blue187;\red155\green0\blue211;\red0\green0\blue0;}
{\*\generator Riched20 10.0.22000}\viewkind4\uc1 
\pard\sl276\slmult1\cf1\f0\fs20\lang9 GetHtmlFromElementAsync\cf0\par
\cf2\f1\lang1049\'ce\'ef\'e8\'f1\'e0\'ed\'e8\'e5\cf0 : \'ec\'e5\'f2\'ee\'e4 \'e2\'ee\'e7\'e2\'f0\'e0\'f9\'e0\'e5\'f2 html \'ef\'f0\'e5\'e4\'f1\'f2\'e0\'e2\'eb\'e5\'ed\'e8\'e5 \'ee\'e1\'fa\'e5\'ea\'f2\'e0 \'e2 \'f1\'f2\'f0\'ee\'f7\'ed\'ee\'ec \'e2\'fb\'f0\'e0\'e6\'e5\'ed\'e8\'e8\par
\cf2\'d1\'e8\'ed\'f2\'e0\'ea\'f1\'e8\'f1\cf0 : \f0\lang1033 GetHtmlFromElementAsync(string by, string locator)\f1\lang1049\par
\cf3\par
\cf2\'cf\'f0\'e8\'ec\'e5\'f0\cf3 :\par
\cf0\f0\lang1033 FRAMEElement frame = await tester.GetFrameAsync(0);\par
string html = await frame.GetHtmlFromElementAsync(Tester.BY_CSS, ""#auth > h2"");\par
\par
string html = await frame.GetHtmlFromElementAsync(Tester.BY_XPATH, ""//div[@id='auth']//h2"");\par
    }",

@"{\rtf1\ansi\ansicpg1251\deff0\nouicompat\deflang1049{\fonttbl{\f0\fnil\fcharset0 Calibri;}{\f1\fnil\fcharset204 Calibri;}}
{\colortbl ;\red0\green77\blue187;\red155\green0\blue211;\red0\green0\blue0;}
{\*\generator Riched20 10.0.22000}\viewkind4\uc1 
\pard\sl276\slmult1\cf1\f0\fs20\lang9 GetOptionAsync\cf0\par
\cf2\f1\lang1049\'ce\'ef\'e8\'f1\'e0\'ed\'e8\'e5\cf0 : \'ec\'e5\'f2\'ee\'e4 \'e2\'ee\'e7\'e2\'f0\'e0\'f9\'e0\'e5\'f2 \'e8\'ed\'e4\'e5\'ea\'f1, \'f2\'e5\'ea\'f1\'f2 \'e8\'eb\'e8 \'e7\'ed\'e0\'f7\'e5\'ed\'e8\'e5 \'e2\'fb\'e1\'ee\'e0\'ed\'ed\'ee\'e9 \'ee\'ef\'f6\'e8\'e8 \'e2\'fb\'e1\'f0\'e0\'ed\'ed\'ee\'e9 option \'e8\'e7 \'fd\'eb\'e5\'ec\'e5\'ed\'f2\'e0 select\par
\cf2\'d1\'e8\'ed\'f2\'e0\'ea\'f1\'e8\'f1\cf0 : \f0\lang1033 GetOptionAsync(string by, string locator, string type)\f1\lang1049\par
\cf3\par
\cf2\'cf\'f0\'e8\'ec\'e5\'f0\cf3 :\par
\cf0\f0\lang1033 FRAMEElement frame = await tester.GetFrameAsync(0);\par
\par
string index = await frame.GetOptionAsync(Tester.BY_CSS, ""#MySelect"", FRAMEElement.BY_INDEX);\par
string value = await frame.GetOptionAsync(Tester.BY_CSS, ""#MySelect"", FRAMEElement.BY_VALUE);\par
string text = await frame.GetOptionAsync(Tester.BY_CSS, ""#MySelect"", FRAMEElement.BY_TEXT);\par
\par
string index = await frame.GetOptionAsync(Tester.BY_XPATH, ""//*[@id='MySelect']"", FRAMEElement.BY_INDEX);\par
string value = await frame.GetOptionAsync(Tester.BY_XPATH, ""//*[@id='MySelect']"", FRAMEElement.BY_VALUE);\par
string text = await frame.GetOptionAsync(Tester.BY_XPATH, ""//*[@id='MySelect']"", FRAMEElement.BY_TEXT);\par
    }",

@"{\rtf1\ansi\ansicpg1251\deff0\nouicompat\deflang1049{\fonttbl{\f0\fnil\fcharset0 Calibri;}{\f1\fnil\fcharset204 Calibri;}}
{\colortbl ;\red0\green77\blue187;\red155\green0\blue211;\red0\green0\blue0;}
{\*\generator Riched20 10.0.22000}\viewkind4\uc1 
\pard\sl276\slmult1\cf1\f0\fs20\lang9 GetTextFromElementAsync\cf0\par
\cf2\f1\lang1049\'ce\'ef\'e8\'f1\'e0\'ed\'e8\'e5\cf0 : \'ec\'e5\'f2\'ee\'e4 \'e2\'ee\'e7\'e2\'f0\'e0\'f9\'e0\'e5\'f2 \'f2\'e5\'ea\'f1\'f2 \'e8\'e7 \'f3\'ea\'e0\'e7\'e0\'ed\'ed\'ee\'e3\'ee \'fd\'eb\'e5\'ec\'e5\'ed\'f2\'e0\par
\cf2\'d1\'e8\'ed\'f2\'e0\'ea\'f1\'e8\'f1\cf0 : \f0\lang1033 GetTextFromElementAsync(string by, string locator)\f1\lang1049\par
\cf3\par
\cf2\'cf\'f0\'e8\'ec\'e5\'f0\cf3 :\par
\cf0\f0\lang1033 FRAMEElement frame = await tester.GetFrameAsync(0);\par
string text = await frame.GetTextFromElementAsync(Tester.BY_CSS, ""#auth > h2"");\par
\par
string text = await frame.GetTextFromElementAsync(Tester.BY_XPATH, ""//div[@id='auth']//h2"");\par
    }",

@"{\rtf1\ansi\ansicpg1251\deff0\nouicompat\deflang1049{\fonttbl{\f0\fnil\fcharset0 Calibri;}{\f1\fnil\fcharset204 Calibri;}}
{\colortbl ;\red0\green77\blue187;\red155\green0\blue211;\red0\green0\blue0;}
{\*\generator Riched20 10.0.22000}\viewkind4\uc1 
\pard\sl276\slmult1\cf1\f0\fs20\lang9 GetTitleAsync\cf0\par
\cf2\f1\lang1049\'ce\'ef\'e8\'f1\'e0\'ed\'e8\'e5\cf0 : \'ec\'e5\'f2\'ee\'e4 \'e2\'ee\'e7\'e2\'f0\'e0\'f9\'e0\'e5\'f2 \'e7\'e0\'e3\'ee\'eb\'ee\'e2\'ee\'ea \'f1\'f2\'f0\'e0\'ed\'e8\'f6\'fb\par
\cf2\'d1\'e8\'ed\'f2\'e0\'ea\'f1\'e8\'f1\cf0 : \f0\lang1033 GetTitleAsync()\f1\lang1049\par
\cf3\par
\cf2\'cf\'f0\'e8\'ec\'e5\'f0\cf3 :\par
\cf0\f0\lang1033 FRAMEElement frame = await tester.GetFrameAsync(0);\par
string title = await frame.GetTitleAsync();\par
}",

@"{\rtf1\ansi\ansicpg1251\deff0\nouicompat\deflang1049{\fonttbl{\f0\fnil\fcharset0 Calibri;}{\f1\fnil\fcharset204 Calibri;}}
{\colortbl ;\red0\green77\blue187;\red155\green0\blue211;\red0\green0\blue0;}
{\*\generator Riched20 10.0.22000}\viewkind4\uc1 
\pard\sl276\slmult1\cf1\f0\fs20\lang9 GetUrlAsync\cf0\par
\cf2\f1\lang1049\'ce\'ef\'e8\'f1\'e0\'ed\'e8\'e5\cf0 : \'ec\'e5\'f2\'ee\'e4 \'e2\'ee\'e7\'e2\'f0\'e0\'f9\'e0\'e5\'f2 \'f2\'e5\'ea\'f3\'f9\'e8\'e9 URL\par
\cf2\'d1\'e8\'ed\'f2\'e0\'ea\'f1\'e8\'f1\cf0 : \f0\lang1033 GetUrlAsync()\f1\lang1049\par
\cf3\par
\cf2\'cf\'f0\'e8\'ec\'e5\'f0\cf3 :\par
\cf0\f0\lang1033 FRAMEElement frame = await tester.GetFrameAsync(0);\par
string url = await frame.GetUrlAsync();\par
}",

@"{\rtf1\ansi\ansicpg1251\deff0\nouicompat\deflang1049{\fonttbl{\f0\fnil\fcharset0 Calibri;}{\f1\fnil\fcharset204 Calibri;}}
{\colortbl ;\red0\green77\blue187;\red155\green0\blue211;\red0\green0\blue0;}
{\*\generator Riched20 10.0.22000}\viewkind4\uc1 
\pard\sl276\slmult1\cf1\f0\fs20\lang9 GetValueFromElementAsync\cf0\par
\cf2\f1\lang1049\'ce\'ef\'e8\'f1\'e0\'ed\'e8\'e5\cf0 : \'ec\'e5\'f2\'ee\'e4 \'e2\'ee\'e7\'e2\'f0\'e0\'f9\'e0\'e5\'f2 \'e7\'ed\'e0\'f7\'e5\'ed\'e8\'e5 \'e8\'e7 \'f3\'ea\'e0\'e7\'e0\'ed\'ed\'ee\'e3\'ee \'fd\'eb\'e5\'ec\'e5\'ed\'f2\'e0\par
\cf2\'d1\'e8\'ed\'f2\'e0\'ea\'f1\'e8\'f1\cf0 : \f0\lang1033 GetValueFromElementAsync(string by, string locator)\f1\lang1049\par
\cf3\par
\cf2\'cf\'f0\'e8\'ec\'e5\'f0\cf3 :\par
\cf0\f0\lang1033 FRAMEElement frame = await tester.GetFrameAsync(0);\par
string value = await frame.GetValueFromElementAsync(Tester.BY_CSS, ""input[id='login']"");\par
\par
string value = await frame.GetValueFromElementAsync(Tester.BY_XPATH, ""//input[@id='login']"");\par
    }",

@"{\rtf1\ansi\ansicpg1251\deff0\nouicompat\deflang1049{\fonttbl{\f0\fnil\fcharset0 Calibri;}{\f1\fnil\fcharset204 Calibri;}}
{\colortbl ;\red0\green77\blue187;\red155\green0\blue211;\red0\green0\blue0;}
{\*\generator Riched20 10.0.22000}\viewkind4\uc1 
\pard\sl276\slmult1\cf1\f0\fs20\lang9 IsClickableElementAsync\cf0\par
\cf2\f1\lang1049\'ce\'ef\'e8\'f1\'e0\'ed\'e8\'e5\cf0 : \'ec\'e5\'f2\'ee\'e4 \'ee\'ef\'f0\'e5\'e4\'e5\'eb\'ff\'e5\'f2 \'ea\'eb\'e8\'ea\'e0\'e1\'e5\'eb\'fc\'ed\'ee\'f1\'f2\'fc \'fd\'eb\'e5\'ec\'e5\'ed\'f2\'e0 \'e8 \'e2\'ee\'e7\'e2\'f0\'e0\'f9\'e0\'e5\'f2 true \'e8\'eb\'e8 false\par
\cf2\'d1\'e8\'ed\'f2\'e0\'ea\'f1\'e8\'f1\cf0 : \f0\lang1033 IsClickableElementAsync(string by, string locator)\f1\lang1049\par
\cf3\par
\cf2\'cf\'f0\'e8\'ec\'e5\'f0\cf3 :\par
\cf0\f0\lang1033 FRAMEElement frame = await tester.GetFrameAsync(0);\par
bool clickable = await frame.IsClickableElementAsync(Tester.BY_CSS, ""#buttonLogin"");\par
await frame.AssertTrueAsync(clickable);\par
\par
bool clickable = await frame.IsClickableElementAsync(Tester.BY_XPATH, ""//*[@id='buttonLogin']"");\par
await frame.AssertTrueAsync(clickable);\par
\par
    }",

@"{\rtf1\ansi\ansicpg1251\deff0\nouicompat\deflang1049{\fonttbl{\f0\fnil\fcharset0 Calibri;}{\f1\fnil\fcharset204 Calibri;}}
{\colortbl ;\red0\green77\blue187;\red155\green0\blue211;\red0\green0\blue0;}
{\*\generator Riched20 10.0.22000}\viewkind4\uc1 
\pard\sl276\slmult1\cf1\f0\fs20\lang9 ScrollToElementAsync\cf0\par
\cf2\f1\lang1049\'ce\'ef\'e8\'f1\'e0\'ed\'e8\'e5\cf0 : \'ec\'e5\'f2\'ee\'e4 \'e2\'fb\'ef\'ee\'eb\'ed\'ff\'e5\'f2 \'ef\'f0\'ee\'ea\'f0\'f3\'f2\'ea\'f3 \'ea \'f3\'ea\'e0\'e7\'e0\'ed\'ed\'ee\'ec\'f3 \'fd\'eb\'e5\'ec\'e5\'ed\'f2\'f3 (\'ef\'e0\'f0\'e0\'ec\'e5\'f2\'f0 behaviorSmooth \'ee\'ef\'f0\'e5\'e4\'e5\'eb\'ff\'e5\'f2 \'ef\'eb\'e0\'e2\'ed\'ee\'f1\'f2\'fc \'ef\'f0\'ee\'ea\'f0\'f3\'f2\'ea\'e8)\par
\cf2\'d1\'e8\'ed\'f2\'e0\'ea\'f1\'e8\'f1\cf0 : \f0\lang1033 ScrollToElementAsync(string by, string locator, bool behaviorSmooth = false)\f1\lang1049\par
\cf3\par
\cf2\'cf\'f0\'e8\'ec\'e5\'f0\cf3 :\par
\cf0\f0\lang1033 FRAMEElement frame = await tester.GetFrameAsync(0);\par
await frame.ScrollToElementAsync(Tester.BY_CSS, ""body > footer"", true);\par
\par
await frame.ScrollToElementAsync(Tester.BY_XPATH, ""/html/body/footer"", true);\par
    }",

@"{\rtf1\ansi\ansicpg1251\deff0\nouicompat\deflang1049{\fonttbl{\f0\fnil\fcharset0 Calibri;}{\f1\fnil\fcharset204 Calibri;}}
{\colortbl ;\red0\green77\blue187;\red155\green0\blue211;\red0\green0\blue0;}
{\*\generator Riched20 10.0.22000}\viewkind4\uc1 
\pard\sl276\slmult1\cf1\f0\fs20\lang9 SelectOptionAsync\cf0\par
\cf2\f1\lang1049\'ce\'ef\'e8\'f1\'e0\'ed\'e8\'e5\cf0 : \'ec\'e5\'f2\'ee\'e4 \'e2\'fb\'e1\'e8\'f0\'e0\'e5\'f2 option \'e8\'e7 \'fd\'eb\'e5\'ec\'e5\'ed\'f2\'e0 select \'ef\'ee \'f3\'ea\'e0\'e7\'e0\'ed\'ed\'ee\'ec\'f3 \'e8\'ed\'e4\'e5\'ea\'f1\'f3, \'f2\'e5\'ea\'f1\'f2\'f3 \'e8\'eb\'e8 \'e7\'ed\'e0\'f7\'e5\'ed\'e8\'fe\par
\cf2\'d1\'e8\'ed\'f2\'e0\'ea\'f1\'e8\'f1\cf0 : \f0\lang1033 SelectOptionAsync(string by, string locator, string type, string value)\f1\lang1049\par
\cf3\par
\cf2\'cf\'f0\'e8\'ec\'e5\'f0\cf3 :\par
\cf0\f0\lang1033 FRAMEElement frame = await tester.GetFrameAsync(0);\par
await frame.SelectOptionAsync(Tester.BY_CSS, ""#MySelect"", FRAMEElement.BY_INDEX, ""2"");\par
await frame.SelectOptionAsync(Tester.BY_CSS, ""#MySelect"", FRAMEElement.BY_VALUE, ""Mobile"");\par
await frame.SelectOptionAsync(Tester.BY_CSS, ""#MySelect"", FRAMEElement.BY_TEXT, ""Other"");\par
\par
await frame.SelectOptionAsync(Tester.BY_XPATH, ""//*[@id='MySelect']"", FRAMEElement.BY_INDEX, ""2"");\par
await frame.SelectOptionAsync(Tester.BY_XPATH, ""//*[@id='MySelect']"", FRAMEElement.BY_VALUE, ""Mobile"");\par
await frame.SelectOptionAsync(Tester.BY_XPATH, ""//*[@id='MySelect']"", FRAMEElement.BY_TEXT, ""Other"");\par
    }",

@"{\rtf1\ansi\ansicpg1251\deff0\nouicompat\deflang1049{\fonttbl{\f0\fnil\fcharset0 Calibri;}{\f1\fnil\fcharset204 Calibri;}}
{\colortbl ;\red0\green77\blue187;\red155\green0\blue211;\red0\green0\blue0;}
{\*\generator Riched20 10.0.22000}\viewkind4\uc1 
\pard\sl276\slmult1\cf1\f0\fs20\lang9 SetAttributeInElementAsync\cf0\par
\cf2\f1\lang1049\'ce\'ef\'e8\'f1\'e0\'ed\'e8\'e5\cf0 : \'ec\'e5\'f2\'ee\'e4 \'e2\'f1\'f2\'e0\'e2\'eb\'ff\'e5\'f2 \'e0\'f2\'f0\'e8\'e1\'f3\'f2 \'f1\'ee \'e7\'ed\'e0\'f7\'e5\'ed\'e8\'e5\'ec \'e2 \'f3\'ea\'e0\'e7\'e0\'ed\'ed\'fb\'e9 \'fd\'eb\'e5\'ec\'e5\'ed\'f2\par
\cf2\'d1\'e8\'ed\'f2\'e0\'ea\'f1\'e8\'f1\cf0 : \f0\lang1033 SetAttributeInElementAsync(string by, string locator, string attribute, string value)\f1\lang1049\par
\cf3\par
\cf2\'cf\'f0\'e8\'ec\'e5\'f0\cf3 :\par
\cf0\f0\lang1033 FRAMEElement frame = await tester.GetFrameAsync(0);\par
await frame.SetAttributeInElementAsync(Tester.BY_CSS, ""#auth > h2"", ""name"", ""test"");\par
\par
await frame.SetAttributeInElementAsync(Tester.BY_XPATH, ""//div[@id='auth']//h2"", ""name"", ""test"");\par
    }",

@"{\rtf1\ansi\ansicpg1251\deff0\nouicompat\deflang1049{\fonttbl{\f0\fnil\fcharset0 Calibri;}{\f1\fnil\fcharset204 Calibri;}}
{\colortbl ;\red0\green77\blue187;\red155\green0\blue211;\red0\green0\blue0;}
{\*\generator Riched20 10.0.22000}\viewkind4\uc1 
\pard\sl276\slmult1\cf1\f0\fs20\lang9 SetAttributeInElementsAsync\cf0\par
\cf2\f1\lang1049\'ce\'ef\'e8\'f1\'e0\'ed\'e8\'e5\cf0 : \'ec\'e5\'f2\'ee\'e4 \'e2\'f1\'f2\'e0\'e2\'e8\'f2\'fc \'e0\'f2\'f0\'e8\'e1\'f3\'f2 \'f1 \'f3\'ea\'e0\'e7\'e0\'ed\'ed\'fb\'ec \'e7\'ed\'e0\'f7\'e5\'ed\'e8\'e5\'ec \'e2 \'ec\'ed\'ee\'e6\'e5\'f1\'f2\'e2\'ee \'fd\'eb\'e5\'ec\'e5\'ed\'f2\'ee\'e2 \'e8 \'e2 \'f0\'e5\'e7\'f3\'eb\'fc\'f2\'e0\'f2\'e5 \'e2\'ee\'e7\'e2\'f0\'e0\'f9\'e0\'e5\'f2 \'f1\'ef\'e8\'f1\'ee\'ea\par
\cf2\'d1\'e8\'ed\'f2\'e0\'ea\'f1\'e8\'f1\cf0 : \f0\lang1033 SetAttributeInElementsAsync(string by, string locator, string attribute, string value)\f1\lang1049\par
\cf3\par
\cf2\'cf\'f0\'e8\'ec\'e5\'f0\cf3 :\par
\cf0\f0\lang1033 FRAMEElement frame = await tester.GetFrameAsync(0);\par
\par
List<string> values = await frame.SetAttributeInElementsAsync(Tester.BY_CSS, ""input"", ""class"", ""test-class"");\par
foreach (string value in values)\par
\{\par
\tab tester.ConsoleMsg(value);\par
\}\par
\par
List<string> values = await frame.SetAttributeInElementsAsync(Tester.BY_XPATH, ""//input"", ""class"", ""test-class"");\par
foreach (string value in values)\par
\{\par
\tab tester.ConsoleMsg(value);\par
\}\par
}",

@"{\rtf1\ansi\ansicpg1251\deff0\nouicompat\deflang1049{\fonttbl{\f0\fnil\fcharset0 Calibri;}{\f1\fnil\fcharset204 Calibri;}}
{\colortbl ;\red0\green77\blue187;\red155\green0\blue211;\red0\green0\blue0;}
{\*\generator Riched20 10.0.22000}\viewkind4\uc1 
\pard\sl276\slmult1\cf1\f0\fs20\lang9 SetHtmlInElementAsync\cf0\par
\cf2\f1\lang1049\'ce\'ef\'e8\'f1\'e0\'ed\'e8\'e5\cf0 : \'ec\'e5\'f2\'ee\'e4 \'e2\'f1\'f2\'e0\'e2\'eb\'ff\'e5\'f2 html \'ef\'f0\'e5\'e4\'f1\'f2\'e0\'e2\'eb\'e5\'ed\'e8\'e5 \'ee\'e1\'fa\'e5\'ea\'f2\'e0 \'e2 \'f3\'ea\'e0\'e7\'e0\'ed\'ed\'fb\'e9 \'fd\'eb\'e5\'ec\'e5\'ed\'f2\par
\cf2\'d1\'e8\'ed\'f2\'e0\'ea\'f1\'e8\'f1\cf0 : \f0\lang1033 SetHtmlInElementAsync(string by, string locator, string html)\f1\lang1049\par
\cf3\par
\cf2\'cf\'f0\'e8\'ec\'e5\'f0\cf3 :\par
\cf0\f0\lang1033 FRAMEElement frame = await tester.GetFrameAsync(0);\par
await frame.SetHtmlInElementAsync(Tester.BY_CSS, ""#auth > h2"", ""<div>\f1\lang1049\'d2\'e5\'f1\'f2\'ee\'e2\'fb\'e9 \'e1\'eb\'ee\'ea</div>"");\par
\par
await \f0\lang1033 frame\f1\lang1049 .SetHtmlInElementAsync(Tester.BY_XPATH, ""//div[@id='auth']//h2"", ""<div>\'d2\'e5\'f1\'f2\'ee\'e2\'fb\'e9 \'e1\'eb\'ee\'ea</div>"");\f0\lang1033\par
    }",

@"{\rtf1\ansi\ansicpg1251\deff0\nouicompat\deflang1049{\fonttbl{\f0\fnil\fcharset0 Calibri;}{\f1\fnil\fcharset204 Calibri;}}
{\colortbl ;\red0\green77\blue187;\red155\green0\blue211;\red0\green0\blue0;}
{\*\generator Riched20 10.0.22000}\viewkind4\uc1 
\pard\sl276\slmult1\cf1\f0\fs20\lang9 SetTextInElementAsync\cf0\par
\cf2\f1\lang1049\'ce\'ef\'e8\'f1\'e0\'ed\'e8\'e5\cf0 : \'ec\'e5\'f2\'ee\'e4 \'e2\'f1\'f2\'e0\'e2\'eb\'ff\'e5\'f2 \'f2\'e5\'ea\'f1\'f2 \'e2 \'f3\'ea\'e0\'e7\'e0\'ed\'ed\'fb\'e9 \'fd\'eb\'e5\'ec\'e5\'ed\'f2\par
\cf2\'d1\'e8\'ed\'f2\'e0\'ea\'f1\'e8\'f1\cf0 : \f0\lang1033 SetTextInElementAsync(string by, string locator, string text)\f1\lang1049\par
\cf3\par
\cf2\'cf\'f0\'e8\'ec\'e5\'f0\cf3 :\par
\cf0\f0\lang1033 FRAMEElement frame = await tester.GetFrameAsync(0);\par
await frame.SetTextInElementAsync(Tester.BY_CSS, ""#auth > h2"", ""\f1\lang1049\'d2\'e5\'f1\'f2\'ee\'e2\'fb\'e9 \'e7\'e0\'e3\'ee\'eb\'ee\'e2\'ee\'ea"");\par
\par
await \f0\lang1033 frame\f1\lang1049 .SetTextInElementAsync(Tester.BY_XPATH, ""//div[@id='auth']//h2"", ""\'d2\'e5\'f1\'f2\'ee\'e2\'fb\'e9 \'e7\'e0\'e3\'ee\'eb\'ee\'e2\'ee\'ea"");\f0\lang1033\par
    }",

@"{\rtf1\ansi\ansicpg1251\deff0\nouicompat\deflang1049{\fonttbl{\f0\fnil\fcharset0 Calibri;}{\f1\fnil\fcharset204 Calibri;}}
{\colortbl ;\red0\green77\blue187;\red155\green0\blue211;\red0\green0\blue0;}
{\*\generator Riched20 10.0.22000}\viewkind4\uc1 
\pard\sl276\slmult1\cf1\f0\fs20\lang9 SetValueInElementAsync\cf0\par
\cf2\f1\lang1049\'ce\'ef\'e8\'f1\'e0\'ed\'e8\'e5\cf0 : \'ec\'e5\'f2\'ee\'e4 \'e2\'f1\'f2\'e0\'e2\'eb\'ff\'e5\'f2 \'e7\'ed\'e0\'f7\'e5\'ed\'e8\'e5 \'e2 \'f3\'ea\'e0\'e7\'e0\'ed\'ed\'fb\'e9 \'fd\'eb\'e5\'ec\'e5\'ed\'f2\par
\cf2\'d1\'e8\'ed\'f2\'e0\'ea\'f1\'e8\'f1\cf0 : \f0\lang1033 SetValueInElementAsync(string by, string locator, string value)\f1\lang1049\par
\cf3\par
\cf2\'cf\'f0\'e8\'ec\'e5\'f0\cf3 :\par
\cf0\f0\lang1033 FRAMEElement frame = await tester.GetFrameAsync(0);\par
await frame.SetValueInElementAsync(Tester.BY_CSS, ""input[id='login']"", ""admin"");\par
await frame.SetValueInElementAsync(Tester.BY_CSS, ""input[id='pass']"", ""0000"");\par
\par
await frame.SetValueInElementAsync(Tester.BY_XPATH, ""//input[@id='login']"", ""admin"");\par
await frame.SetValueInElementAsync(Tester.BY_XPATH, ""//input[@id='pass']"", ""0000"");\par
    }",

@"{\rtf1\ansi\ansicpg1251\deff0\nouicompat\deflang1049{\fonttbl{\f0\fnil\fcharset0 Calibri;}{\f1\fnil\fcharset204 Calibri;}}
{\colortbl ;\red0\green77\blue187;\red155\green0\blue211;\red0\green0\blue0;}
{\*\generator Riched20 10.0.22000}\viewkind4\uc1 
\pard\sl276\slmult1\cf1\f0\fs20\lang9 WaitNotVisibleElementAsync\cf0\par
\cf2\f1\lang1049\'ce\'ef\'e8\'f1\'e0\'ed\'e8\'e5\cf0 : \'ec\'e5\'f2\'ee\'e4 \'e2\'fb\'ef\'ee\'eb\'ed\'ff\'e5\'f2 \'e2\'f0\'e5\'ec\'e5\'ed\'ed\'f3\'fe \'ee\'f1\'f2\'e0\'ed\'ee\'e2\'ea\'f3 \'e2\'fb\'ef\'ee\'eb\'ed\'e5\'ed\'e8\'ff \'f2\'e5\'f1\'f2\'e0 \'ed\'e0 \'f3\'ea\'e0\'e7\'e0\'ed\'ed\'ee\'e5 \'ea\'ee\'eb\'e8\'f7\'e5\'f1\'f2\'e2\'ee \'f1\'e5\'ea\'f3\'ed\'e4 \'e8 \'e6\'e4\'e5\'f2 \'ea\'ee\'e3\'e4\'e0  \'e7\'e0\'ef\'f0\'e0\'f8\'e8\'e2\'e0\'e5\'ec\'fb\'e9 \'fd\'eb\'e5\'ec\'e5\'ed\'f2 \'ef\'e5\'f0\'e5\'f1\'f2\'e0\'ed\'e5\'f2 \'ee\'f2\'ee\'e1\'f0\'e0\'e6\'e0\'e5\'f2\'fc\'f1\'ff \par
\cf2\'d1\'e8\'ed\'f2\'e0\'ea\'f1\'e8\'f1\cf0 : \f0\lang1033 WaitNotVisibleElementAsync(string by, string locator, int sec)\f1\lang1049\par
\cf3\par
\cf2\'cf\'f0\'e8\'ec\'e5\'f0\cf3 :\par
\cf0\f0\lang1033 FRAMEElement frame = await tester.GetFrameAsync(0);\par
await frame.WaitNotVisibleElementAsync(Tester.BY_CSS, ""div[id='result']"", 2);\par
\par
await frame.WaitNotVisibleElementAsync(Tester.BY_XPATH, ""//div[@id='result']"", 2);\par
    }",

@"{\rtf1\ansi\ansicpg1251\deff0\nouicompat\deflang1049{\fonttbl{\f0\fnil\fcharset0 Calibri;}{\f1\fnil\fcharset204 Calibri;}}
{\colortbl ;\red0\green77\blue187;\red155\green0\blue211;\red0\green0\blue0;}
{\*\generator Riched20 10.0.22000}\viewkind4\uc1 
\pard\sl276\slmult1\cf1\f0\fs20\lang9 WaitVisibleElementAsync\cf0\par
\cf2\f1\lang1049\'ce\'ef\'e8\'f1\'e0\'ed\'e8\'e5\cf0 : \'ec\'e5\'f2\'ee\'e4 \'e2\'fb\'ef\'ee\'eb\'ed\'ff\'e5\'f2 \'e2\'f0\'e5\'ec\'e5\'ed\'ed\'f3\'fe \'ee\'f1\'f2\'e0\'ed\'ee\'e2\'ea\'f3 \'e2\'fb\'ef\'ee\'eb\'ed\'e5\'ed\'e8\'ff \'f2\'e5\'f1\'f2\'e0 \'ed\'e0 \'f3\'ea\'e0\'e7\'e0\'ed\'ed\'ee\'e5 \'ea\'ee\'eb\'e8\'f7\'e5\'f1\'f2\'e2\'ee \'f1\'e5\'ea\'f3\'ed\'e4 \'e8 \'e6\'e4\'e5\'f2 \'ea\'ee\'e3\'e4\'e0  \'e7\'e0\'ef\'f0\'e0\'f8\'e8\'e2\'e0\'e5\'ec\'fb\'e9 \'fd\'eb\'e5\'ec\'e5\'ed\'f2 \'ee\'f2\'ee\'e1\'f0\'e0\'e7\'e8\'f2\'f1\'ff\par
\cf2\'d1\'e8\'ed\'f2\'e0\'ea\'f1\'e8\'f1\cf0 : \f0\lang1033 WaitVisibleElementAsync(string by, string locator, int sec)\f1\lang1049\par
\cf3\par
\cf2\'cf\'f0\'e8\'ec\'e5\'f0\cf3 :\par
\cf0\f0\lang1033 FRAMEElement frame = await tester.GetFrameAsync(0);\par
await frame.WaitVisibleElementAsync(Tester.BY_CSS, ""div[id='result']"", 2);\par
\par
await frame.WaitVisibleElementAsync(Tester.BY_XPATH, ""//div[@id='result']"", 2);\par
    }",

@"{\rtf1\ansi\ansicpg1251\deff0\nouicompat\deflang1049{\fonttbl{\f0\fnil\fcharset0 Calibri;}{\f1\fnil\fcharset204 Calibri;}}
{\colortbl ;\red0\green77\blue187;\red155\green0\blue211;\red0\green0\blue0;}
{\*\generator Riched20 10.0.22000}\viewkind4\uc1 
\pard\sl276\slmult1\cf1\f0\fs20\lang9 GetTestResult\cf0\par
\cf2\f1\lang1049\'ce\'ef\'e8\'f1\'e0\'ed\'e8\'e5\cf0 : \'ec\'e5\'f2\'ee\'e4 \'e2\'ee\'e7\'e2\'f0\'e0\'f9\'e0\'e5\'f2 \'f1\'f2\'e0\'f2\'f3\'f1 \'e2\'fb\'ef\'ee\'eb\'ed\'ff\'e5\'ec\'ee\'e3\'ee \'f2\'e5\'f1\'f2\'e0\par
\cf2\'d1\'e8\'ed\'f2\'e0\'ea\'f1\'e8\'f1\cf0 : \f0\lang1033 GetTestResult()\f1\lang1049\par
\cf3\par
\cf2\'cf\'f0\'e8\'ec\'e5\'f0\cf3 :\par
\cf0\f0\lang1033 Tester tester = new Tester(browserForm);\par
await tester.TestBeginAsync();\par
\f1\lang1049 ...\par
\f0\lang1033 if(tester.GetTestResult() == Tester.PROCESS) \{ \}\par
\f1\lang1049 ...\par
\f0\lang1033 await tester.TestEndAsync();\par
if(tester.GetTestResult() == Tester.FAILED) \{ \}\par
if(tester.GetTestResult() == Tester.PASSED) \{ \}\par
\par
}",

@"{\rtf1\ansi\ansicpg1251\deff0\nouicompat\deflang1049{\fonttbl{\f0\fnil\fcharset0 Calibri;}{\f1\fnil\fcharset204 Calibri;}}
{\colortbl ;\red0\green77\blue187;\red155\green0\blue211;\red0\green0\blue0;}
{\*\generator Riched20 10.0.22000}\viewkind4\uc1 
\pard\sl276\slmult1\cf1\f0\fs20\lang9 TimerStart\cf0\par
\cf2\f1\lang1049\'ce\'ef\'e8\'f1\'e0\'ed\'e8\'e5\cf0 : \'ec\'e5\'f2\'ee\'e4 \'e7\'e0\'ef\'f3\'f1\'ea\'e0\'e5\'f2 \'ee\'f2\'f1\'f7\'e5\'f2 \'e2\'f0\'e5\'ec\'e5\'ed\'e8 \'e8 \'e2\'ee\'e7\'e2\'f0\'e0\'f9\'e0\'e5\'f2 \'e7\'ed\'e0\'f7\'e5\'ed\'e8\'e5 DateTime\par
\cf2\'d1\'e8\'ed\'f2\'e0\'ea\'f1\'e8\'f1\cf0 : \f0\lang1033 TimerStart()\f1\lang1049\par
\cf3\par
\cf2\'cf\'f0\'e8\'ec\'e5\'f0\cf3 :\par
\cf0\f0\lang1033 Tester tester = new Tester(browserForm);\par
DateTime start = await tester.TimerStart();\par
await tester.TestBeginAsync();\par
\f1\lang1049 ...\par
\f0\lang1033 await tester.TestEndAsync();\par
TimeSpan result = await tester.TimerStop(start);\par
tester.ConsoleMsg(""Time "" + result.TotalSeconds);\par
}",

@"{\rtf1\ansi\ansicpg1251\deff0\nouicompat\deflang1049{\fonttbl{\f0\fnil\fcharset0 Calibri;}{\f1\fnil\fcharset204 Calibri;}}
{\colortbl ;\red0\green77\blue187;\red155\green0\blue211;\red0\green0\blue0;}
{\*\generator Riched20 10.0.22000}\viewkind4\uc1 
\pard\sl276\slmult1\cf1\f0\fs20\lang9 TimerStop\cf0\par
\cf2\f1\lang1049\'ce\'ef\'e8\'f1\'e0\'ed\'e8\'e5\cf0 : \'ec\'e5\'f2\'ee\'e4 \'e7\'e0\'e2\'e5\'f0\'f8\'e0\'e5\'f2 \'ee\'f2\'f1\'f7\'e5\'f2 \'e2\'f0\'e5\'ec\'e5\'ed\'e8 \'e8 \'e2\'ee\'e7\'e2\'f0\'e0\'f9\'e0\'e5\'f2 \'e7\'ed\'e0\'f7\'e5\'ed\'e8\'e5 TimeSpan \'f1 \'f0\'e5\'e7\'f3\'eb\'fc\'f2\'e0\'f2\'ee\'ec (\'ed\'e0\'ef\'f0\'e8\'ec\'e5\'f0 7,132157 \'f7\'f2\'ee \'e7\'ed\'e0\'f7\'e8\'f2 7 \'f1\'e5\'ea\'f3\'ed\'e4 \'e8 132157 \'ec\'e8\'eb\'eb\'e8\'f1\'e5\'ea\'f3\'ed\'e4)\par
\cf2\'d1\'e8\'ed\'f2\'e0\'ea\'f1\'e8\'f1\cf0 : \f0\lang1033 TimerStop(DateTime start)\f1\lang1049\par
\cf3\par
\cf2\'cf\'f0\'e8\'ec\'e5\'f0\cf3 :\par
\cf0\f0\lang1033 Tester tester = new Tester(browserForm);\par
DateTime start = await tester.TimerStart();\par
await tester.TestBeginAsync();\par
\f1\lang1049 ...\par
\f0\lang1033 await tester.TestEndAsync();\par
TimeSpan result = await tester.TimerStop(start);\par
tester.ConsoleMsg(""Time "" + result.TotalSeconds);\par
}",

@"{\rtf1\ansi\ansicpg1251\deff0\nouicompat\deflang1049{\fonttbl{\f0\fnil\fcharset0 Calibri;}{\f1\fnil\fcharset204 Calibri;}}
{\colortbl ;\red0\green77\blue187;\red155\green0\blue211;\red0\green0\blue0;}
{\*\generator Riched20 10.0.22000}\viewkind4\uc1 
\pard\sl276\slmult1\cf1\f0\fs20\lang9 AssertNoErrorsAsync\cf0\par
\cf2\f1\lang1049\'ce\'ef\'e8\'f1\'e0\'ed\'e8\'e5\cf0 : \'ec\'e5\'f2\'ee\'e4 \'e2\'fb\'ef\'ee\'eb\'ed\'ff\'e5\'f2 \'ef\'f0\'ee\'e2\'e5\'f0\'ea\'f3 \'ee\'f2\'f1\'f3\'f2\'f1\'f2\'e2\'e8\'ff \'ee\'f8\'e8\'e1\'ee\'ea \'ed\'e0 \'f1\'f2\'f0\'e0\'ed\'e8\'f6\'e5 \'e8 \'e5\'f1\'eb\'e8 \'ee\'f8\'e8\'e1\'ea\'e8 \'ef\'f0\'e8\'f1\'f3\'f2\'f1\'f2\'e2\'f3\'fe\'f2 \'ef\'f0\'ee\'e2\'e5\'f0\'ea\'e0 \'e1\'f3\'e4\'e5\'f2 \'f1\'f7\'e8\'f2\'e0\'f2\'fc\'f1\'ff \'ef\'f0\'ee\'e2\'e0\'eb\'fc\'ed\'ee\'e9\par
\cf2\'d1\'e8\'ed\'f2\'e0\'ea\'f1\'e8\'f1\cf0 : \f0\lang1033 AssertNoErrorsAsync(bool showListErrors = false, string[] listIgnored = null)\f1\lang1049\par
\cf3\par
\cf2\'cf\'f0\'e8\'ec\'e5\'f0\cf3 :\par
\cf0\f0\lang1033 Tester tester = new Tester(browserForm);\par
await tester.TestBeginAsync();\par
await tester.GoToUrlAsync(""https://somovstudio.github.io/test_error.html"", 25);\par
await tester.AssertNoErrorsAsync(true, new string[1] { ""stats.g.doubleclick.net"" });\par
await tester.TestEndAsync();\par
}",

@"{\rtf1\ansi\ansicpg1251\deff0\nouicompat\deflang1049{\fonttbl{\f0\fnil\fcharset0 Calibri;}{\f1\fnil\fcharset204 Calibri;}}
{\colortbl ;\red0\green77\blue187;\red155\green0\blue211;\red0\green0\blue0;}
{\*\generator Riched20 10.0.22000}\viewkind4\uc1 
\pard\sl276\slmult1\cf1\f0\fs20\lang9 AssertNetworkEventsAsync\cf0\par
\cf2\f1\lang1049\'ce\'ef\'e8\'f1\'e0\'ed\'e8\'e5\cf0 : \'ec\'e5\'f2\'ee\'e4 \'e2\'fb\'ef\'ee\'eb\'ed\'ff\'e5\'f2 \'ef\'f0\'ee\'e2\'e5\'f0\'ea\'f3 \'ef\'f0\'e8\'f1\'f3\'f2\'f1\'f2\'e2\'e8\'ff (presence \f0\lang1033 = true\f1\lang1049 )\f0\lang1033  \f1\lang1049\'e8\'eb\'e8 \'ee\'f2\'f1\'f3\'f2\'f1\'f2\'e2\'e8\'e5 (presence \f0\lang1033 = false\f1\lang1049 )\f0\lang1033  \f1\lang1049\'f3\'ea\'e0\'e7\'e0\'ed\'ed\'fb\'f5 \'f1\'ee\'e1\'fb\'f2\'e8\'e9 (events),\f0\lang1033  \f1\lang1049  \'e4\'e0\'ed\'ed\'fb\'e9 \'ec\'e5\'f2\'ee\'e4 \'f5\'ee\'f0\'ee\'f8\'ee \'e8\'f1\'ef\'ee\'eb\'fc\'e7\'ee\'e2\'e0\'f2\'fc \'ef\'f0\'e8 \'ef\'f0\'ee\'e2\'e5\'f0\'ea\'e5 \'f1\'ee\'e1\'fb\'f2\'e8\'e9 Google Analytics \'e8 \'df\'ed\'e4\'e5\'ea\'f1 \'cc\'e5\'f2\'f0\'e8\'ea\'e0\par
\cf2\'d1\'e8\'ed\'f2\'e0\'ea\'f1\'e8\'f1\cf0 : \f0\lang1033 AssertNetworkEventsAsync(bool presence, string[] events)\f1\lang1049\par
\cf3\par
\cf2\'cf\'f0\'e8\'ec\'e5\'f0\cf3 :\par
\cf0\f0\lang1033 await tester.AssertNetworkEventsAsync(true, new string[] \{ \par
\f1\lang1049\tab\f0\lang1033 ""ec=zayavka"", ""ea=b2c_new_main"", ""el=some_shpd_nm"", ""zayavka_shpd"" \par
\});\par
}",

@"{\rtf1\ansi\ansicpg1251\deff0\nouicompat\deflang1049{\fonttbl{\f0\fnil\fcharset0 Calibri;}{\f1\fnil\fcharset204 Calibri;}}
{\colortbl ;\red0\green77\blue187;\red155\green0\blue211;\red0\green0\blue0;}
{\*\generator Riched20 10.0.22000}\viewkind4\uc1 
\pard\sl276\slmult1\cf1\f0\fs20\lang9 SendMsgToMailAsync\cf0\par
\cf2\f1\lang1049\'ce\'ef\'e8\'f1\'e0\'ed\'e8\'e5\cf0 : \'ec\'e5\'f2\'ee\'e4 \'ee\'f2\'ef\'f0\'e0\'e2\'eb\'ff\'e5\'f2 \'ef\'e8\'f1\'fc\'ec\'ee \'ed\'e0 \'ef\'ee\'f7\'f2\'f3\par
\cf2\'d1\'e8\'ed\'f2\'e0\'ea\'f1\'e8\'f1\cf0 : \f0\lang1033 SendMsgToMailAsync(string subject, string body, string filename = """", string addresses = """")\f1\lang1049\par
\cf3\par
\cf2\'cf\'f0\'e8\'ec\'e5\'f0\cf3 :\par
\cf0\f0\lang1033 Tester tester = new Tester(browserForm);\par
await tester.TestBeginAsync();\par
...\par
await tester.TestEndAsync();\par
if(tester.GetTestResult() == Tester.PASSED) \par
\{ \par
\tab await tester.SendMsgToMailAsync(""\f1\lang1049\'d2\'e5\'f1\'f2\'ee\'e2\'ee\'e5 \'f1\'ee\'ee\'e1\'f9\'e5\'ed\'e8\'e5"", ""\'d2\'e5\'f1\'f2 \'e7\'e0\'e2\'e5\'f0\'f8\'e8\'eb\'f1\'ff \'f3\'f1\'ef\'e5\'f8\'ed\'ee"");\par
\}\f0\lang1033\par
}",

@"{\rtf1\ansi\ansicpg1251\deff0\nouicompat\deflang1049{\fonttbl{\f0\fnil\fcharset0 Calibri;}{\f1\fnil\fcharset204 Calibri;}}
{\colortbl ;\red0\green77\blue187;\red155\green0\blue211;\red0\green0\blue0;}
{\*\generator Riched20 10.0.22000}\viewkind4\uc1 
\pard\sl276\slmult1\cf1\f0\fs20\lang9 SendMsgToTelegramAsync\cf0\par
\cf2\f1\lang1049\'ce\'ef\'e8\'f1\'e0\'ed\'e8\'e5\cf0 : \'ec\'e5\'f2\'ee\'e4 \'ee\'f2\'ef\'f0\'e0\'e2\'eb\'ff\'e5\'f2 \'f1\'ee\'ee\'e1\'f9\'e5\'ed\'e8\'e5 \'e2 \'d2\'e5\'eb\'e5\'e3\'f0\'e0\'ec, \'e4\'eb\'ff \'fd\'f2\'ee\'e3\'ee \'ed\'f3\'e6\'ed\'ee \'f1\'ee\'e7\'e4\'e0\'f2\'fc \'e1\'ee\'f2\'e0 \'e4\'ee\'e1\'e0\'e2\'e8\'f2\'fc \'e5\'e3\'ee \'e2 \'f7\'e0\'f2 \'e8 \'e2 \'f4\'f3\'ed\'ea\'f6\'e8\'fe \'ef\'e5\'f0\'e5\'e4\'e0\'f2\'fc \'f2\'ee\'ea\'e5\'ed \'e1\'ee\'f2\'e0 (\f0\lang1033 botToken\f1\lang1049 ), \'e8\'e4\'e5\'ed\'f2\'e8\'f4\'e8\'ea\'e0\'f2\'ee\'f0 \'f7\'e0\'f2\'e0 \f0\lang1033 (chatId)\f1\lang1049 , \'f2\'e5\'ea\'f1\'f2 \'f1\'ee\'ee\'e1\'f9\'e5\'ed\'e8\'ff \f0\lang1033 (text) \f1\lang1049\'e8 \'ef\'f0\'e8 \'ed\'e5\'ee\'e1\'f5\'ee\'e4\'e8\'ec\'ee\'f1\'f2\'e8 \'f3\'ea\'e0\'e7\'e0\'f2\'fc \'ea\'ee\'e4\'e8\'f0\'ee\'e2\'ea\'f3 (\f0\lang1033 charset\f1\lang1049 )\par
\cf2\'d1\'e8\'ed\'f2\'e0\'ea\'f1\'e8\'f1\cf0 : \f0\lang1033 SendMsgToTelegramAsync(string botToken, string chatId, string text, string charset = ""UTF-8"")\f1\lang1049\par
\cf3\par
\cf2\'cf\'f0\'e8\'ec\'e5\'f0\cf3 :\par
\cf0\f0\lang1033 Tester tester = new Tester(browserForm);\par
await tester.TestBeginAsync();\par
...\par
await tester.TestEndAsync();\par
if(tester.GetTestResult() == Tester.FAILED) \par
\{ \par
\tab await tester.SendMsgToTelegramAsync(""0000000001:ABCDabcd123ABCDabcd123ABCDabcd123ZX"", ""-123456789"", ""\f1\lang1049\'d2\'e5\'f1\'f2\'ee\'e2\'ee\'e5 \'f1\'ee\'ee\'e1\'f9\'e5\'ed\'e8\'e5"");\par
\}\f0\lang1033\par
}",

@"{\rtf1\ansi\ansicpg1251\deff0\nouicompat\deflang1049{\fonttbl{\f0\fnil\fcharset0 Calibri;}{\f1\fnil\fcharset204 Calibri;}}
{\colortbl ;\red0\green77\blue187;\red155\green0\blue211;\red0\green0\blue0;}
{\*\generator Riched20 10.0.22000}\viewkind4\uc1 
\pard\sl276\slmult1\cf1\f0\fs20\lang9 AssertNotNullAsync\cf0\par
\cf2\f1\lang1049\'ce\'ef\'e8\'f1\'e0\'ed\'e8\'e5\cf0 : \'ec\'e5\'f2\'ee\'e4 \'e2\'fb\'ef\'ee\'eb\'ed\'ff\'e5\'f2 \'ef\'f0\'ee\'e2\'e5\'f0\'ea\'f3 \'e7\'ed\'e0\'f7\'e5\'ed\'e8\'ff \'ea\'ee\'f2\'ee\'f0\'ee\'e5 \'ed\'e5 \'e4\'ee\'eb\'e6\'ed\'ee \'e1\'fb\'f2\'fc \f0\lang1033 null\f1\lang1049 , \'e8 \'e2 \'f1\'eb\'f3\'f7\'e0\'e5 \'e5\'f1\'eb\'e8 \'e7\'ed\'e0\'f7\'e5\'ed\'e8\'e5 \f0\lang1033 null \f1\lang1049\'ef\'f0\'ee\'e2\'e5\'f0\'ea\'e0 \'e1\'f3\'e4\'e5\'f2 \'f1\'f7\'e8\'f2\'e0\'f2\'fc\'f1\'ff \'ef\'f0\'ee\'e2\'e0\'eb\'fc\'ed\'ee\'e9\par
\cf2\'d1\'e8\'ed\'f2\'e0\'ea\'f1\'e8\'f1\cf0 : \f0\lang1033 AssertNotNullAsync(dynamic obj)\f1\lang1049\par
\cf3\par
\cf2\'cf\'f0\'e8\'ec\'e5\'f0\cf3 :\par
\cf0\f0\lang1033 string text = ""\f1\lang1049\'f2\'e5\'ea\'f1\'f2"";\par
await tester.AssertNotNullAsync(text);\f0\lang1033\par
}",

@"{\rtf1\ansi\ansicpg1251\deff0\nouicompat\deflang1049{\fonttbl{\f0\fnil\fcharset0 Calibri;}{\f1\fnil\fcharset204 Calibri;}}
{\colortbl ;\red0\green77\blue187;\red155\green0\blue211;\red0\green0\blue0;}
{\*\generator Riched20 10.0.22000}\viewkind4\uc1 
\pard\sl276\slmult1\cf1\f0\fs20\lang9 AssertNullAsync\cf0\par
\cf2\f1\lang1049\'ce\'ef\'e8\'f1\'e0\'ed\'e8\'e5\cf0 : \'ec\'e5\'f2\'ee\'e4 \'e2\'fb\'ef\'ee\'eb\'ed\'ff\'e5\'f2 \'ef\'f0\'ee\'e2\'e5\'f0\'ea\'f3 \'e7\'ed\'e0\'f7\'e5\'ed\'e8\'ff \'ea\'ee\'f2\'ee\'f0\'ee\'e5 \'e4\'ee\'eb\'e6\'ed\'ee \'e1\'fb\'f2\'fc \f0\lang1033 null\f1\lang1049 , \'e8 \'e2 \'f1\'eb\'f3\'f7\'e0\'e5 \'e5\'f1\'eb\'e8 \'e7\'ed\'e0\'f7\'e5\'ed\'e8\'e5 \'ed\'e5 \f0\lang1033 null \f1\lang1049\'ef\'f0\'ee\'e2\'e5\'f0\'ea\'e0 \'e1\'f3\'e4\'e5\'f2 \'f1\'f7\'e8\'f2\'e0\'f2\'fc\'f1\'ff \'ef\'f0\'ee\'e2\'e0\'eb\'fc\'ed\'ee\'e9\par
\cf2\'d1\'e8\'ed\'f2\'e0\'ea\'f1\'e8\'f1\cf0 : \f0\lang1033 AssertNullAsync(dynamic obj)\f1\lang1049\par
\cf3\par
\cf2\'cf\'f0\'e8\'ec\'e5\'f0\cf3 :\par
\cf0\f0\lang1033 string text = null;\par
await tester.AssertNullAsync(text);\par
}",

@"{\rtf1\ansi\ansicpg1251\deff0\nouicompat\deflang1049{\fonttbl{\f0\fnil\fcharset0 Calibri;}{\f1\fnil\fcharset204 Calibri;}}
{\colortbl ;\red0\green77\blue187;\red155\green0\blue211;\red0\green0\blue0;}
{\*\generator Riched20 10.0.22000}\viewkind4\uc1 
\pard\sl276\slmult1\cf1\f0\fs20\lang9 WaitElementInDomAsync\cf0\par
\cf2\f1\lang1049\'ce\'ef\'e8\'f1\'e0\'ed\'e8\'e5\cf0 : \'ec\'e5\'f2\'ee\'e4 \'e2\'fb\'ef\'ee\'eb\'ed\'ff\'e5\'f2 \'e2\'f0\'e5\'ec\'e5\'ed\'ed\'f3\'fe \'ee\'f1\'f2\'e0\'ed\'ee\'e2\'ea\'f3 \'e2\'fb\'ef\'ee\'eb\'ed\'e5\'ed\'e8\'ff \'f2\'e5\'f1\'f2\'e0 \'ed\'e0 \'f3\'ea\'e0\'e7\'e0\'ed\'ed\'ee\'e5 \'ea\'ee\'eb\'e8\'f7\'e5\'f1\'f2\'e2\'ee \'f1\'e5\'ea\'f3\'ed\'e4 \'e8 \'e6\'e4\'e5\'f2 \'ea\'ee\'e3\'e4\'e0 \'e7\'e0\'ef\'f0\'e0\'f8\'e8\'e2\'e0\'e5\'ec\'fb\'e9 \'fd\'eb\'e5\'ec\'e5\'ed\'f2 \'ef\'ee\'ff\'e2\'e8\'f2\'f1\'ff \'e2 DOM\par
\cf2\'d1\'e8\'ed\'f2\'e0\'ea\'f1\'e8\'f1\cf0 : \f0\lang1033 WaitElementInDomAsync(string by, string locator, int sec)\f1\lang1049\par
\cf3\par
\cf2\'cf\'f0\'e8\'ec\'e5\'f0\cf3 :\par
\cf0\f0\lang1033 await tester.WaitElementInDomAsync(Tester.BY_XPATH, ""//div[@id='result']"", 5);\par
await tester.WaitElementInDomAsync(Tester.BY_CSS, ""#result"", 5);\par
}",

@"{\rtf1\ansi\ansicpg1251\deff0\nouicompat\deflang1049{\fonttbl{\f0\fnil\fcharset0 Calibri;}{\f1\fnil\fcharset204 Calibri;}}
{\colortbl ;\red0\green77\blue187;\red155\green0\blue211;\red0\green0\blue0;}
{\*\generator Riched20 10.0.22000}\viewkind4\uc1 
\pard\sl276\slmult1\cf1\f0\fs20\lang9 WaitElementNotDomAsync\cf0\par
\cf2\f1\lang1049\'ce\'ef\'e8\'f1\'e0\'ed\'e8\'e5\cf0 : \'ec\'e5\'f2\'ee\'e4 \'e2\'fb\'ef\'ee\'eb\'ed\'ff\'e5\'f2 \'e2\'f0\'e5\'ec\'e5\'ed\'ed\'f3\'fe \'ee\'f1\'f2\'e0\'ed\'ee\'e2\'ea\'f3 \'e2\'fb\'ef\'ee\'eb\'ed\'e5\'ed\'e8\'ff \'f2\'e5\'f1\'f2\'e0 \'ed\'e0 \'f3\'ea\'e0\'e7\'e0\'ed\'ed\'ee\'e5 \'ea\'ee\'eb\'e8\'f7\'e5\'f1\'f2\'e2\'ee \'f1\'e5\'ea\'f3\'ed\'e4 \'e8 \'e6\'e4\'e5\'f2 \'ea\'ee\'e3\'e4\'e0 \'e7\'e0\'ef\'f0\'e0\'f8\'e8\'e2\'e0\'e5\'ec\'fb\'e9 \'fd\'eb\'e5\'ec\'e5\'ed\'f2 \'ef\'e5\'f0\'e5\'f1\'f2\'e0\'ed\'e5\'f2 \'ef\'f0\'e8\'f1\'f3\'f2\'f1\'f2\'e2\'ee\'e2\'e0\'f2\'fc \'e2 \f0\lang1033 DOM\f1\lang1049  \par
\cf2\'d1\'e8\'ed\'f2\'e0\'ea\'f1\'e8\'f1\cf0 : \f0\lang1033 WaitElementNotDomAsync(string by, string locator, int sec)\f1\lang1049\par
\cf3\par
\cf2\'cf\'f0\'e8\'ec\'e5\'f0\cf3 :\par
\cf0\f0\lang1033 await tester.WaitElementNotDomAsync(Tester.BY_XPATH, ""//div[@id='element']"", 5);\par
await tester.WaitElementNotDomAsync(Tester.BY_CSS, ""#element"", 5);\par
}",

@"{\rtf1\ansi\ansicpg1251\deff0\nouicompat\deflang1049{\fonttbl{\f0\fnil\fcharset0 Calibri;}{\f1\fnil\fcharset204 Calibri;}}
{\colortbl ;\red0\green77\blue187;\red155\green0\blue211;\red0\green0\blue0;}
{\*\generator Riched20 10.0.22000}\viewkind4\uc1 
\pard\sl276\slmult1\cf1\f0\fs20\lang9 GoToUrlBaseAuthAsync\cf0\par
\cf2\f1\lang1049\'ce\'ef\'e8\'f1\'e0\'ed\'e8\'e5\cf0 : \'ec\'e5\'f2\'ee\'e4 \'e2\'fb\'ef\'ee\'eb\'ed\'ff\'e5\'f2 \'e7\'e0\'e3\'f0\'f3\'e7\'ea\'f3 \'e2\'e5\'e1 \'f1\'e0\'e9\'f2\'e0 \'ef\'ee \'f3\'ea\'e0\'e7\'e0\'ed\'ee\'ec\'f3 URL \'ef\'f0\'e8 \'e1\'e0\'e7\'ee\'e2\'ee\'e9 \'e0\'e2\'f2\'ee\'f0\'e8\'e7\'e0\'f6\'e8\'e8 \'f1 \'f3\'ea\'e0\'e7\'e0\'ed\'ed\'fb\'ec \'ee\'e6\'e8\'e4\'e0\'ed\'e8\'e5\'ec \'e2 \'f1\'e5\'ea\'f3\'ed\'e4\'e0\'f5\par
\cf2\'d1\'e8\'ed\'f2\'e0\'ea\'f1\'e8\'f1\cf0 : \f0\lang1033 GoToUrlBaseAuthAsync(string url, string login, string pass, int sec, bool abortLoadAfterTime = false)\f1\lang1049\par
\cf3\par
\cf2\'cf\'f0\'e8\'ec\'e5\'f0\cf3 :\par
\cf0\f0\lang1033 await tester.GoToUrlBaseAuthAsync(""https://dev.site.com"", ""login"", ""pass"", 25);\par
}",

@"{\rtf1\ansi\ansicpg1251\deff0\nouicompat\deflang1049{\fonttbl{\f0\fnil\fcharset0 Calibri;}{\f1\fnil\fcharset204 Calibri;}}
{\colortbl ;\red0\green77\blue187;\red155\green0\blue211;\red0\green0\blue0;}
{\*\generator Riched20 10.0.22000}\viewkind4\uc1 
\pard\sl276\slmult1\cf1\f0\fs20\lang9 BrowserPageReloadAsync\cf0\par
\cf2\f1\lang1049\'ce\'ef\'e8\'f1\'e0\'ed\'e8\'e5\cf0 : \'ec\'e5\'f2\'ee\'e4 \'e2\'fb\'ef\'ee\'eb\'ed\'ff\'e5\'f2 \'ef\'e5\'f0\'e5\'e7\'e0\'e3\'f0\'f3\'e7\'ea\'f3 \'ee\'f2\'ea\'f0\'fb\'f2\'ee\'e9 \'f1\'f2\'f0\'e0\'ed\'e8\'f6\'fb \'e2 \'e1\'f0\'e0\'f3\'e7\'e5\'f0\'e5\par
\cf2\'d1\'e8\'ed\'f2\'e0\'ea\'f1\'e8\'f1\cf0 : \f0\lang1033 BrowserPageReloadAsync(int sec, bool abortLoadAfterTime = false)\f1\lang1049\par
\cf3\par
\cf2\'cf\'f0\'e8\'ec\'e5\'f0\cf3 :\par
\cf0\f0\lang1033 await tester.BrowserPageReloadAsync(25);\par
}",

@"{\rtf1\ansi\ansicpg1251\deff0\nouicompat\deflang1049{\fonttbl{\f0\fnil\fcharset0 Calibri;}{\f1\fnil\fcharset204 Calibri;}}
{\colortbl ;\red0\green77\blue187;\red155\green0\blue211;\red0\green0\blue0;}
{\*\generator Riched20 10.0.22000}\viewkind4\uc1 
\pard\sl276\slmult1\cf1\f0\fs20\lang9 GetListRedirectUrlAsync\cf0\par
\cf2\f1\lang1049\'ce\'ef\'e8\'f1\'e0\'ed\'e8\'e5\cf0 : \'ec\'e5\'f2\'ee\'e4 \'e2\'ee\'e7\'e2\'f0\'e0\'f9\'e0\'e5\'f2 \'f1\'ef\'e8\'f1\'ee\'ea \'f0\'e5\'e4\'e8\'f0\'e5\'ea\'f2\'ee\'e2 \'ef\'f0\'ee\'e8\'e7\'ee\'f8\'e5\'e4\'f8\'e8\'f5 \'ef\'f0\'e8 \'e7\'e0\'e3\'f0\'f3\'e7\'ea\'e5 \'f1\'f2\'f0\'e0\'ed\'e8\'f6\'fb\par
\cf2\'d1\'e8\'ed\'f2\'e0\'ea\'f1\'e8\'f1\cf0 : \f0\lang1033 GetListRedirectUrlAsync()\f1\lang1049\par
\cf3\par
\cf2\'cf\'f0\'e8\'ec\'e5\'f0\cf3 :\par
\cf0\f0\lang1033 Tester tester = new Tester(browserForm);\par
await tester.TestBeginAsync();\par
await tester.GoToUrlAsync(""https://yandex.ru/"", 5);\par
\par
List<string> redirects = await tester.GetListRedirectUrlAsync();\par
foreach (string url in redirects)\par
\{\par
\tab tester.ConsoleMsg(url);\par
\}\par
}",

@"{\rtf1\ansi\ansicpg1251\deff0\nouicompat\deflang1049{\fonttbl{\f0\fnil\fcharset0 Calibri;}{\f1\fnil\fcharset204 Calibri;}}
{\colortbl ;\red0\green77\blue187;\red155\green0\blue211;\red0\green0\blue0;}
{\*\generator Riched20 10.0.22000}\viewkind4\uc1 
\pard\sl276\slmult1\cf1\f0\fs20\lang9 GetUrlResponseAsync\cf0\par
\cf2\f1\lang1049\'ce\'ef\'e8\'f1\'e0\'ed\'e8\'e5\cf0 : \'ec\'e5\'f2\'ee\'e4 \'e2\'ee\'e7\'e2\'f0\'e0\'f9\'e0\'e5\'f2\f0\lang1033  \f1\lang1049 HTTP \'ee\'f2\'e2\'e5\'f2 \'f3\'ea\'e0\'e7\'e0\'ed\'ed\'ee\'e3\'ee URL\par
\cf2\'d1\'e8\'ed\'f2\'e0\'ea\'f1\'e8\'f1\cf0 : \f0\lang1033 GetUrlResponseAsync(string url)\f1\lang1049\par
\cf3\par
\cf2\'cf\'f0\'e8\'ec\'e5\'f0\cf3 :\par
\cf0\f0\lang1033 int response = await tester.GetUrlResponseAsync(""https://somovstudio.github.io/test.html"");\par
await tester.AssertEqualsAsync(200, response);\par
}",

@"{\rtf1\ansi\ansicpg1251\deff0\nouicompat\deflang1049{\fonttbl{\f0\fnil\fcharset0 Calibri;}{\f1\fnil\fcharset204 Calibri;}}
{\colortbl ;\red0\green77\blue187;\red155\green0\blue211;\red0\green0\blue0;}
{\*\generator Riched20 10.0.19041}\viewkind4\uc1 
\pard\sl276\slmult1\cf1\f0\fs20\lang9 RestPostAsync\cf0\par
\cf2\f1\lang1049\'ce\'ef\'e8\'f1\'e0\'ed\'e8\'e5\cf0 : \'ec\'e5\'f2\'ee\'e4 \'e2\'fb\'ef\'ee\'eb\'ed\'ff\'e5\'f2 \f0\lang1033 Post\f1\lang1049  Rest \'e7\'e0\'ef\'f0\'ee\'f1\f0\lang1033  \f1\lang1049\'f1 \'ee\'f2\'ef\'f0\'e0\'e2\'ea\'ee\'e9 \'e4\'e0\'ed\'ed\'fb\'f5 \'e2 \'f4\'ee\'f0\'ec\'e0\'f2\'e5 \f0\lang1033 json\f1\lang1049  \'e8 \'ef\'ee\'eb\'f3\'f7\'e0\'e5\'f2 \'f0\'e5\'e7\'f3\'eb\'fc\'f2\'e0\'f2 \'f2\'e0\'ea \'e6\'e5 \'e2 \'f4\'ee\'f0\'ec\'e0\'f2\'e5 json\par
\cf2\'d1\'e8\'ed\'f2\'e0\'ea\'f1\'e8\'f1\cf0 : \f0\lang1033 RestPostAsync(string url, string json, TimeSpan timeout, string charset = ""UTF-8"")\f1\lang1049\par
\cf3\par
\cf2\'cf\'f0\'e8\'ec\'e5\'f0\cf3 :\par
\cf0\f0\lang1033 string result = await tester.RestPostAsync(""https://jsonplaceholder.typicode.com/posts/1/"",\f1\lang1049  \f0\lang1033 ""\{\}"", TimeSpan.FromDays(1), ""UTF-8"");\par
tester.ConsoleMsg(result);\par
\par
\cf2\f1\lang1049\'c4\'e0\'ed\'ed\'fb\'e9 \'ec\'e5\'f2\'ee\'e4 \'e8\'f1\'ef\'ee\'eb\'fc\'e7\'f3\'e5\'f2 \'f1\'f2\'e0\'ed\'e4\'e0\'f0\'f2\'ed\'fb\'e9 \'ef\'ee\'e4\'f5\'ee\'e4\cf3 :\par
\cf0\f0\lang1033 using System.Net;\par
using System.Net.Http;\par
using System.Net.Http.Headers;\par
\par
Uri uri = new Uri(url);\par
HttpClient client = new HttpClient();\par
client.Timeout = TimeSpan.FromDays(1);\par
client.DefaultRequestHeaders.Add(""charset"", ""UTF-8"");\par
client.DefaultRequestHeaders.Add(""User-Agent"", userAgent);\par
HttpContent content = new StringContent(""\{\}"", Encoding.UTF8, ""application/json"");\par
HttpResponseMessage response = await client.PostAsync(uri, content);\par
if (response.IsSuccessStatusCode)\par
\{\par
\tab return await response.Content.ReadAsStringAsync();\par
\}\par
}",

@"{\rtf1\ansi\ansicpg1251\deff0\nouicompat\deflang1049{\fonttbl{\f0\fnil\fcharset0 Calibri;}{\f1\fnil\fcharset204 Calibri;}}
{\colortbl ;\red0\green77\blue187;\red155\green0\blue211;\red0\green0\blue0;}
{\*\generator Riched20 10.0.19041}\viewkind4\uc1 
\pard\sl276\slmult1\cf1\f0\fs20\lang9 GetLocatorAsync\cf0\par
\cf2\f1\lang1049\'ce\'ef\'e8\'f1\'e0\'ed\'e8\'e5\cf0 : \'ec\'e5\'f2\'ee\'e4 \'e2\'ee\'e7\'e2\'f0\'e0\'f9\'e0\'e5\'f2 \'eb\'ee\'ea\'e0\'f2\'ee\'f0 \'fd\'eb\'e5\'ec\'e5\'ed\'f2\'e0\par
\cf2\'d1\'e8\'ed\'f2\'e0\'ea\'f1\'e8\'f1\cf0 : \f0\lang1033 GetLocatorAsync()\f1\lang1049\par
\cf3\par
\cf2\'cf\'f0\'e8\'ec\'e5\'f0\cf3 :\par
\cf0\f0\lang1033 string locator = await tester.GetLocatorAsync();\par
tester.ConsoleMsg(locator);\cf2\f1\lang1049\par
}",

@"{\rtf1\ansi\ansicpg1251\deff0\nouicompat\deflang1049{\fonttbl{\f0\fnil\fcharset0 Calibri;}{\f1\fnil\fcharset204 Calibri;}}
{\colortbl ;\red0\green77\blue187;\red155\green0\blue211;\red0\green0\blue0;}
{\*\generator Riched20 10.0.19041}\viewkind4\uc1 
\pard\sl276\slmult1\cf1\f0\fs20\lang9 ClickMouseAsync\cf0\par
\cf2\f1\lang1049\'ce\'ef\'e8\'f1\'e0\'ed\'e8\'e5\cf0 : \'ec\'e5\'f2\'ee\'e4 \'e2\'fb\'ef\'ee\'eb\'ed\'ff\'e5\'f2 \'ed\'e0\'e6\'e0\'f2\'e8\'e5 \'ed\'e0 \'fd\'eb\'e5\'ec\'e5\'ed\'f2\f0\lang1033  \f1\lang1049\'fd\'ec\'f3\'eb\'e8\'f0\'f3\'ff \'ec\'fb\'f8\'ea\'f3\par
\cf2\'d1\'e8\'ed\'f2\'e0\'ea\'f1\'e8\'f1\cf0 : \f0\lang1033 ClickMouseAsync()\f1\lang1049\par
\cf3\par
\cf2\'cf\'f0\'e8\'ec\'e5\'f0\cf3 :\par
\cf0\f0\lang1033 await element.ClickMouseAsync();\par
}",

@"{\rtf1\ansi\ansicpg1251\deff0\nouicompat\deflang1049{\fonttbl{\f0\fnil\fcharset0 Calibri;}{\f1\fnil\fcharset204 Calibri;}}
{\colortbl ;\red0\green77\blue187;\red155\green0\blue211;\red0\green0\blue0;}
{\*\generator Riched20 10.0.19041}\viewkind4\uc1 
\pard\sl276\slmult1\cf1\f0\fs20\lang9 RestGetStatusCodeAsync\cf0\par
\cf2\f1\lang1049\'ce\'ef\'e8\'f1\'e0\'ed\'e8\'e5\cf0 : \'ec\'e5\'f2\'ee\'e4 \'e2\'fb\'ef\'ee\'eb\'ed\'ff\'e5\'f2 Get Rest \'e7\'e0\'ef\'f0\'ee\'f1 \'e8 \'e2 \'f0\'e5\'e7\'f3\'eb\'fc\'f2\'e0\'f2\'e5 \'ef\'ee\'eb\'f3\'f7\'e0\'e5\'f2 \'ea\'ee\'e4 \'f1\'f2\'e0\'f2\'f3\'f1\'e0\par
\cf2\'d1\'e8\'ed\'f2\'e0\'ea\'f1\'e8\'f1\cf0 : \f0\lang1033 RestGetStatusCodeAsync(string url)\f1\lang1049\par
\cf3\par
\cf2\'cf\'f0\'e8\'ec\'e5\'f0\cf3 :\par
\cf0\f0\lang1033 int statusCode = await tester.RestGetAsync(""https://jsonplaceholder.typicode.com"");\par
tester.ConsoleMsg(statusCode.ToString());\par
}",

@"{\rtf1\ansi\ansicpg1251\deff0\nouicompat\deflang1049{\fonttbl{\f0\fnil\fcharset0 Calibri;}{\f1\fnil\fcharset204 Calibri;}}
{\colortbl ;\red0\green77\blue187;\red155\green0\blue211;\red0\green0\blue0;}
{\*\generator Riched20 10.0.19041}\viewkind4\uc1 
\pard\sl276\slmult1\cf1\f0\fs20\lang9 BrowserClearNetworkAsync\cf0\par
\cf2\f1\lang1049\'ce\'ef\'e8\'f1\'e0\'ed\'e8\'e5\cf0 : \'ec\'e5\'f2\'ee\'e4 \'e2\'fb\'ef\'ee\'eb\'ed\'ff\'e5\'f2 \'ee\'f7\'e8\'f1\'f2\'ea\'f3 \'f1\'ee\'e1\'fb\'f2\'e8\'e9 network \'e2 \'e1\'f0\'e0\'f3\'e7\'e5\'f0\'e5\par
\cf2\'d1\'e8\'ed\'f2\'e0\'ea\'f1\'e8\'f1\cf0 : \f0\lang1033 BrowserClearNetworkAsync()\f1\lang1049\par
\cf3\par
\cf2\'cf\'f0\'e8\'ec\'e5\'f0\cf3 :\par
\cf0\f0\lang1033 string events = await tester.BrowserClearNetworkAsync();\par
tester.ConsoleMsg(events);\par
}",

@"{\rtf1\ansi\ansicpg1251\deff0\nouicompat\deflang1049{\fonttbl{\f0\fnil\fcharset0 Calibri;}{\f1\fnil\fcharset204 Calibri;}}
{\colortbl ;\red0\green77\blue187;\red155\green0\blue211;\red0\green0\blue0;}
{\*\generator Riched20 10.0.19041}\viewkind4\uc1 
\pard\sl276\slmult1\cf1\f0\fs20\lang9 Description\cf0\par
\cf2\f1\lang1049\'ce\'ef\'e8\'f1\'e0\'ed\'e8\'e5\cf0 : \'ec\'e5\'f2\'ee\'e4 \'f3\'f1\'f2\'e0\'ed\'e0\'e2\'eb\'e8\'e2\'e0\'e5\'f2 \'ee\'ef\'e8\'f1\'e0\'ed\'e8\'e5 \'f2\'e5\'f1\'f2\'e0 \'ea\'ee\'f2\'ee\'f0\'ee\'e5 \'ef\'ee\'f2\'ee\'ec \'e2\'fb\'e2\'ee\'e4\'e8\'f2\'ff \'e2 \'ee\'f2\'f7\'e5\'f2\'e5 \'e8 \'ef\'e8\'f1\'fc\'ec\'e5\par
\cf2\'d1\'e8\'ed\'f2\'e0\'ea\'f1\'e8\'f1\cf0 : \f0\lang1033 Description(string text)\f1\lang1049\par
\cf3\par
\cf2\'cf\'f0\'e8\'ec\'e5\'f0\cf3 :\par
\cf0\f0\lang1033 public async Task setUp()\par
\{\par
\tab tester.Description(""\f1\lang1049\'d2\'e5\'f1\'f2 \'ef\'f0\'ee\'e2\'e5\'f0\'ff\'e5\'f2 \'e0\'e2\'f2\'ee\'f0\'e8\'e7\'e0\'f6\'e8\'fe \'ed\'e0 \'f1\'e0\'e9\'f2\'e5"");\par
\}\f0\lang1033\par
}",

@"{\rtf1\ansi\ansicpg1251\deff0\nouicompat\deflang1049{\fonttbl{\f0\fnil\fcharset0 Calibri;}{\f1\fnil\fcharset204 Calibri;}}
{\colortbl ;\red0\green77\blue187;\red155\green0\blue211;\red0\green0\blue0;}
{\*\generator Riched20 10.0.19041}\viewkind4\uc1 
\pard\sl276\slmult1\cf1\f0\fs20\lang9 SendMessageDebug\cf0\par
\cf2\f1\lang1049\'ce\'ef\'e8\'f1\'e0\'ed\'e8\'e5\cf0 : \'ec\'e5\'f2\'ee\'e4 \'e2\'fb\'e2\'ee\'e4\'e8\'f2 \'ee\'f2\'eb\'e0\'e4\'ee\'f7\'ed\'ee\'e5 \'f1\'ee\'ee\'e1\'f9\'e5\'ed\'e8\'e5 \'ea\'ee\'f2\'ee\'f0\'ee\'e5 \'ec\'ee\'e6\'e5\'f2 \'e1\'fb\'f2\'fc \'ee\'f2\'ea\'eb\'fe\'f7\'e5\'ed\'ee \'e4\'eb\'ff \'e2\'fb\'e2\'ee\'e4\'e0 \'e2 \'ee\'f2\'f7\'e5\'f2 \'e8 \'ef\'e8\'f1\'fc\'ec\'ee\par
\cf2\'d1\'e8\'ed\'f2\'e0\'ea\'f1\'e8\'f1\cf0 : SendMessageDebug(string actionRus, string actionEng, string status,  string commentRus, string commentEng, int image)\par
\cf3\par
\cf2\'cf\'f0\'e8\'ec\'e5\'f0\cf3 :\par
\cf0 SendMessageDebug(""\'e4\'e5\'e9\'f1\'f2\'e2\'e8\'ff"", \f0\lang1033 ""action"",\f1\lang1049  \f0\lang1033 Tester.\f1\lang1049 PROCESS, ""\'ea\'ee\'ec\'ec\'e5\'ed\'f2\'e0\'f0\'e8\'e9"", \f0\lang1033 ""comment"", \f1\lang1049  \f0\lang1033 Tester.\f1\lang1049 IMAGE_STATUS_PROCESS);\par

\pard\sa200\sl276\slmult1\f0\fs22\lang9\par
}",

@"{\rtf1\ansi\ansicpg1251\deff0\nouicompat\deflang1049{\fonttbl{\f0\fnil\fcharset0 Calibri;}{\f1\fnil\fcharset204 Calibri;}}
{\colortbl ;\red0\green77\blue187;\red155\green0\blue211;\red0\green0\blue0;}
{\*\generator Riched20 10.0.19041}\viewkind4\uc1 
\pard\sl276\slmult1\cf1\f0\fs20\lang9 EditMessage\lang1033 Debug\cf0\lang9\par
\cf2\f1\lang1049\'ce\'ef\'e8\'f1\'e0\'ed\'e8\'e5\cf0 : \'ec\'e5\'f2\'ee\'e4 \'e8\'e7\'ec\'e5\'ed\'ff\'e5\'f2 \'f0\'e0\'ed\'e5\'e5 \'e2\'fb\'e2\'e5\'e4\'e5\'ed\'ed\'ee\'e5\f0\lang1033  \f1\lang1049\'ee\'f2\'eb\'e0\'e4\'ee\'f7\'ed\'ee\'e5 \'f1\'ee\'ee\'e1\'f9\'e5\'ed\'e8\'e5 \'ea\'ee\'f2\'ee\'f0\'ee\'e5 \'ec\'ee\'e6\'e5\'f2 \'e1\'fb\'f2\'fc \'ee\'f2\'ea\'eb\'fe\'f7\'e5\'ed\'ee \'e4\'eb\'ff \'e2\'fb\'e2\'ee\'e4\'e0 \'e2 \'ee\'f2\'f7\'e5\'f2 \'e8 \'ef\'e8\'f1\'fc\'ec\'ee\par
\cf2\'d1\'e8\'ed\'f2\'e0\'ea\'f1\'e8\'f1\cf0 : EditMessageDebug(int index, string actionRus, string actionEng, string status, string commentRus, string commentEng, int image)\par
\cf3\par
\cf2\'cf\'f0\'e8\'ec\'e5\'f0\cf3 :\par
\cf0\f0\lang1033 tester.\f1\lang1049 EditMessageDebug(step, ""\'e4\'e5\'e9\'f1\'f2\'e2\'e8\'ff"",\f0\lang1033  ""action"", Tester.\f1\lang1049 PASSED, ""\'ea\'ee\'ec\'ec\'e5\'ed\'f2\'e0\'f0\'e8\'e9"", \f0\lang1033 ""comment"", \f1\lang1049  \f0\lang1033 Tester.\f1\lang1049 IMAGE_STATUS_PASSED);\par
\par
\f0\lang1033 tester.\f1\lang1049 EditMessageDebug(step, \f0\lang1033 null\f1\lang1049 ,\f0\lang1033  null,\f1\lang1049  \f0\lang1033 Tester.\f1\lang1049 FAILED, ""\'ea\'ee\'ec\'ec\'e5\'ed\'f2\'e0\'f0\'e8\'e9"", \f0\lang1033 ""comment"",\f1\lang1049  \f0\lang1033 Tester.\f1\lang1049 IMAGE_STATUS_FAILED);\par
\par
\pard\sa200\sl276\slmult1\f0\fs22\lang9\par
}",

@"{\rtf1\ansi\ansicpg1251\deff0\nouicompat\deflang1049{\fonttbl{\f0\fnil\fcharset0 Calibri;}{\f1\fnil\fcharset204 Calibri;}}
{\colortbl ;\red0\green77\blue187;\red155\green0\blue211;\red0\green0\blue0;}
{\*\generator Riched20 10.0.19041}\viewkind4\uc1 
\pard\sl276\slmult1\cf1\f0\fs20\lang9 GetStyleAsync\cf0\par
\cf2\f1\lang1049\'ce\'ef\'e8\'f1\'e0\'ed\'e8\'e5\cf0 : \'ec\'e5\'f2\'ee\'e4 \'e2\'ee\'e7\'e2\'f0\'e0\'f9\'e0\'e5\'f2 \'f1\'f2\'e8\'eb\'fc \'fd\'eb\'e5\'ec\'e5\'ed\'f2\'e0 \'e8\'e7 \'f3\'ea\'e0\'e7\'e0\'ed\'ed\'ee\'e3\'ee \'f1\'e2\'ee\'e9\'f1\'f2\'e2\'e0\par
\cf2\'d1\'e8\'ed\'f2\'e0\'ea\'f1\'e8\'f1\cf0 : GetStyleAsync(string property)\par
\cf3\par
\cf2\'cf\'f0\'e8\'ec\'e5\'f0\cf3 :\par
\cf0\f0\lang1033 string style = await element.GetStyleAsync(""width"");\f1\lang1049\par
\pard\sa200\sl276\slmult1\f0\fs22\lang9\par
}",

@"{\rtf1\ansi\ansicpg1251\deff0\nouicompat\deflang1049{\fonttbl{\f0\fnil\fcharset0 Calibri;}{\f1\fnil\fcharset204 Calibri;}}
{\colortbl ;\red0\green77\blue187;\red155\green0\blue211;\red0\green0\blue0;}
{\*\generator Riched20 10.0.19041}\viewkind4\uc1 
\pard\sl276\slmult1\cf1\f0\fs20\lang9 SetStyleAsync\cf0\par
\cf2\f1\lang1049\'ce\'ef\'e8\'f1\'e0\'ed\'e8\'e5\cf0 : \'ec\'e5\'f2\'ee\'e4 \'f3\'f1\'f2\'e0\'ed\'e0\'e2\'eb\'e8\'e2\'e0\'e5\'f2 \'f1\'f2\'e8\'eb\'fc \'fd\'eb\'e5\'ec\'e5\'ed\'f2\'e0\par
\cf2\'d1\'e8\'ed\'f2\'e0\'ea\'f1\'e8\'f1\cf0 : SetStyleAsync(string cssText)\par
\cf3\par
\cf2\'cf\'f0\'e8\'ec\'e5\'f0\cf3 :\par
\cf0\f0\lang1033 await element.\f1\lang1049 SetStyleAsync\f0\lang1033 (""width: 250px; background-color: #000000;"");\par
}",

@"{\rtf1\ansi\ansicpg1251\deff0\nouicompat\deflang1049{\fonttbl{\f0\fnil\fcharset0 Calibri;}{\f1\fnil\fcharset204 Calibri;}}
{\colortbl ;\red0\green77\blue187;\red155\green0\blue211;\red0\green0\blue0;}
{\*\generator Riched20 10.0.19041}\viewkind4\uc1 
\pard\sl276\slmult1\cf1\f0\fs20\lang9 GetStyleFromElementAsync\cf0\par
\cf2\f1\lang1049\'ce\'ef\'e8\'f1\'e0\'ed\'e8\'e5\cf0 : \'ec\'e5\'f2\'ee\'e4 \'e2\'ee\'e7\'e2\'f0\'e0\'f9\'e0\'e5\'f2 \'f1\'f2\'e8\'eb\'fc \'fd\'eb\'e5\'ec\'e5\'ed\'f2\'e0 \'e8\'e7 \'f3\'ea\'e0\'e7\'e0\'ed\'ed\'ee\'e3\'ee \'f1\'e2\'ee\'e9\'f1\'f2\'e2\'e0\par
\cf2\'d1\'e8\'ed\'f2\'e0\'ea\'f1\'e8\'f1\cf0 : GetStyleFromElementAsync(string by, string locator, string property)\par
\cf3\par
\cf2\'cf\'f0\'e8\'ec\'e5\'f0\cf3 :\par
\cf0\f0\lang1033 FRAMEElement frame = await tester.GetFrameAsync(0);\par
string style = await frame.GetStyleFromElementAsync(Tester.BY_CSS, ""#auth > h2"", ""width"");\f1\lang1049\par
\par
\f0\lang1033 string style = await frame.GetStyleFromElementAsync(Tester.BY_XPATH, ""//div[@id='auth']"", ""width"");\fs22\lang9\par
}",

@"{\rtf1\ansi\ansicpg1251\deff0\nouicompat\deflang1049{\fonttbl{\f0\fnil\fcharset0 Calibri;}{\f1\fnil\fcharset204 Calibri;}}
{\colortbl ;\red0\green77\blue187;\red155\green0\blue211;\red0\green0\blue0;}
{\*\generator Riched20 10.0.19041}\viewkind4\uc1 
\pard\sl276\slmult1\cf1\f0\fs20\lang9 SetStyleInElementAsync\cf0\par
\cf2\f1\lang1049\'ce\'ef\'e8\'f1\'e0\'ed\'e8\'e5\cf0 : \'ec\'e5\'f2\'ee\'e4 \'f3\'f1\'f2\'e0\'ed\'e0\'e2\'eb\'e8\'e2\'e0\'e5\'f2 \'f1\'f2\'e8\'eb\'fc \'fd\'eb\'e5\'ec\'e5\'ed\'f2\'e0\par
\cf2\'d1\'e8\'ed\'f2\'e0\'ea\'f1\'e8\'f1\cf0 : SetStyleInElementAsync(string by, string locator, string cssText)\par
\cf3\par
\cf2\'cf\'f0\'e8\'ec\'e5\'f0\cf3 :\par
\cf0\f0\lang1033 FRAMEElement frame = await tester.GetFrameAsync(0);\par
await frame .SetStyleInElementAsync(Tester.BY_XPATH, ""//div[@id='auth']"", ""background-color: #000000;"");\par
\par
await frame .SetStyleInElementAsync(Tester.BY_CSS, ""#auth"", ""background-color: #000000;"");\par
}",

@"{\rtf1\ansi\ansicpg1251\deff0\nouicompat\deflang1049{\fonttbl{\f0\fnil\fcharset0 Calibri;}{\f1\fnil\fcharset204 Calibri;}}
{\colortbl ;\red0\green77\blue187;\red155\green0\blue211;\red0\green0\blue0;}
{\*\generator Riched20 10.0.19041}\viewkind4\uc1 
\pard\sl276\slmult1\cf1\f0\fs20\lang9 GetStyleFromElementAsync\cf0\par
\cf2\f1\lang1049\'ce\'ef\'e8\'f1\'e0\'ed\'e8\'e5\cf0 : \'ec\'e5\'f2\'ee\'e4 \'e2\'ee\'e7\'e2\'f0\'e0\'f9\'e0\'e5\'f2 \'f1\'f2\'e8\'eb\'fc \'fd\'eb\'e5\'ec\'e5\'ed\'f2\'e0 \'e8\'e7 \'f3\'ea\'e0\'e7\'e0\'ed\'ed\'ee\'e3\'ee \'f1\'e2\'ee\'e9\'f1\'f2\'e2\'e0\par
\cf2\'d1\'e8\'ed\'f2\'e0\'ea\'f1\'e8\'f1\cf0 : GetStyleFromElementAsync(string by, string locator, string property)\par
\cf3\par
\cf2\'cf\'f0\'e8\'ec\'e5\'f0\cf3 :\par
\cf0\f0\lang1033 string style = await tester.GetStyleFromElementAsync(Tester.BY_CSS, ""#auth"", ""padding"");\f1\lang1049\par
\par
\f0\lang1033 string style = await tester.GetStyleFromElementAsync(Tester.BY_XPATH, ""//div[@id='auth']"", ""position"");\fs22\lang9\par
}",

@"{\rtf1\ansi\ansicpg1251\deff0\nouicompat\deflang1049{\fonttbl{\f0\fnil\fcharset0 Calibri;}{\f1\fnil\fcharset204 Calibri;}}
{\colortbl ;\red0\green77\blue187;\red155\green0\blue211;\red0\green0\blue0;}
{\*\generator Riched20 10.0.19041}\viewkind4\uc1 
\pard\sl276\slmult1\cf1\f0\fs20\lang9 GetStyleFromElementByClassAsync\cf0\par
\cf2\f1\lang1049\'ce\'ef\'e8\'f1\'e0\'ed\'e8\'e5\cf0 : \'ec\'e5\'f2\'ee\'e4 \'e2\'ee\'e7\'e2\'f0\'e0\'f9\'e0\'e5\'f2 \'f1\'f2\'e8\'eb\'fc \'fd\'eb\'e5\'ec\'e5\'ed\'f2\'e0 \'e8\'e7 \'f3\'ea\'e0\'e7\'e0\'ed\'ed\'ee\'e3\'ee \'f1\'e2\'ee\'e9\'f1\'f2\'e2\'e0\par
\cf2\'d1\'e8\'ed\'f2\'e0\'ea\'f1\'e8\'f1\cf0 : GetStyleFromElementByClassAsync(string _class, int index, string property)\par
\cf3\par
\cf2\'cf\'f0\'e8\'ec\'e5\'f0\cf3 :\par
\cf0\f0\lang1033 string style = await tester.GetStyleFromElementByClassAsync(""text-field"", 0, ""border"");\par
}",

@"{\rtf1\ansi\ansicpg1251\deff0\nouicompat\deflang1049{\fonttbl{\f0\fnil\fcharset0 Calibri;}{\f1\fnil\fcharset204 Calibri;}}
{\colortbl ;\red0\green77\blue187;\red155\green0\blue211;\red0\green0\blue0;}
{\*\generator Riched20 10.0.19041}\viewkind4\uc1 
\pard\sl276\slmult1\cf1\f0\fs20\lang9 GetStyleFromElementByIdAsync\cf0\par
\cf2\f1\lang1049\'ce\'ef\'e8\'f1\'e0\'ed\'e8\'e5\cf0 : \'ec\'e5\'f2\'ee\'e4 \'e2\'ee\'e7\'e2\'f0\'e0\'f9\'e0\'e5\'f2 \'f1\'f2\'e8\'eb\'fc \'fd\'eb\'e5\'ec\'e5\'ed\'f2\'e0 \'e8\'e7 \'f3\'ea\'e0\'e7\'e0\'ed\'ed\'ee\'e3\'ee \'f1\'e2\'ee\'e9\'f1\'f2\'e2\'e0\par
\cf2\'d1\'e8\'ed\'f2\'e0\'ea\'f1\'e8\'f1\cf0 : GetStyleFromElementByIdAsync(string id, string property)\par
\cf3\par
\cf2\'cf\'f0\'e8\'ec\'e5\'f0\cf3 :\par
\cf0\f0\lang1033 string style = await tester.GetStyleFromElementByIdAsync(""buttonLogin"", ""background-color"");\par
}",

@"{\rtf1\ansi\ansicpg1251\deff0\nouicompat\deflang1049{\fonttbl{\f0\fnil\fcharset0 Calibri;}{\f1\fnil\fcharset204 Calibri;}}
{\colortbl ;\red0\green77\blue187;\red155\green0\blue211;\red0\green0\blue0;}
{\*\generator Riched20 10.0.19041}\viewkind4\uc1 
\pard\sl276\slmult1\cf1\f0\fs20\lang9 GetStyleFromElementByNameAsync\cf0\par
\cf2\f1\lang1049\'ce\'ef\'e8\'f1\'e0\'ed\'e8\'e5\cf0 : \'ec\'e5\'f2\'ee\'e4 \'e2\'ee\'e7\'e2\'f0\'e0\'f9\'e0\'e5\'f2 \'f1\'f2\'e8\'eb\'fc \'fd\'eb\'e5\'ec\'e5\'ed\'f2\'e0 \'e8\'e7 \'f3\'ea\'e0\'e7\'e0\'ed\'ed\'ee\'e3\'ee \'f1\'e2\'ee\'e9\'f1\'f2\'e2\'e0\par
\cf2\'d1\'e8\'ed\'f2\'e0\'ea\'f1\'e8\'f1\cf0 : GetStyleFromElementByNameAsync(string name, int index, string property)\par
\cf3\par
\cf2\'cf\'f0\'e8\'ec\'e5\'f0\cf3 :\par
\cf0\f0\lang1033 string style = await tester.GetStyleFromElementByNameAsync(""pass"", 0, ""height"");\par
}",

@"{\rtf1\ansi\ansicpg1251\deff0\nouicompat\deflang1049{\fonttbl{\f0\fnil\fcharset0 Calibri;}{\f1\fnil\fcharset204 Calibri;}}
{\colortbl ;\red0\green77\blue187;\red155\green0\blue211;\red0\green0\blue0;}
{\*\generator Riched20 10.0.19041}\viewkind4\uc1 
\pard\sl276\slmult1\cf1\f0\fs20\lang9 GetStyleFromElementByTagAsync\cf0\par
\cf2\f1\lang1049\'ce\'ef\'e8\'f1\'e0\'ed\'e8\'e5\cf0 : \'ec\'e5\'f2\'ee\'e4 \'e2\'ee\'e7\'e2\'f0\'e0\'f9\'e0\'e5\'f2 \'f1\'f2\'e8\'eb\'fc \'fd\'eb\'e5\'ec\'e5\'ed\'f2\'e0 \'e8\'e7 \'f3\'ea\'e0\'e7\'e0\'ed\'ed\'ee\'e3\'ee \'f1\'e2\'ee\'e9\'f1\'f2\'e2\'e0\par
\cf2\'d1\'e8\'ed\'f2\'e0\'ea\'f1\'e8\'f1\cf0 : GetStyleFromElementByTagAsync(string tag, int index, string property)\par
\cf3\par
\cf2\'cf\'f0\'e8\'ec\'e5\'f0\cf3 :\par
\cf0\f0\lang1033 string style = await tester.GetStyleFromElementByTagAsync(""h2"", 0, ""width"");\par
}",

@"{\rtf1\ansi\ansicpg1251\deff0\nouicompat\deflang1049{\fonttbl{\f0\fnil\fcharset0 Calibri;}{\f1\fnil\fcharset204 Calibri;}}
{\colortbl ;\red0\green77\blue187;\red155\green0\blue211;\red0\green0\blue0;}
{\*\generator Riched20 10.0.19041}\viewkind4\uc1 
\pard\sl276\slmult1\cf1\f0\fs20\lang9 SetStyleInElementAsync\cf0\par
\cf2\f1\lang1049\'ce\'ef\'e8\'f1\'e0\'ed\'e8\'e5\cf0 : \'ec\'e5\'f2\'ee\'e4 \'f3\'f1\'f2\'e0\'ed\'e0\'e2\'eb\'e8\'e2\'e0\'e5\'f2 \'f1\'f2\'e8\'eb\'fc \'fd\'eb\'e5\'ec\'e5\'ed\'f2\'e0\par
\cf2\'d1\'e8\'ed\'f2\'e0\'ea\'f1\'e8\'f1\cf0 : SetStyleInElementAsync(string by, string locator, string cssText)\par
\cf3\par
\cf2\'cf\'f0\'e8\'ec\'e5\'f0\cf3 :\par
\cf0\f0\lang1033 await tester.SetStyleInElementAsync(Tester.BY_XPATH, ""//div[@id='auth']"", ""width: 250px; color: white; background-color: #000000;"");\par
\par
await tester.SetStyleInElementAsync(Tester.BY_CSS, ""#auth"", ""width: 250px; color: white; background-color: #000000;"");\par
}",

@"{\rtf1\ansi\ansicpg1251\deff0\nouicompat\deflang1049{\fonttbl{\f0\fnil\fcharset0 Calibri;}{\f1\fnil\fcharset204 Calibri;}}
{\colortbl ;\red0\green77\blue187;\red155\green0\blue211;\red0\green0\blue0;}
{\*\generator Riched20 10.0.19041}\viewkind4\uc1 
\pard\sl276\slmult1\cf1\f0\fs20\lang9 SetStyleInElementByClassAsync\cf0\par
\cf2\f1\lang1049\'ce\'ef\'e8\'f1\'e0\'ed\'e8\'e5\cf0 : \'ec\'e5\'f2\'ee\'e4 \'f3\'f1\'f2\'e0\'ed\'e0\'e2\'eb\'e8\'e2\'e0\'e5\'f2 \'f1\'f2\'e8\'eb\'fc \'fd\'eb\'e5\'ec\'e5\'ed\'f2\'e0\par
\cf2\'d1\'e8\'ed\'f2\'e0\'ea\'f1\'e8\'f1\cf0 : SetStyleInElementByClassAsync(string _class, int index, string cssText)\par
\cf3\par
\cf2\'cf\'f0\'e8\'ec\'e5\'f0\cf3 :\par
\cf0\f0\lang1033 await tester.SetStyleInElementByClassAsync(""text-field"", 0, ""background-color: #123456;"");\par
}",

@"{\rtf1\ansi\ansicpg1251\deff0\nouicompat\deflang1049{\fonttbl{\f0\fnil\fcharset0 Calibri;}{\f1\fnil\fcharset204 Calibri;}}
{\colortbl ;\red0\green77\blue187;\red155\green0\blue211;\red0\green0\blue0;}
{\*\generator Riched20 10.0.19041}\viewkind4\uc1 
\pard\sl276\slmult1\cf1\f0\fs20\lang9 SetStyleInElementByIdAsync\cf0\par
\cf2\f1\lang1049\'ce\'ef\'e8\'f1\'e0\'ed\'e8\'e5\cf0 : \'ec\'e5\'f2\'ee\'e4 \'f3\'f1\'f2\'e0\'ed\'e0\'e2\'eb\'e8\'e2\'e0\'e5\'f2 \'f1\'f2\'e8\'eb\'fc \'fd\'eb\'e5\'ec\'e5\'ed\'f2\'e0\par
\cf2\'d1\'e8\'ed\'f2\'e0\'ea\'f1\'e8\'f1\cf0 : SetStyleInElementByIdAsync(string id, string cssText)\par
\cf3\par
\cf2\'cf\'f0\'e8\'ec\'e5\'f0\cf3 :\par
\cf0\f0\lang1033 await tester.SetStyleInElementByIdAsync(""buttonLogin"", ""background-color: #123456;"");\par
}",

@"{\rtf1\ansi\ansicpg1251\deff0\nouicompat\deflang1049{\fonttbl{\f0\fnil\fcharset0 Calibri;}{\f1\fnil\fcharset204 Calibri;}}
{\colortbl ;\red0\green77\blue187;\red155\green0\blue211;\red0\green0\blue0;}
{\*\generator Riched20 10.0.19041}\viewkind4\uc1 
\pard\sl276\slmult1\cf1\f0\fs20\lang9 SetStyleInElementByNameAsync\cf0\par
\cf2\f1\lang1049\'ce\'ef\'e8\'f1\'e0\'ed\'e8\'e5\cf0 : \'ec\'e5\'f2\'ee\'e4 \'f3\'f1\'f2\'e0\'ed\'e0\'e2\'eb\'e8\'e2\'e0\'e5\'f2 \'f1\'f2\'e8\'eb\'fc \'fd\'eb\'e5\'ec\'e5\'ed\'f2\'e0\par
\cf2\'d1\'e8\'ed\'f2\'e0\'ea\'f1\'e8\'f1\cf0 : SetStyleInElementByNameAsync(string name, int index, string cssText)\par
\cf3\par
\cf2\'cf\'f0\'e8\'ec\'e5\'f0\cf3 :\par
\cf0\f0\lang1033 await tester.SetStyleInElementByNameAsync(""pass"", 0, ""background-color: #123456;"");\par
}",

@"{\rtf1\ansi\ansicpg1251\deff0\nouicompat\deflang1049{\fonttbl{\f0\fnil\fcharset0 Calibri;}{\f1\fnil\fcharset204 Calibri;}}
{\colortbl ;\red0\green77\blue187;\red155\green0\blue211;\red0\green0\blue0;}
{\*\generator Riched20 10.0.19041}\viewkind4\uc1 
\pard\sl276\slmult1\cf1\f0\fs20\lang9 SetStyleInElementByTagAsync\cf0\par
\cf2\f1\lang1049\'ce\'ef\'e8\'f1\'e0\'ed\'e8\'e5\cf0 : \'ec\'e5\'f2\'ee\'e4 \'f3\'f1\'f2\'e0\'ed\'e0\'e2\'eb\'e8\'e2\'e0\'e5\'f2 \'f1\'f2\'e8\'eb\'fc \'fd\'eb\'e5\'ec\'e5\'ed\'f2\'e0\par
\cf2\'d1\'e8\'ed\'f2\'e0\'ea\'f1\'e8\'f1\cf0 : SetStyleInElementByTagAsync(string tag, int index, string cssText)\par
\cf3\par
\cf2\'cf\'f0\'e8\'ec\'e5\'f0\cf3 :\par
\cf0\f0\lang1033 await tester.SetStyleInElementByTagAsync(""h2"", 0, ""background-color: #123456;"");\par
}",

@"{\rtf1\ansi\ansicpg1251\deff0\nouicompat\deflang1049{\fonttbl{\f0\fnil\fcharset0 Calibri;}{\f1\fnil\fcharset204 Calibri;}}
{\colortbl ;\red0\green77\blue187;\red155\green0\blue211;\red0\green0\blue0;}
{\*\generator Riched20 10.0.19041}\viewkind4\uc1 
\pard\sl276\slmult1\cf1\f0\fs20\lang9 DEFAULT\cf0\par
\cf2\f1\lang1049\'ce\'ef\'e8\'f1\'e0\'ed\'e8\'e5\cf0 : \'ea\'ee\'ed\'f1\'f2\'e0\'ed\'f2\'e0 \'ee\'e1\'ee\'e7\'ed\'e0\'f7\'e0\'e5\'f2 \'f2\'e8\'ef \'ea\'ee\'e4\'e8\'f0\'ee\'e2\'ea\'e8 \'e4\'eb\'ff \'f4\'e0\'e9\'eb\'e0\par
\cf2\'d1\'e8\'ed\'f2\'e0\'ea\'f1\'e8\'f1\cf0 : DEFAULT = ""DEFAULT""\par
\cf3\par
\cf2\'cf\'f0\'e8\'ec\'e5\'f0\cf3 :\par
\cf0\f0\lang1033 string text = await tester.FileReadAsync(Tester.\f1\lang1049 DEFAULT\f0\lang1033 , ""C:\\\\Hat\\\\file.txt"");\par
await tester.FileWriteAsync(text, Tester.\f1\lang1049 DEFAULT\f0\lang1033 , ""C:\\\\Hat\\\\file_copy.txt"");\par
}",

@"{\rtf1\ansi\ansicpg1251\deff0\nouicompat\deflang1049{\fonttbl{\f0\fnil\fcharset0 Calibri;}{\f1\fnil\fcharset204 Calibri;}}
{\colortbl ;\red0\green77\blue187;\red155\green0\blue211;\red0\green0\blue0;}
{\*\generator Riched20 10.0.19041}\viewkind4\uc1 
\pard\sl276\slmult1\cf1\f0\fs20\lang9 UTF8\cf0\par
\cf2\f1\lang1049\'ce\'ef\'e8\'f1\'e0\'ed\'e8\'e5\cf0 : \'ea\'ee\'ed\'f1\'f2\'e0\'ed\'f2\'e0 \'ee\'e1\'ee\'e7\'ed\'e0\'f7\'e0\'e5\'f2 \'f2\'e8\'ef \'ea\'ee\'e4\'e8\'f0\'ee\'e2\'ea\'e8 \'e4\'eb\'ff \'f4\'e0\'e9\'eb\'e0\par
\cf2\'d1\'e8\'ed\'f2\'e0\'ea\'f1\'e8\'f1\cf0 : UTF8 = ""UTF-8""\par
\cf3\par
\cf2\'cf\'f0\'e8\'ec\'e5\'f0\cf3 :\par
\cf0\f0\lang1033 string text = await tester.FileReadAsync(Tester.\f1\lang1049 UTF8\f0\lang1033 , ""C:\\\\Hat\\\\file.txt"");\par
await tester.FileWriteAsync(text, Tester.\f1\lang1049 UTF8\f0\lang1033 , ""C:\\\\Hat\\\\file_copy.txt"");\par
}",

@"{\rtf1\ansi\ansicpg1251\deff0\nouicompat\deflang1049{\fonttbl{\f0\fnil\fcharset0 Calibri;}{\f1\fnil\fcharset204 Calibri;}}
{\colortbl ;\red0\green77\blue187;\red155\green0\blue211;\red0\green0\blue0;}
{\*\generator Riched20 10.0.19041}\viewkind4\uc1 
\pard\sl276\slmult1\cf1\f0\fs20\lang9 UTF8BOM\cf0\par
\cf2\f1\lang1049\'ce\'ef\'e8\'f1\'e0\'ed\'e8\'e5\cf0 : \'ea\'ee\'ed\'f1\'f2\'e0\'ed\'f2\'e0 \'ee\'e1\'ee\'e7\'ed\'e0\'f7\'e0\'e5\'f2 \'f2\'e8\'ef \'ea\'ee\'e4\'e8\'f0\'ee\'e2\'ea\'e8 \'e4\'eb\'ff \'f4\'e0\'e9\'eb\'e0\par
\cf2\'d1\'e8\'ed\'f2\'e0\'ea\'f1\'e8\'f1\cf0 : UTF8BOM = ""UTF-8 BOM""\par
\cf3\par
\cf2\'cf\'f0\'e8\'ec\'e5\'f0\cf3 :\par
\cf0\f0\lang1033 string text = await tester.FileReadAsync(Tester.\f1\lang1049 UTF8BOM\f0\lang1033 , ""C:\\\\Hat\\\\file.txt"");\par
await tester.FileWriteAsync(text, Tester.\f1\lang1049 UTF8BOM\f0\lang1033 , ""C:\\\\Hat\\\\file_copy.txt"");\par
}",

@"{\rtf1\ansi\ansicpg1251\deff0\nouicompat\deflang1049{\fonttbl{\f0\fnil\fcharset0 Calibri;}{\f1\fnil\fcharset204 Calibri;}}
{\colortbl ;\red0\green77\blue187;\red155\green0\blue211;\red0\green0\blue0;}
{\*\generator Riched20 10.0.19041}\viewkind4\uc1 
\pard\sl276\slmult1\cf1\f0\fs20\lang9 WINDOWS1251\cf0\par
\cf2\f1\lang1049\'ce\'ef\'e8\'f1\'e0\'ed\'e8\'e5\cf0 : \'ea\'ee\'ed\'f1\'f2\'e0\'ed\'f2\'e0 \'ee\'e1\'ee\'e7\'ed\'e0\'f7\'e0\'e5\'f2 \'f2\'e8\'ef \'ea\'ee\'e4\'e8\'f0\'ee\'e2\'ea\'e8 \'e4\'eb\'ff \'f4\'e0\'e9\'eb\'e0\par
\cf2\'d1\'e8\'ed\'f2\'e0\'ea\'f1\'e8\'f1\cf0 : WINDOWS1251 = ""WINDOWS-1251""\par
\cf3\par
\cf2\'cf\'f0\'e8\'ec\'e5\'f0\cf3 :\par
\cf0\f0\lang1033 string text = await tester.FileReadAsync(Tester.\f1\lang1049 WINDOWS1251\f0\lang1033 , ""C:\\\\Hat\\\\file.txt"");\par
await tester.FileWriteAsync(text, Tester.\f1\lang1049 WINDOWS1251\f0\lang1033 , ""C:\\\\Hat\\\\file_copy.txt"");\par
}",

@"{\rtf1\ansi\ansicpg1251\deff0\nouicompat\deflang1049{\fonttbl{\f0\fnil\fcharset0 Calibri;}{\f1\fnil\fcharset204 Calibri;}}
{\colortbl ;\red0\green77\blue187;\red155\green0\blue211;\red0\green0\blue0;}
{\*\generator Riched20 10.0.19041}\viewkind4\uc1 
\pard\sl276\slmult1\cf1\f0\fs20\lang9 FileReadAsync\cf0\par
\cf2\f1\lang1049\'ce\'ef\'e8\'f1\'e0\'ed\'e8\'e5\cf0 : \'ec\'e5\'f2\'ee\'e4 \'e2\'fb\'ef\'ee\'eb\'ed\'ff\'e5\'f2 \'f7\'f2\'e5\'ed\'e8\'e5 \'f3\'ea\'e0\'e7\'e0\'ed\'ed\'ee\'e3\'ee \'f4\'e0\'e9\'eb\'e0\par
\cf2\'d1\'e8\'ed\'f2\'e0\'ea\'f1\'e8\'f1\cf0 : FileReadAsync(string encoding, string filename)\par
\cf3\par
\cf2\'cf\'f0\'e8\'ec\'e5\'f0\cf3 :\par
\cf0\f0\lang1033 string text = await tester.FileReadAsync(Tester.\f1\lang1049 UTF8\f0\lang1033 , ""C:\\\\Hat\\\\file.txt"");\par
}",

@"{\rtf1\ansi\ansicpg1251\deff0\nouicompat\deflang1049{\fonttbl{\f0\fnil\fcharset0 Calibri;}{\f1\fnil\fcharset204 Calibri;}}
{\colortbl ;\red0\green77\blue187;\red155\green0\blue211;\red0\green0\blue0;}
{\*\generator Riched20 10.0.19041}\viewkind4\uc1 
\pard\sl276\slmult1\cf1\f0\fs20\lang9 FileWriteAsync\cf0\par
\cf2\f1\lang1049\'ce\'ef\'e8\'f1\'e0\'ed\'e8\'e5\cf0 : \'ec\'e5\'f2\'ee\'e4 \'e2\'fb\'ef\'ee\'eb\'ed\'ff\'e5\'f2 \'e7\'e0\'ef\'e8\'f1\'fc \'f2\'e5\'ea\'f1\'f2\'e0 \'e2 \'f3\'ea\'e0\'e7\'e0\'ed\'ed\'fb\'e9 \'f4\'e0\'e9\'eb\'e0\par
\cf2\'d1\'e8\'ed\'f2\'e0\'ea\'f1\'e8\'f1\cf0 : FileWriteAsync(string content, string encoding, string filename)\par
\cf3\par
\cf2\'cf\'f0\'e8\'ec\'e5\'f0\cf3 :\par
\cf0\f0\lang1033 string text = ""my text"";\par
await tester.FileWriteAsync(text, Tester.UTF8, ""C:\\\\Hat\\\\file.txt"");\par
}",

@"{\rtf1\ansi\ansicpg1251\deff0\nouicompat\deflang1049{\fonttbl{\f0\fnil\fcharset0 Calibri;}{\f1\fnil\fcharset204 Calibri;}}
{\colortbl ;\red0\green77\blue187;\red155\green0\blue211;\red0\green0\blue0;}
{\*\generator Riched20 10.0.19041}\viewkind4\uc1 
\pard\sl276\slmult1\cf1\f0\fs20\lang9 FileDownloadAsync\cf0\par
\cf2\f1\lang1049\'ce\'ef\'e8\'f1\'e0\'ed\'e8\'e5\cf0 : \'ec\'e5\'f2\'ee\'e4 \'f1\'ea\'e0\'f7\'e8\'e2\'e0\'e5\'f2 \'f4\'e0\'e9\'eb\'e0 \'ef\'ee \'f3\'ea\'e0\'e7\'e0\'ed\'ed\'ee\'ec\'f3 \f0\lang1033 URL\f1\lang1049\par
\cf2\'d1\'e8\'ed\'f2\'e0\'ea\'f1\'e8\'f1\cf0 : FileDownloadAsync(string fileURL, string filename, int waitingSec = 60)\par
\cf3\par
\cf2\'cf\'f0\'e8\'ec\'e5\'f0\cf3 :\par
\cf0\f0\lang1033 await tester.FileDownloadAsync(""https://somovstudio.github.io/img/logo.png"", \par
""C:\\\\download\\\\logo.png"", 60);\par
}",

@"{\rtf1\ansi\ansicpg1251\deff0\nouicompat\deflang1049{\fonttbl{\f0\fnil\fcharset0 Calibri;}{\f1\fnil\fcharset204 Calibri;}}
{\colortbl ;\red0\green77\blue187;\red155\green0\blue211;\red0\green0\blue0;}
{\*\generator Riched20 10.0.19041}\viewkind4\uc1 
\pard\sl276\slmult1\cf1\f0\fs20\lang9 FileGetHashMD5Async\cf0\par
\cf2\f1\lang1049\'ce\'ef\'e8\'f1\'e0\'ed\'e8\'e5\cf0 : \'ec\'e5\'f2\'ee\'e4 \'ee\'ef\'f0\'e5\'e4\'e5\'eb\'ff\'e5\'f2 \f0\lang1033 HashMD5 \f1\lang1049\'ea\'ee\'e4 \'f3\'ea\'e0\'e7\'e0\'ed\'ed\'ee\'e3\'ee \'f4\'e0\'e9\'eb\'e0\par
\cf2\'d1\'e8\'ed\'f2\'e0\'ea\'f1\'e8\'f1\cf0 : FileGetHashMD5Async(string filename)\par
\cf3\par
\cf2\'cf\'f0\'e8\'ec\'e5\'f0\cf3 :\par
\cf0\f0\lang1033 string hash = await tester.FileGetHashMD5Async(""C:\\\\download\\\\logo.png"");\par
}",

@"{\rtf1\ansi\ansicpg1251\deff0\nouicompat\deflang1049{\fonttbl{\f0\fnil\fcharset0 Calibri;}{\f1\fnil\fcharset204 Calibri;}}
{\colortbl ;\red0\green77\blue187;\red155\green0\blue211;\red0\green0\blue0;}
{\*\generator Riched20 10.0.19041}\viewkind4\uc1 
\pard\sl276\slmult1\cf1\f0\fs20\lang9 CreateHashMD5FromTextAsync\cf0\par
\cf2\f1\lang1049\'ce\'ef\'e8\'f1\'e0\'ed\'e8\'e5\cf0 : \'ec\'e5\'f2\'ee\'e4 \'f1\'ee\'e7\'e4\'e0\'e5\'f2 \f0\lang1033 HashDM5 \f1\lang1049\'ea\'ee\'e4 \'e8\'e7 \'f3\'ea\'e0\'e7\'e0\'ed\'ed\'ee\'e3\'ee \'f2\'e5\'ea\'f1\'f2\'e0\par
\cf2\'d1\'e8\'ed\'f2\'e0\'ea\'f1\'e8\'f1\cf0 : CreateHashMD5FromTextAsync(string text)\par
\cf3\par
\cf2\'cf\'f0\'e8\'ec\'e5\'f0\cf3 :\par
\cf0\f0\lang1033 string text = ""Hello World"";\par
string hash = await tester.CreateHashMD5FromTextAsync(text);\par
tester.ConsoleMsg(hash);\par
}",

@"{\rtf1\ansi\ansicpg1251\deff0\nouicompat\deflang1049{\fonttbl{\f0\fnil\fcharset0 Calibri;}{\f1\fnil\fcharset204 Calibri;}}
{\colortbl ;\red0\green77\blue187;\red155\green0\blue211;\red0\green0\blue0;}
{\*\generator Riched20 10.0.19041}\viewkind4\uc1 
\pard\sl276\slmult1\cf1\f0\fs20\lang9 DisableDebugInReport\cf0\par
\cf2\f1\lang1049\'ce\'ef\'e8\'f1\'e0\'ed\'e8\'e5\cf0 : \'ec\'e5\'f2\'ee\'e4 \'ee\'f2\'ea\'eb\'fe\'f7\'e0\'e5\'f2 \'e2\'fb\'e2\'ee\'e4 \'ee\'f2\'eb\'e0\'e4\'ee\'f7\'ed\'fb\'f5 \'f1\'ee\'ee\'e1\'f9\'e5\'ed\'e8\'e9 SendMessageDebug \'e2 \'ee\'f2\'f7\'e5\'f2\par
\cf2\'d1\'e8\'ed\'f2\'e0\'ea\'f1\'e8\'f1\cf0 : DisableDebugInReport()\par
\cf3\par
\cf2\'cf\'f0\'e8\'ec\'e5\'f0\cf3 :\par
\cf0\f0\lang1033 tester.DisableDebugInReport();\par
}",

@"{\rtf1\ansi\ansicpg1251\deff0\nouicompat\deflang1049{\fonttbl{\f0\fnil\fcharset0 Calibri;}{\f1\fnil\fcharset204 Calibri;}}
{\colortbl ;\red0\green77\blue187;\red155\green0\blue211;\red0\green0\blue0;}
{\*\generator Riched20 10.0.19041}\viewkind4\uc1 
\pard\sl276\slmult1\cf1\f0\fs20\lang9 IsVisibleElementAsync\cf0\par
\cf2\f1\lang1049\'ce\'ef\'e8\'f1\'e0\'ed\'e8\'e5\cf0 : \'ec\'e5\'f2\'ee\'e4 \'ee\'ef\'f0\'e5\'e4\'e5\'eb\'ff\'e5\'f2 \'e2\'e8\'e4\'e8\'ec\'ee\'f1\'f2\'fc \'fd\'eb\'e5\'ec\'e5\'ed\'f2\'e0 \'e8 \'e2\'ee\'e7\'e2\'f0\'e0\'f9\'e0\'e5\'f2 \'e7\'ed\'e0\'f7\'e5\'ed\'e8\'e5 \f0\lang1033 true \f1\lang1049\'e8\'eb\'e8 \f0\lang1033 false\f1\lang1049\par
\cf2\'d1\'e8\'ed\'f2\'e0\'ea\'f1\'e8\'f1\cf0 : IsVisibleElementAsync(string by, string locator)\par
\cf3\par
\cf2\'cf\'f0\'e8\'ec\'e5\'f0\cf3 :\par
\cf0\f0\lang1033 bool result = await tester.IsVisibleElementAsync(Tester.BY_XPATH, ""//*[@id='login']"");\par
\par
bool result = await tester.IsVisibleElementAsync(Tester.BY_CSS, ""#login"");\par
}",

@"{\rtf1\ansi\ansicpg1251\deff0\nouicompat\deflang1049{\fonttbl{\f0\fnil\fcharset0 Calibri;}{\f1\fnil\fcharset204 Calibri;}}
{\colortbl ;\red0\green77\blue187;\red155\green0\blue211;\red0\green0\blue0;}
{\*\generator Riched20 10.0.19041}\viewkind4\uc1 
\pard\sl276\slmult1\cf1\f0\fs20\lang9 IsVisibleElementAsync\cf0\par
\cf2\f1\lang1049\'ce\'ef\'e8\'f1\'e0\'ed\'e8\'e5\cf0 : \'ec\'e5\'f2\'ee\'e4 \'ee\'ef\'f0\'e5\'e4\'e5\'eb\'ff\'e5\'f2 \'e2\'e8\'e4\'e8\'ec\'ee\'f1\'f2\'fc \'fd\'eb\'e5\'ec\'e5\'ed\'f2\'e0 \'e8 \'e2\'ee\'e7\'e2\'f0\'e0\'f9\'e0\'e5\'f2 \'e7\'ed\'e0\'f7\'e5\'ed\'e8\'e5 \f0\lang1033 true \f1\lang1049\'e8\'eb\'e8 \f0\lang1033 false\f1\lang1049\par
\cf2\'d1\'e8\'ed\'f2\'e0\'ea\'f1\'e8\'f1\cf0 : IsVisibleElementAsync(string by, string locator)\par
\cf3\par
\cf2\'cf\'f0\'e8\'ec\'e5\'f0\cf3 :\par
\cf0\f0\lang1033 FRAMEElement frame = await tester.GetFrameAsync(0);\par
\par
bool result = await frame.IsVisibleElementAsync(Tester.BY_XPATH, ""//*[@id='login']"");\par
\par
bool result = await frame.IsVisibleElementAsync(Tester.BY_CSS, ""#login"");\par
\par
await frame.AssertTrueAsync(result);\par
}",

@"",
@"",
@"",
@"",
@"",
@""
        };

        private void CodeEditorForm_Load(object sender, EventArgs e)
        {
            try
            {
                files = new List<object[]>();
                this.TopMost = Config.editorTopMost;
                toolStripStatusLabel2.Text = Config.encoding;
            }
            catch (Exception ex)
            {
                parent.ConsoleMsgError(ex.ToString());
            }

            /*
            // Removes the selected tab:  
            tabControl1.TabPages.Remove(tabControl1.SelectedTab);
            // Removes all the tabs:  
            tabControl1.TabPages.Clear();
            */
        }

        private void CodeEditorForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            parent.СloseCodeEditor();
        }

        private void CodeEditorForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            // проверка несохранённых файлов
            try
            {
                int count = files.Count;
                if (count > 0)
                {
                    for (int i = 0; i < count; i++)
                    {
                        if (files[i][2].ToString() == STATUS_NOT_SAVE)
                        {
                            e.Cancel = true;
                            break;
                        }
                    }

                    if (e.Cancel == true)
                    {
                        DialogResult dialogResult = MessageBox.Show($"Не все открытые файлы сохранены. {Environment.NewLine}Вы хотите сохранить изменения в файлах?", "Вопрос", MessageBoxButtons.YesNoCancel);

                        if (dialogResult == DialogResult.Cancel)
                        {
                            e.Cancel = true;
                        }
                        else if (dialogResult == DialogResult.No)
                        {
                            e.Cancel = false;
                        }
                        else if (dialogResult == DialogResult.Yes)
                        {
                            saveFileAll();
                            e.Cancel = false;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                parent.ConsoleMsgError(ex.ToString());
            }
        }

        public void OpenFile(string filename, string path)
        {
            try
            {
                int count = files.Count;
                if (count > 0)
                {
                    for (int i = 0; i < count; i++)
                    {
                        if (files[i][1].ToString() == path)
                        {
                            parent.ConsoleMsg($"Файл {filename} уже открыт в редакторе");
                            return;
                        }
                    }
                }

                int index = tabControl1.TabPages.Count;
                
                WorkOnFiles reader = new WorkOnFiles();

                ElementHost host = new ElementHost();
                host.Dock = DockStyle.Fill;

                TextEditor textEditorControl = new TextEditor();
                textEditorControl.SyntaxHighlighting = ICSharpCode.AvalonEdit.Highlighting.HighlightingManager.Instance.GetDefinition("C#");
                textEditorControl.ShowLineNumbers= true;
                textEditorControl.Tag = index.ToString();
                textEditorControl.Name = "textEditorControl" + index.ToString();
                textEditorControl.Text = reader.readFile(Config.encoding, path);
                textEditorControl.FontFamily = new System.Windows.Media.FontFamily("Consolas");
                textEditorControl.FontSize = 14;
                textEditorControl.TextChanged += new System.EventHandler(this.textEditorControl_TextChanged);
                textEditorControl.KeyDown += new System.Windows.Input.KeyEventHandler(textEditorControl_KeyDown);
                textEditorControl.TextArea.TextEntering += textEditor_TextArea_TextEntering;
                textEditorControl.TextArea.TextEntered += textEditor_TextArea_TextEntered;
                SearchPanel.Install(textEditorControl);

                host.Child = textEditorControl;

                TabPage tab = new TabPage(filename);
                tab.Controls.Add(host);
                tabControl1.TabPages.Add(tab);

                // [ 0 - имя файла | 1 - путь файла | 2 - статус | 3 - индекс | 4 - TabPage (вкладка) | 5 - TextEditorControl (редактор)]
                files.Add(new object[] { filename, path, STATUS_SAVED, index, tab, textEditorControl });

                toolStripStatusLabel5.Text = path;
            }
            catch (Exception ex)
            {
                parent.ConsoleMsgError(ex.ToString());
            }
        }

        private void textEditorControl_KeyDown(object sender, System.Windows.Input.KeyEventArgs e)
        {
            if (e.Key == Key.S && ModifierKeys == Keys.Control)
            {
                e.Handled = true;
                saveFile();
            }
            if (e.Key == Key.Space && ModifierKeys == Keys.Control)
            {
                e.Handled = true;
                showCompletionWindow(((TextEditor)sender).TextArea);
            }
        }

        private void textEditorControl_TextChanged(object sender, EventArgs e)
        {
            TextEditor textEditorControl = (TextEditor)sender;
            int index = Convert.ToInt32(textEditorControl.Tag);
            files[index][2] = STATUS_NOT_SAVE;
            (files[index][4] as TabPage).Text = files[index][0].ToString() + " *";
        }

        private void showCompletionWindow(TextArea textArea)
        {
            completionWindow = new CompletionWindow(textArea);
            IList<ICompletionData> data = completionWindow.CompletionList.CompletionData;
            data.Add(new CompletionData("AssertEqualsAsync", "AssertEqualsAsync(dynamic expected, dynamic actual)"));
            data.Add(new CompletionData("AssertFalseAsync", "AssertFalseAsync(bool condition)"));
            data.Add(new CompletionData("AssertNetworkEventsAsync", "AssertNetworkEventsAsync(bool presence, string[] events)"));
            data.Add(new CompletionData("AssertNoErrorsAsync", "AssertNoErrorsAsync(bool showListErrors = false, string[] listIgnored = null)"));
            data.Add(new CompletionData("AssertNotEqualsAsync", "AssertNotEqualsAsync(dynamic expected, dynamic actual)"));
            data.Add(new CompletionData("AssertNotNullAsync", "AssertNotNullAsync(dynamic obj)"));
            data.Add(new CompletionData("AssertNullAsync", "AssertNullAsync(dynamic obj)"));
            data.Add(new CompletionData("AssertTrueAsync", "AssertTrueAsync(bool condition)"));
            data.Add(new CompletionData("BrowserBasicAuthenticationAsync", "BrowserBasicAuthenticationAsync(string user, string pass)"));
            data.Add(new CompletionData("BrowserClearNetworkAsync", "BrowserClearNetworkAsync()"));
            data.Add(new CompletionData("BrowserCloseAsync", "BrowserCloseAsync()"));
            data.Add(new CompletionData("BrowserEnableSendMailAsync", "BrowserEnableSendMailAsync(bool byFailure = true, bool bySuccess = true)"));
            data.Add(new CompletionData("BrowserFullScreenAsync", "BrowserFullScreenAsync()"));
            data.Add(new CompletionData("BrowserGetErrorsAsync", "BrowserGetErrorsAsync()"));
            data.Add(new CompletionData("BrowserGetNetworkAsync", "BrowserGetNetworkAsync()"));
            data.Add(new CompletionData("BrowserGetUserAgentAsync", "BrowserGetUserAgentAsync()"));
            data.Add(new CompletionData("BrowserGoBackAsync", "BrowserGoBackAsync(int sec, bool abortLoadAfterTime = false)"));
            data.Add(new CompletionData("BrowserGoForwardAsync", "BrowserGoForwardAsync(int sec, bool abortLoadAfterTime = false)"));
            data.Add(new CompletionData("BrowserPageReloadAsync", "BrowserPageReloadAsync(int sec, bool abortLoadAfterTime = false)"));
            data.Add(new CompletionData("BrowserSetUserAgentAsync", "BrowserSetUserAgentAsync(string value)"));
            data.Add(new CompletionData("BrowserSizeAsync", "BrowserSizeAsync(int width, int height)"));
            data.Add(new CompletionData("BrowserView", "WebView2 BrowserView"));
            data.Add(new CompletionData("BrowserWindow", "Form BrowserWindow"));
            data.Add(new CompletionData("BY_CSS", "BY_CSS"));
            data.Add(new CompletionData("BY_INDEX", "BY_INDEX"));
            data.Add(new CompletionData("BY_TEXT", "BY_TEXT"));
            data.Add(new CompletionData("BY_VALUE", "BY_VALUE"));
            data.Add(new CompletionData("BY_XPATH", "BY_XPATH"));
            data.Add(new CompletionData("Class", "string Class { get; set; }"));
            data.Add(new CompletionData("ClearMessage", "ClearMessage()"));
            data.Add(new CompletionData("ClickAsync", "ClickAsync()"));
            data.Add(new CompletionData("ClickElementAsync", "ClickElementAsync(string by, string locator)"));
            data.Add(new CompletionData("ClickElementByClassAsync", "ClickElementByClassAsync(string _class, int index);"));
            data.Add(new CompletionData("ClickElementByIdAsync", "ClickElementByIdAsync(string id)"));
            data.Add(new CompletionData("ClickElementByNameAsync", "ClickElementByNameAsync(string name, int index)"));
            data.Add(new CompletionData("ClickElementByTagAsync", "ClickElementByTagAsync(string tag, int index)"));
            data.Add(new CompletionData("ClickMouseAsync", "ClickMouseAsync()"));
            data.Add(new CompletionData("COMPLETED", "COMPLETED"));
            data.Add(new CompletionData("ConsoleMsg", "ConsoleMsg(string message)"));
            data.Add(new CompletionData("ConsoleMsgError", "ConsoleMsgError(string message)"));
            data.Add(new CompletionData("CreateHashMD5FromTextAsync", "CreateHashMD5FromTextAsync(string text)"));
            data.Add(new CompletionData("DEFAULT", "DEFAULT"));
            data.Add(new CompletionData("DefineTestStop", "DefineTestStop()"));
            data.Add(new CompletionData("Description", "Description(string text)"));
            data.Add(new CompletionData("DisableDebugInReport", "DisableDebugInReport()"));
            data.Add(new CompletionData("ExecuteJavaScriptAsync", "ExecuteJavaScriptAsync(string script)"));
            data.Add(new CompletionData("FAILED", "FAILED"));
            data.Add(new CompletionData("FindElementAsync", "FindElementAsync(string by, string locator, int sec)"));
            data.Add(new CompletionData("FindElementByClassAsync", "FindElementByClassAsync(string _class, int index, int sec)"));
            data.Add(new CompletionData("FindElementByIdAsync", "FindElementByIdAsync(string id, int sec)"));
            data.Add(new CompletionData("FindElementByNameAsync", "FindElementByNameAsync(string name, int index, int sec)"));
            data.Add(new CompletionData("FindElementByTagAsync", "FindElementByTagAsync(string tag, int index, int sec)"));
            data.Add(new CompletionData("FindVisibleElementAsync", "FindVisibleElementAsync(string by, string locator, int sec)"));
            data.Add(new CompletionData("FindVisibleElementByClassAsync", "FindVisibleElementByClassAsync(string _class, int index, int sec)"));
            data.Add(new CompletionData("FindVisibleElementByIdAsync", "FindVisibleElementByIdAsync(string id, int sec)"));
            data.Add(new CompletionData("FindVisibleElementByNameAsync", "FindVisibleElementByNameAsync(string name, int index, int sec)"));
            data.Add(new CompletionData("FindVisibleElementByTagAsync", "FindVisibleElementByTagAsync(string tag, int index, int sec)"));
            data.Add(new CompletionData("FileDownloadAsync", "FileDownloadAsync(string fileURL, string filename, int waitingSec = 60)"));
            data.Add(new CompletionData("FileGetHashMD5Async", "FileGetHashMD5Async(string filename)"));
            data.Add(new CompletionData("FileReadAsync", "FileReadAsync(string encoding, string filename)"));
            data.Add(new CompletionData("FileWriteAsync", "FileWriteAsync(string content, string encoding, string filename)"));
            data.Add(new CompletionData("GetAttributeAsync", "GetAttributeAsync(string name)"));
            data.Add(new CompletionData("GetAttributeFromElementAsync", "GetAttributeFromElementAsync(string by, string locator, string attribute)"));
            data.Add(new CompletionData("GetAttributeFromElementByClassAsync", "GetAttributeFromElementByClassAsync(string _class, int index, string attribute)"));
            data.Add(new CompletionData("GetAttributeFromElementByIdAsync", "GetAttributeFromElementByIdAsync(string id, string attribute)"));
            data.Add(new CompletionData("GetAttributeFromElementByNameAsync", "GetAttributeFromElementByNameAsync(string name, int index, string attribute)"));
            data.Add(new CompletionData("GetAttributeFromElementByTagAsync", "GetAttributeFromElementByTagAsync(string tag, int index, string attribute)"));
            data.Add(new CompletionData("GetAttributeFromElementsAsync", "GetAttributeFromElementsAsync(string by, string locator, string attribute)"));
            data.Add(new CompletionData("GetAttributeFromElementsByClassAsync", "GetAttributeFromElementsByClassAsync(string _class, string attribute)"));
            data.Add(new CompletionData("GetAttributeFromElementsByNameAsync", "GetAttributeFromElementsByNameAsync(string name, string attribute)"));
            data.Add(new CompletionData("GetAttributeFromElementsByTagAsync", "GetAttributeFromElementsByTagAsync(string tag, string attribute)"));
            data.Add(new CompletionData("GetCountElementsAsync", "GetCountElementsAsync(string by, string locator)"));
            data.Add(new CompletionData("GetCountElementsByClassAsync", "GetCountElementsByClassAsync(string _class)"));
            data.Add(new CompletionData("GetCountElementsByNameAsync", "GetCountElementsByNameAsync(string name)"));
            data.Add(new CompletionData("GetCountElementsByTagAsync", "GetCountElementsByTagAsync(string tag)"));
            data.Add(new CompletionData("GetElementAsync", "GetElementAsync(string by, string locator)"));
            data.Add(new CompletionData("GetFrameAsync", "GetFrameAsync(int index)"));
            data.Add(new CompletionData("GetHtmlAsync", "GetHtmlAsync()"));
            data.Add(new CompletionData("GetHtmlFromElementAsync", "GetHtmlFromElementAsync(string by, string locator)"));
            data.Add(new CompletionData("GetHtmlFromElementByClassAsync", "GetHtmlFromElementByClassAsync(string _class, int index)"));
            data.Add(new CompletionData("GetHtmlFromElementByIdAsync", "GetHtmlFromElementByIdAsync(string id)"));
            data.Add(new CompletionData("GetHtmlFromElementByNameAsync", "GetHtmlFromElementByNameAsync(string name, int index)"));
            data.Add(new CompletionData("GetHtmlFromElementByTagAsync", "GetHtmlFromElementByTagAsync(string tag, int index)"));
            data.Add(new CompletionData("GetListRedirectUrlAsync", "GetListRedirectUrlAsync()"));
            data.Add(new CompletionData("GetLocatorAsync", "GetLocatorAsync()"));
            data.Add(new CompletionData("GetOptionAsync", "GetOptionAsync(string by)"));
            data.Add(new CompletionData("GetStyleAsync", "GetStyleAsync(string property)"));
            data.Add(new CompletionData("GetStyleFromElementAsync", "GetStyleFromElementAsync(string by, string locator, string property)"));
            data.Add(new CompletionData("GetStyleFromElementByClassAsync", "GetStyleFromElementByClassAsync(string _class, int index, string property)"));
            data.Add(new CompletionData("GetStyleFromElementByIdAsync", "GetStyleFromElementByIdAsync(string id, string property)"));
            data.Add(new CompletionData("GetStyleFromElementByNameAsync", "GetStyleFromElementByNameAsync(string name, int index, string property)"));
            data.Add(new CompletionData("GetStyleFromElementByTagAsync", "GetStyleFromElementByTagAsync(string tag, int index, string property)"));
            data.Add(new CompletionData("GetTestResult", "GetTestResult()"));
            data.Add(new CompletionData("GetTextAsync", "GetTextAsync()"));
            data.Add(new CompletionData("GetTextFromElementAsync", "GetTextFromElementAsync(string by, string locator)"));
            data.Add(new CompletionData("GetTextFromElementByClassAsync", "GetTextFromElementByClassAsync(string _class, int index)"));
            data.Add(new CompletionData("GetTextFromElementByIdAsync", "GetTextFromElementByIdAsync(string id)"));
            data.Add(new CompletionData("GetTextFromElementByNameAsync", "GetTextFromElementByNameAsync(string name, int index)"));
            data.Add(new CompletionData("GetTextFromElementByTagAsync", "GetTextFromElementByTagAsync(string tag, int index)"));
            data.Add(new CompletionData("GetTitleAsync", "GetTitleAsync()"));
            data.Add(new CompletionData("GetUrlAsync", "GetUrlAsync()"));
            data.Add(new CompletionData("GetUrlResponseAsync", "GetUrlResponseAsync(string url)"));
            data.Add(new CompletionData("GetValueAsync", "GetValueAsync()"));
            data.Add(new CompletionData("GetValueFromElementAsync", "GetValueFromElementAsync(string by, string locator)"));
            data.Add(new CompletionData("GetValueFromElementByClassAsync", "GetValueFromElementByClassAsync(string _class, int index)"));
            data.Add(new CompletionData("GetValueFromElementByIdAsync", "GetValueFromElementByIdAsync(string id)"));
            data.Add(new CompletionData("GetValueFromElementByNameAsync", "GetValueFromElementByNameAsync(string name, int index)"));
            data.Add(new CompletionData("GetValueFromElementByTagAsync", "GetValueFromElementByTagAsync(string tag, int index)"));
            data.Add(new CompletionData("GoToUrlAsync", "GoToUrlAsync(string url, int sec, bool abortLoadAfterTime = false)"));
            data.Add(new CompletionData("GoToUrlBaseAuthAsync", "GoToUrlBaseAuthAsync(string url, string login, string pass, int sec, bool abortLoadAfterTime = false)"));
            data.Add(new CompletionData("Id", "string Id { get; set; }"));
            data.Add(new CompletionData("IMAGE_STATUS_FAILED", "IMAGE_STATUS_FAILED"));
            data.Add(new CompletionData("IMAGE_STATUS_MESSAGE", "IMAGE_STATUS_MESSAGE"));
            data.Add(new CompletionData("IMAGE_STATUS_PASSED", "IMAGE_STATUS_PASSED"));
            data.Add(new CompletionData("IMAGE_STATUS_PROCESS", "IMAGE_STATUS_PROCESS"));
            data.Add(new CompletionData("IMAGE_STATUS_WARNING", "IMAGE_STATUS_WARNING"));
            data.Add(new CompletionData("Index", "int Index { get; set; }"));
            data.Add(new CompletionData("IsClickableAsync", "IsClickableAsync()"));
            data.Add(new CompletionData("IsClickableElementAsync", "IsClickableElementAsync(string by, string locator)"));
            data.Add(new CompletionData("IsVisibleElementAsync", "IsVisibleElementAsync(string by, string locator)"));
            data.Add(new CompletionData("Name", "string Name { get; set; }"));
            data.Add(new CompletionData("PASSED", "PASSED"));
            data.Add(new CompletionData("PROCESS", "PROCESS"));
            data.Add(new CompletionData("RestGetAsync", "RestGetAsync(string url, TimeSpan timeout, string charset = \"UTF-8\")"));
            data.Add(new CompletionData("RestGetBasicAuthAsync", "RestGetBasicAuthAsync(string login, string pass, string url, TimeSpan timeout, string charset = \"UTF-8\")"));
            data.Add(new CompletionData("RestGetStatusCodeAsync", "RestGetStatusCodeAsync(string url)"));
            data.Add(new CompletionData("RestPostAsync", "RestPostAsync(string url, string json, TimeSpan timeout, string charset = \"UTF-8\")"));
            data.Add(new CompletionData("ScrollToAsync", "ScrollToAsync(bool behaviorSmooth = false)"));
            data.Add(new CompletionData("ScrollToElementAsync", "ScrollToElementAsync(string by, string locator, bool behaviorSmooth = false)"));
            data.Add(new CompletionData("SelectOptionAsync", "SelectOptionAsync(string by, string value)"));
            data.Add(new CompletionData("SendMessage", "SendMessage(string action, string status, string comment)"));
            data.Add(new CompletionData("SendMessageDebug", "SendMessageDebug(string actionRus, string actionEng, string status,  string commentRus, string commentEng, int image)"));
            data.Add(new CompletionData("SendMsgToMailAsync", "SendMsgToMailAsync(string subject, string body, string filename = \"\", string addresses = \"\")"));
            data.Add(new CompletionData("SendMsgToTelegramAsync", "SendMsgToTelegramAsync(string botToken, string chatId, string text, string charset = \"UTF-8\")"));
            data.Add(new CompletionData("SetAttributeAsync", "GetAttributeAsync(string name)"));
            data.Add(new CompletionData("SetAttributeInElementAsync", "SetAttributeInElementAsync(string by, string locator, string attribute, string value)"));
            data.Add(new CompletionData("SetAttributeInElementByClassAsync", "SetAttributeInElementByClassAsync(string _class, int index, string attribute, string value)"));
            data.Add(new CompletionData("SetAttributeInElementByIdAsync", "SetAttributeInElementByIdAsync(string id, string attribute, string value)"));
            data.Add(new CompletionData("SetAttributeInElementByNameAsync", "SetAttributeInElementByNameAsync(string name, int index, string attribute, string value)"));
            data.Add(new CompletionData("SetAttributeInElementByTagAsync", "SetAttributeInElementByTagAsync(string tag, int index, string attribute, string value)"));
            data.Add(new CompletionData("SetAttributeInElementsAsync", "SetAttributeInElementsAsync(string by, string locator, string attribute, string value)"));
            data.Add(new CompletionData("SetAttributeInElementsByClassAsync", "SetAttributeInElementsByClassAsync(string _class, string attribute, string value)"));
            data.Add(new CompletionData("SetAttributeInElementsByNameAsync", "SetAttributeInElementsByNameAsync(string name, string attribute, string value)"));
            data.Add(new CompletionData("SetAttributeInElementsByTagAsync", "SetAttributeInElementsByTagAsync(string tag, string attribute, string value)"));
            data.Add(new CompletionData("SetHtmlAsync", "SetHtmlAsync(string html)"));
            data.Add(new CompletionData("SetHtmlInElementAsync", "SetHtmlInElementAsync(string by, string locator, string html)"));
            data.Add(new CompletionData("SetHtmlInElementByClassAsync", "SetHtmlInElementByClassAsync(string _class, int index, string html)"));
            data.Add(new CompletionData("SetHtmlInElementByIdAsync", "SetHtmlInElementByIdAsync(string id, string html)"));
            data.Add(new CompletionData("SetHtmlInElementByNameAsync", "SetHtmlInElementByNameAsync(string name, int index, string html)"));
            data.Add(new CompletionData("SetHtmlInElementByTagAsync", "SetHtmlInElementByTagAsync(string tag, int index, string html)"));
            data.Add(new CompletionData("SetStyleAsync", "SetStyleAsync(string cssText)"));
            data.Add(new CompletionData("SetStyleInElementAsync", "SetStyleInElementAsync(string by, string locator, string cssText)"));
            data.Add(new CompletionData("SetStyleInElementByClassAsync", "SetStyleInElementByClassAsync(string _class, int index, string cssText)"));
            data.Add(new CompletionData("SetStyleInElementByIdAsync", "SetStyleInElementByIdAsync(string id, string cssText)"));
            data.Add(new CompletionData("SetStyleInElementByNameAsync", "SetStyleInElementByNameAsync(string name, int index, string cssText)"));
            data.Add(new CompletionData("SetStyleInElementByTagAsync", "SetStyleInElementByTagAsync(string tag, int index, string cssText)"));
            data.Add(new CompletionData("SetTextAsync", "SetTextAsync(string text)"));
            data.Add(new CompletionData("SetTextInElementAsync", "SetTextInElementAsync(string by, string locator, string text)"));
            data.Add(new CompletionData("SetTextInElementByClassAsync", "SetTextInElementByClassAsync(string _class, int index, string text)"));
            data.Add(new CompletionData("SetTextInElementByIdAsync", "SetTextInElementByIdAsync(string id, string text)"));
            data.Add(new CompletionData("SetTextInElementByNameAsync", "SetTextInElementByNameAsync(string name, int index, string text)"));
            data.Add(new CompletionData("SetTextInElementByTagAsync", "SetTextInElementByTagAsync(string tag, int index, string text)"));
            data.Add(new CompletionData("SetValueAsync", "SetValueAsync(string value)"));
            data.Add(new CompletionData("SetValueInElementAsync", "SetValueInElementAsync(string by, string locator, string value)"));
            data.Add(new CompletionData("SetValueInElementByClassAsync", "SetValueInElementByClassAsync(string _class, int index, string value)"));
            data.Add(new CompletionData("SetValueInElementByIdAsync", "SetValueInElementByIdAsync(string id, string value)"));
            data.Add(new CompletionData("SetValueInElementByNameAsync", "SetValueInElementByNameAsync(string name, int index, string value)"));
            data.Add(new CompletionData("SetValueInElementByTagAsync", "SetValueInElementByTagAsync(string tag, int index, string value)"));
            data.Add(new CompletionData("STOPPED", "STOPPED"));
            data.Add(new CompletionData("TestBeginAsync", "TestBeginAsync()"));
            data.Add(new CompletionData("TestEndAsync", "TestEndAsync()"));
            data.Add(new CompletionData("TestStopAsync", "TestStopAsync()"));
            data.Add(new CompletionData("TimerStart", "TimerStart()"));
            data.Add(new CompletionData("TimerStop", "TimerStop(DateTime start)"));
            data.Add(new CompletionData("Type", "string Type { get; set; }"));
            data.Add(new CompletionData("UTF8", "UTF-8"));
            data.Add(new CompletionData("UTF8BOM", "UTF-8 BOM"));
            data.Add(new CompletionData("WaitAsync", "WaitAsync(int sec)"));
            data.Add(new CompletionData("WaitElementInDomAsync", "WaitElementInDomAsync(string by, string locator, int sec)"));
            data.Add(new CompletionData("WaitElementNotDomAsync", "WaitElementNotDomAsync(string by, string locator, int sec)"));
            data.Add(new CompletionData("WaitNotVisibleAsync", "WaitNotVisibleAsync(int sec)"));
            data.Add(new CompletionData("WaitNotVisibleElementAsync", "WaitNotVisibleElementAsync(string by, string locator, int sec)"));
            data.Add(new CompletionData("WaitNotVisibleElementByClassAsync", "WaitNotVisibleElementByClassAsync(string _class, int index, int sec)"));
            data.Add(new CompletionData("WaitNotVisibleElementByIdAsync", "WaitNotVisibleElementByIdAsync(string id, int sec)"));
            data.Add(new CompletionData("WaitNotVisibleElementByNameAsync", "WaitNotVisibleElementByNameAsync(string name, int index, int sec)"));
            data.Add(new CompletionData("WaitNotVisibleElementByTagAsync", "WaitNotVisibleElementByTagAsync(string tag, int index, int sec)"));
            data.Add(new CompletionData("WaitVisibleAsync", "WaitVisibleAsync(int sec)"));
            data.Add(new CompletionData("WaitVisibleElementAsync", "WaitVisibleElementAsync(string by, string locator, int sec)"));
            data.Add(new CompletionData("WaitVisibleElementByClassAsync", "WaitVisibleElementByClassAsync(string _class, int index, int sec)"));
            data.Add(new CompletionData("WaitVisibleElementByIdAsync", "WaitVisibleElementByIdAsync(string id, int sec)"));
            data.Add(new CompletionData("WaitVisibleElementByNameAsync", "WaitVisibleElementByNameAsync(string name, int index, int sec)"));
            data.Add(new CompletionData("WaitVisibleElementByTagAsync", "WaitVisibleElementByTagAsync(string tag, int index, int sec)"));
            data.Add(new CompletionData("WARNING", "WARNING"));
            data.Add(new CompletionData("WINDOWS1251", "WINDOWS-1251"));
            completionWindow.Width = 250;
            completionWindow.Show();
            completionWindow.Closed += delegate {
                completionWindow = null;
            };
        }

        private void textEditor_TextArea_TextEntered(object sender, TextCompositionEventArgs e)
        {
            if (e.Text == ".")
            {
                showCompletionWindow((TextArea)sender);
            }
        }

        void textEditor_TextArea_TextEntering(object sender, TextCompositionEventArgs e)
        {
            if (e.Text.Length > 0 && completionWindow != null)
            {
                if (!char.IsLetterOrDigit(e.Text[0]))
                {
                    completionWindow.CompletionList.RequestInsertion(e);
                }
            }
        }

        private void closeFile()
        {
            try
            {
                
                int index = tabControl1.SelectedIndex;
                int count = files.Count;
                if (index < 0 && count <= 0) return;

                string filename = "";
                if (Convert.ToInt32(files[index][3]) == index)
                {
                    filename = files[index][0].ToString();
                   
                    if (files[index][2].ToString() == STATUS_NOT_SAVE)
                    {
                        DialogResult dialogResult = MessageBox.Show($"Закрывается файл {filename} {Environment.NewLine}Вы хотите сохранить изменения в файле?", "Вопрос", MessageBoxButtons.YesNoCancel);

                        if (dialogResult == DialogResult.Cancel)
                        {
                            //parent.ConsoleMsg("Отмена закрытия файла");
                        }
                        else if (dialogResult == DialogResult.No)
                        {
                            tabControl1.TabPages.Remove(tabControl1.SelectedTab);
                            files.RemoveAt(index);
                            parent.ConsoleMsg($"Файл {filename} - закрыт без сохранения");
                            updateListFiles();
                        }
                        else if (dialogResult == DialogResult.Yes)
                        {
                            saveFile();
                            tabControl1.TabPages.Remove(tabControl1.SelectedTab);
                            files.RemoveAt(index);
                            updateListFiles();
                            parent.ConsoleMsg($"Файл {filename} - закрыт");
                        }
                    }
                    else
                    {
                        tabControl1.TabPages.Remove(tabControl1.SelectedTab);
                        files.RemoveAt(index);
                        parent.ConsoleMsg($"Файл {filename} - закрыт");
                        updateListFiles();
                    }
                }
                else
                {
                    parent.ConsoleMsg($"Файл {filename} - неудалось закрыть");
                }
            }
            catch (Exception ex)
            {
                parent.ConsoleMsgError(ex.ToString());
            }
        }

        private void updateListFiles()
        {
            try
            {
                int count = tabControl1.TabPages.Count;
                for (int i = 0; i < count; i++)
                {
                    files[i][3] = i.ToString();
                    (files[i][5] as TextEditor).Tag = i.ToString();
                    //parent.ConsoleMsg($"{files[i][0]} | {files[i][1]} | {files[i][2]} | {files[i][3]} | {files[i][4]} | {(files[i][5] as TextEditorControl).Tag} | ");
                }
            }
            catch (Exception ex)
            {
                parent.ConsoleMsgError(ex.ToString());
            }
        }

        private void fileCloseToolStripMenuItem_Click(object sender, EventArgs e)
        {
            closeFile();
        }

        private void closeFileToolStripMenuItem_Click(object sender, EventArgs e)
        {
            closeFile();
        }

        private void tabControl1_Selected(object sender, TabControlEventArgs e)
        {
            try
            {
                int index = tabControl1.SelectedIndex;
                if (index < 0) return;
                toolStripStatusLabel5.Text = files[index][1].ToString();
            }
            catch (Exception ex)
            {
                parent.ConsoleMsgError(ex.ToString());
            }
            
        }

        private void saveFile()
        {
            try
            {
                int index = tabControl1.SelectedIndex;
                int count = files.Count;
                if (index < 0 && count <= 0) return;

                string filename = files[index][0].ToString();
                string path = files[index][1].ToString();
                string content = (files[index][5] as TextEditor).Text;

                WorkOnFiles write = new WorkOnFiles();
                write.writeFile(content, toolStripStatusLabel2.Text, path);

                (files[index][4] as TabPage).Text = filename;
                files[index][2] = STATUS_SAVED;

                parent.ConsoleMsg($"Файл {filename} - сохранён");
            }
            catch (Exception ex)
            {
                parent.ConsoleMsgError(ex.ToString());
            }
        }

        private void saveFileAs()
        {
            try
            {
                int index = tabControl1.SelectedIndex;
                int count = files.Count;
                if (index < 0 && count <= 0) return;

                if (saveFileDialog1.ShowDialog() == DialogResult.OK)
                {
                    string filename = Config.browserForm.getFileName(saveFileDialog1.FileName);
                    string path = saveFileDialog1.FileName;

                    WorkOnFiles write = new WorkOnFiles();
                    write.writeFile((files[index][5] as TextEditor).Text, toolStripStatusLabel2.Text, path);

                    files[index][0] = filename;
                    files[index][1] = path;
                    (files[index][4] as TabPage).Text = filename;
                    files[index][2] = STATUS_SAVED;

                    toolStripStatusLabel5.Text = path;
                    Config.browserForm.ConsoleMsg($"Файл {filename} - сохранён");
                    Config.browserForm.projectUpdate();
                }
            }
            catch (Exception ex)
            {
                parent.ConsoleMsgError(ex.ToString());
            }
        }

        private void saveFileAll()
        {
            try
            {
                int count = files.Count;
                if (count <= 0) return;

                for (int i = 0; i < count; i++)
                {
                    // [ 0 - имя файла | 1 - путь файла | 2 - статус | 3 - индекс | 4 - TabPage (вкладка) | 5 - TextEditorControl (редактор)]
                    if (files[i][2].ToString() == STATUS_NOT_SAVE)
                    {
                        string filename = files[i][0].ToString();
                        string path = files[i][1].ToString();
                        string content = (files[i][5] as TextEditor).Text;

                        WorkOnFiles write = new WorkOnFiles();
                        write.writeFile(content, toolStripStatusLabel2.Text, path);

                        (files[i][4] as TabPage).Text = filename;
                        files[i][2] = STATUS_SAVED;

                        parent.ConsoleMsg($"Файл {filename} - сохранён");
                    }
                }


            }
            catch (Exception ex)
            {
                parent.ConsoleMsgError(ex.ToString());
            }
        }

        private void fileSaveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            saveFile();
        }

        private void fileSaveAsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            saveFileAs();
        }

        private void toolStripButton1_Click(object sender, EventArgs e)
        {
            saveFile();
        }

        private void toolStripButton2_Click(object sender, EventArgs e)
        {
            saveFileAs();
        }

        private void toolStripButton6_Click(object sender, EventArgs e)
        {
            saveFileAll();
        }

        private void filesSaveAllToolStripMenuItem_Click(object sender, EventArgs e)
        {
            saveFileAll();
        }

        private void createCmd()
        {
            try
            {
                int index = tabControl1.SelectedIndex;
                int count = files.Count;
                if (index < 0 && count <= 0) return;

                if (Config.projectPath != "(не открыт)")
                {
                    CreateCmdForm createCmdForm = new CreateCmdForm();
                    createCmdForm.textBox.Text = $"cd {Directory.GetCurrentDirectory()}" + Environment.NewLine;
                    createCmdForm.textBox.Text += $"Hat.exe {files[index][0]} {Config.projectPath}";
                    createCmdForm.ShowDialog();
                }
                else
                {
                    Config.browserForm.ConsoleMsg("Проект не открыт");
                }
            }
            catch (Exception ex)
            {
                Config.browserForm.ConsoleMsgError(ex.ToString());
            }
        }

        private void toolStripButton5_Click(object sender, EventArgs e)
        {
            createCmd();
        }

        private void commandToolStripMenuItem_Click(object sender, EventArgs e)
        {
            createCmd();
        }

        private void testPlay()
        {
            try
            {
                int index = tabControl1.SelectedIndex;
                int count = files.Count;
                if (index < 0 && count <= 0) return;

               
                Config.selectName = files[index][0].ToString(); // имя файла или папки
                Config.selectValue = files[index][1].ToString(); // путь к файлу или к папке
                Config.browserForm.toolStripStatusLabelProjectFolderFile.Text = Config.selectName;
                Config.browserForm.PlayTest(Config.selectName);
            }
            catch (Exception ex)
            {
                Config.browserForm.ConsoleMsgError(ex.ToString());
            }
        }

        private void testStop()
        {
            Config.browserForm.StopTest();
        }

        private void toolStripButton3_Click(object sender, EventArgs e)
        {
            testPlay();
        }

        private void toolStripButton4_Click(object sender, EventArgs e)
        {
            testStop();
        }

        private void testPlayToolStripMenuItem_Click(object sender, EventArgs e)
        {
            testPlay();
        }

        private void testStopToolStripMenuItem_Click(object sender, EventArgs e)
        {
            testStop();
        }

        private void treeView1_AfterSelect(object sender, TreeViewEventArgs e)
        {
            try
            {
                richTextBox1.Text = "";
                richTextBox1.Rtf = "";
                if (treeView1.SelectedNode != null)
                {
                    string value = treeView1.SelectedNode.Text;
                    string tag = treeView1.SelectedNode.Tag.ToString();
                    if (value == "Tester" && tag == "Tester") richTextBox1.Rtf = handbook[0];
                    if (value == "IMAGE_STATUS_PROCESS" && tag == "Tester") richTextBox1.Rtf = handbook[1];
                    if (value == "IMAGE_STATUS_PASSED" && tag == "Tester") richTextBox1.Rtf = handbook[2];
                    if (value == "IMAGE_STATUS_FAILED" && tag == "Tester") richTextBox1.Rtf = handbook[3];
                    if (value == "IMAGE_STATUS_MESSAGE" && tag == "Tester") richTextBox1.Rtf = handbook[4];
                    if (value == "IMAGE_STATUS_WARNING" && tag == "Tester") richTextBox1.Rtf = handbook[5];
                    if (value == "PASSED" && tag == "Tester") richTextBox1.Rtf = handbook[6];
                    if (value == "FAILED" && tag == "Tester") richTextBox1.Rtf = handbook[7];
                    if (value == "STOPPED" && tag == "Tester") richTextBox1.Rtf = handbook[8];
                    if (value == "PROCESS" && tag == "Tester") richTextBox1.Rtf = handbook[9];
                    if (value == "COMPLETED" && tag == "Tester") richTextBox1.Rtf = handbook[10];
                    if (value == "WARNING" && tag == "Tester") richTextBox1.Rtf = handbook[11];
                    if (value == "BrowserView" && tag == "Tester") richTextBox1.Rtf = handbook[12];
                    if (value == "BrowserWindow" && tag == "Tester") richTextBox1.Rtf = handbook[13];
                    if (value == "BrowserCloseAsync" && tag == "Tester") richTextBox1.Rtf = handbook[14];
                    if (value == "BrowserSizeAsync" && tag == "Tester") richTextBox1.Rtf = handbook[15];
                    if (value == "BrowserFullScreenAsync" && tag == "Tester") richTextBox1.Rtf = handbook[16];
                    if (value == "BrowserSetUserAgentAsync" && tag == "Tester") richTextBox1.Rtf = handbook[17];
                    if (value == "BrowserGetUserAgentAsync" && tag == "Tester") richTextBox1.Rtf = handbook[18];
                    if (value == "ConsoleMsg" && tag == "Tester") richTextBox1.Rtf = handbook[19];
                    if (value == "ConsoleMsgError" && tag == "Tester") richTextBox1.Rtf = handbook[20];
                    if (value == "ClearMessage" && tag == "Tester") richTextBox1.Rtf = handbook[21];
                    if (value == "SendMessage" && tag == "Tester") richTextBox1.Rtf = handbook[22];
                    if (value == "EditMessage" && tag == "Tester") richTextBox1.Rtf = handbook[23];
                    if (value == "TestBeginAsync" && tag == "Tester") richTextBox1.Rtf = handbook[24];
                    if (value == "TestEndAsync" && tag == "Tester") richTextBox1.Rtf = handbook[25];
                    if (value == "TestStopAsync" && tag == "Tester") richTextBox1.Rtf = handbook[26];
                    if (value == "DefineTestStop" && tag == "Tester") richTextBox1.Rtf = handbook[27];
                    if (value == "ClickElementByClassAsync" && tag == "Tester") richTextBox1.Rtf = handbook[28];
                    if (value == "ClickElementAsync" && tag == "Tester") richTextBox1.Rtf = handbook[29];
                    if (value == "ClickElementByIdAsync" && tag == "Tester") richTextBox1.Rtf = handbook[30];
                    if (value == "ClickElementByNameAsync" && tag == "Tester") richTextBox1.Rtf = handbook[31];
                    if (value == "ClickElementByTagAsync" && tag == "Tester") richTextBox1.Rtf = handbook[32];
                    if (value == "FindElementByClassAsync" && tag == "Tester") richTextBox1.Rtf = handbook[33];
                    if (value == "FindElementAsync" && tag == "Tester") richTextBox1.Rtf = handbook[34];
                    if (value == "FindElementByIdAsync" && tag == "Tester") richTextBox1.Rtf = handbook[35];
                    if (value == "FindElementByNameAsync" && tag == "Tester") richTextBox1.Rtf = handbook[36];
                    if (value == "FindElementByTagAsync" && tag == "Tester") richTextBox1.Rtf = handbook[37];
                    if (value == "FindVisibleElementByClassAsync" && tag == "Tester") richTextBox1.Rtf = handbook[38];
                    if (value == "FindVisibleElementAsync" && tag == "Tester") richTextBox1.Rtf = handbook[39];
                    if (value == "FindVisibleElementByIdAsync" && tag == "Tester") richTextBox1.Rtf = handbook[40];
                    if (value == "FindVisibleElementByNameAsync" && tag == "Tester") richTextBox1.Rtf = handbook[41];
                    if (value == "FindVisibleElementByTagAsync" && tag == "Tester") richTextBox1.Rtf = handbook[42];
                    if (value == "GetAttributeFromElementByClassAsync" && tag == "Tester") richTextBox1.Rtf = handbook[43];
                    if (value == "GetAttributeFromElementAsync" && tag == "Tester") richTextBox1.Rtf = handbook[44];
                    if (value == "GetAttributeFromElementByIdAsync" && tag == "Tester") richTextBox1.Rtf = handbook[45];
                    if (value == "GetAttributeFromElementByNameAsync" && tag == "Tester") richTextBox1.Rtf = handbook[46];
                    if (value == "GetAttributeFromElementByTagAsync" && tag == "Tester") richTextBox1.Rtf = handbook[47];
                    if (value == "GetAttributeFromElementsAsync" && tag == "Tester") richTextBox1.Rtf = handbook[48];
                    if (value == "GetCountElementsByClassAsync" && tag == "Tester") richTextBox1.Rtf = handbook[49];
                    if (value == "GetCountElementsAsync" && tag == "Tester") richTextBox1.Rtf = handbook[50];
                    if (value == "GetCountElementsByNameAsync" && tag == "Tester") richTextBox1.Rtf = handbook[51];
                    if (value == "GetCountElementsByTagAsync" && tag == "Tester") richTextBox1.Rtf = handbook[52];
                    if (value == "GetElementAsync" && tag == "Tester") richTextBox1.Rtf = handbook[53];
                    if (value == "GetTextFromElementByClassAsync" && tag == "Tester") richTextBox1.Rtf = handbook[54];
                    if (value == "GetTextFromElementAsync" && tag == "Tester") richTextBox1.Rtf = handbook[55];
                    if (value == "GetTextFromElementByIdAsync" && tag == "Tester") richTextBox1.Rtf = handbook[56];
                    if (value == "GetTextFromElementByNameAsync" && tag == "Tester") richTextBox1.Rtf = handbook[57];
                    if (value == "GetTextFromElementByTagAsync" && tag == "Tester") richTextBox1.Rtf = handbook[58];
                    if (value == "GetTitleAsync" && tag == "Tester") richTextBox1.Rtf = handbook[59];
                    if (value == "GetUrlAsync" && tag == "Tester") richTextBox1.Rtf = handbook[60];
                    if (value == "GetValueFromElementByClassAsync" && tag == "Tester") richTextBox1.Rtf = handbook[61];
                    if (value == "GetValueFromElementAsync" && tag == "Tester") richTextBox1.Rtf = handbook[62];
                    if (value == "GetValueFromElementByIdAsync" && tag == "Tester") richTextBox1.Rtf = handbook[63];
                    if (value == "GetValueFromElementByNameAsync" && tag == "Tester") richTextBox1.Rtf = handbook[64];
                    if (value == "GetValueFromElementByTagAsync" && tag == "Tester") richTextBox1.Rtf = handbook[65];
                    if (value == "GoToUrlAsync" && tag == "Tester") richTextBox1.Rtf = handbook[66];
                    if (value == "ScrollToElementAsync" && tag == "Tester") richTextBox1.Rtf = handbook[67];
                    if (value == "SetAttributeInElementByClassAsync" && tag == "Tester") richTextBox1.Rtf = handbook[68];
                    if (value == "SetAttributeInElementAsync" && tag == "Tester") richTextBox1.Rtf = handbook[69];
                    if (value == "SetAttributeInElementByIdAsync" && tag == "Tester") richTextBox1.Rtf = handbook[70];
                    if (value == "SetAttributeInElementByNameAsync" && tag == "Tester") richTextBox1.Rtf = handbook[71];
                    if (value == "SetAttributeInElementByTagAsync" && tag == "Tester") richTextBox1.Rtf = handbook[72];
                    if (value == "SetTextInElementByClassAsync" && tag == "Tester") richTextBox1.Rtf = handbook[73];
                    if (value == "SetTextInElementAsync" && tag == "Tester") richTextBox1.Rtf = handbook[74];
                    if (value == "SetTextInElementByIdAsync" && tag == "Tester") richTextBox1.Rtf = handbook[75];
                    if (value == "SetTextInElementByNameAsync" && tag == "Tester") richTextBox1.Rtf = handbook[76];
                    if (value == "SetTextInElementByTagAsync" && tag == "Tester") richTextBox1.Rtf = handbook[77];
                    if (value == "SetValueInElementByClassAsync" && tag == "Tester") richTextBox1.Rtf = handbook[78];
                    if (value == "SetValueInElementAsync" && tag == "Tester") richTextBox1.Rtf = handbook[79];
                    if (value == "SetValueInElementByIdAsync" && tag == "Tester") richTextBox1.Rtf = handbook[80];
                    if (value == "SetValueInElementByNameAsync" && tag == "Tester") richTextBox1.Rtf = handbook[81];
                    if (value == "SetValueInElementByTagAsync" && tag == "Tester") richTextBox1.Rtf = handbook[82];
                    if (value == "WaitAsync" && tag == "Tester") richTextBox1.Rtf = handbook[83];
                    if (value == "WaitNotVisibleElementByClassAsync" && tag == "Tester") richTextBox1.Rtf = handbook[84];
                    if (value == "WaitNotVisibleElementAsync" && tag == "Tester") richTextBox1.Rtf = handbook[85];
                    if (value == "WaitNotVisibleElementByIdAsync" && tag == "Tester") richTextBox1.Rtf = handbook[86];
                    if (value == "WaitNotVisibleElementByNameAsync" && tag == "Tester") richTextBox1.Rtf = handbook[87];
                    if (value == "WaitNotVisibleElementByTagAsync" && tag == "Tester") richTextBox1.Rtf = handbook[88];
                    if (value == "WaitVisibleElementByClassAsync" && tag == "Tester") richTextBox1.Rtf = handbook[89];
                    if (value == "WaitVisibleElementAsync" && tag == "Tester") richTextBox1.Rtf = handbook[90];
                    if (value == "WaitVisibleElementByIdAsync" && tag == "Tester") richTextBox1.Rtf = handbook[91];
                    if (value == "WaitVisibleElementByNameAsync" && tag == "Tester") richTextBox1.Rtf = handbook[92];
                    if (value == "WaitVisibleElementByTagAsync" && tag == "Tester") richTextBox1.Rtf = handbook[93];
                    if (value == "ExecuteJavaScriptAsync" && tag == "Tester") richTextBox1.Rtf = handbook[94];
                    if (value == "AssertEqualsAsync" && tag == "Tester") richTextBox1.Rtf = handbook[95];
                    if (value == "AssertNotEqualsAsync" && tag == "Tester") richTextBox1.Rtf = handbook[96];
                    if (value == "AssertTrueAsync" && tag == "Tester") richTextBox1.Rtf = handbook[97];
                    if (value == "AssertFalseAsync" && tag == "Tester") richTextBox1.Rtf = handbook[98];
                    if (value == "BrowserGetErrorsAsync" && tag == "Tester") richTextBox1.Rtf = handbook[99];
                    if (value == "BrowserGetNetworkAsync" && tag == "Tester") richTextBox1.Rtf = handbook[100];
                    if (value == "GetAttributeFromElementsByClassAsync" && tag == "Tester") richTextBox1.Rtf = handbook[101];
                    if (value == "GetAttributeFromElementsByNameAsync" && tag == "Tester") richTextBox1.Rtf = handbook[102];
                    if (value == "GetAttributeFromElementsByTagAsync" && tag == "Tester") richTextBox1.Rtf = handbook[103];
                    if (value == "SetAttributeInElementsByClassAsync" && tag == "Tester") richTextBox1.Rtf = handbook[104];
                    if (value == "SetAttributeInElementsAsync" && tag == "Tester") richTextBox1.Rtf = handbook[105];
                    if (value == "SetAttributeInElementsByNameAsync" && tag == "Tester") richTextBox1.Rtf = handbook[106];
                    if (value == "SetAttributeInElementsByTagAsync" && tag == "Tester") richTextBox1.Rtf = handbook[107];
                    if (value == "GetHtmlFromElementByClassAsync" && tag == "Tester") richTextBox1.Rtf = handbook[108];
                    if (value == "GetHtmlFromElementAsync" && tag == "Tester") richTextBox1.Rtf = handbook[109];
                    if (value == "GetHtmlFromElementByIdAsync" && tag == "Tester") richTextBox1.Rtf = handbook[110];
                    if (value == "GetHtmlFromElementByNameAsync" && tag == "Tester") richTextBox1.Rtf = handbook[111];
                    if (value == "GetHtmlFromElementByTagAsync" && tag == "Tester") richTextBox1.Rtf = handbook[112];
                    if (value == "SetHtmlInElementByClassAsync" && tag == "Tester") richTextBox1.Rtf = handbook[113];
                    if (value == "SetHtmlInElementAsync" && tag == "Tester") richTextBox1.Rtf = handbook[114];
                    if (value == "SetHtmlInElementByIdAsync" && tag == "Tester") richTextBox1.Rtf = handbook[115];
                    if (value == "SetHtmlInElementByNameAsync" && tag == "Tester") richTextBox1.Rtf = handbook[116];
                    if (value == "SetHtmlInElementByTagAsync" && tag == "Tester") richTextBox1.Rtf = handbook[117];
                    if (value == "HTMLElement" && tag == "HTMLElement") richTextBox1.Rtf = handbook[118];
                    if (value == "Id" && tag == "HTMLElement") richTextBox1.Rtf = handbook[119];
                    if (value == "Name" && tag == "HTMLElement") richTextBox1.Rtf = handbook[120];
                    if (value == "Class" && tag == "HTMLElement") richTextBox1.Rtf = handbook[121];
                    if (value == "Type" && tag == "HTMLElement") richTextBox1.Rtf = handbook[122];
                    if (value == "ClickAsync" && tag == "HTMLElement") richTextBox1.Rtf = handbook[123];
                    if (value == "GetAttributeAsync" && tag == "HTMLElement") richTextBox1.Rtf = handbook[124];
                    if (value == "GetHtmlAsync" && tag == "HTMLElement") richTextBox1.Rtf = handbook[125];
                    if (value == "GetTextAsync" && tag == "HTMLElement") richTextBox1.Rtf = handbook[126];
                    if (value == "GetValueAsync" && tag == "HTMLElement") richTextBox1.Rtf = handbook[127];
                    if (value == "ScrollToAsync" && tag == "HTMLElement") richTextBox1.Rtf = handbook[128];
                    if (value == "SetAttributeAsync" && tag == "HTMLElement") richTextBox1.Rtf = handbook[129];
                    if (value == "SetHtmlAsync" && tag == "HTMLElement") richTextBox1.Rtf = handbook[130];
                    if (value == "SetTextAsync" && tag == "HTMLElement") richTextBox1.Rtf = handbook[131];
                    if (value == "SetValueAsync" && tag == "HTMLElement") richTextBox1.Rtf = handbook[132];
                    if (value == "WaitNotVisibleAsync" && tag == "HTMLElement") richTextBox1.Rtf = handbook[133];
                    if (value == "WaitVisibleAsync" && tag == "HTMLElement") richTextBox1.Rtf = handbook[134];
                    if (value == "BY_CSS" && tag == "Tester") richTextBox1.Rtf = handbook[135];
                    if (value == "BY_XPATH" && tag == "Tester") richTextBox1.Rtf = handbook[136];
                    if (value == "RestGetAsync" && tag == "Tester") richTextBox1.Rtf = handbook[137];
                    if (value == "RestGetBasicAuthAsync" && tag == "Tester") richTextBox1.Rtf = handbook[138];
                    if (value == "BrowserGoBackAsync" && tag == "Tester") richTextBox1.Rtf = handbook[139];
                    if (value == "BrowserGoForwardAsync" && tag == "Tester") richTextBox1.Rtf = handbook[140];
                    if (value == "BrowserBasicAuthenticationAsync" && tag == "Tester") richTextBox1.Rtf = handbook[141];
                    if (value == "BrowserEnableSendMailAsync" && tag == "Tester") richTextBox1.Rtf = handbook[142];
                    if (value == "SelectOptionAsync" && tag == "HTMLElement") richTextBox1.Rtf = handbook[143];
                    if (value == "GetOptionAsync" && tag == "HTMLElement") richTextBox1.Rtf = handbook[144];
                    if (value == "BY_INDEX" && tag == "HTMLElement") richTextBox1.Rtf = handbook[145];
                    if (value == "BY_TEXT" && tag == "HTMLElement") richTextBox1.Rtf = handbook[146];
                    if (value == "BY_VALUE" && tag == "HTMLElement") richTextBox1.Rtf = handbook[147];
                    if (value == "IsClickableAsync" && tag == "HTMLElement") richTextBox1.Rtf = handbook[148];
                    if (value == "IsClickableElementAsync" && tag == "Tester") richTextBox1.Rtf = handbook[149];
                    if (value == "GetFrameAsync") richTextBox1.Rtf = handbook[150];
                    if (value == "FRAMEElement" && tag == "FRAMEElement") richTextBox1.Rtf = handbook[151];
                    if (value == "BY_INDEX" && tag == "FRAMEElement") richTextBox1.Rtf = handbook[152];
                    if (value == "BY_TEXT" && tag == "FRAMEElement") richTextBox1.Rtf = handbook[153];
                    if (value == "BY_VALUE" && tag == "FRAMEElement") richTextBox1.Rtf = handbook[154];
                    if (value == "Name" && tag == "FRAMEElement") richTextBox1.Rtf = handbook[155];
                    if (value == "Index" && tag == "FRAMEElement") richTextBox1.Rtf = handbook[156];
                    if (value == "ClickElementAsync" && tag == "FRAMEElement") richTextBox1.Rtf = handbook[157];
                    if (value == "FindElementAsync" && tag == "FRAMEElement") richTextBox1.Rtf = handbook[158];
                    if (value == "FindVisibleElementAsync" && tag == "FRAMEElement") richTextBox1.Rtf = handbook[159];
                    if (value == "GetAttributeFromElementAsync" && tag == "FRAMEElement") richTextBox1.Rtf = handbook[160];
                    if (value == "GetAttributeFromElementsAsync" && tag == "FRAMEElement") richTextBox1.Rtf = handbook[161];
                    if (value == "GetCountElementsAsync" && tag == "FRAMEElement") richTextBox1.Rtf = handbook[162];
                    if (value == "GetHtmlFromElementAsync" && tag == "FRAMEElement") richTextBox1.Rtf = handbook[163];
                    if (value == "GetOptionAsync" && tag == "FRAMEElement") richTextBox1.Rtf = handbook[164];
                    if (value == "GetTextFromElementAsync" && tag == "FRAMEElement") richTextBox1.Rtf = handbook[165];
                    if (value == "GetTitleAsync" && tag == "FRAMEElement") richTextBox1.Rtf = handbook[166];
                    if (value == "GetUrlAsync" && tag == "FRAMEElement") richTextBox1.Rtf = handbook[167];
                    if (value == "GetValueFromElementAsync" && tag == "FRAMEElement") richTextBox1.Rtf = handbook[168];
                    if (value == "IsClickableElementAsync" && tag == "FRAMEElement") richTextBox1.Rtf = handbook[169];
                    if (value == "ScrollToElementAsync" && tag == "FRAMEElement") richTextBox1.Rtf = handbook[170];
                    if (value == "SelectOptionAsync" && tag == "FRAMEElement") richTextBox1.Rtf = handbook[171];
                    if (value == "SetAttributeInElementAsync" && tag == "FRAMEElement") richTextBox1.Rtf = handbook[172];
                    if (value == "SetAttributeInElementsAsync" && tag == "FRAMEElement") richTextBox1.Rtf = handbook[173];
                    if (value == "SetHtmlInElementAsync" && tag == "FRAMEElement") richTextBox1.Rtf = handbook[174];
                    if (value == "SetTextInElementAsync" && tag == "FRAMEElement") richTextBox1.Rtf = handbook[175];
                    if (value == "SetValueInElementAsync" && tag == "FRAMEElement") richTextBox1.Rtf = handbook[176];
                    if (value == "WaitNotVisibleElementAsync" && tag == "FRAMEElement") richTextBox1.Rtf = handbook[177];
                    if (value == "WaitVisibleElementAsync" && tag == "FRAMEElement") richTextBox1.Rtf = handbook[178];
                    if (value == "GetTestResult" && tag == "Tester") richTextBox1.Rtf = handbook[179];
                    if (value == "TimerStart" && tag == "Tester") richTextBox1.Rtf = handbook[180];
                    if (value == "TimerStop" && tag == "Tester") richTextBox1.Rtf = handbook[181];
                    if (value == "AssertNoErrorsAsync" && tag == "Tester") richTextBox1.Rtf = handbook[182];
                    if (value == "AssertNetworkEventsAsync" && tag == "Tester") richTextBox1.Rtf = handbook[183];
                    if (value == "SendMsgToMailAsync" && tag == "Tester") richTextBox1.Rtf = handbook[184];
                    if (value == "SendMsgToTelegramAsync" && tag == "Tester") richTextBox1.Rtf = handbook[185];
                    if (value == "AssertNotNullAsync" && tag == "Tester") richTextBox1.Rtf = handbook[186];
                    if (value == "AssertNullAsync" && tag == "Tester") richTextBox1.Rtf = handbook[187];
                    if (value == "WaitElementInDomAsync" && tag == "Tester") richTextBox1.Rtf = handbook[188];
                    if (value == "WaitElementNotDomAsync" && tag == "Tester") richTextBox1.Rtf = handbook[189];
                    if (value == "GoToUrlBaseAuthAsync" && tag == "Tester") richTextBox1.Rtf = handbook[190];
                    if (value == "BrowserPageReloadAsync" && tag == "Tester") richTextBox1.Rtf = handbook[191];
                    if (value == "GetListRedirectUrlAsync" && tag == "Tester") richTextBox1.Rtf = handbook[192];
                    if (value == "GetUrlResponseAsync" && tag == "Tester") richTextBox1.Rtf = handbook[193];
                    if (value == "RestPostAsync" && tag == "Tester") richTextBox1.Rtf = handbook[194];
                    if (value == "GetLocatorAsync" && tag == "HTMLElement") richTextBox1.Rtf = handbook[195];
                    if (value == "ClickMouseAsync" && tag == "HTMLElement") richTextBox1.Rtf = handbook[196];
                    if (value == "RestGetStatusCodeAsync" && tag == "Tester") richTextBox1.Rtf = handbook[197];
                    if (value == "BrowserClearNetworkAsync" && tag == "Tester") richTextBox1.Rtf = handbook[198];
                    if (value == "Description" && tag == "Tester") richTextBox1.Rtf = handbook[199];
                    if (value == "SendMessageDebug" && tag == "Tester") richTextBox1.Rtf = handbook[200];
                    if (value == "EditMessageDebug" && tag == "Tester") richTextBox1.Rtf = handbook[201];
                    if (value == "GetStyleAsync" && tag == "HTMLElement") richTextBox1.Rtf = handbook[202];
                    if (value == "SetStyleAsync" && tag == "HTMLElement") richTextBox1.Rtf = handbook[203];
                    if (value == "GetStyleFromElementAsync" && tag == "FRAMEElement") richTextBox1.Rtf = handbook[204];
                    if (value == "SetStyleInElementAsync" && tag == "FRAMEElement") richTextBox1.Rtf = handbook[205];
                    if (value == "GetStyleFromElementAsync" && tag == "Tester") richTextBox1.Rtf = handbook[206];
                    if (value == "GetStyleFromElementByClassAsync" && tag == "Tester") richTextBox1.Rtf = handbook[207];
                    if (value == "GetStyleFromElementByIdAsync" && tag == "Tester") richTextBox1.Rtf = handbook[208];
                    if (value == "GetStyleFromElementByNameAsync" && tag == "Tester") richTextBox1.Rtf = handbook[209];
                    if (value == "GetStyleFromElementByTagAsync" && tag == "Tester") richTextBox1.Rtf = handbook[210];
                    if (value == "SetStyleInElementAsync" && tag == "Tester") richTextBox1.Rtf = handbook[211];
                    if (value == "SetStyleInElementByClassAsync" && tag == "Tester") richTextBox1.Rtf = handbook[212];
                    if (value == "SetStyleInElementByIdAsync" && tag == "Tester") richTextBox1.Rtf = handbook[213];
                    if (value == "SetStyleInElementByNameAsync" && tag == "Tester") richTextBox1.Rtf = handbook[214];
                    if (value == "SetStyleInElementByTagAsync" && tag == "Tester") richTextBox1.Rtf = handbook[215];
                    if (value == "DEFAULT" && tag == "Tester") richTextBox1.Rtf = handbook[216];
                    if (value == "UTF8" && tag == "Tester") richTextBox1.Rtf = handbook[217];
                    if (value == "UTF8BOM" && tag == "Tester") richTextBox1.Rtf = handbook[218];
                    if (value == "WINDOWS1251" && tag == "Tester") richTextBox1.Rtf = handbook[219];
                    if (value == "FileReadAsync" && tag == "Tester") richTextBox1.Rtf = handbook[220];
                    if (value == "FileWriteAsync" && tag == "Tester") richTextBox1.Rtf = handbook[221];
                    if (value == "FileDownloadAsync" && tag == "Tester") richTextBox1.Rtf = handbook[222];
                    if (value == "FileGetHashMD5Async" && tag == "Tester") richTextBox1.Rtf = handbook[223];
                    if (value == "CreateHashMD5FromTextAsync" && tag == "Tester") richTextBox1.Rtf = handbook[224];
                    if (value == "DisableDebugInReport" && tag == "Tester") richTextBox1.Rtf = handbook[225];
                    if (value == "IsVisibleElementAsync" && tag == "Tester") richTextBox1.Rtf = handbook[226];
                    if (value == "IsVisibleElementAsync" && tag == "FRAMEElement") richTextBox1.Rtf = handbook[227];

                    /*
                    if (value == "" && tag == "") richTextBox1.Rtf = handbook[228];
                    if (value == "" && tag == "") richTextBox1.Rtf = handbook[229];
                    if (value == "" && tag == "") richTextBox1.Rtf = handbook[230];
                    */

                }
            }
            catch (Exception ex)
            {
                Config.browserForm.ConsoleMsgError(ex.ToString());
            }
        }

        private void setValueInCode()
        {
            try
            {
                int index = tabControl1.SelectedIndex;
                int count = files.Count;
                if (index < 0 && count <= 0) return;

                if (treeView1.SelectedNode != null)
                {
                    if (treeView1.SelectedNode.Text == "Класс: Tester") return;
                    if (treeView1.SelectedNode.Text == "Конструктор") return;
                    if (treeView1.SelectedNode.Text == "Константы") return;
                    if (treeView1.SelectedNode.Text == "Переменные") return;
                    if (treeView1.SelectedNode.Text == "Методы для работы с браузером") return;
                    if (treeView1.SelectedNode.Text == "Методы для вывода сообщений") return;
                    if (treeView1.SelectedNode.Text == "Методы для подготовки и завершения тестирования") return;

                    if (treeView1.SelectedNode.Text == "Методы для выполнения действий") return;
                    if (treeView1.SelectedNode.Text == "Нажатие") return;
                    if (treeView1.SelectedNode.Text == "Поиск") return;
                    if (treeView1.SelectedNode.Text == "Атрибуты") return;
                    if (treeView1.SelectedNode.Text == "Объекты") return;
                    if (treeView1.SelectedNode.Text == "Текст") return;
                    if (treeView1.SelectedNode.Text == "Стили") return;
                    if (treeView1.SelectedNode.Text == "Страница") return;
                    if (treeView1.SelectedNode.Text == "Значение") return;
                    if (treeView1.SelectedNode.Text == "Ожидание") return;

                    if (treeView1.SelectedNode.Text == "Методы для выполнения JavaScript") return;
                    if (treeView1.SelectedNode.Text == "Методы для выполнения Rest запросов") return;
                    if (treeView1.SelectedNode.Text == "Методы для замера затраченного времени") return;
                    if (treeView1.SelectedNode.Text == "Методы для отправки email и message") return;
                    if (treeView1.SelectedNode.Text == "Методы для проверки результата") return;
                    if (treeView1.SelectedNode.Text == "Методы для работы с файлами") return;
                    if (treeView1.SelectedNode.Text == "Методы для разных задач") return;

                    if (treeView1.SelectedNode.Text == "Класс: FRAMEElement") return;
                    if (treeView1.SelectedNode.Text == "Класс: HTMLElement") return;
                    if (treeView1.SelectedNode.Text == "Конструктор") return;
                    if (treeView1.SelectedNode.Text == "Переменные") return;
                    if (treeView1.SelectedNode.Text == "Методы") return;

                    Clipboard.SetText(treeView1.SelectedNode.Text);
                    (files[index][5] as TextEditor).Paste();

                    //Clipboard.SetText(treeView1.SelectedNode.Text);
                    //(files[index][5] as RichTextBox).Focus();
                    //SendKeys.Send("+{INSERT}");
                    //SendKeys.Send("^{v}");
                    //SendKeys.Send("^v");
                    //SendKeys.Send("^(v)");
                }
            }
            catch (Exception ex)
            {
                Config.browserForm.ConsoleMsgError(ex.ToString());
            }
        }

        private void treeView1_DoubleClick(object sender, EventArgs e)
        {
            setValueInCode();
        }

        private void вставитьВКодToolStripMenuItem_Click(object sender, EventArgs e)
        {
            setValueInCode();
        }

        private void toolStripButton7_Click(object sender, EventArgs e)
        {
            try
            {
                int index = tabControl1.SelectedIndex;
                int count = files.Count;
                if (index < 0 && count <= 0) return;
                (files[index][5] as TextEditor).Focus();
                //SendKeys.Send("^f");
                SendKeys.SendWait("^{f}");
            }
            catch (Exception ex)
            {
                Config.browserForm.ConsoleMsgError(ex.ToString());
            }
        }
    }
}
