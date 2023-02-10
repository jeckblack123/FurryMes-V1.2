using OpenQA.Selenium;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace FurryMes_V1._2
{
    class NetworkService
    {
        static ArrayList validatedList = new ArrayList();
        static ArrayList notificatedUsers = new ArrayList();
        static public Boolean flag = true;


        private static IWebDriver CreateBrowserDriver()
        {
            try
            {
                var options = new OpenQA.Selenium.Chrome.ChromeOptions();
                options.AddArguments("--disable-extensions");

                var service = OpenQA.Selenium.Chrome.ChromeDriverService.CreateDefaultService();
                service.SuppressInitialDiagnosticInformation = true;

                return new OpenQA.Selenium.Chrome.ChromeDriver(service, options);
            }
            catch
            {
                throw new Exception("Please install Google Chrome.");
            }
        }

        static public IWebDriver driver = CreateBrowserDriver();
        public static async void startPoint()
        {
            try
            {
                int counter = 1;
                string pathURL = "https://www.furaffinity.net/watchlist/to/keplen/";
                while (flag)
                {
                    driver.Url = pathURL + counter;

                    ArrayList list = new ArrayList();

                    IList<IWebElement> elements = driver.FindElements(By.TagName("a"));
                    if (elements != null) { flag = true; counter++; } else { flag = false; };
                    if (!flag) { break; };
                    foreach (IWebElement e in elements)
                    {
                        list.Add(e.Text);
                    }


                    foreach (string e in list)
                    {

                        driver.Navigate().GoToUrl($"https://www.furaffinity.net/user/{e}");
                        IWebElement watchers = driver.FindElement(By.XPath("//a[contains(text(), 'View List (Watched by')]"));
                        ValidateData(watchers.Text, e);

                    }
                }
            }
            catch (Exception ex)
            {

                Console.WriteLine(validatedList.Count);
            }
            FileService.writeUser(validatedList);
        }


        public static void ValidateData(string watchers, string nickname)
        {
            int resultStringWatched = Int32.Parse(Regex.Match(watchers, @"\d+").Value);
            if (resultStringWatched > 2500)
            {
                throw new Exception("Need less watchers");
            }
            else
            {
                createUser(nickname);
            }

        }


        public static void createUser(string nickname)
        {
            validatedList.Add(nickname);
        }


        public static void SendMessage(string subjectText, string messageText)
        {

            ArrayList listOfUsers = new ArrayList(FileService.GetAllWatchers());
            for (int i = 0; i < 5; i++)
            {
                try
                {
                    driver.Navigate().GoToUrl($"https://www.furaffinity.net/newpm/{listOfUsers[i]}/");
              /*    driver.FindElement(By.XPath("//*[@id=\"note-form\"]/section/div[1]/div[1]/div[2]/div[2]/input")).SendKeys(subjectText);
                    driver.FindElement(By.XPath("//*[@id=\"JSMessage_compose\"]")).SendKeys(messageText);
                    driver.FindElement(By.XPath("//*[@id=\"note-form\"]/section/div[2]/div/input")).Click();*/
                    notificatedUsers.Add(listOfUsers[i]);
                    listOfUsers.Remove(listOfUsers[i]);
                }
                catch (Exception ex)
                {
                    i++;
                }

            }
            FileService.writeUserWithMessage(notificatedUsers);
            FileService.writeUserWithoutMessage(listOfUsers);
            driver.Quit();

        }

    }
}
