# Finteq_Assesment
# Library Management System

This is a simple console-based Library Management System implemented in C#.

## Features

- **Add a New Book:** Allows users to add a new book to the library.
- **View All Books:** Displays information about all the books in the library.
- **Update Book Details:** Enables users to update details such as title, author, description, and publication year of a book.
- **Delete a Book:** Allows users to remove a book from the library.
- **Check Out a Book:** Marks a book as checked out, changing its status to unavailable.
- **Return a Book:** Updates the status of a checked-out book, marking it as returned.
- **Search for a Book:** Enables users to search for books by title or author.

## Instructions

1. **Run the Program:**
   - Compile and run the program to start the Library Management System.

2. **Menu Options:**
   - Choose from the menu options (1-7) to perform different operations on the library.

3. **Data Persistence:**
   - Book data is loaded from and saved to a file (`books.txt`) on program execution.
   - Data is loaded only once during the first execution to avoid redundancy.

4. **Exit or Continue:**
   - After performing an operation, decide whether to continue with additional operations or exit the program.

## How to Use

1. **Adding a New Book:**
   - Choose option 1 from the menu.
   - Enter details such as title, author, description, and publication year.

2. **Viewing All Books:**
   - Select option 2 to display information about all the books in the library.

3. **Updating Book Details:**
   - Choose option 3 and follow the prompts to update the details of a specific book.

4. **Deleting a Book:**
   - Select option 4 to delete a book by entering its ID.

5. **Checking Out a Book:**
   - Choose option 5 to mark a book as checked out.

6. **Returning a Book:**
   - Select option 6 to mark a checked-out book as returned.

7. **Searching for a Book:**
   - Choose option 7 and enter a part of the title or author to search for matching books.

8. **Exiting the Program:**
   - Choose to exit the program when you are done. The data will be saved automatically.

## Note
- Ensure that the `books.txt` file is in the same directory as the program for data persistence.
- Follow on-screen instructions and prompts for a smooth experience.
