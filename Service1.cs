using System;
using System.ServiceProcess;
using System.IO;
using EnterpriseDT.Net.Ftp;
using EmailLib;
using System.Collections.Generic;
using System.Xml;
using System.Threading;

namespace CallExeSrv
{
    public partial class Service1 : ServiceBase
    {
        private static Dictionary<string, string> dict;
        private System.Timers.Timer timer;

        private int spanTime;

        private string logPath;
        private string logPath2;
        private string logPath3;
        private string logPath4Me;
        private string logPath4Pub;
        private string logPath34Pub;
        private string logArchive;

        private string remoteLogPath4Pub;
        private string remoteLogPath4Me;
        private string remotelogPath2;
        private string remotelogPath3;
        private string remoteBasicPath;

        private int lowTmp;
        private int highTmp;
        private int inventoryk;
        private int inventoryd;
        private int ratioTmp;

        private int percentd;
        private int percentk;

        private int lowTmpWithOpen;
        private int highTmpWithOpen;

        private static string content = "";
        private static string previousDateTime = "2013-10-22 22:27:17";

        private int addHours;

        private bool onSound;
        private string onSoundPath;
        private string offSoundPath;

        private string dropFolder;

        private int profitpts;
        private int priced;// price when dk
        private int pricek;// price when kk 
        private int currectPrice;

        private int kpTmp;
        private int dpTmp;

        private bool isCSV;

        private static int cntLost;
        private int totalLost;

        private bool is_1_2_3;
        private bool is_2_3_1;
        private bool is_3_1_2;
        private bool is_2_1_3;
        private bool is_1_3_2;
        private bool is_3_2_1;
        private bool is_3_3_1;
        private bool is987;
        private bool is9_1_2;
        private bool is98_1;

        private bool is123;
        private bool is213;
        private bool is132;
        private bool is231;
        private bool is312;
        private bool is321;
        private bool is331;
        private bool is_9_8_7;
        private bool is_912;
        private bool is_9_81;

        public Service1()
        {
            InitializeComponent();
            //LoadConfig();
        }

        private void LoadConfig()
        {
            dict = new Dictionary<string, string>();
            XmlDocument doc = new XmlDocument();
            doc.Load(@"D:\vl-docs\config.xml");
            XmlNodeList nodes = doc.GetElementsByTagName("add");
            for (int i = 0; i < nodes.Count; i++)
            {
                string keystr = nodes[i].Attributes["key"].Value.Trim();
                string valuestr = nodes[i].Attributes["value"].Value.Trim();
                dict.Add(keystr, valuestr);
            }
        }

        private static string GetValueByKey(string key)
        {
            foreach (KeyValuePair<string, string> entry in dict)
            {
                if (entry.Key.ToUpper().Trim().Equals(key.ToUpper()))
                    return entry.Value.Trim();
            }
            return "";
        }

        protected override void OnStart(string[] args)
        {
            spanTime = 10;
            if (timer == null)
                timer = new System.Timers.Timer();
            timer.Enabled = true;
            timer.Interval = spanTime * 1000;
            timer.Start();
            timer.Elapsed += new System.Timers.ElapsedEventHandler(timer_Elapsed);
        }

        private void timer_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            if (timer != null)
                timer.Stop();
            LoadConfig();
            taskBegin();
        }

        private void taskBegin()
        {
            logPath = GetValueByKey("logPath");
            logPath2 = GetValueByKey("logPath2");
            logPath3 = GetValueByKey("logPath3");
            logPath4Me = GetValueByKey("logPath4Me");
            logPath4Pub = GetValueByKey("logPath4Pub");
            logPath34Pub = GetValueByKey("logPath34Pub");
            logArchive = GetValueByKey("logArchive");

            remoteLogPath4Pub = GetValueByKey("RemoteLogPath4Pub");
            remoteLogPath4Me = GetValueByKey("RemoteLogPath4Me");
            remotelogPath2 = GetValueByKey("RemotelogPath2");
            remotelogPath3 = GetValueByKey("RemotelogPath3");
            remoteBasicPath = GetValueByKey("RemoteBasicPath");

            addHours = int.Parse(GetValueByKey("addHours"));

            onSoundPath = GetValueByKey("onSoundPath");
            offSoundPath = GetValueByKey("offSoundPath");
            dropFolder = GetValueByKey("dropFolder");

            profitpts = int.Parse(GetValueByKey("profitpts"));
            totalLost = int.Parse(GetValueByKey("totalLost"));

            //added 03/20
            is_1_2_3 = GetValueByKey("-1-2-3").Equals("1") ? true : false;
            is_2_3_1 = GetValueByKey("-2-3-1").Equals("1") ? true : false;
            is_3_1_2 = GetValueByKey("-3-1-2").Equals("1") ? true : false;
            is_2_1_3 = GetValueByKey("-2-1-3").Equals("1") ? true : false;
            is_1_3_2 = GetValueByKey("-1-3-2").Equals("1") ? true : false;
            is_3_2_1 = GetValueByKey("-3-2-1").Equals("1") ? true : false;
            is_3_3_1 = GetValueByKey("-3-3-1").Equals("1") ? true : false;
            is987 = GetValueByKey("+++").Equals("1") ? true : false;
            is9_1_2 = GetValueByKey("+--").Equals("1") ? true : false;
            is98_1 = GetValueByKey("++-").Equals("1") ? true : false;

            is123 = GetValueByKey("123").Equals("1") ? true : false;
            is213 = GetValueByKey("213").Equals("1") ? true : false;
            is132 = GetValueByKey("132").Equals("1") ? true : false;
            is231 = GetValueByKey("231").Equals("1") ? true : false;
            is312 = GetValueByKey("312").Equals("1") ? true : false;
            is321 = GetValueByKey("321").Equals("1") ? true : false;
            is331 = GetValueByKey("331").Equals("1") ? true : false;
            is_9_8_7 = GetValueByKey("---").Equals("1") ? true : false;
            is_912 = GetValueByKey("-++").Equals("1") ? true : false;
            is_9_81 = GetValueByKey("--+").Equals("1") ? true : false;

            //offAlarmSound();//added on 01/09/15
            clearInventoryManually();//added on 01/09/15
            clearInventoryLastMin(addHours);
            backupFtpCSV(addHours); //added on 02/02/15
            try
            {
                string tmpdt = lastModifiedDateTime(logPath2);
                if (!previousDateTime.Equals(tmpdt))
                {
                    previousDateTime = tmpdt;
                    Random rnd = new Random();
                    int randomN = rnd.Next(50);

                    //verify file length match
                    if (!isMatch(logPath, logPath2))
                    {
                        string[] tmp1 = FileLoad2Arr(logPath);
                        int tmp1len = tmp1.Length;
                        string[] tmp2 = FileLoad2Arr(logPath2);
                        int tmp2len = tmp2.Length;
                        if (tmp1len < tmp2len)
                        {
                            string missingit = tmp1[tmp1len - 1];
                            LogUtil.catchup(missingit + ",");
                        }
                    }

                    string[] strlist = FileLoad2Arr(logPath);
                    int len = strlist.Length;
                    string[] pricelist = FileLoad2Arr(logPath3);
                    int len3 = pricelist.Length;

                    if (len > 0 && len <= 3)
                    {
                        int t1 = int.Parse(strlist[len - 1]);
                        int t1Rand = t1 + randomN;
                        string strPrice = pricelist[len3 - 1];
                        int intPrice = int.Parse(strPrice);
                        currectPrice = intPrice;
                        LogUtil.writeLog(t1 + ",");
                        LogUtil.writeRand(t1Rand + ",");
                        LogUtil.writePrice(strPrice + ",");
                    }
                    else
                    {
                        int t1 = int.Parse(strlist[len - 1]);
                        int t1Rand = t1 + randomN;
                        string strPrice = pricelist[len3 - 1];
                        int intPrice = int.Parse(strPrice);
                        currectPrice = intPrice;
                        LogUtil.writeLog(t1 + ",");
                        LogUtil.writeRand(t1Rand + ",");
                        LogUtil.writePrice(strPrice + ",");

                        int t2 = int.Parse(strlist[len - 2]);
                        int t3 = int.Parse(strlist[len - 3]);
                        int t4 = int.Parse(strlist[len - 4]);

                        clearInventoryMaxProfits(intPrice);

                        #region kk
                        if (t4 > 0 && t3 <= 0 && t2 < 0 && t1 < 0 && inventoryk == 0) //1,0,-1,-2
                        {
                            string[] list = FileLoad2Arr(logPath2);
                            int tk2 = int.Parse(list[len - 2]);
                            int tk3 = int.Parse(list[len - 3]);
                            int tk4 = int.Parse(list[len - 4]);
                            if (tk2 < 0 && tk3 < 0 && tk4 < 0)
                            {
                                int abstk4 = Math.Abs(tk4);
                                int abstk3 = Math.Abs(tk3);
                                int abstk2 = Math.Abs(tk2);
                                //-1, -2, -3 80%
                                if (abstk4 <= abstk3 && abstk3 <= abstk2 && is_1_2_3) StatusKK(intPrice, 98); 
                                //-2, -3, -1 100%
                                if (abstk4 <= abstk3 && abstk3 >= abstk2 && abstk4 >= abstk2 && is_2_3_1) StatusKK(intPrice, 97);
                                //-3, -1, -2 50%
                                if (abstk4 >= abstk3 && abstk3 <= abstk2 && abstk4 >= abstk2 && is_3_1_2) StatusKK(intPrice, 75);
                                //-2, -1, -3 50%
                                if (abstk4 >= abstk3 && abstk3 <= abstk2 && abstk4 <= abstk2 && is_2_1_3) StatusKK(intPrice, 76);
                                //-1, -3, -2 33%
                                if (abstk4 <= abstk3 && abstk3 >= abstk2 && abstk4 <= abstk2 && abstk3 >= 350 && abstk2 >= 350 && is_1_3_2) StatusKK(intPrice, 80);
                                //-3, -2, -1 not [kk] add-12/30/14
                                if (abstk4 >= abstk3 && abstk3 >= abstk2 && abstk4 >= abstk2 && is_3_2_1) StatusKK(intPrice, 81);
                                //-3, -3, -1
                                if (ApproximatelyEquals(abstk4, abstk3, 0.25) && is_3_3_1)
                                {
                                    abstk3 = abstk4;
                                    if (abstk4 <= abstk3 && abstk3 >= abstk2 && abstk4 >= abstk2) StatusKK(intPrice, 90);
                                }

                            }
                            if (tk2 > 0 && tk3 > 0 && tk4 > 0)
                            {
                                if (tk4 >= tk3 && tk3 >= tk2 && is987) StatusKK(intPrice, 96); 
                            }
                            //+--
                            if (tk4 > 0 && tk3 < 0 && tk2 < 0 && is9_1_2)
                            {
                                if (Math.Abs(tk3) < Math.Abs(tk2)) StatusKK(intPrice, 66);
                            }
                            //++-
                            if (tk4 > 0 && tk3 > 0 && tk2 < 0 && is98_1)
                            {
                                if (tk4 > tk3) StatusKK(intPrice, 67);
                            }

                        }
                        #endregion

                        #region dk
                        if (t4 < 0 && t3 >= 0 && t2 > 0 && t1 > 0 && inventoryd == 0) //-1, 0, 1, 2
                        {
                            string[] list = FileLoad2Arr(logPath2);
                            int tk2 = int.Parse(list[len - 2]);
                            int tk3 = int.Parse(list[len - 3]);
                            int tk4 = int.Parse(list[len - 4]);
                            if (tk2 > 0 && tk3 > 0 && tk4 > 0)
                            {
                                //1, 2, 3 100%
                                if (tk4 <= tk3 && tk3 <= tk2 && is123) StatusDK(intPrice, 99); 
                                //2, 1, 3 33%
                                if (tk4 >= tk3 && tk3 <= tk2 && tk4 <= tk2 && is213) StatusDK(intPrice, 70);
                                //1, 3, 2 75%
                                if (tk4 <= tk3 && tk3 >= tk2 && tk4 <= tk2 && is132) StatusDK(intPrice, 80);
                                //2, 3, 1 
                                if (tk4 <= tk3 && tk3 >= tk2 && tk4 >= tk2 && is231) StatusDK(intPrice, 84);
                                //3, 1, 2
                                if (tk4 >= tk3 && tk3 <= tk2 && tk4 >= tk2 && is312) StatusDK(intPrice, 85);
                                //3, 2, 1 not [dk]     add-12/30/14  
                                if (tk4 >= tk3 && tk3 >= tk2 && tk4 >= tk2 && tk4 >= 350 && tk3 >= 350 && is321) StatusDK(intPrice, 87);
                                //3, 3, 1
                                if (ApproximatelyEquals(tk4, tk3, 0.25) && is331)
                                {
                                    tk3 = tk4;
                                    if (tk4 <= tk3 && tk3 >= tk2 && tk4 >= tk2) StatusDK(intPrice, 88);
                                }
                            }

                            if (tk2 < 0 && tk3 < 0 && tk4 < 0)
                            {
                                //-3,-2,-1
                                if (Math.Abs(tk4) >= Math.Abs(tk3) && Math.Abs(tk3) >= Math.Abs(tk2) && is_9_8_7) StatusDK(intPrice, 97);
                            }
                            //-++
                            if (tk4 < 0 && tk3 > 0 && tk2 > 0 && is_912)
                            {
                                if (tk3 < tk2) StatusDK(intPrice, 68);
                            }
                            //--+
                            if (tk4 < 0 && tk3 < 0 && tk2 > 0 && is_9_81)
                            {
                                if (Math.Abs(tk4) > Math.Abs(tk3)) StatusDK(intPrice, 69);
                            }

                        }
                        #endregion

                        #region KP
                        if (t3 < 0 && t2 < 0 && t1 < 0 && Math.Abs(t1) < Math.Abs(t2) && Math.Abs(t3) < Math.Abs(t2)) //-1,-3,-2
                        {
                            if (inventoryk == 1) lowTmpWithOpen = t2;
                            
                            if (lowTmp < Math.Abs(t2))
                            {
                                lowTmp = t2;
                                if (ratioTmp < Math.Abs(t2))
                                {
                                    ratioTmp = Convert.ToInt32(Math.Abs(t2 * 0.618));
                                }
                                LogUtil.writeLog("[" + ratioTmp + "],");
                            }
                        }


                        if (lowTmp < 0 && inventoryk == 1 && t1 < 0 && t2 < 0 && t3 < 0 && lowTmpWithOpen != 0)
                        {
                            string[] list = FileLoad2Arr(logPath2);
                            int tk1 = int.Parse(list[len - 1]);
                            int tk2 = int.Parse(list[len - 2]);
                            int tk3 = int.Parse(list[len - 3]);
                            int tk4 = int.Parse(list[len - 4]);

                            if (Math.Abs(lowTmpWithOpen) < ratioTmp && tk1 < 0 && tk2 < 0 && tk3 < 0)
                            {
                                lowTmpWithOpen = 0;
                            }
                            if (Math.Abs(lowTmpWithOpen) < ratioTmp && (tk1 > 0 || tk2 > 0 || tk3 > 0))
                            {
                                StatusKP(intPrice);
                            }
                            if (t1 < 0 && t2 < 0 && t3 < 0 && Math.Abs(t1) < ratioTmp && Math.Abs(t2) < ratioTmp && Math.Abs(t3) < ratioTmp && tk1 > 300)
                            {
                                StatusKP(intPrice);
                            }
                        }

                        if (t1 > 0 && t2 <= 0 && inventoryk == 1)
                        {
                            StatusKP(intPrice);
                        }
                        #endregion

                        #region dp
                        if (t3 > 0 && t2 > 0 && t1 > 0 && t1 < t2 && t3 < t2)
                        {
                            if (inventoryd == 1) highTmpWithOpen = t2;
                            
                            if (highTmp < t2)
                            {
                                highTmp = t2;
                                if (ratioTmp < t2)
                                {
                                    ratioTmp = Convert.ToInt32(t2 * 0.618);
                                }
                                LogUtil.writeLog("[" + ratioTmp + "],");
                            }
                        }

                        if (highTmp > 0 && inventoryd == 1 && t1 > 0 && t2 > 0 && t3 > 0 && highTmpWithOpen != 0)
                        {
                            string[] list = FileLoad2Arr(logPath2);
                            int tk1 = int.Parse(list[len - 1]);
                            int tk2 = int.Parse(list[len - 2]);
                            int tk3 = int.Parse(list[len - 3]);
                            int tk4 = int.Parse(list[len - 4]);

                            if (highTmpWithOpen < ratioTmp && tk1 > 0 && tk2 > 0 && tk3 > 0)
                            {
                                highTmpWithOpen = 0;
                            }
                            if (highTmpWithOpen < ratioTmp && (tk1 < 0 || tk2 < 0 || tk3 < 0))
                            {
                                StatusDP(intPrice);
                            }
                            if (t1 < ratioTmp && t2 < ratioTmp && t3 < ratioTmp && tk1 < 0 && Math.Abs(tk1) > 300)
                            {
                                StatusDP(intPrice);
                            }
                        }

                        if (t1 < 0 && t2 >= 0 && inventoryd == 1)
                        {
                            StatusDP(intPrice);
                        }
                        #endregion

                    }


                    //ftp log4me and log4pub
                    //ftpLogFile(logPath4Pub, remoteBasicPath, "ru1501.txt");
                    ftpLogFile(logPath34Pub, remoteBasicPath, "ru1501.txt");
                    ftpLogFile(logPath4Pub, remoteLogPath4Pub, "log4pub[" + GetCurrentDateTime(addHours) + "].txt");
                    ftpLogFile(logPath4Me,  remoteLogPath4Me, "log4me[" + GetCurrentDateTime(addHours) + "].txt");
                    ftpLogFile(logPath2, remotelogPath2, "log24me[" + GetCurrentDateTime(addHours) + "].txt");
                    ftpLogFile(logPath3, remotelogPath3, "log34me[" + GetCurrentDateTime(addHours) + "].txt");

                    //Archive log.txt to [D:\vl-docs\_save\log]
                    string f1 = logPath;
                    string f2 = logArchive + "\\" + GetCurrentDateTime(addHours) + ".txt";
                    if (Directory.Exists(logArchive)) File.Copy(f1, f2, true);
                }
                //send email to gmail
                if (!content.Equals(""))
                {
                    sendEmail(content);
                    System.IO.File.WriteAllText(@"D:\sign.txt", content);
                    ftpLogFile(@"D:\sign.txt",remoteBasicPath,"sign.txt");
                    content = "";
                }
            }
            catch (Exception e)
            {
                LogUtil.writeLog(e.Message);
            }
            timer.Start();
        }

        #region Status
        private void StatusKK(int kkPirce, int percentNum)
        {
            if (validTradingPeriod(addHours) && inventoryk == 0 && cntLost < totalLost)
            {
                pricek = kkPirce;
                string percent = percentNum + "%";
                //before kk, must dp;
                if (inventoryd == 1) StatusDP(kkPirce);

                highTmp = 0;
                inventoryk = 1;
                percentk = percentNum;//added on 01/05/15
                lowTmpWithOpen = 0;
                LogUtil.writeLog("[kk],");
                LogUtil.writeRand("<font color=green ><b><u>[kk-" + percent + "][" + GetCurrentDateTimeAdvance(addHours) + "]</u></b></font>,");
                LogUtil.writePrice("<font color=green ><b><u>[kk-" + percent + "][" + GetCurrentDateTimeAdvance(addHours) + "]</u></b></font>,");
                content = kkPirce + "[开空仓--获利可能性:" + percent + "][" + GetCurrentDateTimeAdvance(addHours) + "]";
                //onAlarmSound(); //added on 01/08/15
            }
            
        }

        private void StatusDK(int dkPrice, int percentNum)
        {
            if (validTradingPeriod(addHours) && inventoryd == 0 && cntLost < totalLost)
            {
                priced = dkPrice;
                string percent = percentNum + "%";
                //before dk, must kp;
                if (inventoryk == 1) StatusKP(dkPrice);

                lowTmp = 0;
                inventoryd = 1;
                percentd = percentNum;//added on 01/05/15
                highTmpWithOpen = 0;
                LogUtil.writeLog("[dk],");
                LogUtil.writeRand("<font color=red ><b><u>[dk-" + percent + "][" + GetCurrentDateTimeAdvance(addHours) + "]</u></b></font>,");
                LogUtil.writePrice("<font color=red ><b><u>[dk-" + percent + "][" + GetCurrentDateTimeAdvance(addHours) + "]</u></b></font>,");
                content = dkPrice + "[开多仓--获利可能性:" + percent + "][" + GetCurrentDateTimeAdvance(addHours) + "]";
                //onAlarmSound(); //added on 01/08/15
            }
        }

        private void StatusKP(int kpPrice)
        {
            if (inventoryk == 1)
            {
                int profits = pricek - kpPrice;
                LogUtil.writeLog("[kp],");
                LogUtil.writeRand("<font color=blue ><b><u>[kp][" + GetCurrentDateTimeAdvance(addHours) + "]</u></b></font>,");
                LogUtil.writePrice("<font color=blue ><b><u>[kp][" + GetCurrentDateTimeAdvance(addHours) + "]</u></b></font>,");
                lowTmp = 0;
                inventoryk = 0;
                percentk = 0;//added on 01/05/15
                lowTmpWithOpen = 0;
                pricek = 0;
                if (profits < 0)
                {
                    cntLost++;
                }
                content = kpPrice + "[平空仓--每手获利点数:" + profits + "][" + GetCurrentDateTimeAdvance(addHours) + "]";
                //onAlarmSound(); //added on 01/08/15
            }
            
        }

        private void StatusDP(int dpPrice)
        {
            if (inventoryd == 1)
            {
                int profits = dpPrice - priced;
                LogUtil.writeLog("[dp],");
                LogUtil.writeRand("<font color=blue ><b><u>[dp][" + GetCurrentDateTimeAdvance(addHours) + "]</u></b></font>,");
                LogUtil.writePrice("<font color=blue ><b><u>[dp][" + GetCurrentDateTimeAdvance(addHours) + "]</u></b></font>,");
                highTmp = 0;
                inventoryd = 0;
                percentd = 0;//added on 01/05/15
                highTmpWithOpen = 0;
                priced = 0;
                if (profits < 0)
                {
                    cntLost++;
                }
                content = dpPrice + "[平多仓--每手获利点数:" + profits + "][" + GetCurrentDateTimeAdvance(addHours) + "]";
                //onAlarmSound(); //added on 01/08/15
            }
            
        } 
        #endregion

        #region ftpLogFile
        private void ftpLogFile(string localNameWithPath, string remoteDir, string remoteName)
        { 
            FTPConnection ftp = new FTPConnection();
            ftp.ServerAddress =  GetValueByKey("host");
            ftp.UserName = GetValueByKey("user");
            ftp.Password = GetValueByKey("password");
            ftp.Connect();
            ftp.TransferType = FTPTransferType.BINARY;
            if (!ftp.DirectoryExists(remoteDir))
                ftp.CreateDirectory(remoteDir);
            //ftp.ChangeWorkingDirectory("/public_html");
            ftp.UploadFile(localNameWithPath, remoteDir + "//" + remoteName);
            ftp.Close();
        }
        #endregion

        #region sendEmail
        private void sendEmail(string content)
        {
            Email email = new Email();
            email.strSmtpServe = GetValueByKey("strSmtpServe");
            email.strFrom = GetValueByKey("strFrom");
            email.strFromPass = GetValueByKey("strFromPass");
            email.IsHtml = false;
            email.boolEnableSsl = Convert.ToBoolean(1);
            email.iPort = int.Parse(GetValueByKey("iPort"));
            email.SendSMTPEmail(GetValueByKey("strTo"), GetValueByKey("strSubject"), GetValueByKey("strBody")+ content);
        }
        #endregion

        #region lastModifiedDateTime
        private string lastModifiedDateTime(string logPath)
        {
            DateTime dt = File.GetLastWriteTime(logPath);
            return dt.ToString("yyyy-MM-dd HH:mm:ss");
        }
        #endregion

        #region GetCurrentDateTime
        private static string GetCurrentDateTime(int hours)
        {
            DateTime dt = DateTime.Now.AddHours(hours);
            return dt.ToString("yyyyMMdd");
        }

        private static string GetCurrentDateTimeAdvance(int hours)
        {
            DateTime dt = DateTime.Now.AddHours(hours);
            return dt.ToString("yyyyMMdd HH:mm:ss");
        }

        private static string GetCurrentDateTimeSpec(int hours)
        {
            DateTime dt = DateTime.Now.AddHours(hours);
            return dt.ToString("yyyy-MM-dd");
        }
        #endregion

        #region FileLoad2Arr
        private string[] FileLoad2Arr(string path)
        {
            string rawData = File.ReadAllText(path);
            char[] splitter = new char[] { ',' };
            string[] strlist = rawData.Split(splitter, StringSplitOptions.RemoveEmptyEntries);
            return strlist;
        } 
        #endregion

        #region AlarmSound
        private void onAlarmSound()
        {
            onSound = true;
            ftpLogFile(onSoundPath, remoteBasicPath, "sound.txt");
        }

        private void offAlarmSound()
        {
            if (onSound)
            {
                onSound = false;
                ftpLogFile(offSoundPath, remoteBasicPath, "sound.txt");
            }
        } 
        #endregion

        #region ClearInventory
        private void clearInventoryManually()
        {
            if (!Directory.Exists(dropFolder)) Directory.CreateDirectory(dropFolder);
            string[] fileArr = Directory.GetFiles(dropFolder);
            foreach (string str in fileArr)
            {
                if (Path.GetExtension(str).Equals(".txt"))
                {
                    string oper = Path.GetFileNameWithoutExtension(str);
                    if (oper.Equals("kp") && inventoryk == 1)
                    {
                        if (currectPrice != 0)
                        {
                            StatusKP(currectPrice);
                            ftpLogFile(logPath34Pub, remoteBasicPath, "ru1501.txt");
                        }
                    }

                    if (oper.Equals("dp") && inventoryd == 1)
                    {
                        if (currectPrice != 0)
                        {
                            StatusDP(currectPrice);
                            ftpLogFile(logPath34Pub, remoteBasicPath, "ru1501.txt");
                        }
                    }

                    if (oper.Equals("kk") && inventoryk == 0)
                    {
                        if (currectPrice != 0)
                        {
                            StatusKK(currectPrice, 91);
                            ftpLogFile(logPath34Pub, remoteBasicPath, "ru1501.txt");
                        }
                    }

                    if (oper.Equals("dk") && inventoryd == 0)
                    {
                        if (currectPrice != 0)
                        {
                            StatusDK(currectPrice, 93);
                            ftpLogFile(logPath34Pub, remoteBasicPath, "ru1501.txt");
                        }
                    }

                }
                File.Delete(str);
            }

            //added on 02/10/15
            /*
            FTPConnection ftp = new FTPConnection();
            ftp.ServerAddress = GetValueByKey("host");
            ftp.UserName = GetValueByKey("user");
            ftp.Password = GetValueByKey("password");
            ftp.Connect();
            string[] files = ftp.GetFiles("/public_html/drop_folder");
            if (files.Length > 2)
            {
                foreach (string str in files)
                {
                    if (Path.GetExtension(str).Equals(".txt"))
                    {
                        string oper = Path.GetFileNameWithoutExtension(str);
                        if (oper.Equals("kp") && inventoryk == 1)
                        {
                            if (currectPrice != 0)
                            {
                                StatusKP(currectPrice);
                                ftpLogFile(logPath34Pub, remoteBasicPath, "ru1501.txt");
                            }
                        }

                        if (oper.Equals("dp") && inventoryd == 1)
                        {
                            if (currectPrice != 0)
                            {
                                StatusDP(currectPrice);
                                ftpLogFile(logPath34Pub, remoteBasicPath, "ru1501.txt");
                            }
                        }

                        if (oper.Equals("kk") && inventoryk == 0)
                        {
                            if (currectPrice != 0)
                            {
                                StatusKK(currectPrice, 91);
                                ftpLogFile(logPath34Pub, remoteBasicPath, "ru1501.txt");
                            }
                        }

                        if (oper.Equals("dk") && inventoryd == 0)
                        {
                            if (currectPrice != 0)
                            {
                                StatusDK(currectPrice, 93);
                                ftpLogFile(logPath34Pub, remoteBasicPath, "ru1501.txt");
                            }
                        }
                        ftp.DeleteFile("/public_html/drop_folder/"+str);
                    }
                }
            }
            ftp.Close();
            */
        }

        private void clearInventoryLastMin(int addHours)
        {
            //added-01/05/15  last minute for afternoon
            DateTime Nowdt = DateTime.Now.AddHours(addHours);
            if (Nowdt > DateTime.Parse(GetCurrentDateTimeSpec(addHours) + " 22:59:00") && Nowdt < DateTime.Parse(GetCurrentDateTimeSpec(addHours) + " 22:59:59"))//editd on 03/23
            {
                if (inventoryd == 1)
                {
                    if (currectPrice != 0)
                    {
                        StatusDP(currectPrice);
                        ftpLogFile(logPath34Pub, remoteBasicPath, "ru1501.txt");
                    }
                }
                if (inventoryk == 1)
                {
                    if (currectPrice != 0)
                    {
                        StatusKP(currectPrice);
                        ftpLogFile(logPath34Pub, remoteBasicPath, "ru1501.txt");
                    }
                }
            }
        }

        private void clearInventoryMaxProfits(int price)
        {
            if (inventoryd == 1 && priced != 0 && price != 0)
            {
                if ((price - priced) > profitpts && dpTmp == 0)
                {
                    dpTmp = price;
                    //StatusDP(price);
                }
                if (price > dpTmp && dpTmp != 0)
                {
                    dpTmp = price;
                }
                if (price < dpTmp && dpTmp != 0)
                {
                    StatusDP(price);
                    dpTmp = 0;
                }
            }
            if (inventoryk == 1 && pricek != 0 && price != 0)
            {
                if ((pricek - price) > profitpts && kpTmp == 0)
                {
                    kpTmp = price;
                    //StatusKP(price);
                }
                if (price < kpTmp && kpTmp != 0)
                {
                    kpTmp = price;
                }
                if (price > kpTmp && kpTmp != 0)
                {
                    StatusKP(price);
                    kpTmp = 0;
                }
            }
        }
        #endregion

        private void backupFtpCSV(int addHours)
        { 
            DateTime Nowdt = DateTime.Now.AddHours(addHours);
            if (Nowdt > DateTime.Parse(GetCurrentDateTimeSpec(addHours) + " 22:59:00") && Nowdt < DateTime.Parse(GetCurrentDateTimeSpec(addHours) + " 23:00:00") && !isCSV)
            {
                string logPathRawData = File.ReadAllText(logPath);
                LogUtil.writeCSV(logPathRawData);
                string logPath2RawData = File.ReadAllText(logPath2);
                LogUtil.writeCSV(logPath2RawData);
                string logPath3RawData = File.ReadAllText(logPath3);
                LogUtil.writeCSV(logPath3RawData);
                if (File.Exists(@"D:\records.csv"))
                {
                    ftpLogFile(@"D:\records.csv", remoteBasicPath, "records.csv");
                }
                isCSV = true;
            }
        }

        #region validTradingPeriod
        private bool validTradingPeriod(int addHours)
        {
            DateTime Nowdt = DateTime.Now.AddHours(addHours);
            if (Nowdt > DateTime.Parse(GetCurrentDateTimeSpec(addHours) + " 08:55:00") && Nowdt < DateTime.Parse(GetCurrentDateTimeSpec(addHours) + " 22:45:00"))
                return true;
            else
                return false;
        } 
        #endregion

        #region ApproximatelyEquals

        private bool isMatch(string file1, string file2)
        {
            int file1Arr = FileLoad2Arr(file1).Length;
            int file2Arr = FileLoad2Arr(file2).Length;
            if (file1Arr == file2Arr) return true;
            else return false;
        }

        private bool ApproximatelyEquals(double value1, double value2, double acceptableDifference)
        {
            if (value1 >= 350 && value2 >= 350)
            {
                double ratio = value1 / value2;
                double diff = Math.Abs(ratio - 1);
                return diff <= acceptableDifference;
            }
            return false;
        } 
        #endregion

        #region OnStop
        protected override void OnStop()
        {
            if (timer != null)
                timer.Stop();
        }
        #endregion
    }
}
