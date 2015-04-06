using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Xml;
using System.Data.OleDb;
using System.Data;
using System.Collections;

namespace CReviewIt
{
    class Program
    {
        #region Field
        private static int[] logArr;
        private static int[] log2Arr;
        private static int[] log3Arr;

        private static Dictionary<string, string> dict;

        private static string logPath;
        private static string logPath2;
        private static string logPath3;

        private static int lowTmp;
        private static int highTmp;
        private static int inventoryk;
        private static int inventoryd;
        private static int ratioTmp;

        private static int percentd;
        private static int percentk;

        private static int lowTmpWithOpen;
        private static int highTmpWithOpen;

        private static int profitpts;
        private static int priced;// price when dk
        private static int pricek;// price when kk 
        private static int currectPrice;

        private static int kpTmp;
        private static int dpTmp;

        private static int kpTmpRatio;
        private static int dpTmpRatio;

        private static int cntLost;
        private static int totalLost;

        private static int totalProfits;
        private static ArrayList totalProfits_al = new ArrayList();

        private static bool is_1_2_3;
        private static int is_1_2_3_profit;
        private static ArrayList is_1_2_3_al = new ArrayList();
        private static int is_1_2_3_profitpts;
        private static int[] is_1_2_3_vol;

        private static bool is_2_3_1;
        private static int is_2_3_1_profit;
        private static ArrayList is_2_3_1_al = new ArrayList();
        private static int is_2_3_1_profitpts;
        private static int[] is_2_3_1_vol;

        private static bool is_3_1_2;
        private static int is_3_1_2_profit;
        private static ArrayList is_3_1_2_al = new ArrayList();
        private static int is_3_1_2_profitpts;
        private static int[] is_3_1_2_vol;

        private static bool is_2_1_3;
        private static int is_2_1_3_profit;
        private static ArrayList is_2_1_3_al = new ArrayList();
        private static int is_2_1_3_profitpts;
        private static int[] is_2_1_3_vol;

        private static bool is_1_3_2;
        private static int is_1_3_2_profit;
        private static ArrayList is_1_3_2_al = new ArrayList();
        private static int is_1_3_2_profitpts;
        private static int[] is_1_3_2_vol;

        private static bool is_3_2_1;
        private static int is_3_2_1_profit;
        private static ArrayList is_3_2_1_al = new ArrayList();
        private static int is_3_2_1_profitpts;
        private static int[] is_3_2_1_vol;

        private static bool is_3_3_1;
        private static int is_3_3_1_profit;
        private static ArrayList is_3_3_1_al = new ArrayList();
        private static int is_3_3_1_profitpts;
        private static int[] is_3_3_1_vol;

        private static bool is987;
        private static int is987_profit;
        private static ArrayList is987_al = new ArrayList();
        private static int is987_profitpts;
        private static int[] is987_vol;

        private static bool is9_1_2;
        private static int is9_1_2_profit;
        private static ArrayList is9_1_2_al = new ArrayList();
        private static int is9_1_2_profitpts;
        private static int[] is9_1_2_vol;

        private static bool is98_1;
        private static int is98_1_profit;
        private static ArrayList is98_1_al = new ArrayList();
        private static int is98_1_profitpts;
        private static int[] is98_1_vol;


        private static bool is123;
        private static int is123_profit;
        private static ArrayList is123_al = new ArrayList();
        private static int is123_profitpts;
        private static int[] is123_vol;

        private static bool is213;
        private static int is213_profit;
        private static ArrayList is213_al = new ArrayList();
        private static int is213_profitpts;
        private static int[] is213_vol;

        private static bool is132;
        private static int is132_profit;
        private static ArrayList is132_al = new ArrayList();
        private static int is132_profitpts;
        private static int[] is132_vol;

        private static bool is231;
        private static int is231_profit;
        private static ArrayList is231_al = new ArrayList();
        private static int is231_profitpts;
        private static int[] is231_vol;

        private static bool is312;
        private static int is312_profit;
        private static ArrayList is312_al = new ArrayList();
        private static int is312_profitpts;
        private static int[] is312_vol;

        private static bool is321;
        private static int is321_profit;
        private static ArrayList is321_al = new ArrayList();
        private static int is321_profitpts;
        private static int[] is321_vol;

        private static bool is331;
        private static int is331_profit;
        private static ArrayList is331_al = new ArrayList();
        private static int is331_profitpts;
        private static int[] is331_vol;

        private static bool is_9_8_7;
        private static int is_9_8_7_profit;
        private static ArrayList is_9_8_7_al = new ArrayList();
        private static int is_9_8_7_profitpts;
        private static int[] is_9_8_7_vol;

        private static bool is_912;
        private static int is_912_profit;
        private static ArrayList is_912_al = new ArrayList();
        private static int is_912_profitpts;
        private static int[] is_912_vol;

        private static bool is_9_81;
        private static int is_9_81_profit;
        private static ArrayList is_9_81_al = new ArrayList(); 
        private static int is_9_81_profitpts;
        private static int[] is_9_81_vol;

        #endregion

        private static string[] profitArr = {"50","75","100","125","150","175","200","225","250","275","300"};
        static void Main(string[] args)
        {
            if (args[0].Equals("1"))
            {
                taskBegin();
            }
            else if (args[0].Equals("2"))
            {
                LoadConfig();
                CalcExcelBySheet();
            }
            else if (args[0].Equals("3"))
            {
                FileStream fs = new FileStream("zhibiao.txt", FileMode.Open, FileAccess.Read);
                StreamReader m_streamReader = new StreamReader(fs);
                m_streamReader.BaseStream.Seek(0, SeekOrigin.Begin);

                string strLine = m_streamReader.ReadLine();
                while (strLine != null)
                {
                    if (!strLine.Trim().Equals(""))
                    {
                        foreach (string pts in profitArr)
                        {
                            totalProfits = 0;
                            UpdateConfig(strLine.Trim(), pts);
                            LoadConfig();
                            CalcExcelBySheet();
                            LogUtil.writeLog1(strLine + "-- pts-" + pts + "--   " + totalProfits);
                        }

                    }
                    strLine = m_streamReader.ReadLine();
                }
                m_streamReader.Close();
            }
            else
            {
                FileStream fs = new FileStream("zhibiao.txt", FileMode.Open, FileAccess.Read);
                StreamReader m_streamReader = new StreamReader(fs);
                m_streamReader.BaseStream.Seek(0, SeekOrigin.Begin);

                string strLine = m_streamReader.ReadLine();
                while (strLine != null)
                {
                    if (!strLine.Trim().Equals(""))
                    {
                        totalProfits = 0;
                        UpdateConfig(strLine.Trim(), "225");
                        LoadConfig();
                        CalcExcelBySheet();
                        LogUtil.writeLog1(strLine + "-- pts-" + "" + "--   " + totalProfits);
                    }
                    strLine = m_streamReader.ReadLine();
                }
                m_streamReader.Close();
            }
        }

        #region CalcExcelBySheet
        private static void CalcExcelBySheet()
        {
            var fileName = string.Format("{0}\\recordss.xlsx", Directory.GetCurrentDirectory());
            var connectionString = string.Format("Provider=Microsoft.ACE.OLEDB.12.0;Data Source={0}; Extended Properties=Excel 12.0;", fileName);
            string[] strArr = GetValueByKey("sheetsName").Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
            OleDbDataAdapter adapter;
            DataSet ds;
            DataTable data;
            StringBuilder sb;
            StringBuilder sb2;
            StringBuilder sb3;
            foreach (string sheetName in strArr)
            {
                cntLost = 0;
                //Console.WriteLine(sheetName);
                adapter = new OleDbDataAdapter("SELECT * FROM [" + sheetName + "$]", connectionString);
                ds = new DataSet();
                adapter.Fill(ds, sheetName);
                data = ds.Tables[sheetName];
                sb = new StringBuilder();
                sb2 = new StringBuilder();
                sb3 = new StringBuilder();
                sb.AppendLine(string.Join("\t", data.Rows[0].ItemArray));
                sb2.AppendLine(string.Join("\t", data.Rows[1].ItemArray));
                sb3.AppendLine(string.Join("\t", data.Rows[2].ItemArray));
                File.WriteAllText("log.txt", sb.ToString());
                File.WriteAllText("log2.txt", sb2.ToString());
                File.WriteAllText("log3.txt", sb3.ToString());
                LogUtil.writeLog("======" + sheetName + "===Start===");
                taskBegin();
                LogUtil.writeLog("======" + sheetName + "===End===");
                LogUtil.writeLog("");
            }
        } 
        #endregion

        #region UpdateConfig
        private static void UpdateConfig(string strLine, string profitpts)
        {
            char[] arrStr = strLine.ToCharArray();
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.Load("config.xml");
            XmlNodeList nodes = xmlDoc.GetElementsByTagName("add");
            string keystr;
            for (int i = 0; i < nodes.Count; i++)
            {
                keystr = nodes[i].Attributes["key"].Value.Trim();
                if (keystr.Equals("-1-2-3"))
                    nodes[i].Attributes["value"].Value = arrStr[0] + "";
                if (keystr.Equals("-2-3-1"))
                    nodes[i].Attributes["value"].Value = arrStr[1] + "";
                if (keystr.Equals("-3-1-2"))
                    nodes[i].Attributes["value"].Value = arrStr[2] + "";
                if (keystr.Equals("-2-1-3"))
                    nodes[i].Attributes["value"].Value = arrStr[3] + "";
                if (keystr.Equals("-1-3-2"))
                    nodes[i].Attributes["value"].Value = arrStr[4] + "";
                if (keystr.Equals("-3-2-1"))
                    nodes[i].Attributes["value"].Value = arrStr[5] + "";
                if (keystr.Equals("-3-3-1"))
                    nodes[i].Attributes["value"].Value = arrStr[6] + "";
                if (keystr.Equals("+++"))
                    nodes[i].Attributes["value"].Value = arrStr[7] + "";
                if (keystr.Equals("+--"))
                    nodes[i].Attributes["value"].Value = arrStr[8] + "";
                if (keystr.Equals("++-"))
                    nodes[i].Attributes["value"].Value = arrStr[9] + "";
                if (keystr.Equals("123"))
                    nodes[i].Attributes["value"].Value = arrStr[10] + "";
                if (keystr.Equals("213"))
                    nodes[i].Attributes["value"].Value = arrStr[11] + "";
                if (keystr.Equals("132"))
                    nodes[i].Attributes["value"].Value = arrStr[12] + "";
                if (keystr.Equals("231"))
                    nodes[i].Attributes["value"].Value = arrStr[13] + "";
                if (keystr.Equals("312"))
                    nodes[i].Attributes["value"].Value = arrStr[14] + "";
                if (keystr.Equals("321"))
                    nodes[i].Attributes["value"].Value = arrStr[15] + "";
                if (keystr.Equals("331"))
                    nodes[i].Attributes["value"].Value = arrStr[16] + "";
                if (keystr.Equals("---"))
                    nodes[i].Attributes["value"].Value = arrStr[17] + "";
                if (keystr.Equals("-++"))
                    nodes[i].Attributes["value"].Value = arrStr[18] + "";
                if (keystr.Equals("--+"))
                    nodes[i].Attributes["value"].Value = arrStr[19] + "";

                if (keystr.Equals("profitpts"))
                    nodes[i].Attributes["value"].Value = profitpts;
            }
            xmlDoc.Save("config.xml");
        } 
        #endregion

        #region LoadConfig
        private static void LoadConfig()
        {
            dict = new Dictionary<string, string>();
            XmlDocument doc = new XmlDocument();
            doc.Load("config.xml");
            XmlNodeList nodes = doc.GetElementsByTagName("add");
            for (int i = 0; i < nodes.Count; i++)
            {
                string keystr = nodes[i].Attributes["key"].Value.Trim();
                string valuestr = nodes[i].Attributes["value"].Value.Trim();
                dict.Add(keystr, valuestr);
            }
        } 
        #endregion

        #region GetValueByKey
        private static string GetValueByKey(string key)
        {
            foreach (KeyValuePair<string, string> entry in dict)
            {
                if (entry.Key.ToUpper().Trim().Equals(key.ToUpper()))
                    return entry.Value.Trim();
            }
            return "";
        } 
        #endregion

        #region ApproximatelyEquals
        private static bool ApproximatelyEquals(double value1, double value2, double acceptableDifference)
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

        #region Status
        private static void StatusKK(int kkPirce, int percentNum)
        {
            if (inventoryk == 0 && cntLost < totalLost)
            {
                pricek = kkPirce;
                string percent = percentNum + "%";
                //before kk, must dp;
                if (inventoryd == 1) StatusDP(kkPirce);

                highTmp = 0;
                inventoryk = 1;
                percentk = percentNum;//added on 01/05/15
                lowTmpWithOpen = 0;
                //LogUtil.writeLog("[kk],");
                //LogUtil.writeRand("<font color=green ><b><u>[kk-" + percent + "][" + GetCurrentDateTimeAdvance(addHours) + "]</u></b></font>,");
                //LogUtil.writePrice("<font color=green ><b><u>[kk-" + percent + "][" + GetCurrentDateTimeAdvance(addHours) + "]</u></b></font>,");
                string content = kkPirce + "[开空仓--获利可能性:" + percent + "]";
                LogUtil.writeLog(content);
            }
            
        }

        private static void StatusDK(int dkPrice, int percentNum)
        {
            if (inventoryd == 0 && cntLost < totalLost)
            {
                priced = dkPrice;
                string percent = percentNum + "%";
                //before dk, must kp;
                if (inventoryk == 1) StatusKP(dkPrice);

                lowTmp = 0;
                inventoryd = 1;
                percentd = percentNum;//added on 01/05/15
                highTmpWithOpen = 0;
                //LogUtil.writeLog("[dk],");
                //LogUtil.writeRand("<font color=red ><b><u>[dk-" + percent + "][" + GetCurrentDateTimeAdvance(addHours) + "]</u></b></font>,");
                //LogUtil.writePrice("<font color=red ><b><u>[dk-" + percent + "][" + GetCurrentDateTimeAdvance(addHours) + "]</u></b></font>,");
                string content = dkPrice + "[开多仓--获利可能性:" + percent + "]";
                LogUtil.writeLog(content);
            }
            
        }

        private static void StatusKP(int kpPrice)
        {
            if (inventoryk == 1)
            {
                int profits = pricek - kpPrice;
                
                //LogUtil.writeLog("[kp],");
                //LogUtil.writeRand("<font color=blue ><b><u>[kp][" + GetCurrentDateTimeAdvance(addHours) + "]</u></b></font>,");
                //LogUtil.writePrice("<font color=blue ><b><u>[kp][" + GetCurrentDateTimeAdvance(addHours) + "]</u></b></font>,");
                lowTmp = 0;
                inventoryk = 0;
                
                lowTmpWithOpen = 0;
                pricek = 0;
                if (profits < 0)
                {
                    cntLost++;
                }
                string content = kpPrice + "[平空仓--每手获利点数:" + profits + "]";
                profitByPercentk(percentk, profits);
                percentk = 0;//added on 01/05/15
                totalProfits += profits;
                totalProfits_al.Add(totalProfits);
                LogUtil.writeLog(content);
                LogUtil.writeLog("  -totalProfits = " + totalProfits);
            }
            
        }

        private static void StatusDP(int dpPrice)
        {
            if (inventoryd == 1)
            {
                int profits = dpPrice - priced;
                //LogUtil.writeLog("[dp],");
                //LogUtil.writeRand("<font color=blue ><b><u>[dp][" + GetCurrentDateTimeAdvance(addHours) + "]</u></b></font>,");
                //LogUtil.writePrice("<font color=blue ><b><u>[dp][" + GetCurrentDateTimeAdvance(addHours) + "]</u></b></font>,");
                highTmp = 0;
                inventoryd = 0;
                
                highTmpWithOpen = 0;
                priced = 0;
                if (profits < 0)
                {
                    cntLost++;
                }
                string content = dpPrice + "[平多仓--每手获利点数:" + profits + "]";
                profitByPercentd(percentd, profits);
                percentd = 0;//added on 01/05/15
                totalProfits += profits;
                totalProfits_al.Add(totalProfits);
                LogUtil.writeLog(content);
                LogUtil.writeLog("  -totalProfits = " + totalProfits);
            }
            
        }
        #endregion

        #region profitByPercent
        private static void profitByPercentk(int percent, int profit)
        {
            switch (percent)
            {
                case 98:
                    is_1_2_3_al.Add(profit);
                    is_1_2_3_profit += profit;
                    //LogUtil.writeLog("  is_1_2_3_Profit = " + is_1_2_3_profit);
                    break;
                case 97:
                    is_2_3_1_al.Add(profit);
                    is_2_3_1_profit += profit;
                    //LogUtil.writeLog("  is_2_3_1_profit = " + is_2_3_1_profit);
                    break;
                case 75:
                    is_3_1_2_al.Add(profit);
                    is_3_1_2_profit += profit;
                    //LogUtil.writeLog("  is_3_1_2_profit = " + is_3_1_2_profit);
                    break;
                case 76:
                    is_2_1_3_al.Add(profit);
                    is_2_1_3_profit += profit;
                    //LogUtil.writeLog("  is_2_1_3_Profit = " + is_2_1_3_profit);
                    break;
                case 80:
                    is_1_3_2_al.Add(profit);
                    is_1_3_2_profit += profit;
                    //LogUtil.writeLog("  is_1_3_2_profit = " + is_1_3_2_profit);
                    break;
                case 81:
                    is_3_2_1_al.Add(profit);
                    is_3_2_1_profit += profit;
                    //LogUtil.writeLog("  is_3_2_1_profit = " + is_3_2_1_profit);
                    break;
                case 90:
                    is_3_3_1_al.Add(profit);
                    is_3_3_1_profit += profit;
                    break;
                case 96:
                    is987_al.Add(profit);
                    is987_profit += profit;
                    //LogUtil.writeLog("  is987_profit = " + is987_profit);
                    break;
                case 66:
                    is9_1_2_al.Add(profit);
                    is9_1_2_profit += profit;
                    //LogUtil.writeLog("  is9_1_2_profit = " + is9_1_2_profit);
                    break;
                case 67:
                    is98_1_al.Add(profit);
                    is98_1_profit += profit;
                    //LogUtil.writeLog("  is98_1_profit = " + is98_1_profit);
                    break;
            }
        }

        private static void profitByPercentd(int percent, int profit)
        {
            switch (percent)
            {
                case 99:
                    is123_al.Add(profit);
                    is123_profit += profit;
                    //LogUtil.writeLog("  is123_profit = " + is123_profit);
                    break;
                case 70:
                    is213_al.Add(profit);
                    is213_profit += profit;
                    //LogUtil.writeLog("  is213_profit = " + is213_profit);
                    break;
                case 80:
                    is132_al.Add(profit);
                    is132_profit += profit;
                    //LogUtil.writeLog("  is132_profit = " + is132_profit);
                    break;
                case 84:
                    is231_al.Add(profit);
                    is231_profit += profit;
                    //LogUtil.writeLog("  is231_profit = " + is231_profit);
                    break;
                case 85:
                    is312_al.Add(profit);
                    is312_profit += profit;
                    //LogUtil.writeLog("  is312_profit = " + is312_profit);
                    break;
                case 87:
                    is321_al.Add(profit);
                    is321_profit += profit;
                    //LogUtil.writeLog("  is321_profit = " + is321_profit);
                    break;
                case 88:
                    is331_al.Add(profit);
                    is331_profit += profit;
                    break;
                case 97:
                    is_9_8_7_al.Add(profit);
                    is_9_8_7_profit += profit;
                    //LogUtil.writeLog("  is_9_8_7_profit = " + is_9_8_7_profit);
                    break;
                case 68:
                    is_912_al.Add(profit);
                    is_912_profit += profit;
                    //LogUtil.writeLog("  is_912_profit = " + is_912_profit);
                    break;
                case 69:
                    is_9_81_al.Add(profit);
                    is_9_81_profit += profit;
                    //LogUtil.writeLog("  is_9_81_profit = " + is_9_81_profit);
                    break;
            }
        } 
        #endregion

        #region clearInventory
        private static void clearInventoryMaxProfits(int price)
        {
            if (inventoryd == 1 && priced != 0 && price != 0)
            {
                int profitptsd = 0;
                switch (percentd)
                {
                    case 99:
                        profitptsd = is123_profitpts;
                        break;
                    case 70:
                        profitptsd = is213_profitpts;
                        break;
                    case 80:
                        profitptsd = is132_profitpts;
                        break;
                    case 84:
                        profitptsd = is231_profitpts;
                        break;
                    case 85:
                        profitptsd = is312_profitpts;
                        break;
                    case 87:
                        profitptsd = is321_profitpts;
                        break;
                    case 88:
                        profitptsd = is331_profitpts;
                        break;
                    case 97:
                        profitptsd = is_9_8_7_profitpts;
                        break;
                    case 68:
                        profitptsd = is_912_profitpts;
                        break;
                    case 69:
                        profitptsd = is_9_81_profitpts;
                        break;
                }
                if ((price - priced) > profitptsd && dpTmp == 0)
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
                int profitptsk = 0;
                switch (percentk)
                {
                    case 98:
                        profitptsk = is_1_2_3_profitpts;
                        break;
                    case 97:
                        profitptsk = is_2_3_1_profitpts;
                        break;
                    case 75:
                        profitptsk = is_3_1_2_profitpts;
                        break;
                    case 76:
                        profitptsk = is_2_1_3_profitpts;
                        break;
                    case 80:
                        profitptsk = is_1_3_2_profitpts;
                        break;
                    case 81:
                        profitptsk = is_3_2_1_profitpts;
                        break;
                    case 90:
                        profitptsk = is_3_3_1_profitpts;
                        break;
                    case 96:
                        profitptsk = is987_profitpts;
                        break;
                    case 66:
                        profitptsk = is9_1_2_profitpts;
                        break;
                    case 67:
                        profitptsk = is98_1_profitpts;
                        break;
                }
                if ((pricek - price) > profitptsk && kpTmp == 0)
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
       

        private static void clearInventoryWithRatio(int price)
        {
            if (inventoryd == 1 && priced != 0 && price != 0)
            {
                int pfptmp = price - priced;

                if (pfptmp >=50 && pfptmp < profitpts && dpTmpRatio == 0)
                {
                    dpTmpRatio = price;
                    //StatusDP(price);
                }
                if (price > dpTmpRatio && dpTmpRatio != 0)
                {
                    dpTmpRatio = price;
                }
                if (price <= dpTmpRatio*0.5 && dpTmpRatio != 0)
                {
                    StatusDP(price);
                    dpTmpRatio = 0;
                }
            }
            if (inventoryk == 1 && pricek != 0 && price != 0)
            {
                int pfptmp = pricek - price;
                if (pfptmp >= 50 && pfptmp < profitpts && kpTmpRatio == 0)
                {
                    kpTmpRatio = price;
                    //StatusKP(price);
                }
                if (price < kpTmpRatio && kpTmpRatio != 0)
                {
                    kpTmpRatio = price;
                }
                if (price >=kpTmpRatio*0.5 && kpTmpRatio != 0)
                {
                    StatusKP(price);
                    kpTmpRatio = 0;
                }
            }
        }

        private static void clearInventoryLastMin()
        {
            if (inventoryd == 1)
            {
                if (currectPrice != 0)
                {
                    StatusDP(currectPrice);
                }
            }
            if (inventoryk == 1)
            {
                if (currectPrice != 0)
                {
                    StatusKP(currectPrice);
                }
            }
        }
        #endregion

        #region taskBegin
        private static void taskBegin()
        {
            logPath = GetValueByKey("logPath");
            logPath2 = GetValueByKey("logPath2");
            logPath3 = GetValueByKey("logPath3");

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

            is_1_2_3_profitpts  = int.Parse(GetValueByKey("-1-2-3_profitpts"));
            is_2_3_1_profitpts  = int.Parse(GetValueByKey("-2-3-1_profitpts"));
            is_3_1_2_profitpts  = int.Parse(GetValueByKey("-3-1-2_profitpts"));
            is_2_1_3_profitpts  = int.Parse(GetValueByKey("-2-1-3_profitpts"));
            is_1_3_2_profitpts  = int.Parse(GetValueByKey("-1-3-2_profitpts"));
            is_3_2_1_profitpts  = int.Parse(GetValueByKey("-3-2-1_profitpts"));
            is_3_3_1_profitpts  = int.Parse(GetValueByKey("-3-3-1_profitpts"));
            is987_profitpts  = int.Parse(GetValueByKey("+++_profitpts"));
            is9_1_2_profitpts  = int.Parse(GetValueByKey("+--_profitpts"));
            is98_1_profitpts  = int.Parse(GetValueByKey("++-_profitpts"));

            is123_profitpts  = int.Parse(GetValueByKey("123_profitpts"));
            is213_profitpts  = int.Parse(GetValueByKey("213_profitpts"));
            is132_profitpts  = int.Parse(GetValueByKey("132_profitpts"));
            is231_profitpts  = int.Parse(GetValueByKey("231_profitpts"));
            is312_profitpts  = int.Parse(GetValueByKey("312_profitpts"));
            is321_profitpts  = int.Parse(GetValueByKey("321_profitpts"));
            is331_profitpts  = int.Parse(GetValueByKey("331_profitpts"));
            is_9_8_7_profitpts  = int.Parse(GetValueByKey("---_profitpts"));
            is_912_profitpts  = int.Parse(GetValueByKey("-++_profitpts"));
            is_9_81_profitpts  = int.Parse(GetValueByKey("--+_profitpts"));

            is_1_2_3_vol = GetValueByKey("-1-2-3_vol").Split(',').Select(n => Convert.ToInt32(n)).ToArray();
            is_2_3_1_vol = GetValueByKey("-2-3-1_vol").Split(',').Select(n => Convert.ToInt32(n)).ToArray();
            is_3_1_2_vol = GetValueByKey("-3-1-2_vol").Split(',').Select(n => Convert.ToInt32(n)).ToArray();
            is_2_1_3_vol = GetValueByKey("-2-1-3_vol").Split(',').Select(n => Convert.ToInt32(n)).ToArray();
            is_1_3_2_vol = GetValueByKey("-1-3-2_vol").Split(',').Select(n => Convert.ToInt32(n)).ToArray();
            is_3_2_1_vol = GetValueByKey("-3-2-1_vol").Split(',').Select(n => Convert.ToInt32(n)).ToArray();
            is_3_3_1_vol = GetValueByKey("-3-3-1_vol").Split(',').Select(n => Convert.ToInt32(n)).ToArray();
            is987_vol = GetValueByKey("+++_vol").Split(',').Select(n => Convert.ToInt32(n)).ToArray();
            is9_1_2_vol = GetValueByKey("+--_vol").Split(',').Select(n => Convert.ToInt32(n)).ToArray();
            is98_1_vol = GetValueByKey("++-_vol").Split(',').Select(n => Convert.ToInt32(n)).ToArray();

            is123_vol = GetValueByKey("123_vol").Split(',').Select(n => Convert.ToInt32(n)).ToArray();
            is213_vol = GetValueByKey("213_vol").Split(',').Select(n => Convert.ToInt32(n)).ToArray();
            is132_vol = GetValueByKey("132_vol").Split(',').Select(n => Convert.ToInt32(n)).ToArray();
            is231_vol = GetValueByKey("231_vol").Split(',').Select(n => Convert.ToInt32(n)).ToArray();
            is312_vol = GetValueByKey("312_vol").Split(',').Select(n => Convert.ToInt32(n)).ToArray();
            is321_vol = GetValueByKey("321_vol").Split(',').Select(n => Convert.ToInt32(n)).ToArray();
            is331_vol = GetValueByKey("331_vol").Split(',').Select(n => Convert.ToInt32(n)).ToArray();
            is_9_8_7_vol = GetValueByKey("---_vol").Split(',').Select(n => Convert.ToInt32(n)).ToArray();
            is_912_vol = GetValueByKey("-++_vol").Split(',').Select(n => Convert.ToInt32(n)).ToArray();
            is_9_81_vol = GetValueByKey("--+_vol").Split(',').Select(n => Convert.ToInt32(n)).ToArray();

            try
            {
                //Console.WriteLine("===");
                string logStr = File.ReadAllText(logPath);
                string log2Str = File.ReadAllText(logPath2);
                string log3Str = File.ReadAllText(logPath3);
                //Console.WriteLine("=0==");

                logArr = logStr.Split('\t').Select(n => Convert.ToInt32(n)).ToArray();
                Console.WriteLine(logArr.Length);
                log2Arr = log2Str.Split('\t').Select(n => Convert.ToInt32(n)).ToArray();
                log3Arr = log3Str.Split('\t').Select(n => Convert.ToInt32(n)).ToArray();
                //Console.WriteLine("=1==");
                Console.WriteLine(logArr.Length);
                Console.WriteLine(log2Arr.Length);
                Console.WriteLine(log3Arr.Length);

                if (logArr.Length == log2Arr.Length && log2Arr.Length == log3Arr.Length)
                {
                    for (int i = 3; i < logArr.Length; i++)
                    {
                        int t1 = logArr[i];
                        int intPrice = log3Arr[i];
                        currectPrice = intPrice;

                        int t2 = logArr[i - 1];
                        int t3 = logArr[i - 2];
                        int t4 = logArr[i - 3];

                        int tk1 = log2Arr[i];
                        int tk2 = log2Arr[i - 1];
                        int tk3 = log2Arr[i - 2];
                        int tk4 = log2Arr[i - 3];

                        //clearInventoryWithRatio(intPrice);
                        clearInventoryMaxProfits(intPrice);

                        if (i == logArr.Length - 1)
                        {
                            clearInventoryLastMin();
                        }
                        #region kk
                        if (t4 > 0 && t3 <= 0 && t2 < 0 && t1 < 0 && inventoryk == 0) //1,0,-1,-2
                        {

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
                                if (abstk4 <= abstk3 && abstk3 >= abstk2 && abstk4 <= abstk2 && abstk3 >= is_1_3_2_vol[1] && abstk2 >= is_1_3_2_vol[2] && is_1_3_2) StatusKK(intPrice, 80); //good
                                //-3, -2, -1 not [kk] add-12/30/14
                                if (abstk4 >= abstk3 && abstk3 >= abstk2 && abstk4 >= abstk2 && abstk4 >= is_3_2_1_vol[0] && abstk3 >= is_3_2_1_vol[1] && is_3_2_1) StatusKK(intPrice, 81);//good
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
                            if (tk2 > 0 && tk3 > 0 && tk4 > 0)
                            {
                                //1, 2, 3 100%
                                if (tk4 <= tk3 && tk3 <= tk2 && tk4 <= tk2 && tk4 <= is123_vol[0] && tk3 <= is123_vol[1] && tk2 <= is123_vol[2] && is123) StatusDK(intPrice, 99);//good
                                //2, 1, 3 33%
                                if (tk4 >= tk3 && tk3 <= tk2 && tk4 <= tk2 && tk4 >= is213_vol[0] && tk3 >= is213_vol[1] && is213) StatusDK(intPrice, 70);//good
                                //1, 3, 2 75%
                                if (tk4 <= tk3 && tk3 >= tk2 && tk4 <= tk2 && tk4 >= is132_vol[0] && tk3 >= is132_vol[1] && is132) StatusDK(intPrice, 80);
                                //2, 3, 1 
                                if (tk4 <= tk3 && tk3 >= tk2 && tk4 >= tk2 && is231) StatusDK(intPrice, 84);
                                //3, 1, 2
                                if (tk4 >= tk3 && tk3 <= tk2 && tk4 >= tk2 && is312) StatusDK(intPrice, 85);
                                //3, 2, 1 not [dk]     add-12/30/14  
                                if (tk4 >= tk3 && tk3 >= tk2 && tk4 >= tk2 && tk4 >= is321_vol[0] && tk3 >= is321_vol[1] && is321) StatusDK(intPrice, 87);//good
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

                    LogUtil.writeLog("~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~");
                    LogUtil.writeLog("  is_1_2_3_Profit = " + is_1_2_3_profit + " [" + String.Join(",", is_1_2_3_al.ToArray()) + "][" + is_1_2_3_al.Count + "]");
                    LogUtil.writeLog("  is_2_3_1_profit = " + is_2_3_1_profit + " [" + String.Join(",", is_2_3_1_al.ToArray()) + "][" + is_2_3_1_al.Count + "]");
                    LogUtil.writeLog("  is_3_1_2_profit = " + is_3_1_2_profit + " [" + String.Join(",", is_3_1_2_al.ToArray()) + "][" + is_3_1_2_al.Count + "]");
                    LogUtil.writeLog("  is_1_3_2_profit = " + is_1_3_2_profit + " [" + String.Join(",", is_1_3_2_al.ToArray()) + "][" + is_1_3_2_al.Count + "]");
                    LogUtil.writeLog("  is_3_2_1_profit = " + is_3_2_1_profit + " [" + String.Join(",", is_3_2_1_al.ToArray()) + "][" + is_3_2_1_al.Count + "]");
                    LogUtil.writeLog("  is_3_3_1_profit = " + is_3_3_1_profit + " [" + String.Join(",", is_3_3_1_al.ToArray()) + "][" + is_3_3_1_al.Count + "]");
                    LogUtil.writeLog("  is987_profit = " + is987_profit + " [" + String.Join(",", is987_al.ToArray()) + "][" + is987_al.Count + "]");
                    LogUtil.writeLog("  is9_1_2_profit = " + is9_1_2_profit + " [" + String.Join(",", is9_1_2_al.ToArray()) + "][" + is9_1_2_al.Count + "]");
                    LogUtil.writeLog("  is98_1_profit = " + is98_1_profit + " [" + String.Join(",", is98_1_al.ToArray()) + "][" + is98_1_al.Count + "]");

                    LogUtil.writeLog("  is123_profit = " + is123_profit + " [" + String.Join(",", is123_al.ToArray()) + "][" + is123_al.Count + "]");
                    LogUtil.writeLog("  is213_profit = " + is213_profit + " [" + String.Join(",", is213_al.ToArray()) + "][" + is213_al.Count + "]");
                    LogUtil.writeLog("  is132_profit = " + is132_profit + " [" + String.Join(",", is132_al.ToArray()) + "][" + is132_al.Count + "]");
                    LogUtil.writeLog("  is231_profit = " + is231_profit + " [" + String.Join(",", is231_al.ToArray()) + "][" + is231_al.Count + "]");
                    LogUtil.writeLog("  is312_profit = " + is312_profit + " [" + String.Join(",", is312_al.ToArray()) + "][" + is312_al.Count + "]");
                    LogUtil.writeLog("  is321_profit = " + is321_profit + " [" + String.Join(",", is321_al.ToArray()) + "][" + is321_al.Count + "]");
                    LogUtil.writeLog("  is331_profit = " + is331_profit + " [" + String.Join(",", is331_al.ToArray()) + "][" + is331_al.Count + "]");
                    LogUtil.writeLog("  is_9_8_7_profit = " + is_9_8_7_profit + " [" + String.Join(",", is_9_8_7_al.ToArray()) + "][" + is_9_8_7_al.Count + "]");
                    LogUtil.writeLog("  is_912_profit = " + is_912_profit + " [" + String.Join(",", is_912_al.ToArray()) + "][" + is_912_al.Count + "]");
                    LogUtil.writeLog("  is_9_81_profit = " + is_9_81_profit + " [" + String.Join(",", is_9_81_al.ToArray()) + "][" + is_9_81_al.Count + "]");
                    //LogUtil.writeLog("  totalProfits = " + totalProfits + " [" + String.Join(",", totalProfits_al.ToArray()) + "]");
                    LogUtil.writeLog("  --totalProfits = " + totalProfits);
                }
            }
            catch (Exception e)
            {
                LogUtil.writeLog(e.Message);
            }
        } 
        #endregion
    }
}
