using Newtonsoft.Json.Linq;
using System.Diagnostics;
using System.Net;
using System.Text;

namespace AccountGeneratorDiscord
{
    public class MailWithId
    {
        public string mail = "";
        public string id = "";

        public MailWithId(string mail, string id)
        {
            this.mail = mail;
            this.id = id;
        }
    }
    public partial class Form1 : Form
    {
        public List<string> proxies = new List<string>();
        public string phoneApiKey = "";
        public string emailApiKey = "";
        public string captchaApiKey = "";


        private async Task<string> GetCaptchaTaskID(string proxySt, string siteKey)
        {

            string taskId = "";
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create("https://api.anti-captcha.com/createTask");
            request.Method = "POST";
            request.UserAgent = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/98.0.4758.102 Safari/537.36";
            request.Timeout = 5000;
            request.ReadWriteTimeout = 5000;
            ServicePointManager.DefaultConnectionLimit = 9999;
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
            if (proxySt != null)
            {
                IWebProxy proxy = WebRequest.DefaultWebProxy;
                proxy.Credentials = CredentialCache.DefaultCredentials;
                var sp = proxySt.Split(':');
                string proxyUser = sp[2].Replace(":", "").Trim();
                string proxyPass = sp[3].Replace(":", "").Trim();
                WebProxy myproxy = new WebProxy(sp[0].Replace(":", "").Trim() + ":" + sp[1].Replace(":", "").Trim(), true);
                myproxy.Credentials= new NetworkCredential(proxyUser, proxyPass);

                request.Proxy = proxy;
            }
            request.ContentType = "application/json";
            request.Accept = "application/json";

            var body = "{    \"clientKey\":\""+captchaApiKey+"\",    \"task\":        {            \"type\":\"HCaptchaTaskProxyless\",            \"websiteURL\":\"https://discord.com/\",            \"websiteKey\":\""+siteKey+"\"        }}";


            byte[] bytes = Encoding.UTF8.GetBytes(body);
            Stream stream = request.GetRequestStream();
            stream.Write(bytes, 0, bytes.Length);
            stream.Close();


            try
            {

                using (HttpWebResponse rp = (HttpWebResponse)request.GetResponse())
                {
                    var js = new StreamReader(rp.GetResponseStream()).ReadToEnd();
                    JObject o = JObject.Parse(js);

                    if (o["errorId"] != null)
                    {
                        Console.WriteLine(o["errorId"].ToString());
                        Console.WriteLine("BÖJÜÞ");
                        if (o["errorId"].ToString().Contains("0"))
                        {
                            Console.WriteLine("BÖJÜÞ");
                            if (o["taskId"] != null)
                            {
                                Console.WriteLine("BÖJÜÞ");
                                taskId =  o["taskId"].ToString();
                                Console.WriteLine("not null");
                            }
                        }

                    }

                    rp.Close();
                }


            }
            catch (Exception ex)
            {
                taskId = "";
              
            }

            return taskId;

        }

        private async Task<string> GetCaptchaCode(string proxySt, string taskId)
        {
            string key = "";
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create("https://api.anti-captcha.com/getTaskResult");
            request.Method = "POST";
            request.UserAgent = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/98.0.4758.102 Safari/537.36";
            if (proxySt != null)
            {
                IWebProxy proxy = WebRequest.DefaultWebProxy;
                proxy.Credentials = CredentialCache.DefaultCredentials;
                var sp = proxySt.Split(':');
                string proxyUser = sp[2].Replace(":", "").Trim();
                string proxyPass = sp[3].Replace(":", "").Trim();
                WebProxy myproxy = new WebProxy(sp[0].Replace(":", "").Trim() + ":" + sp[1].Replace(":", "").Trim(), true);
                myproxy.Credentials= new NetworkCredential(proxyUser, proxyPass);

                request.Proxy = proxy;
            }
            request.ContentType = "application/json";
            request.Accept = "application/json";

            var body = "{    \"clientKey\":\""+captchaApiKey+"\",    \"taskId\":"+taskId+"}";


            byte[] bytes = Encoding.UTF8.GetBytes(body);
            Stream stream = request.GetRequestStream();
            stream.Write(bytes, 0, bytes.Length);
            stream.Close();
            try
            {
                var rp = request.GetResponse();
                var js = new StreamReader(rp.GetResponseStream()).ReadToEnd();

                JObject o = JObject.Parse(js);
                if (o != null)
                {
                    if (o["errorId"] != null)
                    {
                        if (o["errorId"].Contains("0"))
                        {
                            if (o["status"] != null)
                            {
                                if (o["status"].ToString().Contains("ready"))
                                {
                                    if (o["solution"] != null)
                                    {
                                        if (o["solution"].Children().ToList().Count > 0)
                                        {
                                            key =  o["solution"].Children().ToList()[0].ToString();
                                        }

                                    }
                                }
                                else
                                {
                                    key = "NotReady";
                                }
                            }
                        }
                        else
                        {
                            key = "Error";
                        }
                    }

                }

                rp.Close();

            }
            catch (Exception ex)
            {
                key = "Error";
            }

            return key;

        }
        public Form1()
        {
            InitializeComponent();
        }

        private async Task<string> GetFingerPrint(string proxySt)
        {
            var fg = "";
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create("https://discordapp.com/api/v9/experiments");
            request.Method = "GET";
            request.UserAgent = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/98.0.4758.102 Safari/537.36";
            if (proxySt != null)
            {
                IWebProxy proxy = WebRequest.DefaultWebProxy;
                proxy.Credentials = CredentialCache.DefaultCredentials;
                var sp = proxySt.Split(':');
                WebProxy myproxy = new(sp[0].Replace(":", "").Trim() + ":" + sp[1].Replace(":", "").Trim(), true);
               
    
                if (sp.ToList().Count >= 3)
                {
                    
                    string proxyUser = sp[2].Replace(":", "").Trim();
                    string proxyPass = sp[3].Replace(":", "").Trim();
                    myproxy.Credentials= new NetworkCredential(proxyUser, proxyPass);
                    
                }


               

                request.Proxy = myproxy;

            }

            try
            {
                var rp = request.GetResponse();
                var jsSt = new StreamReader(rp.GetResponseStream()).ReadToEnd();
                JObject jo = JObject.Parse(jsSt);
                if (jo["fingerprint"] != null)
                {
                    fg = jo["fingerprint"].ToString();

                }
                else
                {
                    fg = "944336125009469490.6CteNa59lpZU792HwQ_hHKoG7VM";
                }
                rp.Close();

            }
            catch (Exception ex)
            {
                
            }
            return fg;
        }
        private async Task<MailWithId> CreateEmailTask(string proxySt)
        {
            var mailwithid = new MailWithId("", "");
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create("http://api.kopeechka.store/mailbox-get-email?site=discord.com&mail_type=MAILCOM&token=" + emailApiKey);
            request.Method = "GET";

            request.UserAgent = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/97.0.4692.99 Safari/537.36";

            if (proxySt != null)
            {
                IWebProxy proxy = WebRequest.DefaultWebProxy;
                proxy.Credentials = CredentialCache.DefaultCredentials;
                var sp = proxySt.Split(':');
                string proxyUser = sp[2].Replace(":", "").Trim();
                string proxyPass = sp[3].Replace(":", "").Trim();
                WebProxy myproxy = new WebProxy(sp[0].Replace(":", "").Trim() + ":" + sp[1].Replace(":", "").Trim(), true);
                myproxy.Credentials= new NetworkCredential(proxyUser, proxyPass);

                request.Proxy = proxy;
            }

            try
            {
                var rp = request.GetResponse();
                var js = new StreamReader(rp.GetResponseStream()).ReadToEnd();
                JObject o = JObject.Parse(js);
                if (o["status"] != null)
                {
                    if (o["status"].ToString().Contains("OK"))
                    {
                        if (o["mail"] != null)
                        {
                            mailwithid.mail =  o["mail"].ToString();
                            mailwithid.id = o["id"].ToString();

                        }
                        else
                        {

                            mailwithid = null;
                        }
                    }
                    else
                    {

                        mailwithid = null;
                    }
                }
                else
                {

                    mailwithid = null;
                }
                rp.Close();
            }
            catch (Exception ex)
            {

                mailwithid = null;
            }

            return mailwithid;
        }

        private async Task<string> GetMailToken(MailWithId mailwithid, string proxySt)
        {
            var mailtoken = "";
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create("http://api.kopeechka.store/mailbox-get-message?id="+mailwithid.id+"&token=" + emailApiKey);
            request.Method = "GET";

            request.UserAgent = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/97.0.4692.99 Safari/537.36";

          

            try
            {
                var rp = request.GetResponse();
                var js = new StreamReader(rp.GetResponseStream()).ReadToEnd();
                JObject o = JObject.Parse(js);

                if (o["status"] != null)
                {
                    if (o["value"] != null)
                    {
                        if (o["value"].ToString().Contains("WAIT_LINK"))
                        {
                            mailtoken = "Waiting";
                        }
                        else if (o["value"].ToString().Contains("https"))
                        {
                            var maillink = o["value"].ToString();

                            HttpWebRequest request2 = (HttpWebRequest)WebRequest.Create(maillink);
                            request2.Referer = "https://www.discord.com/"; // optional
                            request2.UserAgent =
                                "Mozilla/4.0 (compatible; MSIE 8.0; Windows NT 6.0; WOW64; " +
                                "Trident/4.0; SLCC1; .NET CLR 2.0.50727; Media Center PC 5.0; " +
                                ".NET CLR 3.5.21022; .NET CLR 3.5.30729; .NET CLR 3.0.30618; " +
                                "InfoPath.2; OfficeLiveConnector.1.3; OfficeLivePatch.0.0)";


                            if (proxySt != null)
                            {

                                IWebProxy proxy = WebRequest.DefaultWebProxy;
                                proxy.Credentials = CredentialCache.DefaultCredentials;
                                var sp = proxySt.Split(':');
                                WebProxy myproxy = new(sp[0].Replace(":", "").Trim() + ":" + sp[1].Replace(":", "").Trim(), false);
                             
                                if (sp.ToList().Count >= 3)
                                {

                                    string proxyUser = sp[2].Replace(":", "").Trim();
                                    string proxyPass = sp[3].Replace(":", "").Trim();
                                    myproxy.Credentials= new NetworkCredential(proxyUser, proxyPass);

                                }




                                request.Proxy = myproxy;
                            }

                            try
                            {

                                var rp2 = request2.GetResponse();

                                var responseurl = rp2.ResponseUri.AbsoluteUri;

                                if (responseurl != null)
                                {

                                    if (responseurl.Contains("token="))
                                    {
                                        mailtoken = responseurl.Substring(responseurl.IndexOf("token=") + "token=".Length);

                                    }
                                    else
                                    {
                                        mailtoken = "Error";
                                    }
                                }
                                else
                                {
                                    mailtoken = "Error";
                                }

                                rp2.Close();
                            }
                            catch (Exception ex)
                            {
                                mailtoken = "Error";

                            }
                        }
                    }

                }
                else
                {
                    mailtoken = "Error";
                }
                rp.Close();
            }
            catch (Exception ex)
            {

                mailtoken = "Error";

            }

            return mailtoken;
        }
        private async Task Register(string proxySt)
        {



            int registerCaptchaGetIdTryCount = 0;
            int emailCaptchaGetIdTryCount = 0;
            var RegisterCaptchaID = await TwoCaptchaGetCaptchaID("4c672d35-0701-42b2-88c3-78380b0db560");
            var EmailVerifyCaptchaID = await TwoCaptchaGetCaptchaID("f5561ba9-8f1e-40ca-9b5b-a0b3f719ef34");

            var token = "";

            
            if (!RegisterCaptchaID.Contains("NoBalance") && !EmailVerifyCaptchaID.Contains("NoBalance"))
            {
                while (RegisterCaptchaID.Contains("Error"))
                {
                    if (registerCaptchaGetIdTryCount <= 5)
                    {
                        RegisterCaptchaID =  await TwoCaptchaGetCaptchaID("4c672d35-0701-42b2-88c3-78380b0db560");
                    }
                    else
                    {
                        EmailVerifyCaptchaID = "Error";
                      
                        break;
                    }


                }
              
                if (!RegisterCaptchaID.Contains("Error"))
                {
                    while (EmailVerifyCaptchaID.Contains("Error"))
                    {
                        if (emailCaptchaGetIdTryCount <= 5)
                        {
                            emailCaptchaGetIdTryCount++;
                            EmailVerifyCaptchaID =  await TwoCaptchaGetCaptchaID("f5561ba9-8f1e-40ca-9b5b-a0b3f719ef34");
                        }
                        else
                        {
                            EmailVerifyCaptchaID = "Error";
                            break;
                        }


                    }
                    if (!EmailVerifyCaptchaID.Contains("Error"))
                    {



                      
                        var fg = GetFingerPrint(proxySt).Result;
                      

                        var mailwithid = await CreateEmailTask(null);
                        int trycountMail = 0;
                        while (mailwithid == null)
                        {
                            mailwithid= await CreateEmailTask(null);

                            trycountMail++;
                            if (trycountMail >= 10)
                            {
                                break;
                            }


                        }



                        string captchaCodeRegister = "NotReady";
                        string captchaCodeEmailVerify = "NotReady";
                        for (int i = 0; i > -1; i++)
                        {


                          

                            captchaCodeRegister =  await TwoCaptchaGetCapthcaKey(RegisterCaptchaID);
                            if (!captchaCodeRegister.Contains("NotReady"))
                            {

                                if (!captchaCodeRegister.Contains("NoBalance"))
                                {

                                    if (!captchaCodeRegister.Contains("Error"))
                                    {
                                        break;
                                        captchaCodeRegister = null;
                                    }
                                    else
                                    {
                                        break;
                                        captchaCodeRegister = null;
                                    }
                                }

                            }
                        }
                        while (captchaCodeRegister.Contains("NotReady"))
                        {





                        }




                        if (captchaCodeRegister != null)
                        {
                            if (mailwithid != null)
                            {
                                HttpWebRequest request = (HttpWebRequest)WebRequest.Create("https://discord.com/api/v9/auth/register");
                                request.Method = "POST";
                                request.Headers.Add("x-super-properties", "eyJvcyI6IldpbmRvd3MiLCJicm93c2VyIjoiQ2hyb21lIiwiZGV2aWNlIjoiIiwic3lzdGVtX2xvY2FsZSI6InRyLVRSIiwiYnJvd3Nlcl91c2VyX2FnZW50IjoiTW96aWxsYS81LjAgKFdpbmRvd3MgTlQgMTAuMDsgV2luNjQ7IHg2NCkgQXBwbGVXZWJLaXQvNTM3LjM2IChLSFRNTCwgbGlrZSBHZWNrbykgQ2hyb21lLzk3LjAuNDY5Mi45OSBTYWZhcmkvNTM3LjM2IiwiYnJvd3Nlcl92ZXJzaW9uIjoiOTcuMC40NjkyLjk5Iiwib3NfdmVyc2lvbiI6IjEwIiwicmVmZXJyZXIiOiJodHRwczovL3d3dy5nb29nbGUuY29tLyIsInJlZmVycmluZ19kb21haW4iOiJ3d3cuZ29vZ2xlLmNvbSIsInNlYXJjaF9lbmdpbmUiOiJnb29nbGUiLCJyZWZlcnJlcl9jdXJyZW50IjoiIiwicmVmZXJyaW5nX2RvbWFpbl9jdXJyZW50IjoiIiwicmVsZWFzZV9jaGFubmVsIjoic3RhYmxlIiwiY2xpZW50X2J1aWxkX251bWJlciI6MTEyODI0LCJjbGllbnRfZXZlbnRfc291cmNlIjpudWxsfQ==");
                                request.UserAgent = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/97.0.4692.99 Safari/537.36";
                                request.Headers.Add("x-fingerprint", fg);
                                request.ContentType = "application/json";
                                request.Headers.Add("cookie", "__dcfduid=155bcc10902511ecb6320b69e488e3cb; __sdcfduid=155bcc11902511ecb6320b69e488e3cbeae4393a20e93857b4c916b4e5b25cbffb5ca36df8b745906b6eb6023bf1c544; __cf_bm=HRE9Q1ohdfVUcC31DbbgRQM1wITorXWZBrQRDf6_xQc-1645124949-0-Aa3F/T3I4ao8qbyL4gY3aqdCTnBwTUubvbPVcrBUg0FbmqkNyiyIgyygWbTYRjpjxPTfsC/Xh5IcvjxZj62Dmeu9I8FuvKL+jBPgi5xF+t8sQ5zQ1a+p3yNPAQiaImJ92g==; locale=tr; OptanonConsent=isIABGlobal=false&datestamp=Thu+Feb+17+2022+22%3A09%3A11+GMT%2B0300+(GMT%2B03%3A00)&version=6.17.0&hosts=&landingPath=https%3A%2F%2Fdiscord.com%2F&groups=C0001%3A1%2CC0002%3A1%2CC0003%3A1; _fbp=fb.1.1645124957311.146204447");
                                if (proxySt != null)
                                {
                                    IWebProxy proxy = WebRequest.DefaultWebProxy;
                                    proxy.Credentials = CredentialCache.DefaultCredentials;
                                    var sp = proxySt.Split(':');
                                    WebProxy myproxy = new(sp[0].Replace(":", "").Trim() + ":" + sp[1].Replace(":", "").Trim(), false);
                                
                                    if (sp.ToList().Count >= 3)
                                    {

                                        string proxyUser = sp[2].Replace(":", "").Trim();
                                        string proxyPass = sp[3].Replace(":", "").Trim();
                                        myproxy.Credentials= new NetworkCredential(proxyUser, proxyPass);

                                    }




                                    request.Proxy = myproxy;




                              
                                }

                                var body = "{    \"fingerprint\": \""+fg+"\",    \"email\": \""+mailwithid.mail+"\",    \"username\": \"I'm not a Robot lol\",    \"password\": \"passwordbro123\",    \"invite\": null,    \"consent\": true,    \"date_of_birth\": \"1999-11-11\",    \"gift_code_sku_id\": null,    \"captcha_key\":\""+captchaCodeRegister+"\"}";


                                byte[] bytes = Encoding.UTF8.GetBytes(body);
                                Stream stream = request.GetRequestStream();
                                stream.Write(bytes, 0, bytes.Length);
                                stream.Close();

                                try
                                {
                                    var rp = request.GetResponse();
                                    var js = new StreamReader(rp.GetResponseStream()).ReadToEnd();
                                    JObject o = JObject.Parse(js);

                                    if (o["token"] != null)
                                    {
                                        token = o["token"].ToString();

                                    }
                                    Console.WriteLine(token);
                                    Console.WriteLine(mailwithid.mail);
                                    rp.Close();
                                }
                                catch (Exception ex)
                                {

                                    token = null;
                                }


                            }


                        }






                        var mailtoken = "Waiting";


                        int MailCaptchaSolveErrorCount = 0;
                        int MailCaptchaIDErrorCount = 0;
                        if (token != null)
                        {

                            mailtoken =  await GetMailToken(mailwithid, proxySt);
                            if (!mailtoken.Contains("Error"))
                            {

                                while (mailtoken.Contains("Waiting"))
                                {

                                    mailtoken =  await GetMailToken(mailwithid, proxySt);




                                }


                            }


                        }



                        if (mailtoken != null)
                        {

                            while (captchaCodeEmailVerify.Contains("Ready") || captchaCodeEmailVerify.Contains("Error"))
                            {

                                if (MailCaptchaSolveErrorCount  <= 5)
                                {
                                 
                                    captchaCodeEmailVerify =   await TwoCaptchaGetCapthcaKey(EmailVerifyCaptchaID);

                                    if (captchaCodeEmailVerify.Contains("Error"))
                                    {
                                        MailCaptchaSolveErrorCount++;
                                        EmailVerifyCaptchaID = await TwoCaptchaGetCaptchaID("f5561ba9-8f1e-40ca-9b5b-a0b3f719ef34");

                                        captchaCodeEmailVerify =  await TwoCaptchaGetCapthcaKey(EmailVerifyCaptchaID);
                                        if (EmailVerifyCaptchaID.Contains("No Balance"))
                                        {
                                            captchaCodeEmailVerify = "No Balance";
                                            break;
                                        }

                                        while (EmailVerifyCaptchaID.Contains("Error"))
                                        {
                                            if (MailCaptchaIDErrorCount >= 5)
                                            {
                                                MailCaptchaIDErrorCount++;
                                                EmailVerifyCaptchaID = await TwoCaptchaGetCaptchaID("f5561ba9-8f1e-40ca-9b5b-a0b3f719ef34");
                                            }
                                            else
                                            {
                                                captchaCodeEmailVerify = "Error";
                                                break;
                                            }

                                        }
                                    }

                                }
                                else
                                {
                                    captchaCodeEmailVerify = "Error";

                                    break;
                                }




                            }

                            if (!captchaCodeEmailVerify.Contains("No Balance"))
                            {

                                if (!captchaCodeEmailVerify.Contains("Error"))
                                {



                                    HttpWebRequest request = (HttpWebRequest)WebRequest.Create("https://discord.com/api/v9/auth/verify");
                                    request.Method = "POST";
                                    request.Headers.Add("x-super-properties", "eyJvcyI6IldpbmRvd3MiLCJicm93c2VyIjoiQ2hyb21lIiwiZGV2aWNlIjoiIiwic3lzdGVtX2xvY2FsZSI6InRyLVRSIiwiYnJvd3Nlcl91c2VyX2FnZW50IjoiTW96aWxsYS81LjAgKFdpbmRvd3MgTlQgMTAuMDsgV2luNjQ7IHg2NCkgQXBwbGVXZWJLaXQvNTM3LjM2IChLSFRNTCwgbGlrZSBHZWNrbykgQ2hyb21lLzk4LjAuNDc1OC4xMDIgU2FmYXJpLzUzNy4zNiIsImJyb3dzZXJfdmVyc2lvbiI6Ijk4LjAuNDc1OC4xMDIiLCJvc192ZXJzaW9uIjoiMTAiLCJyZWZlcnJlciI6IiIsInJlZmVycmluZ19kb21haW4iOiIiLCJyZWZlcnJlcl9jdXJyZW50IjoiIiwicmVmZXJyaW5nX2RvbWFpbl9jdXJyZW50IjoiIiwicmVsZWFzZV9jaGFubmVsIjoic3RhYmxlIiwiY2xpZW50X2J1aWxkX251bWJlciI6MTE1NDI3LCJjbGllbnRfZXZlbnRfc291cmNlIjpudWxsfQ");
                                    request.UserAgent = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/97.0.4692.99 Safari/537.36";
                                    request.Headers.Add("x-fingerprint", fg);
                                    request.ContentType = "application/json";
                                    request.Headers.Add("cookie", "__dcfduid=0214d34088ff11ecbefb770fdfec52a9; __sdcfduid=0214d34188ff11ecbefb770fdfec52a996964547316afd72d42cd8897bb667ee6582bcab026f769a38119512a9fe1cd8; _fbp=fb.1.1645140210880.1708761996; __stripe_mid=5a03934a-5b69-4b1a-b1b7-98cff869302455d836; locale=en-US; OptanonConsent=isIABGlobal=false&datestamp=Fri+Feb+18+2022+19%3A29%3A08+GMT%2B0300+(GMT%2B03%3A00)&version=6.17.0&hosts=&landingPath=NotLandingPage&groups=C0001%3A1%2CC0002%3A1%2CC0003%3A1&AwaitingReconsent=false; __cf_bm=WlRGX.Kf6wJLGaD1W07eyoEhOjXM4OsKuh5bPO0SPCU-1645207998-0-AYUG1cUnBxmGrdvdFWWDleA8XRtCzHeiUYFukQZB/fVLfHh65o8RT8F0vhzz23vVrZJXlteuGYwByd1wkBuAjOXbN68BhdISp0+ZP2eUzZjSdFU93x1v6swW+Tx6yc+SzA==");
                                    if (proxySt != null)
                                    {
                                        IWebProxy proxy = WebRequest.DefaultWebProxy;
                                        proxy.Credentials = CredentialCache.DefaultCredentials;
                                        var sp = proxySt.Split(':');
                                        WebProxy myproxy = new(sp[0].Replace(":", "").Trim() + ":" + sp[1].Replace(":", "").Trim(), false);
                                    
                                        if (sp.ToList().Count >= 3)
                                        {

                                            string proxyUser = sp[2].Replace(":", "").Trim();
                                            string proxyPass = sp[3].Replace(":", "").Trim();
                                            myproxy.Credentials= new NetworkCredential(proxyUser, proxyPass);

                                        }




                                        request.Proxy = myproxy;

                                    }

                                    var body = "{    \"token\": \""+mailtoken+"\",    \"captcha_key\": \""+captchaCodeEmailVerify+"\"}";



                                    byte[] bytes = Encoding.UTF8.GetBytes(body);
                                    Stream stream = request.GetRequestStream();
                                    stream.Write(bytes, 0, bytes.Length);
                                    stream.Close();
                                    request.Timeout = 30000;

                                    try
                                    {

                                        var response = request.GetResponse();










                                    }
                                    catch (Exception ex)
                                    {



                                    }
                                }

                            }

                        }
                    }
                }

            }



         


            Console.WriteLine(token);
          
        }
        private void BlaBla(string proxySt)
        {
            Register(proxySt);

            Console.WriteLine("joe");
        }
        private async Task<string> TwoCaptchaGetCaptchaID(string sitekey)
        {
            string id = "";
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create("http://2captcha.com/in.php?key=b9d54eedc012d87e816a77617326b4cd&method=hcaptcha&sitekey="+sitekey+"&pageurl=https://discord.com");
            request.Method = "GET";
            try
            {
                using (var response = request.GetResponse())
                {
                    using (var stream = response.GetResponseStream())
                    {
                        var jsrp = new StreamReader(stream).ReadToEnd();


                        if (jsrp.Contains("OK|"))
                        {
                            id = jsrp.Replace("OK|", "").Trim();

                        }
                        else if (jsrp.Contains("BALANCE"))
                        {
                            id = "No Balance";
                        }
                        else
                        {
                            id = "Error";
                        }
                    }
                }
            }
            catch (Exception ex)
            {

                id = "Error";
            }

            return id;
        }

        private async Task<string> TwoCaptchaGetCapthcaKey(string id)
        {


            string key = "";
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create("http://2captcha.com/res.php?key=b9d54eedc012d87e816a77617326b4cd&action=get&id=" + id);
            request.Method = "GET";
            try
            {
                using (var response = request.GetResponse())
                {
                    using (var stream = response.GetResponseStream())
                    {
                        var jsrp = new StreamReader(stream).ReadToEnd();


                        if (jsrp.Contains("OK|"))
                        {
                            key = jsrp.Replace("OK|", "").Trim();

                        }
                        else if (jsrp.Contains("CAPCHA_NOT_READY"))
                        {
                            key = "NotReady";
                        }
                        else if (jsrp.Contains("BALANCE"))
                        {
                            key = "NoBalance";
                        }
                        else if (jsrp.Contains("SOLVE"))
                        {
                            key = "Error";
                        }
                    }
                }
            }
            catch (Exception ex)
            {

                key = "NotReady";
            }


            return key;
        }
        private async void StartRegisterButton_clickAsync(object sender, EventArgs e)
        {



            System.Net.ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
            System.Net.ServicePointManager.DefaultConnectionLimit = 99999;
            System.Net.ServicePointManager.MaxServicePoints = 9999;
            proxies = File.ReadAllLines(Environment.CurrentDirectory + @"\proxies.txt").ToList();
            Console.WriteLine(proxies.Count);
            Console.WriteLine(System.Net.ServicePointManager.DefaultConnectionLimit);
            
           for(int i = 0; i < 5; i++)
            {
                Console.WriteLine("blabla");
                Register(proxies[i]);
                
             
            }

          


        }
    }
}