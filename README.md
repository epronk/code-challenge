# Code Challenge Instructions

Write a program that transforms `data.json` into `data-transformed.json`. Have a look at `example-output.json` to show how the data should be transformed.

## General Approach and Assumptions

- Solution should be implemented in C#, Swift, or C++.
- Read JSON `data.json` from current directory, convert data, write `data-transformed.json` to current directory.
- Solution should build and run with a single CLI command or via opening a project in an IDE like Visual Studio or Xcode and selecting 'Run' or equivalent.
- Thirdparty dependencies should be kept to a minimum and must be fetched and installed as a part of the build process.
- Solution should write output file directly to project directory unless otherwise specified.

## Criteria

Your work will be evaluated primarily on:

- Consistency of coding style.
- Idiomatic language use.
- Use of asynchronous features if supported by your language of choice.
- Correct and complete unit test coverage.
- General quality of code and technical communication.

## How to submit your work

 1. Fork this project on github.
 2. Update this README.md file with instructions on how to build/test/run your solution.
 3. When you're finished, send us the URL of your public repository.

# Solution notes Eddy Pronk

## Building

* Install the [.NET 5.0 SDK](https://dotnet.microsoft.com/download/dotnet/thank-you/sdk-5.0.202-windows-x64-installer)
* Clone this repository and change into the repository root directory.
* On the command line type `dotnet publish -c Release -r win-x64`
* The resulting executable can be retrieved from `Transform.App\bin\Release\net5.0\win-x64\Transform.App.exe`

## Testing

* On the command line type `dotnet test`

## Running

* On the command line type `Transform.App\bin\Release\net5.0\win-x64\Transform.App.exe`

(A copy of the output of my solution is in the project directory with the name `data-transformed-sample.json`.

## Implementation notes

The transformation splits up customers and orders in the output
file.  The simplest implementation would require the full set of
`Customer` and `OrderCollection` objects to be kept in memory to
write the output.

The memory usage would be proportional to the input file
size. For very large files this could become an issue.

To keep the memory usage low I opted for a solution, where it
scans the file using a two-pass strategy.

Now only a single `OrderCollection` with its associated `Orders`
is kept in memory.  The assumption here is that the number of
`Orders` in a collection relatively low. If the number of orders
is very large, then the same approach could be extended to orders.

## Testing notes

Some unit tests are writen in the Given/When/Then format.

**Given** input X

**and** input Y

**When** action A is performed

**Then** the outcome is B
