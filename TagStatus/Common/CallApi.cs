//===========================================================================
// 
//    Copyright 1993-2011, Bechtel Corporation.  All rights reserved.
//    This software and its associated documentation are confidential,
//    proprietary information of Bechtel Corporation, its subsidiaries,
//    and affiliates, and may not be used or reproduced outside of Bechtel
//    pursuant to a written license agreement.
// 
//    $Author: avaranas $
//    $Date: 2012-02-22 17:36:58 -0500 (Wed, 22 Feb 2012) $
//    $Revision: 1877 $
//    $HeadURL: https://svncorp.becpsn.com/svn/bps/trunk/BPS_5.1/WebProjects/services/API/Api_Demo_WebApp/CallApi.cs $
// 
//===========================================================================
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Configuration;
using System.Data;
using System.Net;
using System.IO;
using System.Xml;
using System.Text;

namespace TagStatus
{
	public class CallApi
	{
		public static bool ConnectCallComplete = false;
		public string ApiUrl { get; set; }
		public string Format { get; set; }
		public string Page { get; set; }
		public string PageSize { get; set; }
		public string Filter { get; set; }
		public string OrderBy { get; set; }

		public CallApi()
		{
		}
		public DataSet RetrieveData(ref string ErrorMsg, ref int intRecordCount)
		{
			intRecordCount = -1;
			HttpWebRequest request = null;
			string strXML = "";
			DataSet ds = null;

			if (String.IsNullOrEmpty(ApiUrl))
			{
				ErrorMsg = "No Api Url specified";
				return null;
			}

			#region Build query string for the Api call

			string strQueryString = "";

			if (!String.IsNullOrEmpty(Format))
			{
				if (strQueryString != "") strQueryString += "&";
				strQueryString += "format=" + Format;
			}
			if (!String.IsNullOrEmpty(Page))
			{
				if (strQueryString != "") strQueryString += "&";
				strQueryString += "start=" + Page;
			}
			if (!String.IsNullOrEmpty(PageSize))
			{
				if (strQueryString != "") strQueryString += "&";
				strQueryString += "limit=" + PageSize;
			}
			if (!String.IsNullOrEmpty(Filter))
			{
				if (strQueryString != "") strQueryString += "&";
				strQueryString += "filter=" + Filter;
			}
			if (!String.IsNullOrEmpty(OrderBy))
			{
				if (strQueryString != "") strQueryString += "&";
				strQueryString += "sortby=" + OrderBy;
			}

			if (strQueryString != "")
			{
				if (ApiUrl.Contains("?"))
				{
					if (ApiUrl.EndsWith("?"))
					{
						ApiUrl += strQueryString;
					}
					else
					{
						ApiUrl += "&" + strQueryString;
					}
				}
				else
				{
					ApiUrl += "?" + strQueryString;
				}
			}

			#endregion

			#region Make the connect call...if not already made

			if (ConnectCallComplete == false)
			{
				try
				{
					string strSessionResponse;
					string strJSON = "{ \"LicenseKey\": \"" + ConfigurationManager.AppSettings["BpsApiLicense"] + "\" }";
					byte[] abyte = Encoding.ASCII.GetBytes(strJSON);

					request = WebRequest.Create(ConfigurationManager.AppSettings["Url_Connect"]) as HttpWebRequest;
					request.UseDefaultCredentials = true;
					request.Headers.Add("X-myPSN-EmailAddress", "kenny");
					request.Headers.Add("X-myPSN-UserAttributes", "{ \"EMailAddress\": \"kenny\" }");
					request.Method = "POST";
					request.ContentType = "application/x-www-form-urlencoded";
					request.ContentLength = abyte.Length;

					// Write data to request
					Stream dataStream = request.GetRequestStream();
					dataStream.Write(abyte, 0, abyte.Length);
					dataStream.Close();

					// Get response  
					using (HttpWebResponse response = request.GetResponse() as HttpWebResponse)
					{
						// Get the response stream  
						StreamReader reader = new StreamReader(response.GetResponseStream());
						strSessionResponse = reader.ReadToEnd();
					}

					if (strSessionResponse == "{ \"Status\": \"OK\" }")
					{
						ConnectCallComplete = true;
					}
					else
					{
						ErrorMsg = strSessionResponse;
						return null;
					}
				}
				catch
				{
					ErrorMsg = "Unable to create Api session";
					return null;
				}
			}

			#endregion

			#region Prepare the request

			try
			{
				request = WebRequest.Create(ApiUrl) as HttpWebRequest;
				request.UseDefaultCredentials = true;
				request.Headers.Add("X-myPSN-BechtelUserName", "kbabu");
				request.Headers.Add("X-myPSN-EmailAddress", "kbabu@bechtel.com");
			}
			catch
			{
				ErrorMsg = "Unable to execute the Api to retrieve Information";
				return null;
			}

			#endregion

			#region Process api response

			try
			{
				using (HttpWebResponse response = request.GetResponse() as HttpWebResponse)
				{
					// Get the response stream  
					StreamReader reader = new StreamReader(response.GetResponseStream());
					strXML = reader.ReadToEnd();
				}

				XmlDocument xDoc = new XmlDocument();
				xDoc.LoadXml(strXML);

				if (xDoc.SelectSingleNode("response/success").InnerText.ToUpper() == "TRUE")
				{
					// Successful api call
					ds = XmltoDataSet(xDoc);

					int intTableIndexToUse = 3;

					if (ds.Tables != null && ds.Tables.Count > 1)
					{
						if (ds.Tables[intTableIndexToUse] != null
							&& ds.Tables[intTableIndexToUse].Columns.Count > 0
							&& ds.Tables[intTableIndexToUse].Rows.Count > 0)
						{
							if (ds.Tables[intTableIndexToUse].Columns.Contains("ERROR"))
							{
								// There was an error
								ErrorMsg = "Error retrieving information";
								return null;
							}
							else if (ds.Tables[intTableIndexToUse].Columns.Contains("INFO"))
							{
								// There was an error
								ErrorMsg = ds.Tables[intTableIndexToUse].Rows[0]["INFO"].ToString();
								return null;
							}
						}

						// Get record count
						try
						{
							intRecordCount = Convert.ToInt32(xDoc.GetElementsByTagName("TotalRows")[0].InnerText);
						}
						catch
						{
							intRecordCount = -1;
						}
					}
				}
				else
				{
					// Failed api call
					ErrorMsg = xDoc.SelectSingleNode("response/status_text").InnerText;
					return null;
				}
			}
			catch
			{
				ErrorMsg = "Unable to fetch information";
				return null;
			}

			#endregion

			ErrorMsg = "";
			return ds;
		}
        public string RetrieveJSON()
        {
            HttpWebRequest request = null;
            string strResJSON = "";

            if (String.IsNullOrEmpty(ApiUrl))
            {
                return null;
            }

            #region Build query string for the Api call

            string strQueryString = "";

            if (!String.IsNullOrEmpty(Format))
            {
                if (strQueryString != "") strQueryString += "&";
                strQueryString += "format=" + Format;
            }
            if (!String.IsNullOrEmpty(Page))
            {
                if (strQueryString != "") strQueryString += "&";
                strQueryString += "start=" + Page;
            }
            if (!String.IsNullOrEmpty(PageSize))
            {
                if (strQueryString != "") strQueryString += "&";
                strQueryString += "limit=" + PageSize;
            }
            if (!String.IsNullOrEmpty(Filter))
            {
                if (strQueryString != "") strQueryString += "&";
                strQueryString += "filter=" + Filter;
            }
            if (!String.IsNullOrEmpty(OrderBy))
            {
                if (strQueryString != "") strQueryString += "&";
                strQueryString += "sortby=" + OrderBy;
            }

            if (strQueryString != "")
            {
                if (ApiUrl.Contains("?"))
                {
                    if (ApiUrl.EndsWith("?"))
                    {
                        ApiUrl += strQueryString;
                    }
                    else
                    {
                        ApiUrl += "&" + strQueryString;
                    }
                }
                else
                {
                    ApiUrl += "?" + strQueryString;
                }
            }

            #endregion

            #region Make the connect call...if not already made

            if (ConnectCallComplete == false)
            {
                try
                {
                    string strSessionResponse;
                    string strJSON = "{ \"LicenseKey\": \"" + ConfigurationManager.AppSettings["BpsApiLicense"] + "\" }";
                    byte[] abyte = Encoding.ASCII.GetBytes(strJSON);

                    request = WebRequest.Create(ConfigurationManager.AppSettings["Url_Connect"]) as HttpWebRequest;
                    request.UseDefaultCredentials = true;
                    request.Headers.Add("X-myPSN-EmailAddress", "kenny");
                    request.Headers.Add("X-myPSN-UserAttributes", "{ \"EMailAddress\": \"kenny\" }");
                    request.Method = "POST";
                    request.ContentType = "application/x-www-form-urlencoded";
                    request.ContentLength = abyte.Length;

                    // Write data to request
                    Stream dataStream = request.GetRequestStream();
                    dataStream.Write(abyte, 0, abyte.Length);
                    dataStream.Close();

                    // Get response  
                    using (HttpWebResponse response = request.GetResponse() as HttpWebResponse)
                    {
                        // Get the response stream  
                        StreamReader reader = new StreamReader(response.GetResponseStream());
                        strSessionResponse = reader.ReadToEnd();
                    }

                    if (strSessionResponse == "{ \"Status\": \"OK\" }")
                    {
                        ConnectCallComplete = true;
                    }
                    else
                    {
                        //ErrorMsg = strSessionResponse;
                        return null;
                    }
                }
                catch
                {
                    //ErrorMsg = "Unable to create Api session";
                    return null;
                }
            }

            #endregion

            #region Prepare the request

            try
            {
                request = WebRequest.Create(ApiUrl) as HttpWebRequest;
                request.UseDefaultCredentials = true;
                request.Headers.Add("X-myPSN-BechtelUserName", "kbabu");
                request.Headers.Add("X-myPSN-EmailAddress", "kbabu@bechtel.com");
            }
            catch
            {
                //ErrorMsg = "Unable to execute the Api to retrieve Information";
                return null;
            }

            #endregion

            #region Process api response

            try
            {
                using (HttpWebResponse response = request.GetResponse() as HttpWebResponse)
                {
                    // Get the response stream  
                    StreamReader reader = new StreamReader(response.GetResponseStream());
                    strResJSON = reader.ReadToEnd();
                }

            }
            catch
            {
                return null;
            }

            #endregion

            return strResJSON;
        }

		private DataSet XmltoDataSet(XmlDocument doc)
		{
			DataSet ds = new DataSet();
			byte[] buf = System.Text.ASCIIEncoding.ASCII.GetBytes(doc.OuterXml);
			System.IO.MemoryStream ms = new System.IO.MemoryStream(buf);

			ds.ReadXml(ms);
			ms.Close();

			return ds;
		}
	}
}
