using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;

namespace InstagramWebScrape.Instagram;
internal class Instagram
{
    private IWebDriver _driver;
    public Instagram(string url)
    {
        _driver = new ChromeDriver();

        try
        {
            _driver.Navigate().GoToUrl(url);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
        }
    }

    public void Images()
    {
        Thread.Sleep(5000);     // Sleeps for 5 seconds to let entire webpage to fully load.

        // Finds each image post and grabs the src attribute.
        var image = _driver.FindElements(By.XPath(".//img[@class='x5yr21d xu96u03 x10l6tqk x13vifvy x87ps6o xh8yej3']"));


        int count = 1;
        foreach (var item in image) 
        {
            Console.WriteLine("-------");
            Console.WriteLine($"{count} : {item.GetAttribute("src")} ");
			Console.WriteLine("-------");

            count++;
		}
    }

    public void DriverClose()
    {
        _driver.Close();
    }
}
