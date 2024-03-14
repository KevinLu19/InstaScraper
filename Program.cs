// See https://aka.ms/new-console-template for more information
using InstagramWebScrape.Database;
using InstagramWebScrape.Instagram;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;


Database insta_user_db = new();
//insta_user_db.CreateUserTable();


// Create an option menu
Console.WriteLine("Here are the options");
Console.WriteLine("1 - Manually add usernames into username database.");
Console.WriteLine("2 - Manually filter out name on username database to search posts.");
Console.WriteLine("3 - Search user post on Instagram.");
Console.WriteLine("4 - Print all user in table.");
Console.Write("Enter your option: ");
var option_choice = Console.ReadLine();

if (option_choice == "1")
{
	Console.WriteLine("You've chosen option 1.");
	OptionOne();
}
else if (option_choice == "2")
{
	Console.WriteLine("You've chosen option 2.");
	Console.Write("Please enter in user: ");
	string user_choice = Console.ReadLine();
	insta_user_db.GetInstaUsernames(user_choice);
}
else if (option_choice == "3")
{
	/*
		Option to serch users without looking at the database.
	 */
    Console.WriteLine("You've chosen option 3.");
    Console.Write("Enter in instagram user to search post: ");
	string user_choice = Console.ReadLine();

	// Integrate user choice to instagram post serch.
	string base_url = $"https://www.instagram.com/{user_choice}";

	Instagram instagram = new(base_url);
	instagram.Images();
	Thread.Sleep(3000);
}
else if (option_choice == "4")
{
    Console.WriteLine("You've picked to print everything from table.");
    insta_user_db.PrintUsername();
}


void OptionOne()
{
	// Manually enter in usernames into database.
	while (true)
	{
		// ** Entering insta username to fill up the database tables.
		Console.Write($"Enter in usernames to fill up table: ");
		var user_input = Console.ReadLine();

		if (user_input == "q")
		{
			break;
		}

		insta_user_db.StoreInstaUsernames(user_input);
	}
}
