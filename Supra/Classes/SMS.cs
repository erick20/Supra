using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Xml;

namespace Supra.Classes
{
    public class SMS
    {
        public string ParsePhone(string p_text)
        {
            try
            {
                p_text = p_text.Replace(" ", String.Empty);
                p_text = p_text.Replace("+", String.Empty);
                while (p_text[0] == '0') // first character
                    p_text = p_text.Remove(0, 1);

                string prefix = p_text.Substring(0, 2);
                if (p_text.Length == 8)
                {
                    if (prefix == "91" || prefix == "93" || prefix == "94" || prefix == "95" || prefix == "96" || prefix == "97" || prefix == "98" || prefix == "99" || prefix == "77" || prefix == "55" || prefix == "41" || prefix == "43" || prefix == "44")
                    {
                        p_text = "374" + prefix + p_text.Substring(2, p_text.Length - 2);
                    }
                }
                else
                {
                    if (p_text.Length > 10)
                    {
                        if (p_text.StartsWith("374"))
                        {
                            p_text = p_text.Substring(3, p_text.Length - 3);
                            p_text = p_text.TrimStart('0');
                            p_text = "374" + p_text;
                        }
                    }
                }
            }
            catch { }
            if (!Methods.IsNumeric(p_text))
                return "";

            return p_text;
        }
                
        public bool SendSMSParadox(string text, string surl, string slogin, string spassword, string phone, string sName)
        {
            text = text.Replace("\r", "");

            //ServicePointManager.CertificatePolicy = new MyCertificatePolicy();
            ReqMaker req = new ReqMaker();

            string Data = "<?xml version=\"1.0\" encoding=\"UTF-8\"?><xml_request name=\"sms_send\"><xml_user lgn=\"" + slogin + "\" pwd=\"" + spassword + "\"/><sms sms_id=\"" + (new Random()).Next(0, 999999999) + "\" number=\"" + phone + "\" source_number=\"" + sName + "\" ttl=\"10\">" + text + "</sms></xml_request>";
            string resp = req.Send(Data, surl);
            XmlDocument doc = new XmlDocument();
            doc.LoadXml(resp);
            string attrVal = doc.SelectSingleNode("/xml_result/@res").Value;
            //if (resp.IndexOf("<xml_result name=\"sms_send\" res=\"0\"") != -1)
            if (attrVal == "0")
            {
                return true;
            }
            else
            {
                //SendAlert(resp);
                return false;
            }

        }

        public bool SendSMSParadoxInt(string text, string surl, string slogin, string spassword, string phone)
        {
            text = text.Replace("\r", "");

            //ServicePointManager.CertificatePolicy = new MyCertificatePolicy();
            ReqMaker req = new ReqMaker();

            string Data = "{\"from\":\"Idram\",\"to\":\"" + phone + "\",\"text\":\"" + text + "\"}";

            string resp = req.Send(Data, surl, "", slogin, spassword, "application/json");


            if (resp.IndexOf("\"description\":\"Message accepted\"") != -1 || resp.IndexOf("\"description\":\"Message sent to next instance\"") != -1)
            {
                return true;
            }
            else
            {
                Methods.Logger(Data, resp);
                return false;
            }

        }
    }
}
