using CefSharp;
using CefSharp.WinForms;
using System;
using System.Drawing;
using System.Windows.Forms;

namespace UntitledBrowser
{
    
    public partial class Form1 : Form
    {

        private Point lastClickPos;
        public Form1()
        {
            InitializeComponent();
            this.KeyPreview = true;
        }



        private void Form1_Load(object sender, EventArgs e)
        {
            CefSettings settings = new CefSettings();
            Cef.Initialize(settings);
            textBox1.Text = "https://google.com";
            ChromiumWebBrowser chrome = new ChromiumWebBrowser(textBox1.Text);
            chrome.Parent = tabControl1.SelectedTab;
            chrome.Dock = DockStyle.Fill;
            chrome.AddressChanged += Chrome_AddressChanged;
            chrome.TitleChanged += Chrome_TitleChanged;
            tabControl1.TabPages.Insert(1, "+");
        }

        private void Chrome_AddressChanged(object sender, AddressChangedEventArgs e)
        {
            this.Invoke(new MethodInvoker(() =>
            {
                textBox1.Text = e.Address;
            }));
        }

        private void button4_Click(object sender, EventArgs e)
        {
            contextMenuStrip1.Show(button4, new Point(0, button4.Height));
        }

        private void button3_Click(object sender, EventArgs e)
        {
            ChromiumWebBrowser chrome = tabControl1.SelectedTab.Controls[0] as ChromiumWebBrowser;
            if (chrome != null)
                chrome.Load(textBox1.Text);
        }

        private void button5_Click(object sender, EventArgs e)
        {
            ChromiumWebBrowser chrome = tabControl1.SelectedTab.Controls[0] as ChromiumWebBrowser;
            if (chrome != null)
                chrome.Refresh();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            ChromiumWebBrowser chrome = tabControl1.SelectedTab.Controls[0] as ChromiumWebBrowser;
            if (chrome != null)
            {
                if (chrome.CanGoForward)
                    chrome.Forward();
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            ChromiumWebBrowser chrome = tabControl1.SelectedTab.Controls[0] as ChromiumWebBrowser;
            if (chrome != null)
            {
                if (chrome.CanGoBack)
                    chrome.Back();
            }
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            Cef.Shutdown();
        }

        private void newTabToolStripMenuItem_Click(object sender, EventArgs e)
        {
            TabPage tab = new TabPage();
            tab.Text = "Google";
            tabControl1.TabPages.Insert(tabControl1.TabCount - 1, tab);
            tabControl1.SelectTab(tabControl1.TabCount - 2);
            ChromiumWebBrowser chrome = new ChromiumWebBrowser("https://google.com");
            chrome.Parent = tab;
            chrome.Dock = DockStyle.Fill;
            textBox1.Text = "https://google.com";
            chrome.AddressChanged += Chrome_AddressChanged;
            chrome.TitleChanged += Chrome_TitleChanged;
        }

        private void Chrome_TitleChanged(object sender, TitleChangedEventArgs e)
        {
            this.Invoke(new MethodInvoker(() =>
            {
                tabControl1.SelectedTab.Text = e.Title;
            }));
        }

        private void showDevToolsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ChromiumWebBrowser chrome = tabControl1.SelectedTab.Controls[0] as ChromiumWebBrowser;
            chrome.ShowDevTools();
        }

        private void tabControl1_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                lastClickPos = Cursor.Position;
                this.contextMenuStrip2.Show(this.tabControl1, e.Location);
            }
        }

        private void closeTabToolStripMenuItem_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < tabControl1.TabCount - 1; i++)
            {
                if (tabControl1.GetTabRect(i).Contains(tabControl1.PointToClient(lastClickPos)))
                {
                    if (i == tabControl1.SelectedIndex)
                    {
                        tabControl1.TabPages[i].Dispose();
                        try
                        {
                            tabControl1.SelectTab(i - 1);
                        }
                        catch (ArgumentOutOfRangeException)
                        {
                            tabControl1.SelectTab(0);
                        }
                    }
                    else if (i != tabControl1.SelectedIndex)
                    {
                        tabControl1.TabPages[i].Dispose();
                    }
                }
            }
        }

        private void tabControl1_Selecting(object sender, TabControlCancelEventArgs e)
        {
                if (e.TabPage.Text == "+")
                {
                    TabPage tab = new TabPage();
                    tab.Text = "Google";
                    tabControl1.TabPages.Insert(tabControl1.TabCount - 1, tab);
                    tabControl1.SelectTab(tabControl1.TabCount - 2);
                    ChromiumWebBrowser chrome = new ChromiumWebBrowser("https://google.com");
                    chrome.Parent = tab;
                    chrome.Dock = DockStyle.Fill;
                    textBox1.Text = "https://google.com";
                    chrome.AddressChanged += Chrome_AddressChanged;
                    chrome.TitleChanged += Chrome_TitleChanged;
                }
        }
        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Control && e.KeyCode == Keys.T)
            {
                TabPage tab = new TabPage();
                tab.Text = "Google";
                tabControl1.TabPages.Insert(tabControl1.TabCount - 1, tab);
                tabControl1.SelectTab(tabControl1.TabCount - 2);
                ChromiumWebBrowser chrome = new ChromiumWebBrowser("https://google.com");
                chrome.Parent = tab;
                chrome.Dock = DockStyle.Fill;
                textBox1.Text = "https://google.com";
                chrome.AddressChanged += Chrome_AddressChanged;
                chrome.TitleChanged += Chrome_TitleChanged;
            }
            if (e.Control && e.KeyCode == Keys.W)
            {
                //BUG:
                //tab unselecting after action
                int currentTab = tabControl1.SelectedIndex;
                try
                {
                    tabControl1.SelectTab(currentTab - 1);
                }
                catch (ArgumentOutOfRangeException)
                {
                    tabControl1.SelectTab(0);
                }
                tabControl1.TabPages[currentTab].Dispose();
            }
        }
    }
    //TODO:
    //text search (you can find it here: https://stackoverflow.com/questions/35884540/how-to-implement-text-search-in-cefsharp)
    //downloading file (you can find it here: https://stackoverflow.com/questions/34289428/download-file-with-cefsharp-winforms)
    //history and favorites (you can find it here: https://www.codeproject.com/Articles/60179/Web-Browser-in-C#History&Favorites4)
    //zoom in/out (you can find it here: https://stackoverflow.com/questions/37681816/cefsharp-chromiumwebbrowser-allow-user-to-zoom-in-out)
    //open new window (you can find it here: https://stackoverflow.com/questions/5957688/creating-windows-forms-open-new-form)
}
