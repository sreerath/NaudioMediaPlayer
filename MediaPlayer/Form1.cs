using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using NAudio.Wave.SampleProviders;
using NAudio.Wave;
using System.IO;
using NAudio;
using NAudio.CoreAudioApi;





namespace MediaPlayer
{
    public partial class Forward : Form
    {
        TrackBar tbrSlider;

        public Forward()
        {
            InitializeComponent();
        }
   

        public string str;
        OpenFileDialog open = new OpenFileDialog();
        private NAudio.Wave.WaveFileReader wave = null;
       
       public  WaveIn wi;
       public WaveOut wo;
       public AudioFileReader aud;
    
       public static int myrec = 0;
       public static int count = 0;
       long[] myArray1=new long[20];
       string[] myArray2=new string[8];


        public void Browse_Click(object sender, EventArgs e)
        {
            
            open.Filter = "Wave or mp3 files(*.wav or *.mp3)|*.mp3";
            
            if (open.ShowDialog() == DialogResult.OK)          
               FileName.Text = open.FileName;
               
        }


        private void Play_Click(object sender, EventArgs e)
        {
            
            Disposewave();
            wo = new WaveOut();
            aud = new AudioFileReader(open.FileName);
            wo.Init(aud);
            wo.Play();

            string duration = aud.TotalTime.ToString("mm\\:ss");
            trackBarcontrol(0);                  
            label1.Text = duration;

        }

        private void Pause_Click(object sender, EventArgs e)
        {
            if(wo !=null)
                if (wo.PlaybackState == NAudio.Wave.PlaybackState.Playing)
                {
                    
                    wo.Pause();
                    myrec++;

                    long newPos = aud.Position;
                    
                    string curmin = aud.CurrentTime.Minutes.ToString();
                    string cursec = aud.CurrentTime.Seconds.ToString();

                    label3.Text = curmin + ":" + cursec;

                    listBox1.Items.Add(label3.Text);
                    myArray2[myrec] = curmin + ":" + cursec;
                    myArray1[myrec] = newPos;
                                   
                    wo.Play();
                }

          }
        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            int i = 0;

            foreach (string str in myArray2)
            {
                i++;

                if ((string)listBox1.SelectedItem == str)
                {
                    aud.Position = myArray1[i];
                                    
                    
                    wo.Init(aud);
                    wo.Play();
                    trackBarcontrol(0);
                    break;
                }
            }
        }
        


        private void Disposewave()
        {
            if(wo!=null)
            {
                if(wo.PlaybackState==NAudio.Wave.PlaybackState.Playing) wo.Stop();
                wo.Dispose();
                     
            }

       }
        private void button2_Click(object sender, EventArgs e)
        {
            if (wo.PlaybackState == NAudio.Wave.PlaybackState.Playing) wo.Stop();
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            Disposewave();

        }
        
        private void button1_Click(object sender, EventArgs e)
        {
          

         /* long newPos = aud.Position + (long)(aud.WaveFormat.AverageBytesPerSecond *30);

            if (newPos > aud.Length)
                aud.Position = aud.Length;
            else if (newPos < 0)
                aud.Position = 0;
            else
               aud.Position = newPos;*/
                   
                      
            aud.Skip(+30);
            wo.Init(aud);
            
            wo.Play();
            trackBarcontrol(+30);
            
        }

        private void Rewind_Click(object sender, EventArgs e)
        {
            //aud.CurrentTime=aud.CurrentTime.Subtract(new TimeSpan(0,0,0,30));
            aud.Skip(-30);
            wo.Init(aud);
            wo.Play();
            
            trackBarcontrol(-30);
        }
   
      

        /*private void trackBar1_Scroll(object sender, EventArgs e)
        {

            trackBar1.Value = 60 * (int)aud.TotalTime.TotalMinutes;

        }*/

        private void trackBarcontrol(int val)
        {
            trackBar1.Minimum = 0;
            trackBar1.Maximum = (int)aud.TotalTime.TotalSeconds;
            timer2.Start();
            
            if(val==0)
            {
                timer2.Enabled = true;
               
                trackBar1.Value = 0;
            }
                           
            trackBar1.Value = 0;
            if (val == +30)
                trackBar1.Value = trackBar1.Value +30;

            if (val == -30 )
            {
                if (trackBar1.Value<=30)
                     trackBar1.Value=30;
                else
                    trackBar1.Value = trackBar1.Value - 30;
            }
          
            
            
            
          
       }

        private void timer2_Tick(object sender, EventArgs e)
        {
            string curmin = aud.CurrentTime.Minutes.ToString();
            string cursec = aud.CurrentTime.Seconds.ToString();
            label3.Text = aud.CurrentTime.ToString("mm\\:ss");
            
            if(count==1)
                trackBar1.Value = (int)aud.CurrentTime.Seconds+59; 
            else 
                trackBar1.Value =  (int)aud.CurrentTime.Seconds;

            if ((int)aud.CurrentTime.Seconds == 59)
                
                count = count + 1;
            
               // trackBar1.Value = trackBar1.Value + 1;
                 
        }

        private void trackBar1_Scroll(object sender, EventArgs e)
        {

            if (wo.PlaybackState == NAudio.Wave.PlaybackState.Playing)
                trackBar1.Value = trackBar1.Value + 1;

        }

      
        
       

  
       
    }
   
}
