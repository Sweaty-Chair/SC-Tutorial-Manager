using UnityEngine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

public static class DatabaseUtils
{

	/// <summary>
	/// Splites the csv to lines array. Remove comment lines
	/// !!!!NOTE: Last line is an empty line, consider that when reading rows. Don't modify it since it is high risk.
	/// </summary>
	public static string[] SplitCsvToRows(TextAsset csv, bool decrypt = true)
	{
		string textDecrypted = string.Empty;
		if (decrypt)
			textDecrypted = SimpleEncryptor.Decrypt(csv.bytes);
		else
			textDecrypted = csv.text;

		// Last line is an empty line
		string[] rows = textDecrypted.Split('\n');

		// Remove empty line
		rows = Array.FindAll(rows, (string s) => {
			return s != string.Empty;
		}).ToArray();

		return rows;
	}

	private static string[] SplitCsvRowSimple(string row)
	{
		return row.Split(',');
	}

	private static string[] SplitCsvRowRegex(string row)
	{
		string pattern = @"
     # Match one value in valid CSV string.
     (?!\s*$)                                      # Don't match empty last value.
     \s*                                           # Strip whitespace before value.
     (?:                                           # Group for value alternatives.
       '(?<val>[^'\\]*(?:\\[\S\s][^'\\]*)*)'       # Either $1: Single quoted string,
     | ""(?<val>[^""\\]*(?:\\[\S\s][^""\\]*)*)""   # or $2: Double quoted string,
     | (?<val>[^,'""\s\\]*(?:\s+[^,'""\s\\]+)*)    # or $3: Non-comma, non-quote stuff.
     )                                             # End group of value alternatives.
     \s*                                           # Strip whitespace after value.
     (?:,|$)                                       # Field ends on comma or EOS.
     ";
		string[] values = (from Match m in Regex.Matches(row, pattern, 
			                       RegexOptions.ExplicitCapture | RegexOptions.IgnorePatternWhitespace | RegexOptions.Multiline)
		                   select m.Groups[1].Value).ToArray();
		return values;        
	}

	public static string[] SplitCsvRow(string row)
	{
		if (!row.Contains('"'))
			return SplitCsvRowSimple(row);
		
		string[] final = CSVReader.SplitCsvLine(row);

		// Remove leading space ( ) and hack to replace duplicated double quote ("") to one
		for (int i = 0; i < final.Length; i++)
			final[i] = final[i].Trim(' ').Replace("\"\"", "\"");
		
		return final;
	}
	
}