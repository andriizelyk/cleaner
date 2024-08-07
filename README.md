# Solution Cleaner

This C# console application recursively searches for all `.sln` files in a specified directory and its subdirectories, then runs `dotnet clean` on each of them. The application optimizes the cleaning process by using a semaphore to control the number of concurrent clean operations, based on the number of CPU cores available.

## Features

- **Recursive Search**: Finds all `.sln` files within the specified directory and its subdirectories.
- **Concurrency Control**: Utilizes semaphores to manage the number of concurrent `dotnet clean` operations, set to `CPU cores - 2` to optimize performance.

## Usage

1. **Clone the Repository**:
   ```sh
   git clone https://github.com/yourusername/solution-cleaner.git
   cd solution-cleaner
2. Build the Project:
Open the project in Visual Studio or your preferred C# development environment and build it.

3. Run the Application:
Open a command prompt and navigate to the output directory (e.g., bin\Debug\net6.0). Run the application with the root folder as an argument:

```sh
Cleaner.exe "C:\Path\To\Your\Folder"