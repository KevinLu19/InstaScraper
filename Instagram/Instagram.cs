using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using Org.BouncyCastle.Bcpg.OpenPgp;

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


    /*
     Find the image and instead of looking immediately into the src of the image. Go find the Href and click on it. 
    From there, look at the image src and potentially more images if it is stacked post.
     */
    public void FindImages()
    {
        Thread.Sleep(3000);

        // Find the href link from image. Need to skip the first 11. 12th is the start of the image post.
        var image = _driver.FindElements(By.XPath("//a[@class='x1i10hfl xjbqb8w x1ejq31n xd10rxx x1sy0etr x17r0tee x972fbf xcfux6l x1qhh985 xm0m39n x9f619 x1ypdohk xt0psk2 xe8uvvx xdj266r x11i5rnm xat24cr x1mh8g0r xexx8yu x4uap5 x18d9i69 xkhd6sd x16tdsg8 x1hl2dhg xggy1nq x1a2a7pz _a6hd']"));

        List<string> img_post = new();

        List<string> img = new();

        int count = 1;
        foreach (var item in image)
        {
            
            // Need to splice last 5 from item. 
            //Console.WriteLine($"{count} : {item.GetAttribute("href")}");

            img_post.Add(item.GetAttribute("href"));

            //count++;
        }

        var posts = img_post.Take(12);
        
        // Prints item in list.
        foreach (var item in posts) 
        {
            Console.WriteLine($"{count} : {item}");

            img.Add(item);

            count++;
        }

        
        FindImgSrc(img);

    }

    // Does the work of retreiving image src link.
    public void FindImgSrc(List<string> list_img)
    {
        foreach (var item in list_img)
        {
            _driver.SwitchTo().NewWindow(WindowType.Tab);

            _driver.Navigate().GoToUrl(item);
            Thread.Sleep(2000);
		}
    }

    public void DriverClose()
    {
        _driver.Close();
    }
}
