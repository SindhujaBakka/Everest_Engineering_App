**Overview**

This project is a C# console application that calculates:

 - Delivery cost per package, including applicable discounts

 - Estimated delivery time per package, using a limited fleet of vehicles

The solution focuses on correctness, clean design, extensibility, and testability, while remaining code intentionally kept simple for a time-boxed coding exercise.

**How to Run**

Open the solution in Visual Studio or any .NET-compatible IDE

Build the solution

Run the console application

Follow the prompts to enter input values
 - Entering the input values separately for each individual propert is to isolate the input and avoid typos.
 - All numeric inputs are validated to prevent negative or invalid values with a proper error message and a clean exit.
 - There is a flag added (isTestMode) to turn off the input prompting for each field and can provide inputs in one go with a space delimiter.

**Design Overview**

 - Program.cs acts as the application entry point and contains no business logic

 - Core business rules are encapsulated in service classes, each with a single responsibility

 - Domain models (Package, Vehicle) are simple data holders

 - Static utility classes are avoided to improve testability and design clarity
   
 - Validations were added to manage the error handling properly in all edge cases.
