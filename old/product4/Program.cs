using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System;
class Program
{
    static void Main()
    {
        string str = "JNPGCZBUXHJAVWXGWIZAXTIQYMRRSSYDNUWCJYVZVZZCYZYKWUMOJNZYUJIKCWXUVDDNOYJDXYIXADXJYZNZTSNQDXGUBYSZPRCRPQYIPTXCSIHNZXWFWSQKVYOHWIZJYWZDQSLPIFXRYWYLXWWYDCBWIKJQGWSUXPHCORZXSXLWWOIZPIMQXCWVCMAYWKKPRNWAYYATXCHQCZKTIWIRLOZVQWKXZGYRZUQJXDJQQYMYLNBZXWWMJXPZXKYPGWRETBPPDHUMQMKNUYHFGQKHMYKJKWYTIBZSTOZFHLQVYXLGCNIEXQFAGBWAFMXSWXTCWZKXSAXUZFLUYPWIGKWYUDTOOYYWZYQZXDVJSYSTGJWXNZGZOZSZCXCHZERWCIWYTIPQRWXZWCYYQYUWTNGZXZUBYKYVZWPEKOYZNWKYGPOYXLTWYYTAFYXPXXQWCWSZLMXRGKVCCWLANWWCBZYWLIRYGJRHMKWVBWXWGRLETQNZHYAQUTZK";
        //string str = "WABCDEWYZXFGHXWBXBYZMK";
		char[] removeChars = new char[] {'W'};
		//削除
		foreach(char target in removeChars)
		{
			str = str.Replace(target.ToString(), "");
		}
		int search_position = 0;
		// Console.WriteLine(str);
		int str_len = str.Length;
		// Console.WriteLine(str_len);
		while(search_position < str_len - 1)
		{
			int X_positon = str.IndexOf("X", search_position);
			if(X_positon == -1)
			{
				break;
			}
			search_position = X_positon + 1;
			//Console.WriteLine(X_positon);
			int target_positon = X_positon + 3;
			// Console.WriteLine(target_positon);
			StringBuilder sb = new StringBuilder(str);
			if(sb[target_positon] != 'X')
			{
				sb[target_positon] = 'E';
			}
			str = sb.ToString();
		}
		// Console.WriteLine(str);
		string str2 = str.Replace("YZ", "E");
		// Console.WriteLine(str2);
		Console.WriteLine(str2[272]);
		Console.WriteLine(str2[273]);
		Console.WriteLine(str2[274]);
		Console.WriteLine(str2[275]);
		Console.WriteLine(str2[276]);
    }
}
