using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using System.Net;
using System.Net.Mail;

namespace WindowsFormsApplication1
{
    public partial class Form1 : Form
    {
        public static string smptServer; // сервер
        public static string from; // от кого
        public static string password; // пароль
        public static string [] to = new string[25]; // кому
        public static int kolvo_to = 0; // количество получателей
        public static string caption; // тема
        public static string message; // смс
        public static string attachFile = null; // файл с смс

        void SendMail(string smtpServer, string from, string password, 
                      int kolvo_to, string [] to, 
                      string caption, string message, string [] attachFiles,
                      bool isHTML, string[] CCs, string[] BccS   )
        {
            for (int i = 0; i < kolvo_to; i++)
            {
                MailMessage mail = new MailMessage
                    (from, to[i], caption, message);

                mail.From = new MailAddress(from);
                mail.To.Add(new MailAddress(to[i]));

                mail.BodyEncoding = Encoding.UTF8;
                mail.SubjectEncoding = Encoding.UTF8;
                mail.Body = message;
                mail.Subject = caption;

                if (!string.IsNullOrEmpty(attachFiles[0]))
                    foreach (string attachFile in attachFiles)
                        mail.Attachments.Add(new Attachment(attachFile));
                mail.IsBodyHtml = isHTML;
                if (CCs[0] != "")
                    foreach (string CC in CCs)
                        mail.CC.Add(CC);
                if (BccS[0] != "")
                    foreach (string Bcc in BccS)
                        mail.Bcc.Add(Bcc);

                SmtpClient client = new SmtpClient ();
                client.Host = smtpServer;
                client.Port = 25;
                client.EnableSsl = true; // шифрование трафика
                client.Credentials = new NetworkCredential
                    (from, password);
                client.DeliveryMethod = SmtpDeliveryMethod.Network;
                client.Send(mail);
                mail.Dispose();
            }
        }
        public Form1()
        {
            InitializeComponent();
            textBox1.Text = "example@bk.ru";
            textBox2.Text = "example@bk.ru";
        }

        private void button3_Click(object sender, EventArgs e)
        {
            kolvo_to = 0;
            for (int i = 0; i < 25; i++) to[i] = null;
            richTextBox1.Text = "";
            
        }

        private void button1_Click(object sender, EventArgs e)
        {
            to[kolvo_to] = textBox2.Text;
            textBox2.Text = "";
            richTextBox1.Text += to[kolvo_to] + "\n"; 
            kolvo_to++;

        }

        private void button2_Click(object sender, EventArgs e)
        {
            try
            {
                if (textBox1.Text != "" && kolvo_to!=0 &&
                    textBox3.Text != "" && richTextBox2.Text != "")
                {

                    from = textBox1.Text;

                    caption = textBox3.Text;
                    message = richTextBox2.Text;

                    try
                    {
                        SendMail("smtp.mail.ru", from, "password!!!!!!!!!!!!!!!!!!!!",
                        kolvo_to, to, caption, message,
                        richTextBox3.Text.Split(new string[] { " // " }, StringSplitOptions.None),
                        checkBox1.Checked, richTextBox4.Text.Split(new string[] { ", " }, StringSplitOptions.None),
                        richTextBox5.Text.Split(new string[] { ", " }, StringSplitOptions.None));
                        MessageBox.Show("Email sent successfully!");
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message);
                    }

                    
                }
                else
                    MessageBox.Show("Some of main field aren't filled!");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            textBox2.Text = "";
            textBox3.Text = "";
            richTextBox1.Text = "";
            richTextBox2.Text = "";
            kolvo_to = 0;
            for (int i = 0; i < 25; i++) to[i] = null;
            richTextBox1.Text = "";

        }

        private void button5_Click(object sender, EventArgs e)
        {
            if (openFileDialog1.ShowDialog() == DialogResult.Cancel)
                return;
            // получаем выбранный файл
            string filename = openFileDialog1.FileName;
            if (richTextBox3.Text == "")
                richTextBox3.Text += filename;
            else
                richTextBox3.Text += " // " + filename;
        }

        private void button6_Click(object sender, EventArgs e)
        {
            if (richTextBox4.Text == "")
                richTextBox4.Text += textBox4.Text;
            else
                richTextBox4.Text += ", " + textBox4.Text;
            textBox4.Text = "";
        }

        private void button7_Click(object sender, EventArgs e)
        {
            if (richTextBox5.Text == "")
                richTextBox5.Text += textBox5.Text;
            else
                richTextBox5.Text += ", " + textBox5.Text;
            textBox5.Text = "";
        }

        private void button8_Click(object sender, EventArgs e)
        {
            richTextBox3.Text = "";
        }

        private void button9_Click(object sender, EventArgs e)
        {
            richTextBox4.Text = "";
            textBox4.Text = "";
        }

        private void button10_Click(object sender, EventArgs e)
        {
            richTextBox5.Text = "";
            textBox5.Text = "";
        }
    }
}
