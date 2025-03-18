using System;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Windows.Forms;
using ServerManager;
using SharpShell.ServerRegistration;

namespace AssetBundlePreview;

public partial class MainForm : Form
{
    public MainForm()
    {
        InitializeComponent();
    }

    private void Install(object sender, EventArgs e)
    {
        SharpShell.ServerRegistration.ServerRegistrationManager.InstallServer(mainEntry.Server, RegistrationType.OS64Bit, true);
        SharpShell.ServerRegistration.ServerRegistrationManager.RegisterServer(mainEntry.Server, RegistrationType.OS64Bit);
        MainForm_Load(sender, e);
    }

    private void Uninstall(object sender, EventArgs e)
    {
        SharpShell.ServerRegistration.ServerRegistrationManager.UnregisterServer(mainEntry.Server, RegistrationType.OS64Bit);
        SharpShell.ServerRegistration.ServerRegistrationManager.UninstallServer(mainEntry.Server, RegistrationType.OS64Bit);
        MainForm_Load(sender, e);
    }

    private ServerEntry mainEntry;

    private void MainForm_Load(object sender, EventArgs e)
    {
        var dir = System.IO.Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
        if (!File.Exists(Path.Combine(dir, "Texture2DDecoderNative.dll")))
        {
            label1.Text = "我那么大一个 Texture2DDecoderNative.dll 呢？";
            button1.Enabled = false;
            button2.Enabled = false;
            return;
        }
        
        var serverEntries = ServerManager.ServerManagerApi.LoadServers(Assembly.GetExecutingAssembly().Location);
        mainEntry = serverEntries.FirstOrDefault();
        var info64 = SharpShell.ServerRegistration.ServerRegistrationManager.GetServerRegistrationInfo(mainEntry.Server.ServerClsid,
            RegistrationType.OS64Bit);
        if (info64 != null)
        {
            if (info64.IsApproved)
                label1.Text = "状态：已注册";
            else
                label1.Text = "状态：未注册";
        }
        else
        {
            label1.Text = "状态：未注册";
        }
    }
}