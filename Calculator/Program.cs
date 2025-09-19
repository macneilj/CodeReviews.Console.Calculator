using CalculatorLibrary;
using System.Text.Json.Nodes;
using System.Text.RegularExpressions;

class Program
{
    static void Main(string[] args)
    {
        bool endApp = false;
        // Display title as the C# console calculator app.
        Console.WriteLine("Console Calculator in C#\r");
        Console.WriteLine("------------------------\n");

        Calculator calculator = new Calculator();

        while (!endApp)
        {
            bool showRecord = false;

            // Declare variables and set to empty.
            // Use Nullable types (with ?) to match type of System.Console.ReadLine
            string? numInput1 = "";
            string? numInput2 = "";
            double result = 0;

            // Ask the user to type the first number.
            Console.Write("Type a number, and then press Enter: ");
            numInput1 = Console.ReadLine();

            double cleanNum1 = 0;
            while (!double.TryParse(numInput1, out cleanNum1))
            {
                Console.Write("This is not valid input. Please enter a numeric value: ");
                numInput1 = Console.ReadLine();
            }

            // Ask the user to type the second number.
            Console.Write("Type another number, and then press Enter: ");
            numInput2 = Console.ReadLine();

            double cleanNum2 = 0;
            while (!double.TryParse(numInput2, out cleanNum2))
            {
                Console.Write("This is not valid input. Please enter a numeric value: ");
                numInput2 = Console.ReadLine();
            }

            // Ask the user to choose an operator.
            Console.WriteLine("Choose an operator from the following list:");
            Console.WriteLine("\ta - Add");
            Console.WriteLine("\ts - Subtract");
            Console.WriteLine("\tm - Multiply");
            Console.WriteLine("\td - Divide");
            Console.Write("Your option? ");

            string? op = Console.ReadLine();

            // Validate input is not null, and matches the pattern
            if (op == null || !Regex.IsMatch(op, "[a|s|m|d]"))
            {
                Console.WriteLine("Error: Unrecognized input.");
            }
            else
            {
                try
                {
                    result = calculator.DoOperation(cleanNum1, cleanNum2, op);
                    if (double.IsNaN(result))
                    {
                        Console.WriteLine("This operation will result in a mathematical error.\n");
                    }
                    else Console.WriteLine("Your result: {0:0.##}\n", result);
                }
                catch (Exception e)
                {
                    Console.WriteLine("Oh no! An exception occurred trying to do the math.\n - Details: " + e.Message);
                }
            }
            Console.WriteLine("------------------------\n");

            // Wait for the user to respond before closing.
            Console.Write("Press 'n' and Enter to close the app, press 'r' to see past calcutions, or press any other key and Enter to continue: ");
            var choice = Console.ReadLine();
            if (choice == "n")
            {
                endApp = true;
            }
            else if (choice == "r")
            {
                showRecord = true;
            }


            while (showRecord)
            {
                JsonArray records = calculator.GetCalculations();
                Console.WriteLine($"You have used the calculator {records.Count} time(s).");

                foreach (var record in records)
                {
                    int index = record.GetElementIndex();
                    Console.WriteLine($"ID: {index} - {record["Operation"]}: {record["Operand1"]} {record["Symbol"]} {record["Operand2"]} = {record["Result"]}");

                }

                Console.WriteLine("Enter a record ID to get more options");
                var userIndex = Console.ReadLine();


                if (int.TryParse(userIndex, out int value))
                {
                    JsonNode matchRecord = records[value];
                    if (matchRecord is not null)
                    {
                        Console.WriteLine("Press 'e' to perform a different operation, or 'd' to delete the record");
                        var recordChoice = Console.ReadLine();

                        if (recordChoice == "d")
                        {
                            //delete record
                            records.RemoveAt(value);
                        }
                        else if (recordChoice == "e")
                        {
                            {
                                //This isn't DRY but i would move it's own method
                                Console.WriteLine("Choose a different operator from the following list:");
                                Console.WriteLine("\ta - Add");
                                Console.WriteLine("\ts - Subtract");
                                Console.WriteLine("\tm - Multiply");
                                Console.WriteLine("\td - Divide");

                                Console.Write("Your option? ");

                                string? newOp = Console.ReadLine();

                                // Validate input is not null, and matches the pattern
                                if (newOp == null || !Regex.IsMatch(newOp, "[a|s|m|d]"))
                                {
                                    Console.WriteLine("Error: Unrecognized input.");
                                }
                                else
                                {
                                    try
                                    {
                                        result = calculator.DoOperation((double)matchRecord["Operand1"].AsValue(), (double)matchRecord["Operand2"].AsValue(), newOp);
                                        if (double.IsNaN(result))
                                        {
                                            Console.WriteLine("This operation will result in a mathematical error.\n");
                                        }
                                        else Console.WriteLine("Your result: {0:0.##}\n", result);
                                    }
                                    catch (Exception e)
                                    {
                                        Console.WriteLine("Oh no! An exception occurred trying to do the math.\n - Details: " + e.Message);
                                    }
                                }
                                Console.WriteLine("------------------------\n");
                            }

                        }
                        else
                        {
                            Console.WriteLine("ID not found");
                        }

                    }
                    else
                    {
                        Console.WriteLine("You must enter a proper ID");
                    }

                    showRecord = false;
                    Console.WriteLine("\n"); // Friendly linespacing.
                }

            }

            Console.WriteLine("\n"); // Friendly linespacing.
        }

        calculator.Finish();

        return;
    }
}