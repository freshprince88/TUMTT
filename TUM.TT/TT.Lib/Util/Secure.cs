using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace TT.Lib.Util
{
    public class Secure
    {
        private string globalPath;
        private string startDate = Base64Encode(new DateTime(2018, 01, 01).ToShortDateString());
        private string endDate = Base64Encode(new DateTime(2019, 01, 01).ToShortDateString());


        public enum Mode
        {
            Period,
            Date
        }

        private Mode _mode;

        public Secure(Mode mode)
        {
            _mode = mode;
        }

        private void updateDates()
        {
            RegistryKey regkey = Registry.CurrentUser;
            regkey = regkey.CreateSubKey(globalPath); //path
            regkey.SetValue("Start", startDate); //Value Name,Value Data
            regkey.SetValue("End", endDate); //Value Name,Value Data
        }

        private void firstTime()
        {
            RegistryKey regkey = Registry.CurrentUser;
            regkey = regkey.CreateSubKey(globalPath); //path

            DateTime dt = DateTime.Now;
            string onlyDate = dt.ToShortDateString(); // get only date not time

            regkey.SetValue("Install", onlyDate); //Value Name,Value Data
            regkey.SetValue("Use", onlyDate); //Value Name,Value Data
            regkey.SetValue("Start", startDate); //Value Name,Value Data
            regkey.SetValue("End", endDate); //Value Name,Value Data
        }

        private String checkfirstDate()
        {
            RegistryKey regkey = Registry.CurrentUser;
            regkey = regkey.CreateSubKey(globalPath); //path
            string Br = (string)regkey.GetValue("Install");
            if (regkey.GetValue("Install") == null)
                return "First";
            else
                return Br;
        }

        private bool checkPassword(String pass)
        {
            RegistryKey regkey = Registry.CurrentUser;
            regkey = regkey.CreateSubKey(globalPath); //path
            string Br = (string)regkey.GetValue("Password");
            if (Br == pass)
                return true; //good
            else
                return false;//bad
        }

        private String dayDifPutPresent()
        {

            // get present date from system
            DateTime dt = DateTime.Now;
            string today = dt.ToShortDateString();
            DateTime presentDate = Convert.ToDateTime(today);

            if (_mode.Equals(Mode.Period))
            {
                // get instalation date
                RegistryKey regkey = Registry.CurrentUser;
                regkey = regkey.CreateSubKey(globalPath); //path
                string Br = (string)regkey.GetValue("Install");
                DateTime installationDate = Convert.ToDateTime(Br);

                TimeSpan diff = presentDate.Subtract(installationDate); //first.Subtract(second);
                int totaldays = (int)diff.TotalDays;

                // special check if user changed date in system
                string usd = (string)regkey.GetValue("Use");
                DateTime lastUse = Convert.ToDateTime(usd);
                TimeSpan diff1 = presentDate.Subtract(lastUse); //first.Subtract(second);
                int useBetween = (int)diff1.TotalDays;

                // put next use day in registry
                regkey.SetValue("Use", today); //Value Name,Value Data

                if (useBetween >= 0)
                {

                    if (totaldays < 0)
                        return "Error"; // if user change date in system like date set before installation
                    else if (totaldays >= 0 && totaldays <= 5)
                        return installationDate.AddDays(5).ToShortDateString(); //how many days remaining
                    else
                        return "Expired"; //Expired
                }
                else
                    return "Error"; // if user change date in system
            }
            else if (_mode.Equals(Mode.Date))
            {
                // get start and end date
                RegistryKey regkey = Registry.CurrentUser;
                regkey = regkey.CreateSubKey(globalPath); //path
                string start = Base64Decode((string)regkey.GetValue("Start"));
                string end = Base64Decode((string)regkey.GetValue("End"));
                DateTime startDate = Convert.ToDateTime(start);
                DateTime endDate = Convert.ToDateTime(end);

                TimeSpan diffStart = presentDate.Subtract(startDate); //first.Subtract(second);
                int totaldays = (int)diffStart.TotalDays;

                TimeSpan diffEnd = endDate.Subtract(presentDate);
                int remainingDays = (int)diffEnd.TotalDays;

                // special check if user changed date in system
                string usd = (string)regkey.GetValue("Use");
                DateTime lastUse = Convert.ToDateTime(usd);
                TimeSpan diff1 = presentDate.Subtract(lastUse); //first.Subtract(second);
                int cheat = (int)diff1.TotalDays;
                // put next use day in registry
                regkey.SetValue("Use", today); //Value Name,Value Data

                if (cheat >= 0)
                {

                    if (totaldays < 0)
                        return "Error"; // if user change date in system like date set before installation
                    else if (totaldays >= 0 && remainingDays >= 0)
                        return endDate.ToShortDateString(); //how many days remaining
                    else
                        return "Expired"; //Expired
                }
                else
                    return "Error"; // if user change date in system
            }
            else
                return "Expired";
        }

        private void blackList()
        {
            RegistryKey regkey = Registry.CurrentUser;
            regkey = regkey.CreateSubKey(globalPath); //path

            regkey.SetValue("Black", "True");

        }

        private bool blackListCheck()
        {
            RegistryKey regkey = Registry.CurrentUser;
            regkey = regkey.CreateSubKey(globalPath); //path
            string Br = (string)regkey.GetValue("Black");
            if (regkey.GetValue("Black") == null)
                return false; //No
            else
                return true;//Yes
        }

        public bool Algorithm(String appPassword, String pass)
        {
            globalPath = pass;
            bool chpass = checkPassword(appPassword);
            if (chpass == true) //execute
                return true;
            else
            {
                string chinstall = checkfirstDate();
                if (chinstall == "First")
                {
                    firstTime();// installation date                        
                }
                else
                {
                    updateDates();
                }

                string status = dayDifPutPresent();
                if (status == "Error")
                {
                    blackList();
                    MessageBox.Show("Ihre Version wurde auf Grund einer unauthorisierten Änderung des Datums deaktiviert!", "Unerlaubtes Datum.", MessageBoxButton.OK, MessageBoxImage.Error);
                    return false;
                }
                else if (status == "Expired")
                {
                    MessageBox.Show("Ihre Version ist leider abgelaufen!", "Trial expired", MessageBoxButton.OK, MessageBoxImage.Information);
                    return false;
                }
                else // execute with how many day remaining
                {
                    //MessageBox.Show("Sie verwenden eine Demo-Version des Beachviewers. Diese Version ist noch bis zum " + status + " gültig.", "Trial Version", MessageBoxButton.OK, MessageBoxImage.Information);
                    return true;
                }
            }
        }

        public static string Base64Encode(string plainText)
        {
            var plainTextBytes = System.Text.Encoding.UTF8.GetBytes(plainText);
            return System.Convert.ToBase64String(plainTextBytes);
        }

        public static string Base64Decode(string base64EncodedData)
        {
            var base64EncodedBytes = System.Convert.FromBase64String(base64EncodedData);
            return System.Text.Encoding.UTF8.GetString(base64EncodedBytes);
        }
    }
}
