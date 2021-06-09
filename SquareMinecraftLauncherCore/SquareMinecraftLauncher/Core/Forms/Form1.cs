using System;
using System.Windows.Forms;

namespace MicrosoftLoginFroms
{
    public partial class Form1 : Form
    {
        public static string url = "";

        public Form1 () => InitializeComponent();

        void Form1_Load(object sender, EventArgs e)
        {
            webBrowser1.Url = new Uri("https://login.live.com/oauth20_authorize.srf?client_id=00000000402b5328&response_type=code&scope=service%3A%3Auser.auth.xboxlive.com%3A%3AMBI_SSL&redirect_uri=https%3A%2F%2Flogin.live.com%2Foauth20_desktop.srf");
        }
        void webBrowser1_DocumentCompleted(object sender, WebBrowserDocumentCompletedEventArgs e)
        {
        }

        void webBrowser1_Navigated(object sender, WebBrowserNavigatedEventArgs e)
        {
            if (webBrowser1.Url.AbsoluteUri.IndexOf("https://login.live.com/oauth20_desktop.srf") >= 0)
            {
                url = webBrowser1.Url.AbsoluteUri;
                webBrowser1.Dispose();
                Close();
            }
        }
    }
}