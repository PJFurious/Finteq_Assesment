using System.Linq;
using System.Runtime.CompilerServices;

class operations
{
    static List<Book> books = new List<Book> { } ;
    static bool dataLoaded = false ;

    static void Main()
    {
        if (!dataLoaded)
        {
            LoadFromFile(Directory.GetCurrentDirectory() + "\\books.txt");
            dataLoaded = true ;
        }
        Console.WriteLine("Welcome to the library management system.\n");
        // show menu just lists the different operators
        ShowMenu();

        // dictionary is used as it removes the need for many if statements
        // each value has a corresponding method/function.
        Dictionary<string, Action> operation = new Dictionary<string, Action>
        {
            { "1" , Create },
            { "2" , Read },
            { "3" , Update },
            { "4" , Delete },
            { "5" , () => UpdateCheckin(true, "available", "checked out", "in store") },
            { "6" , () => UpdateCheckin(false, "booked out", "returned", "checked out") },
            { "7" , Search }
        }; 

        // get the operation from the user
        string option = GetNonEmptyInput("Operation number: ");

        // while loop is used to ensure user provides a valid option wihtin the menu
        while (!operation.ContainsKey(option))
            option = GetInput("Please enter one of the listed numbers: ");
        Console.WriteLine();

        // excecutes the wanted operation.
        if (operation.TryGetValue(option, out Action selectedOperation))   selectedOperation.Invoke();

        SaveToFile(Directory.GetCurrentDirectory() + "\\books.txt");

        // check to see if user wants to continue or exit.
        string ans = GetInput("\nDo you want to use other operators?\nType the letter \"Y\" to continue: ").ToUpper();
        if (ans == "Y")
        {
            Console.Clear();
            Main();
        }
        else
            Environment.Exit(0);
    }

    static void LoadFromFile(string path)
    {
        try
        {
            // create file if it does not exist
            if (!File.Exists(path))
                File.Create(path).Close();

            // read each line and save to the list
            foreach (string line in File.ReadLines(path))
            {
                string[] vars = line.Split('*');
                Book newBook = new Book()
                {
                    bookId = vars[0],
                    title = vars[1],
                    author = vars[2],
                    description = vars[3],
                    publicatioYear = Convert.ToInt32(vars[4]),
                    isCheckedIn = Convert.ToBoolean(vars[5])
                };

                books.Add(newBook);
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error reading from file: {ex}");
        }
    }

    static void SaveToFile(string path)
    {
        // save book to file
        try
        {
            using (StreamWriter writer = new StreamWriter(path))
                foreach (var book in books)
                    // Write each book's attributes to a new line in the file
                    writer.WriteLine($"{book.bookId}*{book.title}*{book.author}*{book.description}*{book.publicatioYear}*{book.isCheckedIn}");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error writing to file: {ex}");
        }
    }


    static void Create()
    {
        // Creating new 'Book' item with its needed values
        var book = new Book
        {
            // adding one to the last book in the list, prevents user from adding duplicates.
            bookId = (books.Count != 0) ? (int.Parse(books.Last().bookId) + 1).ToString() : "1",
            title = GetNonEmptyInput("Title: "),
            author = GetNonEmptyInput("Auther: "),
            description = GetNonEmptyInput("Description: "),
            publicatioYear = GetYear("Year: ") ,
            isCheckedIn = true, // when adding a book it will be in the store so checked in is set to true by default
        };

        // add book to the list of books
        books.Add(book);
        Console.Write("\nBook was succesfully added!");
    }

    static void Read()
    {
        if (books.Count > 0)
        {
            Console.WriteLine("All Books:\n------------------------");

            // Runs through all the books in the books list and displaying its attributes.
            foreach (var book in books)
            {
                Console.WriteLine($"Book ID: {book.bookId}");
                Console.WriteLine($"Title: {book.title}");
                Console.WriteLine($"Author: {book.author}");
                Console.WriteLine($"Description: {book.description}");
                Console.WriteLine($"Publication Year: {book.publicatioYear}");
                Console.WriteLine($"Checked In: {book.isCheckedIn}");
                Console.WriteLine("------------------------");
            }
        }
        else Console.WriteLine("There are currently no books");
    }

    static void Read(List<Book> x)
    {
        if (books.Count > 0)
        {
            Console.WriteLine("All Books:\n------------------------");

            // Runs through all the books in the books list and displaying its attributes.
            foreach (var book in x)
            {
                Console.WriteLine($"Book ID: {book.bookId}");
                Console.WriteLine($"Title: {book.title}");
                Console.WriteLine($"Author: {book.author}");
                Console.WriteLine($"Description: {book.description}");
                Console.WriteLine($"Publication Year: {book.publicatioYear}");
                Console.WriteLine($"Checked In: {book.isCheckedIn}");
                Console.WriteLine("------------------------");
            }
        }
        else Console.WriteLine("There are currently no books");
    }

    static void Update()
    {
        if (books.Count > 0) 
        {
            // first display the books so infomation about it can be seen and the user knows what to change
            Read();

            // get book id from user 
            string bookId = GetNonEmptyInput("\nEnter book id to update: ");

            // while loop wil run if incorrect value was provided until correct one is given.
            while (!books.Any(b => b.bookId == bookId))
                bookId = GetNonEmptyInput("Enter a valid book id to update: ");

            Console.Write("\n(Press enter to skip a field you want to remain the same)\n");

            // only allowing the user to update the values that should be updated, leaving the attributes that should not be changed
            string uTitle = GetInput("New title: ");
            string uAuther = GetInput("New auther: ");
            string uDescription = GetInput("New description: ");
            string uYear = GetInput("New publication Year: ");

            // checking to see if the year was correctly provided.
            int iYear = books.Where(b => b.bookId == bookId).Select(b => b.publicatioYear).FirstOrDefault();
            if (uYear != string.Empty)
                if (!int.TryParse(uYear, out iYear))
                    iYear = GetYear("Please enter a valid year: ");
                else iYear = int.Parse(uYear);

            // Updating book where id matched in the list.
            books.Where(b => b.bookId == bookId).ToList().ForEach(v =>
            {
                v.title = (uTitle != string.Empty) ? uTitle : books.Where(b => b.bookId == bookId).Select(b => b.title).FirstOrDefault();
                v.author = (uAuther != string.Empty) ? uAuther : books.Where(b => b.bookId == bookId).Select(b => b.author).FirstOrDefault();
                v.description = (uDescription != string.Empty) ? uDescription : books.Where(b => b.bookId == bookId).Select(b => b.description).FirstOrDefault();
                v.publicatioYear = iYear;
            });

            Console.Write("\nBook was succesfully updated!");
        }
        else Console.WriteLine("There are currently no books");
    }

    static void Delete()
    {
        if (books.Count > 0) 
        {
            // displaying books with id and just name
            foreach (var book in books)
                Console.WriteLine($"{book.bookId}. {book.title}");

            // get book id from user,
            string bookId = GetNonEmptyInput("\nEnter the ID of the book you want to delete: ");

            // while loop will keep running until the user provides a input that is contained within the list of books
            while (!books.Any(b => b.bookId == bookId))
                bookId = GetInput("Please enter one of the listed books: ");

            // set list equal to itself where the ids dont match the one that has to be deleted
            books = books.Where(b => b.bookId != bookId).ToList();

            Console.Write("\nBook was succesfully removed!");
        }
        else Console.WriteLine("There are currently no books");
        
    }

    static void Search()
    {
        if (books.Count > 0)
        {
            // Get input from user, Name or Author
            string searchStr = GetNonEmptyInput("Please enter any part of the author name or book title you want to search for: ");

            // create new list with all the matched books
            var newBooks = books.Where(b => b.title.ToUpper().Contains(searchStr.ToUpper()) || b.author.ToUpper().Contains(searchStr.ToUpper()));

            // display it or indicate that no matching book was found.
            if (newBooks.Count() > 0)
                Read(newBooks.ToList());
            else
                Console.WriteLine("No matching book was found.");
        }
        else Console.WriteLine("There are currently no books");
    }

    static void UpdateCheckin(bool checkedIn, string current, string success, string inStore)
    {
        // create list to dispaly the books that has been displayed.
        var ids = new List<string>();

        // check to see what books shoudl be displayed.
        // if they want ot return a book only the ones that have been checked out should be displayed and worked with and ivsa versa
        if (books.Any(b => b.isCheckedIn == checkedIn))
        {
            // create new list with the needed books
            var newBooks = books.Where(b => b.isCheckedIn == checkedIn);

            // dispalay all the relevant books and add the ids to a list
            Console.WriteLine($"All books that are currently {current}:");
            Console.WriteLine("-----------------");
            foreach (var book in newBooks)
            {
                ids.Add(book.bookId); // add ids to list
                Console.WriteLine($"{book.bookId}. {book.title}");
            }
            Console.WriteLine("-----------------");

            // get chosen book id form user
            string bookId = GetNonEmptyInput($"Enter book ID to be {success}: ");

            // check to see if the given id is within th eavailable options, else re-ask
            while (!ids.Contains(bookId))
                bookId = GetInput("Please enter one of the listed books: ");

            // update book checkin status where ids match
            books.Where(b => b.bookId == bookId).ToList().ForEach(v => { v.isCheckedIn = !checkedIn; });
            Console.WriteLine($"Book has successfully been {success}.");
        }
        else
        {
            Console.WriteLine($"There are currently no books {inStore }.");
        }
    }

    static string GetInput(string sentence)
    {
        // method for inormation to be retrieved
        Console.Write(sentence); return Console.ReadLine().Trim();
    }

    static string GetNonEmptyInput(string sentence)
    {
        // method to get input that is not empty
        string input = GetInput(sentence);

        while (input == string.Empty) input = GetInput("Please enter a valid input: ");

        return input;
    }

    static int GetYear(string sentence)
    {
        // method to see if it is a valid year
        bool isInt = false;
        int year = -1;

        // while loop keeps requesting input it it is incorrectly provided
        while (!isInt)
        {
            if (int.TryParse(GetNonEmptyInput(sentence), out year))
                // check to see if the year is bigger than 0
                if (year > 0)
                    isInt = true;
                else isInt = false;
            else
            {
                isInt = false;
                sentence = "Please enter a valid year: ";
            }
        }

        return year;
    }

    static void ShowMenu()
    {
        // method just to display the menu. (Corresponds with a dictionary in the main function)
        Console.WriteLine("These are the available operations:");
        Console.WriteLine("1. Add a new book.");
        Console.WriteLine("2. View all books.");
        Console.WriteLine("3. Update a book’s details.");
        Console.WriteLine("4. Delete a book.");
        Console.WriteLine("5. Check out a book.");
        Console.WriteLine("6. Return a book.");
        Console.WriteLine("7. Search for a book by Title or Auther.");
        Console.WriteLine("Please enter the number of the opration you want to run\n(Ex. 2)");
    }

    class Book
    {
        // Book class with the needed attributes of the book.
        public string bookId { get; set; }
        public string description { get; set; }
        public string title { get; set; }
        public string author { get; set; }
        public int publicatioYear { get; set; }
        public bool isCheckedIn { get; set; }
    }
}