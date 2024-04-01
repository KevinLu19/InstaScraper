using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
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

    public void SingleImagePost()
    {
        Thread.Sleep(5000);

		// Single image post.
		var img = _driver.FindElement(By.XPath("//img[@class='x5yr21d xu96u03 x10l6tqk x13vifvy x87ps6o xh8yej3']"));
		var src = img.GetAttribute("src");

		Console.WriteLine($"{_img_counter} : {src}");
		_img_counter++;
	}

    public void MultiImagePost()
    {
        Thread.Sleep(5000);

		// //li//img[@class='x5yr21d xu96u03 x10l6tqk x13vifvy x87ps6o xh8yej3 hoverZoomLink']  for more percise embedded images.
		bool image = _driver.FindElement(By.XPath("//li//img[@class='x5yr21d xu96u03 x10l6tqk x13vifvy x87ps6o xh8yej3']")).Displayed;

        var img = _driver.FindElement(By.XPath("//li//img[@class='x5yr21d xu96u03 x10l6tqk x13vifvy x87ps6o xh8yej3']"));
        var src = img.GetAttribute("src");

		Console.WriteLine($"{_img_counter} : {src}");
		_img_counter++;
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
        List<string> list_of_post_url = new();

        int count = 1;

        foreach (var item in img_post)
        {
            Console.WriteLine("Stuff in img post");
            Console.WriteLine(item);

            Console.WriteLine("+++++++++++");
        }

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

            list_of_post_url.Add(item);

            count++;
        }
        Console.WriteLine("------------------");

        FindImgSrc(list_of_post_url);

    }

    // Does the work of retreiving image src link.
    public void FindImgSrc(List<string> list_img)
    {
		_driver.SwitchTo().NewWindow(WindowType.Tab);

		foreach (var item in list_img)
        {
            _driver.Navigate().GoToUrl(item);

            Thread.Sleep(5000);

            var video_reel = FindElementIfExists(_driver, By.XPath("//video[@class='x1lliihq x5yr21d xh8yej3']"));
            
            if (video_reel != null)
            {
                Console.WriteLine("-------");
                Console.WriteLine(_driver.Url);
                Console.WriteLine( "Printing SRC");
                Console.WriteLine(video_reel.GetAttribute("src"));
                Console.WriteLine("-------");
            }

			var arrow_btn = FindElementIfExists(_driver, By.XPath("//div[@class=' _9zm2']"));

            if (arrow_btn != null)
            {
                MultiImagePost();
            }
            else
            {
                SingleImagePost();
            }

		}
    }

	// Checks if element exists or not. 
	// Usage: var element = driver.FindElementIfExists(By.CssSelector("a[data-value*='09.0']"));
	public IWebElement FindElementIfExists(IWebDriver _driver, By by)
    {
        var elements = _driver.FindElements(by);

        return (elements.Count >= 1) ? elements.First() : null;
    }

    // Located the arrow button on the image posts. If there is one, locate and click it until there is none signaling that it's at the end of the post.
    public bool ArrowButton()
    {
        Thread.Sleep(5000);

		try
        {
			var arrow_btn = _driver.FindElement(By.XPath("//div[@class=' _9zm2']"));

            while (arrow_btn.Displayed)
            {
                arrow_btn.Click();
                
            }

            return true;
        }
        catch 
        {
            Console.WriteLine("Button dont exist.");

            return false;
        }

    }

    public void DriverClose()
    {
        _driver.Close();
    }
}
