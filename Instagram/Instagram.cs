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
    private int _img_counter = 1;
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

    public void ImageSource()
    {
        Thread.Sleep(5000);

        var image = _driver.FindElement(By.XPath("//img[@class='x5yr21d xu96u03 x10l6tqk x13vifvy x87ps6o xh8yej3']"));
        var src = image.GetAttribute("src");


        Console.WriteLine($"{_img_counter} : {src}");
        _img_counter++;
    }

    public void ReelSource()
    {
        Thread.Sleep(5000);

        var reel = _driver.FindElement(By.XPath(""));
        var src = reel.GetAttribute("src");
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
        // Used to verify if the arrow button exists in the current post that driver is nagivating to.
        //var arrow_btn_verification = _driver.FindElement(By.XPath("//button[@class=' _afxv _al46 _al47']"));
        //var check_btn = _driver.FindElement(By.XPath("//button[@class=' _afxv _al46 _al47']")).Displayed;

		foreach (var item in list_img)
        {
            _driver.SwitchTo().NewWindow(WindowType.Tab);

            _driver.Navigate().GoToUrl(item);
			// Find the img src and then check if arrow button exists.

			// ** Need to fix the posts where it is a reel instead of an image.
			// Video class is <video class="x1lliihq x5yr21d xh8yej3" />
			// Need to also fix where is one post have multiple pictures, do not navigate to another post until all images are taken.
            // In order to get reel url, just get selenium's tab url.
			ImageSource();
            ArrowButton();

            Thread.Sleep(2000);
		}
    }


    // Located the arrow button on the image posts. If there is one, locate and click it until there is none signaling that it's at the end of the post.
    public void ArrowButton()
    {
        Thread.Sleep(5000);

		try
        {
			var arrow_btn = _driver.FindElement(By.XPath("//div[@class=' _9zm2']"));

            while (arrow_btn.Displayed)
            {
                arrow_btn.Click();
                ImageSource();
            }

        }
        catch 
        {
            Console.WriteLine("Button dont exist.");
        }

    }

    public void DriverClose()
    {
        _driver.Close();
    }
}
