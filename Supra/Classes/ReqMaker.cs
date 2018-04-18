using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace Supra.Classes
{
    public class ReqMaker
    {
        public string SendAnsi(string data, string url)
        {
            WebRequest req = null;
            HttpWebResponse rsp = null;
            try
            {
                ServicePointManager.ServerCertificateValidationCallback = new RemoteCertificateValidationCallback(IgnoreCertificateErrorHandler);
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
                request.ContentType = "application/x-www-form-urlencoded";
                request.Method = "POST";
                request.KeepAlive = true;

                //byte[] message = new byte[256];
                // fill message
                System.Text.ASCIIEncoding encodedData = new System.Text.ASCIIEncoding();
                byte[] message = encodedData.GetBytes(data);
                // post message
                using (Stream requestStream = request.GetRequestStream())
                {
                    requestStream.Write(message, 0, message.Length);
                    requestStream.Close();
                }
                string respHTML = "";
                HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                using (StreamReader sr = new StreamReader(response.GetResponseStream(), System.Text.Encoding.GetEncoding(1252)))
                {
                    var responseString = sr.ReadToEnd();
                    respHTML = responseString;
                }
                return respHTML;
            }
            catch (WebException webEx)
            {
                return "SendError";
            }
            catch (Exception ex)
            {
                return "SendError";
            }
            finally
            {
                if (rsp != null) rsp.GetResponseStream().Close();
            }
        }

        public string Send(string Url)
        {
            string Out = "";

            string ErrMsg = "";
            WebResponse rsp = null;
            try
            {
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Ssl3 | SecurityProtocolType.Tls12 | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls;
                ServicePointManager.ServerCertificateValidationCallback = new RemoteCertificateValidationCallback(IgnoreCertificateErrorHandler);

                HttpWebRequest req = (HttpWebRequest)WebRequest.Create(Url);
                req.UserAgent = "Mozilla/5.0 (Windows NT 6.1; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/43.0.2357.130 Safari/537.36";
                req.ServicePoint.Expect100Continue = false;
                req.Timeout = 30000;
                req.KeepAlive = false;

                rsp = req.GetResponse();
                using (Stream stream = rsp.GetResponseStream())
                {
                    using (System.IO.StreamReader sr = new System.IO.StreamReader(stream))
                    {
                        Out = sr.ReadToEnd();
                        sr.Close();
                    }
                }
            }
            catch (WebException webEx)
            {
                using (WebResponse response = webEx.Response)
                {
                    HttpWebResponse httpResponse = (HttpWebResponse)response;
                    using (Stream d = response.GetResponseStream())
                    {
                        ErrMsg = new StreamReader(d).ReadToEnd();
                    }
                }

                return ErrMsg;
            }
            catch (Exception ex)
            {
                return "SendError";
            }
            finally
            {
                if (rsp != null) rsp.GetResponseStream().Close();
            }
            return Out;
        }

        public string Send(string data, string url)
        {
            WebRequest req = null;
            HttpWebResponse rsp = null;
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Ssl3 | SecurityProtocolType.Tls12 | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls;
            try
            {
                //ServicePointManager.ServerCertificateValidationCallback = new RemoteCertificateValidationCallback(IgnoreCertificateErrorHandler);
                ServicePointManager.ServerCertificateValidationCallback = new RemoteCertificateValidationCallback(IgnoreCertificateErrorHandler);
                req = WebRequest.Create(url);

                req.Method = "POST";        // Post method
                req.Timeout = 120000;		// 120 sec.

                req.ContentType = "application/x-www-form-urlencoded";
                //((HttpWebRequest)req).UserAgent = "ACBA/1.1/1.0";
                UTF8Encoding encodedData = new UTF8Encoding();
                byte[] byteArray = encodedData.GetBytes(data);
                req.ContentLength = byteArray.Length;
                Stream dataStream = req.GetRequestStream();
                dataStream.Write(byteArray, 0, byteArray.Length);
                dataStream.Close();

                rsp = (HttpWebResponse)req.GetResponse();
                StringBuilder sb = new StringBuilder();

                // used on each read operation
                byte[] buf = new byte[8192];

                //Stream resStream = rsp.GetResponseStream();
                Stream streamResponse = rsp.GetResponseStream();
                StreamReader streamRead = new StreamReader(streamResponse);
                string respHTML = streamRead.ReadToEnd();
                streamRead.Close();
                streamResponse.Close();
                rsp.Close();

                return respHTML;
            }
            catch (WebException webEx)
            {
                return "SendError " + webEx.Message;
            }
            catch (Exception ex)
            {
                return "SendError " + ex.Message;
            }
            finally
            {
                if (rsp != null) rsp.GetResponseStream().Close();
            }
        }

        public string Send(string data, string url, SecurityProtocolType protocol)
        {
            WebRequest req = null;
            HttpWebResponse rsp = null;
            try
            {
                ServicePointManager.ServerCertificateValidationCallback = new RemoteCertificateValidationCallback(IgnoreCertificateErrorHandler);
                ServicePointManager.SecurityProtocol = protocol;
                ServicePointManager.Expect100Continue = true;
                req = WebRequest.Create(url);

                req.Method = "POST";        // Post method
                req.Timeout = 120000;		// 120 sec.

                req.ContentType = "application/x-www-form-urlencoded";
                UTF8Encoding encodedData = new UTF8Encoding();
                byte[] byteArray = encodedData.GetBytes(data);
                req.ContentLength = byteArray.Length;
                Stream dataStream = req.GetRequestStream();
                dataStream.Write(byteArray, 0, byteArray.Length);
                dataStream.Close();

                rsp = (HttpWebResponse)req.GetResponse();
                StringBuilder sb = new StringBuilder();

                // used on each read operation
                byte[] buf = new byte[8192];

                //Stream resStream = rsp.GetResponseStream();
                Stream streamResponse = rsp.GetResponseStream();
                StreamReader streamRead = new StreamReader(streamResponse);
                string respHTML = streamRead.ReadToEnd();
                streamRead.Close();
                streamResponse.Close();
                rsp.Close();

                return respHTML;
            }
            catch (WebException webEx)
            {
                //MessageBox.Show(webEx.InnerException.ToString(), webEx.Message.ToString());
                return "SendError";
            }
            catch (Exception ex)
            {
                //MessageBox.Show(ex.InnerException.ToString(), ex.Message.ToString());
                return "SendError";
            }
            finally
            {
                if (rsp != null) rsp.GetResponseStream().Close();
            }
        }

        public string Send(string data, string url, int TimeOut)
        {
            WebRequest req = null;
            HttpWebResponse rsp = null;
            try
            {
                ServicePointManager.ServerCertificateValidationCallback = new RemoteCertificateValidationCallback(IgnoreCertificateErrorHandler);
                req = WebRequest.Create(url);

                req.Method = "POST";        // Post method
                req.Timeout = TimeOut;

                req.ContentType = "application/x-www-form-urlencoded";
                UTF8Encoding encodedData = new UTF8Encoding();
                byte[] byteArray = encodedData.GetBytes(data);
                req.ContentLength = byteArray.Length;
                Stream dataStream = req.GetRequestStream();
                dataStream.Write(byteArray, 0, byteArray.Length);
                dataStream.Close();

                rsp = (HttpWebResponse)req.GetResponse();
                StringBuilder sb = new StringBuilder();

                // used on each read operation
                byte[] buf = new byte[8192];

                //Stream resStream = rsp.GetResponseStream();
                Stream streamResponse = rsp.GetResponseStream();
                StreamReader streamRead = new StreamReader(streamResponse);
                string respHTML = streamRead.ReadToEnd();
                streamRead.Close();
                streamResponse.Close();
                rsp.Close();

                return respHTML;
            }
            catch (WebException webEx)
            {
                //MessageBox.Show(webEx.InnerException.ToString(), webEx.Message.ToString());
                return "SendError";
            }
            catch (Exception ex)
            {
                //MessageBox.Show(ex.InnerException.ToString(), ex.Message.ToString());
                return "SendError";
            }
            finally
            {
                if (rsp != null) rsp.GetResponseStream().Close();
            }
        }

        public string Send(string data, string url, string crt_file)
        {
            HttpWebRequest req = null;
            HttpWebResponse rsp = null;
            try
            {
                ServicePointManager.Expect100Continue = true;
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Ssl3;
                ServicePointManager.ServerCertificateValidationCallback = new RemoteCertificateValidationCallback(IgnoreCertificateErrorHandler);
                req = (HttpWebRequest)WebRequest.Create(url);

                req.Method = "POST";        // Post method
                req.Timeout = 120000;		// 120 sec.
                //ServicePointManager.CertificatePolicy = new MTG.ArCa.ArCaInterface.TrustAllCertificatePolicy();
                X509Certificate x509 = X509Certificate.CreateFromCertFile(crt_file);
                req.ClientCertificates.Add(x509);
                req.ContentType = "application/x-www-form-urlencoded";
                //((HttpWebRequest)req).UserAgent = "ACBA/1.1/1.0";
                UTF8Encoding encodedData = new UTF8Encoding();
                byte[] byteArray = encodedData.GetBytes(data);
                req.ContentLength = byteArray.Length;
                Stream dataStream = req.GetRequestStream();
                dataStream.Write(byteArray, 0, byteArray.Length);
                dataStream.Close();

                rsp = (HttpWebResponse)req.GetResponse();
                StringBuilder sb = new StringBuilder();

                // used on each read operation
                byte[] buf = new byte[8192];

                //Stream resStream = rsp.GetResponseStream();
                Stream streamResponse = rsp.GetResponseStream();
                StreamReader streamRead = new StreamReader(streamResponse);
                string respHTML = streamRead.ReadToEnd();
                streamRead.Close();
                streamResponse.Close();
                rsp.Close();

                return respHTML;
            }
            catch (WebException webEx)
            {
                //MessageBox.Show(webEx.InnerException.ToString(), webEx.Message.ToString());
                //Response.Write(webEx.Message.ToString());
                return webEx.Message.ToString();
            }
            catch (Exception ex)
            {
                //MessageBox.Show(ex.InnerException.ToString(), ex.Message.ToString());
                //Response.Write(ex.Message.ToString());
                return ex.Message.ToString();
            }
            finally
            {
                if (rsp != null) rsp.GetResponseStream().Close();
            }
        }

        public string Send(string url, string postData, string crt_file, string password)
        {
            string response = string.Empty;
            try
            {
                X509Certificate2 mobiCer = new X509Certificate2(crt_file, password, X509KeyStorageFlags.DefaultKeySet);
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Ssl3 | SecurityProtocolType.Tls | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls12;
                ServicePointManager.ServerCertificateValidationCallback = new System.Net.Security.RemoteCertificateValidationCallback(
                    delegate (object sender1, System.Security.Cryptography.X509Certificates.X509Certificate certificate, X509Chain chain, SslPolicyErrors sslPolicyErrors) { return true; });

                HttpWebRequest webRequest = (HttpWebRequest)WebRequest.Create(url);
                webRequest.ClientCertificates.Add(mobiCer);
                webRequest.Method = "POST";
                webRequest.UserAgent = ".NET Framework 3.5 Client";
                webRequest.ServicePoint.Expect100Continue = true;
                webRequest.ContentType = "application/x-www-form-urlencoded";
                webRequest.Timeout = 120000;

                System.Text.Encoding mASCII = System.Text.Encoding.ASCII;
                byte[] bPostData = mASCII.GetBytes(postData);
                webRequest.ContentLength = bPostData.Length; //bytes.Length;
                using (Stream reqStream = webRequest.GetRequestStream())
                {
                    reqStream.Write(bPostData, 0, bPostData.Length);
                }

                using (HttpWebResponse webResponse = (HttpWebResponse)webRequest.GetResponse())
                {
                    using (Stream respStream = webResponse.GetResponseStream())
                    {
                        using (StreamReader sr = new StreamReader(respStream, System.Text.Encoding.UTF8))
                        {
                            response = sr.ReadToEnd();
                        }
                    }
                }
            }
            catch (WebException webEx)
            {
                return webEx.Message.ToString() + webEx.StackTrace;
            }
            catch (Exception ex)
            {
                return ex.Message.ToString() + ex.StackTrace;
            }
            finally
            {
            }
            return response;
        }

        public string SendPFXPost(string url, string postData, string crt_file, string password)
        {
            string response = string.Empty;
            try
            {
                X509Certificate2 mobiCer = new X509Certificate2(crt_file, password, X509KeyStorageFlags.MachineKeySet);
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Ssl3 | SecurityProtocolType.Tls;
                ServicePointManager.ServerCertificateValidationCallback = new System.Net.Security.RemoteCertificateValidationCallback(
                    delegate (object sender1, System.Security.Cryptography.X509Certificates.X509Certificate certificate, X509Chain chain, SslPolicyErrors sslPolicyErrors) { return true; });

                HttpWebRequest webRequest = (HttpWebRequest)WebRequest.Create(url);
                webRequest.ClientCertificates.Add(mobiCer);
                webRequest.Method = "POST";
                webRequest.UserAgent = ".NET Framework 3.5 Client";
                webRequest.ServicePoint.Expect100Continue = true;
                webRequest.ContentType = "text/xml;";
                webRequest.Timeout = 120000;

                System.Text.Encoding mASCII = System.Text.Encoding.ASCII;
                byte[] bPostData = mASCII.GetBytes(postData);
                webRequest.ContentLength = bPostData.Length; //bytes.Length;
                using (Stream reqStream = webRequest.GetRequestStream())
                {
                    reqStream.Write(bPostData, 0, bPostData.Length);
                }

                using (HttpWebResponse webResponse = (HttpWebResponse)webRequest.GetResponse())
                {
                    using (Stream respStream = webResponse.GetResponseStream())
                    {
                        using (StreamReader sr = new StreamReader(respStream, System.Text.Encoding.UTF8))
                        {
                            response = sr.ReadToEnd();
                        }
                    }
                }
            }
            catch (WebException webEx)
            {
                return webEx.Message.ToString() + webEx.StackTrace;
            }
            catch (Exception ex)
            {
                return ex.Message.ToString() + ex.StackTrace;
            }
            finally
            {
            }
            return response;
        }

        public string SendPFXGet(string Url, string crt_file, string password)
        {
            string Out = "";

            string ErrMsg = "";
            WebResponse rsp = null;
            try
            {
                X509Certificate2 mobiCer = new X509Certificate2(crt_file, password, X509KeyStorageFlags.DefaultKeySet);
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Ssl3 | SecurityProtocolType.Tls;
                ServicePointManager.ServerCertificateValidationCallback = new System.Net.Security.RemoteCertificateValidationCallback(
                    delegate (object sender1, System.Security.Cryptography.X509Certificates.X509Certificate certificate, X509Chain chain, SslPolicyErrors sslPolicyErrors) { return true; });

                HttpWebRequest req = (HttpWebRequest)WebRequest.Create(Url);
                req.ClientCertificates.Add(mobiCer);
                req.UserAgent = "Mozilla/5.0 (Windows NT 6.1; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/43.0.2357.130 Safari/537.36";
                req.ServicePoint.Expect100Continue = false;
                req.Timeout = 30000;
                req.KeepAlive = false;

                rsp = req.GetResponse();
                using (Stream stream = rsp.GetResponseStream())
                {
                    using (System.IO.StreamReader sr = new System.IO.StreamReader(stream))
                    {
                        Out = sr.ReadToEnd();
                        sr.Close();
                    }
                }
            }
            catch (WebException webEx)
            {
                using (WebResponse response = webEx.Response)
                {
                    HttpWebResponse httpResponse = (HttpWebResponse)response;
                    using (Stream d = response.GetResponseStream())
                    {
                        ErrMsg = new StreamReader(d).ReadToEnd();
                    }
                }
                return ErrMsg;
            }
            catch (Exception ex)
            {
                return "SendError " + ex.Message;
            }
            finally
            {
                if (rsp != null) rsp.GetResponseStream().Close();
            }
            return Out;
        }


        public string Send(string data, string url, string useragent, string username, string pass)
        {
            WebRequest req = null;
            HttpWebResponse rsp = null;

            try
            {
                ServicePointManager.ServerCertificateValidationCallback = new RemoteCertificateValidationCallback(delegate (object sender2, X509Certificate certificate, X509Chain chain, SslPolicyErrors sslPolicyErrors) { return true; });
                req = WebRequest.Create(url);

                req.Method = "POST";        // Post method

                ((HttpWebRequest)req).UserAgent = useragent;
                req.ContentType = "text/xml;";
                if (username != "" || pass != "")
                {
                    string authInfo = username + ":" + pass;
                    authInfo = Convert.ToBase64String(ASCIIEncoding.Default.GetBytes(authInfo));
                    req.Headers["Authorization"] = "Basic " + authInfo;
                }
                UTF8Encoding encodedData = new UTF8Encoding();
                byte[] byteArray = encodedData.GetBytes(data);
                req.ContentLength = byteArray.Length;
                Stream dataStream = req.GetRequestStream();
                dataStream.Write(byteArray, 0, byteArray.Length);
                dataStream.Close();

                rsp = (HttpWebResponse)req.GetResponse();
                StringBuilder sb = new StringBuilder();

                // used on each read operation
                byte[] buf = new byte[8192];

                //Stream resStream = rsp.GetResponseStream();
                Stream streamResponse = rsp.GetResponseStream();
                StreamReader streamRead = new StreamReader(streamResponse);
                string respHTML = streamRead.ReadToEnd();
                streamRead.Close();
                streamResponse.Close();
                rsp.Close();

                return respHTML;
            }
            catch (WebException webEx)
            {
                //MessageBox.Show(webEx.InnerException.ToString(), webEx.Message.ToString());
                return "SendError:" + webEx.Message.ToString();
            }
            catch (Exception ex)
            {
                //MessageBox.Show(ex.InnerException.ToString(), ex.Message.ToString());
                return "SendError:" + ex.Message.ToString();
            }
            finally
            {
                if (rsp != null) rsp.GetResponseStream().Close();
            }
        }

        public string Send(string data, string url, string useragent, string username, string pass, string contenttype)
        {
            WebRequest req = null;
            HttpWebResponse rsp = null;

            try
            {
                ServicePointManager.ServerCertificateValidationCallback = new RemoteCertificateValidationCallback(delegate (object sender2, X509Certificate certificate, X509Chain chain, SslPolicyErrors sslPolicyErrors) { return true; });
                req = WebRequest.Create(url);

                req.Method = "POST";        // Post method

                ((HttpWebRequest)req).UserAgent = useragent;
                req.ContentType = contenttype;
                if (username != "" || pass != "")
                {
                    string authInfo = username + ":" + pass;
                    authInfo = Convert.ToBase64String(ASCIIEncoding.Default.GetBytes(authInfo));
                    req.Headers["Authorization"] = "Basic " + authInfo;
                }
                UTF8Encoding encodedData = new UTF8Encoding();
                byte[] byteArray = encodedData.GetBytes(data);
                req.ContentLength = byteArray.Length;
                Stream dataStream = req.GetRequestStream();
                dataStream.Write(byteArray, 0, byteArray.Length);
                dataStream.Close();

                rsp = (HttpWebResponse)req.GetResponse();
                StringBuilder sb = new StringBuilder();

                // used on each read operation
                byte[] buf = new byte[8192];

                //Stream resStream = rsp.GetResponseStream();
                Stream streamResponse = rsp.GetResponseStream();
                StreamReader streamRead = new StreamReader(streamResponse);
                string respHTML = streamRead.ReadToEnd();
                streamRead.Close();
                streamResponse.Close();
                rsp.Close();

                return respHTML;
            }
            catch (WebException webEx)
            {
                //MessageBox.Show(webEx.InnerException.ToString(), webEx.Message.ToString());
                return "SendError:" + webEx.Message.ToString();
            }
            catch (Exception ex)
            {
                //MessageBox.Show(ex.InnerException.ToString(), ex.Message.ToString());
                return "SendError:" + ex.Message.ToString();
            }
            finally
            {
                if (rsp != null) rsp.GetResponseStream().Close();
            }
        }

        


        private bool IgnoreCertificateErrorHandler(object sender, System.Security.Cryptography.X509Certificates.X509Certificate certificate, System.Security.Cryptography.X509Certificates.X509Chain chain, System.Net.Security.SslPolicyErrors sslPolicyErrors)
        {
            return true;
        }
    }
}
