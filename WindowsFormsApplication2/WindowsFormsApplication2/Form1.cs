using System;
using System.Data;
using System.Drawing;
using System.Text;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows.Forms;
using CefSharp;
using CefSharp.WinForms;
using System.Runtime.InteropServices;
using System.IO.Ports;
using System.IO;
using System.Net;


namespace WindowsFormsApplication2
{
    public partial class Form1 : Form
    {
        [DllImport("SPCNSecuCAT.dll", CallingConvention = CallingConvention.StdCall)]
        public static extern int SPCNSecuCAT_Payment(int comport, int baud, byte[] input_msg, byte[] output_msg);

        [DllImport("SPCNSecuCAT.dll", CallingConvention = CallingConvention.StdCall)]
        public static extern int SPCNSecuCAT_GetRemain(int comport, int baud, byte[] output_msg);

        [DllImport("SPCNSecuCAT.dll", CallingConvention = CallingConvention.StdCall)]
        public static extern int SPCNSecuCAT_GetRemain_Sign(int comport, int baud, byte[] output_msg);

        [DllImport("SPCNSecuCAT.dll", CallingConvention = CallingConvention.StdCall)]
        public static extern int SPCNSecuCAT_GetBMPFileFromSignData(int sign_len, byte[] sign_data, byte[] file_path);

        [DllImport("SPCNSecuCAT.dll", CallingConvention = CallingConvention.StdCall)]
        public static extern int SPCNSecuCAT_Print(int com_port, int baud_rate, byte[] input_msg, int input_len, int isHex);

        SerialPort SerialPort;
        string past_suc;
        ChromiumWebBrowser ChromeBrowser;

        public Form1()
        {
            InitializeComponent();

            if (!Cef.IsInitialized)
            {
                initCefSharp();
             //   webLogin();
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            string[] port_list;
            port_list = System.IO.Ports.SerialPort.GetPortNames();

            for (int i = 0; i < port_list.Length; i++)
                combobox_port.Items.Add(port_list[i]);

            if (port_list.Length > 0)
                combobox_port.SelectedIndex = 0;

            combobox_baudrate.SelectedIndex = 1;
            combobox_type.SelectedIndex = 0;
            comboBox_req.SelectedIndex = 0;
            SerialPort = new SerialPort();
        }

        #region Cef 초기화
        /// <summary>
        /// initCefSharp()
        /// </summary>
        public void initCefSharp()
        {
            // 브라우저 설정 초기화
            CefSettings settings = new CefSettings();
        //    settings.CachePath = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData) + @"\CEF";
            Cef.Initialize(settings);
            ChromeBrowser = new ChromiumWebBrowser();
            WindowState = FormWindowState.Maximized;
            ChromeBrowser.Location = new Point(0, 0);

#if DEBUG  
            // Local hosting
            ChromeBrowser.LoadUrl("http://admin1.xmx.kr/");//   
#else
            ChromeBrowser.LoadUrl("http://admin1.xmx.kr/");
#endif

			this.panel1.Controls.Add(ChromeBrowser);
            ChromeBrowser.Dock = DockStyle.Fill;
            ChromeBrowser.LoadingStateChanged += OnLoadingStateChanged;

            ChromeBrowser.JavascriptObjectRepository.Settings.LegacyBindingEnabled = true;
            ChromeBrowser.JavascriptObjectRepository.Register("cAPI", new ChromeAPI(), false, BindingOptions.DefaultBinder);
        }

        #endregion

        private void OnLoadingStateChanged(object sender, LoadingStateChangedEventArgs args)
        {
            if (!args.IsLoading)
            {
                webLogin();
            }
        }

        #region 결제 및 취소 - PaymentProcess
        /// <summary>
        /// PaymentProcess
        /// </summary>
        public void PaymentProcess(string sParam)
        {
            // sParam = S0^3300^1
            int rc = -1;
            string s_comport;
            int com_len;
            int comport;
            int baudrate;
            StringBuilder req = new StringBuilder();
            String rep = "";

            byte[] req_array = new byte[10000];
            byte[] rep_array = new byte[10000];

            req.Clear();
            Array.Clear(req_array, 0, req_array.Length);
            Array.Clear(rep_array, 0, rep_array.Length);
            this.textBox_recv.Text = "";

            string reqType = "ZA";                // 전문구분(Z0, Z6, ZA...)
            string type = "";                   // 거래유형(S0, S4, C0....)
            string sSignSend = string.Empty;    // 서명여부
            string sAmount = string.Empty;      // 금액
            string sInstallCnt = "00";          // 할부개월(00, 03, 06...)

            if (!string.IsNullOrEmpty(sParam) && sParam.Length > 8)
            {
                string[] arrParam = sParam.Split('^');
                type = arrParam[0].ToString();
                sAmount = arrParam[1].ToString();  
             //   sSignSend = arrParam[2].ToString();
                sInstallCnt = arrParam[2].ToString();
           //     reqType = type.Equals("S0") && sSignSend.Equals("1") ? "Z6" : "Z0";
            }
            else
            {
                sAmount = textBox_amount.Text;
                type = combobox_type.SelectedItem.ToString().Substring(0, 2);
                sSignSend = checkBox_sign.Checked ? "1" : "0";
            }

            try
            {
                // 전문구분
                req = insertLeftJustify(req, reqType, 2);

                // 거래유형
                req = insertLeftJustify(req, type, 2);

                // 할부개월
                req = insertLeftJustify(req, " ", 2);

                // 승인금액
                req = insertLeftZero(req, sAmount, 9);

                // 봉사료
                req = insertLeftZero(req, "", 9);

                // 세금
                req = insertLeftZero(req, "", 9);
                // 승인번호
                // 제로페이인 경우 거래고유번호 필드에서 설정
                // 카카오페이인 경우 추가항목 필드에서 설정
                if (type.Equals("Z0") || type.Equals("Z1") || type.Equals("Z2") || type.Equals("Y0") || type.Equals("Y1"))
                    req = insertLeftJustify(req, " ", 12);
                else
                    req = insertLeftJustify(req, " ", 12);
                // 거래일자
                req = insertLeftJustify(req, " ", 6);

                // 포인트거래구분
                req = insertLeftJustify(req, " ", 2);

                // 상품코드
                req = insertLeftJustify(req, " ", 6);

                // 전표인쇄여부
                req = insertLeftJustify(req, " ", 1);

                // 추가항목
                req = insertLeftJustify(req, " ", 300);

                // 서명전송여부, CAT ID
                if (reqType.Equals("Z8") || reqType.Equals("Z9") || reqType.Equals("ZA"))
                {
                    req = insertLeftJustify(req, sSignSend, 1);
                    req = insertLeftJustify(req, "", 10);
                }

                // 거래상세코드, 고객정보, Pos Entry Mode, TRACK DATA
                if (reqType.Equals("Z9") || reqType.Equals("ZA"))
                {
                    switch (type)
                    {
                        case "A2":
                            req = insertLeftJustify(req, "00", 2);
                            req = insertLeftJustify(req, " ", 13);
                            req = insertLeftJustify(req, " ", 2);
                            req = insertLeftJustify(req, " ", 512);
                            break;

                        case "S0":
                        case "S1":
                        case "S2":
                        case "S3":
                        case "S4":
                        case "S5":
                            req = insertLeftJustify(req, " ", 2);
                            req = insertLeftJustify(req, " ", 13);

                            if (string.IsNullOrWhiteSpace(""))
                                req = insertLeftJustify(req, " ", 2);
                            else
                                req = insertLeftJustify(req, "TK", 2);

                            req = insertLeftJustify(req, "", 512);
                            break;

                        case "Z0":
                        case "Z1":
                        case "Z2":
                            req = insertLeftJustify(req, " ", 2);
                            req = insertLeftJustify(req, " ", 13);
                            req = insertLeftJustify(req, " ", 2);
                            req = insertLeftJustify(req, "", 512);
                            break;

                        case "Y0":
                        case "Y1":
                            req = insertLeftJustify(req, " ", 2);
                            req = insertLeftJustify(req, " ", 13);
                            req = insertLeftJustify(req, "04", 2);
                            req = insertLeftJustify(req, "", 512);
                            break;

                        default:
                            req = insertLeftJustify(req, " ", 2);
                            req = insertLeftJustify(req, " ", 13);
                            req = insertLeftJustify(req, " ", 2);
                            req = insertLeftJustify(req, " ", 512);
                            break;
                    }
                }

                // 요청전문 출력
                this.textBox_send.Text = req.ToString();
                req_array = System.Text.Encoding.UTF8.GetBytes(req.ToString());
                
#if DEBUG
                // Test
                s_comport = "COM3";
                com_len = s_comport.Length;
                comport = Convert.ToInt32(s_comport.Substring(3, com_len - 3));  // 3 
                baudrate = 38400;
#else
                s_comport = string.IsNullOrEmpty(combobox_port.SelectedItem.ToString()) ? "COM3" : combobox_port.SelectedItem.ToString();
                com_len = s_comport.Length;        
                comport = Convert.ToInt32(s_comport.Substring(3, com_len - 3));  // 3
                baudrate = string.IsNullOrEmpty(combobox_baudrate.SelectedItem.ToString()) ? 38400 : Convert.ToInt32(combobox_baudrate.SelectedItem.ToString());  // 38400
#endif

                rc = SPCNSecuCAT_Payment(comport, baudrate, req_array, rep_array);
                rep = System.Text.Encoding.Default.GetString(rep_array);

                if (rc < 0)
                {
                    string ret = ErrCodeToMsg(rc);
                    this.textBox_recv.Text = String.Format("오류코드 : {0}, " + ret, rc);
                }
                else
                {
                    //    past_suc = rep;
                    this.textBox_recv.Text = rep;
                    //   ParseResponseData();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }
        #endregion

        #region 결제 버튼 클릭 이벤트
        /// <summary>
        /// BTN_PAYMENT_Click
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void BTN_PAYMENT_Click(object sender, EventArgs e)
        {
            int rc = -1;
            string s_comport;
            int com_len;
            int comport;
            int baudrate;
            StringBuilder req = new StringBuilder();
            String rep = "";

            byte[] req_array = new byte[10000];
            byte[] rep_array = new byte[10000];

            req.Clear();
            Array.Clear(req_array, 0, req_array.Length);
            Array.Clear(rep_array, 0, rep_array.Length);

            this.textBox_recv.Text = "";

            // 210119 PSM 전문구분, 거래유형, 서명전송여부
            string reqType = comboBox_req.SelectedItem.ToString().Substring(0, 2);
            string type = combobox_type.SelectedItem.ToString().Substring(0, 2);
            bool isSignSend = false;

            try
            {
                // 전문구분
                req = insertLeftJustify(req, reqType, 2);

                // 거래유형
                req = insertLeftJustify(req, type, 2);

                // 할부개월
                req = insertLeftJustify(req, " ", 2);

                // 승인금액
                req = insertLeftZero(req, textBox_amount.Text, 9);

                // 봉사료
                req = insertLeftZero(req, "", 9);

                // 세금
                req = insertLeftZero(req, "", 9);
                // 승인번호
                // 제로페이인 경우 거래고유번호 필드에서 설정
                // 카카오페이인 경우 추가항목 필드에서 설정
                if (type.Equals("Z0") || type.Equals("Z1") || type.Equals("Z2") || type.Equals("Y0") || type.Equals("Y1"))
                    req = insertLeftJustify(req, " ", 12);
                else
                    req = insertLeftJustify(req, " ", 12);
                // 거래일자
                req = insertLeftJustify(req, " ", 6);

                // 포인트거래구분
                req = insertLeftJustify(req, " ", 2);

                // 상품코드
                req = insertLeftJustify(req, " ", 6);

                // 전표인쇄여부
                req = insertLeftJustify(req, " ", 1);

                // 추가항목
                req = insertLeftJustify(req, " ", 300);

                // 서명전송여부, CAT ID
                if (reqType.Equals("Z8") || reqType.Equals("Z9") || reqType.Equals("ZA"))
                {
                    req = insertLeftJustify(req, isSignSend ? "1" : "0", 1);
                    req = insertLeftJustify(req, "", 10);
                }

                // 거래상세코드, 고객정보, Pos Entry Mode, TRACK DATA
                if (reqType.Equals("Z9") || reqType.Equals("ZA"))
                {
                    switch (type)
                    {
                        case "A2":
                            req = insertLeftJustify(req, "00", 2);
                            req = insertLeftJustify(req, " ", 13);
                            req = insertLeftJustify(req, " ", 2);
                            req = insertLeftJustify(req, " ", 512);
                            break;

                        case "S0":
                        case "S1":
                        case "S2":
                        case "S3":
                        case "S4":
                        case "S5":
                            req = insertLeftJustify(req, " ", 2);
                            req = insertLeftJustify(req, " ", 13);

                            if (string.IsNullOrWhiteSpace(""))
                                req = insertLeftJustify(req, " ", 2);
                            else
                                req = insertLeftJustify(req, "TK", 2);

                            req = insertLeftJustify(req, "", 512);
                            break;

                        case "Z0":
                        case "Z1":
                        case "Z2":
                            req = insertLeftJustify(req, " ", 2);
                            req = insertLeftJustify(req, " ", 13);
                            req = insertLeftJustify(req, " ", 2);
                            req = insertLeftJustify(req, "", 512);
                            break;

                        case "Y0":
                        case "Y1":
                            req = insertLeftJustify(req, " ", 2);
                            req = insertLeftJustify(req, " ", 13);
                            req = insertLeftJustify(req, "04", 2);
                            req = insertLeftJustify(req, "", 512);
                            break;

                        default:
                            req = insertLeftJustify(req, " ", 2);
                            req = insertLeftJustify(req, " ", 13);
                            req = insertLeftJustify(req, " ", 2);
                            req = insertLeftJustify(req, " ", 512);
                            break;
                    }
                }

                //#if DEBUG
                //                string sSendData = string.Empty;
                //                sSendData = UsUtil.RPadH("Z0S0  000003300000000300000000000", 360);
                //                //string sIp = "192.168.0.3";

                //                // 요청전문 출력
                //                textBox_send.Text = sSendData;
                //                req_array = System.Text.Encoding.UTF8.GetBytes(sSendData);

                //                comport = 3;
                //                baudrate = Convert.ToInt32(combobox_baudrate.SelectedItem.ToString());
                //                rc = SPCNSecuCAT_Payment(comport, baudrate, req_array, rep_array);
                //#else
                // 요청전문 출력
                textBox_send.Text = req.ToString();
                req_array = System.Text.Encoding.UTF8.GetBytes(req.ToString());

                s_comport = combobox_port.SelectedItem.ToString();
                com_len = s_comport.Length;
                comport = Convert.ToInt32(s_comport.Substring(3, com_len - 3));
                baudrate = Convert.ToInt32(combobox_baudrate.SelectedItem.ToString());
                rc = SPCNSecuCAT_Payment(comport, baudrate, req_array, rep_array);
                //#endif

                rep = System.Text.Encoding.Default.GetString(rep_array);

                if (rc < 0)
                {
                    string ret = ErrCodeToMsg(rc);
                    textBox_recv.Text = String.Format("오류코드 : {0}, " + ret, rc);
                }
                else
                {
                //    past_suc = rep;
                    textBox_recv.Text = rep;
                 //   ParseResponseData();

                    // Web API 호출
                   // callWebApi();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }
        #endregion

        #region Web API 호출 - 미사용
        /// <summary>
        /// callWebApi
        /// </summary>
        private void callWebApi()
        {
            //string sUrl = "https://localhost:49862/";
            string sUrl = "https://localhost:7256/M_Payment";
            string sData = "{ \"sAuthNo\": \"999\", \"iPaymentAmt\" : \"1100\", \"iPaymentType\" : \"11\",  \"err_code\" : \"99\", \"err_msg\" : \"failed\",}";
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(sUrl);
            request.Method = "POST";
            request.ContentType = "application/json";
            request.Timeout = 30 * 1000;

            // Request Stream
            byte[] bytes = Encoding.ASCII.GetBytes(sData);
            request.ContentLength = bytes.Length; // 바이트수 지정

            using (Stream reqStream = request.GetRequestStream())
            {
                reqStream.Write(bytes, 0, bytes.Length);
            }

            // Response
            string responseText = string.Empty;
            using (WebResponse resp = request.GetResponse())
            {
                Stream respStream = resp.GetResponseStream();
                using (StreamReader sr = new StreamReader(respStream))
                {
                    responseText = sr.ReadToEnd();
                }
            }
        }
        #endregion
            
        #region 로그인 처리(윈폼 -> 웹 페이지 함수 호출) 
        /// <summary>
        /// 로그인 처리
        /// </summary>
        public async void webLogin()
        {
            await Task.Delay(1500);

            // 로그인처리
            string sId = "JHTest01";
            string sPwd = "test01";
            ChromeBrowser.ExecuteScriptAsync("document.getElementById('txtUseId').value=" + '\'' + sId + '\'');
            ChromeBrowser.ExecuteScriptAsync("document.getElementById('txtPwd').value=" + '\'' + sPwd + '\'');

            await Task.Delay(1000);
            ChromeBrowser.ExecuteScriptAsync("document.getElementById('btnLogin').submit();");
            
        }
        #endregion

        #region 에러코드 메세지
        /// <summary>
        /// 에러코드 메세지
        /// </summary>
        /// <param name="rc"></param>
        /// <returns></returns>
        public string ErrCodeToMsg(int rc)
        {
            string outdd = string.Empty;

            switch (rc)
            {
                case -101: outdd = "시리얼포트번호오류"; break;
                case -102: outdd = "통신속도오류"; break;
                case -103: outdd = "전문길이오류"; break;
                case -210: outdd = "할부개월오류"; break;
                case -211: outdd = "금액봉사료세금오류"; break;
                case -212: outdd = "원거래일자오류"; break;
                case -213: outdd = "포인트거래구분오류"; break;
                case -301: outdd = "거래중카드제거"; break;
                case -302: outdd = "단말기 사용자 강제종료"; break;
                case -303: outdd = "기타단말기오류"; break;
                case -304: outdd = "카드삽입되어있음"; break;
                case -305: outdd = "사용자강제취소"; break;
                case -401: outdd = "시리얼포트오픈오류"; break;
                case -402: outdd = "시리얼쓰기오류"; break;
                case -403: outdd = "시리얼읽기오류"; break;
                case -404: outdd = "시리얼닫기오류"; break;
                case -405: outdd = "시리얼타임아웃"; break;
                case -411: outdd = "미전송내역없음"; break;
                default: outdd = "정의되어 있지 않은 메세지" + "[" + rc + "]"; break;
            }
            return outdd;
        }
        #endregion

        #region ParseResponseData
        private void ParseResponseData()
        {
            byte[] encodingBytes = Encoding.Default.GetBytes(past_suc);

            // 원거래 승인번호
            // 제로페이인 경우 거래고유번호
            string type = Encoding.Default.GetString(encodingBytes, 4, 2);
            if (type.Equals("Z0") || type.Equals("Z1") || type.Equals("Z2"))
            {
                if (checkBox_sign.Checked)
                    textBox_auth_number.Text = Encoding.Default.GetString(encodingBytes, 2656, 20);
                else
                    textBox_auth_number.Text = Encoding.Default.GetString(encodingBytes, 608, 20);
            }
            else
            {
                textBox_auth_number.Text = Encoding.Default.GetString(encodingBytes, 161, 12);
            }
        }
        #endregion

        #region insertLeftJustify - 왼쪽정렬함수
        // 왼쪽정렬함수
        private StringBuilder insertLeftJustify(StringBuilder target, string item, int maxLen)
        {
            int myLen = maxLen;

            if (item.Length < maxLen)
            {
                target.Append(item);
                myLen = myLen - item.Length;

                for (int i = 0; i < myLen; i++)
                    target.Append(" ");   

                return target;
            }
            else if (item.Length == maxLen)
            {
                target.Append(item);
                return target;
            }
            else
            {
                for (int i = 0; i < myLen; i++)
                    target.Append(item[i]);
                
                return target;
            }
        }
        #endregion

        #region insertLeftZero - 왼쪽0채움
        // 왼쪽0채움
        private StringBuilder insertLeftZero(StringBuilder target, string item, int maxLen)
        {
            int myLen = maxLen;

            if (item.Length < maxLen)
            {
                myLen = myLen - item.Length;
                for (int i = 0; i < myLen; i++)
                    target.Append("0");

                target.Append(item);
                return target;
            }
            else if (item.Length == maxLen)
            {
                target.Append(item);
                return target;
            }
            else
            {
                for (int i = 0; i < myLen; i++)
                    target.Append(item[i]);

                return target;
            }
        }
        #endregion

        #region 윈폼 웹 컨트롤 이벤트

        private void btnBack_Click(object sender, EventArgs e)
        {
            ChromeBrowser.BrowserCore.GoBack();
        }

        private void btnNext_Click(object sender, EventArgs e)
        {
            ChromeBrowser.BrowserCore.GoForward();
        }

        private void btnReload_Click(object sender, EventArgs e)
        {
            ChromeBrowser.BrowserCore.Reload();
        }

        #endregion

    }


    public class ChromeAPI
    {

        public void showMsg(string sParam)
        {
            if (Cef.IsInitialized)
            {
                Form1 form = new Form1();
                form.PaymentProcess(sParam); 
            }
            else
            {
                ChromiumWebBrowser ChromeBrowser = new ChromiumWebBrowser();
                CefSettings cefSettings = new CefSettings();

                Cef.Initialize(cefSettings);
                MessageBox.Show("IsInitialized is required");
            }
        }
    }
}
