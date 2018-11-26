using System.Collections.Generic;
using System.Text; 

namespace BAMUtil
{
    public class ParseCommandLine
    {
        public static Dictionary<string, string> Parse(string[] args)
        {
            var outputArguments = new Dictionary<string, string>();

            string currentCommand = "";
            foreach (var arg in args)
            {
                if (string.IsNullOrEmpty(currentCommand))
                {
                    currentCommand = arg;
                    if (!outputArguments.ContainsKey(currentCommand))
                    {
                        outputArguments.Add(currentCommand, null);
                    }
                }
                else
                {
                    if (arg.StartsWith("-"))
                    {
                        currentCommand = arg.TrimStart('-');
                        if (!outputArguments.ContainsKey(currentCommand))
                        {
                            outputArguments.Add(currentCommand, null);
                        }
                    }
                    else
                    {
                        if (outputArguments.ContainsKey(currentCommand))
                        {
                            outputArguments[currentCommand] = arg;
                            currentCommand = null;
                        }
                    }
                }
            }

            return outputArguments;
        }

        public static string ListParameters(string[] args)
        {
            var sb = new StringBuilder();

            foreach (var kv in ParseCommandLine.Parse(args))
            {
                sb.AppendLine($"{kv.Key}: {kv.Value}");
            }

            return sb.ToString();
        }
    }
}
