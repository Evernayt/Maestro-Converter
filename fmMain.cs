using Maestro_Converter.Properties;
using Microsoft.Win32;
using System.Diagnostics;
using System.Security.Principal;

namespace Maestro_Converter
{
    public partial class fmMain : Form
    {
        static readonly string appPath = Application.ExecutablePath;
        private readonly string[] targerExtensions = ["jpg", "png", "pdf"];
        private bool isContextMenuCreated = false;

        public fmMain()
        {
            InitializeComponent();
            InitializeAdminSettings();
        }

        private void InitializeAdminSettings()
        {
            if (!IsAdministrator())
            {
                RunAsAdministrator();
            }
            else
            {
                if (Settings.Default.FirstRun)
                {
                    CreateAllContextMenu();
                    Settings.Default.FirstRun = false;
                    Settings.Default.Save();
                }
                else
                {
                    isContextMenuCreated = IsContextMenuCreated();

                    if (isContextMenuCreated)
                    {
                        ShowDeleteButton();
                    }
                    else
                    {
                        ShowCreateButton();
                    }
                }
            }
        }

        private static bool IsAdministrator()
        {
            WindowsIdentity identity = WindowsIdentity.GetCurrent();
            WindowsPrincipal principal = new(identity);
            return principal.IsInRole(WindowsBuiltInRole.Administrator);
        }

        private static void RunAsAdministrator()
        {
            ProcessStartInfo startInfo = new()
            {
                UseShellExecute = true,
                WorkingDirectory = Environment.CurrentDirectory,
                FileName = appPath,
                Verb = "runas"
            };
            try
            {
                Process.Start(startInfo);
            }
            catch
            {
                DialogResult dialogResult = MessageBox.Show("Для изменения настроек приложелия требуются права администратора.", "Maestro Converter", MessageBoxButtons.RetryCancel, MessageBoxIcon.Warning);
                if (dialogResult == DialogResult.Retry)
                {
                    RunAsAdministrator();
                    return;
                }
            }
            Environment.Exit(0);
        }

        private static bool IsContextMenuCreated()
        {
            string subMenuKey = $@"SystemFileAssociations\.jpg\Shell\MaestroConverter";
            using var key = Registry.ClassesRoot.OpenSubKey(subMenuKey);
            return key != null;
        }

        private void CreateAllContextMenu()
        {
            CreateContextMenu("jpg");
            CreateContextMenu("jpeg");
            CreateContextMenu("png");
            CreateContextMenu("webp");
            CreateContextMenu("heic");
            CreateContextMenu("bmp");
            CreateContextMenu("tiff");

            isContextMenuCreated = true;
            ShowDeleteButton();
        }

        private void CreateContextMenu(string extension)
        {
            string subMenuKey = $@"SystemFileAssociations\.{extension}\Shell\MaestroConverter";
            using RegistryKey subMenu = Registry.ClassesRoot.CreateSubKey(subMenuKey);
            subMenu.SetValue("MUIVerb", "Конвертировать");
            subMenu.SetValue("Icon", appPath);
            subMenu.SetValue("MultiSelectModel", "Player");

            string subCommands = "";
            foreach (string targerExtension in targerExtensions)
            {
                if (targerExtension == extension) continue;

                CreateCommand(targerExtension);
                subCommands += $"MaestroConverter.{targerExtension};";
            }
            subMenu.SetValue("SubCommands", subCommands);
        }

        private static void CreateCommand(string targerExtension)
        {
            string commandKey = $@"SOFTWARE\Microsoft\Windows\CurrentVersion\Explorer\CommandStore\Shell\MaestroConverter.{targerExtension}";
            using RegistryKey command = RegistryKey.OpenBaseKey(RegistryHive.LocalMachine, RegistryView.Registry64).OpenSubKey(commandKey, RegistryKeyPermissionCheck.ReadWriteSubTree) ?? RegistryKey.OpenBaseKey(RegistryHive.LocalMachine, RegistryView.Registry64).CreateSubKey(commandKey);
            command.SetValue("MUIVerb", targerExtension.ToUpper());

            string commandPath = $@"SOFTWARE\Microsoft\Windows\CurrentVersion\Explorer\CommandStore\Shell\MaestroConverter.{targerExtension}\command";
            using RegistryKey commandExec = RegistryKey.OpenBaseKey(RegistryHive.LocalMachine, RegistryView.Registry64).OpenSubKey(commandPath, RegistryKeyPermissionCheck.ReadWriteSubTree) ?? RegistryKey.OpenBaseKey(RegistryHive.LocalMachine, RegistryView.Registry64).CreateSubKey(commandPath);

            if (targerExtension == "pdf")
            {
                string singleinstancePath = Path.GetDirectoryName(appPath) + "\\Resources\\singleinstance.exe";
                commandExec.SetValue("", $"\"{singleinstancePath}\" \"%1\" \"{appPath}\" $files --si-timeout 400");
            }
            else
            {
                commandExec.SetValue("", $"\"{appPath}\" \"{targerExtension}\" \"%1\"");
            }
        }

        private void DeleteAllContextMenu()
        {
            DeleteContextMenu("jpg");
            DeleteContextMenu("jpeg");
            DeleteContextMenu("png");
            DeleteContextMenu("webp");
            DeleteContextMenu("heic");
            DeleteContextMenu("bmp");
            DeleteContextMenu("tiff");

            isContextMenuCreated = false;
            ShowCreateButton();
        }

        private void DeleteContextMenu(string extension)
        {
            string subMenuKey = $@"SystemFileAssociations\.{extension}\Shell";
            using (var subMenu = Registry.ClassesRoot.OpenSubKey(subMenuKey, true))
            {
                if (subMenu != null && subMenu.SubKeyCount > 0)
                {
                    subMenu.DeleteSubKeyTree("MaestroConverter", false);
                }
            }

            foreach (string targerExtension in targerExtensions)
            {
                string commandKey = $@"SOFTWARE\Microsoft\Windows\CurrentVersion\Explorer\CommandStore\Shell";

                using var command = RegistryKey.OpenBaseKey(RegistryHive.LocalMachine, RegistryView.Registry64).OpenSubKey(commandKey, true);
                if (command != null && command.SubKeyCount > 0)
                {
                    command.DeleteSubKeyTree($"MaestroConverter.{targerExtension}", false);
                }
            }
        }

        private void btnCreateToggle_Click(object sender, EventArgs e)
        {
            if (isContextMenuCreated)
            {
                DeleteAllContextMenu();
            }
            else
            {
                CreateAllContextMenu();
            }
        }

        private void ShowDeleteButton()
        {
            btnCreateToggle.Text = "Убрать пункт меню";
            btnCreateToggle.BackColor = Color.Firebrick;
            pbxInfo.Image = Resources.success;
            lblInfo.Text = "Пункт меню «Конвертировать» добавлен";
        }

        private void ShowCreateButton()
        {
            btnCreateToggle.Text = "Создать пункт меню";
            btnCreateToggle.BackColor = Color.ForestGreen;
            pbxInfo.Image = Resources.disabled;
            lblInfo.Text = "Пункт меню «Конвертировать» не добавлен";
        }
    }
}
