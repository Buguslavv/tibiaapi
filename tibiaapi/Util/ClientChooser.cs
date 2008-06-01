using System;
using System.Collections.Generic;
using System.Windows.Forms;
using Tibia.Objects;

namespace Tibia.Util
{
    public partial class ClientChooser : Form
    {
        private static ClientChooser newClientChooser;
        private static Objects.Client client;
        
        public ClientChooser()
        {
            InitializeComponent();
            client = null;
        }

        /// <summary>
        /// Opens a box to pick a client.
        /// </summary>
        /// <returns></returns>
        public static Client ShowBox()
        {
            return ShowBox(new ClientChooserOptions());
        }
        /// <summary>
        /// Open a box to pick a client with the desired options.
        /// </summary>
        /// <param name="options"></param>
        /// <returns></returns>
        public static Client ShowBox(ClientChooserOptions options)
        {
            List<Objects.Client> clients = Objects.Client.GetClients();
            if (options.Smart && !options.ShowOTOption && clients.Count == 1)
                return clients[0];
            else
            {
                newClientChooser = new ClientChooser();
                newClientChooser.Text = options.Title == string.Empty ? "Choose a client." : options.Title;
                foreach (Client c in clients)
                    newClientChooser.uxClients.Items.Add(c);
                newClientChooser.uxClients.Items.Add("New client...");
                newClientChooser.uxClients.SelectedIndex = 0;

                if (options.ShowOTOption)
                {
                    newClientChooser.Height = 134;
                    newClientChooser.uxUseOT.Checked = options.UseOT;
                    newClientChooser.SetOTState();
                    newClientChooser.uxServer.Text = options.Server;
                    newClientChooser.uxPort.Text = options.Port.ToString();
                }
                else
                {
                    newClientChooser.Height = 54;
                }

                newClientChooser.ShowDialog();
                return client;
            }
        }

        private void uxChoose_Click(object sender, EventArgs e)
        {
            ChooseClient();
        }

        private void uxUseOT_CheckedChanged(object sender, EventArgs e)
        {
            SetOTState();
            if (uxUseOT.Checked)
                uxServer.Focus();
        }

        public void SetOTState()
        {
            newClientChooser.uxServer.Enabled = uxUseOT.Checked;
            newClientChooser.uxServerLabel.Enabled = uxUseOT.Checked;
            newClientChooser.uxPort.Enabled = uxUseOT.Checked;
            newClientChooser.uxPortLabel.Enabled = uxUseOT.Checked;
        }

        private void ChooseClient()
        {
            if (uxClients.SelectedItem.GetType() == typeof(string))
            {
                client = Client.Open();
                System.Threading.Thread.Sleep(1000);
            }
            else
            {
                client = (Client)uxClients.SelectedItem;
            }

            // Set OT server
            if (client != null && uxUseOT.Checked)
            {
                client.OpenTibiaServer = new LoginServer(uxServer.Text, short.Parse(uxPort.Text));
            }
            newClientChooser.Dispose();
        }

        private void CommonKeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                ChooseClient();
            }
        }
    }

    /// <summary>
    /// Options for the ClientChooser class
    /// </summary>
    public class ClientChooserOptions
    {
        /// <summary>
        /// If true, will not open a box if there is only one 
        /// client; just returns that client.
        /// </summary>
        public bool Smart = true;

        /// <summary>
        /// Use a custom title for the client chooser.
        /// </summary>
        public string Title = string.Empty;

        /// <summary>
        /// Show the open tibia server section
        /// </summary>
        public bool ShowOTOption = true;

        /// <summary>
        /// Default value for the Use OT checkbox
        /// </summary>
        public bool UseOT = false;

        /// <summary>
        /// Default value for the server box
        /// </summary>
        public string Server = null;

        /// <summary>
        /// Default value for the port box
        /// </summary>
        public short Port = 7171;
    }
}