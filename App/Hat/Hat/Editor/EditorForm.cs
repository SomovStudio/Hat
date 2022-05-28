using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using ICSharpCode.TextEditor;

namespace Hat
{
    public partial class EditorForm : Form
    {
        public EditorForm()
        {
            InitializeComponent();
        }

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
\cf0 BrowserView.Reload();\par
BrowserView.Source = new Uri(url);\par
BrowserView.Update();\par
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
\pard\sl276\slmult1\cf1\f0\fs20\lang9 BrowserSetUserAgent\cf0\par
\cf2\f1\lang1049\'ce\'ef\'e8\'f1\'e0\'ed\'e8\'e5\cf0 : \'ec\'e5\'f2\'ee\'e4 \'f3\'f1\'f2\'e0\'ed\'e0\'e2\'eb\'e8\'e2\'e0\'e5\'f2 \'e7\'ed\'e0\'f7\'e5\'ed\'e8\'e5 \f0\lang1033 User-Agent\f1\lang1049  \'e4\'eb\'ff \'e1\'f0\'e0\'f3\'e7\'e5\'f0\'e0\par
\cf2\'d1\'e8\'ed\'f2\'e0\'ea\'f1\'e8\'f1\cf0 : BrowserSetUserAgent(string value)\par
\cf3\par
\cf2\'cf\'f0\'e8\'ec\'e5\'f0\cf3 :\par
\cf0 await tester.BrowserSetUserAgent(\f0\lang1033 ""my user-agent""\f1\lang1049 );\par
}",

@"{\rtf1\ansi\ansicpg1251\deff0\nouicompat\deflang1049{\fonttbl{\f0\fnil\fcharset0 Calibri;}{\f1\fnil\fcharset204 Calibri;}}
{\colortbl ;\red0\green77\blue187;\red155\green0\blue211;\red0\green0\blue0;}
{\*\generator Riched20 10.0.22000}\viewkind4\uc1 
\pard\sl276\slmult1\cf1\f0\fs20\lang9 BrowserGetUserAgent\cf0\par
\cf2\f1\lang1049\'ce\'ef\'e8\'f1\'e0\'ed\'e8\'e5\cf0 : \'ec\'e5\'f2\'ee\'e4 \'e2\'ee\'e7\'e2\'f0\'e0\'f9\'e0\'e5\'f2 \'f1\'f2\'f0\'ee\'f7\'ed\'ee\'e5 \'e7\'ed\'e0\'f7\'e5\'ed\'e8\'e5 \f0\lang1033 User-Agent\f1\lang1049  \'e1\'f0\'e0\'f3\'e7\'e5\'f0\'e0\par
\cf2\'d1\'e8\'ed\'f2\'e0\'ea\'f1\'e8\'f1\cf0 : BrowserGetUserAgent()\par
\cf3\par
\cf2\'cf\'f0\'e8\'ec\'e5\'f0\cf3 :\par
\cf0\f0\lang1033 string ua = \f1\lang1049 await tester.BrowserGetUserAgent();\par
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
\cf2\'d1\'e8\'ed\'f2\'e0\'ea\'f1\'e8\'f1\cf0 : SendMessage(string action, string status, string comment, int image)\par
\cf3\par
\cf2\'cf\'f0\'e8\'ec\'e5\'f0\cf3 :\par
\cf0 int step = \f0\lang1033 tester.\f1\lang1049 SendMessage(""\'f2\'e5\'ea\'f1\'f2 \'e4\'e5\'e9\'f1\'f2\'e2\'e8\'ff"", \f0\lang1033 tester.\f1\lang1049 PROCESS, ""\'f2\'e5\'ea\'f1\'f2 \'ea\'ee\'ec\'ec\'e5\'ed\'f2\'e0\'f0\'e8\'ff"", \f0\lang1033 tester.\f1\lang1049 IMAGE_STATUS_PROCESS);\par
}",

@"{\rtf1\ansi\ansicpg1251\deff0\nouicompat\deflang1049{\fonttbl{\f0\fnil\fcharset0 Calibri;}{\f1\fnil\fcharset204 Calibri;}}
{\colortbl ;\red0\green77\blue187;\red155\green0\blue211;\red0\green0\blue0;}
{\*\generator Riched20 10.0.22000}\viewkind4\uc1 
\pard\sl276\slmult1\cf1\f0\fs20\lang9 EditMessage\cf0\par
\cf2\f1\lang1049\'ce\'ef\'e8\'f1\'e0\'ed\'e8\'e5\cf0 : \'ec\'e5\'f2\'ee\'e4 \'e8\'e7\'ec\'e5\'ed\'ff\'e5\'f2 \'f0\'e0\'ed\'e5\'e5 \'e2\'fb\'e2\'e5\'e4\'e5\'ed\'ed\'ee\'e5 \'f1\'ee\'ee\'e1\'f9\'e5\'ed\'e8\'ff \'e2 \'f2\'e0\'e1\'eb\'e8\'f6\'e5 \'ef\'f0\'ee\'f6\'e5\'f1\'f1\'e0 \'e2\'fb\'ef\'ee\'eb\'ed\'e5\'ed\'e8\'ff \'f2\'e5\'f1\'f2\'e0\par
\cf2\'d1\'e8\'ed\'f2\'e0\'ea\'f1\'e8\'f1\cf0 : EditMessage(int index, string action, string status, string comment, int image)\par
\cf3\par
\cf2\'cf\'f0\'e8\'ec\'e5\'f0\cf3 :\par
\cf0\f0\lang1033 tester.\f1\lang1049 EditMessage(step, ""\'f2\'e5\'ea\'f1\'f2 \'e4\'e5\'e9\'f1\'f2\'e2\'e8\'ff"", \f0\lang1033 tester.\f1\lang1049 PASSED, ""\'f2\'e5\'ea\'f1\'f2 \'ea\'ee\'ec\'ec\'e5\'ed\'f2\'e0\'f0\'e8\'ff"", \f0\lang1033 tester.\f1\lang1049 IMAGE_STATUS_PASSED);\par
\par
\f0\lang1033 tester.\f1\lang1049 EditMessage(step, \f0\lang1033 null\f1\lang1049 , \f0\lang1033 tester.\f1\lang1049 FAILED, ""\'f2\'e5\'ea\'f1\'f2 \'ea\'ee\'ec\'ec\'e5\'ed\'f2\'e0\'f0\'e8\'ff"", \f0\lang1033 tester.\f1\lang1049 IMAGE_STATUS_FAILED);\par
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
\cf2\'d1\'e8\'ed\'f2\'e0\'ea\'f1\'e8\'f1\cf0 : DefineTestStop(int stepIndex)\par
\cf3\par
\cf2\'cf\'f0\'e8\'ec\'e5\'f0\cf3 :\par
\cf0\f0\lang1033 if (tester.DefineTestStop(step) == true) return;\f1\lang1049\par
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
\pard\sl276\slmult1\cf1\f0\fs20\lang9 ClickElementByCssAsync\cf0\par
\cf2\f1\lang1049\'ce\'ef\'e8\'f1\'e0\'ed\'e8\'e5\cf0 : \'ec\'e5\'f2\'ee\'e4 \'e2\'fb\'ef\'ee\'eb\'ed\'ff\'e5\'f2 \'ed\'e0\'e6\'e0\'f2\'e8\'e5 \'ed\'e0 \'fd\'eb\'e5\'ec\'e5\'ed\'f2\par
\cf2\'d1\'e8\'ed\'f2\'e0\'ea\'f1\'e8\'f1\cf0 : ClickElementByCssAsync(string locator)\par
\cf3\par
\cf2\'cf\'f0\'e8\'ec\'e5\'f0\cf3 :\par
\cf0\f0\lang1033 await tester.ClickElementByCssAsync(""#MyElement"");\f1\lang1049\par
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
\pard\sl276\slmult1\cf1\f0\fs20\lang9 FindElementByCssAsync\cf0\par
\cf2\f1\lang1049\'ce\'ef\'e8\'f1\'e0\'ed\'e8\'e5\cf0 : \'ec\'e5\'f2\'ee\'e4 \'e2\'fb\'ef\'ee\'eb\'ed\'ff\'e5\'f2 \'ef\'ee\'e8\'f1\'ea \'fd\'eb\'e5\'ec\'e5\'ed\'f2 \'e2 DOM \'f1 \'ee\'e6\'e8\'e4\'e0\'ed\'e8\'e5\'ec \'e2 \'f1\'e5\'ea\'f3\'ed\'e4\'e0\'f5 \'e8 \'e2\'ee\'e7\'e2\'f0\'e0\'f9\'e0\'e5\'f2 \'eb\'ee\'e3\'e8\'f7\'e5\'f1\'ea\'e8\'e9 \'f0\'e5\'e7\'f3\'eb\'fc\'f2\'e0\'f2 \f0\lang1033 true \f1\lang1049\'e8\'eb\'e8 \f0\lang1033 false\f1\lang1049\par
\cf2\'d1\'e8\'ed\'f2\'e0\'ea\'f1\'e8\'f1\cf0 : FindElementByCssAsync(string locator, int sec)\par
\cf3\par
\cf2\'cf\'f0\'e8\'ec\'e5\'f0\cf3 :\par
\cf0\f0\lang1033 bool result = await tester.FindElementByCssAsync(""#MyElement"", 5);\f1\lang1049\par
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
\pard\sl276\slmult1\cf1\f0\fs20\lang9 FindVisibleElementByCssAsync\cf0\par
\cf2\f1\lang1049\'ce\'ef\'e8\'f1\'e0\'ed\'e8\'e5\cf0 : \'ec\'e5\'f2\'ee\'e4 \'e2\'fb\'ef\'ee\'eb\'ed\'ff\'e5\'f2 \'ef\'ee\'e8\'f1\'ea\f0\lang1033  \f1\lang1049\'e2\'e8\'e7\'f3\'e0\'eb\'fc\'ed\'ee \'ee\'f2\'ee\'e1\'f0\'e0\'e6\'e0\'e5\'ec\'ee\'e3\'ee \'fd\'eb\'e5\'ec\'e5\'ed\'f2\'e0 \'f1 \'ee\'e6\'e8\'e4\'e0\'ed\'e8\'e5\'ec \'e2 \'f1\'e5\'ea\'f3\'ed\'e4\'e0\'f5 \'e8 \'e2\'ee\'e7\'e2\'f0\'e0\'f9\'e0\'e5\'f2 \'eb\'ee\'e3\'e8\'f7\'e5\'f1\'ea\'e8\'e9 \'f0\'e5\'e7\'f3\'eb\'fc\'f2\'e0\'f2 \f0\lang1033 true \f1\lang1049\'e8\'eb\'e8 \f0\lang1033 false\f1\lang1049\par
\cf2\'d1\'e8\'ed\'f2\'e0\'ea\'f1\'e8\'f1\cf0 : FindVisibleElementByCssAsync(string locator, int sec)\par
\cf3\par
\cf2\'cf\'f0\'e8\'ec\'e5\'f0\cf3 :\par
\cf0\f0\lang1033 bool result = await tester.FindVisibleElementByCssAsync(""#MyElement"", 5);\f1\lang1049\par
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
\pard\sl276\slmult1\cf1\f0\fs20\lang9 GetAttributeFromElementByCssAsync\cf0\par
\cf2\f1\lang1049\'ce\'ef\'e8\'f1\'e0\'ed\'e8\'e5\cf0 : \'ec\'e5\'f2\'ee\'e4 \'e2\'ee\'e7\'e2\'f0\'e0\'f9\'e0\'e5\'f2 \'f1\'f2\'f0\'ee\'f7\'ed\'ee\'e5 \'e7\'ed\'e0\'f7\'e5\'ed\'e8\'e5 \'e8\'e7 \'f3\'ea\'e0\'e7\'e0\'ed\'ed\'ee\'e3\'ee \'e0\'f2\'f0\'e8\'e1\'f3\'f2\'e0 \'e2 \'e2\'fb\'e1\'f0\'e0\'ed\'ed\'ee\'ec \'fd\'eb\'e5\'ec\'e5\'ed\'f2\'e5\par
\cf2\'d1\'e8\'ed\'f2\'e0\'ea\'f1\'e8\'f1\cf0 : GetAttributeFromElementByCssAsync(string locator, string attribute)\par
\cf3\par
\cf2\'cf\'f0\'e8\'ec\'e5\'f0\cf3 :\par
\cf0\f0\lang1033 string value = await tester.GetAttributeFromElementByCssAsync(""#MyElement"", ""href"");\f1\lang1049\par
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
\pard\sl276\slmult1\cf1\f0\fs20\lang9 GetAttributeFromElementsByCssAsync\cf0\par
\cf2\f1\lang1049\'ce\'ef\'e8\'f1\'e0\'ed\'e8\'e5\cf0 : \'ec\'e5\'f2\'ee\'e4 \'e2\'ee\'e7\'e2\'f0\'e0\'f9\'e0\'e5\'f2\f0\lang1033  \f1\lang1049\'f1\'ef\'e8\'f1\'ee\'ea \'e7\'ed\'e0\'f7\'e5\'ed\'e8\'e9 \'f3\'ea\'e0\'e7\'e0\'ed\'ed\'ee\'e3\'ee \'e0\'f2\'f0\'e8\'e1\'f3\'f2\'e0 \'e8\'e7 \'ec\'ed\'ee\'e6\'e5\'f1\'f2\'e2\'e0 \'fd\'eb\'e5\'ec\'e5\'ed\'f2\'ee\'e2\par
\cf2\'d1\'e8\'ed\'f2\'e0\'ea\'f1\'e8\'f1\cf0 : GetAttributeFromElementsByCssAsync(string locator, string attribute)\par
\cf3\par
\cf2\'cf\'f0\'e8\'ec\'e5\'f0\cf3 :\par
\cf0\f0\lang1033 List<string> values = await tester.GetAttributeFromElementsByCssAsync(""a"", ""href"");\par
foreach (string value in values)\par
\{\par
\tab tester.ConsoleMsg(value);\par
\}\f1\lang1049\par
}
",

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
\pard\sl276\slmult1\cf1\f0\fs20\lang9 GetCountElementsByCssAsync\cf0\par
\cf2\f1\lang1049\'ce\'ef\'e8\'f1\'e0\'ed\'e8\'e5\cf0 : \'ec\'e5\'f2\'ee\'e4 \'e2\'ee\'e7\'e2\'f0\'e0\'f9\'e0\'e5\'f2\f0\lang1033  \f1\lang1049\'ea\'ee\'eb\'e8\'f7\'e5\'f1\'f2\'e2\'ee \'ed\'e0\'e9\'e4\'e5\'ed\'ed\'fb\'f5 \'fd\'eb\'e5\'ec\'e5\'ed\'f2\'ee\'e2\par
\cf2\'d1\'e8\'ed\'f2\'e0\'ea\'f1\'e8\'f1\cf0 : GetCountElementsByCssAsync(string locator)\par
\cf3\par
\cf2\'cf\'f0\'e8\'ec\'e5\'f0\cf3 :\par
\cf0\f0\lang1033 int count = await tester.GetCountElementsByCssAsync(""#MyElement"");\f1\lang1049\par
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
\pard\sl276\slmult1\cf1\f0\fs20\lang9 GetHtmlElementAsync\cf0\par
\cf2\f1\lang1049\'ce\'ef\'e8\'f1\'e0\'ed\'e8\'e5\cf0 : \f0\lang1033 (\f1\lang1049\'e2 \'f0\'e0\'e7\'f0\'e0\'e1\'ee\'f2\'ea\'e5\f0\lang1033 ) \f1\lang1049\'ec\'e5\'f2\'ee\'e4 \'e2\'ee\'e7\'e2\'f0\'e0\'f9\'e0\'e5\'f2\f0\lang1033  \f1\lang1049\'fd\'eb\'e5\'ec\'e5\'ed\'f2 \'e2 \'e2\'e8\'e4\'e5 \'ee\'e1\'fa\'e5\'ea\'f2 \'ea\'eb\'e0\'f1\'f1 \'ea\'ee\'f2\'ee\'f0\'ee\'e3\'ee \f0\lang1033 HTMLElement\f1\lang1049\par
\cf2\'d1\'e8\'ed\'f2\'e0\'ea\'f1\'e8\'f1\cf0 : GetHtmlElementAsync(string locator)\par
\cf3\par
\cf2\'cf\'f0\'e8\'ec\'e5\'f0\cf3 :\par
\cf0\f0\lang1033 HTMLElement element = await tester.GetHtmlElementAsync(""#MyElement"");\par
await element.ClickAsync();\f1\lang1049\par
    }",

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
\pard\sl276\slmult1\cf1\f0\fs20\lang9 GetTextFromElementByCssAsync\cf0\par
\cf2\f1\lang1049\'ce\'ef\'e8\'f1\'e0\'ed\'e8\'e5\cf0 : \'ec\'e5\'f2\'ee\'e4 \'e2\'ee\'e7\'e2\'f0\'e0\'f9\'e0\'e5\'f2\f0\lang1033  \f1\lang1049\'f2\'e5\'ea\'f1\'f2 \'e8\'e7 \'f3\'ea\'e0\'e7\'e0\'ed\'ed\'ee\'e3\'ee \'fd\'eb\'e5\'ec\'e5\'ed\'f2\'e0\par
\cf2\'d1\'e8\'ed\'f2\'e0\'ea\'f1\'e8\'f1\cf0 : GetTextFromElementByCssAsync(string locator)\par
\cf3\par
\cf2\'cf\'f0\'e8\'ec\'e5\'f0\cf3 :\par
\cf0\f0\lang1033 string text = await tester.GetTextFromElementByCssAsync(""#MyElement"");\f1\lang1049\par
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
\pard\sl276\slmult1\cf1\f0\fs20\lang9 GetValueFromElementByCssAsync\cf0\par
\cf2\f1\lang1049\'ce\'ef\'e8\'f1\'e0\'ed\'e8\'e5\cf0 : \'ec\'e5\'f2\'ee\'e4 \'e2\'ee\'e7\'e2\'f0\'e0\'f9\'e0\'e5\'f2\f0\lang1033  \f1\lang1049\'e7\'ed\'e0\'f7\'e5\'ed\'e8\'e5 \'e8\'e7 \'f3\'ea\'e0\'e7\'e0\'ed\'ed\'ee\'e3\'ee \'fd\'eb\'e5\'ec\'e5\'ed\'f2\'e0\par
\cf2\'d1\'e8\'ed\'f2\'e0\'ea\'f1\'e8\'f1\cf0 : GetValueFromElementByCssAsync(string locator)\par
\cf3\par
\cf2\'cf\'f0\'e8\'ec\'e5\'f0\cf3 :\par
\cf0\f0\lang1033 string value = await tester.GetValueFromElementByCssAsync(""#MyElement"");\f1\lang1049\par
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
\cf2\'d1\'e8\'ed\'f2\'e0\'ea\'f1\'e8\'f1\cf0 : GoToUrlAsync(string url, int sec)\par
\cf3\par
\cf2\'cf\'f0\'e8\'ec\'e5\'f0\cf3 :\par
\cf0\f0\lang1033 await tester.GoToUrlAsync(@""https://www.google.com/"", 25);\f1\lang1049\par
}",

@"{\rtf1\ansi\ansicpg1251\deff0\nouicompat\deflang1049{\fonttbl{\f0\fnil\fcharset0 Calibri;}{\f1\fnil\fcharset204 Calibri;}}
{\colortbl ;\red0\green77\blue187;\red155\green0\blue211;\red0\green0\blue0;}
{\*\generator Riched20 10.0.22000}\viewkind4\uc1 
\pard\sl276\slmult1\cf1\f0\fs20\lang9 ScrollToElementByCssAsync\cf0\par
\cf2\f1\lang1049\'ce\'ef\'e8\'f1\'e0\'ed\'e8\'e5\cf0 : \'ec\'e5\'f2\'ee\'e4 \'e2\'fb\'ef\'ee\'eb\'ed\'ff\'e5\'f2 \'ef\'f0\'ee\'ea\'f0\'f3\'f2\'ea\'f3 \'ea \'f3\'ea\'e0\'e7\'e0\'ed\'ed\'ee\'ec\'f3 \'fd\'eb\'e5\'ec\'e5\'ed\'f2\'f3 (\'ef\'e0\'f0\'e0\'ec\'e5\'f2\'f0 behaviorSmooth \'ee\'ef\'f0\'e5\'e4\'e5\'eb\'ff\'e5\'f2 \'ef\'eb\'e0\'e2\'ed\'ee\'f1\'f2\'fc \'ef\'f0\'ee\'ea\'f0\'f3\'f2\'ea\'e8)\par
\cf2\'d1\'e8\'ed\'f2\'e0\'ea\'f1\'e8\'f1\cf0 : ScrollToElementByCssAsync(string locator, bool behaviorSmooth = false)\par
\cf3\par
\cf2\'cf\'f0\'e8\'ec\'e5\'f0\cf3 :\par
\cf0\f0\lang1033 await tester.ScrollToElementByCssAsync(""#MyElement"", true);\f1\lang1049\par
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
\pard\sl276\slmult1\cf1\f0\fs20\lang9 SetAttributeInElementByCssAsync\cf0\par
\cf2\f1\lang1049\'ce\'ef\'e8\'f1\'e0\'ed\'e8\'e5\cf0 : \'ec\'e5\'f2\'ee\'e4 \'e2\'f1\'f2\'e0\'e2\'eb\'ff\'e5\'f2 \'e0\'f2\'f0\'e8\'e1\'f3\'f2 \'f1\'ee \'e7\'ed\'e0\'f7\'e5\'ed\'e8\'e5\'ec \'e2 \'f3\'ea\'e0\'e7\'e0\'ed\'ed\'fb\'e9 \'fd\'eb\'e5\'ec\'e5\'ed\'f2 \par
\cf2\'d1\'e8\'ed\'f2\'e0\'ea\'f1\'e8\'f1\cf0 : SetAttributeInElementByCssAsync(string locator, string attribute, string value)\par
\cf3\par
\cf2\'cf\'f0\'e8\'ec\'e5\'f0\cf3 :\par
\cf0\f0\lang1033 await tester.SetAttributeInElementByCssAsync(""#MyElement"", ""value"",  ""\f1\lang1049\'e7\'ed\'e0\'f7\'e5\'ed\'e8\'e5\f0\lang1033 "");\f1\lang1049\par
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
\pard\sl276\slmult1\cf1\f0\fs20\lang9 SetTextInElementByCssAsync\cf0\par
\cf2\f1\lang1049\'ce\'ef\'e8\'f1\'e0\'ed\'e8\'e5\cf0 : \'ec\'e5\'f2\'ee\'e4 \'e2\'f1\'f2\'e0\'e2\'eb\'ff\'e5\'f2 \'f2\'e5\'ea\'f1\'f2 \'e2 \'f3\'ea\'e0\'e7\'e0\'ed\'ed\'fb\'e9 \'fd\'eb\'e5\'ec\'e5\'ed\'f2 \par
\cf2\'d1\'e8\'ed\'f2\'e0\'ea\'f1\'e8\'f1\cf0 : SetTextInElementByCssAsync(string locator, string text)\par
\cf3\par
\cf2\'cf\'f0\'e8\'ec\'e5\'f0\cf3 :\par
\cf0\f0\lang1033 await tester.SetTextInElementByCssAsync(""#MyElement"", ""\f1\lang1049\'f2\'e5\'ea\'f1\'f2\f0\lang1033 "");\f1\lang1049\par
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
\pard\sl276\slmult1\cf1\f0\fs20\lang9 SetValueInElementByCssAsync\cf0\par
\cf2\f1\lang1049\'ce\'ef\'e8\'f1\'e0\'ed\'e8\'e5\cf0 : \'ec\'e5\'f2\'ee\'e4 \'e2\'f1\'f2\'e0\'e2\'eb\'ff\'e5\'f2 \'f2\'e5\'ea\'f1\'f2 \'e2 \'f3\'ea\'e0\'e7\'e0\'ed\'ed\'fb\'e9 \'fd\'eb\'e5\'ec\'e5\'ed\'f2 \par
\cf2\'d1\'e8\'ed\'f2\'e0\'ea\'f1\'e8\'f1\cf0 : SetValueInElementByCssAsync(string locator, string value)\par
\cf3\par
\cf2\'cf\'f0\'e8\'ec\'e5\'f0\cf3 :\par
\cf0\f0\lang1033 await tester.SetValueInElementByCssAsync(""#MyElement"", ""\f1\lang1049\'e7\'ed\'e0\'f7\'e5\'ed\'e8\'e5\f0\lang1033 "");\f1\lang1049\par
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
\pard\sl276\slmult1\cf1\f0\fs20\lang9 WaitNotVisibleElementByCssAsync\cf0\par
\cf2\f1\lang1049\'ce\'ef\'e8\'f1\'e0\'ed\'e8\'e5\cf0 : \'ec\'e5\'f2\'ee\'e4 \'e2\'fb\'ef\'ee\'eb\'ed\'ff\'e5\'f2 \'e2\'f0\'e5\'ec\'e5\'ed\'ed\'f3\'fe \'ee\'f1\'f2\'e0\'ed\'ee\'e2\'ea\'f3 \'e2\'fb\'ef\'ee\'eb\'ed\'e5\'ed\'e8\'ff \'f2\'e5\'f1\'f2\'e0 \'ed\'e0 \'f3\'ea\'e0\'e7\'e0\'ed\'ed\'ee\'e5 \'ea\'ee\'eb\'e8\'f7\'e5\'f1\'f2\'e2\'ee \'f1\'e5\'ea\'f3\'ed\'e4\f0\lang1033  \f1\lang1049\'e8 \'e6\'e4\'e5\'f2 \'ea\'ee\'e3\'e4\'e0  \'e7\'e0\'ef\'f0\'e0\'f8\'e8\'e2\'e0\'e5\'ec\'fb\'e9 \'fd\'eb\'e5\'ec\'e5\'ed\'f2 \'ef\'e5\'f0\'e5\'f1\'f2\'e0\'ed\'e5\'f2 \'ee\'f2\'ee\'e1\'f0\'e0\'e6\'e0\'e5\'f2\'fc\'f1\'ff \par
\cf2\'d1\'e8\'ed\'f2\'e0\'ea\'f1\'e8\'f1\cf0 : WaitNotVisibleElementByCssAsync(string locator, int sec)\par
\cf3\par
\cf2\'cf\'f0\'e8\'ec\'e5\'f0\cf3 :\par
\cf0\f0\lang1033 await tester.WaitNotVisibleElementByCssAsync(""#MyElement"", 25);\f1\lang1049\par
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
\pard\sl276\slmult1\cf1\f0\fs20\lang9 WaitVisibleElementByCssAsync\cf0\par
\cf2\f1\lang1049\'ce\'ef\'e8\'f1\'e0\'ed\'e8\'e5\cf0 : \'ec\'e5\'f2\'ee\'e4 \'e2\'fb\'ef\'ee\'eb\'ed\'ff\'e5\'f2 \'e2\'f0\'e5\'ec\'e5\'ed\'ed\'f3\'fe \'ee\'f1\'f2\'e0\'ed\'ee\'e2\'ea\'f3 \'e2\'fb\'ef\'ee\'eb\'ed\'e5\'ed\'e8\'ff \'f2\'e5\'f1\'f2\'e0 \'ed\'e0 \'f3\'ea\'e0\'e7\'e0\'ed\'ed\'ee\'e5 \'ea\'ee\'eb\'e8\'f7\'e5\'f1\'f2\'e2\'ee \'f1\'e5\'ea\'f3\'ed\'e4\f0\lang1033  \f1\lang1049\'e8 \'e6\'e4\'e5\'f2 \'ea\'ee\'e3\'e4\'e0  \'e7\'e0\'ef\'f0\'e0\'f8\'e8\'e2\'e0\'e5\'ec\'fb\'e9 \'fd\'eb\'e5\'ec\'e5\'ed\'f2 \'ee\'f2\'ee\'e1\'f0\'e0\'e7\'e8\'f2\'f1\'ff\par
\cf2\'d1\'e8\'ed\'f2\'e0\'ea\'f1\'e8\'f1\cf0 : WaitVisibleElementByCssAsync(string locator, int sec)\par
\cf3\par
\cf2\'cf\'f0\'e8\'ec\'e5\'f0\cf3 :\par
\cf0\f0\lang1033 await tester.WaitVisibleElementByCssAsync(""#MyElement"", 25);\f1\lang1049\par
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
\cf0\f0\lang1033 string string = ""(function()\{ var element = document.getElementById('MyElement'); return element.innerText; \}());"";\par
string result = await tester.ExecuteJavaScriptAsync(string);\f1\lang1049\par
}
",

@"{\rtf1\ansi\ansicpg1251\deff0\nouicompat\deflang1049{\fonttbl{\f0\fnil\fcharset0 Calibri;}{\f1\fnil\fcharset204 Calibri;}}
{\colortbl ;\red0\green77\blue187;\red155\green0\blue211;\red0\green0\blue0;}
{\*\generator Riched20 10.0.22000}\viewkind4\uc1 
\pard\sl276\slmult1\cf1\f0\fs20\lang9 AssertEqualsAsync\cf0\par
\cf2\f1\lang1049\'ce\'ef\'e8\'f1\'e0\'ed\'e8\'e5\cf0 : \'ec\'e5\'f2\'ee\'e4 \'e2\'fb\'ef\'ee\'eb\'ed\'ff\'e5\'f2 \'ef\'f0\'ee\'e2\'e5\'f0\'ea\'f3 \'ec\'e5\'e6\'e4\'f3 \'f4\'e0\'ea\'f2\'e8\'f7\'e5\'f1\'ea\'e8\'ec \'e8 \'ee\'e6\'e8\'e4\'e0\'e5\'ec\'fb\'ec \'e7\'ed\'e0\'f7\'e5\'ed\'e8\'ff\'ec\'e8, \'e2 \'f1\'eb\'f3\'f7\'e0\'e5 \'ed\'e5\'f1\'ee\'e2\'ef\'e0\'e4\'e5\'ed\'e8\'ff \'ef\'f0\'ee\'e2\'e5\'f0\'ea\'e0 \'e1\'f3\'e4\'e5\'f2 \'f1\'f7\'e8\'f2\'e0\'f2\'fc\'f1\'ff \'ef\'f0\'ee\'e2\'e0\'eb\'fc\'ed\'ee\'e9\par
\cf2\'d1\'e8\'ed\'f2\'e0\'ea\'f1\'e8\'f1\cf0 : AssertEqualsAsync(string expected, string actual)\par
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
\cf2\'d1\'e8\'ed\'f2\'e0\'ea\'f1\'e8\'f1\cf0 : AssertNotEqualsAsync(string expected, string actual)\par
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

@"",
@"",
@""
        };

        private void EditorForm_Load(object sender, EventArgs e)
        {
            try
            {
                this.Text = Config.selectName;
                this.TopMost = Config.editorTopMost;
                WorkOnFiles reader = new WorkOnFiles();
                textEditorControl1.SetHighlighting("C#");
                textEditorControl1.Text = reader.readFile(Config.encoding, Config.selectValue);
                toolStripStatusLabel2.Text = Config.encoding;
                toolStripStatusLabel5.Text = Config.selectValue;
                toolStripStatusLabel6.Text = "";

                
            }
            catch (Exception ex)
            {
                Config.browserForm.consoleMsgError(ex.ToString());
            }
        }

        private void toolStripButton1_Click(object sender, EventArgs e)
        {
            if (this.Text == "") return;
            try
            {
                WorkOnFiles write = new WorkOnFiles();
                write.writeFile(textEditorControl1.Text, toolStripStatusLabel2.Text, toolStripStatusLabel5.Text);
                Config.browserForm.consoleMsg($"Файл {this.Text} - сохранён");
                toolStripStatusLabel6.Text = "";
            }
            catch (Exception ex)
            {
                Config.browserForm.consoleMsgError(ex.ToString());
            }
        }

        private void toolStripButton2_Click(object sender, EventArgs e)
        {
            try
            {
                if (saveFileDialog1.ShowDialog() == DialogResult.OK)
                {
                    this.Text = Config.browserForm.getFileName(saveFileDialog1.FileName);
                    toolStripStatusLabel5.Text = saveFileDialog1.FileName;

                    WorkOnFiles write = new WorkOnFiles();
                    write.writeFile(textEditorControl1.Text, toolStripStatusLabel2.Text, toolStripStatusLabel5.Text);
                    Config.browserForm.consoleMsg($"Файл {this.Text} - сохранён");
                    Config.browserForm.projectUpdate();
                }
            }
            catch (Exception ex)
            {
                Config.browserForm.consoleMsgError(ex.ToString());
            }
        }

        private void toolStripButton3_Click(object sender, EventArgs e)
        {
            if(this.Text != "") Config.browserForm.PlayTest(this.Text);
        }

        private void toolStripButton4_Click(object sender, EventArgs e)
        {
            Config.browserForm.StopTest();
        }

        private void setValueInCode()
        {
            try
            {
                if (treeView1.SelectedNode != null)
                {
                    if (treeView1.SelectedNode.Text == "Класс: Tester") return;
                    if (treeView1.SelectedNode.Text == "Конструктор") return;
                    if (treeView1.SelectedNode.Text == "Константы") return;
                    if (treeView1.SelectedNode.Text == "Переменные") return;
                    if (treeView1.SelectedNode.Text == "Методы для работы с браузером") return;
                    if (treeView1.SelectedNode.Text == "Методы для вывода сообщений") return;
                    if (treeView1.SelectedNode.Text == "Методы для подготовки и завершению тестирования") return;
                    if (treeView1.SelectedNode.Text == "Методы выполнения действий") return;
                    if (treeView1.SelectedNode.Text == "Методы выполнения JavaScript") return;
                    if (treeView1.SelectedNode.Text == "Методы для проверки результата") return;
                    
                    Clipboard.SetText(treeView1.SelectedNode.Text);
                    textEditorControl1.Focus();
                    SendKeys.Send("+{INSERT}");
                    //SendKeys.Send("^{v}");
                    //SendKeys.Send("^v");
                    //SendKeys.Send("^(v)");
                }
            }
            catch (Exception ex)
            {
                Config.browserForm.consoleMsgError(ex.ToString());
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

        private void treeView1_AfterSelect(object sender, TreeViewEventArgs e)
        {
            try
            {
                richTextBox1.Text = "";
                richTextBox1.Rtf = "";
                if (treeView1.SelectedNode != null)
                {
                    string value = treeView1.SelectedNode.Text;
                    if (value == "Tester") richTextBox1.Rtf = handbook[0];
                    if (value == "IMAGE_STATUS_PROCESS") richTextBox1.Rtf = handbook[1];
                    if (value == "IMAGE_STATUS_PASSED") richTextBox1.Rtf = handbook[2];
                    if (value == "IMAGE_STATUS_FAILED") richTextBox1.Rtf = handbook[3];
                    if (value == "IMAGE_STATUS_MESSAGE") richTextBox1.Rtf = handbook[4];
                    if (value == "IMAGE_STATUS_WARNING") richTextBox1.Rtf = handbook[5];
                    if (value == "PASSED") richTextBox1.Rtf = handbook[6];
                    if (value == "FAILED") richTextBox1.Rtf = handbook[7];
                    if (value == "STOPPED") richTextBox1.Rtf = handbook[8];
                    if (value == "PROCESS") richTextBox1.Rtf = handbook[9];
                    if (value == "COMPLETED") richTextBox1.Rtf = handbook[10];
                    if (value == "WARNING") richTextBox1.Rtf = handbook[11];

                    if (value == "BrowserView") richTextBox1.Rtf = handbook[12];
                    if (value == "BrowserWindow") richTextBox1.Rtf = handbook[13];

                    if (value == "BrowserCloseAsync") richTextBox1.Rtf = handbook[14];
                    if (value == "BrowserSizeAsync") richTextBox1.Rtf = handbook[15];
                    if (value == "BrowserFullScreenAsync") richTextBox1.Rtf = handbook[16];
                    if (value == "BrowserSetUserAgent") richTextBox1.Rtf = handbook[17];
                    if (value == "BrowserGetUserAgent") richTextBox1.Rtf = handbook[18];

                    if (value == "ConsoleMsg") richTextBox1.Rtf = handbook[19];
                    if (value == "ConsoleMsgError") richTextBox1.Rtf = handbook[20];
                    if (value == "ClearMessage") richTextBox1.Rtf = handbook[21];
                    if (value == "SendMessage") richTextBox1.Rtf = handbook[22];
                    if (value == "EditMessage") richTextBox1.Rtf = handbook[23];

                    if (value == "TestBeginAsync") richTextBox1.Rtf = handbook[24];
                    if (value == "TestEndAsync") richTextBox1.Rtf = handbook[25];
                    if (value == "TestStopAsync") richTextBox1.Rtf = handbook[26];
                    if (value == "DefineTestStop") richTextBox1.Rtf = handbook[27];

                    if (value == "ClickElementByClassAsync") richTextBox1.Rtf = handbook[28];
                    if (value == "ClickElementByCssAsync") richTextBox1.Rtf = handbook[29];
                    if (value == "ClickElementByIdAsync") richTextBox1.Rtf = handbook[30];
                    if (value == "ClickElementByNameAsync") richTextBox1.Rtf = handbook[31];
                    if (value == "ClickElementByTagAsync") richTextBox1.Rtf = handbook[32];
                    if (value == "FindElementByClassAsync") richTextBox1.Rtf = handbook[33];
                    if (value == "FindElementByCssAsync") richTextBox1.Rtf = handbook[34];
                    if (value == "FindElementByIdAsync") richTextBox1.Rtf = handbook[35];
                    if (value == "FindElementByNameAsync") richTextBox1.Rtf = handbook[36];
                    if (value == "FindElementByTagAsync") richTextBox1.Rtf = handbook[37];
                    if (value == "FindVisibleElementByClassAsync") richTextBox1.Rtf = handbook[38];
                    if (value == "FindVisibleElementByCssAsync") richTextBox1.Rtf = handbook[39];
                    if (value == "FindVisibleElementByIdAsync") richTextBox1.Rtf = handbook[40];
                    if (value == "FindVisibleElementByNameAsync") richTextBox1.Rtf = handbook[41];
                    if (value == "FindVisibleElementByTagAsync") richTextBox1.Rtf = handbook[42];
                    if (value == "GetAttributeFromElementByClassAsync") richTextBox1.Rtf = handbook[43];
                    if (value == "GetAttributeFromElementByCssAsync") richTextBox1.Rtf = handbook[44];
                    if (value == "GetAttributeFromElementByIdAsync") richTextBox1.Rtf = handbook[45];
                    if (value == "GetAttributeFromElementByNameAsync") richTextBox1.Rtf = handbook[46];
                    if (value == "GetAttributeFromElementByTagAsync") richTextBox1.Rtf = handbook[47];
                    if (value == "GetAttributeFromElementsByCssAsync") richTextBox1.Rtf = handbook[48];
                    if (value == "GetCountElementsByClassAsync") richTextBox1.Rtf = handbook[49];
                    if (value == "GetCountElementsByCssAsync") richTextBox1.Rtf = handbook[50];
                    if (value == "GetCountElementsByNameAsync") richTextBox1.Rtf = handbook[51];
                    if (value == "GetCountElementsByTagAsync") richTextBox1.Rtf = handbook[52];
                    if (value == "GetHtmlElementAsync") richTextBox1.Rtf = handbook[53];
                    if (value == "GetTextFromElementByClassAsync") richTextBox1.Rtf = handbook[54];
                    if (value == "GetTextFromElementByCssAsync") richTextBox1.Rtf = handbook[55];
                    if (value == "GetTextFromElementByIdAsync") richTextBox1.Rtf = handbook[56];
                    if (value == "GetTextFromElementByNameAsync") richTextBox1.Rtf = handbook[57];
                    if (value == "GetTextFromElementByTagAsync") richTextBox1.Rtf = handbook[58];
                    if (value == "GetTitleAsync") richTextBox1.Rtf = handbook[59];
                    if (value == "GetUrlAsync") richTextBox1.Rtf = handbook[60];
                    if (value == "GetValueFromElementByClassAsync") richTextBox1.Rtf = handbook[61];
                    if (value == "GetValueFromElementByCssAsync") richTextBox1.Rtf = handbook[62];
                    if (value == "GetValueFromElementByIdAsync") richTextBox1.Rtf = handbook[63];
                    if (value == "GetValueFromElementByNameAsync") richTextBox1.Rtf = handbook[64];
                    if (value == "GetValueFromElementByTagAsync") richTextBox1.Rtf = handbook[65];
                    if (value == "GoToUrlAsync") richTextBox1.Rtf = handbook[66];
                    if (value == "ScrollToElementByCssAsync") richTextBox1.Rtf = handbook[67];
                    if (value == "SetAttributeInElementByClassAsync") richTextBox1.Rtf = handbook[68];
                    if (value == "SetAttributeInElementByCssAsync") richTextBox1.Rtf = handbook[69];
                    if (value == "SetAttributeInElementByIdAsync") richTextBox1.Rtf = handbook[70];
                    if (value == "SetAttributeInElementByNameAsync") richTextBox1.Rtf = handbook[71];
                    if (value == "SetAttributeInElementByTagAsync") richTextBox1.Rtf = handbook[72];
                    if (value == "SetTextInElementByClassAsync") richTextBox1.Rtf = handbook[73];
                    if (value == "SetTextInElementByCssAsync") richTextBox1.Rtf = handbook[74];
                    if (value == "SetTextInElementByIdAsync") richTextBox1.Rtf = handbook[75];
                    if (value == "SetTextInElementByNameAsync") richTextBox1.Rtf = handbook[76];
                    if (value == "SetTextInElementByTagAsync") richTextBox1.Rtf = handbook[77];
                    if (value == "SetValueInElementByClassAsync") richTextBox1.Rtf = handbook[78];
                    if (value == "SetValueInElementByCssAsync") richTextBox1.Rtf = handbook[79];
                    if (value == "SetValueInElementByIdAsync") richTextBox1.Rtf = handbook[80];
                    if (value == "SetValueInElementByNameAsync") richTextBox1.Rtf = handbook[81];
                    if (value == "SetValueInElementByTagAsync") richTextBox1.Rtf = handbook[82];
                    if (value == "WaitAsync") richTextBox1.Rtf = handbook[83];
                    if (value == "WaitNotVisibleElementByClassAsync") richTextBox1.Rtf = handbook[84];
                    if (value == "WaitNotVisibleElementByCssAsync") richTextBox1.Rtf = handbook[85];
                    if (value == "WaitNotVisibleElementByIdAsync") richTextBox1.Rtf = handbook[86];
                    if (value == "WaitNotVisibleElementByNameAsync") richTextBox1.Rtf = handbook[87];
                    if (value == "WaitNotVisibleElementByTagAsync") richTextBox1.Rtf = handbook[88];
                    if (value == "WaitVisibleElementByClassAsync") richTextBox1.Rtf = handbook[89];
                    if (value == "WaitVisibleElementByCssAsync") richTextBox1.Rtf = handbook[90];
                    if (value == "WaitVisibleElementByIdAsync") richTextBox1.Rtf = handbook[91];
                    if (value == "WaitVisibleElementByNameAsync") richTextBox1.Rtf = handbook[92];
                    if (value == "WaitVisibleElementByTagAsync") richTextBox1.Rtf = handbook[93];

                    if (value == "ExecuteJavaScriptAsync") richTextBox1.Rtf = handbook[94];

                    if (value == "AssertEqualsAsync") richTextBox1.Rtf = handbook[95];
                    if (value == "AssertNotEqualsAsync") richTextBox1.Rtf = handbook[96];
                    if (value == "AssertTrueAsync") richTextBox1.Rtf = handbook[97];
                    if (value == "AssertFalseAsync") richTextBox1.Rtf = handbook[98];
                }
            }
            catch (Exception ex)
            {
                Config.browserForm.consoleMsgError(ex.ToString());
            }
        }

        private void textEditorControl1_TextChanged(object sender, EventArgs e)
        {
            toolStripStatusLabel6.Text = "(изменения не сохранены) |";
        }

        private void EditorForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            if(toolStripStatusLabel6.Text == "(изменения не сохранены) |")
            {
                if(MessageBox.Show("Изменения не сохранены, всё равно закрыть редактор?", "Вопрос", MessageBoxButtons.YesNo) == DialogResult.No)
                {
                    e.Cancel = true;
                }
            }
        }
    }
}
