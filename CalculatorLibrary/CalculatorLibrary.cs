using Newtonsoft.Json;
using System.Dynamic;
using System.Security.Principal;
using System.Text.Json;
using System.Text.Json.Nodes;


namespace CalculatorLibrary
{
    public class Calculator
    {
        JsonArray jsonArray = new JsonArray();

        public Calculator()
        {
        }

        // CalculatorLibrary.cs
        public double DoOperation(double num1, double num2, string op)
        {
            double result = double.NaN; // Default value is "not-a-number" if an operation, such as division, could result in an error.
            string operation = "";
            string opSmybol = "";

            // Use a switch statement to do the math.
            switch (op)
            {
                case "a":
                    result = num1 + num2;
                    operation = "Add";
                    opSmybol = "+";

                    break;
                case "s":
                    result = num1 - num2;
                    operation = "Subtract";
                    opSmybol = "-";

                    break;
                case "m":
                    result = num1 * num2;
                    operation = "Multiply";
                    opSmybol = "*";

                    break;
                case "d":
                    // Ask the user to enter a non-zero divisor.
                    if (num2 != 0)
                    {
                        result = num1 / num2;
                    }
                    operation = "Divide";
                    opSmybol = "/";

                    break;
                // Return text for an incorrect option entry.
                default:
                    break;
            }

            var date = DateTime.Now;

            jsonArray.Add(new JsonObject
            {
                ["Operand1"] = num1,
                ["Operand2"] = num2,
                ["Operation"] = operation,
                ["Symbol"] = opSmybol,
                ["Result"] = result,

            });

            return result;
        }

        public int GetRecordIndex(int index)
        {
            if (index < 0)
            {
                return 0;
            }
            else
            {
                return index -1;
            }

        }

        public void Finish()
        {

            string json = jsonArray.ToJsonString(new JsonSerializerOptions
            {
                WriteIndented = true
            });

            File.WriteAllText("calculatorlog.json", json);

        }

        public JsonArray GetCalculations()
        {
            return jsonArray;
        }
    }
}