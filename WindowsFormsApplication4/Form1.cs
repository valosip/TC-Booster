using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Net;
using System.Web.UI;
using System.Web.Script.Serialization;
using System.IO;
using Newtonsoft.Json;
using System.Threading;

namespace WindowsFormsApplication4
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            useridBox.Visible = false;
            cookieBox.Visible = false;
            userNameLabel.Visible = true;
            label2.Visible = true;
            questionsRatedLabel.Visible = true;
            int count = 0;
            int countlimit = 1000;
            GetUsername();
                while (count <= countlimit)
                {

                        GetAchievements();
                        RateQuestion();
                        Thread.Sleep(500);
                }
        }
        
        private void VisitLink()
        {
            linkLabel1.LinkVisited = true;
            System.Diagnostics.Process.Start("http://www.google.com");
        }
        public class GetQuestion {
            public int id { get; set; }
            public string category { get; set; }
            public string text { get; set; }
            public List<string> answers { get; set; }
            public int correct_answer { get; set; }
            public string media_type { get; set; }
        }
        public delegate void SetTextDelegate(System.Windows.Forms.Control ctrl, string text);
        public static void SetText2(System.Windows.Forms.Control ctrl, string text)
        {

            if (ctrl.InvokeRequired)
            {
                object[] params_list = new object[] { ctrl, text };
                ctrl.Invoke(new SetTextDelegate(SetText2), params_list);
            }
            else
            {
                ctrl.Text = text;
            }
            Application.DoEvents();
        }
        public class GetAchieve
        {
            public int id { get; set; }
            public string status { get; set; }
            public int value { get; set; }
            public int counter { get; set; }
            public int percent { get; set; }
            public bool disabled { get; set; }
            public string reward_type { get; set; }
            public int rewards { get; set; }
        }
        public class Podium
        {
            public int first_place { get; set; }
            public int second_place { get; set; }
            public int third_place { get; set; }
        }

        public class Rankings
        {
            public Podium podium { get; set; }
        }

        public class Challenges
        {
            public int won { get; set; }
            public int lost { get; set; }
        }

        public class CategoryQuestion
        {
            public string category { get; set; }
            public int correct { get; set; }
            public int incorrect { get; set; }
            public bool worst { get; set; }
        }

        public class LanguageQuestion
        {
            public string language { get; set; }
            public int correct { get; set; }
            public int incorrect { get; set; }
        }

        public class Statistics
        {
            public Rankings rankings { get; set; }
            public Challenges challenges { get; set; }
            public IList<CategoryQuestion> category_questions { get; set; }
            public IList<LanguageQuestion> language_questions { get; set; }
            public int games_won { get; set; }
            public int games_lost { get; set; }
            public int games_resigned { get; set; }
            public int duel_games_won { get; set; }
            public int duel_games_lost { get; set; }
            public int consecutive_games_won { get; set; }
            public int consecutive_answers_correct { get; set; }
        }

        public class LevelData
        {
            public int level { get; set; }
            public int points { get; set; }
            public int progress { get; set; }
            public int goal_points { get; set; }
            public bool level_up { get; set; }
        }

        public class Profile
        {
            public bool is_friend { get; set; }
            public string phone { get; set; }
            public string zip_code { get; set; }
            public IList<object> buddiesFacebookId { get; set; }
            public Statistics statistics { get; set; }
            public int id { get; set; }
            public bool is_blocked { get; set; }
            public bool allow_og_posts { get; set; }
            public string username { get; set; }
            public int friends { get; set; }
            public string description { get; set; }
            public int blocked_users_count { get; set; }
            public LevelData level_data { get; set; }
            public bool has_pass { get; set; }
            public bool is_app_user { get; set; }
            public IList<object> games_by_language { get; set; }
            public string online_status { get; set; }
            public bool fb_show_name { get; set; }
            public string last_play_date { get; set; }
            public string photo_url { get; set; }
            public string last_log { get; set; }
            public bool fb_show_picture { get; set; }
            public string twitter_name { get; set; }
            public string country { get; set; }
            public string email { get; set; }
        }
        public void GetUsername()
        {
            string userid = useridBox.Text;
            string cookie = cookieBox.Text;
            var httpWebRequest = (HttpWebRequest)WebRequest.Create("http://api.preguntados.com/api/users/" + userid + "/profiles/"+userid);
            httpWebRequest.ContentType = "application/json";
            httpWebRequest.Accept = "*/*";
            httpWebRequest.Method = "GET";
            httpWebRequest.UserAgent = "Preguntados/1.9.2 (iPhone; iOS 8.1.2; Scale/2.00)";
            httpWebRequest.Headers.Add("Accept-Language", "en-us");
            httpWebRequest.Headers.Add("Accept-Encoding", "gzip, deflate");
            httpWebRequest.Headers.Add("Eter-Agent", "1|iOS-AppStr|iPhone6,1|0|iOS 8.1.2|0|1.9.2|fr|fr|CA|1");
            httpWebRequest.CookieContainer = new CookieContainer();
            httpWebRequest.CookieContainer.Add(new Uri("http://api.preguntados.com/api/users/" + userid + "/profiles/" + userid),
                new Cookie("ap_session", cookie));

            var httpResponse = (HttpWebResponse)httpWebRequest.GetResponse();

            using (var reader = new StreamReader(httpResponse.GetResponseStream()))
            {
                JavaScriptSerializer js = new JavaScriptSerializer();
                var objText = reader.ReadToEnd();


                Profile result = JsonConvert.DeserializeObject<Profile>(objText);
                //var items = JsonConvert.DeserializeObject<List<GetAchieve>>(objText);

                string username = result.username;
               // MessageBox.Show(username);
                userNameLabel.Text = username;
                SetText2(userNameLabel, username);
                
                //MessageBox.Show(QuestionCount.ToString());
            }
        }
        public void GetAchievements()
        {
            string userid = useridBox.Text;
            string cookie = cookieBox.Text;
            var httpWebRequest = (HttpWebRequest)WebRequest.Create("http://api.preguntados.com/api/users/" + userid + "/achievements");
            httpWebRequest.ContentType = "application/json";
            httpWebRequest.Accept = "*/*";
            httpWebRequest.Method = "GET";
            httpWebRequest.UserAgent = "Preguntados/1.9.2 (iPhone; iOS 8.1.2; Scale/2.00)";
            httpWebRequest.Headers.Add("Accept-Language", "en-us");
            httpWebRequest.Headers.Add("Accept-Encoding", "gzip, deflate");
            httpWebRequest.Headers.Add("Eter-Agent", "1|iOS-AppStr|iPhone6,1|0|iOS 8.1.2|0|1.9.2|fr|fr|CA|1");
            httpWebRequest.CookieContainer = new CookieContainer();
            httpWebRequest.CookieContainer.Add(new Uri("http://api.preguntados.com/api/users/" + userid + "/achievements"),
                new Cookie("ap_session", cookie));
            
            var httpResponse = (HttpWebResponse)httpWebRequest.GetResponse();

            using (var reader = new StreamReader(httpResponse.GetResponseStream()))
            {
                JavaScriptSerializer js = new JavaScriptSerializer();
                var objText = reader.ReadToEnd();


                GetAchieve[] result = JsonConvert.DeserializeObject<GetAchieve[]>(objText);
                var items = JsonConvert.DeserializeObject<List<GetAchieve>>(objText);
                

                int QuestionCount = result[45].counter;
                questionsRatedLabel.Text = QuestionCount.ToString();
                SetText2(questionsRatedLabel, QuestionCount.ToString());
                if (QuestionCount >= 1000)
                {
                    MessageBox.Show("You've successfully rated 1000 questions,\n                            EXITING");
                    Environment.Exit(0); 
                }
                
                //MessageBox.Show(QuestionCount.ToString());
            }
        }

        public void RateQuestion()
        {
            string userid = useridBox.Text;
            string cookie = cookieBox.Text;
            var httpWebRequest = (HttpWebRequest)WebRequest.Create("http://api.preguntados.com/api/users/" + userid + "/question-rating?country=GX&lang=EN");
            httpWebRequest.ContentType = "application/json";
            httpWebRequest.Accept = "*/*";
            httpWebRequest.Method = "GET";
            httpWebRequest.UserAgent = "Preguntados/1.9.2 (iPhone; iOS 8.1.2; Scale/2.00)";
            httpWebRequest.Headers.Add("Accept-Language", "en-us");
            httpWebRequest.Headers.Add("Accept-Encoding", "gzip, deflate");
            httpWebRequest.Headers.Add("Eter-Agent", "1|iOS-AppStr|iPhone6,1|0|iOS 8.1.2|0|1.9.2|fr|fr|CA|1");
            httpWebRequest.CookieContainer = new CookieContainer();
            httpWebRequest.CookieContainer.Add(new Uri("http://api.preguntados.com/api/users/" + userid + "/question-rating?country=GX&lang=EN"),
                new Cookie("ap_session", cookie));
            var httpResponse = (HttpWebResponse)httpWebRequest.GetResponse();

            using (var reader = new StreamReader(httpResponse.GetResponseStream()))
            {
                JavaScriptSerializer js = new JavaScriptSerializer();
                var objText = reader.ReadToEnd();
                GetQuestion myojb = (GetQuestion)js.Deserialize(objText, typeof(GetQuestion));
                string answer = "{\"id\":\"" + myojb.id + "\",\"language\":\"EN\",\"vote\":\"POSITIVE\"}";
                // MessageBox.Show(answer);
                sendAnswer(answer, userid);
            }
        }

        public void sendAnswer(string answer, string userid)
        {
            string cookie = cookieBox.Text;
            string post_data = answer;
            var httpWebRequest = (HttpWebRequest)WebRequest.Create("http://api.preguntados.com/api/users/" + userid + "/question-rating?country=GX&lang=EN");
            //httpWebRequest.ContentType = "application/json";
            //httpWebRequest.Accept = "*/*";
            httpWebRequest.Method = "POST";
            httpWebRequest.UserAgent = "Preguntados/1.9.2 (iPhone; iOS 8.1.2; Scale/2.00)";
            httpWebRequest.ContentType = "application/json; charset=utf-8";
            httpWebRequest.Headers.Add("Accept-Language", "en-us");
            httpWebRequest.Headers.Add("Accept-Encoding", "gzip, deflate");
            httpWebRequest.Headers.Add("Eter-Agent", "1|iOS-AppStr|iPhone6,1|0|iOS 8.1.2|0|1.9.2|fr|fr|CA|1");
            httpWebRequest.CookieContainer = new CookieContainer();
            httpWebRequest.CookieContainer.Add(new Uri("http://api.preguntados.com/api/users/" + userid + "/question-rating?country=GX&lang=EN"),
                new Cookie("ap_session", cookie));
            byte[] postBytes = Encoding.ASCII.GetBytes(post_data);
            httpWebRequest.ContentLength = postBytes.Length;

            Stream requestStream = httpWebRequest.GetRequestStream();
            requestStream.Write(postBytes, 0, postBytes.Length);
            requestStream.Close();
            var httpResponse = (HttpWebResponse)httpWebRequest.GetResponse();
            

        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            try
            {
                VisitLink();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Unable to open link that was clicked.");
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Environment.Exit(0);
        }
        
    }
}
