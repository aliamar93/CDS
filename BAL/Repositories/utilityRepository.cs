using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using DAL.Models;
using DAL.DBEntities;

namespace BAL.Repositories
{
    /// <summary>
    /// AUTHOR      : Fateh Ali Shah
    /// Description : Utility repository is created for any .net web application which
    /// provides Encryption, api call, Fire Base Notification and other help full mehtods
    /// for web application. This class includes synchronous and asynchronous methods.
    /// </summary>
    public class utilityRepository
    {
        public static string fcmKey = ConfigurationManager.AppSettings["FCMKey"].ToString();
        public static string fcmSenderID = "32081778500";
        public static string key = ConfigurationManager.AppSettings["KeySec"].ToString();

        //Containst only encryption method not decryption
        #region OnlyForPasswordUse
        public enum HashName
        {
            SHA1 = 1,
            MD5 = 2,
            SHA256 = 4,
            SHA384 = 8,
            SHA512 = 16
        }

        public static string ComputeHash(string plainText)
        {
            return ComputeHash(plainText, key, HashName.MD5);
        }

        private static string ComputeHash(string plainText, string salt, HashName hashName)
        {
            if (!string.IsNullOrEmpty(plainText))
            {
                // Convert plain text into a byte array. 
                byte[] plainTextBytes = ASCIIEncoding.ASCII.GetBytes(plainText);
                // Allocate array, which will hold plain text and salt. 
                byte[] plainTextWithSaltBytes = null;
                byte[] saltBytes;
                if (!string.IsNullOrEmpty(salt))
                {
                    // Convert salt text into a byte array. 
                    saltBytes = ASCIIEncoding.ASCII.GetBytes(salt);
                    plainTextWithSaltBytes =
                        new byte[plainTextBytes.Length + saltBytes.Length];
                }
                else
                {
                    // Define min and max salt sizes. 
                    int minSaltSize = 4;
                    int maxSaltSize = 8;
                    // Generate a random number for the size of the salt. 
                    Random random = new Random();
                    int saltSize = random.Next(minSaltSize, maxSaltSize);
                    // Allocate a byte array, which will hold the salt. 
                    saltBytes = new byte[saltSize];
                    // Initialize a random number generator. 
                    RNGCryptoServiceProvider rngCryptoServiceProvider =
                                new RNGCryptoServiceProvider();
                    // Fill the salt with cryptographically strong byte values. 
                    rngCryptoServiceProvider.GetNonZeroBytes(saltBytes);
                }
                // Copy plain text bytes into resulting array. 
                for (int i = 0; i < plainTextBytes.Length; i++)
                {
                    plainTextWithSaltBytes[i] = plainTextBytes[i];
                }
                // Append salt bytes to the resulting array. 
                for (int i = 0; i < saltBytes.Length; i++)
                {
                    plainTextWithSaltBytes[plainTextBytes.Length + i] =
                                        saltBytes[i];
                }
                HashAlgorithm hash = null;
                switch (hashName)
                {
                    case HashName.SHA1:
                        hash = new SHA1Managed();
                        break;
                    case HashName.SHA256:
                        hash = new SHA256Managed();
                        break;
                    case HashName.SHA384:
                        hash = new SHA384Managed();
                        break;
                    case HashName.SHA512:
                        hash = new SHA512Managed();
                        break;
                    case HashName.MD5:
                        hash = new MD5CryptoServiceProvider();
                        break;
                }
                // Compute hash value of our plain text with appended salt. 
                byte[] hashBytes = hash.ComputeHash(plainTextWithSaltBytes);
                // Create array which will hold hash and original salt bytes. 
                byte[] hashWithSaltBytes =
                    new byte[hashBytes.Length + saltBytes.Length];
                // Copy hash bytes into resulting array. 
                for (int i = 0; i < hashBytes.Length; i++)
                {
                    hashWithSaltBytes[i] = hashBytes[i];
                }
                // Append salt bytes to the result. 
                for (int i = 0; i < saltBytes.Length; i++)
                {
                    hashWithSaltBytes[hashBytes.Length + i] = saltBytes[i];
                }
                // Convert result into a base64-encoded string. 
                string hashValue = Convert.ToBase64String(hashWithSaltBytes);
                // Return the result. 
                return hashValue;
            }
            return string.Empty;
        }

        #endregion


        //Tested Encryption and Decryption
        #region Encryption

        public static string Encrypt(string plainText)
        {
            return Encrypt(plainText, key, true);
        }
        public static string Decrypt(string encryptedText)
        {
            return Decrypt(encryptedText, key, true);
        }

        public static string Encrypt(string toEncrypt, string key, bool useHashing)
        {
            byte[] keyArray;
            byte[] toEncryptArray = UTF8Encoding.UTF8.GetBytes(toEncrypt);
            // Get the key from config file

            //System.Windows.Forms.MessageBox.Show(key);
            //If hashing use get hashcode regards to your key
            if (useHashing)
            {
                MD5CryptoServiceProvider hashmd5 = new MD5CryptoServiceProvider();
                keyArray = hashmd5.ComputeHash(UTF8Encoding.UTF8.GetBytes(key));
                //Always release the resources and flush data
                // of the Cryptographic service provide. Best Practice

                hashmd5.Clear();
            }
            else
                keyArray = UTF8Encoding.UTF8.GetBytes(key);

            TripleDESCryptoServiceProvider tdes = new TripleDESCryptoServiceProvider();
            //set the secret key for the tripleDES algorithm
            tdes.Key = keyArray;
            //mode of operation. there are other 4 modes.
            //We choose ECB(Electronic code Book)
            tdes.Mode = CipherMode.ECB;
            //padding mode(if any extra byte added)

            tdes.Padding = PaddingMode.PKCS7;

            ICryptoTransform cTransform = tdes.CreateEncryptor();
            //transform the specified region of bytes array to resultArray
            byte[] resultArray =
              cTransform.TransformFinalBlock(toEncryptArray, 0,
              toEncryptArray.Length);
            //Release resources held by TripleDes Encryptor
            tdes.Clear();
            //Return the encrypted data into unreadable string format
            return Convert.ToBase64String(resultArray, 0, resultArray.Length);
        }


        public static string Decrypt(string cipherString, string key, bool useHashing)
        {
            byte[] keyArray;
            //get the byte code of the string

            byte[] toEncryptArray = Convert.FromBase64String(cipherString);

            if (useHashing)
            {
                //if hashing was used get the hash code with regards to your key
                MD5CryptoServiceProvider hashmd5 = new MD5CryptoServiceProvider();
                keyArray = hashmd5.ComputeHash(UTF8Encoding.UTF8.GetBytes(key));
                //release any resource held by the MD5CryptoServiceProvider

                hashmd5.Clear();
            }
            else
            {
                //if hashing was not implemented get the byte code of the key
                keyArray = UTF8Encoding.UTF8.GetBytes(key);
            }

            TripleDESCryptoServiceProvider tdes = new TripleDESCryptoServiceProvider();
            //set the secret key for the tripleDES algorithm
            tdes.Key = keyArray;
            //mode of operation. there are other 4 modes. 
            //We choose ECB(Electronic code Book)

            tdes.Mode = CipherMode.ECB;
            //padding mode(if any extra byte added)
            tdes.Padding = PaddingMode.PKCS7;

            ICryptoTransform cTransform = tdes.CreateDecryptor();
            byte[] resultArray = cTransform.TransformFinalBlock(
                                 toEncryptArray, 0, toEncryptArray.Length);
            //Release resources held by TripleDes Encryptor                
            tdes.Clear();
            //return the Clear decrypted TEXT
            return UTF8Encoding.UTF8.GetString(resultArray);
        }

        #endregion

        //getResponse generic overload method region
        #region getResponse
        public static T getResponse<T>(string url, string jsonData, bool isPost, Dictionary<string, string> headerKeys)
        {
            string responseText = string.Empty;
            try
            {

                string methodType = isPost == true ? "POST" : "GET";
                string newUrl = !isPost ? url + "?" + jsonData : url;



                System.Net.HttpWebRequest request = (HttpWebRequest)WebRequest.Create(string.Format(newUrl));

                request.Method = methodType;
                if (headerKeys != null && headerKeys.Count > 0)
                {
                    foreach (var keyPair in headerKeys)
                    {
                        if (!string.IsNullOrEmpty(keyPair.Value))
                        {
                            request.Headers.Add(keyPair.Key, keyPair.Value);
                        }
                        else
                        {
                            request.Headers.Add(keyPair.Key);
                        }
                    }
                }
                if (isPost)
                {
                    request.ContentType = "application/json";
                    // request.ContentType = "application/x-www-form-urlencoded";

                    using (var sw = new StreamWriter(request.GetRequestStream()))
                    {
                        string json = jsonData;
                        sw.Write(json);
                        sw.Flush();
                    }
                }
                var response = (HttpWebResponse)request.GetResponse();
                var encoding = ASCIIEncoding.ASCII;
                responseText = string.Empty;
                using (var reader = new System.IO.StreamReader(response.GetResponseStream(), encoding))
                {
                    responseText = reader.ReadToEnd();
                }
            }
            catch (Exception ex)
            {
                string errMsg = string.Empty;
                if (ex.Message.Contains("See the inner exception for details."))
                {
                    errMsg = ex.InnerException.Message;
                    if (errMsg.Contains("See the inner exception for details."))
                    {
                        errMsg = ex.InnerException.InnerException.Message;
                    }
                }
                else
                {
                    errMsg = ex.Message;
                }
                responseText = "{\"Code\":401,\"Message\":\"" + errMsg + "\"}";
            }

            //JsonConvert responseText
            object vm;
            try
            {
                vm = Newtonsoft.Json.JsonConvert.DeserializeObject<T>(responseText);
            }
            catch (Exception ex)
            {

                responseText = "{\"Code\":500,\"Message\":\"Error in parsing object.\"}";
                vm = Newtonsoft.Json.JsonConvert.DeserializeObject<T>(responseText);
            }

            return (T)vm;
        }

        public static async Task<T> getAsyncResponse<T>(string url, string jsonData, bool isPost, Dictionary<string, string> headerKeys)
        {
            string responseText = string.Empty;
            try
            {

                string methodType = isPost == true ? "POST" : "GET";
                string newUrl = !isPost ? url + "?" + jsonData : url;



                System.Net.HttpWebRequest request = (HttpWebRequest)WebRequest.Create(string.Format(newUrl));

                request.Method = methodType;
                if (headerKeys != null && headerKeys.Count > 0)
                {
                    foreach (var keyPair in headerKeys)
                    {
                        if (!string.IsNullOrEmpty(keyPair.Value))
                        {
                            request.Headers.Add(keyPair.Key, keyPair.Value);
                        }
                        else
                        {
                            request.Headers.Add(keyPair.Key);
                        }
                    }
                }
                if (isPost)
                {
                    request.ContentType = "application/json";
                    // request.ContentType = "application/x-www-form-urlencoded";

                    using (var sw = new StreamWriter(await request.GetRequestStreamAsync()))
                    {
                        string json = jsonData;
                        sw.Write(json);
                        sw.Flush();
                    }
                }
                //  var responses = (HttpWebResponse)request.GetResponse();
                var response = (HttpWebResponse)await request.GetResponseAsync();
                var encoding = ASCIIEncoding.ASCII;
                responseText = string.Empty;
                using (var reader = new System.IO.StreamReader(response.GetResponseStream(), encoding))
                {
                    responseText = await reader.ReadToEndAsync();
                }
            }
            catch (Exception ex)
            {
                string errMsg = string.Empty;
                if (ex.Message.Contains("See the inner exception for details."))
                {
                    errMsg = ex.InnerException.Message;
                    if (errMsg.Contains("See the inner exception for details."))
                    {
                        errMsg = ex.InnerException.InnerException.Message;
                    }
                }
                else
                {
                    errMsg = ex.Message;
                }
                responseText = "{\"Code\":401,\"Message\":\"" + errMsg + "\"}";
            }

            //JsonConvert responseText
            object vm;
            try
            {
                vm = Newtonsoft.Json.JsonConvert.DeserializeObject<T>(responseText);
            }
            catch (Exception ex)
            {

                responseText = "{\"Code\":500,\"Message\":\"Error in parsing object.\"}";
                vm = Newtonsoft.Json.JsonConvert.DeserializeObject<T>(responseText);
            }

            return (T)vm;
        }

        public static T getResponse<T>(string url, string jsonData, bool isPost)
        {
            string responseText = string.Empty;

            var headerKeys = new Dictionary<string, string>();
            headerKeys.Add("Authentication", "Bearer " + CacheModel.codServiceAccessToken);

            object vm = new Object();
            vm = getResponse<T>(url, jsonData, isPost, headerKeys);

            Type myType = vm.GetType();
            IList<System.Reflection.PropertyInfo> props = new List<System.Reflection.PropertyInfo>(myType.GetProperties());
            var responseCode = props.Where(x => x.Name == "Code").FirstOrDefault();
            if (responseCode != null)
            {
                int returnCode = (int)responseCode.GetValue(vm, null);

                //if(returnCode == (int)ResponseCode.TokenExpired)
                // {
                //     System.Web.HttpRequest req = System.Web.HttpContext.Current.Request;
                // }
                if (returnCode == (int)HttpStatusCode.Unauthorized)
                {
                    CacheModel.codServiceAccessToken = string.Empty;
                    vm = getResponse<T>(url, jsonData, isPost, headerKeys);
                }
            }

            return (T)vm;
        }

        public static async Task<T> getAsyncResponse<T>(string url, string jsonData, bool isPost)
        {
            string responseText = string.Empty;

            var headerKeys = new Dictionary<string, string>();
            headerKeys.Add("Authentication", "Bearer " + CacheModel.codServiceAccessToken);

            object vm = new Object();
            vm = await getAsyncResponse<T>(url, jsonData, isPost, headerKeys);

            Type myType = vm.GetType();
            IList<System.Reflection.PropertyInfo> props = new List<System.Reflection.PropertyInfo>(myType.GetProperties());
            var responseCode = props.Where(x => x.Name == "Code").FirstOrDefault();
            if (responseCode != null)
            {
                int returnCode = (int)responseCode.GetValue(vm, null);

                //if(returnCode == (int)ResponseCode.TokenExpired)
                // {
                //     System.Web.HttpRequest req = System.Web.HttpContext.Current.Request;
                // }
                if (returnCode == (int)HttpStatusCode.Unauthorized)
                {
                    CacheModel.codServiceAccessToken = string.Empty;
                    vm = await getAsyncResponse<T>(url, jsonData, isPost, headerKeys);
                }
            }

            return (T)vm;
        }

        public static string getAppSettingKey(string keyName)
        {
            string url = string.Empty;
            var endPoint = ConfigurationManager.AppSettings[keyName];
            if (endPoint != null)
            {
                url = endPoint.ToString();
            }
            return url;
        }

        //Application base url centeralize method
        public static string getBaseUrl()
        {
            string hostingEnvironment = getAppSettingKey("HostingEnvironment");
            string baseUrl = string.Empty;

            if (hostingEnvironment == "0")
            {
                baseUrl = ConfigurationManager.AppSettings["ApiBaseUrlCODLocal"].ToString();
            }

            return baseUrl;
        }
        #endregion
        //common utitlity methods
        #region commonMethods
        public static bool isNumber(string number)
        {
            long outVal = 0;
            return long.TryParse(number, out outVal);
        }

        public static string ReadFile(string path)
        {
            string result = string.Empty;

            if (!string.IsNullOrEmpty(path))
            {
                using (StreamReader reader = new StreamReader(path))
                {
                    result = reader.ReadToEnd();
                }
            }

            return result;
        }


        public static T getNotificationJSON<T>(string filepath)
        {
            object vm = null;
            try
            {
                vm = Newtonsoft.Json.JsonConvert.DeserializeObject<T>(utilityRepository.ReadFile(System.Web.Hosting.HostingEnvironment.MapPath(Constants.NotificationsFileBasePath + filepath)));
            }
            catch (Exception)
            {

            }
            return (T)vm;
        }
        #endregion

        //FireBase Notification Method
        #region fireBaseNotification
        public static T SendNotificationFromFirebaseCloud<T>(string deviceId, string message)
        {
            var result = string.Empty;
            var fcmUrl = "https://fcm.googleapis.com/fcm/send";


            var headerList = new Dictionary<string, string>();
            headerList.Add(string.Format("Authorization: key={0}", fcmKey), string.Empty);
            headerList.Add(string.Format("Sender: id={0}", fcmSenderID), string.Empty);

            object value = getResponse<T>(fcmUrl, message, true, headerList);
            //(result.Split(new string[] { "error" }, StringSplitOptions.None))[1].Replace(":", ":\"")
            return (T)value;
        }
        #endregion
    }
}
