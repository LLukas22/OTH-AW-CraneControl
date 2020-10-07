using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Input;
using CraneControl;
using Cranecontrol.Server;
using CraneControl.Server;
using Kinect;
using Prism.Events;
using SharedRessources;

namespace OTH_AW_CraneControl
{
    public partial class CraneControlForm : Form
    {

        private readonly IEventAggregator eventAggregator = new EventAggregator();

        private KinectInIt kinect;

        private Settings settings = new Settings();
        private readonly Payload payload;
        private readonly TCPServer tcpServer;
        private Instructions directionInstructions = new Instructions();

        private bool regler = false;
        private bool axis = false;
        private bool Key_down = false;



        public CraneControlForm()
        {
            InitializeComponent();

            settings = settings.Load() ?? new Settings();
            eventAggregator.GetEvent<FrameEvent>().Subscribe(FrameArrived);

            EnumToComboBox<Frames>(cBkinectframe);
            cBkinectframe.SelectedIndex = 1;

            kinect = new KinectInIt(eventAggregator, (Frames)((cBkinectframe as ComboBox).SelectedItem as dynamic).value);

            payload = new Payload(settings.Acceleration);
            tcpServer = new TCPServer(payload, settings.Port);


            txPort.Text = settings.Port.ToString();
            txAcceleration.Text = settings.Acceleration.ToString();

            cpbVelocity.progressBrush = Brushes.Green;
            cpbVelocity.progressFont = new Font("Arial", 10);
            cpbVelocity.progressFontBrush = Brushes.Black;
            cpbVelocity.Refresh(0);

            ColorDirectionButtons(directionInstructions);

            tcpServer.ClientConnectedEvent += TcpServer_ClientConnectedEvent;

            splitContainer1.Panel2Collapsed = true;

        }

        private void TcpServer_ClientConnectedEvent(object sender, client_connected e)
        {
            pBClient.BackgroundImage = e.Client ? CraneControl.Properties.Resources.on : CraneControl.Properties.Resources.off;
        }

        private void FrameArrived(FrameEventArgs frameEventArgs)
        {
            directionInstructions = frameEventArgs.Instructions;
            if(!Key_down) DataToPayload(directionInstructions);
            SetBitmap(frameEventArgs.Bitmap);
        }

        private void DataToPayload(Instructions dirInstructions)
        {
            ColorDirectionButtons(directionInstructions);
            cpbVelocity.Refresh(payload.Refresh(dirInstructions.leftState, dirInstructions.rightState, dirInstructions.upState,
                    dirInstructions.downState, dirInstructions.controllerState, dirInstructions.axisState));
        }

        private void SetBitmap(Bitmap image)
        {
            if (image == null) return;
            if (pBCam.InvokeRequired)
                Invoke(new Action<Bitmap>(SetBitmap), image);
            else
                pBCam.Image = image;
        }

        private void CranecontrolForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            Turnoff();
            settings = new Settings
            {
                Port = int.Parse(txPort.Text),
                Acceleration = int.Parse(txAcceleration.Text)
            };
            settings.Store();
        }

        private void ColorDirectionButtons(Instructions instructions)
        {
            pbAxis.BackgroundImage = instructions.axisState ? CraneControl.Properties.Resources.arrow_up_down : CraneControl.Properties.Resources.arrow_left_right;
            pbController.BackgroundImage = instructions.controllerState ? CraneControl.Properties.Resources.on : CraneControl.Properties.Resources.off;
            pbRight.BackColor = instructions.rightState ? Color.Green : Color.Red;
            pbDown.BackColor = instructions.downState ? Color.Green : Color.Red;
            pbLeft.BackColor = instructions.leftState ? Color.Green : Color.Red;
            pbUp.BackColor = instructions.upState ? Color.Green : Color.Red;
        }

        private void TxPort_TextChanged(object sender, EventArgs e)
        {
            
            try
            {
                tcpServer.port = int.Parse(txPort.Text);
            }
            catch (System.FormatException exception)
            {
                tcpServer.port = 54000;
                txPort.Text = "54000";
            }
        }

        private void TxAcceleration_TextChanged(object sender, EventArgs e)
        {
            try
            {
                payload.acceleration = int.Parse(txAcceleration.Text);
            }
            catch (System.FormatException exception)
            {
                payload.acceleration = 1;
                txAcceleration.Text = "1";
            }
        }


        private void EnumToComboBox<TEnum>(ComboBox comboBox)
        {
            comboBox.DataSource = Enum.GetValues(typeof(TEnum))
                .Cast<Enum>()
                .Select(value => new
                {
                    (Attribute.GetCustomAttribute(value.GetType().GetField(value.ToString()),
                        typeof(DescriptionAttribute)) as DescriptionAttribute)?.Description,
                    value
                })
                .OrderBy(item => item.value)
                .ToList();
            comboBox.DisplayMember = "Description";
            comboBox.ValueMember = "value";
        }


        private void Turnoff()
        {
            //Kinect Cleanup
            kinect?.Stop();
            kinect = null;

            cBkinectframe.Enabled = false;
            pBCam.Image = null;

            //Collect Garbage
            GC.Collect();
            GC.WaitForPendingFinalizers();
        }

        private void cBkinectframe_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (kinect == null) return;
            switch ((Frames)((sender as ComboBox).SelectedItem as dynamic).value)
            {
                case Frames.ir:
                    kinect.Frame = Frames.ir;
                    break;
                case Frames.color:
                    kinect.Frame = Frames.color;
                    break;
                case Frames.depth:
                    kinect.Frame = Frames.depth;
                    break;
                default:
                    throw new InvalidEnumArgumentException("cBkinectframe encountered an unexpected Value");
            }
        }

        private void btPause_Click(object sender, EventArgs e)
        {
            tcpServer.active = !tcpServer.active;
            btPause.Text = tcpServer.active ? "Pause" : "Start";
            btPause.BackColor = tcpServer.active ? Color.LightGray : Color.Red;
        }

        private void btServerControll_Click(object sender, EventArgs e)
        {
            tcpServer.serverOn = !tcpServer.serverOn;
            if(tcpServer.serverOn)
            { tcpServer.Start();}
            btServerControll.Text = tcpServer.serverOn ? "Server off" : "start Server";
            btServerControll.BackColor = tcpServer.serverOn ? Color.LightGray : Color.Red;
        }

        private void btSettings_Click(object sender, EventArgs e)
        {
            splitContainer1.Panel2Collapsed = !splitContainer1.Panel2Collapsed;
        }

        private void CraneControlForm_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar >= 'a' && e.KeyChar <= 'z')
            {
                if (cBkinectframe.Focused) btSettings.Focus();
                if (txPort.Focused) btSettings.Focus();
                if (txAcceleration.Focused) btSettings.Focus();
                switch (e.KeyChar)
                {
                    case 'q':
                        if (!axis) directionInstructions.axisState = !directionInstructions.axisState;
                        axis = true;
                        break;
                    case 'w':
                        directionInstructions.resetDirections();
                        directionInstructions.upState = true;
                        break;
                    case 'e':
                        if (!regler) directionInstructions.controllerState = !directionInstructions.controllerState;
                        regler = true;
                        break;
                    case 'a':
                        directionInstructions.resetDirections();
                        directionInstructions.leftState = true;
                        break;
                    case 's':
                        directionInstructions.resetDirections();
                        directionInstructions.downState = true;
                        break;
                        directionInstructions.resetDirections();
                        directionInstructions.downState = true;
                    case 'd':
                        directionInstructions.resetDirections();
                        directionInstructions.rightState = true;
                        break;
                    default:
                        break;
                }

                DataToPayload(directionInstructions);
                e.Handled = true;
                return;
            }
            e.Handled = false;
        }

        private void CraneControlForm_KeyUp(object sender, System.Windows.Forms.KeyEventArgs e)
        {
            axis = false;
            regler = false;
            Key_down = false;
            e.Handled = false;
        }

        private void CraneControlForm_KeyDown(object sender, System.Windows.Forms.KeyEventArgs e)
        {
            Key_down = true;
            e.Handled = false;
        }
    }
}